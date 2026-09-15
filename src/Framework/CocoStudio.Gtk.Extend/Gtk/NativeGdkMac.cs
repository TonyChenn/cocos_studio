using System;
using System.Runtime.InteropServices;
using AppKit;
using Gdk;

namespace Gtk
{
	public static class NativeGdkMac
	{
		[DllImport("libgdk-quartz-2.0.0.dylib", EntryPoint = "gdk_quartz_window_get_nswindow")]
		public static extern IntPtr GetWindowHandle(IntPtr gdkWindow);

		[DllImport("libgdk-quartz-2.0.0.dylib", EntryPoint = "gdk_quartz_window_get_nsview")]
		public static extern IntPtr GetNSViewHandle(IntPtr gdkWindow);

		[DllImport("libgdk-quartz-2.0.0.dylib", EntryPoint = "gdk_window_ensure_native")]
		public static extern bool Gdk_window_ensure_native(IntPtr gdkWindow);

		public static NSWindow GetNSWindow(Gdk.Window gdkWindow)
		{
			IntPtr windowHandle = NativeGdkMac.GetWindowHandle(gdkWindow.Handle);
			return new NSWindow
			{
				Handle = windowHandle
			};
		}

		public static void SetNSWindowStyle(Gdk.Window gdkWindow, NSWindowStyle style)
		{
			NSWindow nswindow = NativeGdkMac.GetNSWindow(gdkWindow);
			nswindow.StyleMask = style;
			nswindow.Handle = IntPtr.Zero;
		}

		private const string dllGdkName = "libgdk-quartz-2.0.0.dylib";
	}
}
