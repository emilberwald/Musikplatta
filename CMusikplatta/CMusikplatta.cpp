// CMusikplatta.cpp : Defines the entry point for the application.
//
#include "CMusikplatta.h"

#include "Common.h"
#include "Program.h"
#include "ShowWindowFlags.h"
#include "explain_wm.h"
#include "explain_wt.h"
#include "framework.h"

#include <iomanip>
#include <spdlog/spdlog.h>
#include <sstream>
#include <string>
void SetupConsoleWindow()
{
	AllocConsole();
	FILE *input		   = nullptr;
	FILE *errput	   = nullptr;
	FILE *output	   = nullptr;
	auto  stdin_errno  = freopen_s(&input, "CONIN$", "r", stdin);
	auto  stderr_errno = freopen_s(&errput, "CONOUT$", "w", stderr);
	auto  sdout_errno  = freopen_s(&output, "CONOUT$", "w", stdout);
}
VOID CALLBACK timer_procedure(HWND hWnd, UINT nMsg, UINT_PTR nIDEvent, DWORD dwTime)
{
	spdlog::info("Run loop. windowId={} mMsg={} nIDEvent={} dwTime={}", hWnd->unused, nMsg, nIDEvent, dwTime);
}

#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE		 hInst;								   // current instance
WCHAR			 szTitle[MAX_LOADSTRING];			   // The title bar text
WCHAR			 szWindowClass[MAX_LOADSTRING];		   // the main window class name
ATOM			 MyRegisterClass(HINSTANCE hInstance); //<! FORWARD DECLARATION
LRESULT CALLBACK ProcessWindowMessage(HWND, UINT, WPARAM,
									  LPARAM);		//<! FORWARD DECLARATION
INT_PTR CALLBACK About(HWND, UINT, WPARAM, LPARAM); //<! FORWARD DECLARATION

/// <summary>
/// Initializes the instance.
/// Saves instance handle and creates main window
/// In this function, we save the instance handle in a global variable and
/// create and display the main program window.
/// </summary>
/// <param name="hInstance">The h instance.</param>
/// <param name="nCmdShow">The n command show.</param>
/// <returns></returns>
template<class T>
BOOL InitInstance(HINSTANCE hInstance, ShowWindowFlags nCmdShow, T *userdata)
{
	hInst	  = hInstance; // Store instance handle in our global variable
	HWND hWnd = CreateWindowW(szWindowClass,
							  szTitle,
							  WS_OVERLAPPEDWINDOW,
							  CW_USEDEFAULT,
							  0,
							  CW_USEDEFAULT,
							  0,
							  nullptr,
							  nullptr,
							  hInstance,
							  userdata);
	if(!hWnd) { return FALSE; }
	ShowWindow(hWnd, (int)nCmdShow);
	UpdateWindow(hWnd); //!< Sends WM_PAINT
	return TRUE;
}

/// <summary>
/// Entry point.
/// </summary>
/// <param name="hInstance">The instance.</param>
/// <param name="hPrevInstance">[[deprecated]] The previous instance.</param>
/// <param name="lpCmdLine">The command line pointer.</param>
/// <param name="nCmdShow">The number of commands.</param>
/// <returns></returns>
int APIENTRY wWinMain(_In_ HINSTANCE	 hInstance,
					  _In_opt_ HINSTANCE hPrevInstance,
					  _In_ LPWSTR		 lpCmdLine,
					  _In_ int			 nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	SetupConsoleWindow();

	spdlog::info("Starting program.");
	spdlog::info("Setting up timer ...");
	auto TimerId = SetTimer(NULL, 0, 1000, &timer_procedure);

	// Initialize global strings
	LoadStringW(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadStringW(hInstance, IDC_CMUSIKPLATTA, szWindowClass, MAX_LOADSTRING);

	std::unique_ptr<mp::IProgram> program(new mp::Program{});

	MyRegisterClass(hInstance);
	if(!InitInstance<mp::IProgram>(hInstance,
								   static_cast<ShowWindowFlags>(nCmdShow),
								   program.get())) // Perform application initialization
	{ return FALSE; }

	HACCEL hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_CMUSIKPLATTA));
	MSG	   msg{};
	// Main messageType loop:
	while(GetMessage(&msg, nullptr, 0, 0))
	{
		if(!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int)msg.wParam;
}

