using System;
using System.Collections.Generic;
using CocoStudio.Core;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	[Extension(typeof(IEditorController))]
	internal class AtlasCharWidthController : Base2DController
	{
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
