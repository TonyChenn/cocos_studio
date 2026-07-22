using System;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;

namespace Modules.UI.MainTool.View
{
	// Token: 0x0200000D RID: 13
	internal class ToolbarExtend : HBox
	{
		// Token: 0x0600004F RID: 79 RVA: 0x000039A3 File Offset: 0x00001BA3
		public ToolbarExtend()
		{
			base.Spacing = 6;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000039B8 File Offset: 0x00001BB8
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
