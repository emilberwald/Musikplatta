using System;
using Serilog;

namespace Musikplatta
{
    public class SharpWintabPen : ITouch
    {
        public SharpWintabPen(ILogger log)
        {
            this.log = log;
        }

        private bool isDisposed = false;
        private ILogger log;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                this.isDisposed = true;
            }
        }

        ~SharpWintabPen()
        {
            this.Dispose(false);
        }
    }
}