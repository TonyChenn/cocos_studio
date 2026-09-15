using System;

namespace Cocos.Launcher.Control
{
	public class LinkClickedEventArgs : EventArgs
	{
		public object Tag { get; private set; }

		public LinkClickedEventArgs(object tag)
		{
			this.Tag = tag;
		}
	}
}
