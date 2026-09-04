using System;
using Modules.Communal.Render.Model;
using Mono.Addins;

namespace Modules.Communal.Render.ExtensionModel
{
	// Token: 0x0200001D RID: 29
	[TypeExtensionPoint]
	public interface IObjectContextMenu : IActivateControl
	{
		// Token: 0x06000107 RID: 263
		void CanShow(ContextMenuShowingArgs args);

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000108 RID: 264
		string Type { get; }
	}
}
