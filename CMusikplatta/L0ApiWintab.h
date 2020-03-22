#pragma once
#include <Windows.h>

#define NOWTCALLBACKS
#define NOWTFUNCTIONS
#include "3pp/wintab/WINTAB.H"
#define PACKETDATA		 0xFFFF
#define PACKETMODE		 0xFFFF
#define PACKETEXPKEYS	 PKEXT_ABSOLUTE
#define PACKETTOUCHSTRIP PKEXT_ABSOLUTE
#define PACKETTOUCHRING	 PKEXT_ABSOLUTE
#include "3pp/wintab/PKTDEF.H"