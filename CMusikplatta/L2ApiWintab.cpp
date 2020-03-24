#include "L2ApiWintab.h"

#include "Common.h"
#include "L0ApiWintab.h"
#include "L1ApiWintab.h"

#include <fmt/format.h>

namespace mp
{
std::shared_ptr<LOGCONTEXTA> AddDefaultDigitizingContext(std::shared_ptr<LOGCONTEXTA> context)
{
	spdlog::info("AddDefaultDigitizingContext");
	auto size = WTInfoA(WTI_DEFCONTEXT, 0, nullptr);

	MP_THROW_IF(size != sizeof(LOGCONTEXTA), mp::wintab_exception);
	size = WTInfoA(WTI_DEFCONTEXT, 0, context.get());
	MP_THROW_IF(size != sizeof(LOGCONTEXTA), mp::wintab_exception);
	context->lcSysMode = false;
	context->lcOptions |= CXO_MESSAGES | CXO_CSRMESSAGES;
	return context;
}

std::shared_ptr<LOGCONTEXTA> AddExtensions(std::shared_ptr<LOGCONTEXTA> context)
{
	spdlog::info("AddExtensions");
	for(const auto& value: {
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
		})
	{
		context->lcPktData |= value;
		// uncomment if relative mode is wanted
		// context.LcPktMode |= value
		context->lcMoveMask |= value;
	}
	// not sure how these work
	context->lcBtnUpMask = context->lcBtnDnMask;
	return context;
}

std::vector<Extension> AddAllOverrides(HCTX context)
{
	std::vector<Extension> extensions{};
	for(unsigned int tabletIndex = 0; tabletIndex < GetNumberOfDevices(context); ++tabletIndex)
	{
		try
		{
			auto overrides = AddOverrides(context, tabletIndex);
			extensions.insert(extensions.end(), overrides.begin(), overrides.end());
		}
		catch(wintab_exception ex)
		{
			spdlog::warn("Override failed: {}", std::string(ex.what()));
		}
	}
	return extensions;
}

std::vector<Extension> AddOverrides(HCTX context, unsigned int tabletIndex, std::vector<unsigned int> extensionTags)
{
	std::vector<Extension> extensions{};
	for(auto extTagIndex: extensionTags)
	{
		auto nofControls
			= ControlPropertyGet<unsigned int>(context, tabletIndex, extTagIndex, 0, 0, TABLET_PROPERTY_CONTROLCOUNT);
		for(auto controlIndex = 0u; controlIndex < nofControls; controlIndex++)
		{
			auto nofFuncs = ControlPropertyGet<unsigned int>(context,
															 tabletIndex,
															 extTagIndex,
															 controlIndex,
															 0,
															 TABLET_PROPERTY_FUNCCOUNT);
			for(auto functionIndex = 0u; functionIndex < nofFuncs; functionIndex++)
			{
				auto overrides = Override(context, tabletIndex, extTagIndex, functionIndex, controlIndex);
				extensions.insert(extensions.end(), overrides.begin(), overrides.end());
			}
		}
	}
	return extensions;
}

std::vector<Extension> Override(HCTX		 context,
								unsigned int tabletIndex,
								unsigned int extTagIndex,
								unsigned int functionIndex,
								unsigned int controlIndex)
{
	BOOL		PropertyOverride = true;
	std::string Name
		= fmt::format("{} {} {} {}", tabletIndex, ExtensionTagName(extTagIndex), controlIndex, functionIndex);
	BOOL Available	= ControlPropertyGet<BOOL>(context,
											   tabletIndex,
											   extTagIndex,
											   controlIndex,
											   functionIndex,
											   TABLET_PROPERTY_AVAILABLE);
	auto extensions = std::vector<Extension>{};
	if(Available)
	{
		ControlPropertySet(context,
						   tabletIndex,
						   extTagIndex,
						   controlIndex,
						   functionIndex,
						   TABLET_PROPERTY_OVERRIDE,
						   &PropertyOverride,
						   sizeof(PropertyOverride));
		try
		{
			ControlPropertySet(context,
							   tabletIndex,
							   extTagIndex,
							   controlIndex,
							   functionIndex,
							   TABLET_PROPERTY_OVERRIDE_NAME,
							   Name.c_str(),
							   Name.length() + 1);
		}
		catch(wintab_exception ex)
		{
			spdlog::warn("Could not set property name: {}", ex.what());
		}

		uint32_t Location = ControlPropertyGet<uint32_t>(context,
														 tabletIndex,
														 extTagIndex,
														 controlIndex,
														 functionIndex,
														 TABLET_PROPERTY_LOCATION);

		uint32_t MinRange = ControlPropertyGet<uint32_t>(context,
														 tabletIndex,
														 extTagIndex,
														 controlIndex,
														 functionIndex,
														 TABLET_PROPERTY_MIN);

		uint32_t MaxRange = ControlPropertyGet<uint32_t>(context,
														 tabletIndex,
														 extTagIndex,
														 controlIndex,
														 functionIndex,
														 TABLET_PROPERTY_MAX);

		uint32_t IconFormat = ControlPropertyGet<uint32_t>(context,
														   tabletIndex,
														   extTagIndex,
														   controlIndex,
														   functionIndex,
														   TABLET_PROPERTY_ICON_FORMAT);
		if(IconFormat == TABLET_ICON_FMT_NONE) {}
		else if(IconFormat == TABLET_ICON_FMT_4BPP_GRAY)
		{
			auto size = GetIconSize(context, tabletIndex, extTagIndex, functionIndex, controlIndex);
			/*
				// send the vector as property "TABLET_PROPERTY_OVERRIDE_ICON"
				return CtrlPropertySet(ext_I, tablet_I, control_I, function_I,
					TABLET_PROPERTY_OVERRIDE_ICON, imageData);
			*/
			spdlog::info("IconFormat: {} ({}x{})", IconFormat, std::get<0>(size), std::get<1>(size));
		}
		else
		{
			auto size = GetIconSize(context, tabletIndex, extTagIndex, functionIndex, controlIndex);
			/*
				// send the vector as property "TABLET_PROPERTY_OVERRIDE_ICON"
				return CtrlPropertySet(ext_I, tablet_I, control_I, function_I,
					TABLET_PROPERTY_OVERRIDE_ICON, imageData);
			*/
			spdlog::info("IconFormat: {} ({}x{})", IconFormat, std::get<0>(size), std::get<1>(size));
		}

		spdlog::info("OVERRIDDEN: {}, {}, {}, {}", tabletIndex, extTagIndex, controlIndex, functionIndex);
		spdlog::info("{} {} {} {} {}", PropertyOverride, Name, Location, MinRange, MaxRange);
		extensions.push_back(
			Extension{ tabletIndex, extTagIndex, functionIndex, controlIndex, Name, Location, MinRange, MaxRange });
	}
	else
	{
		spdlog::info("UNAVAILABLE: {}, {}, {}, {}", tabletIndex, extTagIndex, controlIndex, functionIndex);
	}
	return extensions;
}

void ControlPropertySet(HCTX		 handle,
						unsigned int tabletIndex,
						unsigned int extTagIndex,
						unsigned int controlIndex,
						unsigned int functionIndex,
						int			 propertyId,
						const void*	 value_p,
						size_t		 value_s)
{
	// Does not seem to matter if one allocates more than neeed. ( somtimes it does not work anyway ;D )
	std::unique_ptr<uint8_t> buffer(new uint8_t[sizeof(EXTPROPERTY) + value_s]);

	*((EXTPROPERTY*)buffer.get()) = { (unsigned char)0,
									  (unsigned char)tabletIndex,
									  (unsigned char)controlIndex,
									  (unsigned char)functionIndex,
									  (unsigned short)propertyId,
									  (unsigned short)0,
									  (unsigned long)value_s,
									  (unsigned char)0 };

	for(int index = 0; index < value_s; index++)
	{ ((EXTPROPERTY*)buffer.get())->data[index] = reinterpret_cast<const uint8_t*>(value_p)[index]; }

	MP_THROW_IF_WHAT(WTExtSet(handle, extTagIndex, buffer.get()) == 0, mp::wintab_exception, PropertyIdName(propertyId))

	return;
}

const char* ExtensionTagDescription(unsigned int extTagIndex)
{
	spdlog::info("ExtensionTagDescription");
	switch(extTagIndex)
	{
		case WTX_OBT: return "Out of bounds tracking";
		case WTX_FKEYS: return "Function keys";
		case WTX_TILT: return "Raw Cartesian tilt; 1.1";
		case WTX_CSRMASK: return "select input by cursor type; 1.1";
		case WTX_XBTNMASK: return "Extended button mask; 1.1";
		case WTX_EXPKEYS: return "ExpressKeys; 1.3 - DEPRECATED: see WTX_EXPKEYS2";
		case WTX_TOUCHSTRIP: return "TouchStrips; 1.4";
		case WTX_TOUCHRING: return "TouchRings; 1.4";
		case WTX_EXPKEYS2: return "ExpressKeys; 1.4";
		default: throw mp::wintab_exception(MP_HERE);
	}
}

const char* ExtensionTagName(unsigned int extTagIndex)
{
	spdlog::info("ExtensionTagName");
	switch(extTagIndex)
	{
		case WTX_OBT: return "WTX_OBT";
		case WTX_FKEYS: return "WTX_FKEYS";
		case WTX_TILT: return "WTX_TILT";
		case WTX_CSRMASK: return "WTX_CSRMASK";
		case WTX_XBTNMASK: return "WTX_XBTNMASK";
		case WTX_EXPKEYS: return "WTX_EXPKEYS";
		case WTX_TOUCHSTRIP: return "WTX_TOUCHSTRIP";
		case WTX_TOUCHRING: return "WTX_TOUCHRING";
		case WTX_EXPKEYS2: return "WTX_EXPKEYS2";
		default: throw mp::wintab_exception(MP_HERE);
	}
}
unsigned int GetNumberOfDevices(HCTX context)
{
	spdlog::info("GetNumberOfDevices");
	unsigned int nofDevices{};
	MP_THROW_IF(WTInfoA(WTI_INTERFACE, IFC_NDEVICES, &nofDevices) != sizeof(nofDevices), mp::wintab_exception)
	return nofDevices;
}

PACKET GetDataPacket(HCTX context, WPARAM wparam)
{
	PACKET packet{};
	MP_WARN_IF(WTPacket(context, wparam, &packet) == 0);
	return packet;
}

std::string WTInfoName(int WTI_CATEGORY, int INDEX)
{
	switch(WTI_CATEGORY)
	{
		case WTI_CURSORS:
			return std::string("WTI_CURSORS") + ":" + std::invoke([&INDEX]() -> std::string {
					   switch(INDEX)
					   {
						   case CSR_NAME: return "CSR_NAME";
						   case CSR_ACTIVE: return "CSR_ACTIVE";
						   case CSR_PKTDATA: return "CSR_PKTDATA";
						   case CSR_BUTTONS: return "CSR_BUTTONS";
						   case CSR_BUTTONBITS: return "CSR_BUTTONBITS";
						   case CSR_BTNNAMES: return "CSR_BTNNAMES";
						   case CSR_BUTTONMAP: return "CSR_BUTTONMAP";
						   case CSR_SYSBTNMAP: return "CSR_SYSBTNMAP";
						   case CSR_NPBUTTON: return "CSR_NPBUTTON";
						   case CSR_NPBTNMARKS: return "CSR_NPBTNMARKS";
						   case CSR_NPRESPONSE: return "CSR_NPRESPONSE";
						   case CSR_TPBUTTON: return "CSR_TPBUTTON";
						   case CSR_TPBTNMARKS: return "CSR_TPBTNMARKS";
						   case CSR_TPRESPONSE: return "CSR_TPRESPONSE";
						   case CSR_PHYSID: return "CSR_PHYSID";
						   case CSR_MODE: return "CSR_MODE";
						   case CSR_MINPKTDATA: return "CSR_MINPKTDATA";
						   case CSR_MINBUTTONS: return "CSR_MINBUTTONS";
						   case CSR_CAPABILITIES: return "CSR_CAPABILITIES";
						   case CSR_TYPE: return "CSR_TYPE";
						   default: return "<CSR_UNKNOWN>";
					   }
				   });
		case WTI_DEVICES:
			return std::string("WTI_DEVICES") + ":" + std::invoke([&INDEX]() -> std::string {
					   switch(INDEX)
					   {
						   case DVC_NAME: return "DVC_NAME";
						   case DVC_HARDWARE: return "DVC_HARDWARE";
						   case DVC_NCSRTYPES: return "DVC_NCSRTYPES";
						   case DVC_FIRSTCSR: return "DVC_FIRSTCSR";
						   case DVC_PKTRATE: return "DVC_PKTRATE";
						   case DVC_PKTDATA: return "DVC_PKTDATA";
						   case DVC_PKTMODE: return "DVC_PKTMODE";
						   case DVC_CSRDATA: return "DVC_CSRDATA";
						   case DVC_XMARGIN: return "DVC_XMARGIN";
						   case DVC_YMARGIN: return "DVC_YMARGIN";
						   case DVC_ZMARGIN: return "DVC_ZMARGIN";
						   case DVC_X: return "DVC_X";
						   case DVC_Y: return "DVC_Y";
						   case DVC_Z: return "DVC_Z";
						   case DVC_NPRESSURE: return "DVC_NPRESSURE";
						   case DVC_TPRESSURE: return "DVC_TPRESSURE";
						   case DVC_ORIENTATION: return "DVC_ORIENTATION";
						   case DVC_ROTATION: return "DVC_ROTATION";
						   case DVC_PNPID: return "DVC_PNPID";
						   default: return "<DVC_UNKNOWN>";
					   }
				   });
		case WTI_DEFCONTEXT: return "WTI_DEFCONTEXT";
		case WTI_DEFSYSCTX: return "WTI_DEFSYSCTX";
		case WTI_DDCTXS: return "WTI_DDCTXS";
		case WTI_DSCTXS:
			return std::string("WTI_DSCTXS") + ":" + std::invoke([&INDEX]() -> std::string {
					   switch(INDEX)
					   {
						   case CTX_NAME: return "CTX_NAME";
						   case CTX_OPTIONS: return "CTX_OPTIONS";
						   case CTX_STATUS: return "CTX_STATUS";
						   case CTX_LOCKS: return "CTX_LOCKS";
						   case CTX_MSGBASE: return "CTX_MSGBASE";
						   case CTX_DEVICE: return "CTX_DEVICE";
						   case CTX_PKTRATE: return "CTX_PKTRATE";
						   case CTX_PKTDATA: return "CTX_PKTDATA";
						   case CTX_PKTMODE: return "CTX_PKTMODE";
						   case CTX_MOVEMASK: return "CTX_MOVEMASK";
						   case CTX_BTNDNMASK: return "CTX_BTNDNMASK";
						   case CTX_BTNUPMASK: return "CTX_BTNUPMASK";
						   case CTX_INORGX: return "CTX_INORGX";
						   case CTX_INORGY: return "CTX_INORGY";
						   case CTX_INORGZ: return "CTX_INORGZ";
						   case CTX_INEXTX: return "CTX_INEXTX";
						   case CTX_INEXTY: return "CTX_INEXTY";
						   case CTX_INEXTZ: return "CTX_INEXTZ";
						   case CTX_OUTORGX: return "CTX_OUTORGX";
						   case CTX_OUTORGY: return "CTX_OUTORGY";
						   case CTX_OUTORGZ: return "CTX_OUTORGZ";
						   case CTX_OUTEXTX: return "CTX_OUTEXTX";
						   case CTX_OUTEXTY: return "CTX_OUTEXTY";
						   case CTX_OUTEXTZ: return "CTX_OUTEXTZ";
						   case CTX_SENSX: return "CTX_SENSX";
						   case CTX_SENSY: return "CTX_SENSY";
						   case CTX_SENSZ: return "CTX_SENSZ";
						   case CTX_SYSMODE: return "CTX_SYSMODE";
						   case CTX_SYSORGX: return "CTX_SYSORGX";
						   case CTX_SYSORGY: return "CTX_SYSORGY";
						   case CTX_SYSEXTX: return "CTX_SYSEXTX";
						   case CTX_SYSEXTY: return "CTX_SYSEXTY";
						   case CTX_SYSSENSX: return "CTX_SYSSENSX";
						   case CTX_SYSSENSY: return "CTX_SYSSENSY";
						   default: return "<CTX_UNKNOWN>";
					   }
				   });
		case WTI_EXTENSIONS:
			return (std::string("WTI_EXTENSIONS") + ":" + std::invoke([&INDEX]() -> std::string {
						switch(INDEX)
						{
							case EXT_NAME: return "EXT_NAME";
							case EXT_TAG: return "EXT_TAG";
							case EXT_MASK: return "EXT_MASK";
							case EXT_SIZE: return "EXT_SIZE";
							case EXT_AXES: return "EXT_AXES";
							case EXT_DEFAULT: return "EXT_DEFAULT";
							case EXT_DEFCONTEXT: return "EXT_DEFCONTEXT";
							case EXT_DEFSYSCTX: return "EXT_DEFSYSCTX";
							case EXT_CURSORS: return "EXT_CURSORS";
							case EXT_DEVICES: return "EXT_DEVICES";
							default: return "<EXT_UNKNOWN>";
						}
					}));
		case WTI_INTERFACE:
			return std::string("WTI_INTERFACE") + ":" + std::invoke([&INDEX]() {
					   switch(INDEX)
					   {
						   case IFC_WINTABID: return "IFC_WINTABID";
						   case IFC_SPECVERSION: return "IFC_SPECVERSION";
						   case IFC_IMPLVERSION: return "IFC_IMPLVERSION";
						   case IFC_NDEVICES: return "IFC_NDEVICES";
						   case IFC_NCURSORS: return "IFC_NCURSORS";
						   case IFC_NCONTEXTS: return "IFC_NCONTEXTS";
						   case IFC_CTXOPTIONS: return "IFC_CTXOPTIONS";
						   case IFC_CTXSAVESIZE: return "IFC_CTXSAVESIZE";
						   case IFC_NEXTENSIONS: return "IFC_NEXTENSIONS";
						   case IFC_NMANAGERS: return "IFC_NMANAGERS";
						   default: return "<IFC_UNKNOWN>";
					   }
				   });
		case WTI_STATUS:
			return std::string("WTI_STATUS") + ":" + std::invoke([&INDEX]() -> std::string {
					   switch(INDEX)
					   {
						   case STA_CONTEXTS: return "STA_CONTEXTS";
						   case STA_SYSCTXS: return "STA_SYSCTXS";
						   case STA_PKTRATE: return "STA_PKTRATE";
						   case STA_PKTDATA: return "STA_PKTDATA";
						   case STA_MANAGERS: return "STA_MANAGERS";
						   case STA_SYSTEM: return "STA_SYSTEM";
						   case STA_BUTTONUSE: return "STA_BUTTONUSE";
						   case STA_SYSBTNUSE: return "STA_SYSBTNUSE";
						   default: return "<STA_UNKNOWN>";
					   }
				   });
		default: return "<CATEGORY_UNKNOWN>";
	}
}

template<class T>
T get_valueA(int WTI_CATEGORY, int INDEX)
{
	T	 t{};
	auto expectedSize = WTInfoA(WTI_EXTENSIONS, EXT_TAG, nullptr);

	MP_WARN_IF_WHAT(expectedSize != sizeof(T),
					" {} = expectedSize != sizeof(T) = {} : {}",
					expectedSize,
					sizeof(T),
					WTInfoName(WTI_CATEGORY, INDEX));

	std::unique_ptr<uint8_t> buffer(new uint8_t[expectedSize]);

	auto actualSize = WTInfoA(WTI_EXTENSIONS, EXT_TAG, buffer.get());

	MP_WARN_IF_WHAT(actualSize != expectedSize,
					" {} = actualSize != expectedSize = {} : {} ",
					actualSize,
					expectedSize,
					WTInfoName(WTI_CATEGORY, INDEX));

	MP_THROW_IF_WHAT(actualSize == 0, mp::wintab_exception, "{} {}", WTI_CATEGORY, INDEX);

	MP_WARN_IF_WHAT(actualSize != sizeof(T),
					" {} = actualSize != sizeof(T) = {} : {}",
					actualSize,
					sizeof(T),
					WTInfoName(WTI_CATEGORY, INDEX));

	t = *reinterpret_cast<T*>(buffer.get());

	return t;
}

std::string get_stringA(int WTI_CATEGORY, int INDEX)
{
	auto				  expectedSize = WTInfoA(WTI_CATEGORY, INDEX, nullptr);
	std::unique_ptr<char> buffer(new char[expectedSize]);
	auto				  actualSize = WTInfoA(WTI_CATEGORY, INDEX, buffer.get());

	if(actualSize != expectedSize)
	{
		spdlog::warn(MP_HERE + " {} = actualSize != expectedSize = {} : {}",
					 actualSize,
					 expectedSize,
					 WTInfoName(WTI_CATEGORY, INDEX));
	}

	MP_THROW_IF_WHAT(actualSize == 0, mp::wintab_exception, "{}", WTInfoName(WTI_CATEGORY, INDEX));

	return as_string(std::basic_string<char>(buffer.get(), actualSize));
}

template<class T>
std::vector<T> get_vectorA(int WTI_CATEGORY, int INDEX)
{
	std::vector<T>	   result;
	auto			   expectedSize = WTInfoA(WTI_CATEGORY, INDEX, nullptr);
	std::unique_ptr<T> buffer(new T[expectedSize / sizeof(T)]);
	auto			   actualSize = WTInfoA(WTI_CATEGORY, INDEX, buffer.get());

	MP_WARN_IF_WHAT(actualSize != expectedSize,
					" {} = actualSize != expectedSize = {} : {}",
					actualSize,
					expectedSize,
					WTInfoName(WTI_CATEGORY, INDEX));

	MP_WARN_IF_WHAT(actualSize == 0, " : {} ", WTInfoName(WTI_CATEGORY, INDEX));

	for(auto index = 0; index < actualSize / sizeof(UINT); ++index) { result.push_back(buffer.get()[index]); }

	return result;
}

WintabCursorCapabilities GetWintabCursorCapabilities()
{
	WintabCursorCapabilities caps{};
	caps.name						   = get_stringA(WTI_CURSORS, CSR_NAME);
	caps.active						   = get_valueA<BOOL>(WTI_CURSORS, CSR_ACTIVE);
	caps.supported_packet_data_bitmask = get_valueA<WTPKT>(WTI_CURSORS, CSR_PKTDATA);
	caps.nof_buttons				   = get_valueA<UINT>(WTI_CURSORS, CSR_BUTTONS);
	caps.nof_button_bits			   = get_valueA<UINT>(WTI_CURSORS, CSR_BUTTONBITS);
	auto button_names				   = get_vectorA<char>(WTI_CURSORS, CSR_BTNNAMES);
	for(auto button_begin = button_names.begin(); button_begin != button_names.end();)
	{
		auto button_end = std::find(button_begin + 1, button_names.end(), '\0');
		if(*button_begin != 0) caps.button_names.push_back(std::string(button_begin, button_end));
		button_begin = button_end;
	}
	MP_ERROR_IF(WTInfoA(WTI_CURSORS, CSR_BUTTONMAP, &caps.logical_button_ids[0]) == 0)
	MP_ERROR_IF(WTInfoA(WTI_CURSORS, CSR_SYSBTNMAP, &caps.logical_button_ids_action_code[0]) == 0)
	caps.physical_button_id_normal_pressure = get_valueA<UINT>(WTI_CURSORS, CSR_NPBUTTON);
	MP_ERROR_IF(WTInfoA(WTI_CURSORS, CSR_NPBTNMARKS, &caps.normal_pressure_button_marks[0]) == 0)
	caps.normal_pressure_response_curve			= get_vectorA<UINT>(WTI_CURSORS, CSR_NPRESPONSE);
	caps.physical_button_id_tangential_pressure = get_valueA<UINT>(WTI_CURSORS, CSR_TPBUTTON);
	MP_ERROR_IF(WTInfoA(WTI_CURSORS, CSR_TPBTNMARKS, &caps.tangential_pressure_button_marks[0]) == 0)
	caps.tangential_pressure_response_curve	 = get_vectorA<UINT>(WTI_CURSORS, CSR_TPRESPONSE);
	caps.physical_cursor_id					 = get_valueA<DWORD>(WTI_CURSORS, CSR_PHYSID);
	caps.cursor_mode						 = get_valueA<UINT>(WTI_CURSORS, CSR_MODE);
	caps.physical_cursor_minimum_packet_data = get_valueA<UINT>(WTI_CURSORS, CSR_MINPKTDATA);
	caps.physical_cursor_minimum_nof_buttons = get_valueA<UINT>(WTI_CURSORS, CSR_MINBUTTONS);
	caps.cursor_capability_flags = static_cast<CursorCapabilityFlag>(get_valueA<UINT>(WTI_CURSORS, CSR_CAPABILITIES));
	return caps;
}

WintabExtensionCapabilities GetWintabExtensionCapabilities()
{
	WintabExtensionCapabilities caps{};
	caps.name			  = get_stringA(WTI_EXTENSIONS, EXT_NAME);
	caps.extension_tag_id = get_valueA<UINT>(WTI_EXTENSIONS, EXT_TAG);
	caps.extension_mask	  = get_valueA<WTPKT>(WTI_EXTENSIONS, EXT_MASK);
	MP_ERROR_IF(WTInfoA(WTI_EXTENSIONS, EXT_SIZE, &caps.extension_size[0]) == 0);
	caps.extension_axes				 = get_vectorA<AXIS>(WTI_EXTENSIONS, EXT_AXES);
	caps.current_global_default_data = get_vectorA<BYTE>(WTI_EXTENSIONS, EXT_DEFAULT);
	caps.default_digitizing_context	 = get_vectorA<BYTE>(WTI_EXTENSIONS, EXT_DEFCONTEXT);
	caps.default_system_context		 = get_vectorA<BYTE>(WTI_EXTENSIONS, EXT_DEFSYSCTX);
	caps.extension_cursors			 = get_vectorA<BYTE>(WTI_EXTENSIONS, EXT_CURSORS);
	return caps;
}

WintabDeviceCapabilities GetWintabDeviceCapabilities()
{
	WintabDeviceCapabilities caps{};
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_NPRESSURE, &caps.normal_pressure) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_ORIENTATION, &caps.orientation[0]) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_X, &caps.position[0]) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_Y, &caps.position[1]) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_Z, &caps.position[2]) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_ROTATION, &caps.rotation[0]) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_TPRESSURE, &caps.tangential_pressure) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_HARDWARE, &caps.hardware_capabilities) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_XMARGIN, &caps.margin[0]) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_YMARGIN, &caps.margin[1]) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_ZMARGIN, &caps.margin[2]) == 0)
	caps.plug_and_play_id = get_stringA(WTI_DEVICES, DVC_PNPID);
	caps.name			  = get_stringA(WTI_DEVICES, DVC_NAME);
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_FIRSTCSR, &caps.first_cursor_type_number) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_PKTRATE, &caps.max_packet_report_rate_hz) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_NCSRTYPES, &caps.nof_supported_cursor_types) == 0)

	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_PKTDATA, &caps.supported_packet_data_bitmask) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_CSRDATA, &caps.supported_packet_data_bitmask_certain_cursors) == 0)
	MP_ERROR_IF(WTInfoA(WTI_DEVICES, DVC_PKTMODE, &caps.supported_packet_data_bitmask_relative_change) == 0)

	return caps;
}

