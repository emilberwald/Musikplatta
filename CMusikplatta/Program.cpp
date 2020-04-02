#include "Program.h"

#include "Common.h"
#include "L1ApiWintab.h"
#include "L2ApiWintab.h"
#include "Midi.h"
#include "MidiOut.h"
#include "explain_wm.h"
#include "explain_wt.h"

#include <any>
#include <stdexcept>

namespace mp
{
Program::Program(): window_id(nullptr), context_descriptor(new LOGCONTEXTA{}), extensions()
{
	this->context_descriptor			= AddExtensions(AddDefaultDigitizingContext(this->context_descriptor));
	this->wintab_device_capabilities	= GetWintabDeviceCapabilities();
	this->wintab_cursor_capabilities	= GetWintabCursorCapabilities();
	this->wintab_extension_capabilities = GetWintabExtensionCapabilities();
	MidiOut::note_on_burn_in = MidiOut::second_t{ 1.0 / this->wintab_device_capabilities.max_packet_report_rate_hz };
}

Program::~Program() {}

void Program::HandleMessage(HWND windowId, UINT messageType, WPARAM wparam, LPARAM lparam)
{
	if(this->context == nullptr)
	{
		spdlog::info("OpenWintabContext");
		this->context = mp::OpenWintabContext(windowId, this->context_descriptor, true);
	}
	if(this->context)
	{
		switch(messageType)
		{
			case WM_KEYDOWN:
			{
				switch(wparam)
				{
					case VK_UP:
					{
						this->current_instrument = static_cast<Midi::Timbre>(
							std::clamp<int>(static_cast<int>(this->current_instrument) + 1, 0, 127));
						spdlog::info("VK_UP: {}", (int)this->current_instrument);
						break;
					}
					case VK_DOWN:
					{
						this->current_instrument = static_cast<Midi::Timbre>(
							std::clamp<int>(static_cast<int>(this->current_instrument) - 1, 0, 127));
						spdlog::info("VK_DOWN: {}", (int)this->current_instrument);
						break;
					}
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
					{
						this->current_channel = wparam;
						this->midi_out.Set(this->current_instrument, this->current_channel);
						break;
					}
					case 'A':
					{
						this->midi_out.Play(this->current_channel, Midi::Key::A4, 100);
						std::this_thread::sleep_for(MidiOut::note_on_burn_in);
						this->midi_out.Play(this->current_channel, Midi::Key::A4, 100);
						break;
					}
					default: break;
				}
				break;
			}
			case WM_KEYUP:
			{
				switch(wparam)
				{
					case 'A':
					{
						this->midi_out.Play(this->current_channel, Midi::Key::A4, 0);
						break;
					}
					default: break;
				}
				break;
			}
			case WM_ACTIVATE:
			{
				switch(wparam)
				{
					case WA_INACTIVE:
					{
						spdlog::info("{} => remove overrides, set overlap order to bottom.",
									 std::get<0>(explain_wm[messageType]));
						// NOTE: It is recommended that applications remove their overrides when the application's main
						// window is no longer active (use WM_ACTIVATE). Extension overrides take effect across the entire
						// system; if an application leaves its overrides in place, that control will not function correctly
						// in other applications.
						RemoveAllOverrides(this->context);
						this->extensions.clear();
						// When applications receive the WM_ACTIVATE message, they should push their contexts to the bottom
						// of the overlap order if their application is being deactivated, and should bring their context to
						// the top if they are being activated.
						MP_THROW_IF(WTOverlap(this->context, FALSE) == 0, mp::wintab_exception)
						break;
					}
					case WA_ACTIVE:
					case WA_CLICKACTIVE:
					{
						spdlog::info("{} => add overrides, set overlap order to top.",
									 std::get<0>(explain_wm[messageType]));
						auto allOverrides = AddAllOverrides(this->context);
						this->extensions.insert(this->extensions.begin(), allOverrides.begin(), allOverrides.end());
						// When applications receive the WM_ACTIVATE message, they should push their contexts to the bottom
						// of the overlap order if their application is being deactivated, and should bring their context to
						// the top if they are being activated.
						MP_THROW_IF(WTOverlap(this->context, TRUE) == 0, mp::wintab_exception)
						break;
					}
					default: break;
				}
				break;
			}
			case WM_SYSCOMMAND:
			{
				switch(wparam)
				{
					case SC_MINIMIZE:
					{
						// Applications should also disable their contexts when they are minimized.
						spdlog::info("{} => disable context.", std::get<0>(explain_wm[messageType]));
						MP_THROW_IF(WTEnable(this->context, FALSE) == 0, mp::wintab_exception)
						break;
					}
					case SC_RESTORE:
					case SC_MAXIMIZE:
					{
						spdlog::info("{} => enable context.", std::get<0>(explain_wm[messageType]));
						MP_THROW_IF(WTEnable(this->context, TRUE) == 0, mp::wintab_exception)
						break;
					}
					default: break;
				}
				break;
			}
			case WM_LBUTTONDOWN:
			{
				spdlog::info("{} => stylus button down.", std::get<0>(explain_wm[messageType]));
				break;
			}
			case WM_LBUTTONUP:
			{
				spdlog::info("{} => stylus button up.", std::get<0>(explain_wm[messageType]));
				// stylus button
				break;
			}
			case WT_PACKET:
			{
				/*
				The interface defines a standard orientation for reporting device native coordinates.
				When the user is viewing the device in its normal position, the coordinate origin will be at the lower
			   left of the device. The coordinate system will be right-handed, that is, the positive x axis points from
			   left to right, and the posi­tive y axis points either upward or away from the user. The z axis, if
			   supported, points either to­ward the user or upward. For devices that lay flat on a table top, the x-y
			   plane will be horizontal and the z axis will point upward. For devices that are oriented vertically (for
			   example, a touch screen on a conventional dis­play), the x-y plane will be vertical, and the z axis will
			   point toward the user.
			*/
				auto packet = GetDataPacket(this->context, wparam);

				auto normal_pressure = double(packet.pkNormalPressure)
									   / (double(this->wintab_device_capabilities.normal_pressure.axMax)
										  - double(this->wintab_device_capabilities.normal_pressure.axMin));
				auto x = double(packet.pkX)
						 / (double(this->wintab_device_capabilities.position[0].axMax)
							- double(this->wintab_device_capabilities.position[0].axMin));
				auto y = double(packet.pkY)
						 / (double(this->wintab_device_capabilities.position[1].axMax)
							- double(this->wintab_device_capabilities.position[1].axMin));
				auto z = double(packet.pkZ)
						 / (double(this->wintab_device_capabilities.position[2].axMax)
							- double(this->wintab_device_capabilities.position[2].axMin));
				auto azimuth = double(packet.pkOrientation.orAzimuth)
							   / (double(this->wintab_device_capabilities.orientation[0].axMax)
								  - double(this->wintab_device_capabilities.orientation[0].axMin));
				auto elevation = double(packet.pkOrientation.orAltitude)
								 / (double(this->wintab_device_capabilities.orientation[1].axMax)
									- double(this->wintab_device_capabilities.orientation[1].axMin));
				this->midi_out.Play(
					this->current_channel,
					static_cast<Midi::Key>(std::clamp<int>(static_cast<int>(127 * x), 0, 127)),
					std::clamp<uint8_t>((normal_pressure > 0 ? std::clamp<uint8_t>(127 * normal_pressure, 1, 127) : 0),
										0,
										127));

				spdlog::debug("({}:{}) ({:03.5f}) ({:03.5f},{:03.5f},{:03.5f}) ({:03.5f},{:03.5f})",
							  GetName(GetButtonState(packet)),
							  GetButtonNumber(packet),
							  normal_pressure,
							  x,
							  y,
							  z,
							  azimuth,
							  elevation);
				spdlog::debug("pkButtons:State {} "
							  "pkButtons:Number {} "
							  "pkChanged {} "
							  "pkContext {} "
							  "pkCursor {} "
							  "pkNormalPressure {:03.3f} "
							  "pkOrientation.orAzimuth {:03.3f} "
							  "pkOrientation.orAltitude {:03.3f} "
							  "pkOrientation.orTwist {:03.3f} "
							  "pkRotation.roPitch {:03.3f} "
							  "pkRotation.roRoll {:03.3f} "
							  "pkRotation.roYaw {:03.3f} "
							  "pkStatus {} "
							  "pkTangentPressure {:03.3f} "
							  "pkTime {} "
							  "pkX {:03.3f} "
							  "pkY {:03.3f} "
							  "pkZ {:03.3f} ",
							  GetName(GetButtonState(packet)),
							  GetButtonNumber(packet),
							  packet.pkChanged,
							  (void*)packet.pkContext,
							  packet.pkCursor,
							  double(packet.pkNormalPressure)
								  / (double(this->wintab_device_capabilities.normal_pressure.axMax)
									 - double(this->wintab_device_capabilities.normal_pressure.axMin)),
							  double(packet.pkOrientation.orAzimuth)
								  / (double(this->wintab_device_capabilities.orientation[0].axMax)
									 - double(this->wintab_device_capabilities.orientation[0].axMin)),
							  double(packet.pkOrientation.orAltitude)
								  / (double(this->wintab_device_capabilities.orientation[1].axMax)
									 - double(this->wintab_device_capabilities.orientation[1].axMin)),
							  double(packet.pkOrientation.orTwist)
								  / (double(this->wintab_device_capabilities.orientation[2].axMax)
									 - double(this->wintab_device_capabilities.orientation[2].axMin)),
							  double(packet.pkRotation.roPitch)
								  / (double(this->wintab_device_capabilities.rotation[0].axMax)
									 - double(this->wintab_device_capabilities.rotation[0].axMin)),
							  double(packet.pkRotation.roRoll)
								  / (double(this->wintab_device_capabilities.rotation[1].axMax)
									 - double(this->wintab_device_capabilities.rotation[1].axMin)),
							  double(packet.pkRotation.roYaw)
								  / (double(this->wintab_device_capabilities.rotation[2].axMax)
									 - double(this->wintab_device_capabilities.rotation[2].axMin)),
							  packet.pkStatus,
							  double(packet.pkTangentPressure)
								  / (double(this->wintab_device_capabilities.tangential_pressure.axMax)
									 - double(this->wintab_device_capabilities.tangential_pressure.axMin)),
							  packet.pkTime,
							  double(packet.pkX)
								  / (double(this->wintab_device_capabilities.position[0].axMax)
									 - double(this->wintab_device_capabilities.position[0].axMin)),
							  double(packet.pkY)
								  / (double(this->wintab_device_capabilities.position[1].axMax)
									 - double(this->wintab_device_capabilities.position[1].axMin)),
							  double(packet.pkZ)
								  / (double(this->wintab_device_capabilities.position[2].axMax)
									 - double(this->wintab_device_capabilities.position[2].axMin)));
				break;
			}
			case WT_PACKETEXT:
			{
				auto packet = GetDataPacketExt(this->context, wparam);
				spdlog::info("pkBase.nContext {}"
							 "pkBase.nSerialNumber {} "
							 "pkBase.nStatus {} "
							 "pkBase.nTime {} "
							 "pkExpKeys.nControl {} "
							 "pkExpKeys.nLocation {} "
							 "pkExpKeys.nReserved {} "
							 "pkExpKeys.nState {} "
							 "pkExpKeys.nTablet {} "
							 "pkTouchRing.nControl {} "
							 "pkTouchRing.nMode {} "
							 "pkTouchRing.nPosition {} "
							 "pkTouchRing.nReserved {} "
							 "pkTouchRing.nTablet {} "
							 "pkTouchStrip.nControl {} "
							 "pkTouchStrip.nMode {} "
							 "pkTouchStrip.nPosition {} "
							 "pkTouchStrip.nReserved {} "
							 "pkTouchStrip.nTablet {} ",
							 (void*)packet.pkBase.nContext,
							 packet.pkBase.nSerialNumber,
							 packet.pkBase.nStatus,
							 packet.pkBase.nTime,
							 packet.pkExpKeys.nControl,
							 packet.pkExpKeys.nLocation,
							 packet.pkExpKeys.nReserved,
							 packet.pkExpKeys.nState,
							 packet.pkExpKeys.nTablet,
							 packet.pkTouchRing.nControl,
							 packet.pkTouchRing.nMode,
							 packet.pkTouchRing.nPosition,
							 packet.pkTouchRing.nReserved,
							 packet.pkTouchRing.nTablet,
							 packet.pkTouchStrip.nControl,
							 packet.pkTouchStrip.nMode,
							 packet.pkTouchStrip.nPosition,
							 packet.pkTouchStrip.nReserved,
							 packet.pkTouchStrip.nTablet);
				break;
			}
			case WT_CTXOPEN:
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
			case WT_CTXCLOSE:
			{
				/*
				+-------------+-------------------------------------------------------------------------------------------------------------------------+
				| Description | The WT_CTXCLOSE message is sent to the owning win­dow and to any manager windows when a
			   context is about to be closed.  |
				+-------------+-----------+-------------------------------------------------------------------------------------------------------------+
				|             | Parameter | Description |
				+-------------+-----------+-------------------------------------------------------------------------------------------------------------+
				|             | wParam    | Contains the context handle of the context to be closed. |
				+-------------+-----------+-------------------------------------------------------------------------------------------------------------+
				|             | lParam    | Contains the current context status flags. |
				+-------------+-----------+-------------------------------------------------------------------------------------------------------------+
				| Comments    | Tablet manager applications should note that the context is being closed and take
			   appropriate action.                   |
				+-------------+-------------------------------------------------------------------------------------------------------------------------+
			*/
				break;
			}
			case WT_CTXUPDATE:
			{
				/*
				+-------------+-----------------------------------------------------------------------------------------------------------------------------+
				| Description | The WT_CTXUPDATE message is sent to the owning window and to any manager windows when a
			   context is changed.                 |
				+-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
				|             | Parameter | Description |
				+-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
				|             | wParam    | Contains the context handle of the changed context. |
				+-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
				|             | lParam    | Contains the current context status flags. |
				+-------------+-----------+-----------------------------------------------------------------------------------------------------------------+
				| Comments    | Applications may want to call WTGet or WTExtGet on receiving this message to find out
			   what context attributes were changed. |
				+-------------+-----------------------------------------------------------------------------------------------------------------------------+
			*/
				break;
			}
			case WT_CTXOVERLAP:
			{
				/*
				+-------------+-----------------------------------------------------------------------------------------------------------------------------------+
				| Description | The WT_CTXOVERLAP message is sent to the owning window and to any manager windows when a
			   context is moved in the overlap order.   |
				+-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
				|             | Parameter | Description |
				+-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
				|             | wParam    | Contains the context handle of the re-overlapped context. |
				+-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
				|             | lParam    | Contains the current context status flags. |
				+-------------+-----------+-----------------------------------------------------------------------------------------------------------------------+
				| Comments    | Tablet managers can handle this message to keep track of context overlap reÂ­quests by
			   applications.                               |
				+-------------+-----------------------------------------------------------------------------------------------------------------------------------+
				|             | Applications can handle this message to find out when their context is obscured by
			   another context.                               |
				+-------------+-----------------------------------------------------------------------------------------------------------------------------------+
			*/
				break;
			}
			case WT_PROXIMITY:
			{
				/*
				+-------------+---------------------------------------------------------------------------------------------------------------------------------------+
				| Description | The WT_PROXIMITY message is posted to the owning window and any manager windows when the
			   cursor enters or leaves context proximity.   |
				+-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
				|             | Parameter | Description |
				+-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
				|             | wParam    | Contains the handle of the context that the cursor is entering or leavÂ­ing.
			   |
				+-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
				|             | lParam    | The low-order word is non-zero when the cursor is entering the context and
			   zero when it is leaving the context.           | |             |           | The high-order word is
			   non-zero when the cursor is leaving or entering hardÂ­ware proximity.                               |
				+-------------+-----------+---------------------------------------------------------------------------------------------------------------------------+
				| Comments    | Proximity events are handled separately from regular tablet events. | |             |
			   Applications will receive proximity messages even if they haven't requested event messages. |
				+-------------+---------------------------------------------------------------------------------------------------------------------------------------+
			*/
				break;
			}
			case WT_INFOCHANGE:
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
				break;
			}
			case WT_CSRCHANGE:
			{
				/*
				+-------------+-------------------------------------------------------------------------------------------------+
				| Description | The WT_CSRCHANGE message is posted to the owning window when a new cursor enters the
			   context.   |
				+-------------+-----------+-------------------------------------------------------------------------------------+
				|             | Parameter | Description |
				+-------------+-----------+-------------------------------------------------------------------------------------+
				|             | wParam    | Contains the serial number of the packet that generated the mesÂ­sage. |
				+-------------+-----------+-------------------------------------------------------------------------------------+
				|             | lParam    | Contains the handle of the context that processed the packet. |
				+-------------+-----------+-------------------------------------------------------------------------------------+
				| Comments    | Only contexts that have the CXO_CSRMESSAGES option selected will generate this message.
			   |
				+-------------+-------------------------------------------------------------------------------------------------+
			*/
				break;
			}
			default: return;
		}
	}
}
} // namespace mp
