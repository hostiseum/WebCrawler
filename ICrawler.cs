using System.Threading.Tasks;

namespace WebCrawler
{
    public interface ICrawler
    {
        public  Task RunCrawlAsync(string rootUrl = "http://wiprodigital.com");
    }
}