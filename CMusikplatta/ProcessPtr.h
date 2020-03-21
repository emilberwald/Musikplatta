#pragma once
#include "framework.h"

class ProcessPtr
{
  public:
	ProcessPtr(FARPROC ptr, const char *name = nullptr);

	template<typename T>
	operator T *() const
	{
		return reinterpret_cast<T *>(_ptr);
	}

  private:
	FARPROC _ptr;
};