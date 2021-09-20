using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Threading.Tasks;

namespace WebCrawler
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
              .WriteTo.Console()
              .WriteTo.File(new CompactJsonFormatter(), "./logs/myapp.json")
              .CreateLogger();

            Console.WriteLine("Enter URL to crawl:");
            var url = Console.ReadLine();

            if (string.IsNullOrEmpty(url))
            {
                Console.WriteLine("Invalid Url");
                return;
            }


            var serviceProvider = new ServiceCollection()
            .AddLogging()
          //.AddSingleton<IConfigurationService, ConfigurationService>()
          .AddSingleton<IMemoryCacheService, MemoryCacheService>()

          .AddTransient<string>(_ => url)
          .AddTransient<ICrawler, Crawler>()
          .AddTransient<IUrlUtilities, UrlUtilities>()
          //.AddTransient<IAdvertisementService, AdvertisementService>()
          .BuildServiceProvider();



            ICrawler crawler = serviceProvider.GetRequiredService<ICrawler>();
            await crawler.RunCrawlAsync(url, "output.json");

            Console.WriteLine("Press any key to continue...");
        }
    }
}
