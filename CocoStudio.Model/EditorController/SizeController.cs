using System;
using System.Collections.Generic;
using CocoStudio.Core;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	// Token: 0x0200004D RID: 77
	[Extension(typeof(IEditorController))]
	internal class SizeController : Base2DController
	{
		// Token: 0x0600029C RID: 668 RVA: 0x00008584 File Offset: 0x00006784
		public SizeController()
		{
			base.AddCorrespondProperty("IsCustomSize");
			base.AddCorrespondProperty("FileData");
		}

		// Token: 0x0600029D RID: 669 RVA: 0x000085A8 File Offset: 0x000067A8
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
