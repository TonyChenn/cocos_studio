using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	internal class ExternalItemLoader : MarshalByRefObject
	{
		public string LoadItems(string asmName, string typeName, string fileName)
		{
			XmlDataSerializer xmlDataSerializer = new XmlDataSerializer(Services.ProjectService.DataContext);
			ToolboxList toolboxList = new ToolboxList();
			object obj = Activator.CreateInstance(asmName, typeName).Unwrap();
			IExternalToolboxLoader externalToolboxLoader = (IExternalToolboxLoader)obj;
			IList<ItemToolboxNode> collection = externalToolboxLoader.Load(fileName);
			toolboxList.AddRange(collection);
			StringWriter stringWriter = new StringWriter();
			xmlDataSerializer.Serialize(stringWriter, toolboxList);
			return stringWriter.ToString();
		}
	}
}
