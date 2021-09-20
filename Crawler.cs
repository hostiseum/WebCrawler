using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace WebCrawler
{
    public class Crawler : ICrawler
    {
        private string _domainUrl;
        private IUrlUtilities _urlUtilities;
        private IMemoryCacheService _memoryCacheService;
        private ILogger<Crawler> _logger;
        public Crawler(string domainUrl, ILogger<Crawler> logger, IUrlUtilities urlUtilities, IMemoryCacheService memoryCacheService)
        {
            _domainUrl = domainUrl;
            _logger = logger;
            _urlUtilities = urlUtilities;
            _memoryCacheService = memoryCacheService;
        }

        private string UserAgent
        {
            get { return "myBot1.0"; }
        }

        public async Task RunCrawlAsync(string rootUrl, string outputFilePath)
        {
            Log.Logger.Information($"Starting to crawl {rootUrl}");

            await RunCrawlAsync(rootUrl);

            Log.Logger.Information($"Finished crawling {rootUrl}");
            Log.Logger.Information($"Writing data to the file: {outputFilePath}");

            await WriteCacheToDisk(outputFilePath).ContinueWith((r) =>
            {
                if (r.Exception != null)
                {
                    Log.Logger.Error($"Error writing data to the file: {outputFilePath}");
                }
                else
                {
                    Log.Logger.Information($"Finished writing data to the file: {outputFilePath}");
                }
            });
            return;
        }
        private async Task RunCrawlAsync(string rootUrl)
        {

            if (string.IsNullOrEmpty(rootUrl)
                || _memoryCacheService.Contains(rootUrl)
                || new Uri(_domainUrl).Host != new Uri(rootUrl).Host)
                return;

            var crawledSite = await CrawlUrl(rootUrl);

            if (crawledSite == null)
            {
                return;
            }

            var updatedUrls = _urlUtilities.CleanUp(_domainUrl, rootUrl, crawledSite.htmlTags);

            crawledSite.htmlTags = updatedUrls;

            _memoryCacheService.Set(rootUrl, crawledSite);

            Parallel.ForEach(updatedUrls, l =>
            {
                var t = RunCrawlAsync(l).GetAwaiter();
                t.GetResult();
            });

        }

        private async Task<CrawledSite> CrawlUrl(string url)
        {
            using HttpClient httpClient = new HttpClient();

            if (!string.IsNullOrEmpty(UserAgent))
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            }

            return await httpClient.GetStringAsync(url)
             .ContinueWith((r) =>
             {
                 if (r.Exception != null)
                 {
                     _memoryCacheService.Set(url,
                                             new CrawledSite
                                             {
                                                 Url = url,
                                                 Crawled = false,
                                                 ErrorIfNotCrawled = r.Exception.Message
                                             });
                     return null;
                 }

                 if (r.IsCompletedSuccessfully)
                 {
                     var htmlContent = r.Result;
                     var hreftags = _urlUtilities.GetValues(htmlContent, "a", "href");
                     var imagetags = _urlUtilities.GetValues(htmlContent, "img", "src");
                     var crawledSite = new CrawledSite { Url = url, Crawled = true, htmlTags = hreftags, imageTags = imagetags };

                     return crawledSite;

                 }
                 return null;
             });
        }

        private async Task WriteCacheToDisk(string outputFilePath)
        {
            var memCache = _memoryCacheService.GetCurrentCache();
            using FileStream createStream = File.Create(outputFilePath);
            await JsonSerializer.SerializeAsync(createStream, memCache);
        }
    }
}
