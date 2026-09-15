using System;
using Gdk;

namespace CocoStudio.Model.ViewModel.Input
{
	public class MouseWheelEventArgs
	{
		public MouseWheelEventArgs(Point point, int delta)
		{
			this.Point = point;
			this.Delta = delta;
		}

		public int Delta { get; private set; }

		public Point Point { get; private set; }
	}
}
