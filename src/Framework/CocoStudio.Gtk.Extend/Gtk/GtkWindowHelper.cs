using System;
using GLib;
using MonoDevelop.Core;
using OpenDialogs;

namespace Gtk
{
	public static class GtkWindowHelper
	{
		public static void CanActivationTop(Window window)
		{
			if (window != null)
			{
				window.WidgetEvent += GtkWindowHelper.GtkWindow_WidgetEvent;
				window.Destroyed += GtkWindowHelper.window_Destroyed;
			}
		}

		private static void window_Destroyed(object sender, EventArgs e)
		{
			Window window = sender as Window;
			if (window != null)
			{
				window.Destroyed -= GtkWindowHelper.window_Destroyed;
				window.WidgetEvent -= GtkWindowHelper.GtkWindow_WidgetEvent;
			}
		}

		[ConnectBefore]
		private static void GtkWindow_WidgetEvent(object o, WidgetEventArgs args)
		{
			if (Platform.IsWindows)
			{
				Window window = o as Window;
				if (window != null && !window.IsFocus)
				{
					if (args.Event.ToString().Equals("Gdk.EventButton"))
					{
						WindowHelper.ShowCurrentWindowHandle();
					}
				}
			}
		}

		public static T GetParentWidget<T>(this Widget widget) where T : Widget
		{
			Widget widget2 = widget;
			while (widget2 != null)
			{
				widget2 = widget2.Parent;
				if (widget2 == null || widget2 is T)
				{
					break;
				}
			}
			return widget2 as T;
		}
	}
}
