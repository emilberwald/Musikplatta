#pragma once
#include <fstream>
#include <iostream>
#include <spdlog/sinks/stdout_color_sinks.h>
#include <spdlog/spdlog.h>
#include <string>

#define MP_HERE		 (std::string(__FILE__) + " " + __func__ + "(" + std::to_string(__LINE__) + "):\t")
#define MP_HEREWIN32 (MP_HERE + mp::GetLatestErrorMessage())
#define MP_WARN_IF(condition)                                                                                          \
	if(condition) { spdlog::warn(MP_HERE + #condition); }
#define MP_ERROR_IF(condition)                                                                                          \
	if(condition) { spdlog::error(MP_HERE + #condition); }
#define MP_THROW_IF(condition, exception) MP_THROW_IF_WHAT(condition, exception, "")

#define MP_THROW_IF_WHAT(condition, exception, what)                                                                   \
	if(condition)                                                                                                      \
	{                                                                                                                  \
		spdlog::error(MP_HERE + #condition + what);                                                                    \
		throw exception(MP_HERE + #condition + what);                                                                  \
	}

namespace mp
{
std::basic_string<char> as_string(std::basic_string<char> input);

std::basic_string<wchar_t> as_wstring(std::basic_string<wchar_t> input);

std::basic_string<char> as_string(std::basic_string<wchar_t> input);

std::basic_string<wchar_t> as_wstring(std::basic_string<char> input);

std::basic_string<char> GetLatestErrorMessage();
} // namespace mp
