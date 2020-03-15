using System;

namespace Musikplatta
{
    public enum WindowsEventMessages
    {
        #region http://pinvoke.net/default.aspx/Enums/WindowsMessages.html

        /// <summary>
        /// The WM_NULL message performs no operation. An application sends the WM_NULL message if it wants to post a message that the recipient window will ignore.
        /// </summary>
        WM_NULL = 0x0000,

        /// <summary>
        /// The WM_CREATE message is sent when an application requests that a window be created by calling the CreateWindowEx or CreateWindow function. (The message is sent before the function returns.) The window procedure of the new window receives this message after the window is created, but before the window becomes visible.
        /// </summary>
        WM_CREATE = 0x0001,

        /// <summary>
        /// The WM_DESTROY message is sent when a window is being destroyed. It is sent to the window procedure of the window being destroyed after the window is removed from the screen.
        /// This message is sent first to the window being destroyed and then to the child windows (if any) as they are destroyed. During the processing of the message, it can be assumed that all child windows still exist.
        /// /// </summary>
        WM_DESTROY = 0x0002,

        /// <summary>
        /// The WM_MOVE message is sent after a window has been moved.
        /// </summary>
        WM_MOVE = 0x0003,

        /// <summary>
        /// The WM_SIZE message is sent to a window after its size has changed.
        /// </summary>
        WM_SIZE = 0x0005,

        /// <summary>
        /// The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately.
        /// </summary>
        WM_ACTIVATE = 0x0006,

        /// <summary>
        /// The WM_SETFOCUS message is sent to a window after it has gained the keyboard focus.
        /// </summary>
        WM_SETFOCUS = 0x0007,

        /// <summary>
        /// The WM_KILLFOCUS message is sent to a window immediately before it loses the keyboard focus.
        /// </summary>
        WM_KILLFOCUS = 0x0008,

        /// <summary>
        /// The WM_ENABLE message is sent when an application changes the enabled state of a window. It is sent to the window whose enabled state is changing. This message is sent before the EnableWindow function returns, but after the enabled state (WS_DISABLED style bit) of the window has changed.
        /// </summary>
        WM_ENABLE = 0x000A,

        /// <summary>
        /// An application sends the WM_SETREDRAW message to a window to allow changes in that window to be redrawn or to prevent changes in that window from being redrawn.
        /// </summary>
        WM_SETREDRAW = 0x000B,

        /// <summary>
        /// An application sends a WM_SETTEXT message to set the text of a window.
        /// </summary>
        WM_SETTEXT = 0x000C,

        /// <summary>
        /// An application sends a WM_GETTEXT message to copy the text that corresponds to a window into a buffer provided by the caller.
        /// </summary>
        WM_GETTEXT = 0x000D,

        /// <summary>
        /// An application sends a WM_GETTEXTLENGTH message to determine the length, in characters, of the text associated with a window.
        /// </summary>
        WM_GETTEXTLENGTH = 0x000E,

        /// <summary>
        /// The WM_PAINT message is sent when the system or another application makes a request to paint a portion of an application's window. The message is sent when the UpdateWindow or RedrawWindow function is called, or by the DispatchMessage function when the application obtains a WM_PAINT message by using the GetMessage or PeekMessage function.
        /// </summary>
        WM_PAINT = 0x000F,

        /// <summary>
        /// The WM_CLOSE message is sent as a signal that a window or an application should terminate.
        /// </summary>
        WM_CLOSE = 0x0010,

        /// <summary>
        /// The WM_QUERYENDSESSION message is sent when the user chooses to end the session or when an application calls one of the system shutdown functions. If any application returns zero, the session is not ended. The system stops sending WM_QUERYENDSESSION messages as soon as one application returns zero.
        /// After processing this message, the system sends the WM_ENDSESSION message with the wParam parameter set to the results of the WM_QUERYENDSESSION message.
        /// </summary>
        WM_QUERYENDSESSION = 0x0011,

        /// <summary>
        /// The WM_QUERYOPEN message is sent to an icon when the user requests that the window be restored to its previous size and position.
        /// </summary>
        WM_QUERYOPEN = 0x0013,

        /// <summary>
        /// The WM_ENDSESSION message is sent to an application after the system processes the results of the WM_QUERYENDSESSION message. The WM_ENDSESSION message informs the application whether the session is ending.
        /// </summary>
        WM_ENDSESSION = 0x0016,

        /// <summary>
        /// The WM_QUIT message indicates a request to terminate an application and is generated when the application calls the PostQuitMessage function. It causes the GetMessage function to return zero.
        /// </summary>
        WM_QUIT = 0x0012,

        /// <summary>
        /// The WM_ERASEBKGND message is sent when the window background must be erased (for example, when a window is resized). The message is sent to prepare an invalidated portion of a window for painting.
        /// </summary>
        WM_ERASEBKGND = 0x0014,

        /// <summary>
        /// This message is sent to all top-level windows when a change is made to a system color setting.
        /// </summary>
        WM_SYSCOLORCHANGE = 0x0015,

        /// <summary>
        /// The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.
        /// </summary>
        WM_SHOWWINDOW = 0x0018,

        /// <summary>
        /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
        /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
        /// </summary>
        WM_WININICHANGE = 0x001A,

        /// <summary>
        /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
        /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
        /// </summary>
        WM_SETTINGCHANGE = WM_WININICHANGE,

        /// <summary>
        /// The WM_DEVMODECHANGE message is sent to all top-level windows whenever the user changes device-mode settings.
        /// </summary>
        WM_DEVMODECHANGE = 0x001B,

        /// <summary>
        /// The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.
        /// </summary>
        WM_ACTIVATEAPP = 0x001C,

        /// <summary>
        /// An application sends the WM_FONTCHANGE message to all top-level windows in the system after changing the pool of font resources.
        /// </summary>
        WM_FONTCHANGE = 0x001D,

        /// <summary>
        /// A message that is sent whenever there is a change in the system time.
        /// </summary>
        WM_TIMECHANGE = 0x001E,

        /// <summary>
        /// The WM_CANCELMODE message is sent to cancel certain modes, such as mouse capture. For example, the system sends this message to the active window when a dialog box or message box is displayed. Certain functions also send this message explicitly to the specified window regardless of whether it is the active window. For example, the EnableWindow function sends this message when disabling the specified window.
        /// </summary>
        WM_CANCELMODE = 0x001F,

        /// <summary>
        /// The WM_SETCURSOR message is sent to a window if the mouse causes the cursor to move within a window and mouse input is not captured.
        /// </summary>
        WM_SETCURSOR = 0x0020,

        /// <summary>
        /// The WM_MOUSEACTIVATE message is sent when the cursor is in an inactive window and the user presses a mouse button. The parent window receives this message only if the child window passes it to the DefWindowProc function.
        /// </summary>
        WM_MOUSEACTIVATE = 0x0021,

        /// <summary>
        /// The WM_CHILDACTIVATE message is sent to a child window when the user clicks the window's title bar or when the window is activated, moved, or sized.
        /// </summary>
        WM_CHILDACTIVATE = 0x0022,

        /// <summary>
        /// The WM_QUEUESYNC message is sent by a computer-based training (CBT) application to separate user-input messages from other messages sent through the WH_JOURNALPLAYBACK Hook procedure.
        /// </summary>
        WM_QUEUESYNC = 0x0023,

        /// <summary>
        /// The WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default maximized size and position, or its default minimum or maximum tracking size.
        /// </summary>
        WM_GETMINMAXINFO = 0x0024,

        /// <summary>
        /// Windows NT 3.51 and earlier: The WM_PAINTICON message is sent to a minimized window when the icon is to be painted. This message is not sent by newer versions of Microsoft Windows, except in unusual circumstances explained in the Remarks.
        /// </summary>
        WM_PAINTICON = 0x0026,

        /// <summary>
        /// Windows NT 3.51 and earlier: The WM_ICONERASEBKGND message is sent to a minimized window when the background of the icon must be filled before painting the icon. A window receives this message only if a class icon is defined for the window; otherwise, WM_ERASEBKGND is sent. This message is not sent by newer versions of Windows.
        /// </summary>
        WM_ICONERASEBKGND = 0x0027,

        /// <summary>
        /// The WM_NEXTDLGCTL message is sent to a dialog box procedure to set the keyboard focus to a different control in the dialog box.
        /// </summary>
        WM_NEXTDLGCTL = 0x0028,

        /// <summary>
        /// The WM_SPOOLERSTATUS message is sent from Print Manager whenever a job is added to or removed from the Print Manager queue.
        /// </summary>
        WM_SPOOLERSTATUS = 0x002A,

        /// <summary>
        /// The WM_DRAWITEM message is sent to the parent window of an owner-drawn button, combo box, list box, or menu when a visual aspect of the button, combo box, list box, or menu has changed.
        /// </summary>
        WM_DRAWITEM = 0x002B,

        /// <summary>
        /// The WM_MEASUREITEM message is sent to the owner window of a combo box, list box, list view control, or menu item when the control or menu is created.
        /// </summary>
        WM_MEASUREITEM = 0x002C,

        /// <summary>
        /// Sent to the owner of a list box or combo box when the list box or combo box is destroyed or when items are removed by the LB_DELETESTRING, LB_RESETCONTENT, CB_DELETESTRING, or CB_RESETCONTENT message. The system sends a WM_DELETEITEM message for each deleted item. The system sends the WM_DELETEITEM message for any deleted list box or combo box item with nonzero item data.
        /// </summary>
        WM_DELETEITEM = 0x002D,

        /// <summary>
        /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_KEYDOWN message.
        /// </summary>
        WM_VKEYTOITEM = 0x002E,

        /// <summary>
        /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_CHAR message.
        /// </summary>
        WM_CHARTOITEM = 0x002F,

        /// <summary>
        /// An application sends a WM_SETFONT message to specify the font that a control is to use when drawing text.
        /// </summary>
        WM_SETFONT = 0x0030,

        /// <summary>
        /// An application sends a WM_GETFONT message to a control to retrieve the font with which the control is currently drawing its text.
        /// </summary>
        WM_GETFONT = 0x0031,

        /// <summary>
        /// An application sends a WM_SETHOTKEY message to a window to associate a hot key with the window. When the user presses the hot key, the system activates the window.
        /// </summary>
        WM_SETHOTKEY = 0x0032,

        /// <summary>
        /// An application sends a WM_GETHOTKEY message to determine the hot key associated with a window.
        /// </summary>
        WM_GETHOTKEY = 0x0033,

        /// <summary>
        /// The WM_QUERYDRAGICON message is sent to a minimized (iconic) window. The window is about to be dragged by the user but does not have an icon defined for its class. An application can return a handle to an icon or cursor. The system displays this cursor or icon while the user drags the icon.
        /// </summary>
        WM_QUERYDRAGICON = 0x0037,

        /// <summary>
        /// The system sends the WM_COMPAREITEM message to determine the relative position of a new item in the sorted list of an owner-drawn combo box or list box. Whenever the application adds a new item, the system sends this message to the owner of a combo box or list box created with the CBS_SORT or LBS_SORT style.
        /// </summary>
        WM_COMPAREITEM = 0x0039,

        /// <summary>
        /// Active Accessibility sends the WM_GETOBJECT message to obtain information about an accessible object contained in a server application.
        /// Applications never send this message directly. It is sent only by Active Accessibility in response to calls to AccessibleObjectFromPoint, AccessibleObjectFromEvent, or AccessibleObjectFromWindow. However, server applications handle this message.
        /// </summary>
        WM_GETOBJECT = 0x003D,

        /// <summary>
        /// The WM_COMPACTING message is sent to all top-level windows when the system detects more than 12.5 percent of system time over a 30- to 60-second interval is being spent compacting memory. This indicates that system memory is low.
        /// </summary>
        WM_COMPACTING = 0x0041,

        /// <summary>
        /// WM_COMMNOTIFY is Obsolete for Win32-Based Applications
        /// </summary>
        [Obsolete]
        WM_COMMNOTIFY = 0x0044,

        /// <summary>
        /// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
        /// </summary>
        WM_WINDOWPOSCHANGING = 0x0046,

        /// <summary>
        /// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
        /// </summary>
        WM_WINDOWPOSCHANGED = 0x0047,

        /// <summary>
        /// Notifies applications that the system, typically a battery-powered personal computer, is about to enter a suspended mode.
        /// Use: POWERBROADCAST
        /// </summary>
        [Obsolete]
        WM_POWER = 0x0048,

        /// <summary>
        /// An application sends the WM_COPYDATA message to pass data to another application.
        /// </summary>
        WM_COPYDATA = 0x004A,

        /// <summary>
        /// The WM_CANCELJOURNAL message is posted to an application when a user cancels the application's journaling activities. The message is posted with a NULL window handle.
        /// </summary>
        WM_CANCELJOURNAL = 0x004B,

        /// <summary>
        /// Sent by a common control to its parent window when an event has occurred or the control requires some information.
        /// </summary>
        WM_NOTIFY = 0x004E,

        /// <summary>
        /// The WM_INPUTLANGCHANGEREQUEST message is posted to the window with the focus when the user chooses a new input language, either with the hotkey (specified in the Keyboard control panel application) or from the indicator on the system taskbar. An application can accept the change by passing the message to the DefWindowProc function or reject the change (and prevent it from taking place) by returning immediately.
        /// </summary>
        WM_INPUTLANGCHANGEREQUEST = 0x0050,

        /// <summary>
        /// The WM_INPUTLANGCHANGE message is sent to the topmost affected window after an application's input language has been changed. You should make any application-specific settings and pass the message to the DefWindowProc function, which passes the message to all first-level child windows. These child windows can pass the message to DefWindowProc to have it pass the message to their child windows, and so on.
        /// </summary>
        WM_INPUTLANGCHANGE = 0x0051,

