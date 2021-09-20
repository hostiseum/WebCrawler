using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;

namespace WebCrawler
{
    public interface IMemoryCacheService
    {
        public CrawledSite Get(string key);
        public bool Contains(string key);
        public void Set(string key, CrawledSite site);
        public ConcurrentDictionary<string, CrawledSite> GetCurrentCache();
    }
}