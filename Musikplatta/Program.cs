using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Musikplatta
{
    internal class Program
    {
        private static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ILogger>(new LoggerConfiguration().WriteTo.Console().WriteTo.File("log.txt", rollingInterval: RollingInterval.Hour).CreateLogger())
                .AddSingleton<ITouch, Pen>()
                .AddSingleton<App>();
        }

        private static void Main(string[] args)
        {
            var collection = new ServiceCollection();
            ConfigureServices(collection);

            using var serviceProvider = collection.BuildServiceProvider();
            var log = serviceProvider.GetRequiredService<ILogger>();
            log.Information(string.Concat(Enumerable.Repeat("=", 80)));
            log.Information("Starting Musikplatta.");
            log.Information(string.Concat(Enumerable.Repeat("=", 80)));
            using var app = serviceProvider.GetRequiredService<App>();
            app.Run();
        }
    }
}