using System;
using Gtk;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x0200001D RID: 29
	internal interface ICreateStep
	{
		// Token: 0x060000F0 RID: 240
		bool Run(CreateParams prms, CocosMonitor monitor);

		// Token: 0x060000F1 RID: 241
		bool CanCreate(CreateParams prms);
	}
}