        /// <summary>
        /// Sent to an application that has initiated a training card with Microsoft Windows Help. The message informs the application when the user clicks an authorable button. An application initiates a training card by specifying the HELP_TCARD command in a call to the WinHelp function.
        /// </summary>
        WM_TCARD = 0x0052,

        /// <summary>
        /// Indicates that the user pressed the F1 key. If a menu is active when F1 is pressed, WM_HELP is sent to the window associated with the menu; otherwise, WM_HELP is sent to the window that has the keyboard focus. If no window has the keyboard focus, WM_HELP is sent to the currently active window.
        /// </summary>
        WM_HELP = 0x0053,

        /// <summary>
        /// The WM_USERCHANGED message is sent to all windows after the user has logged on or off. When the user logs on or off, the system updates the user-specific settings. The system sends this message immediately after updating the settings.
        /// </summary>
        WM_USERCHANGED = 0x0054,

        /// <summary>
        /// Determines if a window accepts ANSI or Unicode structures in the WM_NOTIFY notification message. WM_NOTIFYFORMAT messages are sent from a common control to its parent window and from the parent window to the common control.
        /// </summary>
        WM_NOTIFYFORMAT = 0x0055,

        /// <summary>
        /// The WM_CONTEXTMENU message notifies a window that the user clicked the right mouse button (right-clicked) in the window.
        /// </summary>
        WM_CONTEXTMENU = 0x007B,

        /// <summary>
        /// The WM_STYLECHANGING message is sent to a window when the SetWindowLong function is about to change one or more of the window's styles.
        /// </summary>
        WM_STYLECHANGING = 0x007C,

        /// <summary>
        /// The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles
        /// </summary>
        WM_STYLECHANGED = 0x007D,

        /// <summary>
        /// The WM_DISPLAYCHANGE message is sent to all windows when the display resolution has changed.
        /// </summary>
        WM_DISPLAYCHANGE = 0x007E,

        /// <summary>
        /// The WM_GETICON message is sent to a window to retrieve a handle to the large or small icon associated with a window. The system displays the large icon in the ALT+TAB dialog, and the small icon in the window caption.
        /// </summary>
        WM_GETICON = 0x007F,

        /// <summary>
        /// An application sends the WM_SETICON message to associate a new large or small icon with a window. The system displays the large icon in the ALT+TAB dialog box, and the small icon in the window caption.
        /// </summary>
        WM_SETICON = 0x0080,

        /// <summary>
        /// The WM_NCCREATE message is sent prior to the WM_CREATE message when a window is first created.
        /// </summary>
        WM_NCCREATE = 0x0081,

        /// <summary>
        /// The WM_NCDESTROY message informs a window that its nonclient area is being destroyed. The DestroyWindow function sends the WM_NCDESTROY message to the window following the WM_DESTROY message. WM_DESTROY is used to free the allocated memory object associated with the window.
        /// The WM_NCDESTROY message is sent after the child windows have been destroyed. In contrast, WM_DESTROY is sent before the child windows are destroyed.
        /// </summary>
        WM_NCDESTROY = 0x0082,

        /// <summary>
        /// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.
        /// </summary>
        WM_NCCALCSIZE = 0x0083,

        /// <summary>
        /// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.
        /// </summary>
        WM_NCHITTEST = 0x0084,

        /// <summary>
        /// The WM_NCPAINT message is sent to a window when its frame must be painted.
        /// </summary>
        WM_NCPAINT = 0x0085,

        /// <summary>
        /// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
        /// </summary>
        WM_NCACTIVATE = 0x0086,

        /// <summary>
        /// The WM_GETDLGCODE message is sent to the window procedure associated with a control. By default, the system handles all keyboard input to the control; the system interprets certain types of keyboard input as dialog box navigation keys. To override this default behavior, the control can respond to the WM_GETDLGCODE message to indicate the types of input it wants to process itself.
        /// </summary>
        WM_GETDLGCODE = 0x0087,

        /// <summary>
        /// The WM_SYNCPAINT message is used to synchronize painting while avoiding linking independent GUI threads.
        /// </summary>
        WM_SYNCPAINT = 0x0088,

        /// <summary>
        /// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCMOUSEMOVE = 0x00A0,

        /// <summary>
        /// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCLBUTTONDOWN = 0x00A1,

        /// <summary>
        /// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCLBUTTONUP = 0x00A2,

        /// <summary>
        /// The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCLBUTTONDBLCLK = 0x00A3,

        /// <summary>
        /// The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCRBUTTONDOWN = 0x00A4,

        /// <summary>
        /// The WM_NCRBUTTONUP message is posted when the user releases the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCRBUTTONUP = 0x00A5,

        /// <summary>
        /// The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCRBUTTONDBLCLK = 0x00A6,

        /// <summary>
        /// The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCMBUTTONDOWN = 0x00A7,

        /// <summary>
        /// The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCMBUTTONUP = 0x00A8,

        /// <summary>
        /// The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCMBUTTONDBLCLK = 0x00A9,

        /// <summary>
        /// The WM_NCXBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCXBUTTONDOWN = 0x00AB,

        /// <summary>
        /// The WM_NCXBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCXBUTTONUP = 0x00AC,

        /// <summary>
        /// The WM_NCXBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        WM_NCXBUTTONDBLCLK = 0x00AD,

        /// <summary>
        /// The WM_INPUT_DEVICE_CHANGE message is sent to the window that registered to receive raw input. A window receives this message through its WindowProc function.
        /// </summary>
        WM_INPUT_DEVICE_CHANGE = 0x00FE,

        /// <summary>
        /// The WM_INPUT message is sent to the window that is getting raw input.
        /// </summary>
        WM_INPUT = 0x00FF,

        /// <summary>
        /// This message filters for keyboard messages.
        /// </summary>
        WM_KEYFIRST = 0x0100,

        /// <summary>
        /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
        /// </summary>
        WM_KEYDOWN = 0x0100,

        /// <summary>
        /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, or a keyboard key that is pressed when a window has the keyboard focus.
        /// </summary>
        WM_KEYUP = 0x0101,

        /// <summary>
        /// The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed.
        /// </summary>
        WM_CHAR = 0x0102,

        /// <summary>
        /// The WM_DEADCHAR message is posted to the window with the keyboard focus when a WM_KEYUP message is translated by the TranslateMessage function. WM_DEADCHAR specifies a character code generated by a dead key. A dead key is a key that generates a character, such as the umlaut (double-dot), that is combined with another character to form a composite character. For example, the umlaut-O character (Ö) is generated by typing the dead key for the umlaut character, and then typing the O key.
        /// </summary>
        WM_DEADCHAR = 0x0103,

        /// <summary>
        /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10 key (which activates the menu bar) or holds down the ALT key and then presses another key. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.
        /// </summary>
        WM_SYSKEYDOWN = 0x0104,

        /// <summary>
        /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases a key that was pressed while the ALT key was held down. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.
        /// </summary>
        WM_SYSKEYUP = 0x0105,

        /// <summary>
        /// The WM_SYSCHAR message is posted to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. It specifies the character code of a system character key — that is, a character key that is pressed while the ALT key is down.
        /// </summary>
        WM_SYSCHAR = 0x0106,

        /// <summary>
        /// The WM_SYSDEADCHAR message is sent to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. WM_SYSDEADCHAR specifies the character code of a system dead key — that is, a dead key that is pressed while holding down the ALT key.
        /// </summary>
        WM_SYSDEADCHAR = 0x0107,

        /// <summary>
        /// The WM_UNICHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_UNICHAR message contains the character code of the key that was pressed.
        /// The WM_UNICHAR message is equivalent to WM_CHAR, but it uses Unicode Transformation Format (UTF)-32, whereas WM_CHAR uses UTF-16. It is designed to send or post Unicode characters to ANSI windows and it can can handle Unicode Supplementary Plane characters.
        /// </summary>
        WM_UNICHAR = 0x0109,

        /// <summary>
        /// This message filters for keyboard messages.
        /// </summary>
        WM_KEYLAST = 0x0108,

        /// <summary>
        /// Sent immediately before the IME generates the composition string as a result of a keystroke. A window receives this message through its WindowProc function.
        /// </summary>
        WM_IME_STARTCOMPOSITION = 0x010D,

        /// <summary>
        /// Sent to an application when the IME ends composition. A window receives this message through its WindowProc function.
        /// </summary>
        WM_IME_ENDCOMPOSITION = 0x010E,

        /// <summary>
        /// Sent to an application when the IME changes composition status as a result of a keystroke. A window receives this message through its WindowProc function.
        /// </summary>
        WM_IME_COMPOSITION = 0x010F,

        WM_IME_KEYLAST = 0x010F,

        /// <summary>
        /// The WM_INITDIALOG message is sent to the dialog box procedure immediately before a dialog box is displayed. Dialog box procedures typically use this message to initialize controls and carry out any other initialization tasks that affect the appearance of the dialog box.
        /// </summary>
        WM_INITDIALOG = 0x0110,

        /// <summary>
        /// The WM_COMMAND message is sent when the user selects a command item from a menu, when a control sends a notification message to its parent window, or when an accelerator keystroke is translated.
        /// </summary>
        WM_COMMAND = 0x0111,

        /// <summary>
        /// A window receives this message when the user chooses a command from the Window menu, clicks the maximize button, minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering this out.
        /// </summary>
        WM_SYSCOMMAND = 0x0112,

        /// <summary>
        /// The WM_TIMER message is posted to the installing thread's message queue when a timer expires. The message is posted by the GetMessage or PeekMessage function.
        /// </summary>
        WM_TIMER = 0x0113,

        /// <summary>
        /// The WM_HSCROLL message is sent to a window when a scroll event occurs in the window's standard horizontal scroll bar. This message is also sent to the owner of a horizontal scroll bar control when a scroll event occurs in the control.
        /// </summary>
        WM_HSCROLL = 0x0114,

        /// <summary>
        /// The WM_VSCROLL message is sent to a window when a scroll event occurs in the window's standard vertical scroll bar. This message is also sent to the owner of a vertical scroll bar control when a scroll event occurs in the control.
        /// </summary>
        WM_VSCROLL = 0x0115,

        /// <summary>
        /// The WM_INITMENU message is sent when a menu is about to become active. It occurs when the user clicks an item on the menu bar or presses a menu key. This allows the application to modify the menu before it is displayed.
        /// </summary>
        WM_INITMENU = 0x0116,

        /// <summary>
        /// The WM_INITMENUPOPUP message is sent when a drop-down menu or submenu is about to become active. This allows an application to modify the menu before it is displayed, without changing the entire menu.
        /// </summary>
        WM_INITMENUPOPUP = 0x0117,

        /// <summary>
        /// The WM_MENUSELECT message is sent to a menu's owner window when the user selects a menu item.
        /// </summary>
        WM_MENUSELECT = 0x011F,

        /// <summary>
        /// The WM_MENUCHAR message is sent when a menu is active and the user presses a key that does not correspond to any mnemonic or accelerator key. This message is sent to the window that owns the menu.
        /// </summary>
        WM_MENUCHAR = 0x0120,

        /// <summary>
        /// The WM_ENTERIDLE message is sent to the owner window of a modal dialog box or menu that is entering an idle state. A modal dialog box or menu enters an idle state when no messages are waiting in its queue after it has processed one or more previous messages.
        /// </summary>
        WM_ENTERIDLE = 0x0121,

        /// <summary>
        /// The WM_MENURBUTTONUP message is sent when the user releases the right mouse button while the cursor is on a menu item.
        /// </summary>
        WM_MENURBUTTONUP = 0x0122,

        /// <summary>
        /// The WM_MENUDRAG message is sent to the owner of a drag-and-drop menu when the user drags a menu item.
        /// </summary>
        WM_MENUDRAG = 0x0123,

        /// <summary>
        /// The WM_MENUGETOBJECT message is sent to the owner of a drag-and-drop menu when the mouse cursor enters a menu item or moves from the center of the item to the top or bottom of the item.
        /// </summary>
        WM_MENUGETOBJECT = 0x0124,

        /// <summary>
        /// The WM_UNINITMENUPOPUP message is sent when a drop-down menu or submenu has been destroyed.
        /// </summary>
        WM_UNINITMENUPOPUP = 0x0125,

        /// <summary>
        /// The WM_MENUCOMMAND message is sent when the user makes a selection from a menu.
        /// </summary>
        WM_MENUCOMMAND = 0x0126,

        /// <summary>
        /// An application sends the WM_CHANGEUISTATE message to indicate that the user interface (UI) state should be changed.
        /// </summary>
        WM_CHANGEUISTATE = 0x0127,

        /// <summary>
        /// An application sends the WM_UPDATEUISTATE message to change the user interface (UI) state for the specified window and all its child windows.
        /// </summary>
        WM_UPDATEUISTATE = 0x0128,

        /// <summary>
        /// An application sends the WM_QUERYUISTATE message to retrieve the user interface (UI) state for a window.
        /// </summary>
        WM_QUERYUISTATE = 0x0129,

        /// <summary>
        /// The WM_CTLCOLORMSGBOX message is sent to the owner window of a message box before Windows draws the message box. By responding to this message, the owner window can set the text and background colors of the message box by using the given display device context handle.
        /// </summary>
        WM_CTLCOLORMSGBOX = 0x0132,

        /// <summary>
        /// An edit control that is not read-only or disabled sends the WM_CTLCOLOREDIT message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the edit control.
        /// </summary>
        WM_CTLCOLOREDIT = 0x0133,

