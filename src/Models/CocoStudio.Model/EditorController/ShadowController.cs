using System;
using System.Collections.Generic;
using CocoStudio.Core;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	// Token: 0x0200004C RID: 76
	[Extension(typeof(IEditorController))]
	internal class ShadowController : Base2DController
	{
		// Token: 0x0600029A RID: 666 RVA: 0x000084A7 File Offset: 0x000066A7
		public ShadowController()
		{
			base.AddCorrespondProperty("ShadowEnabled");
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000084C0 File Offset: 0x000066C0
		public override void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			IPropertyEditor editor = service.GetEditor("ShadowOffsetX");
			IPropertyEditor editor2 = service.GetEditor("ShadowColor");
			if (editor != null && editor2 != null)
			{
				bool visible = true;
				foreach (object obj in selectedObjs)
				{
					ILabelEffect labelEffect = obj as ILabelEffect;
					if (labelEffect == null || !labelEffect.ShadowEnabled)
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
