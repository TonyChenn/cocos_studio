using System;
using System.Collections.Generic;
using Gtk;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	[TypeExtensionPoint]
	public interface ICustomMenu
	{
		Type GetObjectType();

		List<MenuItem> GetCustomMenu();
	}
}
