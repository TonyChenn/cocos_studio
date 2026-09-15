using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	[Extension(typeof(IEditorController))]
	internal class OperationModeController : Base2DController
	{
		public OperationModeController()
		{
			base.AddCorrespondProperty("OperationFlag");
		}

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
