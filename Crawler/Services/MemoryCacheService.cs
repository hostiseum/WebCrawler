using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{

    public class MemoryCacheService : IMemoryCacheService
    {
        //MemCache is ThreadSafe
        private ConcurrentDictionary<string, CrawledSite> cache;
        //private const string keyFormat = "Key_{0}";

        public MemoryCacheService() {

            cache = new ConcurrentDictionary<string, CrawledSite>();
        
        }
        public ConcurrentDictionary<string, CrawledSite> GetCurrentCache()
        {
            return cache;
        }

        public CrawledSite Get(string key)
        {
            return cache.ContainsKey(key) ? cache[key] : null;
        }

        public bool Contains(string key)
        {
            return cache.ContainsKey(key);
        }

        public void Set(string key, CrawledSite site)
        {
            cache[key] = site;
        }


    }
}
