using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.EditorController
{
	internal abstract class Base3DController : BaseEditorController
	{
		static Base3DController()
		{
			Base3DController.correspondFileType.Add(NodeType.Scene3D.ToString());
		}

		public override bool CanHandle()
		{
			if (Services.Workbench.ActiveDocument == null)
			{
				return false;
			}
			string fileType = Services.Workbench.ActiveDocument.File.GetFileType();
			return Base3DController.correspondFileType.Contains(fileType);
		}

		private static List<string> correspondFileType = new List<string>();
	}
}
