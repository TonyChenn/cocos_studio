using System;

namespace Gtk
{
	public class WebNavigatingEventArgs : EventArgs
	{
		public string Url { get; private set; }

		public WebNavigatingEventArgs(string url)
		{
			this.Url = url;
		}
	}
}