        /// <summary>
        /// Sent to the parent window of a list box before the system draws the list box. By responding to this message, the parent window can set the text and background colors of the list box by using the specified display device context handle.
        /// </summary>
        WM_CTLCOLORLISTBOX = 0x0134,

        /// <summary>
        /// The WM_CTLCOLORBTN message is sent to the parent window of a button before drawing the button. The parent window can change the button's text and background colors. However, only owner-drawn buttons respond to the parent window processing this message.
        /// </summary>
        WM_CTLCOLORBTN = 0x0135,

        /// <summary>
        /// The WM_CTLCOLORDLG message is sent to a dialog box before the system draws the dialog box. By responding to this message, the dialog box can set its text and background colors using the specified display device context handle.
        /// </summary>
        WM_CTLCOLORDLG = 0x0136,

        /// <summary>
        /// The WM_CTLCOLORSCROLLBAR message is sent to the parent window of a scroll bar control when the control is about to be drawn. By responding to this message, the parent window can use the display context handle to set the background color of the scroll bar control.
        /// </summary>
        WM_CTLCOLORSCROLLBAR = 0x0137,

        /// <summary>
        /// A static control, or an edit control that is read-only or disabled, sends the WM_CTLCOLORSTATIC message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the static control.
        /// </summary>
        WM_CTLCOLORSTATIC = 0x0138,

        /// <summary>
        /// Use WM_MOUSEFIRST to specify the first mouse message. Use the PeekMessage() Function.
        /// </summary>
        WM_MOUSEFIRST = 0x0200,

        /// <summary>
        /// The WM_MOUSEMOVE message is posted to a window when the cursor moves. If the mouse is not captured, the message is posted to the window that contains the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_MOUSEMOVE = 0x0200,

        /// <summary>
        /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_LBUTTONDOWN = 0x0201,

        /// <summary>
        /// The WM_LBUTTONUP message is posted when the user releases the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_LBUTTONUP = 0x0202,

        /// <summary>
        /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_LBUTTONDBLCLK = 0x0203,

        /// <summary>
        /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_RBUTTONDOWN = 0x0204,

        /// <summary>
        /// The WM_RBUTTONUP message is posted when the user releases the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_RBUTTONUP = 0x0205,

        /// <summary>
        /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_RBUTTONDBLCLK = 0x0206,

        /// <summary>
        /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_MBUTTONDOWN = 0x0207,

        /// <summary>
        /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_MBUTTONUP = 0x0208,

        /// <summary>
        /// The WM_MBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_MBUTTONDBLCLK = 0x0209,

        /// <summary>
        /// The WM_MOUSEWHEEL message is sent to the focus window when the mouse wheel is rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
        /// </summary>
        WM_MOUSEWHEEL = 0x020A,

        /// <summary>
        /// The WM_XBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_XBUTTONDOWN = 0x020B,

        /// <summary>
        /// The WM_XBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_XBUTTONUP = 0x020C,

        /// <summary>
        /// The WM_XBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        WM_XBUTTONDBLCLK = 0x020D,

        /// <summary>
        /// The WM_MOUSEHWHEEL message is sent to the focus window when the mouse's horizontal scroll wheel is tilted or rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
        /// </summary>
        WM_MOUSEHWHEEL = 0x020E,

        /// <summary>
        /// Use WM_MOUSELAST to specify the last mouse message. Used with PeekMessage() Function.
        /// </summary>
        WM_MOUSELAST = 0x020E,

        /// <summary>
        /// The WM_PARENTNOTIFY message is sent to the parent of a child window when the child window is created or destroyed, or when the user clicks a mouse button while the cursor is over the child window. When the child window is being created, the system sends WM_PARENTNOTIFY just before the CreateWindow or CreateWindowEx function that creates the window returns. When the child window is being destroyed, the system sends the message before any processing to destroy the window takes place.
        /// </summary>
        WM_PARENTNOTIFY = 0x0210,

        /// <summary>
        /// The WM_ENTERMENULOOP message informs an application's main window procedure that a menu modal loop has been entered.
        /// </summary>
        WM_ENTERMENULOOP = 0x0211,

        /// <summary>
        /// The WM_EXITMENULOOP message informs an application's main window procedure that a menu modal loop has been exited.
        /// </summary>
        WM_EXITMENULOOP = 0x0212,

        /// <summary>
        /// The WM_NEXTMENU message is sent to an application when the right or left arrow key is used to switch between the menu bar and the system menu.
        /// </summary>
        WM_NEXTMENU = 0x0213,

        /// <summary>
        /// The WM_SIZING message is sent to a window that the user is resizing. By processing this message, an application can monitor the size and position of the drag rectangle and, if needed, change its size or position.
        /// </summary>
        WM_SIZING = 0x0214,

        /// <summary>
        /// The WM_CAPTURECHANGED message is sent to the window that is losing the mouse capture.
        /// </summary>
        WM_CAPTURECHANGED = 0x0215,

        /// <summary>
        /// The WM_MOVING message is sent to a window that the user is moving. By processing this message, an application can monitor the position of the drag rectangle and, if needed, change its position.
        /// </summary>
        WM_MOVING = 0x0216,

        /// <summary>
        /// Notifies applications that a power-management event has occurred.
        /// </summary>
        WM_POWERBROADCAST = 0x0218,

        /// <summary>
        /// Notifies an application of a change to the hardware configuration of a device or the computer.
        /// </summary>
        WM_DEVICECHANGE = 0x0219,

        /// <summary>
        /// An application sends the WM_MDICREATE message to a multiple-document interface (MDI) client window to create an MDI child window.
        /// </summary>
        WM_MDICREATE = 0x0220,

        /// <summary>
        /// An application sends the WM_MDIDESTROY message to a multiple-document interface (MDI) client window to close an MDI child window.
        /// </summary>
        WM_MDIDESTROY = 0x0221,

        /// <summary>
        /// An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window.
        /// </summary>
        WM_MDIACTIVATE = 0x0222,

        /// <summary>
        /// An application sends the WM_MDIRESTORE message to a multiple-document interface (MDI) client window to restore an MDI child window from maximized or minimized size.
        /// </summary>
        WM_MDIRESTORE = 0x0223,

        /// <summary>
        /// An application sends the WM_MDINEXT message to a multiple-document interface (MDI) client window to activate the next or previous child window.
        /// </summary>
        WM_MDINEXT = 0x0224,

        /// <summary>
        /// An application sends the WM_MDIMAXIMIZE message to a multiple-document interface (MDI) client window to maximize an MDI child window. The system resizes the child window to make its client area fill the client window. The system places the child window's window menu icon in the rightmost position of the frame window's menu bar, and places the child window's restore icon in the leftmost position. The system also appends the title bar text of the child window to that of the frame window.
        /// </summary>
        WM_MDIMAXIMIZE = 0x0225,

        /// <summary>
        /// An application sends the WM_MDITILE message to a multiple-document interface (MDI) client window to arrange all of its MDI child windows in a tile format.
        /// </summary>
        WM_MDITILE = 0x0226,

        /// <summary>
        /// An application sends the WM_MDICASCADE message to a multiple-document interface (MDI) client window to arrange all its child windows in a cascade format.
        /// </summary>
        WM_MDICASCADE = 0x0227,

        /// <summary>
        /// An application sends the WM_MDIICONARRANGE message to a multiple-document interface (MDI) client window to arrange all minimized MDI child windows. It does not affect child windows that are not minimized.
        /// </summary>
        WM_MDIICONARRANGE = 0x0228,

        /// <summary>
        /// An application sends the WM_MDIGETACTIVE message to a multiple-document interface (MDI) client window to retrieve the handle to the active MDI child window.
        /// </summary>
        WM_MDIGETACTIVE = 0x0229,

        /// <summary>
        /// An application sends the WM_MDISETMENU message to a multiple-document interface (MDI) client window to replace the entire menu of an MDI frame window, to replace the window menu of the frame window, or both.
        /// </summary>
        WM_MDISETMENU = 0x0230,

        /// <summary>
        /// The WM_ENTERSIZEMOVE message is sent one time to a window after it enters the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
        /// The system sends the WM_ENTERSIZEMOVE message regardless of whether the dragging of full windows is enabled.
        /// </summary>
        WM_ENTERSIZEMOVE = 0x0231,

        /// <summary>
        /// The WM_EXITSIZEMOVE message is sent one time to a window, after it has exited the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
        /// </summary>
        WM_EXITSIZEMOVE = 0x0232,

        /// <summary>
        /// Sent when the user drops a file on the window of an application that has registered itself as a recipient of dropped files.
        /// </summary>
        WM_DROPFILES = 0x0233,

        /// <summary>
        /// An application sends the WM_MDIREFRESHMENU message to a multiple-document interface (MDI) client window to refresh the window menu of the MDI frame window.
        /// </summary>
        WM_MDIREFRESHMENU = 0x0234,

        /// <summary>
        /// Sent to an application when a window is activated. A window receives this message through its WindowProc function.
        /// </summary>
        WM_IME_SETCONTEXT = 0x0281,

        /// <summary>
        /// Sent to an application to notify it of changes to the IME window. A window receives this message through its WindowProc function.
        /// </summary>
        WM_IME_NOTIFY = 0x0282,

        /// <summary>
        /// Sent by an application to direct the IME window to carry out the requested command. The application uses this message to control the IME window that it has created. To send this message, the application calls the SendMessage function with the following parameters.
        /// </summary>
        WM_IME_CONTROL = 0x0283,

        /// <summary>
        /// Sent to an application when the IME window finds no space to extend the area for the composition window. A window receives this message through its WindowProc function.
        /// </summary>
        WM_IME_COMPOSITIONFULL = 0x0284,

        /// <summary>
        /// Sent to an application when the operating system is about to change the current IME. A window receives this message through its WindowProc function.
        /// </summary>
        WM_IME_SELECT = 0x0285,

        /// <summary>
        /// Sent to an application when the IME gets a character of the conversion result. A window receives this message through its WindowProc function.
        /// </summary>
        WM_IME_CHAR = 0x0286,

        /// <summary>
        /// Sent to an application to provide commands and request information. A window receives this message through its WindowProc function.
        /// </summary>
        WM_IME_REQUEST = 0x0288,

        /// <summary>
        /// Sent to an application by the IME to notify the application of a key press and to keep message order. A window receives this message through its WindowProc function.
        /// </summary>
        WM_IME_KEYDOWN = 0x0290,

        /// <summary>
        /// Sent to an application by the IME to notify the application of a key release and to keep message order. A window receives this message through its WindowProc function.
        /// </summary>
        WM_IME_KEYUP = 0x0291,

        /// <summary>
        /// The WM_MOUSEHOVER message is posted to a window when the cursor hovers over the client area of the window for the period of time specified in a prior call to TrackMouseEvent.
        /// </summary>
        WM_MOUSEHOVER = 0x02A1,

        /// <summary>
        /// The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
        /// </summary>
        WM_MOUSELEAVE = 0x02A3,

        /// <summary>
        /// The WM_NCMOUSEHOVER message is posted to a window when the cursor hovers over the nonclient area of the window for the period of time specified in a prior call to TrackMouseEvent.
        /// </summary>
        WM_NCMOUSEHOVER = 0x02A0,

        /// <summary>
        /// The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
        /// </summary>
        WM_NCMOUSELEAVE = 0x02A2,

        /// <summary>
        /// The WM_WTSSESSION_CHANGE message notifies applications of changes in session state.
        /// </summary>
        WM_WTSSESSION_CHANGE = 0x02B1,

        WM_TABLET_FIRST = 0x02c0,
        WM_TABLET_LAST = 0x02df,

        /// <summary>
        /// An application sends a WM_CUT message to an edit control or combo box to delete (cut) the current selection, if any, in the edit control and copy the deleted text to the clipboard in CF_TEXT format.
        /// </summary>
        WM_CUT = 0x0300,

        /// <summary>
        /// An application sends the WM_COPY message to an edit control or combo box to copy the current selection to the clipboard in CF_TEXT format.
        /// </summary>
        WM_COPY = 0x0301,

        /// <summary>
        /// An application sends a WM_PASTE message to an edit control or combo box to copy the current content of the clipboard to the edit control at the current caret position. Data is inserted only if the clipboard contains data in CF_TEXT format.
        /// </summary>
        WM_PASTE = 0x0302,

        /// <summary>
        /// An application sends a WM_CLEAR message to an edit control or combo box to delete (clear) the current selection, if any, from the edit control.
        /// </summary>
        WM_CLEAR = 0x0303,

        /// <summary>
        /// An application sends a WM_UNDO message to an edit control to undo the last operation. When this message is sent to an edit control, the previously deleted text is restored or the previously added text is deleted.
        /// </summary>
        WM_UNDO = 0x0304,

        /// <summary>
        /// The WM_RENDERFORMAT message is sent to the clipboard owner if it has delayed rendering a specific clipboard format and if an application has requested data in that format. The clipboard owner must render data in the specified format and place it on the clipboard by calling the SetClipboardData function.
        /// </summary>
        WM_RENDERFORMAT = 0x0305,

        /// <summary>
        /// The WM_RENDERALLFORMATS message is sent to the clipboard owner before it is destroyed, if the clipboard owner has delayed rendering one or more clipboard formats. For the content of the clipboard to remain available to other applications, the clipboard owner must render data in all the formats it is capable of generating, and place the data on the clipboard by calling the SetClipboardData function.
        /// </summary>
        WM_RENDERALLFORMATS = 0x0306,

        /// <summary>
        /// The WM_DESTROYCLIPBOARD message is sent to the clipboard owner when a call to the EmptyClipboard function empties the clipboard.
        /// </summary>
        WM_DESTROYCLIPBOARD = 0x0307,

