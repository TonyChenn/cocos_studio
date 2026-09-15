using System;
using CocoStudio.Model.ViewModel.HitTest;
using Gtk;

namespace CocoStudio.Model.ViewModel
{
	public class MouseEventArgs : EventArgs
	{
		public HitTestResult HitResult { get; private set; }

		public PointF Point { get; private set; }

		public MouseButton Button { get; private set; }

		public bool Handled { get; set; }

		public MouseEventArgs(PointF point, MouseButton button, HitTestResult result)
		{
			this.Point = point;
			this.Button = button;
			this.HitResult = result;
		}
	}
}
