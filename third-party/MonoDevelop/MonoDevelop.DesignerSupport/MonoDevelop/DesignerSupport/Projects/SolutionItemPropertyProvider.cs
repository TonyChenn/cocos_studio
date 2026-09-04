using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport.Projects
{
	internal class SolutionItemPropertyProvider : IPropertyProvider
	{
		public object CreateProvider(object obj)
		{
			if (obj is WorkspaceItem)
			{
				return new WorkspaceItemDescriptor((WorkspaceItem)obj);
			}
			return new SolutionItemDescriptor((SolutionItem)obj);
		}

		public bool SupportsObject(object obj)
		{
			if (!(obj is SolutionItem))
			{
				return obj is WorkspaceItem;
			}
			return true;
		}
	}
}