TabletButtonState GetButtonState(PACKET const& packet)
{
	return static_cast<TabletButtonState>(HIWORD(packet.pkButtons));
}

WORD GetButtonNumber(PACKET const& packet) { return LOWORD(packet.pkButtons); }

PACKETEXT GetDataPacketExt(HCTX context, WPARAM wparam)
{
	PACKETEXT packet_ext{};
	MP_WARN_IF(WTPacket(context, wparam, &packet_ext) == 0)
	return packet_ext;
}

std::tuple<unsigned int, unsigned int> GetIconSize(HCTX			handle,
												   unsigned int tabletIndex,
												   unsigned int extTagIndex,
												   unsigned int functionIndex,
												   unsigned int controlIndex)
{
	// Get the width of the display icon

	auto IconWidth = ControlPropertyGet<unsigned int>(handle,
													  tabletIndex,
													  extTagIndex,
													  controlIndex,
													  functionIndex,
													  TABLET_PROPERTY_ICON_WIDTH);

	// Get the height of the display icon
	auto IconHeight = ControlPropertyGet<unsigned int>(handle,
													   tabletIndex,
													   extTagIndex,
													   controlIndex,
													   functionIndex,
													   TABLET_PROPERTY_ICON_HEIGHT);

	return std::make_tuple(IconWidth, IconHeight);
}

