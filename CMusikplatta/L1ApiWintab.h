#include "Common.h"
#include "DllPtr.h"

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

typedef BOOL(WINAPI *WTENUMPROC)(HCTX, LPARAM);
typedef BOOL(WINAPI *WTCONFIGPROC)(HCTX, HWND);
typedef LRESULT(WINAPI *WTHOOKPROC)(int, WPARAM, LPARAM);
typedef WTHOOKPROC FAR *LPWTHOOKPROC;

BOOL API	   _WTClose(HCTX);
BOOL API	   _WTConfig(HCTX, HWND);
BOOL API	   _WTEnable(HCTX, BOOL);
BOOL API	   _WTExtGet(HCTX, UINT, LPVOID);
BOOL API	   _WTExtSet(HCTX, UINT, LPVOID);
BOOL API	   _WTGet(HCTX, LPLOGCONTEXT);
BOOL API	   _WTGetA(HCTX, LPLOGCONTEXTA);
BOOL API	   _WTGetW(HCTX, LPLOGCONTEXTW);
BOOL API	   _WTMgrClose(HMGR);
BOOL API	   _WTMgrConfigReplace(HMGR, BOOL, WTCONFIGPROC);
BOOL API	   _WTMgrConfigReplaceEx(HMGR, BOOL, LPSTR, LPSTR);
BOOL API	   _WTMgrConfigReplaceExA(HMGR, BOOL, LPSTR, LPSTR);
BOOL API	   _WTMgrConfigReplaceExW(HMGR, BOOL, LPWSTR, LPSTR);
BOOL API	   _WTMgrContextEnum(HMGR, WTENUMPROC, LPARAM);
BOOL API	   _WTMgrCsrButtonMap(HMGR, UINT, LPBYTE, LPBYTE);
BOOL API	   _WTMgrCsrEnable(HMGR, UINT, BOOL);
BOOL API	   _WTMgrCsrExt(HMGR, UINT, UINT, LPVOID);
BOOL API	   _WTMgrCsrPressureBtnMarks(HMGR, UINT, DWORD, DWORD);
BOOL API	   _WTMgrCsrPressureBtnMarksEx(HMGR, UINT, UINT FAR *, UINT FAR *);
BOOL API	   _WTMgrCsrPressureResponse(HMGR, UINT, UINT FAR *, UINT FAR *);
BOOL API	   _WTMgrExt(HMGR, UINT, LPVOID);
BOOL API	   _WTMgrPacketUnhook(HWTHOOK);
BOOL API	   _WTOverlap(HCTX, BOOL);
BOOL API	   _WTPacket(HCTX, UINT, LPVOID);
BOOL API	   _WTQueuePacketsEx(HCTX, UINT FAR *, UINT FAR *);
BOOL API	   _WTQueueSizeSet(HCTX, int);
BOOL API	   _WTSave(HCTX, LPVOID);
BOOL API	   _WTSet(HCTX, LPLOGCONTEXT);
BOOL API	   _WTSetA(HCTX, LPLOGCONTEXTA);
BOOL API	   _WTSetW(HCTX, LPLOGCONTEXTW);
DWORD API	   _WTQueuePackets(HCTX);
HCTX API	   _WTMgrDefContext(HMGR, BOOL);
HCTX API	   _WTMgrDefContextEx(HMGR, UINT, BOOL); /* 1.1 */
HCTX API	   _WTOpen(HWND, LPLOGCONTEXT, BOOL);
HCTX API	   _WTOpenA(HWND, LPLOGCONTEXTA, BOOL);
HCTX API	   _WTOpenW(HWND, LPLOGCONTEXTW, BOOL);
HCTX API	   _WTRestore(HWND, LPVOID, BOOL);
HMGR API	   _WTMgrOpen(HWND, UINT);
HWND API	   _WTMgrContextOwner(HMGR, HCTX);
HWTHOOK API	   _WTMgrPacketHookEx(HMGR, int, LPSTR, LPSTR);
HWTHOOK API	   _WTMgrPacketHookExA(HMGR, int, LPSTR, LPSTR);
HWTHOOK API	   _WTMgrPacketHookExW(HMGR, int, LPWSTR, LPSTR);
int API		   _WTDataGet(HCTX, UINT, UINT, int, LPVOID, LPINT);
int API		   _WTDataPeek(HCTX, UINT, UINT, int, LPVOID, LPINT);
int API		   _WTPacketsGet(HCTX, int, LPVOID);
int API		   _WTPacketsPeek(HCTX, int, LPVOID);
int API		   _WTQueueSizeGet(HCTX);
LRESULT API	   _WTMgrPacketHookDefProc(int, WPARAM, LPARAM, LPWTHOOKPROC);
LRESULT API	   _WTMgrPacketHookNext(HWTHOOK, int, WPARAM, LPARAM);
UINT API	   _WTInfo(UINT, UINT, LPVOID);
UINT API	   _WTInfoA(UINT, UINT, LPVOID);
UINT API	   _WTInfoW(UINT, UINT, LPVOID);
UINT API	   _WTMgrDeviceConfig(HMGR, UINT, HWND);
WTHOOKPROC API _WTMgrPacketHook(HMGR, BOOL, int, WTHOOKPROC);

