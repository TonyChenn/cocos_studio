using System;
using System.Collections.Generic;
using CocoStudio.Core;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	[Extension(typeof(IEditorController))]
	internal class OutlineController : Base2DController
	{
		public OutlineController()
		{
			base.AddCorrespondProperty("OutlineEnabled");
		}

		public override void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			IPropertyEditor editor = service.GetEditor("OutlineSize");
			IPropertyEditor editor2 = service.GetEditor("OutlineColor");
			if (editor != null && editor2 != null)
			{
				bool visible = true;
				foreach (object obj in selectedObjs)
				{
					ILabelEffect labelEffect = obj as ILabelEffect;
					if (labelEffect == null || !labelEffect.OutlineEnabled)
					{
						visible = false;
						break;
					}
				}
				editor.Visible = visible;
				editor2.Visible = visible;
			}
		}
	}
}
