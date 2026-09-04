using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenDialogs
{
	// Token: 0x02000052 RID: 82
	public class WindowHelper
	{
		// Token: 0x060001BC RID: 444 RVA: 0x000084B0 File Offset: 0x000066B0
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

		// Token: 0x060001BD RID: 445 RVA: 0x00008510 File Offset: 0x00006710
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

		// Token: 0x060001BE RID: 446 RVA: 0x0000855C File Offset: 0x0000675C
		public bool IsMainWindow(IntPtr handle)
		{
			return !(WindowHelper.GetWindow(new HandleRef(this, handle), 4) != IntPtr.Zero) && WindowHelper.IsWindowVisible(new HandleRef(this, handle));
		}

		// Token: 0x060001BF RID: 447
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool EnumWindows(WindowHelper.EnumThreadWindowsCallback callback, IntPtr extraData);

		// Token: 0x060001C0 RID: 448
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

		// Token: 0x060001C1 RID: 449
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

		// Token: 0x060001C2 RID: 450
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool IsWindowVisible(HandleRef hWnd);

		// Token: 0x060001C3 RID: 451 RVA: 0x00008598 File Offset: 0x00006798
		public static IntPtr GetCurrentWindowHandle()
		{
			IntPtr zero = IntPtr.Zero;
			uint id = (uint)Process.GetCurrentProcess().Id;
			return new WindowHelper().GetMainWindowHandle((int)id);
		}

		// Token: 0x060001C4 RID: 452
		[DllImport("user32.dll")]
		public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

		// Token: 0x060001C5 RID: 453
		[DllImport("user32.dll")]
		public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

		// Token: 0x060001C6 RID: 454
		[DllImport("user32.dll")]
		public static extern int SetForegroundWindow(IntPtr hwnd);

		// Token: 0x060001C7 RID: 455 RVA: 0x000085C8 File Offset: 0x000067C8
		public static int ShowWindow(IntPtr hwnd)
		{
			WindowHelper.SetForegroundWindow(hwnd);
			return 0;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000085E2 File Offset: 0x000067E2
		public static void ShowCurrentWindowHandle()
		{
			WindowHelper.ShowWindow(WindowHelper.GetCurrentWindowHandle());
		}

		// Token: 0x060001C9 RID: 457
		[DllImport("shfolder.dll", CharSet = CharSet.Auto)]
		private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

		// Token: 0x060001CA RID: 458 RVA: 0x000085F0 File Offset: 0x000067F0
		public static string GetAllUsersDesktopFolderPath()
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			WindowHelper.SHGetFolderPath(IntPtr.Zero, 25, IntPtr.Zero, 0, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x040002ED RID: 749
		private const int SW_SHOWDEFAULT = 10;

		// Token: 0x040002EE RID: 750
		private const int MAX_PATH = 260;

		// Token: 0x040002EF RID: 751
		private const int CSIDL_COMMON_DESKTOPDIRECTORY = 25;

		// Token: 0x040002F0 RID: 752
		public bool haveMainWindow = false;

		// Token: 0x040002F1 RID: 753
		public IntPtr mainWindowHandle = IntPtr.Zero;

		// Token: 0x040002F2 RID: 754
		public int processId = 0;

		// Token: 0x040002F3 RID: 755
		private static Hashtable processWnd = new Hashtable();

		// Token: 0x02000053 RID: 83
		// (Invoke) Token: 0x060001CE RID: 462
		public delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

		// Token: 0x02000054 RID: 84
		// (Invoke) Token: 0x060001D2 RID: 466
		public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);
	}
}
