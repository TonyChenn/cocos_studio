using System;
using System.Collections.Generic;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;

namespace Modules.Communal.Render.ExtensionModel
{
	public class ContextMenuShowingArgs : EventArgs
	{
		public bool Enable { get; set; }

		public IEnumerable<VisualObject> SelectedParentObject { get; private set; }

		public PointF ClickPoint { get; private set; }

		public bool IsShowOnRenderArea { get; private set; }

		public ContextMenuShowingArgs()
		{
			this.Enable = true;
		}

		public ContextMenuShowingArgs(IEnumerable<VisualObject> selectedParentObject, PointF clickPoint, bool isShowOnRenderArea) : this()
		{
			this.SelectedParentObject = selectedParentObject;
			this.ClickPoint = clickPoint;
			this.IsShowOnRenderArea = isShowOnRenderArea;
		}
	}
}
