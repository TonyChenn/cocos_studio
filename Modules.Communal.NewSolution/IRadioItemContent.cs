using System;
using Gtk;

namespace Modules.Communal.NewSolution
{
	// Token: 0x0200001C RID: 28
	public interface IRadioItemContent
	{
		// Token: 0x060000DB RID: 219
		Widget GetGtkWidget();

		// Token: 0x060000DC RID: 220
		void RefreshUI(bool isSelect, ButtonState currentState);
	}
}
