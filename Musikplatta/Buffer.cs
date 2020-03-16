using System;
using System.Runtime.InteropServices;

namespace Musikplatta
{
    internal class Buffer : IDisposable
    {
        private IntPtr buffer = IntPtr.Zero;

        private bool isDisposed = false;

        public Buffer(object value) : this(Marshal.SizeOf(value))
        {
        }

        public Buffer(int nofBytes)
        {
            this.buffer = Marshal.AllocHGlobal(nofBytes);
        }

        ~Buffer()
        {
            this.Dispose(false);
        }

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

                if (this.buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(this.buffer);
                    this.buffer = IntPtr.Zero;
                }

                this.isDisposed = true;
            }
        }
    }
}