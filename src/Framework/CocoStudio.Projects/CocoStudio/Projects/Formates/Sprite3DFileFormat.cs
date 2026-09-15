using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IPublishProcesser))]
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(ICompositeResourceProcesser))]
	public class Sprite3DFileFormat : CompositeFormat, IPublishProcesser
	{
		public Sprite3DFileFormat()
		{
			base.IsHiddenCompositeFile = false;
		}

		protected override bool OnCanWriteFile(object obj)
		{
			return obj is Sprite3DFile;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			try
			{
				if (!this.CheckFileSuffix(file, Sprite3DFile.FileSuffix))
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
			return new Sprite3DFile(file);
		}

		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			return resourceData.Type != EnumResourceType.PlistSubImage && this.CheckFileSuffix(resourceData.Path, Sprite3DFile.FileSuffix);
		}

		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			hashSet.Add(resourceData);
			string filePath = ProjectsService.Instance.GetFullPath(resourceData);
			CSVectorString sprite3DTextureArray = CSCocosHelp.GetSprite3DTextureArray(filePath);
			string directoryName = Path.GetDirectoryName(resourceData.Path);
			if (sprite3DTextureArray != null)
			{
				foreach (string name in sprite3DTextureArray)
				{
					FilePath filePath2 = name;
					string path = Path.Combine(directoryName, filePath2);
					ResourceData item = new ResourceData(resourceData.Type, path);
					hashSet.Add(item);
				}
			}
			return hashSet;
		}

		private bool CheckFileSuffix(string path, string[] FileSuffix)
		{
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				flag |= Path.GetExtension(path).Equals(FileSuffix[i], StringComparison.OrdinalIgnoreCase);
			}
			return flag;
		}

		public override bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		public override List<string> GetFiles(string filePath)
		{
			CSVectorString sprite3DTextureArray = CSCocosHelp.GetSprite3DTextureArray(filePath);
			string directoryName = Path.GetDirectoryName(filePath);
			List<string> list = new List<string>();
			if (sprite3DTextureArray == null)
			{
				return null;
			}
			foreach (string name in sprite3DTextureArray)
			{
				string item = ((FilePath)name).ToAbsolute(directoryName);
				list.Add(item);
			}
			return list;
		}

		public override List<string> GetPretreatmentTypes()
		{
			return Sprite3DFile.FileSuffix.ToList<string>();
		}

		public override List<string> GetAfterTypes()
		{
			return Sprite3DFile.FileSuffix.ToList<string>();
		}
	}
}
