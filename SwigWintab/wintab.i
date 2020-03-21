//============================
// compiler errors
%include <windows.i>
//============================//

//============================
// compiler error fixes
#undef NEAR
#define NEAR
#undef FAR
#define FAR
#define WIN32
#define DECLARE_HANDLE(name) 
//============================//


//============================
// structs, function definitions
%include "include\WINTAB.H"
//============================//
//============================
// to gett packet structs with all bells and whistles
#define PACKETDATA 0xFFFF
#define PACKETMODE 0xFFFF
#define PACKETEXPKEYS PKEXT_ABSOLUTE
#define PACKETTOUCHSTRIP PKEXT_ABSOLUTE
#define PACKETTOUCHRING PKEXT_ABSOLUTE
%include "include\PKTDEF.H"
//============================//

%{
#include "Wintab-getprocaddress.h"
%}
