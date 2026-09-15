using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	[Extension(typeof(IEditorController))]
	internal class Light3DController : Base3DController
	{
		public Light3DController()
		{
			base.AddCorrespondProperty("Type");
		}

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
