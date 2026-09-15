using System;

namespace CocoStudio.Model.Editor
{
	public class FixZoomEventArgs : EventArgs
	{
		public bool Type { get; set; }

		public FixZoomEventArgs(bool type)
		{
			this.Type = type;
		}
	}
}
