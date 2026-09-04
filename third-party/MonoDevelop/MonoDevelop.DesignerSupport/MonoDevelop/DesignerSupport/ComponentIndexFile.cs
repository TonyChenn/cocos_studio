using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Serialization;
using MonoDevelop.DesignerSupport.Toolbox;

namespace MonoDevelop.DesignerSupport
{
	[Serializable]
	internal class ComponentIndexFile
	{
		[ItemProperty]
		private string fileName;

		[ItemProperty]
		private string location;

		[ItemProperty]
		private string timestamp;

		[ItemProperty("Components")]
		private List<ItemToolboxNode> entries = new List<ItemToolboxNode>();

		public bool NeedsUpdate
		{
			get
			{
				if (File.Exists(fileName))
				{
					return GetFileTimestamp(fileName) != timestamp;
				}
				return false;
			}
		}

		public string Location
		{
			get
			{
				if (location == null)
				{
					return fileName;
				}
				return location;
			}
		}

		public string FileName => fileName;

		public string Name => Path.GetFileNameWithoutExtension(fileName);

		public List<ItemToolboxNode> Components => entries;

		public ComponentIndexFile()
		{
		}

		public ComponentIndexFile(string fileName)
		{
			this.fileName = fileName;
		}

		public void Update(LoaderContext ctx)
		{
			IList<ItemToolboxNode> fileItems = DesignerSupport.Service.ToolboxService.GetFileItems(ctx, fileName);
			location = null;
			timestamp = GetFileTimestamp(fileName);
			entries = new List<ItemToolboxNode>(fileItems);
			if (entries.Count > 0 && Runtime.SystemAssemblyService.GetPackageFromPath(fileName) != null)
			{
				location = Runtime.SystemAssemblyService.DefaultAssemblyContext.GetAssemblyFullName(fileName, TargetFramework.Default);
			}
		}

		private string GetFileTimestamp(string file)
		{
			DateTime lastWriteTime = File.GetLastWriteTime(file);
			return XmlConvert.ToString(lastWriteTime, XmlDateTimeSerializationMode.Utc);
		}
	}
}
