#include "Common.h"

std::basic_string<char> as_string(std::basic_string<wchar_t> input)
{
	auto nof_required_bytes = WideCharToMultiByte(CP_UTF8,
												  WC_ERR_INVALID_CHARS,
												  input.c_str(),
												  input.size(),
												  nullptr,
												  0,
												  nullptr,
												  nullptr);
	if(nof_required_bytes == 0)
	{
		spdlog::error(__ERROR__);
		throw std::runtime_error(__ERROR__);
	}

	std::unique_ptr<char[]> buffer(new char[nof_required_bytes]());

	auto nof_written_bytes = WideCharToMultiByte(CP_UTF8,
												 WC_ERR_INVALID_CHARS,
												 input.c_str(),
												 input.size(),
												 buffer.get(),
												 nof_required_bytes,
												 nullptr,
												 nullptr);
	if(nof_written_bytes == 0)
	{
		spdlog::error(__ERROR__);
		throw std::runtime_error(__ERROR__);
	}

	return std::string(buffer.get(), nof_written_bytes);
};

std::basic_string<wchar_t> as_wstring(std::basic_string<char> input)
{
	auto nof_required_bytes = MultiByteToWideChar(CP_UTF8,
												  WC_ERR_INVALID_CHARS,
												  input.c_str(),
												  input.size(),
												  nullptr,
												  0);
	if(nof_required_bytes == 0)
	{
		spdlog::error(__ERROR__);
		throw std::runtime_error(__ERROR__);
	}

	std::unique_ptr<wchar_t[]> buffer(new wchar_t[nof_required_bytes]());

	auto nof_written_bytes = MultiByteToWideChar(CP_UTF8,
												 WC_ERR_INVALID_CHARS,
												 input.c_str(),
												 input.size(),
												 buffer.get(),
												 nof_required_bytes);
	if(nof_written_bytes == 0)
	{
		spdlog::error(__ERROR__);
		throw std::runtime_error(__ERROR__);
	}

	return std::wstring(buffer.get(), nof_written_bytes);
};