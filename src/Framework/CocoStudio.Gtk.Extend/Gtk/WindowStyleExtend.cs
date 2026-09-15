using System;
using AppKit;
using CocoStudio.Basic;
using MonoDevelop.Core;

namespace Gtk
{
	public static class WindowStyleExtend
	{
		public static void SetToDialogStyle(this Window window, Window parentWindow = null, bool closeable = true, bool centerToParent = true, bool setTransient = true)
		{
			if (parentWindow == null)
			{
				parentWindow = ApplicationCurrent.MainWindow;
			}
			if (centerToParent)
			{
				window.CenterToParentWindow(parentWindow);
			}
			if (setTransient)
			{
				window.TransientFor = parentWindow;
			}
			window.Deletable = closeable;
			if (Platform.IsMac)
			{
				NSWindowStyle nswindowStyle = NSWindowStyle.Titled;
				if (closeable)
				{
					nswindowStyle |= NSWindowStyle.Closable;
				}
				if (window.GdkWindow == null)
				{
					window.Show();
				}
				NativeGdkMac.SetNSWindowStyle(window.GdkWindow, nswindowStyle);
			}
		}

		public static void PreSetBeforeShow(this Window child, int width, int height, Window parent)
		{
			int num;
			int num2;
			parent.GetSize(out num, out num2);
			int num3;
			int num4;
			parent.GetPosition(out num3, out num4);
			int x = Math.Max(0, (num - width) / 2) + num3;
			int y = Math.Max(0, (num2 - height) / 2) + num4;
			child.Move(x, y);
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				if (Platform.IsWindows)
				{
					child.Decorated = false;
				}
			}
		}

		public static void CenterToParentWindow(this Window child, Window parent)
		{
			int num;
			int num2;
			child.GetSize(out num, out num2);
			int num3;
			int num4;
			parent.GetSize(out num3, out num4);
			int num5;
			int num6;
			parent.GetPosition(out num5, out num6);
			int x = Math.Max(0, (num3 - num) / 2) + num5;
			int y = Math.Max(0, (num4 - num2) / 2) + num6;
			child.Move(x, y);
		}

		public static void RemoveWindowBorder(this Window wnd)
		{
			if (Platform.IsMac)
			{
				if (wnd.GdkWindow == null)
				{
					wnd.Show();
				}
				NSWindowStyle style = NSWindowStyle.Titled | NSWindowStyle.DocModal;
				NativeGdkMac.SetNSWindowStyle(wnd.GdkWindow, style);
			}
			else if (Platform.IsWindows)
			{
				wnd.Decorated = false;
				Dialog dialog = wnd as Dialog;
				if (dialog != null)
				{
					dialog.VBox.BorderWidth = 0U;
				}
			}
		}
	}
}
