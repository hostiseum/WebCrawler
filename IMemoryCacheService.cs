using Microsoft.Extensions.Caching.Memory;
using System;

namespace WebCrawler
{
    public interface IMemoryCacheService
    {
        public CrawledSite Get(string key);
        public void Set(string key, CrawledSite site);
        public MemoryCache GetCurrentCache();
    }
}