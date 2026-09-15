using System;

namespace Gtk
{
	public class OutputEventArgs : EventArgs
	{
		public string OutputInfo { get; private set; }

		public OutputEventArgs(string info)
		{
			this.OutputInfo = info;
		}
	}
}
