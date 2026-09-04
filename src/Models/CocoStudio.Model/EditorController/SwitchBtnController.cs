using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.Editor;
using CocoStudio.Model.ViewModel;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	// Token: 0x02000046 RID: 70
	[Extension(typeof(IEditorController))]
	internal class SwitchBtnController : Base2DController
	{
		// Token: 0x0600028E RID: 654 RVA: 0x00007C80 File Offset: 0x00005E80
		public SwitchBtnController()
		{
			base.AddCorrespondProperty("OperationFlag");
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00007C98 File Offset: 0x00005E98
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
