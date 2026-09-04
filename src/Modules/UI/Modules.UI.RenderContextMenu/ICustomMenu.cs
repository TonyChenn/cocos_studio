using System;
using System.Collections.Generic;
using Gtk;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000002 RID: 2
	[TypeExtensionPoint]
	public interface ICustomMenu
	{
		// Token: 0x06000001 RID: 1
		Type GetObjectType();

		// Token: 0x06000002 RID: 2
		List<MenuItem> GetCustomMenu();
	}
}
