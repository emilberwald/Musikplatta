using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SharpWintab.Wintab;

namespace Musikplatta
{
    public interface IWintabPen : IDisposable
    {
        public Packet GetDataPacket(Message m);
        public Packetext GetDataPacketExt(Message m);
    }
}
