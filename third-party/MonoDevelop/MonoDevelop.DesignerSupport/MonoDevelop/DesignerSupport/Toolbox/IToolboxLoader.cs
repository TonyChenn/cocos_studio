using System.Collections.Generic;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public interface IToolboxLoader
	{
		string[] FileTypes { get; }

		IList<ItemToolboxNode> Load(LoaderContext context, string filename);
	}
}
