using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Newtonsoft.Json;
using Serilog;
using SharpGen.Runtime.Win32;
using Swig;
using static Swig.SwigWintab;

namespace Musikplatta
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class WintabPen : NativeWindow, IWintabPen
    {
        private LOGCONTEXTW context;
        private SWIGTYPE_p_HCTX contextHandle;
        private int[] extensionTags;
        private Form form;
        private bool isDisposed = false;

        private ILogger log;
        private System.Timers.Timer timer;

        public WintabPen(ILogger log, Form form)
        {
            this.log = log;

            this.timer = new System.Timers.Timer();
            this.timer.Interval = 1000;
            this.timer.Elapsed += new ElapsedEventHandler(this.timerTickEventHandler);

            this.context = this.GetDefaultDigitizingContext();
            this.extensionTags = new int[] { WTX_TOUCHSTRIP, WTX_TOUCHRING, WTX_EXPKEYS2 };
            foreach (var extensionTag in this.extensionTags)
            {
                try
                {
                    this.context.lcPktData |= GetExtensionMask(extensionTag);
                }
                catch (SharpWintabException ex)
                {
                    this.log.Error(ex, "{@extensionTag}", extensionTag);
                }
            }

            this.form = form;
            this.form.HandleCreated += new EventHandler(this.OnHandleCreated);
            this.form.HandleDestroyed += new EventHandler(this.OnHandleDestroyed);
        }

        ~WintabPen()
        {
            this.Dispose(false);
        }

        public bool OverridesAdded { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public SharpWintab.Wintab.Packet GetDataPacket(Message m)
        {
            using var buffer = new Buffer<SharpWintab.Wintab.Packet>();
            if (!Convert.ToBoolean(WTPacket(new SWIGTYPE_p_HCTX(m.LParam, default), (uint)m.WParam.ToInt64(), buffer)))
            {
                throw new SharpWintabException("GetDataPacket: The return value is non-zero if the specified packet was found and returned. It is zero if the specified packet was not found in the queue.");
            }
            return (SharpWintab.Wintab.Packet)buffer;
        }

        public SharpWintab.Wintab.Packetext GetDataPacketExt(Message m)
        {
            using var buffer = new Buffer<SharpWintab.Wintab.Packetext>();
            if (!Convert.ToBoolean(WTPacket(new SWIGTYPE_p_HCTX(m.LParam, default), (uint)m.WParam.ToInt64(), buffer)))
            {
                throw new SharpWintabException("GetDataPacketExt: The return value is non-zero if the specified packet was found and returned. It is zero if the specified packet was not found in the queue.");
            }
            return (SharpWintab.Wintab.Packetext)buffer;
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

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WindowsEventMessages.WM_ACTIVATE:
                {
                    switch (m.WParam.ToInt32())
                    {
                        case (int)WParam.WA_INACTIVE:
                        {
                            //NOTE: It is recommended that applications remove their overrides when the application's main window is no longer active (use WM_ACTIVATE). Extension overrides take effect across the entire system; if an application leaves its overrides in place, that control will not function correctly in other applications.
                            this.RemoveAllOverrides();
                            //When applications receive the WM_ACTIVATE message, they should push their contexts to the bottom of the overlap order if their application is being deactivated, and should bring their context to the top if they are being activated.
                            WTOverlap(this.contextHandle, Convert.ToInt32(false));
                            break;
                        }
                        case (int)WParam.WA_ACTIVE:
                        case (int)WParam.WA_CLICKACTIVE:
                        {
                            this.AddAllOverrides();
                            //When applications receive the WM_ACTIVATE message, they should push their contexts to the bottom of the overlap order if their application is being deactivated, and should bring their context to the top if they are being activated.
                            WTOverlap(this.contextHandle, Convert.ToInt32(true));
                            break;
                        }
                        default:
                            break;
                    }
                    break;
                }
                case (int)WindowsEventMessages.WM_SYSCOMMAND:
                {
                    switch (m.WParam.ToInt32())
                    {
                        case (int)WParam.SC_MINIMIZE:
                        {
                            //Applications should also disable their contexts when they are minimized.
                            WTEnable(this.contextHandle, Convert.ToInt32(false)); //The function returns a non-zero value if the enable or disable request was satis­fied, zero otherwise.
                            break;
                        }
                        case (int)WParam.SC_RESTORE:
                        case (int)WParam.SC_MAXIMIZE:
                        {
                            WTEnable(this.contextHandle, Convert.ToInt32(true)); //The function returns a non-zero value if the enable or disable request was satis­fied, zero otherwise.
                            break;
                        }
                        default:
                            break;
                    }
                    break;
                }
                case (int)WindowsEventMessages.WM_LBUTTONDOWN:
                    //stylus button
                    break;

                case (int)WindowsEventMessages.WM_LBUTTONUP:
                    //stylus button
                    break;

                default:
                {
                    if (m.Msg == WT_PACKET)
                    {
                        /*
                            The interface defines a standard orientation for reporting device native coordinates.
                            When the user is viewing the device in its normal position, the coordinate origin will be at the lower left of the device.
                            The coordinate system will be right-handed, that is,
                                the positive x axis points from left to right, and
                                the posi­tive y axis points either upward or away from the user.
                                The z axis, if supported, points either to­ward the user or upward.
                                For devices that lay flat on a table top, the x-y plane will be horizontal and the z axis will point upward.
                                For devices that are oriented vertically (for example, a touch screen on a conventional dis­play), the x-y plane will be vertical, and the z axis will point toward the user.
                        */
                        var pkt = this.GetDataPacket(m);
                        this.log.Information("{Packet}", JsonConvert.SerializeObject(pkt));
                        break;
                    }
                    else if (m.Msg == WT_PACKETEXT)
                    {
                        var pktExt = this.GetDataPacketExt(m);
                        this.log.Information("{Packetext}", JsonConvert.SerializeObject(pktExt));
                    }
                    else if (m.Msg == WT_CTXOPEN)
                    {
                        /*
                            +---+-------------------------------------------------+---------------+
                            | D | The WT_CTXOPEN message is sent to the owning    |               |
                            | e | window and to any manager windows when a        |               |
                            | s.| context is opened.                              |               |
                            +===+=================================================+===============+
                            |   | Parameter                                       | Description   |
                            +---+-------------------------------------------------+---------------+
                            |   | wParam                                          | Contains the  |
                            |   |                                                 | context       |
                            |   |                                                 | handle of the |
                            |   |                                                 | opened        |
                            |   |                                                 | context.      |
                            +---+-------------------------------------------------+---------------+
                            |   | lParam                                          | Contains the  |
                            |   |                                                 | current       |
                            |   |                                                 | context       |
                            |   |                                                 | status flags. |
                            +---+-------------------------------------------------+---------------+
                            | C | Tablet manager applications should save the     |               |
                            | o | handle of the new context. Managers may want    |               |
                            | m | to query the user to configure the context      |               |
                            | m.| further at this time.                           |               |
                            +---+-------------------------------------------------+---------------+
                        */
                    }
                    else if (m.Msg == WT_CTXCLOSE)
                    {
                        /*
                            +-------------+-------------------------------------------------------------------------------------------------------------------------+
                            | Description | The WT_CTXCLOSE message is sent to the owning winÂ­dow and to any manager windows when a context is about to be closed.  |
                            +-------------+-----------+-------------------------------------------------------------------------------------------------------------+
                            |             | Parameter | Description                                                                                                 |
                            +-------------+-----------+-------------------------------------------------------------------------------------------------------------+
                            |             | wParam    | Contains the context handle of the context to be closed.                                                    |
                            +-------------+-----------+-------------------------------------------------------------------------------------------------------------+
                            |             | lParam    | Contains the current context status flags.                                                                  |
                            +-------------+-----------+-------------------------------------------------------------------------------------------------------------+
                            | Comments    | Tablet manager applications should note that the context is being closed and take appropriate action.                   |
                            +-------------+-------------------------------------------------------------------------------------------------------------------------+
                        */
                    }
                    else if (m.Msg == WT_CTXUPDATE)
                    {
                        /*
                            +-------------+-----------------------------------------------------------------------------------------------------------------------------+
                            | Description | The WT_CTXUPDATE message is sent to the owning window and to any manager windows when a context is changed.                 |
                            +-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
                            |             | Parameter | Description                                                                                                     |
                            +-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
                            |             | wParam    | Contains the context handle of the changed context.                                                             |
                            +-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
                            |             | lParam    | Contains the current context status flags.                                                                      |
                            +-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
                            | Comments    | Applications may want to call WTGet or WTExtGet on receiving this message to find out what context attributes were changed. |
                            +-------------+-----------------------------------------------------------------------------------------------------------------------------+
                        */
                    }
                    else if (m.Msg == WT_CTXOVERLAP)
                    {
                        /*
                            +-------------+-----------------------------------------------------------------------------------------------------------------------------------+
                            | Description | The WT_CTXOVERLAP message is sent to the owning window and to any manager windows when a context is moved in the overlap order.   |
                            +-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
                            |             | Parameter | Description                                                                                                           |
                            +-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
                            |             | wParam    | Contains the context handle of the re-overlapped context.                                                             |
                            +-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
                            |             | lParam    | Contains the current context status flags.                                                                            |
                            +-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
                            | Comments    | Tablet managers can handle this message to keep track of context overlap reÂ­quests by applications.                               |
                            +-------------+-----------------------------------------------------------------------------------------------------------------------------------+
                            |             | Applications can handle this message to find out when their context is obscured by another context.                               |
                            +-------------+-----------------------------------------------------------------------------------------------------------------------------------+
                        */
                    }
                    else if (m.Msg == WT_PROXIMITY)
                    {
                        /*
                            +-------------+---------------------------------------------------------------------------------------------------------------------------------------+
                            | Description | The WT_PROXIMITY message is posted to the owning window and any manager windows when the cursor enters or leaves context proximity.   |
                            +-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
                            |             | Parameter | Description                                                                                                               |
                            +-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
                            |             | wParam    | Contains the handle of the context that the cursor is entering or leavÂ­ing.                                               |
                            +-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
                            |             | lParam    | The low-order word is non-zero when the cursor is entering the context and zero when it is leaving the context.           |
                            |             |           | The high-order word is non-zero when the cursor is leaving or entering hardÂ­ware proximity.                               |
                            +-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
                            | Comments    | Proximity events are handled separately from regular tablet events.                                                                   |
                            |             | Applications will receive proximity messages even if they haven't requested event messages.                                           |
                            +-------------+---------------------------------------------------------------------------------------------------------------------------------------+
                        */
                    }
                    else if (m.Msg == WT_INFOCHANGE)
                    {
                        /*
                                +---+----------------------------------------+---------------------------+
                                | D | The WT_INFOCHANGE message is sent to   |                           |
                                | e | all mangager and context-owning        |                           |
                                | s | windows when the number of connected   |                           |
                                | c.| tablets has changed                    |                           |
                                +===+========================================+===========================+
                                |   | Parameter                              | Description               |
                                +---+----------------------------------------+---------------------------+
                                |   | wParam                                 | Contains the manager      |
                                |   |                                        | handle of the tablet      |
                                |   |                                        | manager that changed the  |
                                |   |                                        | information, or zero if   |
                                |   |                                        | the change was reported   |
                                |   |                                        | through hardware.         |
                                +---+----------------------------------------+---------------------------+
                                |   | lParam                                 | Contains category and     |
                                |   |                                        | index numbers for the     |
                                |   |                                        | changed information.      |
                                |   |                                        | The low-order word        |
                                |   |                                        | contains the category     |
                                |   |                                        | number; the high-order    |
                                |   |                                        | word contains the index   |
                                |   |                                        | number.                   |
                                +---+----------------------------------------+---------------------------+
                                | C | Applications can respond to capability |                           |
                                | o | changes by handling this message.      |                           |
                                | m | Simple applications may want to prompt |                           |
                                | m | the user to close and restart them.    |                           |
                                | e | The most basic applications that use   |                           |
                                | n | the default context should not need to |                           |
                                | t | take any action.                       |                           |
                                | s |                                        |                           |
                                +---+----------------------------------------+---------------------------+
                                |   | Tablet managers should handle this     |                           |
                                |   | message to react to hardware           |                           |
                                |   | configuration changes or changes by    |                           |
                                |   | other managers (in implementations     |                           |
                                |   | that allow multiple tablet managers).  |                           |
                                +---+----------------------------------------+---------------------------+
                        */
                    }
                    else if (m.Msg == WT_CSRCHANGE)
                    {
                        /*
                            +-------------+-------------------------------------------------------------------------------------------------+
                            | Description | The WT_CSRCHANGE message is posted to the owning window when a new cursor enters the context.   |
                            +-------------+-----------+-------------------------------------------------------------------------------------+
                            |             | Parameter | Description                                                                         |
                            +-------------+-----------+-------------------------------------------------------------------------------------+
                            |             | wParam    | Contains the serial number of the packet that generated the mesÂ­sage.               |
                            +-------------+-----------+-------------------------------------------------------------------------------------+
                            |             | lParam    | Contains the handle of the context that processed the packet.                       |
                            +-------------+-----------+-------------------------------------------------------------------------------------+
                            | Comments    | Only contexts that have the CXO_CSRMESSAGES option selected will generate this message.         |
                            +-------------+-------------------------------------------------------------------------------------------------+
                        */
                    }
                    else
                    {
                    }
                    break;
                }
            }
            base.WndProc(ref m);
        }

        private static byte[] BitConvertFrom<T>(T value_t)
        {
            switch (value_t)
            {
                case bool value:
                    return BitConverter.GetBytes(value);

                case char value:
                    return BitConverter.GetBytes(value);

                case string value:
                    return Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(value));

                case float value:
                    return BitConverter.GetBytes(value);

                case double value:
                    return BitConverter.GetBytes(value);

                case Int16 value:
                    return BitConverter.GetBytes(value);

                case Int32 value:
                    return BitConverter.GetBytes(value);

                case Int64 value:
                    return BitConverter.GetBytes(value);

                case UInt16 value:
                    return BitConverter.GetBytes(value);

                case UInt32 value:
                    return BitConverter.GetBytes(value);

                case UInt64 value:
                    return BitConverter.GetBytes(value);

                default:
                    throw new NotSupportedException();
            }
        }

        private static T BitConvertTo<T>(byte[] value)
        {
            switch (typeof(T))
            {
                case var cls when cls == typeof(Boolean):
                {
                    return (T)(object)BitConverter.ToBoolean(value);
                }
                case var cls when cls == typeof(Char):
                {
                    return (T)(object)BitConverter.ToChar(value);
                }
                case var cls when cls == typeof(String):
                {
                    return (T)(object)BitConverter.ToString(value);
                }
                case var cls when cls == typeof(Single):
                {
                    return (T)(object)BitConverter.ToSingle(value);
                }
                case var cls when cls == typeof(Double):
                {
                    return (T)(object)BitConverter.ToDouble(value);
                }
                case var cls when cls == typeof(Int16):
                {
                    return (T)(object)BitConverter.ToInt16(value);
                }
                case var cls when cls == typeof(Int32):
                {
                    return (T)(object)BitConverter.ToInt32(value);
                }
                case var cls when cls == typeof(Int64):
                {
                    return (T)(object)BitConverter.ToInt64(value);
                }
                case var cls when cls == typeof(UInt16):
                {
                    return (T)(object)BitConverter.ToUInt16(value);
                }
                case var cls when cls == typeof(UInt32):
                {
                    return (T)(object)BitConverter.ToUInt32(value);
                }
                case var cls when cls == typeof(UInt64):
                {
                    return (T)(object)BitConverter.ToUInt64(value);
                }
                default:
                    throw new NotSupportedException();
            }
        }

        private static uint GetExtensionCategoryOffset(int extensionTag)
        {
            using var buffer = new Buffer<Int32>();
            /* scan for wTag's info category. */
            for (
                uint extensionCategoryOffset = 0;
                WTInfoW((uint)WTI_EXTENSIONS + extensionCategoryOffset, (uint)EXT_TAG, buffer) > 0;
                extensionCategoryOffset++)
            {
                if (extensionTag == (Int32)buffer)
                {
                    return extensionCategoryOffset;
                }
            }
            throw new SharpWintabException("GetExtensionCategoryOffset");
        }

        private static uint GetExtensionMask(int extensionTag)
        {
            var categoryOffset = GetExtensionCategoryOffset(extensionTag);
            using var buffer = new Buffer<uint>();
            if (WTInfoW((uint)WTI_EXTENSIONS + categoryOffset, (uint)EXT_MASK, buffer) != buffer.Size)
                throw new SharpWintabException("GetExtensionMask");
            return (uint)buffer;
        }

        private static uint GetNumberOfDevices()
        {
            using var buffer = new Buffer<uint>();
            if (WTInfoW((uint)WTI_INTERFACE, (uint)IFC_NDEVICES, buffer) != buffer.Size)
            {
                throw new SharpWintabException("GetNumberOfDevices");
            }
            return (uint)buffer;
        }

        private void AddAllOverrides()
        {
            for (uint tabletIndex = 0; tabletIndex < GetNumberOfDevices(); ++tabletIndex)
            {
                try
                {
                    this.AddOverrides(tabletIndex);
                }
                catch (SharpWintabException ex)
                {
                    this.log.Warning("Exception: {@ex}", ex);
                }
            }
        }

        private void AddOverrides(uint tabletIndex)
        {
            foreach (var extTagIndex in this.extensionTags)
            {
                var nofControls = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, 0, 0, TABLET_PROPERTY_CONTROLCOUNT);
                for (uint controlIndex = 0; controlIndex < nofControls; controlIndex++)
                {
                    uint nofFuncs = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, controlIndex, 0, TABLET_PROPERTY_FUNCCOUNT);
                    for (uint functionIndex = 0; functionIndex < nofFuncs; functionIndex++)
                    {
                        this.Override(tabletIndex, extTagIndex, functionIndex, controlIndex);
                    }
                }
            }
        }

        private string ControlName(uint tabletIndex, int extTagIndex, uint controlIndex, uint functionIndex) => extTagIndex switch
        {
            _ => Convert.ToString(extTagIndex) + Convert.ToString(tabletIndex) + Convert.ToString(controlIndex) + Convert.ToString(functionIndex)
        };

        private T ControlPropertyGet<T>(uint tabletIndex, int extTagIndex, uint controlIndex, uint functionIndex, int propertyId)
        {
            using var buffer = new Buffer<byte[]>(Marshal.SizeOf<SharpWintab.Wintab.Extproperty.__Native>() + Marshal.SizeOf<T>());
            unsafe
            {
                SharpWintab.Wintab.Extproperty.__Native property = new SharpWintab.Wintab.Extproperty.__Native()
                {
                    Version = 0,
                    TabletIndex = (byte)tabletIndex,
                    ControlIndex = (byte)controlIndex,
                    FunctionIndex = (byte)functionIndex,
                    PropertyID = (ushort)propertyId,
                    Reserved = 0,
                    DataSize = (int)Marshal.SizeOf<T>(),
                    Data = 0,
                };
                using var pinnedProperty = new PinPtr(property);
                System.Buffer.MemoryCopy(((IntPtr)pinnedProperty).ToPointer(), ((IntPtr)buffer).ToPointer(), buffer.Size, Marshal.SizeOf<SharpWintab.Wintab.Extproperty.__Native>());
            }
            var swigWintab = WTExtGet(this.contextHandle, (uint)extTagIndex, buffer);
            var sharpWintab = SharpWintab.Wintab.Functions.WTExtGet(new SharpWintab.Wintab.Hctx()
            {
                Unused = SWIGTYPE_p_HCTX.getCPtr(this.contextHandle).Handle.ToInt32()
            }, (uint)extTagIndex, buffer);
            var wintabDN = WintabDN.CWintabFuncs.WTExtGet((UInt32)SWIGTYPE_p_HCTX.getCPtr(this.contextHandle).Handle.ToInt32(), (uint)extTagIndex, buffer);
            //The function returns a non-zero value if the data is retrieved successfully. Oth­er­wise, it returns zero.
            if (Convert.ToBoolean(WTExtGet(this.contextHandle, (uint)extTagIndex, buffer)))
            {
                this.log.Error("WTExtGet failed. {buffer}. Return default.", buffer);
                return default;
            }
            byte[] result = new byte[Marshal.SizeOf<T>()];
            unsafe
            {
                //TODO: Extproperty.Data or byte after it?
                using var pinnedResult = new PinPtr(result);
                System.Buffer.MemoryCopy(IntPtr.Add(((IntPtr)buffer), (int)(buffer.Size)).ToPointer(), ((IntPtr)pinnedResult).ToPointer(), Marshal.SizeOf<T>(), Marshal.SizeOf<T>());
            }
            return BitConvertTo<T>(result);
        }

        private void ControlPropertySet<T>(uint tabletIndex, int extTagIndex, uint controlIndex, uint functionIndex, int propertyId, T value_t)
        {
            // From Wintab 1.4 specification:
            // "Note that this stucture is of variable size. The total size of the structure is sizeof(EXTPROPERTY) + dataSize. Take care to allocate the correct amount of memory before using this structure."
            byte[] value = BitConvertFrom(value_t);
            using var buffer = new Buffer<object>(Marshal.SizeOf<SharpWintab.Wintab.Extproperty.__Native>() + Marshal.SizeOf(value));
            unsafe
            {
                //TODO: Extproperty.Data or byte after it?
                SharpWintab.Wintab.Extproperty.__Native property = new SharpWintab.Wintab.Extproperty.__Native()
                {
                    Version = 0,
                    TabletIndex = (byte)tabletIndex,
                    ControlIndex = (byte)controlIndex,
                    FunctionIndex = (byte)functionIndex,
                    PropertyID = (ushort)propertyId,
                    DataSize = (int)Marshal.SizeOf<T>(),
                    Reserved = 0,
                };
                using var pinnedProperty = new PinPtr(property);
                System.Buffer.MemoryCopy(((IntPtr)pinnedProperty).ToPointer(), ((IntPtr)buffer).ToPointer(), buffer.Size, Marshal.SizeOf<SharpWintab.Wintab.Extproperty.__Native>());
            }
            unsafe
            {
                //TODO: Extproperty.Data or byte after it?
                using var pinnedValue = new PinPtr(value);
                System.Buffer.MemoryCopy(((IntPtr)pinnedValue).ToPointer(), IntPtr.Add(((IntPtr)buffer), Marshal.SizeOf<EXTPROPERTY>()).ToPointer(), buffer.Size, Marshal.SizeOf(value));
            }
            if (Convert.ToBoolean(WTExtSet(this.contextHandle, (uint)extTagIndex, buffer)))
            {
                throw new SharpWintabException("WTExtSet");
            }
        }

        private LOGCONTEXTW GetDefaultDigitizingContext()
        {
            var size = WTInfoW((uint)WTI_DEFCONTEXT, 0, default);
            using var buffer = new Buffer<LOGCONTEXTA>(size);
            if (size != buffer.Size)
                this.log.Warning("WTInfoA returned {Size} instead of expected size {BufferSize}", size, buffer.Size);
            size = WTInfoW((uint)WTI_DEFCONTEXT, 0, buffer);
            if (size != buffer.Size)
                this.log.Warning("WTInfoA returned {Size} instead of expected size {BufferSize}", size, buffer.Size);
            this.context = new LOGCONTEXTW(buffer, default);
            this.context.lcSysMode = Convert.ToInt32(false);
            this.context.lcOptions |= (uint)CXO_MESSAGES | (uint)CXO_CSRMESSAGES;
            var PKs = new int[] {
                PK_BUTTONS,
                PK_CHANGED,
                PK_CONTEXT,
                PK_CURSOR,
                PK_NORMAL_PRESSURE,
                PK_ORIENTATION,
                PK_ROTATION,
                PK_SERIAL_NUMBER,
                PK_STATUS,
                PK_TANGENT_PRESSURE,
                PK_TIME,
                PK_X,
                PK_Y,
                PK_Z,
                PKEXT_ABSOLUTE,
                PKEXT_RELATIVE,//TODO: remove this?
            };
            foreach (uint value in PKs)
            {
                this.context.lcPktData |= value;
                // uncomment if relative mode is wanted
                // this.context.LcPktMode |= value
                this.context.lcMoveMask |= value;
            }
            //not sure how these work
            this.context.lcBtnUpMask = this.context.lcBtnDnMask;
            return this.context;
        }

        private (uint, uint) GetIconSize(uint tabletIndex, int extTagIndex, uint functionIndex, uint controlIndex)
        {
            // Get the width of the display icon
            UInt32 IconWidth = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, controlIndex, functionIndex, TABLET_PROPERTY_ICON_WIDTH);
            // Get the height of the display icon
            UInt32 IconHeight = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, controlIndex, functionIndex, TABLET_PROPERTY_ICON_HEIGHT);
            return (IconWidth, IconHeight);
        }

        private string LocationLabel(uint location) =>
            location switch
            {
                0u => "Left button",
                1u => "Right button",
                2u => "Top button",
                3u => "Bottom button",
                _ => "WTF"
            };

        private void OnHandleCreated(object sender, EventArgs e)
        {
            this.AssignHandle(((Form)sender).Handle);
            using var pinnedProperty = new PinPtr(this.Handle);
            this.contextHandle = WTOpenW(new SWIGTYPE_p_HWND(this.Handle.ToPointer(), default), this.context, Convert.ToInt32(true));

            if (this.contextHandle == default)
            {
                throw new SharpWintabException("WTOpenA failed");
            }

            this.AddAllOverrides();
        }

        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            try
            {
                for (uint tabletIndex = 0; tabletIndex < GetNumberOfDevices(); ++tabletIndex)
                {
                    this.RemoveOverrides(tabletIndex);
                }
                if (Convert.ToBoolean(WTClose(this.contextHandle)))
                {
                    throw new SharpWintabException("WTClose failed.");
                }
            }
            finally
            {
                this.ReleaseHandle();
            }
        }

        private void Override(uint tabletIndex, int extTagIndex, uint functionIndex, uint controlIndex)
        {
            RawBool PropertyOverride = true;
            string Name = this.ControlName(tabletIndex, extTagIndex, controlIndex, functionIndex);
            RawBool Available = this.ControlPropertyGet<RawBool>(tabletIndex, extTagIndex, controlIndex, functionIndex, TABLET_PROPERTY_AVAILABLE);
            if (Available)
            {
                this.ControlPropertySet(tabletIndex, extTagIndex, controlIndex, functionIndex, TABLET_PROPERTY_OVERRIDE, PropertyOverride);

                this.ControlPropertySet(tabletIndex, extTagIndex, controlIndex, functionIndex, TABLET_PROPERTY_OVERRIDE_NAME, Name);

                UInt32 Location = this.ControlPropertyGet<UInt32>(tabletIndex, extTagIndex, controlIndex, functionIndex, TABLET_PROPERTY_LOCATION);

                UInt32 MinRange = this.ControlPropertyGet<UInt32>(tabletIndex, extTagIndex, controlIndex, functionIndex, TABLET_PROPERTY_MIN);

                UInt32 MaxRange = this.ControlPropertyGet<UInt32>(tabletIndex, extTagIndex, controlIndex, functionIndex, TABLET_PROPERTY_MAX);

                Int32 IconFormat = this.ControlPropertyGet<Int32>(tabletIndex, extTagIndex, controlIndex, functionIndex, TABLET_PROPERTY_ICON_FORMAT);
                if (IconFormat == TABLET_ICON_FMT_NONE)
                { }
                else if (IconFormat == TABLET_ICON_FMT_4BPP_GRAY)
                {
                    (uint IconWidth, uint IconHeight) = this.GetIconSize(tabletIndex, extTagIndex, functionIndex, controlIndex);
                    /*
                        // send the vector as property "TABLET_PROPERTY_OVERRIDE_ICON"
                        return CtrlPropertySet(ext_I, tablet_I, control_I, function_I,
                            TABLET_PROPERTY_OVERRIDE_ICON, imageData);
                    */
                    this.log.Information("{@params}", new
                    {
                        IconFormat,
                        IconWidth,
                        IconHeight
                    });
                }
                else
                {
                    (uint IconWidth, uint IconHeight) = this.GetIconSize(tabletIndex, extTagIndex, functionIndex, controlIndex);
                    /*
                        // send the vector as property "TABLET_PROPERTY_OVERRIDE_ICON"
                        return CtrlPropertySet(ext_I, tablet_I, control_I, function_I,
                            TABLET_PROPERTY_OVERRIDE_ICON, imageData);
                    */
                    this.log.Information("{@params}", new
                    {
                        IconFormat,
                        IconWidth,
                        IconHeight
                    });
                }
                this.log.Information("OVERRIDDEN: {deviceIdx}, {extensionTagIdx}, {controlIdx}, {functionIdx}", tabletIndex, extTagIndex, controlIndex, functionIndex);
                this.log.Information("{@params}", new
                {
                    PropertyOverride,
                    Name,
                    Location,
                    MinRange,
                    MaxRange,
                });
                this.OverridesAdded = true;
            }
            else
            {
                this.log.Information("UNAVAILABLE: {deviceIdx}, {extensionTagIdx}, {controlIdx}, {functionIdx}", tabletIndex, extTagIndex, controlIndex, functionIndex);
            }
        }

        private void RemoveAllOverrides()
        {
            for (uint tabletIndex = 0; tabletIndex < GetNumberOfDevices(); ++tabletIndex)
            {
                this.RemoveOverrides(tabletIndex);
            }
            this.OverridesAdded = false;
        }

        private void RemoveOverrides(uint tabletIndex)
        {
            foreach (var extTagIndex in this.extensionTags)
            {
                var nofControls = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, 0, 0, TABLET_PROPERTY_CONTROLCOUNT);
                for (uint controlIndex = 0; controlIndex < nofControls; controlIndex++)
                {
                    uint nofFuncs = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, controlIndex, 0, TABLET_PROPERTY_FUNCCOUNT);
                    for (uint functionIndex = 0; functionIndex < nofFuncs; functionIndex++)
                    {
                        this.ControlPropertySet<byte>(tabletIndex, extTagIndex, controlIndex, functionIndex, TABLET_PROPERTY_OVERRIDE, 0);
                    }
                }
            }
        }

        private void timerTickEventHandler(object sender, ElapsedEventArgs e)
        {
            this.log.Information($"Run loop. {DateTime.UtcNow}");
        }
    }
}