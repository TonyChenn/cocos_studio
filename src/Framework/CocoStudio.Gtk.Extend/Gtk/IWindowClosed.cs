using System;

namespace Gtk
{
	public interface IWindowClosed
	{
		event EventHandler<EventArgs> Closed;
	}
}
