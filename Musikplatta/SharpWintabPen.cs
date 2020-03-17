using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
using Serilog;
using Sharp.Wintab;

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

            this.form = form;
            this.form.HandleCreated += new EventHandler(this.OnHandleCreated);
            this.form.HandleDestroyed += new EventHandler(this.OnHandleDestroyed);
        }

        ~SharpWintabPen()
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

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                this.isDisposed = true;
            }
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            //switch (m.Msg)
            //{
            //    case (int)WindowsEventMessages.WM_ACTIVATE:
            //    {
            //        switch (Marshal.PtrToStructure<uint>(m.WParam))
            //        {
            //            case (int)WParam.WA_INACTIVE:
            //            {
            //                //NOTE: It is recommended that applications remove their overrides when the application's main window is no longer active (use WM_ACTIVATE). Extension overrides take effect across the entire system; if an application leaves its overrides in place, that control will not function correctly in other applications.
            //                this.RemoveOverrides();
            //                //When applications receive the WM_ACTIVATE message, they should push their contexts to the bottom of the overlap order if their application is being deactivated, and should bring their context to the top if they are being activated.
            //                this.wtContext.SetOverlapOrder(toTop_I: false);
            //                break;
            //            }
            //            case (int)WParam.WA_ACTIVE:
            //            case (int)WParam.WA_CLICKACTIVE:
            //            {
            //                this.AddOverrides();
            //                //When applications receive the WM_ACTIVATE message, they should push their contexts to the bottom of the overlap order if their application is being deactivated, and should bring their context to the top if they are being activated.
            //                this.wtContext.SetOverlapOrder(toTop_I: true);
            //                break;
            //            }
            //            default:
            //                break;
            //        }
            //        break;
            //    }
            //    case (int)WindowsEventMessages.WM_SYSCOMMAND:
            //    {
            //        switch (Marshal.PtrToStructure<uint>(m.WParam))
            //        {
            //            case (uint)WParam.SC_MINIMIZE:
            //            {
            //                //Applications should also disable their contexts when they are minimized.
            //                this.wtContext.Enable(enable_I: false);
            //                break;
            //            }
            //            case (uint)WParam.SC_RESTORE:
            //            case (uint)WParam.SC_MAXIMIZE:
            //            {
            //                this.wtContext.Enable(enable_I: true);
            //                break;
            //            }
            //            default:
            //                break;
            //        }
            //        break;
            //    }
            //    case (int)WindowsEventMessages.WM_LBUTTONDOWN:
            //        //stylus button
            //        break;

            //    case (int)WindowsEventMessages.WM_LBUTTONUP:
            //        //stylus button
            //        break;

            //    case (int)Wt.Packet:
            //    {
            //        /*
            //            +-------------+--------------------------------------------------------------------------------------------------------------------+
            //            | Description | The WT_PACKET message is posted to the context-owning windows that have requested messag­ing for their context.     |
            //            +-------------+-----------+--------------------------------------------------------------------------------------------------------+
            //            |             | Parameter | Description                                                                                            |
            //            +-------------+-----------+--------------------------------------------------------------------------------------------------------+
            //            |             | wParam    | Contains the serial number of the packet that generated the mesÂ­sage.                                  |
            //            +-------------+-----------+--------------------------------------------------------------------------------------------------------+
            //            |             | lParam    | Contains the handle of the context that processed the packet.                                          |
            //            +-------------+-----------+--------------------------------------------------------------------------------------------------------+
            //            | Comments    | Applications should call the WTPacket function or other packet retrieval func­tions upon receiving this message.    |
            //            +-------------+--------------------------------------------------------------------------------------------------------------------+
            //            | See Also    | The WTPacket function in section 6.1.1, and the advanced packet and queue func­tions in section 5.4.                |
            //            +-------------+--------------------------------------------------------------------------------------------------------------------+
            //        */
            //        /*
            //            The interface defines a standard orientation for reporting device native coordinates.
            //            When the user is viewing the device in its normal position, the coordinate origin will be at the lower left of the device.
            //            The coordinate system will be right-handed, that is,
            //                the positive x axis points from left to right, and
            //                the posi­tive y axis points either upward or away from the user.
            //                The z axis, if supported, points either to­ward the user or upward.
            //                For devices that lay flat on a table top, the x-y plane will be horizontal and the z axis will point upward.
            //                For devices that are oriented vertically (for example, a touch screen on a conventional dis­play), the x-y plane will be vertical, and the z axis will point toward the user.
            //        */
            //        uint hCtx = Marshal.PtrToStructure<uint>(m.LParam);
            //        uint pktID = Marshal.PtrToStructure<uint>(m.WParam);
            //        var pkt = this.wtData.GetDataPacket(hCtx, pktID);
            //        this.log.Information("{packet}", new
            //        {
            //            x = pkt.pkX,
            //            y = pkt.pkY,
            //            z = pkt.pkZ,
            //            p_n = pkt.pkNormalPressure,
            //            p_t = pkt.pkTangentPressure,
            //            altitude = pkt.pkOrientation.orAltitude,
            //            azimuth = pkt.pkOrientation.orAzimuth,
            //            twist = pkt.pkOrientation.orTwist,
            //            pitch = pkt.pkRotation.rotPitch,
            //            roll = pkt.pkRotation.rotRoll,
            //            yaw = pkt.pkRotation.rotYaw,
            //            pkt.pkContext,
            //            pkt.pkStatus,
            //            pkt.pkTime,
            //            pkt.pkChanged,
            //            pkt.pkSerialNumber,
            //            pkt.pkCursor,
            //            pkt.pkButtons,
            //        });
            //        break;
            //    }
            //    case (int)Wt.Packetext:
            //    {
            //        uint hCtx = Marshal.PtrToStructure<uint>(m.LParam);
            //        uint pktID = Marshal.PtrToStructure<uint>(m.WParam);
            //        this.log.Information("GetDataPacketExt({@context}, {@packetId})", hCtx, pktID);
            //        var pktExt = this.wtData.GetDataPacketExt(hCtx, pktID);
            //        this.log.Information("{packetExtension}", new
            //        {
            //            BaseContext = pktExt.pkBase.nContext,
            //            BaseSerialNumber = pktExt.pkBase.nSerialNumber,
            //            BaseStatus = pktExt.pkBase.nStatus,
            //            BaseTime = pktExt.pkBase.nTime,
            //            ExpressKeyControl = pktExt.pkExpKey.nControl,
            //            ExpressKeyLocation = pktExt.pkExpKey.nLocation,
            //            ExpressKeyReserved = pktExt.pkExpKey.nReserved,
            //            ExpressKeyState = pktExt.pkExpKey.nState,
            //            ExpressKeyTablet = pktExt.pkExpKey.nTablet,
            //            TouchRingControl = pktExt.pkTouchRing.nControl,
            //            TouchRingMode = pktExt.pkTouchRing.nMode,
            //            TouchRingPosition = pktExt.pkTouchRing.nPosition,
            //            TouchRingReserver = pktExt.pkTouchRing.nReserved,
            //            TouchRingTablet = pktExt.pkTouchRing.nTablet,
            //            TouchStripControl = pktExt.pkTouchStrip.nControl,
            //            TouchStripMode = pktExt.pkTouchStrip.nMode,
            //            TouchStripPosition = pktExt.pkTouchStrip.nPosition,
            //            TouchStripReserved = pktExt.pkTouchStrip.nReserved,
            //            TouchStripTablet = pktExt.pkTouchStrip.nTablet
            //        });
            //        break;
            //    }
            //    case (int)Wt.Ctxopen:
            //    {
            //        /*
            //            +---+-------------------------------------------------+---------------+
            //            | D | The WT_CTXOPEN message is sent to the owning    |               |
            //            | e | window and to any manager windows when a        |               |
            //            | s.| context is opened.                              |               |
            //            +===+=================================================+===============+
            //            |   | Parameter                                       | Description   |
            //            +---+-------------------------------------------------+---------------+
            //            |   | wParam                                          | Contains the  |
            //            |   |                                                 | context       |
            //            |   |                                                 | handle of the |
            //            |   |                                                 | opened        |
            //            |   |                                                 | context.      |
            //            +---+-------------------------------------------------+---------------+
            //            |   | lParam                                          | Contains the  |
            //            |   |                                                 | current       |
            //            |   |                                                 | context       |
            //            |   |                                                 | status flags. |
            //            +---+-------------------------------------------------+---------------+
            //            | C | Tablet manager applications should save the     |               |
            //            | o | handle of the new context. Managers may want    |               |
            //            | m | to query the user to configure the context      |               |
            //            | m.| further at this time.                           |               |
            //            +---+-------------------------------------------------+---------------+
            //        */
            //        break;
            //    }
            //    case (int)Wt.Ctxclose:
            //    /*
            //        +-------------+-------------------------------------------------------------------------------------------------------------------------+
            //        | Description | The WT_CTXCLOSE message is sent to the owning winÂ­dow and to any manager windows when a context is about to be closed.  |
            //        +-------------+-----------+-------------------------------------------------------------------------------------------------------------+
            //        |             | Parameter | Description                                                                                                 |
            //        +-------------+-----------+-------------------------------------------------------------------------------------------------------------+
            //        |             | wParam    | Contains the context handle of the context to be closed.                                                    |
            //        +-------------+-----------+-------------------------------------------------------------------------------------------------------------+
            //        |             | lParam    | Contains the current context status flags.                                                                  |
            //        +-------------+-----------+-------------------------------------------------------------------------------------------------------------+
            //        | Comments    | Tablet manager applications should note that the context is being closed and take appropriate action.                   |
            //        +-------------+-------------------------------------------------------------------------------------------------------------------------+
            //    */
            //    case (int)Wt.Ctxupdate:
            //    /*
            //        +-------------+-----------------------------------------------------------------------------------------------------------------------------+
            //        | Description | The WT_CTXUPDATE message is sent to the owning window and to any manager windows when a context is changed.                 |
            //        +-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
            //        |             | Parameter | Description                                                                                                     |
            //        +-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
            //        |             | wParam    | Contains the context handle of the changed context.                                                             |
            //        +-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
            //        |             | lParam    | Contains the current context status flags.                                                                      |
            //        +-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
            //        | Comments    | Applications may want to call WTGet or WTExtGet on receiving this message to find out what context attributes were changed. |
            //        +-------------+-----------------------------------------------------------------------------------------------------------------------------+
            //    */
            //    case (int)Wt.Ctxoverlap:
            //    /*
            //        +-------------+-----------------------------------------------------------------------------------------------------------------------------------+
            //        | Description | The WT_CTXOVERLAP message is sent to the owning window and to any manager windows when a context is moved in the overlap order.   |
            //        +-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
            //        |             | Parameter | Description                                                                                                           |
            //        +-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
            //        |             | wParam    | Contains the context handle of the re-overlapped context.                                                             |
            //        +-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
            //        |             | lParam    | Contains the current context status flags.                                                                            |
            //        +-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
            //        | Comments    | Tablet managers can handle this message to keep track of context overlap reÂ­quests by applications.                               |
            //        +-------------+-----------------------------------------------------------------------------------------------------------------------------------+
            //        |             | Applications can handle this message to find out when their context is obscured by another context.                               |
            //        +-------------+-----------------------------------------------------------------------------------------------------------------------------------+
            //    */
            //    case (int)Wt.Proximity:
            //    /*
            //        +-------------+---------------------------------------------------------------------------------------------------------------------------------------+
            //        | Description | The WT_PROXIMITY message is posted to the owning window and any manager windows when the cursor enters or leaves context proximity.   |
            //        +-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
            //        |             | Parameter | Description                                                                                                               |
            //        +-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
            //        |             | wParam    | Contains the handle of the context that the cursor is entering or leavÂ­ing.                                               |
            //        +-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
            //        |             | lParam    | The low-order word is non-zero when the cursor is entering the context and zero when it is leaving the context.           |
            //        |             |           | The high-order word is non-zero when the cursor is leaving or entering hardÂ­ware proximity.                               |
            //        +-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
            //        | Comments    | Proximity events are handled separately from regular tablet events.                                                                   |
            //        |             | Applications will receive proximity messages even if they haven't requested event messages.                                           |
            //        +-------------+---------------------------------------------------------------------------------------------------------------------------------------+
            //    */
            //    case (int)Wt.Infochange:
            //    /*
            //            +---+----------------------------------------+---------------------------+
            //            | D | The WT_INFOCHANGE message is sent to   |                           |
            //            | e | all mangager and context-owning        |                           |
            //            | s | windows when the number of connected   |                           |
            //            | c.| tablets has changed                    |                           |
            //            +===+========================================+===========================+
            //            |   | Parameter                              | Description               |
            //            +---+----------------------------------------+---------------------------+
            //            |   | wParam                                 | Contains the manager      |
            //            |   |                                        | handle of the tablet      |
            //            |   |                                        | manager that changed the  |
            //            |   |                                        | information, or zero if   |
            //            |   |                                        | the change was reported   |
            //            |   |                                        | through hardware.         |
            //            +---+----------------------------------------+---------------------------+
            //            |   | lParam                                 | Contains category and     |
            //            |   |                                        | index numbers for the     |
            //            |   |                                        | changed information.      |
            //            |   |                                        | The low-order word        |
            //            |   |                                        | contains the category     |
            //            |   |                                        | number; the high-order    |
            //            |   |                                        | word contains the index   |
            //            |   |                                        | number.                   |
            //            +---+----------------------------------------+---------------------------+
            //            | C | Applications can respond to capability |                           |
            //            | o | changes by handling this message.      |                           |
            //            | m | Simple applications may want to prompt |                           |
            //            | m | the user to close and restart them.    |                           |
            //            | e | The most basic applications that use   |                           |
            //            | n | the default context should not need to |                           |
            //            | t | take any action.                       |                           |
            //            | s |                                        |                           |
            //            +---+----------------------------------------+---------------------------+
            //            |   | Tablet managers should handle this     |                           |
            //            |   | message to react to hardware           |                           |
            //            |   | configuration changes or changes by    |                           |
            //            |   | other managers (in implementations     |                           |
            //            |   | that allow multiple tablet managers).  |                           |
            //            +---+----------------------------------------+---------------------------+
            //    */
            //    case (int)Wt.Csrchange:
            //    {
            //        /*
            //            +-------------+-------------------------------------------------------------------------------------------------+
            //            | Description | The WT_CSRCHANGE message is posted to the owning window when a new cursor enters the context.   |
            //            +-------------+-----------+-------------------------------------------------------------------------------------+
            //            |             | Parameter | Description                                                                         |
            //            +-------------+-----------+-------------------------------------------------------------------------------------+
            //            |             | wParam    | Contains the serial number of the packet that generated the mesÂ­sage.               |
            //            +-------------+-----------+-------------------------------------------------------------------------------------+
            //            |             | lParam    | Contains the handle of the context that processed the packet.                       |
            //            +-------------+-----------+-------------------------------------------------------------------------------------+
            //            | Comments    | Only contexts that have the CXO_CSRMESSAGES option selected will generate this message.         |
            //            +-------------+-------------------------------------------------------------------------------------------------+
            //        */
            //        break;
            //    }
            //    default:
            //        break;
            //}
            base.WndProc(ref m);
        }

        private static Logcontexta GetDefaultDigitizingContext()
        {
            using var buffer = new Buffer<Logcontexta>();
            if (Functions.WTInfoA((uint)Wti.Defcontext, 0, IntPtr.Zero) != buffer.Size)
                throw new SharpWintabException();
            Functions.WTInfoA((uint)Wti.Defcontext, 0, buffer);

            var context = (Logcontexta)buffer;
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

        private static uint GetExtensionCategoryOffset(Wtx extensionTag)
        {
            using var buffer = new Buffer<Wtx>();
            /* scan for wTag's info category. */
            for (
                uint extensionCategoryOffset = 0;
                Functions.WTInfoA((uint)Wti.Extensions + extensionCategoryOffset, (uint)Ext.Tag, buffer) > 0;
                extensionCategoryOffset++)
            {
                if (extensionTag == (Wtx)buffer)
                {
                    return extensionCategoryOffset;
                }
            }
            throw new SharpWintabException();
        }

        private static int GetExtensionMask(Wtx extensionTag)
        {
            var categoryOffset = GetExtensionCategoryOffset(extensionTag);
            using var buffer = new Buffer<int>();
            if (Functions.WTInfoA((uint)Wti.Extensions + categoryOffset, (uint)Ext.Mask, buffer) != buffer.Size)
                throw new SharpWintabException();
            return (int)buffer;
        }

        private static uint GetNumberOfDevices()
        {
            using var buffer = new Buffer<uint>();
            if (Functions.WTInfoA((uint)Wti.Interface, (uint)Ifc.Ndevices, buffer) != buffer.Size)
            {
                throw new SharpWintabException();
            }
            return (uint)buffer;
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
                throw new SharpWintabException();
            }
            switch (typeof(T))
            {
                case var cls when cls == typeof(Boolean):
                {
                    return (T)(object)BitConverter.ToBoolean(((Extproperty)buffer).Data);
                }
                case var cls when cls == typeof(Int16):
                {
                    return (T)(object)BitConverter.ToInt16(((Extproperty)buffer).Data);
                }
                case var cls when cls == typeof(UInt16):
                {
                    return (T)(object)BitConverter.ToUInt16(((Extproperty)buffer).Data);
                }
                case var cls when cls == typeof(Int32):
                {
                    return (T)(object)BitConverter.ToInt32(((Extproperty)buffer).Data);
                }
                case var cls when cls == typeof(UInt32):
                {
                    return (T)(object)BitConverter.ToUInt32(((Extproperty)buffer).Data);
                }
                default:
                    throw new NotSupportedException();
            }
        }

        private void OnHandleCreated(object sender, EventArgs e)
        {
            this.AssignHandle(((Form)sender).Handle);

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

            this.contextHandle = Sharp.Wintab.Functions.WTOpenA(Marshal.PtrToStructure<int>(this.Handle), ref this.context, true);
            if (this.contextHandle.Unused == 0)
            {
                throw new SharpWintabException();
            }

            for (uint tabletIndex = 0; tabletIndex < GetNumberOfDevices(); ++tabletIndex)
            {
                this.AddOverrides(tabletIndex);
            }
        }

        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            this.ReleaseHandle();
        }

        private void Override(uint tabletIndex, Wtx extTagIndex, uint functionIndex, uint controlIndex)
        {
            uint PropertyOverride = 1;
            string Name = this.ControlName(tabletIndex, extTagIndex, controlIndex, functionIndex);
            uint Available = this.ControlPropertyGet<uint>(tabletIndex, extTagIndex, controlIndex, functionIndex, Tablet.PropertyOverride);
            if (Available > 0)
            { 
                
            }
            uint Location = default;
            uint MinRange = default;
            uint MaxRange = default;
            throw new NotImplementedException();
        }

        private void timerTickEventHandler(object sender, ElapsedEventArgs e)
        {
            this.log.Information($"Run loop. {DateTime.UtcNow}");
        }
    }
}