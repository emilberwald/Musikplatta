#pragma once
#include "L1ApiWintab.h"

#include <array>

LOGCONTEXTW GetDefaultDigitizingContext()
{
	auto size = WTInfoW(WTI_DEFCONTEXT, 0, nullptr);
	if(size != sizeof(LOGCONTEXTW)) { spdlog::warn(__HERE__); }
	LOGCONTEXTW context {};
	size = WTInfoW(WTI_DEFCONTEXT, 0, &context);
	if(size != sizeof(LOGCONTEXTW)) { spdlog::warn(__HERE__); }
	context.lcSysMode = false;
	context.lcOptions |= CXO_MESSAGES | CXO_CSRMESSAGES;
	for(const auto &value: {
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
		context.lcPktData |= value;
		// uncomment if relative mode is wanted
		// context.LcPktMode |= value
		context.lcMoveMask |= value;
	}
	// not sure how these work
	context.lcBtnUpMask = context.lcBtnDnMask;
	return context;
}

template<class T>
T ControlPropertyGet(HCTX		  handle,
					 unsigned int tabletIndex,
					 unsigned int extTagIndex,
					 unsigned int controlIndex,
					 unsigned int functionIndex,
					 int		  propertyId)
{
	std::array<char, sizeof(EXTPROPERTY) + sizeof(T)> buffer {};
	EXTPROPERTY										  ext_property = { (unsigned char)0,
								   (unsigned char)tabletIndex,
								   (unsigned char)controlIndex,
								   (unsigned char)functionIndex,
								   (unsigned short)propertyId,
								   (unsigned short)0,
								   (unsigned long)sizeof(T),
								   (unsigned char)0 };
	std::memcpy(buffer.data(), &ext_property, sizeof(property));

	if(WTExtGet(handle, extTagIndex, buffer) == 0) { spdlog::warn(__HERE__); }

	T prop {};
	std::memcpy(prop, buffer.data() + sizeof(EXTPROPERTY), sizeof(T));
	return prop;
}

template<class T>
void ControlPropertySet(HCTX		 handle,
						unsigned int tabletIndex,
						unsigned int extTagIndex,
						unsigned int controlIndex,
						unsigned int functionIndex,
						int			 propertyId,
						T			 value)
{
	std::array<char, sizeof(EXTPROPERTY) + sizeof(T)> buffer {};
	EXTPROPERTY										  ext_property = { (unsigned char)0,
								   (unsigned char)tabletIndex,
								   (unsigned char)controlIndex,
								   (unsigned char)functionIndex,
								   (unsigned short)propertyId,
								   (unsigned short)0,
								   (unsigned long)sizeof(T),
								   (unsigned char)0 };
	std::memcpy(buffer.data(), &ext_property, sizeof(ext_property));
	std::memcpy(buffer.data() + sizeof(EXTPROPERTY), &value, sizeof(value));
	WTExtSet(handle, extTagIndex, buffer.data());

	if(WTExtSet(handle, extTagIndex, buffer.data()) == 0) { spdlog::warn(__HERE__); }
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
