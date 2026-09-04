using System;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.Render.ExtensionModel
{
	// Token: 0x0200001E RID: 30
	[TypeExtensionPoint]
	public interface IObjectDragMenu
	{
		// Token: 0x06000109 RID: 265
		bool CanShow(DragMenuShowingArgs args);

		// Token: 0x0600010A RID: 266
		Menu GetPopupMenu();
	}
}
