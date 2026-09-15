using System;

namespace CocoStudio.Model.Editor
{
	public class Scale9EventArgs : EventArgs
	{
		public CurrentRange Type { get; set; }

		public double Left { get; set; }

		public double Right { get; set; }

		public double Top { get; set; }

		public double Bottom { get; set; }

		public Scale9EventArgs(CurrentRange type, double left, double right, double top, double bottom)
		{
			this.Type = type;
			this.Left = left;
			this.Right = right;
			this.Top = top;
			this.Bottom = bottom;
		}
	}
}
