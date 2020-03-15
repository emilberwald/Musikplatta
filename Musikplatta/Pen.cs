using System;
using System.Collections.Generic;
using Serilog;
using WintabDN;

namespace Musikplatta
{
    public interface ITouch : IDisposable { };

    public class Pen : ITouch
    {
        private bool disposedValue = false;
        private bool HasOverrides;
        private ILogger log;
        private CWintabContext wtContext;
        private CWintabData wtData;

        public Pen(ILogger log)
        {
            this.log = log;
            this.wtContext = CWintabInfo.GetDefaultDigitizingContext(ECTXOptionValues.CXO_MESSAGES);
            var extensionMasks = new Dictionary<EWTXExtensionTag, uint>
            {
                { EWTXExtensionTag.WTX_TOUCHSTRIP, CWintabExtensions.GetWTExtensionMask(EWTXExtensionTag.WTX_TOUCHSTRIP) },
                { EWTXExtensionTag.WTX_TOUCHRING, CWintabExtensions.GetWTExtensionMask(EWTXExtensionTag.WTX_TOUCHRING) },
                { EWTXExtensionTag.WTX_EXPKEYS2, CWintabExtensions.GetWTExtensionMask(EWTXExtensionTag.WTX_EXPKEYS2) },
            };

            foreach (var (extensionTag, extensionMask) in extensionMasks)
            {
                if (extensionMask > 0)
                {
                    this.log.Information("Enabling {extensionTag}.", extensionTag);
                    this.wtContext.PktData |= (WTPKT)extensionMask;
                }
            }

            if (!this.wtContext.Open())
            {
                throw new InvalidOperationException("Failed to open context.");
            }

            this.wtData = new CWintabData(this.wtContext);
            this.wtData.SetWTPacketEventHandler(this.WinTabPacketEventHandler);

            {
                var nofAttachedTablets = CWintabInfo.GetNumberOfDevices();
                var nofConfiguredTablets = CWintabInfo.GetNumberOfConfiguredDevices();
                if (nofAttachedTablets != nofConfiguredTablets)
                {
                    this.log.Warning("{@nofAttachedTables} != {@nofConfiguredTables}", nofAttachedTablets, nofConfiguredTablets);
                }
            }

            this.AddOverrides();
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~Pen()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(false);
        }

