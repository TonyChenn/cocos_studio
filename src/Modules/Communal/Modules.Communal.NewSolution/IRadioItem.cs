using System;
using Gtk;

namespace Modules.Communal.NewSolution
{
	public interface IRadioItem
	{
		bool IsSelected { get; }

		object Tag { get; set; }

		IRadioItemContent GtkContent { get; }

		event EventHandler<RadioItemArgs> Selected;

		event EventHandler<RadioItemArgs> DoubleClicked;

		void Select();

		void Unselect();

		Widget GetGtkWidget();
	}
}
