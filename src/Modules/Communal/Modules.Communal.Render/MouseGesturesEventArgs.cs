using System;
using CocoStudio.Model;

namespace Modules.Communal.Render
{
	public class MouseGesturesEventArgs : EventArgs
	{
		public double X { get; private set; }

		public double Y { get; private set; }

		public double Zoom { get; private set; }

		public bool Retval { get; set; }

		public PointF GetPoint()
		{
			return new PointF((float)this.X, (float)this.Y);
		}

		public bool CheckRetval()
		{
			return this.Retval;
		}

		public MouseGesturesEventArgs(double x, double y, double zoom)
		{
			this.X = x;
			this.Y = y;
			this.Zoom = zoom;
		}
	}
}
