using System;
using Gtk;
using Mono.Addins;

namespace Modules.Communal.Render.ExtensionModel
{
	[TypeExtensionPoint]
	public interface IObjectDragMenu
	{
		bool CanShow(DragMenuShowingArgs args);

		Menu GetPopupMenu();
	}
}
