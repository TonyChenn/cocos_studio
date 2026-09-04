using System;
using GLib;
using MonoDevelop.Core;
using OpenDialogs;

namespace Gtk
{
	// Token: 0x02000062 RID: 98
	public static class GtkWindowHelper
	{
		// Token: 0x06000215 RID: 533 RVA: 0x00009508 File Offset: 0x00007708
		public static void CanActivationTop(Window window)
		{
			if (window != null)
			{
				window.WidgetEvent += GtkWindowHelper.GtkWindow_WidgetEvent;
				window.Destroyed += GtkWindowHelper.window_Destroyed;
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00009548 File Offset: 0x00007748
		private static void window_Destroyed(object sender, EventArgs e)
		{
			Window window = sender as Window;
			if (window != null)
			{
				window.Destroyed -= GtkWindowHelper.window_Destroyed;
				window.WidgetEvent -= GtkWindowHelper.GtkWindow_WidgetEvent;
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00009590 File Offset: 0x00007790
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

		// Token: 0x06000218 RID: 536 RVA: 0x000095EC File Offset: 0x000077EC
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
