using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebCrawler.Test
{
    public class CrawlerTest
    {
        [Fact]
        public void CrawlerCreates_Success()
        {
            Mock<IMemoryCacheService> mockMemoryCacheService = new Mock<IMemoryCacheService>();
            Mock<IUrlUtilities> mockUrlUtilities = new Mock<IUrlUtilities>();
            Mock<ILogger<Crawler>> mockLogger = new Mock<ILogger<Crawler>>();

            Crawler crawler = new Crawler(It.IsAny<string>(), 
                                            mockLogger.Object, 
                                            mockUrlUtilities.Object, 
                                            mockMemoryCacheService.Object);
        
        }

        [Fact]

        public void RunCrawlAsync_InvalidUrl()
        {
            Mock<IMemoryCacheService> mockMemoryCacheService = new Mock<IMemoryCacheService>();
            Mock<IUrlUtilities> mockUrlUtilities = new Mock<IUrlUtilities>();
            Mock<ILogger<Crawler>> mockLogger = new Mock<ILogger<Crawler>>();

            Crawler crawler = new Crawler(It.IsAny<string>(),
                                            mockLogger.Object,
                                            mockUrlUtilities.Object,
                                            mockMemoryCacheService.Object);
            Assert.ThrowsAsync<ApplicationException>(async ()=> await crawler.RunCrawlAsync(string.Empty, It.IsAny<string>()));
        }
     }
}
