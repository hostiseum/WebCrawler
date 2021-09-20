using System.Collections.Generic;

namespace WebCrawler
{
    public interface IUrlUtilities
    {
        IEnumerable<string> UpdateUrls(string domainUrl, string rootUrl, IEnumerable<string> list);
        IEnumerable<string> GetHtmlTagValues(string htmlContent, string rootTag, string attribute);
    }
}