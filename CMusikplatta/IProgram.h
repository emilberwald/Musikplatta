#pragma once
#include "framework.h"
namespace mp
{
class IProgram
{
  public:
	virtual ~IProgram() {}
	virtual void HandleMessage(HWND windowId, UINT messageType, WPARAM wparam, LPARAM lparam) = 0;
};
} // namespace mp
