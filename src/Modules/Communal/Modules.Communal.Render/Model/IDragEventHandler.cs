using System;
using Gtk;

namespace Modules.Communal.Render.Model
{
	public interface IDragEventHandler
	{
		void OnDragOver(DragMotionArgs args);

		void OnDragLeave(DragLeaveArgs args);

		void OnDragDrop(DragDropArgs args);

		void DragDataReceived(DragDataReceivedArgs args);
	}
}
