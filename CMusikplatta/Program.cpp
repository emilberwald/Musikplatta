#include "Program.h"

#include "Common.h"
#include "L1ApiWintab.h"
#include "L2ApiWintab.h"
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
				spdlog::info("pkButtons {} "
							 "pkChanged {} "
							 "pkContext {} "
							 "pkCursor {} "
							 "pkNormalPressure {} "
							 "pkOrientation.orAltitude {} "
							 "pkOrientation.orAzimuth {} "
							 "pkOrientation.orTwist {} "
							 "pkRotation.roPitch {} "
							 "pkRotation.roRoll {} "
							 "pkRotation.roYaw {} "
							 "pkStatus {} "
							 "pkTangentPressure {} "
							 "pkTime {} "
							 "pkX {} "
							 "pkY {} "
							 "pkZ {} ",
							 packet.pkButtons,
							 packet.pkChanged,
							 (void*)packet.pkContext,
							 packet.pkCursor,
							 packet.pkNormalPressure,
							 packet.pkOrientation.orAltitude,
							 packet.pkOrientation.orAzimuth,
							 packet.pkOrientation.orTwist,
							 packet.pkRotation.roPitch,
							 packet.pkRotation.roRoll,
							 packet.pkRotation.roYaw,
							 packet.pkStatus,
							 packet.pkTangentPressure,
							 packet.pkTime,
							 packet.pkX,
							 packet.pkY,
							 packet.pkZ);
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
