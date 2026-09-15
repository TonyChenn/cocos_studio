using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(ICompositeResourceProcesser))]
	[Extension(typeof(IPublishProcesser))]
	internal class MeshFileFormat : CompositeFormat, IPublishProcesser
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is MeshFile;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			try
			{
				if (!FileFormat.CheckFileSuffix(file, new string[]
				{
					".c3b",
					".c3t",
					".obj"
				}))
				{
					return false;
				}
				if (!expectedObjectType.Equals(typeof(ResourceItem)))
				{
					return false;
				}
				return true;
			}
			catch
			{
			}
			return false;
		}

		private bool CheckFileSuffix(string path, string FileSuffix)
		{
			return Path.GetExtension(path).Equals(FileSuffix, StringComparison.OrdinalIgnoreCase);
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new MeshFile(file);
		}

		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			string path = resourceData.Path;
			return FileFormat.CheckFileSuffix(path, new string[]
			{
				".c3b",
				".c3t",
				".obj"
			});
		}

		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			hashSet.Add(resourceData);
			string filePath = ProjectsService.Instance.GetFullPath(resourceData);
			CSVectorString meshResourceArray = CSCocosHelp.GetMeshResourceArray(filePath);
			if (meshResourceArray == null)
			{
				return hashSet;
			}
			string directoryName = Path.GetDirectoryName(resourceData.Path);
			foreach (string path in meshResourceArray)
			{
				string fileName = Path.GetFileName(path);
				string path2 = Path.Combine(directoryName, fileName);
				ResourceData item = new ResourceData(resourceData.Type, path2);
				hashSet.Add(item);
			}
			return hashSet;
		}

		public override bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		public override List<string> GetFiles(string filePath)
		{
			return new List<string>();
		}

		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".c3b",
				".c3t",
				".obj"
			};
		}

		private const string MeshFileSuffixC3B = ".c3b";

		private const string MeshFileSuffixC3T = ".c3t";

		private const string MeshFileSuffixOBJ = ".obj";
	}
}
