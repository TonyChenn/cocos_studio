using System;

namespace Gtk
{
	public class ExpandEvent : EventArgs
	{
		public string Name { get; private set; }

		public bool Expand { get; private set; }

		public ExpandEvent(string name, bool expand)
		{
			this.Name = name;
			this.Expand = expand;
		}
	}
}