        /// <summary>
        /// The WM_DRAWCLIPBOARD message is sent to the first window in the clipboard viewer chain when the content of the clipboard changes. This enables a clipboard viewer window to display the new content of the clipboard.
        /// </summary>
        WM_DRAWCLIPBOARD = 0x0308,

        /// <summary>
        /// The WM_PAINTCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area needs repainting.
        /// </summary>
        WM_PAINTCLIPBOARD = 0x0309,

        /// <summary>
        /// The WM_VSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's vertical scroll bar. The owner should scroll the clipboard image and update the scroll bar values.
        /// </summary>
        WM_VSCROLLCLIPBOARD = 0x030A,

        /// <summary>
        /// The WM_SIZECLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area has changed size.
        /// </summary>
        WM_SIZECLIPBOARD = 0x030B,

        /// <summary>
        /// The WM_ASKCBFORMATNAME message is sent to the clipboard owner by a clipboard viewer window to request the name of a CF_OWNERDISPLAY clipboard format.
        /// </summary>
        WM_ASKCBFORMATNAME = 0x030C,

        /// <summary>
        /// The WM_CHANGECBCHAIN message is sent to the first window in the clipboard viewer chain when a window is being removed from the chain.
        /// </summary>
        WM_CHANGECBCHAIN = 0x030D,

        /// <summary>
        /// The WM_HSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window. This occurs when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's horizontal scroll bar. The owner should scroll the clipboard image and update the scroll bar values.
        /// </summary>
        WM_HSCROLLCLIPBOARD = 0x030E,

        /// <summary>
        /// This message informs a window that it is about to receive the keyboard focus, giving the window the opportunity to realize its logical palette when it receives the focus.
        /// </summary>
        WM_QUERYNEWPALETTE = 0x030F,

        /// <summary>
        /// The WM_PALETTEISCHANGING message informs applications that an application is going to realize its logical palette.
        /// </summary>
        WM_PALETTEISCHANGING = 0x0310,

        /// <summary>
        /// This message is sent by the OS to all top-level and overlapped windows after the window with the keyboard focus realizes its logical palette.
        /// This message enables windows that do not have the keyboard focus to realize their logical palettes and update their client areas.
        /// </summary>
        WM_PALETTECHANGED = 0x0311,

        /// <summary>
        /// The WM_HOTKEY message is posted when the user presses a hot key registered by the RegisterHotKey function. The message is placed at the top of the message queue associated with the thread that registered the hot key.
        /// </summary>
        WM_HOTKEY = 0x0312,

        /// <summary>
        /// The WM_PRINT message is sent to a window to request that it draw itself in the specified device context, most commonly in a printer device context.
        /// </summary>
        WM_PRINT = 0x0317,

        /// <summary>
        /// The WM_PRINTCLIENT message is sent to a window to request that it draw its client area in the specified device context, most commonly in a printer device context.
        /// </summary>
        WM_PRINTCLIENT = 0x0318,

        /// <summary>
        /// The WM_APPCOMMAND message notifies a window that the user generated an application command event, for example, by clicking an application command button using the mouse or typing an application command key on the keyboard.
        /// </summary>
        WM_APPCOMMAND = 0x0319,

        /// <summary>
        /// The WM_THEMECHANGED message is broadcast to every window following a theme change event. Examples of theme change events are the activation of a theme, the deactivation of a theme, or a transition from one theme to another.
        /// </summary>
        WM_THEMECHANGED = 0x031A,

        /// <summary>
        /// Sent when the contents of the clipboard have changed.
        /// </summary>
        WM_CLIPBOARDUPDATE = 0x031D,

        /// <summary>
        /// The system will send a window the WM_DWMCOMPOSITIONCHANGED message to indicate that the availability of desktop composition has changed.
        /// </summary>
        WM_DWMCOMPOSITIONCHANGED = 0x031E,

        /// <summary>
        /// WM_DWMNCRENDERINGCHANGED is called when the non-client area rendering status of a window has changed. Only windows that have set the flag DWM_BLURBEHIND.fTransitionOnMaximized to true will get this message.
        /// </summary>
        WM_DWMNCRENDERINGCHANGED = 0x031F,

        /// <summary>
        /// Sent to all top-level windows when the colorization color has changed.
        /// </summary>
        WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320,

        /// <summary>
        /// WM_DWMWINDOWMAXIMIZEDCHANGE will let you know when a DWM composed window is maximized. You also have to register for this message as well. You'd have other windowd go opaque when this message is sent.
        /// </summary>
        WM_DWMWINDOWMAXIMIZEDCHANGE = 0x0321,

        /// <summary>
        /// Sent to request extended title bar information. A window receives this message through its WindowProc function.
        /// </summary>
        WM_GETTITLEBARINFOEX = 0x033F,

        WM_HANDHELDFIRST = 0x0358,
        WM_HANDHELDLAST = 0x035F,
        WM_AFXFIRST = 0x0360,
        WM_AFXLAST = 0x037F,
        WM_PENWINFIRST = 0x0380,
        WM_PENWINLAST = 0x038F,

        /// <summary>
        /// The WM_APP constant is used by applications to help define private messages, usually of the form WM_APP+X, where X is an integer value.
        /// </summary>
        WM_APP = 0x8000,

        /// <summary>
        /// The WM_USER constant is used by applications to help define private messages for use by private window classes, usually of the form WM_USER+X, where X is an integer value.
        /// </summary>
        WM_USER = 0x0400,

        /// <summary>
        /// An application sends the WM_CPL_LAUNCH message to Windows Control Panel to request that a Control Panel application be started.
        /// </summary>
        WM_CPL_LAUNCH = WM_USER + 0x1000,

        /// <summary>
        /// The WM_CPL_LAUNCHED message is sent when a Control Panel application, started by the WM_CPL_LAUNCH message, has closed. The WM_CPL_LAUNCHED message is sent to the window identified by the wParam parameter of the WM_CPL_LAUNCH message that started the application.
        /// </summary>
        WM_CPL_LAUNCHED = WM_USER + 0x1001,

        /// <summary>
        /// WM_SYSTIMER is a well-known yet still undocumented message. Windows uses WM_SYSTIMER for internal actions like scrolling.
        /// </summary>
        WM_SYSTIMER = 0x118,

        /// <summary>
        /// The accessibility state has changed.
        /// </summary>
        WM_HSHELL_ACCESSIBILITYSTATE = 11,

        /// <summary>
        /// The shell should activate its main window.
        /// </summary>
        WM_HSHELL_ACTIVATESHELLWINDOW = 3,

        /// <summary>
        /// The user completed an input event (for example, pressed an application command button on the mouse or an application command key on the keyboard), and the application did not handle the WM_APPCOMMAND message generated by that input.
        /// If the Shell procedure handles the WM_COMMAND message, it should not call CallNextHookEx. See the Return Value section for more information.
        /// </summary>
        WM_HSHELL_APPCOMMAND = 12,

        /// <summary>
        /// A window is being minimized or maximized. The system needs the coordinates of the minimized rectangle for the window.
        /// </summary>
        WM_HSHELL_GETMINRECT = 5,

        /// <summary>
        /// Keyboard language was changed or a new keyboard layout was loaded.
        /// </summary>
        WM_HSHELL_LANGUAGE = 8,

        /// <summary>
        /// The title of a window in the task bar has been redrawn.
        /// </summary>
        WM_HSHELL_REDRAW = 6,

        /// <summary>
        /// The user has selected the task list. A shell application that provides a task list should return TRUE to prevent Windows from starting its task list.
        /// </summary>
        WM_HSHELL_TASKMAN = 7,

        /// <summary>
        /// A top-level, unowned window has been created. The window exists when the system calls this hook.
        /// </summary>
        WM_HSHELL_WINDOWCREATED = 1,

        /// <summary>
        /// A top-level, unowned window is about to be destroyed. The window still exists when the system calls this hook.
        /// </summary>
        WM_HSHELL_WINDOWDESTROYED = 2,

        /// <summary>
        /// The activation has changed to a different top-level, unowned window.
        /// </summary>
        WM_HSHELL_WINDOWACTIVATED = 4,

        /// <summary>
        /// A top-level window is being replaced. The window exists when the system calls this hook.
        /// </summary>
        WM_HSHELL_WINDOWREPLACED = 13,

        #endregion http://pinvoke.net/default.aspx/Enums/WindowsMessages.html

