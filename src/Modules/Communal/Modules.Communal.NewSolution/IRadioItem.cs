using System;
using Gtk;

namespace Modules.Communal.NewSolution
{
	// Token: 0x0200001A RID: 26
	public interface IRadioItem
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000AF RID: 175
		bool IsSelected { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B0 RID: 176
		// (set) Token: 0x060000B1 RID: 177
		object Tag { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B2 RID: 178
		IRadioItemContent GtkContent { get; }

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000B3 RID: 179
		// (remove) Token: 0x060000B4 RID: 180
		event EventHandler<RadioItemArgs> Selected;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000B5 RID: 181
		// (remove) Token: 0x060000B6 RID: 182
		event EventHandler<RadioItemArgs> DoubleClicked;

		// Token: 0x060000B7 RID: 183
		void Select();

		// Token: 0x060000B8 RID: 184
		void Unselect();

		// Token: 0x060000B9 RID: 185
		Widget GetGtkWidget();
	}
}
