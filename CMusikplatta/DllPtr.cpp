#include "DllPtr.h"

#include "Common.h"
#include "framework.h"

#include <memory>
#include <spdlog/spdlog.h>
#include <string>
#include <system_error>
namespace mp
{
std::basic_string<char> GetLatestErrorMessage()
{
	auto		latest_error_code{ ::GetLastError() };
	LPTSTR		string_ptr{ nullptr };
	const DWORD nof_tchars
		= FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_ALLOCATE_BUFFER,
						NULL,
						latest_error_code,
						MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
						reinterpret_cast<LPTSTR>(&string_ptr),
						0,
						NULL);

	if(nof_tchars > 0)
	{
		auto									  deleter = [](void *p) { ::LocalFree(p); };
		std::unique_ptr<TCHAR, decltype(deleter)> string_ptr_buffer(string_ptr, deleter);
		return as_string(std::basic_string<TCHAR>(string_ptr_buffer.get(), nof_tchars));
	}
	else
	{
		auto error_code{ ::GetLastError() };
		throw std::system_error(error_code, std::system_category(), MP_HERE);
	}
}

DllPtr::DllPtr(const wchar_t *filename): _module(LoadLibraryW(filename))
{
	if(_module == 0) { spdlog::error(MP_HEREWIN32 + as_string(filename)); }
}

DllPtr::~DllPtr()
{
	if(FreeLibrary(_module) == 0)
	{
		try
		{
			spdlog::error(MP_HEREWIN32);
		}
		catch(std::exception ex)
		{
			spdlog::error(MP_HERE);
		}
	}
}

ProcessPtr DllPtr::operator[](const char *proc_name) const
{
	return ProcessPtr(GetProcAddress(_module, proc_name), proc_name);
}
} // namespace mp
