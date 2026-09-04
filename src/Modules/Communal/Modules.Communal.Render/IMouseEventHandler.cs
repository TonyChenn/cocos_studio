using System;
using Gtk;

namespace Modules.Communal.Render
{
	// Token: 0x0200000B RID: 11
	public interface IMouseEventHandler
	{
		// Token: 0x06000065 RID: 101
		void OnMouseMove(MotionNotifyEventArgs args);

		// Token: 0x06000066 RID: 102
		void OnMouseUp(ButtonReleaseEventArgs args);

		// Token: 0x06000067 RID: 103
		void OnMouseDown(ButtonPressEventArgs args);

		// Token: 0x06000068 RID: 104
		void OnMouseEnter(EnterNotifyEventArgs args);

		// Token: 0x06000069 RID: 105
		void OnMouseLeave(LeaveNotifyEventArgs args);

		// Token: 0x0600006A RID: 106
		void OnMouseWheel(ScrollEventArgs args);

		// Token: 0x0600006B RID: 107
		void OnMouseDoubleClick(ButtonPressEventArgs args);

		// Token: 0x0600006C RID: 108
		void OnMouseGestures(MouseGesturesEventArgs args);
	}
}
