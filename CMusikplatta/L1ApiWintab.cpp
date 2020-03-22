#pragma once
#define NOWTCALLBACKS
#define NOWTFUNCTIONS
#include "L1ApiWintab.h"

#include "Common.h"
#include "DllPtr.h"
#include "L0ApiWintab.h"
auto Wintab32 = mp::DllPtr(L"Wintab32.dll");
#define AssignDllFunction(X) decltype(_##X)* X = Wintab32[#X]
namespace mp
{
AssignDllFunction(WTInfoA);
AssignDllFunction(WTInfoW);
AssignDllFunction(WTOpenA);
AssignDllFunction(WTOpenW);
AssignDllFunction(WTPacketsGet);
AssignDllFunction(WTClose);
AssignDllFunction(WTPacket);
AssignDllFunction(WTEnable);
AssignDllFunction(WTOverlap);
AssignDllFunction(WTConfig);
AssignDllFunction(WTGetA);
AssignDllFunction(WTGetW);
AssignDllFunction(WTSetA);
AssignDllFunction(WTSetW);
AssignDllFunction(WTExtGet);
AssignDllFunction(WTExtSet);
AssignDllFunction(WTSave);
AssignDllFunction(WTRestore);
AssignDllFunction(WTPacketsPeek);
AssignDllFunction(WTDataGet);
AssignDllFunction(WTDataPeek);
AssignDllFunction(WTQueueSizeGet);
AssignDllFunction(WTQueueSizeSet);
AssignDllFunction(WTMgrOpen);
AssignDllFunction(WTMgrClose);
AssignDllFunction(WTMgrContextEnum);
AssignDllFunction(WTMgrContextOwner);
AssignDllFunction(WTMgrDefContext);
AssignDllFunction(WTMgrDefContextEx);
AssignDllFunction(WTMgrDeviceConfig);
AssignDllFunction(WTMgrConfigReplace);
AssignDllFunction(WTMgrPacketHook);
AssignDllFunction(WTMgrPacketHookDefProc);
AssignDllFunction(WTMgrExt);
AssignDllFunction(WTMgrCsrEnable);
AssignDllFunction(WTMgrCsrButtonMap);
AssignDllFunction(WTMgrCsrPressureBtnMarks);
AssignDllFunction(WTMgrCsrPressureResponse);
AssignDllFunction(WTMgrCsrExt);
AssignDllFunction(WTQueuePacketsEx);
AssignDllFunction(WTMgrConfigReplaceExA);
AssignDllFunction(WTMgrConfigReplaceExW);
AssignDllFunction(WTMgrPacketHookExA);
AssignDllFunction(WTMgrPacketHookExW);
AssignDllFunction(WTMgrPacketUnhook);
AssignDllFunction(WTMgrPacketHookNext);
AssignDllFunction(WTMgrCsrPressureBtnMarksEx);
} // namespace mp
