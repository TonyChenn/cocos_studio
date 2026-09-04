using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x0200004C RID: 76
	[TypeExtensionPoint(ExtensionAttributeType = typeof(DataModelExtensionAttribute), NodeType = typeof(DataModelExtensionNode))]
	public interface IDataModel
	{
	}
}
