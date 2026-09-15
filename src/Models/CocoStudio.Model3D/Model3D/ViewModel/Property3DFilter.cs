using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model3D.ViewModel
{
	[Extension(typeof(IPropertyFilter))]
	internal class Property3DFilter : IPropertyFilter
	{
		public bool CanHandle()
		{
			if (Services.Workbench.ActiveDocument == null)
			{
				return false;
			}
			string fileType = Services.Workbench.ActiveDocument.File.GetFileType();
			return fileType == NodeType.Scene3D.ToString();
		}

		public bool CanShow(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			return !(propertyName == "ZOrder") || selectedObjs.Count <= 1;
		}
	}
}
