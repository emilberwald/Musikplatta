#pragma once
#include "Common.h"
#include "L0ApiWintab.h"
#include "L1ApiWintab.h"

#include <array>
#include <stdexcept>
#include <vector>

namespace mp
{
struct wintab_exception: std::runtime_error
{
	using std::runtime_error::runtime_error;
};

enum TabletButtonState : WORD
{
	no_change		= TBN_NONE,
	button_released = TBN_UP,
	button_pressed	= TBN_DOWN,
};

struct Extension
{
	unsigned int tablet_index;
	unsigned int extension_tag_index;
	unsigned int function_index;
	unsigned int control_index;
	std::string	 name;
	uint32_t	 location;
	uint32_t	 min_range;
	uint32_t	 max_range;
};

std::shared_ptr<LOGCONTEXTA> AddDefaultDigitizingContext(std::shared_ptr<LOGCONTEXTA> context);

std::shared_ptr<LOGCONTEXTA> AddExtensions(std::shared_ptr<LOGCONTEXTA> context);

HCTX OpenWintabContext(HWND windowId, std::shared_ptr<LOGCONTEXTA> contextDescriptor, bool openEnabled);

const char* ExtensionTagName(unsigned int extTagIndex);

const char* ExtensionTagDescription(unsigned int extTagIndex);

unsigned int GetNumberOfDevices(HCTX context);

std::vector<Extension> Override(HCTX		 context,
								unsigned int tabletIndex,
								unsigned int extTagIndex,
								unsigned int functionIndex,
								unsigned int controlIndex);

std::vector<Extension> AddOverrides(HCTX					  context,
									unsigned int			  tabletIndex,
									std::vector<unsigned int> extensionTags
									= { WTX_TOUCHSTRIP, WTX_TOUCHRING, WTX_EXPKEYS2 });

std::vector<Extension> AddAllOverrides(HCTX context);
void				   RemoveAllOverrides(HCTX						context,
										  std::vector<unsigned int> extensionTags = { WTX_TOUCHSTRIP, WTX_TOUCHRING, WTX_EXPKEYS2 });

enum HardwareCapabilities
{
	hwc_integrated	   = HWC_INTEGRATED,
	hwc_touch		   = HWC_TOUCH,
	hwc_hardprox	   = HWC_HARDPROX,
	hwc_physid_cursors = HWC_PHYSID_CURSORS,
};

struct WintabDeviceCapabilities
{
	AXIS				 normal_pressure;
	AXIS				 orientation[3];
	AXIS				 position[3];
	AXIS				 rotation[3];
	AXIS				 tangential_pressure;
	HardwareCapabilities hardware_capabilities;
	int					 margin[3];
	std::string			 plug_and_play_id;
	std::string			 name;
	UINT				 first_cursor_type_number;
	UINT				 max_packet_report_rate_hz;
	UINT				 nof_supported_cursor_types;
	WTPKT				 supported_packet_data_bitmask;
	WTPKT				 supported_packet_data_bitmask_certain_cursors;
	WTPKT				 supported_packet_data_bitmask_relative_change;
};

enum CursorCapabilityFlag : UINT
{
	crc_multimode
	= CRC_MULTIMODE, // CRC_MULTIMODE Indicates this cursor type describes one of several modes of a single physical cursor. Consecutive cursor type categories de­scribe the modes; the CSR_MODE data item gives the mode number of each cur­sor type.
	crc_aggregate
	= CRC_AGGREGATE, // CRC_AGGREGATE Indicates this cursor type describes sev­eral physical cursors that cannot be dis­tinguished by software.
	crc_invert
	= CRC_INVERT, // CRC_INVERT Indicates this cursor type describes the physical cursor in its inverted orienta­tion; the previous consecutive cursor type category describes the normal orienta­tion.
};
DEFINE_ENUM_FLAG_OPERATORS(CursorCapabilityFlag);

struct WintabCursorCapabilities
{
	// CSR_NAME TCHAR[]  Returns a displayable zero-terminated string containing the name of the cursor.
	std::string name;

	// CSR_ACTIVE BOOL  Returns whether the cursor is currently connected.
	bool active;

	// CSR_PKTDATA WTPKT  Returns a bit mask indicating the packet data items sup­ported when this cursor is connected.
	WTPKT supported_packet_data_bitmask;

	// CSR_BUTTONS BYTE  Returns the number of buttons on this cursor.
	UINT nof_buttons;

	// CSR_BUTTONBITS BYTE  Returns the number of bits of raw button data returned by the hardware.
	UINT nof_button_bits;

	// CSR_BTNNAMES TCHAR[]  Returns a list of zero-terminated strings containing the names of the cursor's buttons. The number of names in the list is the same as the number of buttons on the cursor. The names are sepa­rated by a single zero character; the list is terminated by two zero characters.
	std::vector<std::string> button_names;

	// CSR_BUTTONMAP BYTE[]  Returns a 32 byte array of logical button numbers, one for each physical button.
	BYTE logical_button_ids[32];

	// CSR_SYSBTNMAP BYTE[]  Returns a 32 byte array of button action codes, one for each logical button.
	BYTE logical_button_ids_action_code[32];

	// CSR_NPBUTTON BYTE  Returns the physical button number of the button that is con­trolled by normal pressure.
	UINT physical_button_id_normal_pressure;

	// CSR_NPBTNMARKS UINT[]  Returns an array of two UINTs, specifying the button marks for the normal pressure button. The first UINT contains the release mark; the second contains the press mark.
	UINT normal_pressure_button_marks[2];

