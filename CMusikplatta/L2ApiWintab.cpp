#include "L2ApiWintab.h"

#include "Common.h"
#include "L0ApiWintab.h"
#include "L1ApiWintab.h"

#include <spdlog/spdlog.h>

namespace mp
{
std::shared_ptr<LOGCONTEXTW> AddDefaultDigitizingContext(std::shared_ptr<LOGCONTEXTW> context)
{
	auto size = WTInfoW(WTI_DEFCONTEXT, 0, nullptr);

	if(size != sizeof(LOGCONTEXTW)) MP_THROW_WINTAB_EXCEPTION
	size = WTInfoW(WTI_DEFCONTEXT, 0, context.get());
	if(size != sizeof(LOGCONTEXTW)) MP_THROW_WINTAB_EXCEPTION
	context->lcSysMode = false;
	context->lcOptions |= CXO_MESSAGES | CXO_CSRMESSAGES;
	return context;
}

std::shared_ptr<LOGCONTEXTW> AddExtensions(std::shared_ptr<LOGCONTEXTW> context)
{
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

HCTX OpenWintabContext(HWND windowId, std::shared_ptr<LOGCONTEXTW> contextDescriptor, bool openEnabled)
{
	auto result = WTOpenW(windowId, contextDescriptor.get(), openEnabled);
	if(result == nullptr) MP_THROW_WINTAB_EXCEPTION
	return result;
}

const char* ExtensionTagName(unsigned int extTagIndex)
{
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
		default: MP_THROW_WINTAB_EXCEPTION
	}
}

const char* ExtensionTagDescription(unsigned int extTagIndex)
{
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
		default: MP_THROW_WINTAB_EXCEPTION
	}
}

unsigned int GetNumberOfDevices(HCTX context)
{
	unsigned int nofDevices{};
	if(WTInfoW(WTI_INTERFACE, IFC_NDEVICES, &nofDevices) != sizeof(nofDevices)) MP_THROW_WINTAB_EXCEPTION
	return nofDevices;
}

void Override(HCTX		   context,
			  unsigned int tabletIndex,
			  unsigned int extTagIndex,
			  unsigned int functionIndex,
			  unsigned int controlIndex)
{
	BOOL		PropertyOverride = true;
	std::string Name
		= fmt::format("{}:{}:{}:{}", tabletIndex, ExtensionTagName(extTagIndex), controlIndex, functionIndex);
	BOOL Available = ControlPropertyGet<BOOL>(context,
											  tabletIndex,
											  extTagIndex,
											  controlIndex,
											  functionIndex,
											  TABLET_PROPERTY_AVAILABLE);
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

		ControlPropertySet(context,
						   tabletIndex,
						   extTagIndex,
						   controlIndex,
						   functionIndex,
						   TABLET_PROPERTY_OVERRIDE_NAME,
						   Name.c_str(),
						   Name.length() + 1);

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
	}
	else
	{
		spdlog::info("UNAVAILABLE: {}, {}, {}, {}", tabletIndex, extTagIndex, controlIndex, functionIndex);
	}
}

void AddOverrides(HCTX context, unsigned int tabletIndex, std::vector<unsigned int> extensionTags)
{
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
			{ Override(context, tabletIndex, extTagIndex, functionIndex, controlIndex); }
		}
	}
}

void AddAllOverrides(HCTX context)
{
	for(unsigned int tabletIndex = 0; tabletIndex < GetNumberOfDevices(context); ++tabletIndex)
	{
		try
		{
			AddOverrides(context, tabletIndex);
		}
		catch(wintab_exception ex)
		{
			spdlog::warn("Override failed: {}", std::string(ex.what()));
		}
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

void ControlPropertySet(HCTX		 handle,
						unsigned int tabletIndex,
						unsigned int extTagIndex,
						unsigned int controlIndex,
						unsigned int functionIndex,
						int			 propertyId,
						const void*	 value_p,
						size_t		 value_s)
{
	std::unique_ptr<uint8_t> buffer(new uint8_t[sizeof(EXTPROPERTY) + value_s - sizeof(uint8_t)]);

	*((EXTPROPERTY*)buffer.get()) = { (unsigned char)0,
									  (unsigned char)tabletIndex,
									  (unsigned char)controlIndex,
									  (unsigned char)functionIndex,
									  (unsigned short)propertyId,
									  (unsigned short)0,
									  (unsigned long)value_s,
									  (unsigned char)0 };

	std::memcpy(reinterpret_cast<void*>(&(reinterpret_cast<EXTPROPERTY*>(buffer.get())->data[0])), value_p, value_s);

	if(WTExtSet(handle, extTagIndex, buffer.get()) == 0) MP_THROW_WINTAB_EXCEPTION

	return;
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

PACKETEXT GetDataPacketExt(HCTX context, WPARAM wparam)
{
	PACKETEXT packet_ext{};
	if(WTPacket(context, wparam, &packet_ext) == 0)
	{ spdlog::warn("Packet not found. {} {}", context->unused, wparam); }
	return packet_ext;
}

PACKET GetDataPacket(HCTX context, WPARAM wparam)
{
	PACKET packet{};
	if(WTPacket(context, wparam, &packet) == 0) { spdlog::warn("Packet not found. {} {}", context->unused, wparam); }
	return packet;
}

} // namespace mp
