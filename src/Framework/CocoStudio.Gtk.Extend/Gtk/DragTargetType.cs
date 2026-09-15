using System;

namespace Gtk
{
	public static class DragTargetType
	{
		public static readonly TargetEntry CocoStudioTarget = new TargetEntry("application/CocoStudio", TargetFlags.App, 0U);

		public static readonly TargetEntry FileDropTarget = new TargetEntry("text/uri-list", (TargetFlags)0, 0U);

		public static readonly TargetEntry ColorDropTarget = new TargetEntry("application/x-color", TargetFlags.App, 0U);
	}
}
