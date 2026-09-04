using System;
using Gtk;
using Modules.Communal.Render.Model;
using Modules.Communal.Render.View;

namespace Modules.Communal.Render3D.View
{
	// Token: 0x0200000E RID: 14
	internal class Toolbar3D : HBox
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00003000 File Offset: 0x00001200
		public Toolbar3D(ToolGroup toolGroup)
		{
			ToolGroupView child = new ToolGroupView(toolGroup);
			CoordinateSystemView child2 = new CoordinateSystemView();
			base.Spacing = 6;
			base.PackStart(child, false, false, 0U);
			base.PackStart(new VSeparator());
			base.PackStart(child2, false, false, 1U);
			base.ShowAll();
		}
	}
}
