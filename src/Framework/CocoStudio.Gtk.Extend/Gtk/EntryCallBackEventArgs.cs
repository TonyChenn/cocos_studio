using System;

namespace Gtk
{
	public class EntryCallBackEventArgs : EventArgs
	{
		public EntryCallBackEventArgs(string value)
		{
			this.Value = value;
		}

		public string Value;
	}
}
