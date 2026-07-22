using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200001F RID: 31
	[DataModelExtension(typeof(SingleNodeObject))]
	public class SingleNodeObjectData : NodeObjectData
	{
		// Token: 0x06000142 RID: 322 RVA: 0x000051B0 File Offset: 0x000033B0
		public string GetClassName()
		{
			return "Node";
		}
	}
}
