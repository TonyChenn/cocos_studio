using System;
using System.Collections.Generic;
using CocoStudio.Core;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	// Token: 0x02000045 RID: 69
	[Extension(typeof(IEditorController))]
	internal class AtlasCharWidthController : Base2DController
	{
		// Token: 0x0600028C RID: 652 RVA: 0x00007C3C File Offset: 0x00005E3C
		public override void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			DefaultEditor defaultEditor = service.GetEditor("StartChar") as DefaultEditor;
			if (defaultEditor != null)
			{
				defaultEditor.Entry.MaxLength = 1;
			}
		}
	}
}
