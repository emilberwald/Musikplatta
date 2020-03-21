using System;
using System.Runtime.InteropServices;

namespace Musikplatta
{
    internal class PinPtr : IDisposable
    {
        private bool disposedValue = false;

        public PinPtr(object t)
        {
            this.Handle = GCHandle.Alloc(t, GCHandleType.Pinned);
        }

        // To detect redundant calls

        ~PinPtr()
        {
            this.Dispose(false);
        }

        private GCHandle Handle { get; }

        public static implicit operator IntPtr(PinPtr ptr)
        {
            return ptr.Handle.AddrOfPinnedObject();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                this.Handle.Free();

                this.disposedValue = true;
            }
        }
    }
}