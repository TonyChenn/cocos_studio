using System;
using System.Runtime.InteropServices;

namespace Gtk
{
	// Token: 0x02000095 RID: 149
	public static class NativeGdkWin
	{
		// Token: 0x06000324 RID: 804
		[DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "gdk_win32_window_get_impl_hwnd")]
		public static extern IntPtr GetWindowHandle(IntPtr gdkWindow);

		// Token: 0x06000325 RID: 805
		[DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "gdk_window_ensure_native")]
		public static extern bool Gdk_window_ensure_native(IntPtr gdkWindow);

		// Token: 0x040003AB RID: 939
		private const string dllGdkName = "libgdk-win32-2.0-0.dll";
	}
}
