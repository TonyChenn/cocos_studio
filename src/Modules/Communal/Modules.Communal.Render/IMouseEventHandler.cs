using System;
using Gtk;

namespace Modules.Communal.Render
{
	public interface IMouseEventHandler
	{
		void OnMouseMove(MotionNotifyEventArgs args);

		void OnMouseUp(ButtonReleaseEventArgs args);

		void OnMouseDown(ButtonPressEventArgs args);

		void OnMouseEnter(EnterNotifyEventArgs args);

		void OnMouseLeave(LeaveNotifyEventArgs args);

		void OnMouseWheel(ScrollEventArgs args);

		void OnMouseDoubleClick(ButtonPressEventArgs args);

		void OnMouseGestures(MouseGesturesEventArgs args);
	}
}
