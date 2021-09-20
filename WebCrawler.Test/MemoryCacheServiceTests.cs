using System;
using Xunit;

namespace WebCrawler.Tests
{
    public class MemoryCacheServiceTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetCurrentCache_ReturnsExistingCache(bool addCrawledSite)
        {
            IMemoryCacheService memoryCacheService = new MemoryCacheService();
            Assert.NotNull(memoryCacheService.GetCurrentCache());

            if (addCrawledSite)
            {
                memoryCacheService.Set("Key1", new CrawledSite() { Url = "http://wiprodigital.com" });
                Assert.NotNull(memoryCacheService.Get("Key1"));
            }

        }
    }
}