HCTX OpenWintabContext(HWND windowId, std::shared_ptr<LOGCONTEXTA> contextDescriptor, bool openEnabled)
{
	spdlog::info("OpenWintabContext");
	auto result = WTOpenA(windowId, contextDescriptor.get(), openEnabled);
	MP_THROW_IF(result == nullptr, mp::wintab_exception)
	return result;
}

std::string PropertyIdName(int propertyId)
{
	switch(propertyId)
	{
		case TABLET_PROPERTY_AVAILABLE: return "TABLET_PROPERTY_AVAILABLE";
		case TABLET_PROPERTY_CONTROLCOUNT: return "TABLET_PROPERTY_CONTROLCOUNT";
		case TABLET_PROPERTY_FUNCCOUNT: return "TABLET_PROPERTY_FUNCCOUNT";
		case TABLET_PROPERTY_ICON_FORMAT: return "TABLET_PROPERTY_ICON_FORMAT";
		case TABLET_PROPERTY_ICON_HEIGHT: return "TABLET_PROPERTY_ICON_HEIGHT";
		case TABLET_PROPERTY_ICON_WIDTH: return "TABLET_PROPERTY_ICON_WIDTH";
		case TABLET_PROPERTY_LOCATION: return "TABLET_PROPERTY_LOCATION";
		case TABLET_PROPERTY_MAX: return "TABLET_PROPERTY_MAX";
		case TABLET_PROPERTY_MIN: return "TABLET_PROPERTY_MIN";
		case TABLET_PROPERTY_OVERRIDE_ICON: return "TABLET_PROPERTY_OVERRIDE_ICON";
		case TABLET_PROPERTY_OVERRIDE_NAME: return "TABLET_PROPERTY_OVERRIDE_NAME";
		case TABLET_PROPERTY_OVERRIDE: return "TABLET_PROPERTY_OVERRIDE";
		default: return "TABLET_PROPERTY_UNKNOWN";
	}
}

void RemoveAllOverrides(HCTX context, std::vector<unsigned int> extensionTags)
{
	for(unsigned int tabletIndex = 0; tabletIndex < GetNumberOfDevices(context); ++tabletIndex)
	{
		for(auto extTagIndex: extensionTags)
		{
			uint32_t nofControls
				= ControlPropertyGet<uint32_t>(context, tabletIndex, extTagIndex, 0, 0, TABLET_PROPERTY_CONTROLCOUNT);
			for(auto controlIndex = 0; controlIndex < nofControls; controlIndex++)
			{
				uint32_t nofFuncs = ControlPropertyGet<uint32_t>(context,
																 tabletIndex,
																 extTagIndex,
																 controlIndex,
																 0,
																 TABLET_PROPERTY_FUNCCOUNT);
				for(auto functionIndex = 0; functionIndex < nofFuncs; functionIndex++)
				{
					constexpr uint8_t off = 0;
					ControlPropertySet(context,
									   tabletIndex,
									   extTagIndex,
									   controlIndex,
									   functionIndex,
									   TABLET_PROPERTY_OVERRIDE,
									   &off,
									   sizeof(off));
				}
			}
		}
	}
}
} // namespace mp
