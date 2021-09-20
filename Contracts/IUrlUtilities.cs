using System.Collections.Generic;

namespace WebCrawler
{
    public interface IUrlUtilities
    {
        IEnumerable<string> CleanUp(string domainUrl, string rootUrl, IEnumerable<string> list);
        IEnumerable<string> GetValues(string htmlContent, string rootTag, string attribute);
    }
}