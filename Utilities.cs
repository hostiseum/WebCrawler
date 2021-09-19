using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class UrlUtilities : IUrlUtilities
    {
        public IEnumerable<string> CleanUp(string domainUrl, string rootUrl, IEnumerable<string> hrefs)
        {
            Uri u = new Uri(rootUrl);
            
            var hrefList = hrefs.Where(x => !string.IsNullOrEmpty(x));

            //Ignore javascript hrefs
            hrefList = hrefList.Where(l => !l.StartsWith("javascript"));
            
            //Ignore bookmarks
            hrefList = hrefList.Where(l => !l.StartsWith("#"));

            var hrefsList = hrefList.ToList();
            int totalHrefs = hrefsList.Count();
            //if the links have relevant urls replace them with the domain
            for (int i=0; i < totalHrefs; i++)
            {
                if (hrefsList[i][0] == '/' && hrefsList[i][1] != '/')
                {
                    hrefsList[i] = new Uri(u,hrefsList[i]).ToString(); 
                }
            };

            //Ignore the urls of external urls but leave the urls that are specified using current full domain urls
            hrefList = hrefList.Where(h => h.StartsWith("http")
                                 && new Uri(h).Host == new Uri(domainUrl).Host);

            return hrefsList.Distinct();
        }
    }
}
