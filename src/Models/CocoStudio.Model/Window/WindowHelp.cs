using System;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using Gdk;
using Gtk;
using Xwt.GtkBackend;

namespace CocoStudio.Model.Window
{
	internal static class WindowHelp
	{
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

		public static CSWindow CreateCSWindow(IntPtr windowHandle, int width, int height)
		{
			return new CSWindow(windowHandle.ToInt32(), width, height, WindowHelp.GetStartupPath(), Option.UserConfig.MultiplySample);
		}

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

		private static CSWindow CreateWin32(Gdk.Window gdkWindow)
		{
			NativeGdkWin.Gdk_window_ensure_native(gdkWindow.Handle);
			IntPtr windowHandle = NativeGdkWin.GetWindowHandle(gdkWindow.Handle);
			int width;
			int heigth;
			gdkWindow.GetSize(out width, out heigth);
			return new CSWindow(windowHandle.ToInt32(), width, heigth, WindowHelp.GetStartupPath(), Option.UserConfig.MultiplySample);
		}

		private static string GetStartupPath()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}

		private static Gdk.Window gdkWindow;
	}
}
