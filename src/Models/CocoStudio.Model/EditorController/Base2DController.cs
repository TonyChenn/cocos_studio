using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.EditorController
{
	// Token: 0x02000044 RID: 68
	internal abstract class Base2DController : BaseEditorController
	{
		// Token: 0x06000289 RID: 649 RVA: 0x00007B6C File Offset: 0x00005D6C
		static Base2DController()
		{
			Base2DController.correspondFileType.Add(NodeType.Scene.ToString());
			Base2DController.correspondFileType.Add(NodeType.Layer.ToString());
			Base2DController.correspondFileType.Add(NodeType.Node.ToString());
			Base2DController.correspondFileType.Add(NodeType.Skeleton.ToString());
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00007BDC File Offset: 0x00005DDC
		public override bool CanHandle()
		{
			bool result;
			if (Services.Workbench.ActiveDocument == null)
			{
				result = false;
			}
			else
			{
				string fileType = Services.Workbench.ActiveDocument.File.GetFileType();
				result = Base2DController.correspondFileType.Contains(fileType);
			}
			return result;
		}

		// Token: 0x04000114 RID: 276
		private static List<string> correspondFileType = new List<string>();
	}
}
