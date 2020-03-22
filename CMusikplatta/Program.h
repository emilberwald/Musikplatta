#pragma once
#include "IProgram.h"
#include "L0ApiWintab.h"

#include <memory>

namespace mp
{
class Program: public IProgram
{
  public:
	Program();
	virtual ~Program();
	virtual void HandleMessage(HWND windowId, UINT messageType, WPARAM wparam, LPARAM lparam) override;

  private:
	std::shared_ptr<LOGCONTEXTW> context_descriptor;
	HCTX						 context;
};
} // namespace mp
