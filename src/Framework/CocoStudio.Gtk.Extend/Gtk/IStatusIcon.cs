using System;

namespace Gtk
{
	public interface IStatusIcon
	{
		object PopupMenu { get; set; }

		string IconPath { get; set; }

		event EventHandler<EventArgs> Action;

		string Tooltip { get; set; }

		void Dispose();
	}
}
