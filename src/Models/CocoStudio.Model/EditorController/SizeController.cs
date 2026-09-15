using System;
using System.Collections.Generic;
using CocoStudio.Core;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	[Extension(typeof(IEditorController))]
	internal class SizeController : Base2DController
	{
		public SizeController()
		{
			base.AddCorrespondProperty("IsCustomSize");
			base.AddCorrespondProperty("FileData");
		}

		public override void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			IPropertyEditor editor = service.GetEditor("Size");
			if (editor != null)
			{
				bool enable = true;
				OperationMask? requestOperation = editor.PropertyItem.RequestOperation;
				foreach (object obj in selectedObjs)
				{
					if (!Controller2DService.CheckSizeCanUse(obj))
					{
						enable = false;
						break;
					}
					if (!Controller2DService.CheckOperationModeIsValid(obj, requestOperation))
					{
						enable = false;
						break;
					}
				}
				editor.Enable = enable;
			}
		}
	}
}
