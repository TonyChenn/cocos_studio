using System;
using Gdk;

namespace Gtk
{
	public static class DragContextExtend
	{
		public static void SetDragData(this DragContext context, object data)
		{
			DragDataManager.SetDragData(context, data);
		}

		public static object GetDragData(this DragContext context)
		{
			return DragDataManager.GetDragData(context);
		}

		public static bool GetDataPresent(this DragContext context, Type dataType)
		{
			object dragData = context.GetDragData();
			return dragData != null && dragData.GetType().Equals(dataType);
		}

		public static Widget GetSourceWidget(this DragContext context)
		{
			return Drag.GetSourceWidget(context);
		}
	}
}
