using System;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using Gdk;
using Gtk;
using Xwt.GtkBackend;

namespace CocoStudio.Model.Window
{
	// Token: 0x02000137 RID: 311
	internal static class WindowHelp
	{
		// Token: 0x06000B8C RID: 2956 RVA: 0x0002D728 File Offset: 0x0002B928
		public static CSWindow CreateCSWindow(Gdk.Window gdkWindow)
		{
			CSWindow result;
			if (Platform.IsMac)
			{
				result = WindowHelp.CreateMac(gdkWindow);
			}
			else
			{
				if (!Platform.IsWindows)
				{
					throw new NotImplementedException();
				}
				result = WindowHelp.CreateWin32(gdkWindow);
			}
			return result;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0002D76C File Offset: 0x0002B96C
		public static CSWindow CreateCSWindow(IntPtr windowHandle, int width, int height)
		{
			return new CSWindow(windowHandle.ToInt32(), width, height, WindowHelp.GetStartupPath(), Option.UserConfig.MultiplySample);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0002D79C File Offset: 0x0002B99C
		public static void UpdateOpenGLContext(bool isShowing, CSWindow csWindow, Gdk.Window gdkWindow)
		{
			if (Platform.IsWindows)
			{
				if (isShowing && gdkWindow != null)
				{
					bool flag = NativeGdkWin.Gdk_window_ensure_native(gdkWindow.Handle);
					csWindow.UpdateOpenGLContext(isShowing, NativeGdkWin.GetWindowHandle(gdkWindow.Handle).ToInt32());
					GCHelper.QuickCollect();
				}
			}
			else
			{
				if (!Platform.IsMac)
				{
					throw new NotImplementedException();
				}
				csWindow.UpdateOpenGLContext(isShowing, 0);
			}
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0002D81C File Offset: 0x0002BA1C
		private static CSWindow CreateMac(Gdk.Window window)
		{
			WindowHelp.gdkWindow = window;
			IntPtr windowHandle = NativeGdkMac.GetWindowHandle(WindowHelp.gdkWindow.Handle);
			IntPtr nsviewHandle = NativeGdkMac.GetNSViewHandle(WindowHelp.gdkWindow.Handle);
			int width;
			int height;
			WindowHelp.gdkWindow.GetSize(out width, out height);
			string startupPath = WindowHelp.GetStartupPath();
			return new CSWindow(windowHandle, nsviewHandle, width, height, startupPath, Option.UserConfig.MultiplySample);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0002D888 File Offset: 0x0002BA88
		private static CSWindow CreateWin32(Gdk.Window gdkWindow)
		{
			NativeGdkWin.Gdk_window_ensure_native(gdkWindow.Handle);
			IntPtr windowHandle = NativeGdkWin.GetWindowHandle(gdkWindow.Handle);
			int width;
			int heigth;
			gdkWindow.GetSize(out width, out heigth);
			return new CSWindow(windowHandle.ToInt32(), width, heigth, WindowHelp.GetStartupPath(), Option.UserConfig.MultiplySample);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0002D8DC File Offset: 0x0002BADC
		private static string GetStartupPath()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}

		// Token: 0x040004E6 RID: 1254
		private static Gdk.Window gdkWindow;
	}
}
