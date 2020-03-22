#pragma once
#include <map>
#include <string>
#include <tuple>
namespace mp
{
extern std::map<int, std::tuple<std::string, std::string>> explain_wm;
extern std::map<int, std::tuple<std::string, std::string>> explain_wm_fallback;
} // namespace mp