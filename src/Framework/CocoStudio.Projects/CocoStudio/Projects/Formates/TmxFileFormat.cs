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
	public class TmxFileFormat : CompositeFormat, IPublishProcesser
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is TmxFile;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			try
			{
				if (!this.CheckFileSuffix(file, ".tmx"))
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

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new TmxFile(file);
		}

		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			return resourceData.Type != EnumResourceType.PlistSubImage && this.CheckFileSuffix(resourceData.Path, ".tmx");
		}

		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			hashSet.Add(resourceData);
			string filePath = ProjectsService.Instance.GetFullPath(resourceData);
			CSVectorString tmxMapImageArray = CSCocosHelp.GetTmxMapImageArray(filePath);
			string directoryName = Path.GetDirectoryName(resourceData.Path);
			if (tmxMapImageArray != null)
			{
				foreach (string path in tmxMapImageArray)
				{
					string path2 = Path.Combine(directoryName, path);
					ResourceData item = new ResourceData(resourceData.Type, path2);
					hashSet.Add(item);
				}
			}
			return hashSet;
		}

		private bool CheckFileSuffix(string path, string FileSuffix)
		{
			return Path.GetExtension(path).Equals(FileSuffix, StringComparison.OrdinalIgnoreCase);
		}

		public override bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		public override List<string> GetFiles(string filePath)
		{
			CSVectorString tmxMapImageArray = CSCocosHelp.GetTmxMapImageArray(filePath);
			string directoryName = Path.GetDirectoryName(filePath);
			List<string> list = new List<string>();
			if (tmxMapImageArray == null)
			{
				return null;
			}
			foreach (string name in tmxMapImageArray)
			{
				string item = ((FilePath)name).ToAbsolute(directoryName);
				list.Add(item);
			}
			return list;
		}

		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".tmx"
			};
		}
	}
}