        ACM_OPENA = 1124,
        ACM_OPENW = 1127,
        ACM_PLAY = 1125,
        ACM_STOP = 1126,
        BFFM_ENABLEOK = 1125,
        BFFM_SETSELECTIONA = 1126,
        BFFM_SETSELECTIONW = 1127,
        BFFM_SETSTATUSTEXTA = 1124,
        BFFM_SETSTATUSTEXTW = 1128,
        BM_CLICK = 245,
        BM_GETCHECK = 240,
        BM_GETIMAGE = 246,
        BM_GETSTATE = 242,
        BM_SETCHECK = 241,
        BM_SETDONTCLICK = 248,
        BM_SETIMAGE = 247,
        BM_SETSTATE = 243,
        BM_SETSTYLE = 244,
        CBEM_GETCOMBOCONTROL = 1030,
        CBEM_GETEDITCONTROL = 1031,
        CBEM_GETEXSTYLE = 1033,
        CBEM_GETEXTENDEDSTYLE = 1033,
        CBEM_GETIMAGELIST = 1027,
        CBEM_GETITEMA = 1028,
        CBEM_GETITEMW = 1037,
        CBEM_HASEDITCHANGED = 1034,
        CBEM_INSERTITEMA = 1025,
        CBEM_INSERTITEMW = 1035,
        CBEM_SETEXSTYLE = 1032,
        CBEM_SETEXTENDEDSTYLE = 1038,
        CBEM_SETIMAGELIST = 1026,
        CBEM_SETITEMA = 1029,
        CBEM_SETITEMW = 1036,
        CDM_FIRST = 1124,
        CDM_GETFILEPATH = 1125,
        CDM_GETFOLDERIDLIST = 1127,
        CDM_GETFOLDERPATH = 1126,
        CDM_GETSPEC = 1124,
        CDM_HIDECONTROL = 1129,
        CDM_LAST = 1224,
        CDM_SETCONTROLTEXT = 1128,
        CDM_SETDEFEXT = 1130,
        DDM_BEGIN = 1027,
        DDM_CLOSE = 1026,
        DDM_DRAW = 1025,
        DDM_END = 1028,
        DDM_SETFMT = 1024,
        DL_BEGINDRAG = 1157,
        DL_CANCELDRAG = 1160,
        DL_DRAGGING = 1158,
        DL_DROPPED = 1159,
        DM_GETDEFID = 1024,
        DM_REPOSITION = 1026,
        DM_SETDEFID = 1025,
        EM_AUTOURLDETECT = 1115,
        EM_CANPASTE = 1074,
        EM_CANREDO = 1109,
        EM_CANUNDO = 198,
        EM_CHARFROMPOS = 215,
        EM_CONVPOSITION = 1132,
        EM_DISPLAYBAND = 1075,
        EM_EMPTYUNDOBUFFER = 205,
        EM_EXGETSEL = 1076,
        EM_EXLIMITTEXT = 1077,
        EM_EXLINEFROMCHAR = 1078,
        EM_EXSETSEL = 1079,
        EM_FINDTEXT = 1080,
        EM_FINDTEXTEX = 1103,
        EM_FINDTEXTEXW = 1148,
        EM_FINDTEXTW = 1147,
        EM_FINDWORDBREAK = 1100,
        EM_FMTLINES = 200,
        EM_FORMATRANGE = 1081,
        EM_GETAUTOURLDETECT = 1116,
        EM_GETBIDIOPTIONS = 1225,
        EM_GETCHARFORMAT = 1082,
        EM_GETCTFMODEBIAS = 1261,
        EM_GETCTFOPENSTATUS = 1264,
        EM_GETEDITSTYLE = 1229,
        EM_GETEVENTMASK = 1083,
        EM_GETFIRSTVISIBLELINE = 206,
        EM_GETHANDLE = 191,
        EM_GETHYPHENATEINFO = 1254,
        EM_GETIMECOLOR = 1129,
        EM_GETIMECOMPMODE = 1146,
        EM_GETIMECOMPTEXT = 1266,
        EM_GETIMEMODEBIAS = 1151,
        EM_GETIMEOPTIONS = 1131,
        EM_GETIMEPROPERTY = 1268,
        EM_GETIMESTATUS = 217,
        EM_GETLANGOPTIONS = 1145,
        EM_GETLIMITTEXT = 213,
        EM_GETLINE = 196,
        EM_GETLINECOUNT = 188,
        EM_GETMARGINS = 212,
        EM_GETMODIFY = 185,
        EM_GETOLEINTERFACE = 1084,
        EM_GETOPTIONS = 1102,
        EM_GETPAGE = 1252,
        EM_GETPAGEROTATE = 1259,
        EM_GETPARAFORMAT = 1085,
        EM_GETPASSWORDCHAR = 210,
        EM_GETPUNCTUATION = 1125,
        EM_GETQUERYRTFOBJ = 1293,
        EM_GETRECT = 178,
        EM_GETREDONAME = 1111,
        EM_GETSCROLLPOS = 1245,
        EM_GETSEL = 176,
        EM_GETSELTEXT = 1086,
        EM_GETTEXTEX = 1118,
        EM_GETTEXTLENGTHEX = 1119,
        EM_GETTEXTMODE = 1114,
        EM_GETTEXTRANGE = 1099,
        EM_GETTHUMB = 192,
        EM_GETTYPOGRAPHYOPTIONS = 1227,
        EM_GETUNDONAME = 1110,
        EM_GETVIEWKIND = 1250,
        EM_GETWORDBREAKPROC = 209,
        EM_GETWORDBREAKPROCEX = 1104,
        EM_GETWORDWRAPMODE = 1127,
        EM_GETZOOM = 1248,
        EM_HIDESELECTION = 1087,
        EM_ISIME = 1267,
        EM_LIMITTEXT = 197,
        EM_LINEFROMCHAR = 201,
        EM_LINEINDEX = 189,
        EM_LINELENGTH = 193,
        EM_LINESCROLL = 182,
        EM_OUTLINE = 1244,
        EM_PASTESPECIAL = 1088,
        EM_POSFROMCHAR = 214,
        EM_RECONVERSION = 1149,
        EM_REDO = 1108,
        EM_REPLACESEL = 194,
        EM_REQUESTRESIZE = 1089,
        EM_SCROLL = 181,
        EM_SCROLLCARET = 183,
        EM_SELECTIONTYPE = 1090,
        EM_SETBIDIOPTIONS = 1224,
        EM_SETBKGNDCOLOR = 1091,
        EM_SETCHARFORMAT = 1092,
        EM_SETCTFMODEBIAS = 1262,
        EM_SETCTFOPENSTATUS = 1265,
        EM_SETEDITSTYLE = 1228,
        EM_SETEVENTMASK = 1093,
        EM_SETFONT = 195,
        EM_SETFONTSIZE = 1247,
        EM_SETHANDLE = 190,
        EM_SETHYPHENATEINFO = 1255,
        EM_SETIMECOLOR = 1128,
        EM_SETIMEMODEBIAS = 1150,
        EM_SETIMEOPTIONS = 1130,
        EM_SETIMESTATUS = 216,
        EM_SETLANGOPTIONS = 1144,
        EM_SETLIMITTEXT = 197,
        EM_SETMARGINS = 211,
        EM_SETMODIFY = 187,
        EM_SETOLECALLBACK = 1094,
        EM_SETOPTIONS = 1101,
        EM_SETPAGE = 1253,
        EM_SETPAGEROTATE = 1260,
        EM_SETPALETTE = 1117,
        EM_SETPARAFORMAT = 1095,
        EM_SETPASSWORDCHAR = 204,
        EM_SETPUNCTUATION = 1124,
        EM_SETQUERYRTFOBJ = 1294,
        EM_SETREADONLY = 207,
        EM_SETRECT = 179,
        EM_SETRECTNP = 180,
        EM_SETSCROLLPOS = 1246,
        EM_SETSEL = 177,
        EM_SETTABSTOPS = 203,
        EM_SETTARGETDEVICE = 1096,
        EM_SETTEXTEX = 1121,
        EM_SETTEXTMODE = 1113,
        EM_SETTYPOGRAPHYOPTIONS = 1226,
        EM_SETUNDOLIMIT = 1106,
        EM_SETVIEWKIND = 1251,
        EM_SETWORDBREAK = 202,
        EM_SETWORDBREAKPROC = 209,
        EM_SETWORDBREAKPROCEX = 1105,
        EM_SETWORDWRAPMODE = 1126,
        EM_SETZOOM = 1249,
        EM_SHOWSCROLLBAR = 1120,
        EM_STOPGROUPTYPING = 1112,
        EM_STREAMIN = 1097,
        EM_STREAMOUT = 1098,
        EM_UNDO = 199,
        FM_GETDRIVEINFOA = 1537,
        FM_GETDRIVEINFOW = 1553,
        FM_GETFILESELA = 1540,
        FM_GETFILESELLFNA = 1541,
        FM_GETFILESELLFNW = 1557,
        FM_GETFILESELW = 1556,
        FM_GETFOCUS = 1536,
        FM_GETSELCOUNT = 1538,
        FM_GETSELCOUNTLFN = 1539,
        FM_REFRESH_WINDOWS = 1542,
        FM_RELOAD_EXTENSIONS = 1543,
        HKM_GETHOTKEY = 1026,
        HKM_SETHOTKEY = 1025,
        HKM_SETRULES = 1027,
        IE_DOCOMMAND = 1224,
        IE_GETAPPDATA = 1208,
        IE_GETBKGND = 1180,
        IE_GETCOMMAND = 1225,
        IE_GETCOUNT = 1226,
        IE_GETDRAWOPTS = 1210,
        IE_GETERASERTIP = 1178,
        IE_GETFORMAT = 1212,
        IE_GETGESTURE = 1227,
        IE_GETGRIDORIGIN = 1182,
        IE_GETGRIDPEN = 1184,
        IE_GETGRIDSIZE = 1186,
        IE_GETINK = 1174,
        IE_GETINKINPUT = 1214,
        IE_GETINKRECT = 1190,
        IE_GETMENU = 1228,
        IE_GETMODE = 1188,
        IE_GETNOTIFY = 1216,
        IE_GETPAINTDC = 1229,
        IE_GETPDEVENT = 1230,
        IE_GETPENTIP = 1176,
        IE_GETRECOG = 1218,
        IE_GETSECURITY = 1220,
        IE_GETSEL = 1222,
        IE_GETSELCOUNT = 1231,
        IE_GETSELITEMS = 1232,
        IE_GETSTYLE = 1233,
        IE_MSGFIRST = 1174,
        IE_SETAPPDATA = 1209,
        IE_SETBKGND = 1181,
        IE_SETDRAWOPTS = 1211,
        IE_SETERASERTIP = 1179,
        IE_SETFORMAT = 1213,
        IE_SETGRIDORIGIN = 1183,
        IE_SETGRIDPEN = 1185,
        IE_SETGRIDSIZE = 1187,
        IE_SETINK = 1175,
        IE_SETINKINPUT = 1215,
        IE_SETMODE = 1189,
        IE_SETNOTIFY = 1217,
        IE_SETPENTIP = 1177,
        IE_SETRECOG = 1219,
        IE_SETSECURITY = 1221,
        IE_SETSEL = 1223,
        IPM_CLEARADDRESS = 1124,
        IPM_GETADDRESS = 1126,
        IPM_ISBLANK = 1129,
        IPM_SETADDRESS = 1125,
        IPM_SETFOCUS = 1128,
        IPM_SETRANGE = 1127,
        LVM_APPROXIMATEVIEWRECT = 4160,
        LVM_ARRANGE = 4118,
        LVM_CANCELEDITLABEL = 4275,
        LVM_CREATEDRAGIMAGE = 4129,
        LVM_DELETEALLITEMS = 4105,
        LVM_DELETECOLUMN = 4124,
        LVM_DELETEITEM = 4104,
        LVM_EDITLABELA = 4119,
        LVM_EDITLABELW = 4214,
        LVM_ENABLEGROUPVIEW = 4253,
        LVM_ENSUREVISIBLE = 4115,
        LVM_FINDITEMA = 4109,
        LVM_FINDITEMW = 4179,
        LVM_FIRST = 4096,
        LVM_GETBKCOLOR = 4096,
        LVM_GETBKIMAGEA = 4165,
        LVM_GETBKIMAGEW = 4235,
        LVM_GETCALLBACKMASK = 4106,
        LVM_GETCOLUMNA = 4121,
        LVM_GETCOLUMNORDERARRAY = 4155,
        LVM_GETCOLUMNW = 4191,
        LVM_GETCOLUMNWIDTH = 4125,
        LVM_GETCOUNTPERPAGE = 4136,
        LVM_GETEDITCONTROL = 4120,
        LVM_GETEXTENDEDLISTVIEWSTYLE = 4151,
        LVM_GETGROUPINFO = 4245,
        LVM_GETGROUPMETRICS = 4252,
        LVM_GETHEADER = 4127,
        LVM_GETHOTCURSOR = 4159,
        LVM_GETHOTITEM = 4157,
        LVM_GETHOVERTIME = 4168,
        LVM_GETIMAGELIST = 4098,
        LVM_GETINSERTMARK = 4263,
        LVM_GETINSERTMARKCOLOR = 4267,
        LVM_GETINSERTMARKRECT = 4265,
        LVM_GETISEARCHSTRINGA = 4148,
        LVM_GETISEARCHSTRINGW = 4213,
        LVM_GETITEMA = 4101,
        LVM_GETITEMCOUNT = 4100,
        LVM_GETITEMPOSITION = 4112,
        LVM_GETITEMRECT = 4110,
        LVM_GETITEMSPACING = 4147,
        LVM_GETITEMSTATE = 4140,
        LVM_GETITEMTEXTA = 4141,
        LVM_GETITEMTEXTW = 4211,
        LVM_GETITEMW = 4171,
        LVM_GETNEXTITEM = 4108,
        LVM_GETNUMBEROFWORKAREAS = 4169,
        LVM_GETORIGIN = 4137,
        LVM_GETOUTLINECOLOR = 4272,
        LVM_GETSELECTEDCOLUMN = 4270,
        LVM_GETSELECTEDCOUNT = 4146,
        LVM_GETSELECTIONMARK = 4162,
        LVM_GETSTRINGWIDTHA = 4113,
        LVM_GETSTRINGWIDTHW = 4183,
        LVM_GETSUBITEMRECT = 4152,
        LVM_GETTEXTBKCOLOR = 4133,
        LVM_GETTEXTCOLOR = 4131,
        LVM_GETTILEINFO = 4261,
        LVM_GETTILEVIEWINFO = 4259,
        LVM_GETTOOLTIPS = 4174,
        LVM_GETTOPINDEX = 4135,
        LVM_GETUNICODEFORMAT = 8198,
        LVM_GETVIEW = 4239,
        LVM_GETVIEWRECT = 4130,
        LVM_GETWORKAREAS = 4166,
        LVM_HASGROUP = 4257,
        LVM_HITTEST = 4114,
        LVM_INSERTCOLUMNA = 4123,
        LVM_INSERTCOLUMNW = 4193,
        LVM_INSERTGROUP = 4241,
        LVM_INSERTGROUPSORTED = 4255,
        LVM_INSERTITEMA = 4103,
        LVM_INSERTITEMW = 4173,
        LVM_INSERTMARKHITTEST = 4264,
        LVM_ISGROUPVIEWENABLED = 4271,
        LVM_ISITEMVISIBLE = 4278,
        LVM_MAPIDTOINDEX = 4277,
        LVM_MAPINDEXTOID = 4276,
        LVM_MOVEGROUP = 4247,
        LVM_MOVEITEMTOGROUP = 4250,
        LVM_REDRAWITEMS = 4117,
        LVM_REMOVEALLGROUPS = 4256,
        LVM_REMOVEGROUP = 4246,
        LVM_SCROLL = 4116,
        LVM_SETBKCOLOR = 4097,
        LVM_SETBKIMAGEA = 4164,
        LVM_SETCALLBACKMASK = 4107,
        LVM_SETCOLUMNA = 4122,
        LVM_SETCOLUMNORDERARRAY = 4154,
        LVM_SETCOLUMNW = 4192,
        LVM_SETCOLUMNWIDTH = 4126,
        LVM_SETEXTENDEDLISTVIEWSTYLE = 4150,
        LVM_SETGROUPINFO = 4243,
        LVM_SETGROUPMETRICS = 4251,
        LVM_SETHOTCURSOR = 4158,
        LVM_SETHOTITEM = 4156,
        LVM_SETHOVERTIME = 4167,
        LVM_SETICONSPACING = 4149,
        LVM_SETIMAGELIST = 4099,
        LVM_SETINFOTIP = 4269,
        LVM_SETINSERTMARK = 4262,
        LVM_SETINSERTMARKCOLOR = 4266,
        LVM_SETITEMA = 4102,
        LVM_SETITEMCOUNT = 4143,
        LVM_SETITEMPOSITION = 4111,
        LVM_SETITEMPOSITION32 = 4145,
        LVM_SETITEMSTATE = 4139,
        LVM_SETITEMTEXTA = 4142,
        LVM_SETITEMTEXTW = 4212,
        LVM_SETITEMW = 4172,
        LVM_SETOUTLINECOLOR = 4273,
        LVM_SETSELECTEDCOLUMN = 4236,
        LVM_SETSELECTIONMARK = 4163,
        LVM_SETTEXTBKCOLOR = 4134,
        LVM_SETTEXTCOLOR = 4132,
        LVM_SETTILEINFO = 4260,
        LVM_SETTILEVIEWINFO = 4258,
        LVM_SETTILEWIDTH = 4237,
        LVM_SETTOOLTIPS = 4170,
        LVM_SETUNICODEFORMAT = 8197,
        LVM_SETVIEW = 4238,
        LVM_SETWORKAREAS = 4161,
        LVM_SORTGROUPS = 4254,
        LVM_SORTITEMS = 4144,
        LVM_SUBITEMHITTEST = 4153,
        LVM_UPDATE = 4138,
        MCIWNDM_CAN_CONFIG = 1173,
        MCIWNDM_CAN_EJECT = 1172,
        MCIWNDM_CAN_PLAY = 1168,
        MCIWNDM_CAN_RECORD = 1170,
        MCIWNDM_CAN_SAVE = 1171,
        MCIWNDM_CAN_WINDOW = 1169,
        MCIWNDM_GET_DEST = 1166,
        MCIWNDM_GET_SOURCE = 1164,
        MCIWNDM_GETDEVICEA = 1149,
        MCIWNDM_GETDEVICEW = 1249,
        MCIWNDM_GETERRORA = 1152,
        MCIWNDM_GETERRORW = 1252,
        MCIWNDM_GETFILENAMEA = 1148,
        MCIWNDM_GETFILENAMEW = 1248,
        MCIWNDM_GETINACTIVETIMER = 1157,
        MCIWNDM_GETPALETTE = 1150,
        MCIWNDM_GETTIMEFORMATA = 1144,
        MCIWNDM_GETTIMEFORMATW = 1244,
        MCIWNDM_GETZOOM = 1133,
        MCIWNDM_NOTIFYERROR = 1229,
        MCIWNDM_NOTIFYMEDIA = 1227,
        MCIWNDM_NOTIFYMODE = 1224,
        MCIWNDM_PALETTEKICK = 1174,
        MCIWNDM_PLAYTO = 1147,
        MCIWNDM_PUT_DEST = 1167,
        MCIWNDM_PUT_SOURCE = 1165,
        MCIWNDM_REALIZE = 1142,
        MCIWNDM_SETINACTIVETIMER = 1155,
        MCIWNDM_SETPALETTE = 1151,
        MCIWNDM_SETTIMEFORMATA = 1143,
        MCIWNDM_SETTIMEFORMATW = 1243,
        MCIWNDM_VALIDATEMEDIA = 1145,
        MSG_FTS_JUMP_QWORD = 1059,
        MSG_FTS_JUMP_VA = 1057,
        MSG_FTS_WHERE_IS_IT = 1061,
        MSG_GET_DEFFONT = 1069,
        MSG_REINDEX_REQUEST = 1060,
        NIN_SELECT = 1024,
        OCM__BASE = 8192,
        OCM_CHARTOITEM = 8239,
        OCM_COMMAND = 8465,
        OCM_COMPAREITEM = 8249,
        OCM_CTLCOLOR = 8217,
        OCM_CTLCOLORBTN = 8501,
        OCM_CTLCOLORDLG = 8502,
        OCM_CTLCOLOREDIT = 8499,
        OCM_CTLCOLORLISTBOX = 8500,
        OCM_CTLCOLORMSGBOX = 8498,
        OCM_CTLCOLORSCROLLBAR = 8503,
        OCM_CTLCOLORSTATIC = 8504,
        OCM_DELETEITEM = 8237,
        OCM_DRAWITEM = 8235,
        OCM_HSCROLL = 8468,
        OCM_MEASUREITEM = 8236,
        OCM_NOTIFY = 8270,
        OCM_PARENTNOTIFY = 8720,
        OCM_VKEYTOITEM = 8238,
        OCM_VSCROLL = 8469,
        PBM_DELTAPOS = 1027,
        PBM_GETPOS = 1032,
        PBM_GETRANGE = 1031,
        PBM_SETBARCOLOR = 1033,
        PBM_SETPOS = 1026,
        PBM_SETRANGE = 1025,
        PBM_SETRANGE32 = 1030,
        PBM_SETSTEP = 1028,
        PBM_STEPIT = 1029,
        PSM_ADDPAGE = 1127,
        PSM_APPLY = 1134,
        PSM_CANCELTOCLOSE = 1131,
        PSM_CHANGED = 1128,
        PSM_GETCURRENTPAGEHWND = 1142,
        PSM_GETRESULT = 1159,
        PSM_GETTABCONTROL = 1140,
        PSM_HWNDTOINDEX = 1153,
        PSM_IDTOINDEX = 1157,
        PSM_INDEXTOHWND = 1154,
        PSM_INDEXTOID = 1158,
        PSM_INDEXTOPAGE = 1156,
        PSM_INSERTPAGE = 1143,
        PSM_ISDIALOGMESSAGE = 1141,
        PSM_PAGETOINDEX = 1155,
        PSM_PRESSBUTTON = 1137,
        PSM_QUERYSIBLINGS = 1132,
        PSM_REBOOTSYSTEM = 1130,
        PSM_RECALCPAGESIZES = 1160,
        PSM_REMOVEPAGE = 1126,
        PSM_RESTARTWINDOWS = 1129,
        PSM_SETCURSEL = 1125,
        PSM_SETCURSELID = 1138,
        PSM_SETFINISHTEXTA = 1139,
        PSM_SETFINISHTEXTW = 1145,
        PSM_SETHEADERSUBTITLEA = 1151,
        PSM_SETHEADERSUBTITLEW = 1152,
        PSM_SETHEADERTITLEA = 1149,
        PSM_SETHEADERTITLEW = 1150,
        PSM_SETTITLEA = 1135,
        PSM_SETTITLEW = 1144,
        PSM_SETWIZBUTTONS = 1136,
        PSM_UNCHANGED = 1133,
        RB_BEGINDRAG = 1048,
        RB_DELETEBAND = 1026,
        RB_DRAGMOVE = 1050,
        RB_ENDDRAG = 1049,
        RB_GETBANDBORDERS = 1058,
        RB_GETBANDCOUNT = 1036,
        RB_GETBANDINFOA = 1053,
        RB_GETBANDINFOW = 1052,
        RB_GETBARHEIGHT = 1051,
        RB_GETBARINFO = 1027,
        RB_GETBKCOLOR = 1044,
        RB_GETPALETTE = 1062,
        RB_GETRECT = 1033,
        RB_GETROWCOUNT = 1037,
        RB_GETROWHEIGHT = 1038,
        RB_GETTEXTCOLOR = 1046,
        RB_GETTOOLTIPS = 1041,
        RB_HITTEST = 1032,
        RB_IDTOINDEX = 1040,
        RB_INSERTBANDA = 1025,
        RB_INSERTBANDW = 1034,
        RB_MAXIMIZEBAND = 1055,
        RB_MINIMIZEBAND = 1054,
        RB_MOVEBAND = 1063,
        RB_PUSHCHEVRON = 1067,
        RB_SETBANDINFOA = 1030,
        RB_SETBANDINFOW = 1035,
        RB_SETBARINFO = 1028,
        RB_SETBKCOLOR = 1043,
        RB_SETPALETTE = 1061,
        RB_SETPARENT = 1031,
        RB_SETTEXTCOLOR = 1045,
        RB_SETTOOLTIPS = 1042,
        RB_SHOWBAND = 1059,
        RB_SIZETORECT = 1047,
        SB_GETBORDERS = 1031,
        SB_GETICON = 1044,
        SB_GETPARTS = 1030,
        SB_GETRECT = 1034,
        SB_GETTEXTA = 1026,
        SB_GETTEXTLENGTHA = 1027,
        SB_GETTEXTLENGTHW = 1036,
        SB_GETTEXTW = 1037,
        SB_GETTIPTEXTA = 1042,
        SB_GETTIPTEXTW = 1043,
        SB_ISSIMPLE = 1038,
        SB_SETICON = 1039,
        SB_SETMINHEIGHT = 1032,
        SB_SETPARTS = 1028,
        SB_SETTEXTA = 1025,
        SB_SETTEXTW = 1035,
        SB_SETTIPTEXTA = 1040,
        SB_SETTIPTEXTW = 1041,
        SB_SIMPLE = 1033,
        SBM_ENABLE_ARROWS = 228,
        SBM_GETPOS = 225,
        SBM_GETRANGE = 227,
        SBM_GETSCROLLBARINFO = 235,
        SBM_GETSCROLLINFO = 234,
        SBM_SETPOS = 224,
        SBM_SETRANGE = 226,
        SBM_SETRANGEREDRAW = 230,
        SBM_SETSCROLLINFO = 233,
        SM_GETCURFOCUSA = 2027,
        SM_GETCURFOCUSW = 2028,
        SM_GETOPTIONS = 2029,
        SM_GETSELCOUNT = 2024,
        SM_GETSERVERSELA = 2025,
        SM_GETSERVERSELW = 2026,
        TAPI_REPLY = 1123,
        TB_ADDBITMAP = 1043,
        TB_ADDBUTTONSA = 1044,
        TB_ADDBUTTONSW = 1092,
        TB_ADDSTRINGA = 1052,
        TB_ADDSTRINGW = 1101,
        TB_AUTOSIZE = 1057,
        TB_BUTTONCOUNT = 1048,
        TB_BUTTONSTRUCTSIZE = 1054,
        TB_CHANGEBITMAP = 1067,
        TB_CHECKBUTTON = 1026,
        TB_COMMANDTOINDEX = 1049,
        TB_CUSTOMIZE = 1051,
        TB_DELETEBUTTON = 1046,
        TB_ENABLEBUTTON = 1025,
        TB_GETANCHORHIGHLIGHT = 1098,
        TB_GETBITMAP = 1068,
        TB_GETBITMAPFLAGS = 1065,
        TB_GETBUTTON = 1047,
        TB_GETBUTTONINFOA = 1089,
        TB_GETBUTTONINFOW = 1087,
        TB_GETBUTTONSIZE = 1082,
        TB_GETBUTTONTEXTA = 1069,
        TB_GETBUTTONTEXTW = 1099,
        TB_GETDISABLEDIMAGELIST = 1079,
        TB_GETEXTENDEDSTYLE = 1109,
        TB_GETHOTIMAGELIST = 1077,
        TB_GETHOTITEM = 1095,
        TB_GETIMAGELIST = 1073,
        TB_GETINSERTMARK = 1103,
        TB_GETINSERTMARKCOLOR = 1113,
        TB_GETITEMRECT = 1053,
        TB_GETMAXSIZE = 1107,
        TB_GETOBJECT = 1086,
        TB_GETPADDING = 1110,
        TB_GETRECT = 1075,
        TB_GETROWS = 1064,
        TB_GETSTATE = 1042,
        TB_GETSTRINGA = 1116,
        TB_GETSTRINGW = 1115,
        TB_GETSTYLE = 1081,
        TB_GETTEXTROWS = 1085,
        TB_GETTOOLTIPS = 1059,
        TB_HIDEBUTTON = 1028,
        TB_HITTEST = 1093,
        TB_INDETERMINATE = 1029,
        TB_INSERTBUTTONA = 1045,
        TB_INSERTBUTTONW = 1091,
        TB_INSERTMARKHITTEST = 1105,
        TB_ISBUTTONCHECKED = 1034,
        TB_ISBUTTONENABLED = 1033,
        TB_ISBUTTONHIDDEN = 1036,
        TB_ISBUTTONHIGHLIGHTED = 1038,
        TB_ISBUTTONINDETERMINATE = 1037,
        TB_ISBUTTONPRESSED = 1035,
        TB_LOADIMAGES = 1074,
        TB_MAPACCELERATORA = 1102,
        TB_MAPACCELERATORW = 1114,
        TB_MARKBUTTON = 1030,
        TB_MOVEBUTTON = 1106,
        TB_PRESSBUTTON = 1027,
        TB_REPLACEBITMAP = 1070,
        TB_SAVERESTOREA = 1050,
        TB_SAVERESTOREW = 1100,
        TB_SETANCHORHIGHLIGHT = 1097,
        TB_SETBITMAPSIZE = 1056,
        TB_SETBUTTONINFOA = 1090,
        TB_SETBUTTONINFOW = 1088,
        TB_SETBUTTONSIZE = 1055,
        TB_SETBUTTONWIDTH = 1083,
        TB_SETCMDID = 1066,
        TB_SETDISABLEDIMAGELIST = 1078,
        TB_SETDRAWTEXTFLAGS = 1094,
        TB_SETEXTENDEDSTYLE = 1108,
        TB_SETHOTIMAGELIST = 1076,
        TB_SETHOTITEM = 1096,
        TB_SETIMAGELIST = 1072,
        TB_SETINDENT = 1071,
        TB_SETINSERTMARK = 1104,
        TB_SETINSERTMARKCOLOR = 1112,
        TB_SETMAXTEXTROWS = 1084,
        TB_SETPADDING = 1111,
        TB_SETPARENT = 1061,
        TB_SETROWS = 1063,
        TB_SETSTATE = 1041,
        TB_SETSTYLE = 1080,
        TB_SETTOOLTIPS = 1060,
        TBM_CLEARSEL = 1043,
        TBM_CLEARTICS = 1033,
        TBM_GETBUDDY = 1057,
        TBM_GETCHANNELRECT = 1050,
        TBM_GETLINESIZE = 1048,
        TBM_GETNUMTICS = 1040,
        TBM_GETPAGESIZE = 1046,
        TBM_GETPOS = 1024,
        TBM_GETPTICS = 1038,
        TBM_GETRANGEMAX = 1026,
        TBM_GETRANGEMIN = 1025,
        TBM_GETSELEND = 1042,
        TBM_GETSELSTART = 1041,
        TBM_GETTHUMBLENGTH = 1052,
        TBM_GETTHUMBRECT = 1049,
        TBM_GETTIC = 1027,
        TBM_GETTICPOS = 1039,
        TBM_GETTOOLTIPS = 1054,
        TBM_SETBUDDY = 1056,
        TBM_SETLINESIZE = 1047,
        TBM_SETPAGESIZE = 1045,
        TBM_SETPOS = 1029,
        TBM_SETRANGE = 1030,
        TBM_SETRANGEMAX = 1032,
        TBM_SETRANGEMIN = 1031,
        TBM_SETSEL = 1034,
        TBM_SETSELEND = 1036,
        TBM_SETSELSTART = 1035,
        TBM_SETTHUMBLENGTH = 1051,
        TBM_SETTIC = 1028,
        TBM_SETTICFREQ = 1044,
        TBM_SETTIPSIDE = 1055,
        TBM_SETTOOLTIPS = 1053,
        TTM_ACTIVATE = 1025,
        TTM_ADDTOOLA = 1028,
        TTM_ADDTOOLW = 1074,
        TTM_ADJUSTRECT = 1055,
        TTM_DELTOOLA = 1029,
        TTM_DELTOOLW = 1075,
        TTM_ENUMTOOLSA = 1038,
        TTM_ENUMTOOLSW = 1082,
        TTM_GETBUBBLESIZE = 1054,
        TTM_GETCURRENTTOOLA = 1039,
        TTM_GETCURRENTTOOLW = 1083,
        TTM_GETDELAYTIME = 1045,
        TTM_GETMARGIN = 1051,
        TTM_GETMAXTIPWIDTH = 1049,
        TTM_GETTEXTA = 1035,
        TTM_GETTEXTW = 1080,
        TTM_GETTIPBKCOLOR = 1046,
        TTM_GETTIPTEXTCOLOR = 1047,
        TTM_GETTOOLCOUNT = 1037,
        TTM_GETTOOLINFOA = 1032,
        TTM_GETTOOLINFOW = 1077,
        TTM_HITTESTA = 1034,
        TTM_HITTESTW = 1079,
        TTM_NEWTOOLRECTA = 1030,
        TTM_NEWTOOLRECTW = 1076,
        TTM_POP = 1052,
        TTM_RELAYEVENT = 1031,
        TTM_SETDELAYTIME = 1027,
        TTM_SETMARGIN = 1050,
        TTM_SETMAXTIPWIDTH = 1048,
        TTM_SETTIPBKCOLOR = 1043,
        TTM_SETTIPTEXTCOLOR = 1044,
        TTM_SETTITLEA = 1056,
        TTM_SETTITLEW = 1057,
        TTM_SETTOOLINFOA = 1033,
        TTM_SETTOOLINFOW = 1078,
        TTM_TRACKACTIVATE = 1041,
        TTM_TRACKPOSITION = 1042,
        TTM_UPDATE = 1053,
        TTM_UPDATETIPTEXTA = 1036,
        TTM_UPDATETIPTEXTW = 1081,
        TTM_WINDOWFROMPOINT = 1040,
        UDM_GETACCEL = 1132,
        UDM_GETBASE = 1134,
        UDM_GETBUDDY = 1130,
        UDM_GETPOS = 1128,
        UDM_GETPOS32 = 1138,
        UDM_GETRANGE = 1126,
        UDM_GETRANGE32 = 1136,
        UDM_SETACCEL = 1131,
        UDM_SETBASE = 1133,
        UDM_SETBUDDY = 1129,
        UDM_SETPOS = 1127,
        UDM_SETPOS32 = 1137,
        UDM_SETRANGE = 1125,
        UDM_SETRANGE32 = 1135,
        UM_GETCURFOCUSA = 2029,
        UM_GETCURFOCUSW = 2030,
        UM_GETGROUPSELA = 2027,
        UM_GETGROUPSELW = 2028,
        UM_GETOPTIONS = 2031,
        UM_GETOPTIONS2 = 2032,
        UM_GETSELCOUNT = 2024,
        UM_GETUSERSELA = 2025,
        UM_GETUSERSELW = 2026,
        WIZ_NEXT = 1035,
        WIZ_PREV = 1036,
        WIZ_QUERYNUMPAGES = 1034,
        WLX_WM_SAS = 1625,
        WM_CAP_DRIVER_GET_NAMEW = 1136,
        WM_CAP_DRIVER_GET_VERSIONW = 1137,
        WM_CAP_FILE_GET_CAPTURE_FILEW = 1145,
        WM_CAP_FILE_SAVEASW = 1147,
        WM_CAP_FILE_SAVEDIBW = 1149,
        WM_CAP_FILE_SET_CAPTURE_FILEW = 1144,
        WM_CAP_GET_MCI_DEVICEW = 1191,
        WM_CAP_PAL_OPENW = 1204,
        WM_CAP_PAL_SAVEW = 1205,
        WM_CAP_SET_CALLBACK_ERRORW = 1126,
        WM_CAP_SET_CALLBACK_STATUSW = 1127,
        WM_CAP_SET_MCI_DEVICEW = 1190,
        WM_CAP_UNICODE_START = 1124,
        WM_CHOOSEFONT_GETLOGFONT = 1025,
        WM_CHOOSEFONT_SETFLAGS = 1126,
        WM_CHOOSEFONT_SETLOGFONT = 1125,
        WM_CONVERTREQUEST = 266,
        WM_CONVERTRESULT = 267,
        WM_COPYGLOBALDATA = 73,
        WM_CTLCOLOR = 25,
        WM_CTLINIT = 903,
        WM_GLOBALRCCHANGE = 899,
        WM_HEDITCTL = 901,
        WM_HOOKRCRESULT = 898,
        WM_IME_REPORT = 640,
        WM_IMEKEYDOWN = 656,
        WM_IMEKEYUP = 657,
        WM_INTERIM = 268,
        WM_PENCTL = 901,
        WM_PENEVENT = 904,
        WM_PENMISC = 902,
        WM_PENMISCINFO = 899,
        WM_PSD_ENVSTAMPRECT = 1029,
        WM_PSD_FULLPAGERECT = 1025,
        WM_PSD_GREEKTEXTRECT = 1028,
        WM_PSD_MARGINRECT = 1027,
        WM_PSD_MINMARGINRECT = 1026,
        WM_PSD_PAGESETUPDLG = 1024,
        WM_PSD_YAFULLPAGERECT = 1030,
        WM_RASDIALEVENT = 52429,
        WM_RCRESULT = 897,
        WM_SKB = 900,
        WM_WNT_CONVERTREQUESTEX = 265,
    }

