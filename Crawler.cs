using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public async Task RunCrawlAsync(string rootUrl = "http://wiprodigital.com")
        {
            
            if (string.IsNullOrEmpty(rootUrl) 
                || _memoryCacheService.Get(rootUrl) != null 
                || new Uri(_domainUrl).Host != new Uri(rootUrl).Host)
                return;

            Log.Logger.Information($"Crawling url : {rootUrl}");

            // HttpClient.
            var urls = await GetUrls(rootUrl);

            if (urls == null)
            {
                return;
            }

            var list = _urlUtilities.CleanUp(_domainUrl, rootUrl, urls);

            Parallel.ForEach(list, l =>
            {

                var t = RunCrawlAsync(l).GetAwaiter();
                t.GetResult();
            });

        }

        public async Task<IEnumerable<string>> GetUrls(string url)
        {

            _memoryCacheService.Set(url, new CrawledSite { Url = url });
            using HttpClient httpClient = new HttpClient();
            
            if (!string.IsNullOrEmpty(UserAgent))
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            }

            HtmlDocument htmlDocument = new HtmlDocument();
           
           return await httpClient.GetStringAsync(url)
            .ContinueWith((r) => { 
                if (r.Exception != null)
                {
                    _memoryCacheService.Set(url, 
                                            new CrawledSite { Url = url, 
                                            Crawled = false, 
                                            ErrorIfNotCrawled = r.Exception.Message });
                    return null;
                }
                
                if (r.IsCompletedSuccessfully)
                {
                    _memoryCacheService.Set(url, new CrawledSite { Url = url, Crawled = true });
                    
                    htmlDocument.LoadHtml(r.Result);
                    var anchorTags = htmlDocument.DocumentNode.Descendants("a");
                    return anchorTags.Select(a => a.Attributes.Where(x => x.Name == "href").FirstOrDefault()?.Value);
                }
                return null;
            });
        }
    }
}
