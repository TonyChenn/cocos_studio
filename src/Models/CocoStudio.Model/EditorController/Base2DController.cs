using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.EditorController
{
	internal abstract class Base2DController : BaseEditorController
	{
		static Base2DController()
		{
			Base2DController.correspondFileType.Add(NodeType.Scene.ToString());
			Base2DController.correspondFileType.Add(NodeType.Layer.ToString());
			Base2DController.correspondFileType.Add(NodeType.Node.ToString());
			Base2DController.correspondFileType.Add(NodeType.Skeleton.ToString());
		}

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

		private static List<string> correspondFileType = new List<string>();
	}
}
