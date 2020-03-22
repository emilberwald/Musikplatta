#pragma once
#define NOWTCALLBACKS
#define NOWTFUNCTIONS
#include "Common.h"
#include "DllPtr.h"
#include "L0ApiWintab.h"
#include "L1ApiWintab.h"

typedef BOOL(WINAPI* WTENUMPROC)(HCTX, LPARAM);
typedef BOOL(WINAPI* WTCONFIGPROC)(HCTX, HWND);
typedef LRESULT(WINAPI* WTHOOKPROC)(int, WPARAM, LPARAM);
typedef WTHOOKPROC FAR* LPWTHOOKPROC;
BOOL API				_WTClose(HCTX);
BOOL API				_WTConfig(HCTX, HWND);
BOOL API				_WTEnable(HCTX, BOOL);
BOOL API				_WTExtGet(HCTX, UINT, LPVOID);
BOOL API				_WTExtSet(HCTX, UINT, LPVOID);
BOOL API				_WTGet(HCTX, LPLOGCONTEXT);
BOOL API				_WTGetA(HCTX, LPLOGCONTEXTA);
BOOL API				_WTGetW(HCTX, LPLOGCONTEXTW);
BOOL API				_WTMgrClose(HMGR);
BOOL API				_WTMgrConfigReplace(HMGR, BOOL, WTCONFIGPROC);
BOOL API				_WTMgrConfigReplaceEx(HMGR, BOOL, LPSTR, LPSTR);
BOOL API				_WTMgrConfigReplaceExA(HMGR, BOOL, LPSTR, LPSTR);
BOOL API				_WTMgrConfigReplaceExW(HMGR, BOOL, LPWSTR, LPSTR);
BOOL API				_WTMgrContextEnum(HMGR, WTENUMPROC, LPARAM);
BOOL API				_WTMgrCsrButtonMap(HMGR, UINT, LPBYTE, LPBYTE);
BOOL API				_WTMgrCsrEnable(HMGR, UINT, BOOL);
BOOL API				_WTMgrCsrExt(HMGR, UINT, UINT, LPVOID);
BOOL API				_WTMgrCsrPressureBtnMarks(HMGR, UINT, DWORD, DWORD);
BOOL API				_WTMgrCsrPressureBtnMarksEx(HMGR, UINT, UINT FAR*, UINT FAR*);
BOOL API				_WTMgrCsrPressureResponse(HMGR, UINT, UINT FAR*, UINT FAR*);
BOOL API				_WTMgrExt(HMGR, UINT, LPVOID);
BOOL API				_WTMgrPacketUnhook(HWTHOOK);
BOOL API				_WTOverlap(HCTX, BOOL);
BOOL API				_WTPacket(HCTX, UINT, LPVOID);
BOOL API				_WTQueuePacketsEx(HCTX, UINT FAR*, UINT FAR*);
BOOL API				_WTQueueSizeSet(HCTX, int);
BOOL API				_WTSave(HCTX, LPVOID);
BOOL API				_WTSet(HCTX, LPLOGCONTEXT);
BOOL API				_WTSetA(HCTX, LPLOGCONTEXTA);
BOOL API				_WTSetW(HCTX, LPLOGCONTEXTW);
DWORD API				_WTQueuePackets(HCTX);
HCTX API				_WTMgrDefContext(HMGR, BOOL);
HCTX API				_WTMgrDefContextEx(HMGR, UINT, BOOL);
////* 1.1 */
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

