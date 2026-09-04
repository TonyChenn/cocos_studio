using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	// Token: 0x0200000A RID: 10
	[Extension(typeof(IEditorController))]
	internal class Light3DController : Base3DController
	{
		// Token: 0x06000075 RID: 117 RVA: 0x000027CC File Offset: 0x000009CC
		public Light3DController()
		{
			base.AddCorrespondProperty("Type");
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000027E0 File Offset: 0x000009E0
		public override void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			IPropertyEditor editor = service.GetEditor("Range");
			IPropertyEditor editor2 = service.GetEditor("OuterAngle");
			if (editor == null || editor2 == null)
			{
				return;
			}
			int num = -1;
			foreach (object obj in selectedObjs)
			{
				Light3DObject light3DObject = obj as Light3DObject;
				if (light3DObject == null)
				{
					num = -1;
					break;
				}
				if (num == -1)
				{
					num = (int)light3DObject.Type;
				}
				else if (num != (int)light3DObject.Type)
				{
					num = -1;
					break;
				}
			}
			switch (num)
			{
			case 0:
				editor.Visible = false;
				editor2.Visible = false;
				return;
			case 1:
				editor.Visible = true;
				editor2.Visible = false;
				return;
			case 2:
				editor.Visible = true;
				editor2.Visible = true;
				return;
			case 3:
				editor.Visible = false;
				editor2.Visible = false;
				return;
			default:
				return;
			}
		}
	}
}
