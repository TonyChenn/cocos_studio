using System;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000008 RID: 8
	internal abstract class BaseSkeletonController : BaseEditorController
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00003130 File Offset: 0x00001330
		public override bool CanHandle()
		{
			if (Services.Workbench.ActiveDocument == null)
			{
				return false;
			}
			string fileType = Services.Workbench.ActiveDocument.File.GetFileType();
			return fileType == NodeType.Skeleton.ToString();
		}
	}
}
