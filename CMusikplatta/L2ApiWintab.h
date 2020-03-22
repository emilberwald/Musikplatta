#pragma once
#include "Common.h"
#include "L0ApiWintab.h"
#include "L1ApiWintab.h"

#include <array>
#include <stdexcept>
#include <vector>

#define MP_THROW_WINTAB_EXCEPTION                                                                                      \
	{                                                                                                                  \
		spdlog::error(MP_HERE);                                                                                        \
		throw mp::wintab_exception(MP_HERE);                                                                           \
	}

namespace mp
{
struct wintab_exception: std::runtime_error
{
	using std::runtime_error::runtime_error;
};

std::shared_ptr<LOGCONTEXTW> AddDefaultDigitizingContext(std::shared_ptr<LOGCONTEXTW> context);

std::shared_ptr<LOGCONTEXTW> AddExtensions(std::shared_ptr<LOGCONTEXTW> context);

HCTX OpenWintabContext(HWND windowId, std::shared_ptr<LOGCONTEXTW> contextDescriptor, bool openEnabled);

const char* ExtensionTagName(unsigned int extTagIndex);

const char* ExtensionTagDescription(unsigned int extTagIndex);

unsigned int GetNumberOfDevices(HCTX context);

void Override(HCTX		   context,
			  unsigned int tabletIndex,
			  unsigned int extTagIndex,
			  unsigned int functionIndex,
			  unsigned int controlIndex);

void AddOverrides(HCTX						context,
				  unsigned int				tabletIndex,
				  std::vector<unsigned int> extensionTags = { WTX_TOUCHSTRIP, WTX_TOUCHRING, WTX_EXPKEYS2 });

void AddAllOverrides(HCTX context);
void RemoveAllOverrides(HCTX					  context,
						std::vector<unsigned int> extensionTags = { WTX_TOUCHSTRIP, WTX_TOUCHRING, WTX_EXPKEYS2 });

template<class T>
inline T ControlPropertyGet(HCTX		 handle,
							unsigned int tabletIndex,
							unsigned int extTagIndex,
							unsigned int controlIndex,
							unsigned int functionIndex,
							int			 propertyId)
{
	std::unique_ptr<uint8_t> buffer(new uint8_t[sizeof(EXTPROPERTY) + sizeof(T) - sizeof(uint8_t)]);

	*reinterpret_cast<EXTPROPERTY*>(buffer.get()) = { (unsigned char)0,
													  (unsigned char)tabletIndex,
													  (unsigned char)controlIndex,
													  (unsigned char)functionIndex,
													  (unsigned short)propertyId,
													  (unsigned short)0,
													  (unsigned long)sizeof(T),
													  (unsigned char)0 };

	if(WTExtGet(handle, extTagIndex, buffer.get()) == 0) MP_THROW_WINTAB_EXCEPTION;

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

} // namespace mp
