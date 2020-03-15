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
        private ILogger log;
        private List<dynamic> overriden = new List<dynamic>();
        private List<byte> tabletIndices;
        private CWintabContext wtContext;
        private CWintabData wtData;

        public Pen(ILogger log)
        {
            this.log = log;
            this.wtContext = CWintabInfo.GetDefaultDigitizingContext(ECTXOptionValues.CXO_MESSAGES);
            this.wtContext.PktData |= Math.Clamp(CWintabExtensions.GetWTExtensionMask(EWTXExtensionTag.WTX_EXPKEYS2), 0, byte.MaxValue);
            this.wtContext.PktData |= Math.Clamp(CWintabExtensions.GetWTExtensionMask(EWTXExtensionTag.WTX_TOUCHRING), 0, byte.MaxValue);
            this.wtContext.PktData |= Math.Clamp(CWintabExtensions.GetWTExtensionMask(EWTXExtensionTag.WTX_TOUCHSTRIP), 0, byte.MaxValue);
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

            this.tabletIndices = CWintabInfo.GetFoundDevicesIndexList();
            foreach (var tabletIndex in this.tabletIndices)
            {
                foreach (EWTXExtensionTag extTagIndex in Enum.GetValues(typeof(EWTXExtensionTag)))
                {
                    UInt32 numCtrls = default;
                    UInt32 numFuncs = default;
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

                        for (functionIndex = 0; functionIndex < numFuncs; functionIndex++)
                        {
                            var overriden = this.Override(tabletIndex, extTagIndex, functionIndex, controlIndex);
                            if (overriden != null)
                            {
                                this.log.Information("{@overriden}", overriden);
                                this.overriden.Add(overriden);
                            }
                        }
                    }
                }
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~Pen()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(false);
        }

        // To detect redundant calls
        // This code added to correctly implement the disposable pattern.
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
                    this.tabletIndices = CWintabInfo.GetFoundDevicesIndexList();
                    foreach (var tabletIndex in this.tabletIndices)
                    {
                        foreach (var extTagIndex in Enum.GetValues(typeof(EWTXExtensionTag)))
                        {
                            UInt32 numCtrls = default;
                            UInt32 numFuncs = default;
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
                    this.wtContext.Close();
                }
                this.disposedValue = true;
            }
        }

        private static string ControlName(EWTXExtensionTag extTagIndex_I, byte controlIndex, byte functionIndex) => extTagIndex_I switch
        {
            EWTXExtensionTag.WTX_EXPKEYS2 => "EK: " + Convert.ToString(controlIndex),
            EWTXExtensionTag.WTX_TOUCHRING => "TR: " + Convert.ToString(functionIndex),
            EWTXExtensionTag.WTX_TOUCHSTRIP => "TS: " + Convert.ToString(functionIndex),
            _ => "UK: " + Convert.ToString(functionIndex),
        };

        private dynamic Override(byte tabletIndex, EWTXExtensionTag extTagIndex, byte functionIndex, byte controlIndex)
        {
            uint propOverride = 1;  // true
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
            functionIndex,
            (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_AVAILABLE,
            ref Available);

            if (Available > 0)
            {
                CWintabExtensions.ControlPropertySet(
                this.wtContext.HCtx,
                (byte)extTagIndex,
                tabletIndex,
                controlIndex,
                functionIndex,
                (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_OVERRIDE,
                propOverride);

                CWintabExtensions.ControlPropertySet(
                this.wtContext.HCtx,
                (byte)extTagIndex,
                tabletIndex,
                controlIndex,
                functionIndex,
                (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_OVERRIDE_NAME,
                Name);

                CWintabExtensions.ControlPropertyGet(
                this.wtContext.HCtx,
                (byte)extTagIndex,
                tabletIndex,
                controlIndex,
                functionIndex,
                (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_LOCATION,
                ref Location);

                CWintabExtensions.ControlPropertyGet(
                this.wtContext.HCtx,
                (byte)extTagIndex,
                tabletIndex,
                controlIndex,
                functionIndex,
                (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_MIN,
                ref MinRange);

                CWintabExtensions.ControlPropertyGet(
                this.wtContext.HCtx,
                (byte)extTagIndex,
                tabletIndex,
                controlIndex,
                functionIndex,
                (ushort)EWTExtensionTabletProperty.TABLET_PROPERTY_MAX,
                ref MaxRange);

                return new
                {
                    Name,
                    Available,
                    Location,
                    MinRange,
                    MaxRange,
                };
            }
            return null;
        }

        /// <summary>
        /// Called when Wintab WT_PACKET events are received.
        /// </summary>
        /// <param name="sender_I">The EventMessage object sending the report.</param>
        /// <param name="eventArgs_I">eventArgs_I.Message.WParam contains ID of packet containing the data.</param>
        private void WinTabPacketEventHandler(object sender_I, MessageReceivedEventArgs eventArgs_I)
        {
            //System.Diagnostics.Debug.WriteLine("Received WT_PACKET event");
            if (this.wtData == null)
            {
                return;
            }

            try
            {
                switch ((EWintabEventMessage)eventArgs_I.Message.Msg)
                {
                    case EWintabEventMessage.WT_PACKET:
                    {
                        uint hCtx = (uint)eventArgs_I.Message.LParam;
                        uint pktID = (uint)eventArgs_I.Message.WParam;
                        var pkt = this.wtData.GetDataPacket(hCtx, pktID);
                        this.log.Information("{packet}", new
                        {
                            x = pkt.pkX,
                            y = pkt.pkY,
                            z = pkt.pkZ,
                            p = pkt.pkNormalPressure,
                            h = pkt.pkOrientation.orAltitude,
                            azimuth = pkt.pkOrientation.orAzimuth,
                            twist = pkt.pkOrientation.orTwist,
                            wheel = pkt.pkTangentPressure,
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
                    case EWintabEventMessage.WT_PACKETEXT:
                    {
                        uint hCtx = (uint)eventArgs_I.Message.LParam;
                        uint pktID = (uint)eventArgs_I.Message.WParam;
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
                    case EWintabEventMessage.WT_CTXOPEN:
                    case EWintabEventMessage.WT_CTXCLOSE:
                    case EWintabEventMessage.WT_CTXUPDATE:
                    case EWintabEventMessage.WT_CTXOVERLAP:
                    case EWintabEventMessage.WT_PROXIMITY:
                    case EWintabEventMessage.WT_INFOCHANGE:
                    case EWintabEventMessage.WT_CSRCHANGE:
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