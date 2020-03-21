#ifndef DLL_H
#define DLL_H

#include <type_traits>
#include <string>
#include <memory>
#include <system_error>
#include <stdexcept>
#include <locale>
#include <iostream>
#include <string>
#include <locale>
#include <codecvt>
#include <iomanip>
#include <stdexcept>
#include <Windows.h>
#include <fstream>

#define __HERE__ (std::to_string(__LINE__) + ":" + __FILE__ + " " + __func__ + ":")
#define __ERROR__ (__HERE__ + GetLatestErrorMessage())
#define __LOG(WHAT) { std::cerr << WHAT << std::endl; std::ofstream out("Wintab.h"); if(out) { auto old_rdbuf = std::clog.rdbuf(); std::clog.rdbuf(out.rdbuf()); std::clog << WHAT << std::endl; std::clog.rdbuf(old_rdbuf); } }

std::basic_string<char> as_string(std::basic_string<wchar_t> input)
{
	return std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>>().to_bytes(input);
}

std::basic_string<wchar_t> as_wstring(std::basic_string<char> input)
{
	return std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>>().from_bytes(input);
}

std::basic_string<char> GetLatestErrorMessage()
{
	auto latest_error_code{ ::GetLastError() };
	LPTSTR string_ptr{ nullptr };
	const DWORD nof_tchars = FormatMessage(
		FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_ALLOCATE_BUFFER,
		NULL,
		latest_error_code,
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		reinterpret_cast<LPTSTR>(&string_ptr),
		0,
		NULL);

	if (nof_tchars > 0)
	{
		auto deleter = [](void* p) { ::LocalFree(p); };
		std::unique_ptr<TCHAR, decltype(deleter)> string_ptr_buffer(string_ptr, deleter);
		return as_string(std::basic_string<TCHAR>(string_ptr_buffer.get(), nof_tchars));
	}
	else
	{
		auto error_code{ ::GetLastError() };
		throw std::system_error(error_code, std::system_category(), __HERE__);
	}
}

class ProcPtr {
public:
	ProcPtr(FARPROC ptr, const char* name = nullptr) : _ptr(ptr)
	{
		if (_ptr == nullptr)
		{
			__LOG(__ERROR__ + " " + name);
		}
	}

	template <typename T>
	operator T* () const {
		return reinterpret_cast<T*>(_ptr);
	}

private:
	FARPROC _ptr;
};

class DLL {
public:
	explicit DLL(const wchar_t* filename) : _module(LoadLibraryW(filename))
	{
		if (_module == 0)
		{
			__LOG(__ERROR__ + as_string(filename));
		}
	}

	~DLL()
	{
		if (FreeLibrary(_module) == 0)
		{
			try {
				__LOG(__ERROR__);
			}
			catch (std::exception ex)
			{
				__LOG(__HERE__);
			}
		}
	}

	ProcPtr operator[](const char* proc_name) const
	{
		return ProcPtr(GetProcAddress(_module, proc_name), proc_name);
	}

	static HMODULE _parent_module;
private:
	HMODULE _module;
};
#endif // !DLL_H
