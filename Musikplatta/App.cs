using System;
using System.Threading.Tasks;
using Serilog;

namespace Musikplatta
{
    internal class App
    {
        private ILogger log;
        private ITouch touch;

        public App(ITouch touch, ILogger log)
        {
            this.log = log;
            this.touch = touch;
        }

        public async Task Run()
        {
            while (true)
            {
                this.log.Information($"Run loop. {DateTime.UtcNow}");
                await Task.Delay(1000);
            }
        }
    }
}