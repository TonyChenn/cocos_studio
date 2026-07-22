using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.EditorController
{
	// Token: 0x02000009 RID: 9
	internal abstract class Base3DController : BaseEditorController
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00002760 File Offset: 0x00000960
		static Base3DController()
		{
			Base3DController.correspondFileType.Add(NodeType.Scene3D.ToString());
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002784 File Offset: 0x00000984
		public override bool CanHandle()
		{
			if (Services.Workbench.ActiveDocument == null)
			{
				return false;
			}
			string fileType = Services.Workbench.ActiveDocument.File.GetFileType();
			return Base3DController.correspondFileType.Contains(fileType);
		}

		// Token: 0x04000035 RID: 53
		private static List<string> correspondFileType = new List<string>();
	}
}
