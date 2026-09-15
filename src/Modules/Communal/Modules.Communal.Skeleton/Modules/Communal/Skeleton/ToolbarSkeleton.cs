using System;
using Gtk;
using Modules.Communal.Render.Model;
using Modules.Communal.Render.View;

namespace Modules.Communal.Skeleton
{
	internal class ToolbarSkeleton : HBox
	{
		public ToolbarSkeleton(ToolGroup toolGroup)
		{
			base.Spacing = 6;
			ToolGroupView child = new ToolGroupView(toolGroup);
			base.PackStart(child, false, false, 0U);
		}
	}
}
