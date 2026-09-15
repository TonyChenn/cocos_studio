using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(SingleNodeObject))]
	public class SingleNodeObjectData : NodeObjectData
	{
		public string GetClassName()
		{
			return "Node";
		}
	}
}
