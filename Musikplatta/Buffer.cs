using System;
using System.Runtime.InteropServices;

namespace Musikplatta
{
    internal class kernel32
    {
        [DllImport("kernel32.dll")]
        internal static extern void RtlZeroMemory(IntPtr dst, UIntPtr length);
    }
    internal class Buffer<T> : IDisposable
    {
        private IntPtr buffer = IntPtr.Zero;
        private int extra;
        private bool isDisposed = false;
        private uint size;

        public Buffer() : this((uint)Marshal.SizeOf<T>()){}
        public Buffer(int size) : this((uint)size) { }

        public Buffer(uint size)
        {
            this.size = size;
            this.buffer = Marshal.AllocHGlobal((int)this.size);
            kernel32.RtlZeroMemory(this.buffer, new UIntPtr(this.size));
        }

        ~Buffer()
        {
            this.Dispose(false);
        }

        public uint Size => this.size;

        public static explicit operator T(Buffer<T> buffer)
        {
            return Marshal.PtrToStructure<T>(buffer);
        }

        public override string ToString() 
        {
            unsafe
            {
                ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(((IntPtr)(this)).ToPointer(), (int)this.Size);
                return BitConverter.ToString(span.ToArray());
            }
        }

        public static implicit operator IntPtr(Buffer<T> buffer) => buffer.buffer;

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