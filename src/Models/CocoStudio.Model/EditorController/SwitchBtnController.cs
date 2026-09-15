using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.Editor;
using CocoStudio.Model.ViewModel;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	[Extension(typeof(IEditorController))]
	internal class SwitchBtnController : Base2DController
	{
		public SwitchBtnController()
		{
			base.AddCorrespondProperty("OperationFlag");
		}

		public override void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			UISizeEditor uisizeEditor = service.GetEditor("Size") as UISizeEditor;
			PositionEditor positionEditor = service.GetEditor("Position") as PositionEditor;
			if (uisizeEditor != null || positionEditor != null)
			{
				bool showSwitchButton = true;
				foreach (object obj in selectedObjs)
				{
					AbstractNodeObject abstractNodeObject = obj as AbstractNodeObject;
					if (abstractNodeObject == null || !abstractNodeObject.OperationFlag.HasFlag(OperationMask.LayoutFlag))
					{
						showSwitchButton = false;
						break;
					}
				}
				if (uisizeEditor != null)
				{
					uisizeEditor.ShowSwitchButton = showSwitchButton;
				}
				if (positionEditor != null)
				{
					positionEditor.ShowSwitchButton = showSwitchButton;
				}
			}
		}
	}
}
