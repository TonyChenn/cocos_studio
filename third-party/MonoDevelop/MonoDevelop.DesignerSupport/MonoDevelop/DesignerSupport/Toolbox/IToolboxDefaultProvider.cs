using System.Collections.Generic;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public interface IToolboxDefaultProvider
	{
		IEnumerable<ItemToolboxNode> GetDefaultItems();

		IEnumerable<string> GetDefaultFiles();
	}
}
