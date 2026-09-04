using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport.Projects
{
	internal class ComponentNodeCommandHandler : NodeCommandHandler, IPropertyPadProvider
	{
		public object GetActiveComponent()
		{
			if (base.CurrentNodes.Length == 1)
			{
				return base.CurrentNode.DataItem;
			}
			return null;
		}

		public object GetProvider()
		{
			return null;
		}

		public void OnEndEditing(object obj)
		{
		}

		public void OnChanged(object obj)
		{
			ITreeNavigator nodeAtObject = base.Tree.GetNodeAtObject(obj);
			if (nodeAtObject != null)
			{
				IWorkspaceFileObject workspaceFileObject = (IWorkspaceFileObject)nodeAtObject.GetParentDataItem(typeof(IWorkspaceFileObject), includeCurrent: true);
				if (workspaceFileObject != null)
				{
					IdeApp.ProjectOperations.Save(workspaceFileObject);
				}
			}
		}
	}
}
