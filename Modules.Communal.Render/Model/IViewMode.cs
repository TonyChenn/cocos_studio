using System;
using CocoStudio.Model;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.Render.ExtensionModel;
using Mono.Addins;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000016 RID: 22
	[TypeExtensionPoint]
	public interface IViewMode : IActivateControl, IDocumentEventHandler, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IDragEventHandler
	{
		// Token: 0x060000B7 RID: 183
		void Initialize(IGLView glView);

		// Token: 0x060000B8 RID: 184
		bool CanHandle(CocosItem cocosItem);

		// Token: 0x060000B9 RID: 185
		IObjectContextMenu GetContextMenu();

		// Token: 0x060000BA RID: 186
		bool CanShowContextMenu(PointF scenePoint);

		// Token: 0x060000BB RID: 187
		void OnCanvasSizeChanged();

		// Token: 0x060000BC RID: 188
		void ResetView();

		// Token: 0x060000BD RID: 189
		ICommandService GetCommandService();

		// Token: 0x060000BE RID: 190
		Widget GetToolbar();
	}
}
