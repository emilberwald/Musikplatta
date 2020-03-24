#pragma once
#include <fmt/format.h>
#include <fstream>
#include <iostream>
#include <spdlog/sinks/stdout_color_sinks.h>
#include <spdlog/spdlog.h>
#include <string>

#define MP_HERE		 (std::string(__FILE__) + " " + __func__ + "(" + std::to_string(__LINE__) + "):\t")
#define MP_HEREWIN32 (MP_HERE + mp::GetLatestErrorMessage())
#define MP_WARN_IF(condition)                                                                                          \
	if(condition) { spdlog::warn(MP_HERE + #condition); }
#define MP_WARN_IF_WHAT(condition, ...)                                                                                \
	if(condition) { spdlog::warn(MP_HERE + #condition + ##__VA_ARGS__); }
#define MP_ERROR_IF(condition)                                                                                         \
	if(condition) { spdlog::error(MP_HERE + #condition); }
#define MP_ERROR_IF_WHAT(condition, ...)                                                                               \
	if(condition) { spdlog::error(MP_HERE + #condition + ##__VA_ARGS__); }
#define MP_THROW_IF(condition, exception) MP_THROW_IF_WHAT(condition, exception, "")

#define MP_THROW_IF_WHAT(condition, exception, ...)                                                                    \
	if(condition)                                                                                                      \
	{                                                                                                                  \
		spdlog::error(MP_HERE + #condition + fmt::format(##__VA_ARGS__));                                              \
		throw exception(MP_HERE + #condition + fmt::format(##__VA_ARGS__));                                            \
	}

namespace mp
{
std::basic_string<char> as_string(std::basic_string<char> input);

std::basic_string<wchar_t> as_wstring(std::basic_string<wchar_t> input);

std::basic_string<char> as_string(std::basic_string<wchar_t> input);

std::basic_string<wchar_t> as_wstring(std::basic_string<char> input);

std::basic_string<char> GetLatestErrorMessage();
} // namespace mp