    public enum WParam
    {
        #region https://www.pinvoke.net/default.aspx/Enums/VK.html

        ///<summary>
        ///Left mouse button
        ///</summary>
        VK_LBUTTON = 0x01,

        ///<summary>
        ///Right mouse button
        ///</summary>
        VK_RBUTTON = 0x02,

        ///<summary>
        ///Control-break processing
        ///</summary>
        VK_CANCEL = 0x03,

        ///<summary>
        ///Middle mouse button (three-button mouse)
        ///</summary>
        VK_MBUTTON = 0x04,

        ///<summary>
        ///Windows 2000/XP: X1 mouse button
        ///</summary>
        VK_XBUTTON1 = 0x05,

        ///<summary>
        ///Windows 2000/XP: X2 mouse button
        ///</summary>
        VK_XBUTTON2 = 0x06,

        ///<summary>
        ///BACKSPACE key
        ///</summary>
        VK_BACK = 0x08,

        ///<summary>
        ///TAB key
        ///</summary>
        VK_TAB = 0x09,

        ///<summary>
        ///CLEAR key
        ///</summary>
        VK_CLEAR = 0x0C,

        ///<summary>
        ///ENTER key
        ///</summary>
        VK_RETURN = 0x0D,

        ///<summary>
        ///SHIFT key
        ///</summary>
        VK_SHIFT = 0x10,

