#pragma once
enum ShowWindowFlags : int
{
	/// <summary>
	///  minimizes a window, even if the thread that owns the window is not responding.
	///  this flag should only be used when minimizing windows from a different thread.
	/// </summary>
	sw_forceminimize = SW_FORCEMINIMIZE,
	/// <summary>
	///  hides the window and activates another window.
	/// </summary>
	sw_hide = SW_HIDE,
	/// <summary>
	///  maximizes the specified window.
	/// </summary>
	sw_maximize = SW_MAXIMIZE,
	/// <summary>
	///  minimizes the specified window and activates the next top-level window in the z
	/// <order.
	/// </summary>
	sw_minimize = SW_MINIMIZE,
	/// <summary>
	///  activates and displays the window. if the window is minimized or maximized, the
	///  system restores it to its original size and position. an application should specify this
	///  flag when restoring a minimized window.
	/// </summary>
	sw_restore = SW_RESTORE,
	/// <summary>
	///  activates the window and displays it in its current size and position.
	/// </summary>
	sw_show = SW_SHOW,
	/// <summary>
	///  sets the show state based on the sw_ value specified in the startupinfo
	///  structure passed to the createprocess function by the program that started the application.
	/// </summary>
	sw_showdefault = SW_SHOWDEFAULT,
	/// <summary>
	///  activates the window and displays it as a maximized window.
	/// </summary>
	sw_showmaximized = SW_SHOWMAXIMIZED,
	/// <summary>
	///  activates the window and displays it as a minimized window.
	/// </summary>
	sw_showminimized = SW_SHOWMINIMIZED,
	/// <summary>
	///  displays the window as a minimized window. this value is similar to
	///  sw_showminimized, except the window is not activated.
	/// </summary>
	sw_showminnoactive = SW_SHOWMINNOACTIVE,
	/// <summary>
	///  displays the window in its current size and position. this value is similar to
	///  sw_show, except that the window is not activated.
	/// </summary>
	sw_showna = SW_SHOWNA,
	/// <summary>
	///  displays a window in its most recent size and position. this value is similar to
	///  sw_shownormal, except that the window is not activated.
	/// </summary>
	sw_shownoactivate = SW_SHOWNOACTIVATE,
	/// <summary>
	///  activates and displays a window. if the window is minimized or maximized, the
	///  system restores it to its original size and position. an application should specify this
	///  flag when displaying the window for the first time.
	/// </summary>
	sw_shownormal = SW_SHOWNORMAL,
};

DEFINE_ENUM_FLAG_OPERATORS(ShowWindowFlags)