#define ExternDllFunction(X) extern decltype(_##X)* X;
namespace mp
{
/// <summary>
////Syntax
////UINT WTInfo(wCategory, nIndex, lpOutput)
////This function returns global information about the interface in an application-sup­plied buffer. Different types of information are specified by different index argu­ments. Applications use this function to receive information about tablet coordi­nates, physical dimensions, capabilities, and cursor types.
////Parameter
////Type
////Description
////wCategory
////UINT Identifies the category from which information is being re­quested.
////nIndex
////UINT Identifies which information is being requested from within the category.
////lpOutput
////LPVOID Points to a buffer to hold the requested information.
////Return Value
////The return value specifies the size of the returned information in bytes. If the infor­mation is not supported, the function returns zero. If a tablet is not physi­cally pres­ent, this function always returns zero.
////Comments
////Several important categories of information are available through this function. First, the function provides identification information, including specification and software version numbers, and tablet vendor and model information. Sec­ond, the function provides general capability information, including dimensions, resolutions, optional features, and cursor types. Third, the function provides categories that give defaults for all tablet context attributes. Finally, the func­tion may provide any other implementation- or vendor-specific information cat­egories necessary.
////The information returned by this function is subject to change during a Win­dows session. Applications cannot change the information returned here, but tablet man­ager applications or hardware changes or errors can. Applications can respond to information changes by fielding the WT_INFOCHANGE message. The parameters of the message indicate which information has changed.
////If the wCategory argument is zero, the function copies no data to the output buffer, but returns the size in bytes of the buffer necessary to hold the largest complete category. If the nIndex argument is zero, the function returns all of the information entries in the category in a single data structure.
////If the lpOutput argument is NULL, the function just returns the required buffer size.
/// </summary>
ExternDllFunction(WTInfoA);
/// <summary>
////Syntax
////UINT WTInfo(wCategory, nIndex, lpOutput)
////This function returns global information about the interface in an application-sup­plied buffer. Different types of information are specified by different index argu­ments. Applications use this function to receive information about tablet coordi­nates, physical dimensions, capabilities, and cursor types.
////Parameter
////Type
////Description
////wCategory
////UINT Identifies the category from which information is being re­quested.
////nIndex
////UINT Identifies which information is being requested from within the category.
////lpOutput
////LPVOID Points to a buffer to hold the requested information.
////Return Value
////The return value specifies the size of the returned information in bytes. If the infor­mation is not supported, the function returns zero. If a tablet is not physi­cally pres­ent, this function always returns zero.
////Comments
////Several important categories of information are available through this function. First, the function provides identification information, including specification and software version numbers, and tablet vendor and model information. Sec­ond, the function provides general capability information, including dimensions, resolutions, optional features, and cursor types. Third, the function provides categories that give defaults for all tablet context attributes. Finally, the func­tion may provide any other implementation- or vendor-specific information cat­egories necessary.
////The information returned by this function is subject to change during a Win­dows session. Applications cannot change the information returned here, but tablet man­ager applications or hardware changes or errors can. Applications can respond to information changes by fielding the WT_INFOCHANGE message. The parameters of the message indicate which information has changed.
////If the wCategory argument is zero, the function copies no data to the output buffer, but returns the size in bytes of the buffer necessary to hold the largest complete category. If the nIndex argument is zero, the function returns all of the information entries in the category in a single data structure.
////If the lpOutput argument is NULL, the function just returns the required buffer size.
/// </summary>
ExternDllFunction(WTInfoW);
/// <summary>
////Syntax
////HCTX WTOpen(hWnd, lpLogCtx, fEnable)
////This function establishes an active context on the tablet. On successful comple­tion of this function, the application may begin receiving tablet events via mes­sages (if they were requested), and may use the handle returned to poll the con­text, or to per­form other context-related functions.
////Parameter
////Type
////Description
////hWnd
////HWND
////Identifies the window that owns the tablet context, and receives messages from the context.
////lpLogCtx
////LPLOGCONTEXT
////Points to an application-provided LOGCONTEXT data structure describing the context to be opened.
////fEnable
////BOOL
////Specifies whether the new context will immediately begin processing input data.
////Return Value
////The return value
////identifies the new context. It is NULL if the context is not opened.
////Comments
////Opening a new context allows the application to receive tablet input or creates a context that controls the system cursor or Pen Windows pen. The owning window (and all manager windows) will immediately receive a WT_CTXOPEN message when the context has been opened.
////If the fEnable argument is zero, the context will be created, but will not process input. The context can be enabled using the WTEnable function.
////If tablet event messages were requested in the context specification, the owning window will receive them. The application can control the message numbers used the lcMsgBase field of the LOGCONTEXT structure.
////The window that owns the new context will receive context and information change messages even if event messages were not requested. It is not necessary to handle these in many cases, but some applications may wish to do so.
////The newly opened tablet context will be placed on the top of the context overlap or­der.
////Invalid or out-of-range attribute values in the logical context structure will ei­ther be validated, or cause the open to fail, depending on the attributes involved. Upon a successful return from the function, the context specification pointed to by lpLogCtx will contain the validated values.
/// </summary>
ExternDllFunction(WTOpenA);
/// <summary>
////Syntax
////HCTX WTOpen(hWnd, lpLogCtx, fEnable)
////This function establishes an active context on the tablet. On successful comple­tion of this function, the application may begin receiving tablet events via mes­sages (if they were requested), and may use the handle returned to poll the con­text, or to per­form other context-related functions.
////Parameter
////Type
////Description
////hWnd
////HWND
////Identifies the window that owns the tablet context, and receives messages from the context.
////lpLogCtx
////LPLOGCONTEXT
////Points to an application-provided LOGCONTEXT data structure describing the context to be opened.
////fEnable
////BOOL
////Specifies whether the new context will immediately begin processing input data.
////Return Value
////The return value
////identifies the new context. It is NULL if the context is not opened.
////Comments
////Opening a new context allows the application to receive tablet input or creates a context that controls the system cursor or Pen Windows pen. The owning window (and all manager windows) will immediately receive a WT_CTXOPEN message when the context has been opened.
////If the fEnable argument is zero, the context will be created, but will not process input. The context can be enabled using the WTEnable function.
////If tablet event messages were requested in the context specification, the owning window will receive them. The application can control the message numbers used the lcMsgBase field of the LOGCONTEXT structure.
////The window that owns the new context will receive context and information change messages even if event messages were not requested. It is not necessary to handle these in many cases, but some applications may wish to do so.
////The newly opened tablet context will be placed on the top of the context overlap or­der.
////Invalid or out-of-range attribute values in the logical context structure will ei­ther be validated, or cause the open to fail, depending on the attributes involved. Upon a successful return from the function, the context specification pointed to by lpLogCtx will contain the validated values.
/// </summary>
ExternDllFunction(WTOpenW);
/// <summary>
////Syntax
////int WTPacketsGet(hCtx, cMaxPkts, lpPkts)
////This function copies the next cMaxPkts events from the packet queue of context hCtx to the passed lpPkts buffer and removes them from the queue.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose packets are being returned.
////cMaxPkts
////int Specifies the maximum number of packets to return.
////lpPkts
////LPVOID Points to a buffer to
////receive the event packets.
////Return Value
////The return value is the number of packets copied in the buffer.
////Comments
////The exact structure of the returned packet is determined by the packet infor­mation that was requested when the context was opened.
////The buffer pointed to by lpPkts must be at least cMaxPkts * sizeof(PACKET) bytes long to prevent overflow.
////Applications may flush packets from the queue by calling this function with a NULL lpPktargument.
/// </summary>
ExternDllFunction(WTPacketsGet);
/// <summary>
////Syntax
////BOOL WTClose(hCtx)
////This function closes and destroys the tablet context object.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context to be closed.
////Return Value
////The function returns a non-zero value if the context was valid and was destroyed. Otherwise, it returns zero.
////Comments
////After a call to this function, the passed handle is no longer valid. The owning win­dow (and all manager windows) will receive a WT_CTXCLOSE message when the context has been closed.
/// </summary>
ExternDllFunction(WTClose);
/// <summary>
////Syntax
////BOOL WTPacket(hCtx, wSerial, lpPkt)
////This function fills in the passed lpPkt buffer with the context event packet having the specified serial number. The returned packet and any older packets are removed from the context's internal queue.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose packets are being returned.
////wSerial
////UINT Serial number of the tablet event to return.
////lpPkt
////LPVOID Points to a buffer to receive the event packet.
////Return Value
////The return value is non-zero if the specified packet was found and returned. It is zero if the specified packet was not found in the queue.
////Comments
////The exact structure of the returned packet is determined by the packet infor­mation that was requested when the context was opened.
////The buffer pointed to by lpPkts must be at least sizeof(PACKET) bytes long to pre­vent overflow.
////Applications may flush packets from the queue by calling this function with a NULL lpPktsargument.
/// </summary>
ExternDllFunction(WTPacket);
/// <summary>
////Syntax
////BOOL WTEnable(hCtx, fEnable)
////This function enables or disables a tablet context, temporarily turning on or off the processing of packets.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context to be enabled or disabled.
////fEnable
////BOOL Specifies enabling if non-zero, disabling if zero.
////Return Value
////The function returns a non-zero value if the enable or disable request was satis­fied, zero otherwise.
////Comments
////Calls to this function to enable an already enabled context, or to disable an al­ready disabled context will return a non-zero value, but otherwise do nothing.
////The context’s packet queue is flushed on disable.
////Applications can determine whether a context is currently enabled by using the WTGetfunction and examining the lcStatus field of the LOGCONTEXT struc­ture.
/// </summary>
ExternDllFunction(WTEnable);
/// <summary>
////Syntax
////BOOL WTOverlap(hCtx, fToTop)
////This function sends a tablet context to the top or bottom of the order of over­lapping tablet contexts.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context to move within the overlap order.
////fToTop
////BOOL Specifies sending the context to the top of the overlap or­der if non-zero, or to the bottom if zero.
////Return Value
////The function returns non-zero if successful, zero otherwise.
////Comments
////Tablet contexts' input areas are allowed to overlap. The tablet interface main­tains an overlap order that helps determine which context will process a given event. The topmost context in the overlap order whose input context encom­passes the event, and whose event masks select the event will process the event.
////This function is useful for getting access to input events when the application's con­text is overlapped by other contexts.
////The function will fail only if the context argument is invalid.
/// </summary>
ExternDllFunction(WTOverlap);
/// <summary>
/// </summary>
ExternDllFunction(WTConfig);
/// <summary>
////Syntax
////BOOL WTGet(hCtx, lpLogCtx)
////This function fills the passed structure with the current context attributes for the passed handle.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose attributes are to be copied.
////lpLogCtx
////LPLOGCONTEXT Points to a LOGCONTEXT data structure to which the context attributes are to be copied.
////Return Value
////The function returns a non-zero value if the data is retrieved successfully. Oth­er­wise, it returns zero.
/// </summary>
ExternDllFunction(WTGetA);
/// <summary>
////Syntax
////BOOL WTGet(hCtx, lpLogCtx)
////This function fills the passed structure with the current context attributes for the passed handle.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose attributes are to be copied.
////lpLogCtx
////LPLOGCONTEXT Points to a LOGCONTEXT data structure to which the context attributes are to be copied.
////Return Value
////The function returns a non-zero value if the data is retrieved successfully. Oth­er­wise, it returns zero.
/// </summary>
ExternDllFunction(WTGetW);
/// <summary>
////Syntax
////BOOL WTSet(hCtx, lpLogCtx)
////This function allows some of the context's attributes to be changed on the fly.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose attributes are being changed.
////lpLogCtx
////LPLOGCONTEXT Points to a LOGCONTEXT data structure containing the new context attributes.
////Return Value
////The function returns a non-zero value if the context was changed to match the passed context specification; it returns zero if any of the requested changes could not be made.
////Comments
////If this function is called by the task or process that owns the context, any context attribute may be changed. Otherwise, the function can change attributes that do not affect the format or meaning of the context's event packets and that were not speci­fied as locked when the context was opened. Context lock values can only be changed by the context’s owner.
////1.1: If the hCtx argument is a default context handle returned from WTMgrDef­Context or WTMgrDefContextEx, and the lpLogCtx argument is WTP_LPDEFAULT, the default context will be reset to its initial factory default values.
/// </summary>
ExternDllFunction(WTSetA);
/// <summary>
////Syntax
////BOOL WTSet(hCtx, lpLogCtx)
////This function allows some of the context's attributes to be changed on the fly.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose attributes are being changed.
////lpLogCtx
////LPLOGCONTEXT Points to a LOGCONTEXT data structure containing the new context attributes.
////Return Value
////The function returns a non-zero value if the context was changed to match the passed context specification; it returns zero if any of the requested changes could not be made.
////Comments
////If this function is called by the task or process that owns the context, any context attribute may be changed. Otherwise, the function can change attributes that do not affect the format or meaning of the context's event packets and that were not speci­fied as locked when the context was opened. Context lock values can only be changed by the context’s owner.
////1.1: If the hCtx argument is a default context handle returned from WTMgrDef­Context or WTMgrDefContextEx, and the lpLogCtx argument is WTP_LPDEFAULT, the default context will be reset to its initial factory default values.
/// </summary>
ExternDllFunction(WTSetW);
/// <summary>
////Syntax
////BOOL WTExtGet(hCtx, wExt, lpData)
////This function retrieves any context-specific data for an extension.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose extension attributes are being retrieved.
////wExt
////UINT Identifies the extension tag for which context-specific data is being retrieved.
////lpData
////LPVOID Points to a buffer to hold the retrieved data.
////Return Value
////The function returns a non-zero value if the data is retrieved successfully. Oth­er­wise, it returns zero
/// </summary>
ExternDllFunction(WTExtGet);
/// <summary>
////Syntax
////BOOL WTExtSet(hCtx, wExt, lpData)
////This function sets any context-specific data for an extension.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose extension attributes are being modified.
////wExt
////UINT Identifies the extension tag for which context-specific data is being modified.
////lpData
////LPVOID Points to the new data.
////Return Value
////The function returns a non-zero value if the data is modified successfully. Oth­er­wise, it returns zero.
////Comments
////Extensions may forbid their context-specific data to be changed during the life­time of a context. For such extensions, calls to this function would always fail.
////Extensions may also limit context data editing to the task of the owning window, as with the context locks.
/// </summary>
ExternDllFunction(WTExtSet);
/// <summary>
////Syntax
////BOOL WTSave(hCtx, lpSaveInfo)
////This function fills the passed buffer with binary save information that can be used to restore the equivalent context in a subsequent Windows session.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context that is being saved.
////lpSaveInfo
////LPVOID Points to a buffer to contain the save information.
////Return Value
////The function
////returns non-zero if the save information is successfully retrieved. Oth­erwise, it returns zero.
////Comments
////The size of the save information buffer can be determined by calling the WTInfo function with category WTI_INTERFACE, index IFC_CTXSAVESIZE.
////The save information is returned in a private binary data format. Applications should store the information unmodified and recreate the context by passing the save information to the WTRestore function.
////Using WTSave and WTRestore allows applications to easily save and restore ex­tension data bound to contexts.
/// </summary>
ExternDllFunction(WTSave);
/// <summary>
////Syntax
////HCTX WTRestore(hWnd, lpSaveInfo, fEnable)
////This function creates a tablet context from save information returned from the WTSavefunction.
////Parameter
////Type
////Description
////hWnd
////HWND Identifies the window that owns the tablet context, and receives messages from the context.
////lpSaveInfo
////LPVOID Points to a buffer containing save information.
////fEnable
////BOOL Specifies whether the new context will immediately begin processing input data.
////Return Value
////The function returns a valid context handle if successful. If a context equivalent to the save information could not be created, the function returns NULL.
////Comments
////The save information is in a private binary data format.
////Applications should only pass save information retrieved by the WTSave function.
////This function is much like WTOpen, except that it uses save in­formation for input instead of a logical context. In particular, it will generate a WT_CTXOPEN mes­sage for the new context.
/// </summary>
ExternDllFunction(WTRestore);
/// <summary>
////Syntax
////int WTPacketsPeek(hCtx, cMaxPkts, lpPkts)
////This function copies the next cMaxPkts events from the packet queue of context hCtx to the passed lpPkts buffer without removing them from the queue.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose packets are being read.
////cMaxPkts
////int Specifies the maximum number of packets to return.
////lpPkts
////LPVOID Points to a buffer
////to receive the event packets.
////Return Value
////The return value is the number of packets copied in the buffer.
////Comments
////The buffer pointed to by lpPkts must be at least cMaxPkts * sizeof(PACKET) bytes long to prevent overflow.
/// </summary>
ExternDllFunction(WTPacketsPeek);
/// <summary>
////Syntax
////int WTDataGet(hCtx, wBegin, wEnd, cMaxPkts, lpPkts, lpNPkts)
////This function copies all packets with serial numbers between wBegin and wEnd in­clusive from the context's queue to the passed buffer and removes them from the queue.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose packets are being returned.
////wBegin
////UINT Serial number of the oldest tablet event to return.
////wEnd
////UINT
////Serial number of the newest tablet event to return.
////cMaxPkts
////int Specifies the maximum number of packets to return.
////lpPkts
////LPVOID Points to a buffer to receive the event packets.
////lpNPkts
////LPINT Points to an integer to receive the number of packets ac­tually copied.
////Return Value
////The return value is the total number of packets found in the queue between wBegin and wEnd.
////Comments
////The buffer pointed to by lpPkts must be at least cMaxPkts * sizeof(PACKET) bytes long to prevent overflow.
/// </summary>
ExternDllFunction(WTDataGet);
/// <summary>
////Syntax
////int WTDataPeek(hCtx, wBegin, wEnd, cMaxPkts, lpPkts, lpNPkts)
////This function copies all packets with serial numbers between wBegin and wEnd in­clusive, from the context's queue to the passed buffer without removing them from the queue.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the
////context whose packets are being read.
////wBegin
////UINT Serial number of the oldest tablet event to return.
////wEnd
////UINT
////Serial number of the newest tablet event to return.
////cMaxPkts
////int Specifies the maximum number of packets to return.
////lpPkts
////LPVOID Points to a buffer to receive the event packets.
////lpNPkts
////LPINT Points to an integer to receive the number of packets ac­tually copied.
////Return Value
////The return value is the total number of packets found in the queue between wBegin and wEnd.
////Comments
////The buffer pointed to by lpPkts must be at least cMaxPkts * sizeof(PACKET) bytes long to prevent overflow.
/// </summary>
ExternDllFunction(WTDataPeek);
/// <summary>
////Syntax
////int WTQueueSizeGet(hCtx)
////This function returns the number of packets the context's queue can hold.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose queue size is being re­turned.
////Return Value
////The return value is the number of packet the queue can hold.
/// </summary>
ExternDllFunction(WTQueueSizeGet);
/// <summary>
////Syntax
////BOOL WTQueueSizeSet(hCtx, nPkts)
////This function attempts to change the context's queue size to the value specified in nPkts.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose queue size is being set.
////nPkts
////int Specifies the requested queue size.
////Return Value
////The return value is non-zero if the queue size was successfully changed. Other­wise, it is zero.
////Comments
////If the return value is zero, the context has no queue because the function deletes the original queue before attempting to create a new one. The application must continue calling the function with a smaller queue size until the function returns a non-zero value.
/// </summary>
ExternDllFunction(WTQueueSizeSet);
/// <summary>
////Syntax
////HMGR WTMgrOpen(hWnd, wMsgBase)
////This function opens a tablet manager handle for use by tablet manager and con­figu­ration applications. This handle is required to call the tablet management func­tions.
////Parameter
////Type
////Description
////hWnd
////HWND Identifies the window which owns the manager handle.
////wMsgBase
////UINT Specifies the message base number to use when notifying the manager window.
////Return Value
////The function returns a manager handle if successful, otherwise it returns NULL.
////Comments
////While the manager handle is open, the manager window will receive context mes­sages from all tablet contexts.
////Manager windows also receive information change messages.
////The number of manager handles available is interface implementation-dependent, and can be determined by calling the WTInfo function with category WTI_INTERFACE and index IFC_NMANAGERS.
/// </summary>
ExternDllFunction(WTMgrOpen);
/// <summary>
////Syntax
////BOOL WTMgrClose(hMgr)
////This function closes a tablet manager handle. After this function returns, the passed manager handle is no longer valid.
////Parameter
////Type
////Description
////hMgr
////HMGR Identifies the manager handle to close.
////Return Value
////The function returns non-zero if the handle was valid; otherwise, it returns zero.
/// </summary>
ExternDllFunction(WTMgrClose);
/// <summary>
////Syntax
////BOOL WTMgrContextEnum(hMgr, lpEnumFunc, lParam)
////This function enumerates all tablet context handles by passing the handle of each context, in turn, to the callback
////function pointed to by the lpEnumFunc pa­rameter.
////The enumeration terminates when the callback function returns zero.
////Parameter
////Type
////Description
////hMgr
////HMGR Is the valid manager handle that identifies the caller as a manager application.
////lpEnumFunc
////WTENUMPROC Is the procedure-instance address of the call­back function. See the following
///"Comments" section for details.
////lParam
////LPARAM Specifies the value to be passed to the callback func­tion for the application's use.
////Return Value
////The return value specifies the outcome of the function. It is non-zero if all con­texts have been enumerated. Otherwise, it is zero.
////Comments
////The address passed as the lpEnumFunc parameter must be created by using the MakeProcInstance function.
////The callback function must have attributes equivalent to WINAPI.
////The callback function must have the following form:
////Callback
////BOOL WINAPI EnumFunc(hCtx, lParam)
////HCTX hCtx;
////LPARAM
////lParam;
////EnumFunc is a place holder for the application-supplied function name. The actual name must be exported by including it in an EXPORTS statement in the applica­tion's module-definition file.
////Parameter
////Description
////hCtx
////Identifies the context.
////lParam
////Specifies the 32-bit argument of the WTMgrContextEnum func­tion.
////Return Value
////The function must return a non-zero value to continue enumeration, or zero to stop it.
/// </summary>
ExternDllFunction(WTMgrContextEnum);
/// <summary>
////Syntax
////HWND WTMgrContextOwner(hMgr, hCtx)
////This function returns the handle of the window that owns a tablet context.
////Parameter
////Type
////Description
////hMgr
////HMGR Is the valid manager handle that identifies the caller as a manager application.
////hCtx
////HCTX Identifies the context whose owner is to be returned.
////Return Value
////The function returns the context owner's window handle if the passed arguments are valid. Otherwise, it returns
////NULL.
////Comments
////This function allows the tablet manager to coordinate tablet context manage­ment with the states of the context-owning windows.
/// </summary>
ExternDllFunction(WTMgrContextOwner);
/// <summary>
////Syntax
////HCTX WTMgrDefContext(hMgr, fSystem) (1.4 modified)
////This function retrieves a context handle for either the default system context or the default digitizing context.
////This context is read-only.
////Parameter
////Type
////Description
////hMgr
////HMGR Is the valid manager handle that identifies the caller as a manager application.
////fSystem
////BOOL Specifies retrieval of the default system context if non-zero, or the default digitizing context if zero.
////Return Value
////The return value is the context handle for the specified default context, or NULL if the arguments were invalid.
////Comments
////The default digitizing context is the context whose attributes are returned by the WTInfofunction WTI_DEFCONTEXT category. The default system context is the context whose attributes are returned by the WTInfo function WTI_DEFSYSCTX category.
////If any tablet is connected to the system, then the HCTX value returned by this function , when used with WTGet(), will return a LOGCONTEXT with lcDevice = (UINT)(-1), which represents the so-called "virtual device". A virtual device (or "virtual tablet") accepts data from any connected tablet and sends it to the application.
/// </summary>
ExternDllFunction(WTMgrDefContext);
/// <summary>
////Syntax
////HCTX WTMgrDefContextEx(hMgr, wDevice, fSystem)
////This function retrieves a context handle that allows setting values for the default digit­izing or system context for a specified device. This context is read-only.
////Parameter
////Type
////Description
////hMgr
////HMGR Is the valid manager handle that identifies the caller as a manager application.
////wDevice
////UINT Specifies the device for which a default context handle will be returned.
////fSystem
////BOOL Specifies retrieval of the default system context if non-zero, or the default digitizing context if zero.
////Return Value
////The return value is the context handle for the specified default context, or NULL if the arguments were invalid.
////Comments
////The default digitizing contexts are contexts whose attributes are returned by the WTInfofunction WTI_DDCTXS multiplexed category. The default system con­texts are contexts whose attributes are returned by the WTInfo function WTI_DSCTXS multiplexed category.
////If wDevice = (UINT)(-1), which represents the so-called "virtual device", then the HCTX value returned by this function , when used with WTGet(), will return a LOGCONTEXT with lcDevice = (UINT)(-1). A virtual device (or "virtual tablet") accepts data from any connected tablet and sends it to the application.
/// </summary>
ExternDllFunction(WTMgrDefContextEx);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrDeviceConfig);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrConfigReplace);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrPacketHook);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrPacketHookDefProc);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrExt);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrCsrEnable);
/// <summary>
////Syntax
////BOOL WTMgrCsrButtonMap(hMgr, wCursor, lpLogBtns, lpSysBtns)
////This function allows tablet managers to change the button mappings for each cursor type. Each cursor type has two button maps. The logical button map as­signs physi­cal buttons to logical buttons (applications only use logical buttons). The system cursor button map binds system-button functions to logical buttons.
////Parameter
////Type
////Description
////hMgr
////HMGR Is the valid manager handle that identifies the caller as a manager application.
////wCursor
////UINT Specifies the zero-based cursor id of the cursor type whose button maps are being set.
////lpLogBtns
////LPBYTE Points to a 32 byte array of logical button numbers, one for each physical button. If the value WTP_LPDEFAULT is specified, the function resets the default value for the logical but­ton map.
////lpSysBtns
////LPBYTE Points to a 32 byte array of button action codes, one for each logical button. If the value WTP_LPDEFAULT is speci­fied, the function resets the default value for the system button map.
////Return Value
////The return value is non-zero if the new settings have taken effect. Otherwise, it is zero.
////Comments
////The function will allow two or more physical buttons to be assigned to one logi­cal button. Likewise, it does not ensure that left and right system button events can be properly generated. It is up to the tablet manager to make sure that ap­propriate limi­tations are applied to the logical button map and that the system buttons are not left disabled.
/// </summary>
ExternDllFunction(WTMgrCsrButtonMap);
/// <summary>
////Syntax
////BOOL WTMgrCsrPressureBtnMarks(hMgr, wCsr, dwNMarks, dwTMarks)
////This function allows tablet managers to change the pressure thresholds that control the mapping of pressure activity to button events.
////Parameter
////Type
////Description
////hMgr
////HMGR Is the valid manager handle that identifies the caller as a manager application.
////wCsr
////UINT Specifies the zero-based cursor id of the cursor type whose pressure button marks are being set.
////dwNMarks
////DWORD Specifies the button marks for the normal pressure but­ton. The low order word contains the release mark; the high order word contains the press mark. If the value WTP_DWDEFAULT is specified, the function resets the default value for the marks.
////dwTMarks
////DWORD Specifies the button marks for the tangent pressure but­ton. The low order word contains the release mark; the high order word contains the press mark. If the value WTP_DWDEFAULT is specified, the function resets the default value for the marks.
////Return Value
////The return value is non-zero if the new settings have taken effect. Otherwise, it is zero.
////Comments
////This function is non-portable and is superseded by WTMgrCsrPressureBtnMarksEx.
/// </summary>
ExternDllFunction(WTMgrCsrPressureBtnMarks);
/// <summary>
////Syntax
////BOOL WTMgrCsrPressureResponse(hMgr, wCsr, lpNResp, lpTResp)
////This function allows tablet managers to tune the pressure response for each cursor type.
////Parameter
////Type
////Description
////hMgr
////HMGR Is the valid manager handle that identifies the caller as a manager application.
////wCsr
////UINT Specifies the zero-based cursor id of the cursor whose pressure response curves are being set.
////lpNResp
////UINT FAR * Points to an array of UINTs describing the pressure response curve for normal pressure. If the value WTP_LPDEFAULTis specified, the function resets the default value for the response curve.
////lpTResp
////UINT FAR * Points to an array of UINTs describing the pressure response curve for tangential pressure. If the value WTP_LPDEFAULT is specified, the function resets the default value for the response curve.
////Return Value
////The return value is non-zero if the new settings have taken effect. Otherwise, it is zero.
////Comments
////The pressure response curves represent a transfer function mapping physical pres­sure values to logical pressure values (applications see only logical pressure values). Each array element contains a logical pressure value corresponding to some physi­cal pressure value. The array indices are "stretched" evenly over the input range.
////The number of entries in the arrays depends upon the range of physical pressure values. The number will be the number of physical values or 256, whichever is smaller. When there is an entry for each possible value, the value is used as an in­dex to the array, and the corresponding array element is returned. When there are more values that array elements, the physical value is mapped to a 0-255 scale. The result is used as the index to look up the logical pressure value.
/// </summary>
ExternDllFunction(WTMgrCsrPressureResponse);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrCsrExt);
/// <summary>
////Syntax
////BOOL WTQueuePacketsEx(hCtx, lpOld, lpNew)
////This function returns the serial numbers of the oldest and newest packets cur­rently in the queue.
////Parameter
////Type
////Description
////hCtx
////HCTX Identifies the context whose queue is being queried.
////lpOld
////UINT FAR * Points to an unsigned integer to receive the oldest packet's serial number.
////lpNew
////UINT FAR * Points to an unsigned integer to receive the newest packet's serial number.
////Return Value
////The function returns non-zero if successful, zero otherwise.
/// </summary>
ExternDllFunction(WTQueuePacketsEx);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrConfigReplaceExA);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrConfigReplaceExW);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrPacketHookExA);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrPacketHookExW);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrPacketUnhook);
/// <summary>
/// </summary>
ExternDllFunction(WTMgrPacketHookNext);
///<summary>
////Syntax
////BOOL WTMgrCsrPressureBtnMarksEx(hMgr, wCsr, lpNMarks, lpTMarks)
////This function allows tablet managers to change the pressure thresholds that control the mapping of pressure activity to button events.
////Parameter
////Type
////Description
////hMgr
////HMGR Is the valid manager handle that identifies the caller as a manager application.
////wCsr
////UINT Specifies the zero-based cursor id of the cursor type whose pressure button marks are being set.
////lpNMarks
////UINT FAR * Points to a two-element array containing the button marks for the normal pressure button. The first unsigned integer contains the release mark; the second unsigned integer contains the press mark. If the value WTP_LPDEFAULT is specified, the function resets the default value for the marks.
////lpTMarks
////UINT FAR * Points to a two-element array containing the button marks for the tangent pressure button. The first unsigned integer contains the release mark; the second unsigned integer contains the press mark. If the value WTP_LPDEFAULT is specified, the function resets the default value for the marks.
////Return Value
////The return value is non-zero if the new settings have taken effect. Otherwise, it is zero.
////Comments
////This function allows the tablet manager to let the user control the "feel" of the pres­sure buttons when they are used as conventional buttons.
////The pressure ranges are defined such that the minimum value of the range indi­cates the resting, unpressed state, and the maximum value indicates the fully pressed state. The button marks determine the pressure at which button press and release events are generated. When the button is logically up and pressure rises above the press mark, a button press event is generated. Conversely, when the but­ton is logi­cally down and pressure drops below the release mark, a button release event is generated.
////The marks should be set far enough apart so that the button does not "jitter," yet far enough from the ends of the range so that the button events are easy to gen­erate.
////The marks are valid if and only if the button press mark is greater than the but­ton release mark. Setting the marks to invalid combinations will turn off button-event generation for the button.
///</summary>
ExternDllFunction(WTMgrCsrPressureBtnMarksEx);
} // namespace mp