	// CSR_NPRESPONSE UINT[]  Returns an array of UINTs describing the pressure response curve for normal pressure.
	std::vector<UINT> normal_pressure_response_curve;

	// CSR_TPBUTTON BYTE  Returns the physical button number of the button that is con­trolled by tangential pressure.
	UINT physical_button_id_tangential_pressure;

	// CSR_TPBTNMARKS UINT[]  Returns an array of two UINTs, specifying the button marks for the tangential pressure button. The first UINT contains the release mark; the second contains the press mark.
	UINT tangential_pressure_button_marks[2];

	// CSR_TPRESPONSE UINT[]  Returns an array of UINTs describing the pressure response curve for tangential pressure.
	std::vector<UINT> tangential_pressure_response_curve;

	// CSR_PHYSID (1.1) DWORD  Returns a manufacturer-specific physical identifier for the cursor. This value will distinguish the physical cursor from others on the same device. This physical identifier allows applications to bind func­tions to specific physical cursors, even if category numbers change and multiple, otherwise identical, physical cursors are present.
	DWORD physical_cursor_id;

	// CSR_MODE (1.1) UINT  Returns the cursor mode number of this cursor type, if this cursor type has the CRC_MULTIMODE capability.
	UINT cursor_mode;

	// CSR_MINPKTDATA (1.1) UINT  Returns the minimum set of data available from a physical cursor in this cursor type, if this cursor type has the CRC_AGGREGATE ca­pability.
	UINT physical_cursor_minimum_packet_data;

	// CSR_MINBUTTONS (1.1) UINT  Returns the minimum number of buttons of physical cursors in the cursor type, if this cursor type has the CRC_AGGREGATE capabil­ity.
	UINT physical_cursor_minimum_nof_buttons;

	CursorCapabilityFlag cursor_capability_flags;
};

struct WintabExtensionCapabilities
{
	//EXT_NAME TCHAR[]  Returns a unique, null-terminated string describing the ex­tension.
	std::string name;
	//EXT_TAG UINT  Returns a unique identifier for the extension.
	UINT extension_tag_id;
	//EXT_MASK WTPKT  Returns a mask that can be bitwise OR'ed with WTPKT-type vari­ables to select the extension.
	WTPKT extension_mask;
	// EXT_SIZE UINT[]  Returns an array of two UINTs specifying the extension's size within a packet (in bytes). The first is for absolute mode; the second is for relative mode.
	UINT extension_size[2];
	//EXT_AXES AXIS[]  Returns an array of axis descriptions, as needed for the exten­sion.
	std::vector<AXIS> extension_axes;
	//EXT_DEFAULT BYTE[]  Returns the current global default data, as needed for the ex­tension. This data is modified via the WTMgrExt function.
	std::vector<BYTE> current_global_default_data;
	//EXT_DEFCONTEXT BYTE[]  Each returns the current default context-specific data, as needed for the extension. The indices identify the digitizing- and sys­tem-context defaults, respectively.
	std::vector<BYTE> default_digitizing_context;
	//EXT_DEFSYSCTX BYTE[]  Each returns the current default context-specific data, as needed for the extension. The indices identify the digitizing- and sys­tem-context defaults, respectively.
	std::vector<BYTE> default_system_context;
	// EXT_CURSORS BYTE[]  Is the first of one or more consecutive indices, one per cursor type. Each returns the current default cursor-specific data, as need for the extension. This data is modified via the WTMgrCsrExt function.
	std::vector<BYTE> extension_cursors;
};

WintabCursorCapabilities GetWintabCursorCapabilities();

WintabExtensionCapabilities GetWintabExtensionCapabilities();

WintabDeviceCapabilities GetWintabDeviceCapabilities();

std::string PropertyIdName(int propertyId);

template<class T>
inline T ControlPropertyGet(HCTX		 handle,
							unsigned int tabletIndex,
							unsigned int extTagIndex,
							unsigned int controlIndex,
							unsigned int functionIndex,
							int			 propertyId)
{
	// Does not seem to matter if one allocates more than neeed. ( somtimes it does not work anyway ;D )
	std::unique_ptr<uint8_t> buffer(new uint8_t[sizeof(EXTPROPERTY) + sizeof(T)]);

	*reinterpret_cast<EXTPROPERTY*>(buffer.get()) = { (unsigned char)0,
													  (unsigned char)tabletIndex,
													  (unsigned char)controlIndex,
													  (unsigned char)functionIndex,
													  (unsigned short)propertyId,
													  (unsigned short)0,
													  (unsigned long)sizeof(T),
													  (unsigned char)0 };

	MP_THROW_IF_WHAT(WTExtGet(handle, extTagIndex, buffer.get()) == 0, mp::wintab_exception, PropertyIdName(propertyId))

	T control_property{};
	std::memcpy(&control_property, &(reinterpret_cast<EXTPROPERTY*>(buffer.get())->data[0]), sizeof(T));

	return control_property;
}

void ControlPropertySet(HCTX		 handle,
						unsigned int tabletIndex,
						unsigned int extTagIndex,
						unsigned int controlIndex,
						unsigned int functionIndex,
						int			 propertyId,
						const void*	 value_p,
						size_t		 value_s);

std::tuple<unsigned int, unsigned int> GetIconSize(HCTX			handle,
												   unsigned int tabletIndex,
												   unsigned int extTagIndex,
												   unsigned int functionIndex,
												   unsigned int controlIndex);

PACKETEXT GetDataPacketExt(HCTX context, WPARAM wparam);

PACKET GetDataPacket(HCTX context, WPARAM wparam);

TabletButtonState GetButtonState(PACKET const& packet);

} // namespace mp
