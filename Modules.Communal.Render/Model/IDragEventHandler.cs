using System;
using Gtk;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000015 RID: 21
	public interface IDragEventHandler
	{
		// Token: 0x060000B3 RID: 179
		void OnDragOver(DragMotionArgs args);

		// Token: 0x060000B4 RID: 180
		void OnDragLeave(DragLeaveArgs args);

		// Token: 0x060000B5 RID: 181
		void OnDragDrop(DragDropArgs args);

		// Token: 0x060000B6 RID: 182
		void DragDataReceived(DragDataReceivedArgs args);
	}
}
