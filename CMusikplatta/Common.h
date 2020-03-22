#pragma once
#include <fstream>
#include <iostream>
#include <spdlog/sinks/stdout_color_sinks.h>
#include <spdlog/spdlog.h>
#include <string>

#define MP_HERE		 (std::string(__FILE__) + " " + __func__ + "(" + std::to_string(__LINE__) + "):\t")
#define MP_HEREWIN32 (MP_HERE + mp::GetLatestErrorMessage())

namespace mp
{
std::basic_string<char> as_string(std::basic_string<wchar_t> input);

std::basic_string<wchar_t> as_wstring(std::basic_string<char> input);

std::basic_string<char> GetLatestErrorMessage();
} // namespace mp
