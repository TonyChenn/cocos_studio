using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	// Token: 0x02000048 RID: 72
	[Extension(typeof(IEditorController))]
	internal class OperationModeController : Base2DController
	{
		// Token: 0x06000292 RID: 658 RVA: 0x00007F50 File Offset: 0x00006150
		public OperationModeController()
		{
			base.AddCorrespondProperty("OperationFlag");
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00007F68 File Offset: 0x00006168
		public override void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			List<IPropertyEditor> editors = service.GetEditors();
			foreach (IPropertyEditor propertyEditor in editors)
			{
				if (propertyEditor.PropertyItem.RequestOperation != null)
				{
					bool visible = true;
					bool enable = true;
					foreach (object obj in selectedObjs)
					{
						if (!Controller2DService.CheckOperationModeIsValid(obj, propertyEditor.PropertyItem.RequestOperation))
						{
							if (propertyEditor.PropertyItem.Name == "Size" && (obj is SpriteObject || obj is GameMapObject))
							{
								enable = false;
								break;
							}
							visible = false;
							break;
						}
						else if (propertyEditor.PropertyItem.Name == "Size" && !Controller2DService.CheckSizeCanUse(obj))
						{
							enable = false;
							break;
						}
					}
					propertyEditor.Visible = visible;
					propertyEditor.Enable = enable;
				}
			}
		}
	}
}
