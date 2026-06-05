using System;
using Gdk;

namespace Gtk
{
	// Token: 0x02000077 RID: 119
	public static class DragContextExtend
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x0000AB01 File Offset: 0x00008D01
		public static void SetDragData(this DragContext context, object data)
		{
			DragDataManager.SetDragData(context, data);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000AB0C File Offset: 0x00008D0C
		public static object GetDragData(this DragContext context)
		{
			return DragDataManager.GetDragData(context);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000AB24 File Offset: 0x00008D24
		public static bool GetDataPresent(this DragContext context, Type dataType)
		{
			object dragData = context.GetDragData();
			return dragData != null && dragData.GetType().Equals(dataType);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000AB68 File Offset: 0x00008D68
		public static Widget GetSourceWidget(this DragContext context)
		{
			return Drag.GetSourceWidget(context);
		}
	}
}