        ///<summary>
        ///CTRL key
        ///</summary>
        VK_CONTROL = 0x11,

        ///<summary>
        ///ALT key
        ///</summary>
        VK_MENU = 0x12,

        ///<summary>
        ///PAUSE key
        ///</summary>
        VK_PAUSE = 0x13,

        ///<summary>
        ///CAPS LOCK key
        ///</summary>
        VK_CAPITAL = 0x14,

        ///<summary>
        ///Input Method Editor (IME) Kana mode
        ///</summary>
        VK_KANA = 0x15,

        ///<summary>
        ///IME Hangul mode
        ///</summary>
        VK_HANGUL = 0x15,

        ///<summary>
        ///IME Junja mode
        ///</summary>
        VK_JUNJA = 0x17,

        ///<summary>
        ///IME final mode
        ///</summary>
        VK_FINAL = 0x18,

        ///<summary>
        ///IME Hanja mode
        ///</summary>
        VK_HANJA = 0x19,

        ///<summary>
        ///IME Kanji mode
        ///</summary>
        VK_KANJI = 0x19,

        ///<summary>
        ///ESC key
        ///</summary>
        VK_ESCAPE = 0x1B,

        ///<summary>
        ///IME convert
        ///</summary>
        VK_CONVERT = 0x1C,

        ///<summary>
        ///IME nonconvert
        ///</summary>
        VK_NONCONVERT = 0x1D,

        ///<summary>
        ///IME accept
        ///</summary>
        VK_ACCEPT = 0x1E,

        ///<summary>
        ///IME mode change request
        ///</summary>
        VK_MODECHANGE = 0x1F,

        ///<summary>
        ///SPACEBAR
        ///</summary>
        VK_SPACE = 0x20,

        ///<summary>
        ///PAGE UP key
        ///</summary>
        VK_PRIOR = 0x21,

        ///<summary>
        ///PAGE DOWN key
        ///</summary>
        VK_NEXT = 0x22,

        ///<summary>
        ///END key
        ///</summary>
        VK_END = 0x23,

        ///<summary>
        ///HOME key
        ///</summary>
        VK_HOME = 0x24,

        ///<summary>
        ///LEFT ARROW key
        ///</summary>
        VK_LEFT = 0x25,

        ///<summary>
        ///UP ARROW key
        ///</summary>
        VK_UP = 0x26,

        ///<summary>
        ///RIGHT ARROW key
        ///</summary>
        VK_RIGHT = 0x27,

        ///<summary>
        ///DOWN ARROW key
        ///</summary>
        VK_DOWN = 0x28,

        ///<summary>
        ///SELECT key
        ///</summary>
        VK_SELECT = 0x29,

        ///<summary>
        ///PRINT key
        ///</summary>
        VK_PRINT = 0x2A,

        ///<summary>
        ///EXECUTE key
        ///</summary>
        VK_EXECUTE = 0x2B,

        ///<summary>
        ///PRINT SCREEN key
        ///</summary>
        VK_SNAPSHOT = 0x2C,

        ///<summary>
        ///INS key
        ///</summary>
        VK_INSERT = 0x2D,

        ///<summary>
        ///DEL key
        ///</summary>
        VK_DELETE = 0x2E,

        ///<summary>
        ///HELP key
        ///</summary>
        VK_HELP = 0x2F,

        ///<summary>
        ///0 key
        ///</summary>
        VK_KEY_0 = 0x30,

        ///<summary>
        ///1 key
        ///</summary>
        VK_KEY_1 = 0x31,

        ///<summary>
        ///2 key
        ///</summary>
        VK_KEY_2 = 0x32,

        ///<summary>
        ///3 key
        ///</summary>
        VK_KEY_3 = 0x33,

        ///<summary>
        ///4 key
        ///</summary>
        VK_KEY_4 = 0x34,

        ///<summary>
        ///5 key
        ///</summary>
        VK_KEY_5 = 0x35,

        ///<summary>
        ///6 key
        ///</summary>
        VK_KEY_6 = 0x36,

        ///<summary>
        ///7 key
        ///</summary>
        VK_KEY_7 = 0x37,

        ///<summary>
        ///8 key
        ///</summary>
        VK_KEY_8 = 0x38,

        ///<summary>
        ///9 key
        ///</summary>
        VK_KEY_9 = 0x39,

        ///<summary>
        ///A key
        ///</summary>
        VK_KEY_A = 0x41,

        ///<summary>
        ///B key
        ///</summary>
        VK_KEY_B = 0x42,

        ///<summary>
        ///C key
        ///</summary>
        VK_KEY_C = 0x43,

        ///<summary>
        ///D key
        ///</summary>
        VK_KEY_D = 0x44,

        ///<summary>
        ///E key
        ///</summary>
        VK_KEY_E = 0x45,

        ///<summary>
        ///F key
        ///</summary>
        VK_KEY_F = 0x46,

        ///<summary>
        ///G key
        ///</summary>
        VK_KEY_G = 0x47,

        ///<summary>
        ///H key
        ///</summary>
        VK_KEY_H = 0x48,

        ///<summary>
        ///I key
        ///</summary>
        VK_KEY_I = 0x49,

        ///<summary>
        ///J key
        ///</summary>
        VK_KEY_J = 0x4A,

        ///<summary>
        ///K key
        ///</summary>
        VK_KEY_K = 0x4B,

        ///<summary>
        ///L key
        ///</summary>
        VK_KEY_L = 0x4C,

        ///<summary>
        ///M key
        ///</summary>
        VK_KEY_M = 0x4D,

        ///<summary>
        ///N key
        ///</summary>
        VK_KEY_N = 0x4E,

        ///<summary>
        ///O key
        ///</summary>
        VK_KEY_O = 0x4F,

        ///<summary>
        ///P key
        ///</summary>
        VK_KEY_P = 0x50,

        ///<summary>
        ///Q key
        ///</summary>
        VK_KEY_Q = 0x51,

        ///<summary>
        ///R key
        ///</summary>
        VK_KEY_R = 0x52,

        ///<summary>
        ///S key
        ///</summary>
        VK_KEY_S = 0x53,

        ///<summary>
        ///T key
        ///</summary>
        VK_KEY_T = 0x54,

        ///<summary>
        ///U key
        ///</summary>
        VK_KEY_U = 0x55,

        ///<summary>
        ///V key
        ///</summary>
        VK_KEY_V = 0x56,

        ///<summary>
        ///W key
        ///</summary>
        VK_KEY_W = 0x57,

        ///<summary>
        ///X key
        ///</summary>
        VK_KEY_X = 0x58,

        ///<summary>
        ///Y key
        ///</summary>
        VK_KEY_Y = 0x59,

        ///<summary>
        ///Z key
        ///</summary>
        VK_KEY_Z = 0x5A,

        ///<summary>
        ///Left Windows key (Microsoft Natural keyboard)
        ///</summary>
        VK_LWIN = 0x5B,

        ///<summary>
        ///Right Windows key (Natural keyboard)
        ///</summary>
        VK_RWIN = 0x5C,

        ///<summary>
        ///Applications key (Natural keyboard)
        ///</summary>
        VK_APPS = 0x5D,

        ///<summary>
        ///Computer Sleep key
        ///</summary>
        VK_SLEEP = 0x5F,

