#pragma once
#include <fstream>
#include <iostream>
#include <spdlog/sinks/basic_file_sink.h>
#include <spdlog/spdlog.h>
#include <string>

#define __HERE__  (std::to_string(__LINE__) + ":" + __FILE__ + " " + __func__ + ":")
#define __ERROR__ (__HERE__ + GetLatestErrorMessage())
#define __LOG(WHAT)                                                                                \
	{                                                                                              \
		std::cerr << WHAT << std::endl;                                                            \
		std::ofstream out("Wintab.h");                                                             \
		if(out)                                                                                    \
		{                                                                                          \
			auto old_rdbuf = std::clog.rdbuf();                                                    \
			std::clog.rdbuf(out.rdbuf());                                                          \
			std::clog << WHAT << std::endl;                                                        \
			std::clog.rdbuf(old_rdbuf);                                                            \
		}                                                                                          \
	}

std::basic_string<char> as_string(std::basic_string<wchar_t> input);

std::basic_string<wchar_t> as_wstring(std::basic_string<char> input);

std::basic_string<char> GetLatestErrorMessage();
