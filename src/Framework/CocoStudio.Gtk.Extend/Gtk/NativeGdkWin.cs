using System;
using System.Runtime.InteropServices;

namespace Gtk
{
	public static class NativeGdkWin
	{
		[DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "gdk_win32_window_get_impl_hwnd")]
		public static extern IntPtr GetWindowHandle(IntPtr gdkWindow);

		[DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "gdk_window_ensure_native")]
		public static extern bool Gdk_window_ensure_native(IntPtr gdkWindow);

		private const string dllGdkName = "libgdk-win32-2.0-0.dll";
	}
}
