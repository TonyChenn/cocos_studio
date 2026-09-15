using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenDialogs
{
	public class WindowHelper
	{
		public IntPtr GetMainWindowHandle(int processId)
		{
			if (!this.haveMainWindow)
			{
				this.mainWindowHandle = IntPtr.Zero;
				this.processId = processId;
				WindowHelper.EnumThreadWindowsCallback enumThreadWindowsCallback = new WindowHelper.EnumThreadWindowsCallback(this.EnumWindowsCallback);
				WindowHelper.EnumWindows(enumThreadWindowsCallback, IntPtr.Zero);
				GC.KeepAlive(enumThreadWindowsCallback);
				this.haveMainWindow = true;
			}
			return this.mainWindowHandle;
		}

		public bool EnumWindowsCallback(IntPtr handle, IntPtr extraParameter)
		{
			int num;
			WindowHelper.GetWindowThreadProcessId(new HandleRef(this, handle), out num);
			bool result;
			if (num == this.processId && this.IsMainWindow(handle))
			{
				this.mainWindowHandle = handle;
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public bool IsMainWindow(IntPtr handle)
		{
			return !(WindowHelper.GetWindow(new HandleRef(this, handle), 4) != IntPtr.Zero) && WindowHelper.IsWindowVisible(new HandleRef(this, handle));
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool EnumWindows(WindowHelper.EnumThreadWindowsCallback callback, IntPtr extraData);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool IsWindowVisible(HandleRef hWnd);

		public static IntPtr GetCurrentWindowHandle()
		{
			IntPtr zero = IntPtr.Zero;
			uint id = (uint)Process.GetCurrentProcess().Id;
			return new WindowHelper().GetMainWindowHandle((int)id);
		}

		[DllImport("user32.dll")]
		public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

		[DllImport("user32.dll")]
		public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

		[DllImport("user32.dll")]
		public static extern int SetForegroundWindow(IntPtr hwnd);

		public static int ShowWindow(IntPtr hwnd)
		{
			WindowHelper.SetForegroundWindow(hwnd);
			return 0;
		}

		public static void ShowCurrentWindowHandle()
		{
			WindowHelper.ShowWindow(WindowHelper.GetCurrentWindowHandle());
		}

		[DllImport("shfolder.dll", CharSet = CharSet.Auto)]
		private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

		public static string GetAllUsersDesktopFolderPath()
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			WindowHelper.SHGetFolderPath(IntPtr.Zero, 25, IntPtr.Zero, 0, stringBuilder);
			return stringBuilder.ToString();
		}

		private const int SW_SHOWDEFAULT = 10;

		private const int MAX_PATH = 260;

		private const int CSIDL_COMMON_DESKTOPDIRECTORY = 25;

		public bool haveMainWindow = false;

		public IntPtr mainWindowHandle = IntPtr.Zero;

		public int processId = 0;

		private static Hashtable processWnd = new Hashtable();

		public delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

		public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);
	}
}
