using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	[Extension(typeof(IPropertyFilter))]
	internal class PropertyNodeFilter : IPropertyFilter
	{
		public bool CanHandle()
		{
			bool result;
			if (Services.Workbench.ActiveDocument == null)
			{
				result = false;
			}
			else
			{
				string fileType = Services.Workbench.ActiveDocument.File.GetFileType();
				result = (fileType == NodeType.Node.ToString());
			}
			return result;
		}

		public bool CanShow(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			return !(propertyName == "HorizontalEdge");
		}
	}
}
