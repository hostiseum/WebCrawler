using System.Threading.Tasks;

namespace WebCrawler
{
    public interface ICrawler
    {
        Task RunCrawlAsync(string rootUrl, string outputFilePath);
    }
}