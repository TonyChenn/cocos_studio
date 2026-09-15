using System;

namespace Gtk
{
	public class EntryIntEventArgs : EventArgs
	{
		public float Value { get; private set; }

		public EntryIntEventArgs(float value)
		{
			this.Value = value;
		}
	}
}
