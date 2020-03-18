using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Musikplatta
{
    internal class Program
    {
        private static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<Form>()
                .AddSingleton<ILogger>(new LoggerConfiguration().WriteTo.Console().WriteTo.File("log.txt", rollingInterval: RollingInterval.Hour).CreateLogger())
                .AddSingleton<IWintabPen>(x => new SharpWintabPen(x.GetRequiredService<ILogger>(), x.GetRequiredService<Form>()));
        }

        [STAThread]
        private static void Main(string[] args)
        {
            var collection = new ServiceCollection();
            ConfigureServices(collection);

            using var serviceProvider = collection.BuildServiceProvider();
            var log = serviceProvider.GetRequiredService<ILogger>();
            log.Information(string.Concat(Enumerable.Repeat("=", 80)));
            log.Information("Starting Musikplatta.");
            log.Information(string.Concat(Enumerable.Repeat("=", 80)));

            using var pen = serviceProvider.GetRequiredService<IWintabPen>();
            Application.Run(serviceProvider.GetRequiredService<Form>());
        }
    }
}