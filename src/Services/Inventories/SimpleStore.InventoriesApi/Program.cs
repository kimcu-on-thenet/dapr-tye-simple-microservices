using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SimpleStore.Infrastructure.Common.Extensions;
using SimpleStore.Inventories.Infrastructure.EfCore.Options;
using System.Diagnostics;

namespace SimpleStore.InventoriesApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args).CustomConfigure(typeof(Startup), configuration =>
            {
                var serviceOptions = configuration.GetOptions<ServiceOptions>("Services");
                return serviceOptions.InventoriesApi;
            });
    }
}
