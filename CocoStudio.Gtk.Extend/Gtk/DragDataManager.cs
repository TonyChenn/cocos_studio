using System;
using CocoStudio.Basic;
using Gdk;

namespace Gtk
{
	// Token: 0x02000078 RID: 120
	internal class DragDataManager
	{
		// Token: 0x060002BA RID: 698 RVA: 0x0000AB80 File Offset: 0x00008D80
		public static void SetDragData(DragContext dragContext, object data)
		{
			DragDataManager.data = data;
			DragDataManager.sourceWidget = dragContext.GetSourceWidget();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000AB94 File Offset: 0x00008D94
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

		// Token: 0x060002BC RID: 700 RVA: 0x0000ABF8 File Offset: 0x00008DF8
		private static bool IsSameSourceWidget(DragContext dragContext)
		{
			return DragDataManager.sourceWidget != null && DragDataManager.sourceWidget.Equals(dragContext.GetSourceWidget());
		}

		// Token: 0x0400033F RID: 831
		private static Widget sourceWidget;

		// Token: 0x04000340 RID: 832
		private static object data;
	}
}
