using System;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Skeleton
{
	internal abstract class BaseSkeletonController : BaseEditorController
	{
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
