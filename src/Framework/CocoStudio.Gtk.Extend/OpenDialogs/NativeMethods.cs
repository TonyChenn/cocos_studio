using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenDialogs
{
	// Token: 0x02000043 RID: 67
	public static class NativeMethods
	{
		// Token: 0x06000185 RID: 389
		[DllImport("user32.dll")]
		public static extern int CloseWindow(IntPtr hwnd);

		// Token: 0x06000186 RID: 390
		[DllImport("user32.dll")]
		public static extern int EnableWindow(IntPtr hwnd, int fEnable);

		// Token: 0x06000187 RID: 391
		[DllImport("user32.dll")]
		public static extern int SetWindowText(IntPtr hwnd, string lpString);

		// Token: 0x06000188 RID: 392
		[DllImport("user32.dll")]
		public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

		// Token: 0x06000189 RID: 393
		[DllImport("user32.dll")]
		public static extern int GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessId);

		// Token: 0x0600018A RID: 394
		[DllImport("kernel32.dll")]
		public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		// Token: 0x0600018B RID: 395
		[DllImport("kernel32.dll")]
		public static extern int VirtualAllocEx(int hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

		// Token: 0x0600018C RID: 396
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

		// Token: 0x0600018D RID: 397
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

		// Token: 0x0600018E RID: 398
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetParent(IntPtr hWnd);

		// Token: 0x0600018F RID: 399
		[DllImport("User32.Dll")]
		public static extern int GetDlgCtrlID(IntPtr hWndCtl);

		// Token: 0x06000190 RID: 400
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MapWindowPoints(IntPtr hWnd, IntPtr hWndTo, ref POINT pt, int cPoints);

		// Token: 0x06000191 RID: 401
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetWindowInfo(IntPtr hwnd, out WINDOWINFO pwi);

		// Token: 0x06000192 RID: 402
		[DllImport("User32.Dll")]
		public static extern void GetWindowText(IntPtr hWnd, StringBuilder param, int length);

		// Token: 0x06000193 RID: 403
		[DllImport("User32.Dll")]
		public static extern void GetClassName(IntPtr hWnd, StringBuilder param, int length);

		// Token: 0x06000194 RID: 404
		[DllImport("user32.Dll")]
		public static extern bool EnumChildWindows(IntPtr hWndParent, NativeMethods.EnumWindowsCallBack lpEnumFunc, int lParam);

		// Token: 0x06000195 RID: 405
		[DllImport("user32.Dll")]
		public static extern bool EnumWindows(NativeMethods.EnumWindowsCallBack lpEnumFunc, int lParam);

		// Token: 0x06000196 RID: 406
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern bool ReleaseCapture();

		// Token: 0x06000197 RID: 407
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetCapture(IntPtr hWnd);

		// Token: 0x06000198 RID: 408
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr ChildWindowFromPointEx(IntPtr hParent, POINT pt, ChildFromPointFlags flags);

		// Token: 0x06000199 RID: 409
		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "FindWindowExA")]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		// Token: 0x0600019A RID: 410
		[DllImport("user32.dll")]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		// Token: 0x0600019B RID: 411
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600019C RID: 412
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		// Token: 0x0600019D RID: 413
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600019E RID: 414
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

		// Token: 0x0600019F RID: 415
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		// Token: 0x060001A0 RID: 416
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, StringBuilder param);

		// Token: 0x060001A1 RID: 417
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, char[] chars);

		// Token: 0x060001A2 RID: 418
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr BeginDeferWindowPos(int nNumWindows);

		// Token: 0x060001A3 RID: 419
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DeferWindowPos(IntPtr hWinPosInfo, IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, SetWindowPosFlags flags);

		// Token: 0x060001A4 RID: 420
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool EndDeferWindowPos(IntPtr hWinPosInfo);

		// Token: 0x060001A5 RID: 421
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, SetWindowPosFlags flags);

		// Token: 0x060001A6 RID: 422
		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hwnd, ref RECT rect);

		// Token: 0x060001A7 RID: 423
		[DllImport("user32.dll")]
		public static extern bool GetClientRect(IntPtr hwnd, ref RECT rect);

		// Token: 0x040002AF RID: 687
		public const int HWND_TOP = 0;

		// Token: 0x040002B0 RID: 688
		public const int SW_SHOW = 5;

		// Token: 0x040002B1 RID: 689
		public const int SW_HIDE = 0;

		// Token: 0x02000044 RID: 68
		// (Invoke) Token: 0x060001A9 RID: 425
		public delegate bool EnumWindowsCallBack(IntPtr hWnd, int lParam);
	}
}