auto Wintab32 = DllPtr(L"Wintab32.dll");

#define AssignDllFunction(X) decltype(_##X) *X = Wintab32[#X]

// Wintab function returns global information about the interface in an
// application-sup­plied buffer. Different types of information are specified by
// different index argu­ments. Applications use this function
// to receive information about tablet coordi­nates, physical dimensions,
// capabilities, and cursor types.
AssignDllFunction(WTInfoA);
// Wintab function returns global information about the interface in an
// application-sup­plied buffer. Different types of information are specified by
// different index argu­ments. Applications use this function
// to receive information about tablet coordi­nates, physical dimensions,
// capabilities, and cursor types.
AssignDllFunction(WTInfoW);
// Wintab function establishes an active context on the tablet. On successful
// comple­tion of this function, the application may begin receiving tablet
// events via mes­sages (if they were requested), and may use
// the handle returned to poll the con­text, or to per­form other
// context-related functions.
AssignDllFunction(WTOpenA);
// Wintab function establishes an active context on the tablet. On successful
// comple­tion of this function, the application may begin receiving tablet
// events via mes­sages (if they were requested), and may use
// the handle returned to poll the con­text, or to per­form other
// context-related functions.
AssignDllFunction(WTOpenW);
// Wintab function copies the next cMaxPkts events from the packet queue of
// context hCtx to the passed lpPkts buffer and removes them
// from the queue.
AssignDllFunction(WTPacketsGet);
// Wintab function closes and destroys the tablet context object.
AssignDllFunction(WTClose);
// Wintab function fills in the passed lpPkt buffer with the context event packet
// having the specified serial number. The returned packet
// and any older packets are removed from the context's internal queue.
AssignDllFunction(WTPacket);
// Wintab function enables or disables a tablet context, temporarily turning on or
// off the processing of packets.
AssignDllFunction(WTEnable);
// Wintab function sends a tablet context to the top or bottom of the order of
// over­lapping tablet contexts.
AssignDllFunction(WTOverlap);
AssignDllFunction(WTConfig);
// Wintab function fills the passed structure with the current context attributes
// for the passed handle.
AssignDllFunction(WTGetA);
// Wintab function fills the passed structure with the current context attributes
// for the passed handle.
AssignDllFunction(WTGetW);
// Wintab function allows some of the context's attributes to be changed on the
// fly.
AssignDllFunction(WTSetA);
AssignDllFunction(WTSetW);
// Wintab function retrieves any context-specific data for an extension.
AssignDllFunction(WTExtGet);
// Wintab function sets any context-specific data for an extension.
AssignDllFunction(WTExtSet);
// Wintab function fills the passed buffer with binary save information that can
// be used to restore the equivalent context in a subsequent Windows session.
AssignDllFunction(WTSave);
// Wintab function creates a tablet context from save information returned from
// the WTSavefunction.
AssignDllFunction(WTRestore);
// Wintab function copies the next cMaxPkts events from the packet queue of
// context hCtx to the passed lpPkts buffer without removing
// them from the queue.
AssignDllFunction(WTPacketsPeek);
// Wintab function copies all packets with serial numbers between wBegin and wEnd
// in­clusive from the context's queue to the passed buffer
// and removes them from the queue.
AssignDllFunction(WTDataGet);
// Wintab function copies all packets with serial numbers between wBegin and wEnd
// in­clusive, from the context's queue to the passed buffer
// without removing them from the queue.
AssignDllFunction(WTDataPeek);
// Wintab function returns the number of packets the context's queue can hold.
AssignDllFunction(WTQueueSizeGet);
// Wintab function attempts to change the context's queue size to the value
// specified in nPkts.
AssignDllFunction(WTQueueSizeSet);
// Wintab function opens a tablet manager handle for use by tablet manager and
// con­figu­ration applications. Wintab handle is required to call the tablet
// management func­tions.
AssignDllFunction(WTMgrOpen);
// Wintab function closes a tablet manager handle. After this function returns,
// the passed manager handle is no longer valid.
AssignDllFunction(WTMgrClose);
// Wintab function enumerates all tablet context handles by passing the handle of
// each context, in turn, to the callback function pointed to by the lpEnumFunc
// pa­rameter.
AssignDllFunction(WTMgrContextEnum);
// Wintab function returns the handle of the window that owns a tablet context.
AssignDllFunction(WTMgrContextOwner);
// Wintab function retrieves a context handle for either the default system
// context or the default digitizing context. Wintab context is read-only.
AssignDllFunction(WTMgrDefContext);
// Wintab function retrieves a context handle that allows setting values for the
// default digit­izing or system context for a specified device. Wintab context is
// read-only.
AssignDllFunction(WTMgrDefContextEx);
AssignDllFunction(WTMgrDeviceConfig);
AssignDllFunction(WTMgrConfigReplace);
AssignDllFunction(WTMgrPacketHook);
AssignDllFunction(WTMgrPacketHookDefProc);
AssignDllFunction(WTMgrExt);
AssignDllFunction(WTMgrCsrEnable);
// Wintab function allows tablet managers to change the button mappings for each
// cursor type. Each cursor type has two button maps. The logical button map
// as­signs physi­cal buttons to logical buttons (applications only use logical
// buttons). The system cursor button map binds system-button
// functions to logical buttons.
AssignDllFunction(WTMgrCsrButtonMap);
// Wintab function allows tablet managers to change the pressure thresholds that
// control the mapping of pressure activity to button events.
AssignDllFunction(WTMgrCsrPressureBtnMarks);
// BOOL WTMgrCsrPressureResponse(hMgr, wCsr, lpNResp, lpTResp)
AssignDllFunction(WTMgrCsrPressureResponse);
AssignDllFunction(WTMgrCsrExt);
AssignDllFunction(WTQueuePacketsEx);
AssignDllFunction(WTMgrConfigReplaceExA);
AssignDllFunction(WTMgrConfigReplaceExW);
AssignDllFunction(WTMgrConfigReplaceEx);
AssignDllFunction(WTMgrPacketHookExA);
AssignDllFunction(WTMgrPacketHookExW);
AssignDllFunction(WTMgrPacketHookEx);
AssignDllFunction(WTMgrPacketUnhook);
AssignDllFunction(WTMgrPacketHookNext);
// Wintab function allows tablet managers to change the pressure thresholds that
// control the mapping of pressure activity to button events.
AssignDllFunction(WTMgrCsrPressureBtnMarksEx);