        // To detect redundant calls This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    this.wtData.RemoveWTPacketEventHandler(this.WinTabPacketEventHandler);
                    this.RemoveOverrides();
                    this.wtContext.Close();
                }
                this.disposedValue = true;
            }
        }

        private static string ControlName(EWTXExtensionTag extTagIndex_I, byte controlIndex, uint functionIndex) => extTagIndex_I switch
        {
            EWTXExtensionTag.WTX_EXPKEYS2 => "EK: " + Convert.ToString(controlIndex),
            EWTXExtensionTag.WTX_TOUCHRING => "TR: " + Convert.ToString(functionIndex),
            EWTXExtensionTag.WTX_TOUCHSTRIP => "TS: " + Convert.ToString(functionIndex),
            _ => "UK: " + Convert.ToString(functionIndex),
        };

        private void AddOverrides()
        {
            if (this.HasOverrides)
            {
                this.RemoveOverrides();
            }
            this.log.Information("AddOverrides");
            foreach (byte tabletIndex in CWintabInfo.GetFoundDevicesIndexList())
            {
                foreach (EWTXExtensionTag extTagIndex in Enum.GetValues(typeof(EWTXExtensionTag)))
                {
                    uint numCtrls = default;
                    CWintabExtensions.ControlPropertyGet(
                    this.wtContext.HCtx,
                    (byte)extTagIndex,
                    tabletIndex,
                    0,
                    0,
                    (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_CONTROLCOUNT,
                    ref numCtrls);

                    for (byte controlIndex = 0; controlIndex < numCtrls; controlIndex++)
                    {
                        uint numFuncs = default;
                        CWintabExtensions.ControlPropertyGet(
                        this.wtContext.HCtx,
                        (byte)extTagIndex,
                        tabletIndex,
                        controlIndex,
                        0,
                        (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_FUNCCOUNT,
                        ref numFuncs);

                        for (uint functionIndex = 0; functionIndex < numFuncs; functionIndex++)
                        {
                            this.Override(tabletIndex, extTagIndex, functionIndex, controlIndex);
                        }
                    }
                }
            }
        }

        private dynamic GetLimits(int deviceIndex)
        {
            var MaxNormalPressure = CWintabInfo.GetMaxPressure(true);
            var MaxTangentialPressure = CWintabInfo.GetMaxPressure(false);
            var XAxis = CWintabInfo.GetDeviceAxis(deviceIndex, EAxisDimension.AXIS_X);
            var YAxis = CWintabInfo.GetDeviceAxis(deviceIndex, EAxisDimension.AXIS_Y);
            var ZAxis = CWintabInfo.GetDeviceAxis(deviceIndex, EAxisDimension.AXIS_Z);

            bool RotationSupported;
            var Rotation = CWintabInfo.GetDeviceRotation(out RotationSupported);
            return new
            {
                MaxNormalPressure,
                MaxTangentialPressure,
                XAxis,
                YAxis,
                ZAxis,
                RotationSupported,
                Rotation
            };
        }

        private void Override(byte tabletIndex, EWTXExtensionTag extTagIndex, uint functionIndex, byte controlIndex)
        {
            uint PropertyOverride = 1;
            string Name = ControlName(extTagIndex, controlIndex, functionIndex);
            uint Available = default;
            uint Location = default;
            uint MinRange = default;
            uint MaxRange = default;

            CWintabExtensions.ControlPropertyGet(
            this.wtContext.HCtx,
            (byte)extTagIndex,
            tabletIndex,
            controlIndex,
            (byte)functionIndex,
            (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_AVAILABLE,
            ref Available);

            if (Available > 0)
            {
                if (!CWintabExtensions.ControlPropertySet(
                this.wtContext.HCtx,
                (byte)extTagIndex,
                tabletIndex,
                controlIndex,
                (byte)functionIndex,
                (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_OVERRIDE,
                PropertyOverride))
                {
                    this.log.Warning("TABLET_PROPERTY_OVERRIDE failed.");
                }

                if (!CWintabExtensions.ControlPropertySet(
                this.wtContext.HCtx,
                (byte)extTagIndex,
                tabletIndex,
                controlIndex,
                (byte)functionIndex,
                (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_OVERRIDE_NAME,
                Name))
                {
                    this.log.Warning("TABLET_PROPERTY_OVERRIDE_NAME failed.");
                }

                if (!CWintabExtensions.ControlPropertyGet(
                this.wtContext.HCtx,
                (byte)extTagIndex,
                tabletIndex,
                controlIndex,
                (byte)functionIndex,
                (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_LOCATION,
                ref Location))
                {
                    this.log.Warning("TABLET_PROPERTY_LOCATION failed.");
                }

                if (!CWintabExtensions.ControlPropertyGet(
                this.wtContext.HCtx,
                (byte)extTagIndex,
                tabletIndex,
                controlIndex,
                (byte)functionIndex,
                (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_MIN,
                ref MinRange))
                {
                    this.log.Warning("TABLET_PROPERTY_MIN failed.");
                }

                if (!CWintabExtensions.ControlPropertyGet(
                this.wtContext.HCtx,
                (byte)extTagIndex,
                tabletIndex,
                controlIndex,
                (byte)functionIndex,
                (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_MAX,
                ref MaxRange))
                {
                    this.log.Warning("TABLET_PROPERTY_MAX failed.");
                }

                this.log.Information("OVERRIDDEN: {deviceIdx}, {extensionTagIdx}, {controlIdx}, {functionIdx}", tabletIndex, extTagIndex, controlIndex, functionIndex);
                this.log.Information("{@params}", new
                {
                    PropertyOverride,
                    Name,
                    Location,
                    MinRange,
                    MaxRange
                });
                this.HasOverrides = true;
            }
            else
            {
                this.log.Information("UNAVAILABLE: {deviceIdx}, {extensionTagIdx}, {controlIdx}, {functionIdx}", tabletIndex, extTagIndex, controlIndex, functionIndex);
            }
        }

        private void RemoveOverrides()
        {
            if (this.HasOverrides)
            {
                this.log.Information("RemoveOverrides");
                foreach (byte tabletIndex in CWintabInfo.GetFoundDevicesIndexList())
                {
                    foreach (EWTXExtensionTag extTagIndex in Enum.GetValues(typeof(EWTXExtensionTag)))
                    {
                        uint numCtrls = default;
                        uint numFuncs = default;
                        byte controlIndex = default;
                        byte functionIndex = default;

                        CWintabExtensions.ControlPropertyGet(
                        this.wtContext.HCtx,
                        (byte)extTagIndex,
                        tabletIndex,
                        controlIndex,
                        functionIndex,
                        (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_CONTROLCOUNT,
                        ref numCtrls);

                        for (controlIndex = 0; controlIndex < numCtrls; controlIndex++)
                        {
                            CWintabExtensions.ControlPropertyGet(
                            this.wtContext.HCtx,
                            (byte)extTagIndex,
                            tabletIndex,
                            controlIndex,
                            functionIndex,
                            (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_FUNCCOUNT,
                            ref numFuncs);

                            for (functionIndex = 0; functionIndex < numFuncs; ++functionIndex)
                            {
                                CWintabExtensions.ControlPropertySet(
                                this.wtContext.HCtx,
                                (byte)extTagIndex,
                                tabletIndex,
                                controlIndex,
                                functionIndex,
                                (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_OVERRIDE,
                                0);
                            }
                        }
                    }
                }
                this.HasOverrides = false;
            }
        }

        /// <summary>
        /// Called when Wintab WT_PACKET events are received.
        /// </summary>
        /// <param name="sender">The EventMessage object sending the report.</param>
        /// <param name="eventArgs">
        /// eventArgs_I.Message.WParam contains ID of packet containing the data.
        /// </param>
        private void WinTabPacketEventHandler(object sender, MessageReceivedEventArgs eventArgs)
        {
            try
            {
                this.log.Information(
                    Enum.GetName(typeof(WindowsEventMessages), eventArgs.Message.Msg)
                    ?? Enum.GetName(typeof(EWintabEventMessage), eventArgs.Message.Msg)
                    ?? Convert.ToString(eventArgs.Message.Msg));
            }
            catch (Exception ex)
            {
                this.log.Error("{ex}", ex.ToString());
            }

            if (this.wtData == null)
            {
                return;
            }
            try
            {
                switch (eventArgs.Message.Msg)
                {
                    case (int)WindowsEventMessages.WM_ACTIVATE:
                    {
                        switch ((int)eventArgs.Message.WParam)
                        {
                            case (int)WParam.WA_INACTIVE:
                            {
                                //NOTE: It is recommended that applications remove their overrides when the application's main window is no longer active (use WM_ACTIVATE). Extension overrides take effect across the entire system; if an application leaves its overrides in place, that control will not function correctly in other applications.
                                this.RemoveOverrides();
                                //When applications receive the WM_ACTIVATE message, they should push their contexts to the bottom of the overlap order if their application is being deactivated, and should bring their context to the top if they are being activated.
                                this.wtContext.SetOverlapOrder(toTop_I: false);
                                break;
                            }
                            case (int)WParam.WA_ACTIVE:
                            case (int)WParam.WA_CLICKACTIVE:
                            {
                                this.AddOverrides();
                                //When applications receive the WM_ACTIVATE message, they should push their contexts to the bottom of the overlap order if their application is being deactivated, and should bring their context to the top if they are being activated.
                                this.wtContext.SetOverlapOrder(toTop_I: true);
                                break;
                            }
                            default:
                                break;
                        }
                        break;
                    }
                    case (int)WindowsEventMessages.WM_SYSCOMMAND:
                    {
                        switch ((int)eventArgs.Message.WParam)
                        {
                            case (int)WParam.SC_MINIMIZE:
                            {
                                //Applications should also disable their contexts when they are minimized.
                                this.wtContext.Enable(enable_I: false);
                                break;
                            }
                            case (int)WParam.SC_RESTORE:
                            case (int)WParam.SC_MAXIMIZE:
                            {
                                this.wtContext.Enable(enable_I: true);
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

                    case (int)EWintabEventMessage.WT_PACKET:
                    {
                        /*
                            +-------------+--------------------------------------------------------------------------------------------------------------------+
                            | Description | The WT_PACKET message is posted to the context-owning windows that have requested messag­ing for their context.     |
                            +-------------+-----------+--------------------------------------------------------------------------------------------------------+
                            |             | Parameter | Description                                                                                            |
                            +-------------+-----------+--------------------------------------------------------------------------------------------------------+
                            |             | wParam    | Contains the serial number of the packet that generated the mesÂ­sage.                                  |
                            +-------------+-----------+--------------------------------------------------------------------------------------------------------+
                            |             | lParam    | Contains the handle of the context that processed the packet.                                          |
                            +-------------+-----------+--------------------------------------------------------------------------------------------------------+
                            | Comments    | Applications should call the WTPacket function or other packet retrieval func­tions upon receiving this message.    |
                            +-------------+--------------------------------------------------------------------------------------------------------------------+
                            | See Also    | The WTPacket function in section 6.1.1, and the advanced packet and queue func­tions in section 5.4.                |
                            +-------------+--------------------------------------------------------------------------------------------------------------------+
                        */
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
                        uint hCtx = (uint)eventArgs.Message.LParam;
                        uint pktID = (uint)eventArgs.Message.WParam;
                        var pkt = this.wtData.GetDataPacket(hCtx, pktID);
                        this.log.Information("{packet}", new
                        {
                            x = pkt.pkX,
                            y = pkt.pkY,
                            z = pkt.pkZ,
                            p_n = pkt.pkNormalPressure,
                            p_t = pkt.pkTangentPressure,
                            altitude = pkt.pkOrientation.orAltitude,
                            azimuth = pkt.pkOrientation.orAzimuth,
                            twist = pkt.pkOrientation.orTwist,
                            pitch = pkt.pkRotation.rotPitch,
                            roll = pkt.pkRotation.rotRoll,
                            yaw = pkt.pkRotation.rotYaw,
                            pkt.pkContext,
                            pkt.pkStatus,
                            pkt.pkTime,
                            pkt.pkChanged,
                            pkt.pkSerialNumber,
                            pkt.pkCursor,
                            pkt.pkButtons,
                        });
                        break;
                    }
                    case (int)EWintabEventMessage.WT_PACKETEXT:
                    {
                        uint hCtx = (uint)eventArgs.Message.LParam;
                        uint pktID = (uint)eventArgs.Message.WParam;
                        this.log.Information("GetDataPacketExt({@context}, {@packetId})", hCtx, pktID);
                        var pktExt = this.wtData.GetDataPacketExt(hCtx, pktID);
                        this.log.Information("{packetExtension}", new
                        {
                            BaseContext = pktExt.pkBase.nContext,
                            BaseSerialNumber = pktExt.pkBase.nSerialNumber,
                            BaseStatus = pktExt.pkBase.nStatus,
                            BaseTime = pktExt.pkBase.nTime,
                            ExpressKeyControl = pktExt.pkExpKey.nControl,
                            ExpressKeyLocation = pktExt.pkExpKey.nLocation,
                            ExpressKeyReserved = pktExt.pkExpKey.nReserved,
                            ExpressKeyState = pktExt.pkExpKey.nState,
                            ExpressKeyTablet = pktExt.pkExpKey.nTablet,
                            TouchRingControl = pktExt.pkTouchRing.nControl,
                            TouchRingMode = pktExt.pkTouchRing.nMode,
                            TouchRingPosition = pktExt.pkTouchRing.nPosition,
                            TouchRingReserver = pktExt.pkTouchRing.nReserved,
                            TouchRingTablet = pktExt.pkTouchRing.nTablet,
                            TouchStripControl = pktExt.pkTouchStrip.nControl,
                            TouchStripMode = pktExt.pkTouchStrip.nMode,
                            TouchStripPosition = pktExt.pkTouchStrip.nPosition,
                            TouchStripReserved = pktExt.pkTouchStrip.nReserved,
                            TouchStripTablet = pktExt.pkTouchStrip.nTablet
                        });
                        break;
                    }
                    case (int)EWintabEventMessage.WT_CTXOPEN:
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
                    case (int)EWintabEventMessage.WT_CTXCLOSE:
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
                    case (int)EWintabEventMessage.WT_CTXUPDATE:
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
                    case (int)EWintabEventMessage.WT_CTXOVERLAP:
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
                    case (int)EWintabEventMessage.WT_PROXIMITY:
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
                    case (int)EWintabEventMessage.WT_INFOCHANGE:
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
                    case (int)EWintabEventMessage.WT_CSRCHANGE:
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
            }
            catch (Exception ex)
            {
                this.log.Error("Exception: {@ex}", ex);
            }
        }
    }
}