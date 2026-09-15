using System;
using CocoStudio.Model;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.Render.ExtensionModel;
using Mono.Addins;

namespace Modules.Communal.Render.Model
{
	[TypeExtensionPoint]
	public interface IViewMode : IActivateControl, IDocumentEventHandler, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IDragEventHandler
	{
		void Initialize(IGLView glView);

		bool CanHandle(CocosItem cocosItem);

		IObjectContextMenu GetContextMenu();

		bool CanShowContextMenu(PointF scenePoint);

		void OnCanvasSizeChanged();

		void ResetView();

		ICommandService GetCommandService();

		Widget GetToolbar();
	}
}
