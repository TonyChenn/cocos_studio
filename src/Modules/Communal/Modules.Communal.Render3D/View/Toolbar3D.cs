using System;
using Gtk;
using Modules.Communal.Render.Model;
using Modules.Communal.Render.View;

namespace Modules.Communal.Render3D.View
{
	internal class Toolbar3D : HBox
	{
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
