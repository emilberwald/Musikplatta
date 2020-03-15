using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Serilog;

namespace Musikplatta
{
    internal class App : IDisposable
    {
        private ILogger log;
        private System.Timers.Timer timer;
        private ITouch touch;

        public App(ITouch touch, ILogger log)
        {
            this.log = log;
            this.touch = touch;
            this.timer = new System.Timers.Timer();
            this.timer.Interval = 1000;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timerTickEventHandler);
        }

        public void Run()
        {
            this.log.Information(string.Concat(Enumerable.Repeat("=", 80)));
            this.log.Information("Press ESC to end program.");
            this.log.Information(string.Concat(Enumerable.Repeat("=", 80)));
            this.timer.Start();
            while (!(Console.ReadKey().Key == ConsoleKey.Escape))
            { }
        }

        private void timerTickEventHandler(object sender, ElapsedEventArgs e)
        {
            this.log.Information($"Run loop. {DateTime.UtcNow}");
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.touch.Dispose();
                }
                disposedValue = true;
            }
        }

        #endregion IDisposable Support
    }
}