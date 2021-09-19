using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler
{

    public class MemoryCacheService : IMemoryCacheService
    {
        //MemCache is ThreadSafe
        private MemoryCache cache;
        private const string keyFormat = "Key_{0}";

        public MemoryCacheService() {

            cache = new MemoryCache(new MemoryCacheOptions());
        
        }
        public MemoryCache GetCurrentCache()
        {
            return cache;
        }

        public CrawledSite Get(string key)
        {
            return (CrawledSite)cache.Get(string.Format(keyFormat, key));
        }

        public void Set(string key, CrawledSite site)
        {
            cache.Set(string.Format(keyFormat, key), site, 
                new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
        }
    }
}
