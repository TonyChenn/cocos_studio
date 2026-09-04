using System;
using Gtk;
using Modules.Communal.Render.Model;
using Modules.Communal.Render.View;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000022 RID: 34
	internal class ToolbarSkeleton : HBox
	{
		// Token: 0x0600016B RID: 363 RVA: 0x00008144 File Offset: 0x00006344
		public ToolbarSkeleton(ToolGroup toolGroup)
		{
			base.Spacing = 6;
			ToolGroupView child = new ToolGroupView(toolGroup);
			base.PackStart(child, false, false, 0U);
		}
	}
}
