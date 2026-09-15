using System;

namespace Gtk
{
	public interface IDialogButton
	{
		string Text { get; set; }

		event EventHandler<ButtonReleaseEventArgs> Clicked;

		EnumMainButton ButtonType { get; set; }

		Widget GetWidget();
	}
}
