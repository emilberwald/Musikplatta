using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Musikplatta
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var collection = new ServiceCollection();
            ConfigureServices(collection);

            using var serviceProvider = collection.BuildServiceProvider();

            await serviceProvider.GetRequiredService<App>().Run();

        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ILogger>(new LoggerConfiguration().WriteTo.Console().CreateLogger())
                .AddSingleton<ITouch, Pen>()
                .AddSingleton<App>();
        }
    }
}