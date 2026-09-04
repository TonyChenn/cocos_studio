using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.DesignerSupport.Toolbox;
using MonoDevelop.Ide;

namespace MonoDevelop.DesignerSupport
{
	[Serializable]
	internal class ComponentIndex
	{
		private List<ComponentIndexFile> files = new List<ComponentIndexFile>();

		private static string ToolboxIndexFile => UserProfile.Current.CacheDir.Combine("ToolboxIndex.xml");

		[ItemProperty]
		public List<ComponentIndexFile> Files => files;

		internal static ComponentIndex Load()
		{
			if (!File.Exists(ToolboxIndexFile))
			{
				return new ComponentIndex();
			}
			XmlDataSerializer xmlDataSerializer = new XmlDataSerializer(IdeApp.Services.ProjectService.DataContext);
			try
			{
				using (StreamReader reader = new StreamReader(ToolboxIndexFile))
				{
					return (ComponentIndex)xmlDataSerializer.Deserialize(reader, typeof(ComponentIndex));
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError(ex.ToString());
				return new ComponentIndex();
			}
		}

		public void Save()
		{
			XmlDataSerializer xmlDataSerializer = new XmlDataSerializer(IdeApp.Services.ProjectService.DataContext);
			try
			{
				using (StreamWriter writer = new StreamWriter(ToolboxIndexFile))
				{
					xmlDataSerializer.Serialize(writer, this, typeof(ComponentIndex));
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError(ex.ToString());
			}
		}

		public ComponentIndexFile AddFile(string file)
		{
			ComponentIndexFile componentIndexFile = new ComponentIndexFile(file);
			LoaderContext loaderContext = new LoaderContext();
			try
			{
				componentIndexFile.Update(loaderContext);
			}
			finally
			{
				loaderContext.Dispose();
			}
			if (componentIndexFile.Components.Count == 0)
			{
				return null;
			}
			files.Add(componentIndexFile);
			return componentIndexFile;
		}
	}
}
