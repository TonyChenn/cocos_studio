using System;
using Gtk;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Render.View
{
	// Token: 0x02000038 RID: 56
	internal class Toolbar2D : HBox
	{
		// Token: 0x06000299 RID: 665 RVA: 0x0000E3A4 File Offset: 0x0000C5A4
		public Toolbar2D(ToolGroup toolGroup)
		{
			ToolGroupView child = new ToolGroupView(toolGroup);
			AlignToolView child2 = new AlignToolView();
			base.Spacing = 6;
			base.PackStart(child, false, false, 1U);
			base.PackStart(new VSeparator());
			base.PackStart(child2, false, false, 1U);
		}
	}
}
