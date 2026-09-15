using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	[TypeExtensionPoint(ExtensionAttributeType = typeof(DataModelExtensionAttribute), NodeType = typeof(DataModelExtensionNode))]
	public interface IDataModel
	{
	}
}
