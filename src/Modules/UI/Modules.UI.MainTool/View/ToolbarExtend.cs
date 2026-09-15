using System;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;

namespace Modules.UI.MainTool.View
{
	internal class ToolbarExtend : HBox
	{
		public ToolbarExtend()
		{
			base.Spacing = 6;
		}

		public void OnProjectChanged(CocosItem project)
		{
			this.RemoveAll();
			DocumentExtend activeDocument = Services.Workbench.ActiveDocument;
			if (activeDocument != null)
			{
				Widget toolbar = activeDocument.GetToolbar();
				if (toolbar != null)
				{
					base.PackStart(new VSeparator(), false, false, 0U);
					base.PackStart(toolbar, false, false, 0U);
					base.ShowAll();
				}
			}
		}
	}
}
