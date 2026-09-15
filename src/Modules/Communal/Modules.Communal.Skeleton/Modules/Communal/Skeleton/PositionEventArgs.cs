using System;

namespace Modules.Communal.Skeleton
{
	public class PositionEventArgs : EventArgs
	{
		public double PointX { get; private set; }

		public double PointY { get; private set; }

		public PositionEventArgs(double x, double y)
		{
			this.PointX = x;
			this.PointY = y;
		}
	}
}
