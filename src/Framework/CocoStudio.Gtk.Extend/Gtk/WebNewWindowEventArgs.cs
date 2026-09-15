using System;

namespace Gtk
{
	public class WebNewWindowEventArgs : EventArgs
	{
		public string Url { get; private set; }

		public int X { get; private set; }

		public int Y { get; private set; }

		public WebNewWindowEventArgs(string url, int x, int y)
		{
			this.Url = url;
			this.X = x;
			this.Y = y;
		}
	}
}
