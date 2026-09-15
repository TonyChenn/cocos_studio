using System;
using CocoStudio.Basic;
using Gdk;

namespace Gtk
{
	internal class DragDataManager
	{
		public static void SetDragData(DragContext dragContext, object data)
		{
			DragDataManager.data = data;
			DragDataManager.sourceWidget = dragContext.GetSourceWidget();
		}

		public static object GetDragData(DragContext dragContext)
		{
			object result;
			if (DragDataManager.sourceWidget == null && DragDataManager.data is FileDropInfo)
			{
				result = DragDataManager.data;
			}
			else if (DragDataManager.IsSameSourceWidget(dragContext))
			{
				result = DragDataManager.data;
			}
			else
			{
				LogConfig.Logger.Info("Should set drag data in begin drag event. Then can get drag data.", true);
				result = null;
			}
			return result;
		}

		private static bool IsSameSourceWidget(DragContext dragContext)
		{
			return DragDataManager.sourceWidget != null && DragDataManager.sourceWidget.Equals(dragContext.GetSourceWidget());
		}

		private static Widget sourceWidget;

		private static object data;
	}
}
