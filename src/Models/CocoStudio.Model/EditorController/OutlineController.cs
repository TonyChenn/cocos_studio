using System;
using System.Collections.Generic;
using CocoStudio.Core;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	// Token: 0x02000049 RID: 73
	[Extension(typeof(IEditorController))]
	internal class OutlineController : Base2DController
	{
		// Token: 0x06000294 RID: 660 RVA: 0x000080F4 File Offset: 0x000062F4
		public OutlineController()
		{
			base.AddCorrespondProperty("OutlineEnabled");
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000810C File Offset: 0x0000630C
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
