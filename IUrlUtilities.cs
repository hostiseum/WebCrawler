using System.Collections.Generic;

namespace WebCrawler
{
    public interface IUrlUtilities
    {
        IEnumerable<string> CleanUp(string domainUrl, string rootUrl, IEnumerable<string> list);
    }
}