        ///<summary>
        ///Numeric keypad 0 key
        ///</summary>
        VK_NUMPAD0 = 0x60,

        ///<summary>
        ///Numeric keypad 1 key
        ///</summary>
        VK_NUMPAD1 = 0x61,

        ///<summary>
        ///Numeric keypad 2 key
        ///</summary>
        VK_NUMPAD2 = 0x62,

        ///<summary>
        ///Numeric keypad 3 key
        ///</summary>
        VK_NUMPAD3 = 0x63,

        ///<summary>
        ///Numeric keypad 4 key
        ///</summary>
        VK_NUMPAD4 = 0x64,

        ///<summary>
        ///Numeric keypad 5 key
        ///</summary>
        VK_NUMPAD5 = 0x65,

        ///<summary>
        ///Numeric keypad 6 key
        ///</summary>
        VK_NUMPAD6 = 0x66,

        ///<summary>
        ///Numeric keypad 7 key
        ///</summary>
        VK_NUMPAD7 = 0x67,

        ///<summary>
        ///Numeric keypad 8 key
        ///</summary>
        VK_NUMPAD8 = 0x68,

        ///<summary>
        ///Numeric keypad 9 key
        ///</summary>
        VK_NUMPAD9 = 0x69,

        ///<summary>
        ///Multiply key
        ///</summary>
        VK_MULTIPLY = 0x6A,

        ///<summary>
        ///Add key
        ///</summary>
        VK_ADD = 0x6B,

        ///<summary>
        ///Separator key
        ///</summary>
        VK_SEPARATOR = 0x6C,

        ///<summary>
        ///Subtract key
        ///</summary>
        VK_SUBTRACT = 0x6D,

        ///<summary>
        ///Decimal key
        ///</summary>
        VK_DECIMAL = 0x6E,

        ///<summary>
        ///Divide key
        ///</summary>
        VK_DIVIDE = 0x6F,

        ///<summary>
        ///F1 key
        ///</summary>
        VK_F1 = 0x70,

        ///<summary>
        ///F2 key
        ///</summary>
        VK_F2 = 0x71,

        ///<summary>
        ///F3 key
        ///</summary>
        VK_F3 = 0x72,

        ///<summary>
        ///F4 key
        ///</summary>
        VK_F4 = 0x73,

        ///<summary>
        ///F5 key
        ///</summary>
        VK_F5 = 0x74,

        ///<summary>
        ///F6 key
        ///</summary>
        VK_F6 = 0x75,

        ///<summary>
        ///F7 key
        ///</summary>
        VK_F7 = 0x76,

        ///<summary>
        ///F8 key
        ///</summary>
        VK_F8 = 0x77,

        ///<summary>
        ///F9 key
        ///</summary>
        VK_F9 = 0x78,

        ///<summary>
        ///F10 key
        ///</summary>
        VK_F10 = 0x79,

        ///<summary>
        ///F11 key
        ///</summary>
        VK_F11 = 0x7A,

        ///<summary>
        ///F12 key
        ///</summary>
        VK_F12 = 0x7B,

        ///<summary>
        ///F13 key
        ///</summary>
        VK_F13 = 0x7C,

        ///<summary>
        ///F14 key
        ///</summary>
        VK_F14 = 0x7D,

        ///<summary>
        ///F15 key
        ///</summary>
        VK_F15 = 0x7E,

        ///<summary>
        ///F16 key
        ///</summary>
        VK_F16 = 0x7F,

        ///<summary>
        ///F17 key
        ///</summary>
        VK_F17 = 0x80,

        ///<summary>
        ///F18 key
        ///</summary>
        VK_F18 = 0x81,

        ///<summary>
        ///F19 key
        ///</summary>
        VK_F19 = 0x82,

        ///<summary>
        ///F20 key
        ///</summary>
        VK_F20 = 0x83,

        ///<summary>
        ///F21 key
        ///</summary>
        VK_F21 = 0x84,

        ///<summary>
        ///F22 key, (PPC only) Key used to lock device.
        ///</summary>
        VK_F22 = 0x85,

        ///<summary>
        ///F23 key
        ///</summary>
        VK_F23 = 0x86,

        ///<summary>
        ///F24 key
        ///</summary>
        VK_F24 = 0x87,

        ///<summary>
        ///NUM LOCK key
        ///</summary>
        VK_NUMLOCK = 0x90,

        ///<summary>
        ///SCROLL LOCK key
        ///</summary>
        VK_SCROLL = 0x91,

        ///<summary>
        ///Left SHIFT key
        ///</summary>
        VK_LSHIFT = 0xA0,

        ///<summary>
        ///Right SHIFT key
        ///</summary>
        VK_RSHIFT = 0xA1,

        ///<summary>
        ///Left CONTROL key
        ///</summary>
        VK_LCONTROL = 0xA2,

        ///<summary>
        ///Right CONTROL key
        ///</summary>
        VK_RCONTROL = 0xA3,

        ///<summary>
        ///Left MENU key
        ///</summary>
        VK_LMENU = 0xA4,

        ///<summary>
        ///Right MENU key
        ///</summary>
        VK_RMENU = 0xA5,

        ///<summary>
        ///Windows 2000/XP: Browser Back key
        ///</summary>
        VK_BROWSER_BACK = 0xA6,

        ///<summary>
        ///Windows 2000/XP: Browser Forward key
        ///</summary>
        VK_BROWSER_FORWARD = 0xA7,

        ///<summary>
        ///Windows 2000/XP: Browser Refresh key
        ///</summary>
        VK_BROWSER_REFRESH = 0xA8,

        ///<summary>
        ///Windows 2000/XP: Browser Stop key
        ///</summary>
        VK_BROWSER_STOP = 0xA9,

        ///<summary>
        ///Windows 2000/XP: Browser Search key
        ///</summary>
        VK_BROWSER_SEARCH = 0xAA,

        ///<summary>
        ///Windows 2000/XP: Browser Favorites key
        ///</summary>
        VK_BROWSER_FAVORITES = 0xAB,

        ///<summary>
        ///Windows 2000/XP: Browser Start and Home key
        ///</summary>
        VK_BROWSER_HOME = 0xAC,

        ///<summary>
        ///Windows 2000/XP: Volume Mute key
        ///</summary>
        VK_VOLUME_MUTE = 0xAD,

        ///<summary>
        ///Windows 2000/XP: Volume Down key
        ///</summary>
        VK_VOLUME_DOWN = 0xAE,

        ///<summary>
        ///Windows 2000/XP: Volume Up key
        ///</summary>
        VK_VOLUME_UP = 0xAF,

        ///<summary>
        ///Windows 2000/XP: Next Track key
        ///</summary>
        VK_MEDIA_NEXT_TRACK = 0xB0,

        ///<summary>
        ///Windows 2000/XP: Previous Track key
        ///</summary>
        VK_MEDIA_PREV_TRACK = 0xB1,

        ///<summary>
        ///Windows 2000/XP: Stop Media key
        ///</summary>
        VK_MEDIA_STOP = 0xB2,

        ///<summary>
        ///Windows 2000/XP: Play/Pause Media key
        ///</summary>
        VK_MEDIA_PLAY_PAUSE = 0xB3,

        ///<summary>
        ///Windows 2000/XP: Start Mail key
        ///</summary>
        VK_LAUNCH_MAIL = 0xB4,

        ///<summary>
        ///Windows 2000/XP: Select Media key
        ///</summary>
        VK_LAUNCH_MEDIA_SELECT = 0xB5,

        ///<summary>
        ///Windows 2000/XP: Start Application 1 key
        ///</summary>
        VK_LAUNCH_APP1 = 0xB6,

        ///<summary>
        ///Windows 2000/XP: Start Application 2 key
        ///</summary>
        VK_LAUNCH_APP2 = 0xB7,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        VK_OEM_1 = 0xBA,

        ///<summary>
        ///Windows 2000/XP: For any country/region, the '+' key
        ///</summary>
        VK_OEM_PLUS = 0xBB,

        ///<summary>
        ///Windows 2000/XP: For any country/region, the ',' key
        ///</summary>
        VK_OEM_COMMA = 0xBC,

        ///<summary>
        ///Windows 2000/XP: For any country/region, the '-' key
        ///</summary>
        VK_OEM_MINUS = 0xBD,

        ///<summary>
        ///Windows 2000/XP: For any country/region, the '.' key
        ///</summary>
        VK_OEM_PERIOD = 0xBE,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        VK_OEM_2 = 0xBF,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        VK_OEM_3 = 0xC0,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        VK_OEM_4 = 0xDB,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        VK_OEM_5 = 0xDC,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        VK_OEM_6 = 0xDD,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        VK_OEM_7 = 0xDE,

        ///<summary>
        ///Used for miscellaneous characters; it can vary by keyboard.
        ///</summary>
        VK_OEM_8 = 0xDF,

        ///<summary>
        ///Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
        ///</summary>
        VK_OEM_102 = 0xE2,

        ///<summary>
        ///Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
        ///</summary>
        VK_PROCESSKEY = 0xE5,

        ///<summary>
        ///Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes. The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
        ///</summary>
        VK_PACKET = 0xE7,

        ///<summary>
        ///Attn key
        ///</summary>
        VK_ATTN = 0xF6,

        ///<summary>
        ///CrSel key
        ///</summary>
        VK_CRSEL = 0xF7,

        ///<summary>
        ///ExSel key
        ///</summary>
        VK_EXSEL = 0xF8,

        ///<summary>
        ///Erase EOF key
        ///</summary>
        VK_EREOF = 0xF9,

        ///<summary>
        ///Play key
        ///</summary>
        VK_PLAY = 0xFA,

        ///<summary>
        ///Zoom key
        ///</summary>
        VK_ZOOM = 0xFB,

        ///<summary>
        ///Reserved
        ///</summary>
        VK_NONAME = 0xFC,

        ///<summary>
        ///PA1 key
        ///</summary>
        VK_PA1 = 0xFD,

        ///<summary>
        ///Clear key
        ///</summary>
        VK_OEM_CLEAR = 0xFE,

        #endregion https://www.pinvoke.net/default.aspx/Enums/VK.html

        /// <summary>
        /// The "snap desktop" hot key was pressed.
        /// </summary>
        IDHOT_SNAPDESKTOP = -2,

        /// <summary>
        /// The "snap window" hot key was pressed.
        /// </summary>
        IDHOT_SNAPWINDOW = -1,

        /// <summary>
        /// Activated by some method other than a mouse click (for example, by a call to the SetActiveWindow function or by use of the keyboard interface to select the window).
        /// </summary>
        WA_ACTIVE = 1,

        /// <summary>
        /// Activated by a mouse click.
        /// </summary>
        WA_CLICKACTIVE = 2,

        /// <summary>
        /// Deactivated.
        /// </summary>
        WA_INACTIVE = 0,

        ///<summary>
        ///Closes the window.
        ///</summary>
        SC_CLOSE = 0xF060,

        ///<summary>
        ///Changes the cursor to a question mark with a pointer. If the user then clicks a control in the dialog box, the control receives a WM_HELP message.
        ///</summary>
        SC_CONTEXTHELP = 0xF180,

        ///<summary>
        ///Selects the default item; the user double-clicked the window menu.
        ///</summary>
        SC_DEFAULT = 0xF160,

        ///<summary>
        ///Activates the window associated with the application-specified hot key. The lParam parameter identifies the window to activate.
        ///</summary>
        SC_HOTKEY = 0xF150,

        ///<summary>
        ///Scrolls horizontally.
        /// </summary>
        SC_HSCROLL = 0xF080,

        ///<summary>
        ///Indicates whether the screen saver is secure.
        ///</summary>
        SCF_ISSECURE = 0x00000001,

        ///<summary>
        ///Retrieves the window menu as a result of a keystroke. For more information, see the Remarks section.
        ///</summary>
        SC_KEYMENU = 0xF100,

        ///<summary>
        ///Maximizes the window.
        ///</summary>
        SC_MAXIMIZE = 0xF030,

        ///<summary>
        ///Minimizes the window.
        ///</summary>
        SC_MINIMIZE = 0xF020,

        ///<summary>
        ///Sets the state of the display. This command supports devices that have power-saving features, such as a battery-powered personal computer.
        ///The lParam parameter can have the following values:
        ///<list type="bullet">
        ///<item><description>-1 (the display is powering on)</description></item>
        ///<item><description>1 (the display is going to low power)</description></item>
        ///<item><description>2 (the display is being shut off)</description></item>
        ///</list>
        ///</summary>
        SC_MONITORPOWER = 0xF170,

        ///<summary>
        ///Retrieves the window menu as a result of a mouse click.
        ///</summary>
        SC_MOUSEMENU = 0xF090,

        ///<summary>
        ///Moves the window.
        ///</summary>
        SC_MOVE = 0xF010,

        ///<summary>
        ///Moves to the next window.
        ///</summary>
        SC_NEXTWINDOW = 0xF040,

        ///<summary>
        ///Moves to the previous window.
        ///</summary>
        SC_PREVWINDOW = 0xF050,

        ///<summary>
        ///Restores the window to its normal position and size.
        ///</summary>
        SC_RESTORE = 0xF120,

        ///<summary>
        ///Executes the screen saver application specified in the [boot] section of the System.ini file.
        ///</summary>
        SC_SCREENSAVE = 0xF140,

        ///<summary>
        ///Sizes the window.
        ///</summary>
        SC_SIZE = 0xF000,

        ///<summary>
        ///Activates the Start menu.
        ///</summary>
        SC_TASKLIST = 0xF130,

        ///<summary>
        ///Scrolls vertically.
        ///</summary>
        SC_VSCROLL = 0xF070,
    }
}