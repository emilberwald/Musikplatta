#include "ProcessPtr.h"

#include "Common.h"
#include "framework.h"

#include <spdlog/spdlog.h>

ProcessPtr::ProcessPtr(FARPROC ptr, const char *name): _ptr(ptr)
{
	if(_ptr == nullptr) { spdlog::error(MP_HEREWIN32 + " " + std::string(name)); }
}