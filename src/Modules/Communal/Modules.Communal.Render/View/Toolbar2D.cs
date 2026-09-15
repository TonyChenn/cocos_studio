using System;
using Gtk;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Render.View
{
	internal class Toolbar2D : HBox
	{
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
