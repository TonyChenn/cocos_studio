using System;
using Gdk;

namespace Gtk
{
	public static class DragMotionArgsExtend
	{
		public static void SetAllowDragAction(this DragMotionArgs args, DragAction action)
		{
			Gdk.Drag.Status(args.Context, action, args.Time);
		}
	}
}
