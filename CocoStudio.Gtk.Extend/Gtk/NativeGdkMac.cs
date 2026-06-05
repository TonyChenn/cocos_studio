using System;
using System.Runtime.InteropServices;
using AppKit;
using Gdk;

namespace Gtk
{
	// Token: 0x02000094 RID: 148
	public static class NativeGdkMac
	{
		// Token: 0x0600031F RID: 799
		[DllImport("libgdk-quartz-2.0.0.dylib", EntryPoint = "gdk_quartz_window_get_nswindow")]
		public static extern IntPtr GetWindowHandle(IntPtr gdkWindow);

		// Token: 0x06000320 RID: 800
		[DllImport("libgdk-quartz-2.0.0.dylib", EntryPoint = "gdk_quartz_window_get_nsview")]
		public static extern IntPtr GetNSViewHandle(IntPtr gdkWindow);

		// Token: 0x06000321 RID: 801
		[DllImport("libgdk-quartz-2.0.0.dylib", EntryPoint = "gdk_window_ensure_native")]
		public static extern bool Gdk_window_ensure_native(IntPtr gdkWindow);

		// Token: 0x06000322 RID: 802 RVA: 0x0000CE10 File Offset: 0x0000B010
		public static NSWindow GetNSWindow(Window gdkWindow)
		{
			IntPtr windowHandle = NativeGdkMac.GetWindowHandle(gdkWindow.Handle);
			return new NSWindow
			{
				Handle = windowHandle
			};
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000CE40 File Offset: 0x0000B040
		public static void SetNSWindowStyle(Window gdkWindow, NSWindowStyle style)
		{
			NSWindow nswindow = NativeGdkMac.GetNSWindow(gdkWindow);
			nswindow.StyleMask = style;
			nswindow.Handle = IntPtr.Zero;
		}

		// Token: 0x040003AA RID: 938
		private const string dllGdkName = "libgdk-quartz-2.0.0.dylib";
	}
}
