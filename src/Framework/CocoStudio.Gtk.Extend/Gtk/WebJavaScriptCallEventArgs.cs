using System;

namespace Gtk
{
	public class WebJavaScriptCallEventArgs : EventArgs
	{
		public string Type { get; private set; }

		public string Info { get; private set; }

		public int X { get; private set; }

		public int Y { get; private set; }

		public WebJavaScriptCallEventArgs(string type, string info, int x, int y)
		{
			this.Type = type;
			this.Info = info;
			this.X = x;
			this.Y = y;
		}
	}
}
