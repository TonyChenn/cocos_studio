using System.Collections.Generic;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public interface IExternalToolboxLoader
	{
		IList<ItemToolboxNode> Load(string filename);
	}
}
