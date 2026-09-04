using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	internal class ToolboxConfiguration
	{
		private ToolboxList itemList = new ToolboxList();

		private List<string> loadedDefaultProviders = new List<string>();

		[ItemProperty]
		public ToolboxList ItemList => itemList;

		[ItemProperty("Provider", Scope = "*")]
		[ItemProperty]
		public List<string> LoadedDefaultProviders => loadedDefaultProviders;

		public void SaveContents(string fileName)
		{
			using (StreamWriter writer = new StreamWriter(fileName))
			{
				XmlDataSerializer xmlDataSerializer = new XmlDataSerializer(Services.ProjectService.DataContext);
				xmlDataSerializer.Serialize(writer, this);
			}
		}

		public static ToolboxConfiguration LoadFromFile(string fileName)
		{
			object obj;
			using (StreamReader reader = new StreamReader(fileName))
			{
				XmlDataSerializer xmlDataSerializer = new XmlDataSerializer(Services.ProjectService.DataContext);
				obj = xmlDataSerializer.Deserialize(reader, typeof(ToolboxConfiguration));
			}
			return (ToolboxConfiguration)obj;
		}
	}
}
