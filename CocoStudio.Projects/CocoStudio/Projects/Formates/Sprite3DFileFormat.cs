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
	// Token: 0x0200001C RID: 28
	[Extension(typeof(IPublishProcesser))]
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(ICompositeResourceProcesser))]
	public class Sprite3DFileFormat : CompositeFormat, IPublishProcesser
	{
		// Token: 0x06000095 RID: 149 RVA: 0x000038FC File Offset: 0x00001AFC
		public Sprite3DFileFormat()
		{
			base.IsHiddenCompositeFile = false;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000390B File Offset: 0x00001B0B
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is Sprite3DFile;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003918 File Offset: 0x00001B18
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

		// Token: 0x06000098 RID: 152 RVA: 0x0000396C File Offset: 0x00001B6C
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new Sprite3DFile(file);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003974 File Offset: 0x00001B74
		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			return resourceData.Type != EnumResourceType.PlistSubImage && this.CheckFileSuffix(resourceData.Path, Sprite3DFile.FileSuffix);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003994 File Offset: 0x00001B94
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

		// Token: 0x0600009B RID: 155 RVA: 0x00003A40 File Offset: 0x00001C40
		private bool CheckFileSuffix(string path, string[] FileSuffix)
		{
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				flag |= Path.GetExtension(path).Equals(FileSuffix[i], StringComparison.OrdinalIgnoreCase);
			}
			return flag;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003A6E File Offset: 0x00001C6E
		public override bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003A88 File Offset: 0x00001C88
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
				string item = name.ToAbsolute(directoryName);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003B0C File Offset: 0x00001D0C
		public override List<string> GetPretreatmentTypes()
		{
			return Sprite3DFile.FileSuffix.ToList<string>();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003B18 File Offset: 0x00001D18
		public override List<string> GetAfterTypes()
		{
			return Sprite3DFile.FileSuffix.ToList<string>();
		}
	}
}
