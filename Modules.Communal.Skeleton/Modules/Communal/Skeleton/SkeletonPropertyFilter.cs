using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace Modules.Communal.Skeleton
{
	// Token: 0x0200000B RID: 11
	[Extension(typeof(IPropertyFilter))]
	internal class SkeletonPropertyFilter : IPropertyFilter
	{
		// Token: 0x06000049 RID: 73 RVA: 0x0000324C File Offset: 0x0000144C
		public bool CanHandle()
		{
			if (Services.Workbench.ActiveDocument == null)
			{
				return false;
			}
			string fileType = Services.Workbench.ActiveDocument.File.GetFileType();
			return fileType == NodeType.Skeleton.ToString();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003294 File Offset: 0x00001494
		public bool CanShow(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			if (propertyName == "HorizontalEdge")
			{
				return false;
			}
			if (propertyName == "Position")
			{
				bool? flag = null;
				foreach (object obj in selectedObjs)
				{
					if (flag == null)
					{
						flag = new bool?(obj is BoneObject);
					}
					else
					{
						bool flag2 = obj is BoneObject;
						if (flag != flag2)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}
	}
}
