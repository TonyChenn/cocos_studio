using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Model;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000016 RID: 22
	[Extension(typeof(IFileFormat))]
	internal class MaterialFolderFormat : CompositeFormat, IPublishProcesser
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00003067 File Offset: 0x00001267
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is MaterialFolder;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003074 File Offset: 0x00001274
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

		// Token: 0x0600006D RID: 109 RVA: 0x0000310C File Offset: 0x0000130C
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new MaterialFolder(file);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003114 File Offset: 0x00001314
		public bool CanProcess(ResourceData resourceData)
		{
			return false;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003117 File Offset: 0x00001317
		public HashSet<ResourceData> Process(ResourceData resourceData)
		{
			return null;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000311A File Offset: 0x0000131A
		public override bool CanProcess(string filePath)
		{
			return false;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003120 File Offset: 0x00001320
		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".materials"
			};
		}

		// Token: 0x0400001B RID: 27
		public const string MaterialsFolderName = "materials";

		// Token: 0x0400001C RID: 28
		private const string ScriptsFolderName = "scripts";

		// Token: 0x0400001D RID: 29
		private const string TexturesFolderName = "textures";
	}
}
