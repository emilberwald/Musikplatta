using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Newtonsoft.Json;
using Serilog;
using SharpGen.Runtime.Win32;
using SharpWintab.Wintab;

namespace Musikplatta
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class SharpWintabPen : NativeWindow, IWintabPen
    {
        private Logcontexta context;
        private Hctx contextHandle;
        private Wtx[] extensionTags;
        private Form form;
        private bool isDisposed = false;

        private ILogger log;
        private System.Timers.Timer timer;

        public SharpWintabPen(ILogger log, Form form)
        {
            this.log = log;

            this.timer = new System.Timers.Timer();
            this.timer.Interval = 1000;
            this.timer.Elapsed += new ElapsedEventHandler(this.timerTickEventHandler);

            this.context = GetDefaultDigitizingContext();
            this.extensionTags = new Wtx[] { Wtx.Touchstrip, Wtx.Touchring, Wtx.Expkeys2 };
            foreach (var extensionTag in this.extensionTags)
            {
                try
                {
                    this.context.LcPktData |= GetExtensionMask(extensionTag);
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

        ~SharpWintabPen()
        {
            this.Dispose(false);
        }

        public bool OverridesAdded { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Packet GetDataPacket(Message m)
        {
            using var buffer = new Buffer<Packet>();
            if (!Functions.WTPacket(new Hctx()
            {
                Unused = Marshal.PtrToStructure<int>(m.LParam)
            }, Marshal.PtrToStructure<uint>(m.WParam), buffer))
            {
                throw new SharpWintabException("GetDataPacketExt: The return value is non-zero if the specified packet was found and returned. It is zero if the specified packet was not found in the queue.");
            }
            return (Packet)buffer;
        }

        public Packetext GetDataPacketExt(Message m)
        {
            using var buffer = new Buffer<Packetext>();
            if (!Functions.WTPacket(new Hctx()
            {
                Unused = Marshal.PtrToStructure<int>(m.LParam)
            }, Marshal.PtrToStructure<uint>(m.WParam), buffer))
            {
                throw new SharpWintabException("GetDataPacketExt: The return value is non-zero if the specified packet was found and returned. It is zero if the specified packet was not found in the queue.");
            }
            return (Packetext)buffer;
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
                    switch (Marshal.PtrToStructure<uint>(m.WParam))
                    {
                        case (int)WParam.WA_INACTIVE:
                        {
                            //NOTE: It is recommended that applications remove their overrides when the application's main window is no longer active (use WM_ACTIVATE). Extension overrides take effect across the entire system; if an application leaves its overrides in place, that control will not function correctly in other applications.
                            this.RemoveAllOverrides();
                            //When applications receive the WM_ACTIVATE message, they should push their contexts to the bottom of the overlap order if their application is being deactivated, and should bring their context to the top if they are being activated.
                            Functions.WTOverlap(this.contextHandle, false);
                            break;
                        }
                        case (int)WParam.WA_ACTIVE:
                        case (int)WParam.WA_CLICKACTIVE:
                        {
                            this.AddAllOverrides();
                            //When applications receive the WM_ACTIVATE message, they should push their contexts to the bottom of the overlap order if their application is being deactivated, and should bring their context to the top if they are being activated.
                            Functions.WTOverlap(this.contextHandle, true);
                            break;
                        }
                        default:
                            break;
                    }
                    break;
                }
                case (int)WindowsEventMessages.WM_SYSCOMMAND:
                {
                    switch (Marshal.PtrToStructure<uint>(m.WParam))
                    {
                        case (uint)WParam.SC_MINIMIZE:
                        {
                            //Applications should also disable their contexts when they are minimized.
                            Functions.WTEnable(this.contextHandle, false); //The function returns a non-zero value if the enable or disable request was satis­fied, zero otherwise.
                            break;
                        }
                        case (uint)WParam.SC_RESTORE:
                        case (uint)WParam.SC_MAXIMIZE:
                        {
                            Functions.WTEnable(this.contextHandle, true); //The function returns a non-zero value if the enable or disable request was satis­fied, zero otherwise.
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

                case (int)Wt.Packet:
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
                case (int)Wt.Packetext:
                {
                    var pktExt = this.GetDataPacketExt(m);
                    this.log.Information("{Packetext}", JsonConvert.SerializeObject(pktExt));
                    break;
                }
                case (int)Wt.Ctxopen:
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
                    break;
                }
                case (int)Wt.Ctxclose:
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
                case (int)Wt.Ctxupdate:
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
                case (int)Wt.Ctxoverlap:
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
                case (int)Wt.Proximity:
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
                case (int)Wt.Infochange:
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
                case (int)Wt.Csrchange:
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
                    break;
                }
                default:
                    break;
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

        private static uint GetExtensionCategoryOffset(Wtx extensionTag)
        {
            using var buffer = new Buffer<Int32>();
            /* scan for wTag's info category. */
            for (
                uint extensionCategoryOffset = 0;
                Functions.WTInfoA((uint)Wti.Extensions + extensionCategoryOffset, (uint)Ext.Tag, buffer) > 0;
                extensionCategoryOffset++)
            {
                if (extensionTag == (Wtx)(Int32)buffer)
                {
                    return extensionCategoryOffset;
                }
            }
            throw new SharpWintabException("GetExtensionCategoryOffset");
        }

        private static int GetExtensionMask(Wtx extensionTag)
        {
            var categoryOffset = GetExtensionCategoryOffset(extensionTag);
            using var buffer = new Buffer<int>();
            if (Functions.WTInfoA((uint)Wti.Extensions + categoryOffset, (uint)Ext.Mask, buffer) != buffer.Size)
                throw new SharpWintabException("GetExtensionMask");
            return (int)buffer;
        }

        private static uint GetNumberOfDevices()
        {
            using var buffer = new Buffer<uint>();
            if (Functions.WTInfoA((uint)Wti.Interface, (uint)Ifc.Ndevices, buffer) != buffer.Size)
            {
                throw new SharpWintabException("GetNumberOfDevices");
            }
            return (uint)buffer;
        }

        private void AddAllOverrides()
        {
            for (uint tabletIndex = 0; tabletIndex < GetNumberOfDevices(); ++tabletIndex)
            {
                this.AddOverrides(tabletIndex);
            }
        }

        private void AddOverrides(uint tabletIndex)
        {
            foreach (var extTagIndex in this.extensionTags)
            {
                var nofControls = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, 0, 0, Tablet.PropertyControlcount);
                for (uint controlIndex = 0; controlIndex < nofControls; controlIndex++)
                {
                    uint nofFuncs = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, controlIndex, 0, Tablet.PropertyFunccount);
                    for (uint functionIndex = 0; functionIndex < nofFuncs; functionIndex++)
                    {
                        this.Override(tabletIndex, extTagIndex, functionIndex, controlIndex);
                    }
                }
            }
        }

        private string ControlName(uint tabletIndex, Wtx extTagIndex, uint controlIndex, uint functionIndex) => extTagIndex switch
        {
            _ => Enum.GetName(typeof(Wtx), extTagIndex) + Convert.ToString(tabletIndex) + Convert.ToString(controlIndex) + Convert.ToString(functionIndex)
        };

        private T ControlPropertyGet<T>(uint tabletIndex, Wtx extTagIndex, uint controlIndex, uint functionIndex, Tablet propertyId)
        {
            using var buffer = new Buffer<Extproperty>();
            Marshal.StructureToPtr(new Extproperty()
            {
                Version = 0,
                TabletIndex = (byte)tabletIndex,
                ControlIndex = (byte)controlIndex,
                FunctionIndex = (byte)functionIndex,
                PropertyID = (ushort)propertyId,
                DataSize = Marshal.SizeOf<T>(),
                Reserved = 0,
            }, buffer, false);

            if (!Functions.WTExtGet(this.contextHandle, (uint)extTagIndex, buffer))
            {
                throw new SharpWintabException("WTExtGet");
            }
            return BitConvertTo<T>(((Extproperty)buffer).Data);
        }

        private void ControlPropertySet<T>(uint tabletIndex, Wtx extTagIndex, uint controlIndex, uint functionIndex, Tablet property, T value_t)
        {
            byte[] value = BitConvertFrom(value_t);
            // From Wintab 1.4 specification:
            // "Note that this stucture is of variable size. The total size of the structure is sizeof(EXTPROPERTY) + dataSize. Take care to allocate the correct amount of memory before using this structure."
            using var buffer = new Buffer<Extproperty>(Marshal.SizeOf<Extproperty>() + Marshal.SizeOf(value));
            var extProperty = new Extproperty()
            {
                Version = 0,
                TabletIndex = (byte)tabletIndex,
                ControlIndex = (byte)controlIndex,
                FunctionIndex = (byte)functionIndex,
                PropertyID = (ushort)property,
                Reserved = 0,
                DataSize = Marshal.SizeOf(value),
            };
            Marshal.StructureToPtr(extProperty, buffer, false);
            IntPtr lastByteOfExtProperty = IntPtr.Add((IntPtr)buffer, Marshal.SizeOf<Extproperty>() - 1);
            for (int offset = 0; offset < Marshal.SizeOf(value); offset++)
            {
                Marshal.WriteByte(lastByteOfExtProperty, offset, value[offset]);
            }

            System.Buffer.BlockCopy(value, 0, extProperty.Data, 0, (int)extProperty.DataSize);
            if (!Functions.WTExtSet(this.contextHandle, (uint)extTagIndex, buffer))
            {
                throw new SharpWintabException("WTExtSet");
            }
        }

        private Logcontexta GetDefaultDigitizingContext()
        {
            Logcontexta context = default;
            {
                var size = Functions.WTInfoA((uint)Wti.Defcontext, 0, IntPtr.Zero);
                using var buffer = new Buffer<Logcontexta>(size);
                if (size != buffer.Size)
                    this.log.Warning("WTInfoA returned {Size} instead of expected size {BufferSize}", size, buffer.Size);
                size = Functions.WTInfoA((uint)Wti.Defcontext, 0, buffer);
                if (size != buffer.Size)
                    this.log.Warning("WTInfoA returned {Size} instead of expected size {BufferSize}", size, buffer.Size);
                context = (Logcontexta)buffer;
            }
            context.LcSysMode = false;
            context.LcOptions |= (uint)Cxo.Messages | (uint)Cxo.Csrmessages;
            foreach (int value in Enum.GetValues(typeof(Pk)))
            {
                context.LcPktData |= value;
                // uncomment if relative mode is wanted
                // this.context.LcPktMode |= value
                context.LcMoveMask |= value;
            }
            //not sure how these work
            context.LcBtnUpMask = context.LcBtnDnMask;
            context.LcName = "SharpWintab Default Digitizing Event Data Context";
            return context;
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
            var handle = Marshal.PtrToStructure<int>(this.Handle);
            this.contextHandle = Functions.WTOpenA(handle, ref this.context, true);
            if (this.contextHandle.Unused == 0)
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
                if (!Functions.WTClose(this.contextHandle))
                {
                    throw new SharpWintabException("WTClose failed.");
                }
            }
            finally
            {
                this.ReleaseHandle();
            }
        }

        private void Override(uint tabletIndex, Wtx extTagIndex, uint functionIndex, uint controlIndex)
        {
            RawBool PropertyOverride = true;
            string Name = this.ControlName(tabletIndex, extTagIndex, controlIndex, functionIndex);
            RawBool Available = this.ControlPropertyGet<RawBool>(tabletIndex, extTagIndex, controlIndex, functionIndex, Tablet.PropertyAvailable);
            if (Available)
            {
                this.ControlPropertySet(tabletIndex, extTagIndex, controlIndex, functionIndex, Tablet.PropertyOverride, PropertyOverride);

                this.ControlPropertySet(tabletIndex, extTagIndex, controlIndex, functionIndex, Tablet.PropertyOverrideName, Name);

                UInt32 Location = this.ControlPropertyGet<UInt32>(tabletIndex, extTagIndex, controlIndex, functionIndex, Tablet.PropertyLocation);

                UInt32 MinRange = this.ControlPropertyGet<UInt32>(tabletIndex, extTagIndex, controlIndex, functionIndex, Tablet.PropertyMin);

                UInt32 MaxRange = this.ControlPropertyGet<UInt32>(tabletIndex, extTagIndex, controlIndex, functionIndex, Tablet.PropertyMax);

                Int32 IconFormat = this.ControlPropertyGet<Int32>(tabletIndex, extTagIndex, controlIndex, functionIndex, Tablet.PropertyIconFormat);
                switch (IconFormat)
                {
                    case (Int32)Tablet.IconFmtNone:
                        break;

                    case (Int32)Tablet.IconFmt4bppGray:
                    default:
                    {
                        // Get the width of the display icon
                        UInt32 IconWidth = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, controlIndex, functionIndex, Tablet.PropertyIconWidth);
                        // Get the height of the display icon
                        UInt32 IconHeight = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, controlIndex, functionIndex, Tablet.PropertyIconHeight);
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
                        break;
                    }
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
                var nofControls = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, 0, 0, Tablet.PropertyControlcount);
                for (uint controlIndex = 0; controlIndex < nofControls; controlIndex++)
                {
                    uint nofFuncs = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, controlIndex, 0, Tablet.PropertyFunccount);
                    for (uint functionIndex = 0; functionIndex < nofFuncs; functionIndex++)
                    {
                        this.ControlPropertySet<byte>(tabletIndex, extTagIndex, controlIndex, functionIndex, Tablet.PropertyOverride, 0);
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