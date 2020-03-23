#pragma once
#include "IProgram.h"
#include "L0ApiWintab.h"
#include "L2ApiWintab.h"

#include <memory>
#include <vector>

namespace mp
{
class Program: public IProgram
{
  public:
	Program();
	virtual ~Program();
	virtual void HandleMessage(HWND windowId, UINT messageType, WPARAM wparam, LPARAM lparam) override;

  private:
	std::shared_ptr<LOGCONTEXTA> context_descriptor;
	HCTX						 context;
	HWND						 window_id;
	std::vector<Extension>		 extensions;
	WintabDeviceCapabilities	 wintab_device_capabilities;
	WintabCursorCapabilities	 wintab_cursor_capabilities;
	WintabExtensionCapabilities	 wintab_extension_capabilities;
};
} // namespace mp
