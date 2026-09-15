using System;
using Cocos.Launcher.Control;

namespace Cocos.Launcher.Core
{
	public class OpenClickedEventArgs : EventArgs
	{
		public object Tag { get; private set; }

		public StartEnum StartItem { get; private set; }

		public OpenClickedEventArgs(object tag, StartEnum startItem)
		{
			this.Tag = tag;
			this.StartItem = startItem;
		}
	}
}