/// <summary>
/// Registers the window class.
/// </summary>
/// <param name="hInstance">
/// A handle to the instance that contains the window procedure for the class.
/// HINSTANCE: Handle that identifies the running .dll/.exe's data.
///             (In 16bit windows the data segment was a copied so the process
///             had its own data)
/// HMODULE: Handle to a data structure that describes the parts of the file,
///          where they come from, and where they had been loaded into memory
///          (if at all).
///             (In 16bit windows code and resources were shared)
/// (In 32bit windows programs run in separate address spaces so no instance
/// handles
///  visible across process boundaries)
/// https://devblogs.microsoft.com/oldnewthing/20040614-00/?p=38903
/// </param>
/// <returns></returns>
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEXW wcex;
	wcex.cbSize		   = sizeof(WNDCLASSEX);
	wcex.style		   = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc   = ProcessWindowMessage;
	wcex.cbClsExtra	   = 0;
	wcex.cbWndExtra	   = 0;
	wcex.hInstance	   = hInstance;
	wcex.hIcon		   = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_CMUSIKPLATTA));
	wcex.hCursor	   = LoadCursor(nullptr, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
	wcex.lpszMenuName  = MAKEINTRESOURCEW(IDC_CMUSIKPLATTA);
	wcex.lpszClassName = szWindowClass;
	wcex.hIconSm	   = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));
	return RegisterClassExW(&wcex);
}

template<class T>
std::string to_hex(T t)
{
	std::ostringstream oss;
	oss << std::uppercase << std::showbase << std::hex << t;
	return oss.str();
}

/// <summary>
/// Processes the window message.
/// </summary>
/// <param name="windowId">The window identifier.</param>
/// <param name="messageType">Type of the message.</param>
/// <param name="wParam">The w parameter.</param>
/// <param name="lParam">The l parameter.</param>
/// <returns></returns>
LRESULT CALLBACK ProcessWindowMessage(HWND windowId, UINT messageType, WPARAM wParam, LPARAM lParam)
{
	{
		auto explanation = mp::explain_wm.count(messageType)
							   ? mp::explain_wm[messageType]
							   : mp::explain_wm_fallback.count(messageType)
									 ? mp::explain_wm_fallback[messageType]
									 : mp::explain_wt.count(messageType) ? mp::explain_wt[messageType]
																		 : decltype(mp::explain_wm)::mapped_type{};
		if(std::get<1>(explanation).empty())
		{
			spdlog::info(to_hex(messageType) + "\t" + "<" + std::get<0>(explanation) + ">" + "\t"
						 + std::get<1>(explanation));
		}
		else if(std::get<0>(explanation).rfind("WT", 0) == 0)
		{
			spdlog::info(to_hex(messageType) + "\t" + "<" + std::get<0>(explanation) + ">" + "\t"
						 + std::get<1>(explanation));
		}
		else
		{
			spdlog::info(to_hex(messageType) + "\t" + "<" + std::get<0>(explanation) + ">" + "\t"
						 + std::get<1>(explanation));
		}
		mp::IProgram *program;
		if(messageType == WM_NCCREATE)
		{
			program = static_cast<mp::IProgram *>(reinterpret_cast<CREATESTRUCT *>(lParam)->lpCreateParams);

			if(SetWindowLongPtr(windowId, GWLP_USERDATA, reinterpret_cast<LONG_PTR>(program)) == 0)
			{ spdlog::error(MP_HEREWIN32); }
		}
		else
		{
			program = reinterpret_cast<mp::IProgram *>(GetWindowLongPtr(windowId, GWLP_USERDATA));
			if(program == nullptr) { spdlog::error(MP_HEREWIN32); }
		}

		if(program) { program->HandleMessage(windowId, messageType, wParam, lParam); }
	}

	switch(messageType)
	{
		case WM_COMMAND:
		{
			int wmId = LOWORD(wParam);
			// Parse the menu selections:
			switch(wmId)
			{
				case IDM_ABOUT: DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), windowId, About); break;
				case IDM_EXIT: DestroyWindow(windowId); break;
				default: return DefWindowProc(windowId, messageType, wParam, lParam);
			}
			break;
		}
		case WM_PAINT:
		{
			PAINTSTRUCT ps;
			HDC			hdc = BeginPaint(windowId, &ps);
			// TODO: Add any drawing code that uses hdc here...
			EndPaint(windowId, &ps);
			break;
		}
		case WM_DESTROY: PostQuitMessage(0); break;
		default: return DefWindowProc(windowId, messageType, wParam, lParam);
	}
	return 0;
}

/// <summary>
/// Message handler for about box.
/// </summary>
/// <param name="hDlg">The h dialog.</param>
/// <param name="message">The message.</param>
/// <param name="wParam">The w parameter.</param>
/// <param name="lParam">The l parameter.</param>
/// <returns></returns>
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch(message)
	{
		case WM_INITDIALOG: return (INT_PTR)TRUE;
		case WM_COMMAND:
			if(LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
			{
				EndDialog(hDlg, LOWORD(wParam));
				return (INT_PTR)TRUE;
			}
			break;
	}
	return (INT_PTR)FALSE;
}