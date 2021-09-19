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

            args = new string[] { "http://wiprodigital.com" };


            var serviceProvider = new ServiceCollection()
            .AddLogging()
          //.AddSingleton<IConfigurationService, ConfigurationService>()
          .AddSingleton<IMemoryCacheService, MemoryCacheService>()

          .AddTransient<string>(_ => args[0])
          .AddTransient<ICrawler, Crawler>()
          .AddTransient<IUrlUtilities, UrlUtilities>()
          //.AddTransient<IAdvertisementService, AdvertisementService>()
          .BuildServiceProvider();



            ICrawler crawler = serviceProvider.GetRequiredService<ICrawler>();
            await crawler.RunCrawlAsync();



            Console.WriteLine("Press any key to continue...");
        }
    }
}
