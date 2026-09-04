using Gtk;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.DesignerSupport
{
	internal class PropertyPadVisitor : ICommandTargetVisitor
	{
		private bool found;

		private bool visitedCurrentDoc;

		private bool visitingCurrentDoc;

		private Widget activeWidget;

		public void Start()
		{
			found = false;
			visitedCurrentDoc = false;
			activeWidget = null;
		}

		public void End()
		{
			if (found)
			{
				return;
			}
			if (!visitingCurrentDoc && !visitedCurrentDoc)
			{
				DefaultWorkbench defaultWorkbench = (DefaultWorkbench)IdeApp.Workbench.RootWindow;
				if (defaultWorkbench.ActiveWorkbenchWindow != null)
				{
					visitingCurrentDoc = true;
					IdeApp.CommandService.VisitCommandTargets(this, defaultWorkbench.ActiveWorkbenchWindow);
					visitingCurrentDoc = false;
					return;
				}
			}
			DesignerSupport.Service.ReSetPad();
		}

		public bool Visit(object ob)
		{
			if (activeWidget == null && ob is Widget)
			{
				activeWidget = (Widget)ob;
			}
			if (ob == ((DefaultWorkbench)IdeApp.Workbench.RootWindow).ActiveWorkbenchWindow)
			{
				visitedCurrentDoc = true;
			}
			if (ob is PropertyPad)
			{
				found = true;
				return true;
			}
			if (ob is IPropertyPadProvider)
			{
				DesignerSupport.Service.SetPadContent((IPropertyPadProvider)ob, activeWidget);
				found = true;
				return true;
			}
			if (ob is ICustomPropertyPadProvider)
			{
				DesignerSupport.Service.SetPadContent((ICustomPropertyPadProvider)ob, activeWidget);
				found = true;
				return true;
			}
			return false;
		}
	}
}
