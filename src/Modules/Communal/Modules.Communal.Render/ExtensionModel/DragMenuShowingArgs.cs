using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;

namespace Modules.Communal.Render.ExtensionModel
{
	public class DragMenuShowingArgs
	{
		public bool Enable { get; set; }

		public VisualObject DragObject { get; set; }

		public ResourceItem DragResourceItem { get; set; }

		public DragMenuShowingArgs(VisualObject dragObject, ResourceItem dragResourceItem)
		{
			this.Enable = true;
			this.DragObject = dragObject;
			this.DragResourceItem = dragResourceItem;
		}
	}
}
