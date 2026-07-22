using System;
using CocoStudio.Model.ViewModel;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000008 RID: 8
	internal interface IObjectMenuItem
	{
		// Token: 0x06000027 RID: 39
		void UpdateMenuItemState();

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000029 RID: 41
		// (set) Token: 0x06000028 RID: 40
		VisualObject TriggerObject { get; set; }
	}
}
