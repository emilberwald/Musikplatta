using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Musikplatta
{
    internal class Buffer<T> : IDisposable
    {
        private IntPtr buffer = IntPtr.Zero;

        private bool isDisposed = false;

        public Buffer()
        {
            this.buffer = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
        }

        ~Buffer()
        {
            this.Dispose(false);
        }

        public int Size => Marshal.SizeOf<T>();

        public static explicit operator T(Buffer<T> buffer)
        {
            return Marshal.PtrToStructure<T>(buffer);
        }

        public static implicit operator IntPtr(Buffer<T> buffer) => buffer.buffer;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject((T)this);
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