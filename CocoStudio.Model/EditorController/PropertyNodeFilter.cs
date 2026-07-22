using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	// Token: 0x0200012F RID: 303
	[Extension(typeof(IPropertyFilter))]
	internal class PropertyNodeFilter : IPropertyFilter
	{
		// Token: 0x06000B45 RID: 2885 RVA: 0x0002CA18 File Offset: 0x0002AC18
		public bool CanHandle()
		{
			bool result;
			if (Services.Workbench.ActiveDocument == null)
			{
				result = false;
			}
			else
			{
				string fileType = Services.Workbench.ActiveDocument.File.GetFileType();
				result = (fileType == NodeType.Node.ToString());
			}
			return result;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0002CA74 File Offset: 0x0002AC74
		public bool CanShow(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			return !(propertyName == "HorizontalEdge");
		}
	}
}
