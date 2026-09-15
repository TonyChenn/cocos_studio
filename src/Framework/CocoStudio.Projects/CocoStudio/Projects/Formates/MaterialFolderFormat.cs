using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Model;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IFileFormat))]
	internal class MaterialFolderFormat : CompositeFormat, IPublishProcesser
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is MaterialFolder;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			try
			{
				if (expectedObjectType != typeof(ResourceItem))
				{
					return false;
				}
				if (Directory.Exists(file))
				{
					string path = Path.Combine(file, "materials");
					string path2 = Path.Combine(file, "scripts");
					string path3 = Path.Combine(file, "textures");
					if (Directory.Exists(path) && Directory.Exists(path2) && Directory.Exists(path3))
					{
						return true;
					}
				}
			}
			catch
			{
			}
			return false;
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new MaterialFolder(file);
		}

		public bool CanProcess(ResourceData resourceData)
		{
			return false;
		}

		public HashSet<ResourceData> Process(ResourceData resourceData)
		{
			return null;
		}

		public override bool CanProcess(string filePath)
		{
			return false;
		}

		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".materials"
			};
		}

		public const string MaterialsFolderName = "materials";

		private const string ScriptsFolderName = "scripts";

		private const string TexturesFolderName = "textures";
	}
}
