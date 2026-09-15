using System;
using Modules.Communal.Render.Model;
using Mono.Addins;

namespace Modules.Communal.Render.ExtensionModel
{
	[TypeExtensionPoint]
	public interface IObjectContextMenu : IActivateControl
	{
		void CanShow(ContextMenuShowingArgs args);

		string Type { get; }
	}
}
