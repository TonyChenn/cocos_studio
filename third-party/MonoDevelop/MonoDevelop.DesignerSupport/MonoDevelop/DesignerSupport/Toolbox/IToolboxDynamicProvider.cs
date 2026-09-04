using System;
using System.Collections.Generic;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public interface IToolboxDynamicProvider
	{
		event EventHandler ItemsChanged;

		IEnumerable<ItemToolboxNode> GetDynamicItems(IToolboxConsumer consumer);
	}
}
