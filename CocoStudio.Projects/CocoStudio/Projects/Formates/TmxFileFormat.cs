using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000030 RID: 48
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(ICompositeResourceProcesser))]
	[Extension(typeof(IPublishProcesser))]
	public class TmxFileFormat : CompositeFormat, IPublishProcesser
	{
		// Token: 0x06000117 RID: 279 RVA: 0x0000556F File Offset: 0x0000376F
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is TmxFile;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000557C File Offset: 0x0000377C
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

		// Token: 0x06000119 RID: 281 RVA: 0x000055D0 File Offset: 0x000037D0
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new TmxFile(file);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000055D8 File Offset: 0x000037D8
		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			return resourceData.Type != EnumResourceType.PlistSubImage && this.CheckFileSuffix(resourceData.Path, ".tmx");
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000055F8 File Offset: 0x000037F8
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

		// Token: 0x0600011C RID: 284 RVA: 0x00005698 File Offset: 0x00003898
		private bool CheckFileSuffix(string path, string FileSuffix)
		{
			return Path.GetExtension(path).Equals(FileSuffix, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000056A7 File Offset: 0x000038A7
		public override bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000056C0 File Offset: 0x000038C0
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

		// Token: 0x0600011F RID: 287 RVA: 0x00005744 File Offset: 0x00003944
		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".tmx"
			};
		}
	}
}
