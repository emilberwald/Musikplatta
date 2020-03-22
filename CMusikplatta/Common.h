#pragma once
#include <fstream>
#include <iostream>
#include <string>

#define MP_HERE		 (std::to_string(__LINE__) + ":" + __FILE__ + " " + __func__ + ":")
#define MP_HEREWIN32 (MP_HERE + mp::GetLatestErrorMessage())

namespace mp
{
std::basic_string<char> as_string(std::basic_string<wchar_t> input);

std::basic_string<wchar_t> as_wstring(std::basic_string<char> input);

std::basic_string<char> GetLatestErrorMessage();
} // namespace mp
