using System;
using Gdk;

namespace Gtk
{
	// Token: 0x02000079 RID: 121
	public static class DragMotionArgsExtend
	{
		// Token: 0x060002BE RID: 702 RVA: 0x0000AC3B File Offset: 0x00008E3B
		public static void SetAllowDragAction(this DragMotionArgs args, DragAction action)
		{
			Gdk.Drag.Status(args.Context, action, args.Time);
		}
	}
}
