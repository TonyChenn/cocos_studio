using System;
using Gtk;

namespace Modules.Communal.NewSolution
{
	public interface IRadioItemContent
	{
		Widget GetGtkWidget();

		void RefreshUI(bool isSelect, ButtonState currentState);
	}
}
