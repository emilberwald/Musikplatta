#pragma once
#include "ProcessPtr.h"

#include <Windows.h>
namespace mp
{
class DllPtr
{
  public:
	explicit DllPtr(const wchar_t *filename);
	~DllPtr();

	ProcessPtr operator[](const char *proc_name) const;

	static HMODULE _parent_module;

  private:
	HMODULE _module;
};

} // namespace mp
