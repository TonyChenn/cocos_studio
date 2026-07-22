using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model3D.ViewModel
{
	// Token: 0x02000027 RID: 39
	[Extension(typeof(IPropertyFilter))]
	internal class Property3DFilter : IPropertyFilter
	{
		// Token: 0x06000194 RID: 404 RVA: 0x000063E0 File Offset: 0x000045E0
		public bool CanHandle()
		{
			if (Services.Workbench.ActiveDocument == null)
			{
				return false;
			}
			string fileType = Services.Workbench.ActiveDocument.File.GetFileType();
			return fileType == NodeType.Scene3D.ToString();
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006426 File Offset: 0x00004626
		public bool CanShow(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			return !(propertyName == "ZOrder") || selectedObjs.Count <= 1;
		}
	}
}
