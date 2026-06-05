using System;

namespace Gtk
{
	// Token: 0x02000055 RID: 85
	public static class DragTargetType
	{
		// Token: 0x040002F4 RID: 756
		public static readonly TargetEntry CocoStudioTarget = new TargetEntry("application/CocoStudio", TargetFlags.App, 0U);

		// Token: 0x040002F5 RID: 757
		public static readonly TargetEntry FileDropTarget = new TargetEntry("text/uri-list", (TargetFlags)0, 0U);

		// Token: 0x040002F6 RID: 758
		public static readonly TargetEntry ColorDropTarget = new TargetEntry("application/x-color", TargetFlags.App, 0U);
	}
}
