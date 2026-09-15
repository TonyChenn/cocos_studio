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
	[Extension(typeof(IPropertyFilter))]
	internal class SkeletonPropertyFilter : IPropertyFilter
	{
		public bool CanHandle()
		{
			if (Services.Workbench.ActiveDocument == null)
			{
				return false;
			}
			string fileType = Services.Workbench.ActiveDocument.File.GetFileType();
			return fileType == NodeType.Skeleton.ToString();
		}

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
