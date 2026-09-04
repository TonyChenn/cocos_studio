using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Addins;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000031 RID: 49
	internal class FileFormatManager
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000121 RID: 289 RVA: 0x0000576B File Offset: 0x0000396B
		internal List<IFileFormat> FileFormats
		{
			get
			{
				return this.fileFormats;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005780 File Offset: 0x00003980
		public FileFormatManager()
		{
			this.CollectFileFormat();
			this.defaultResourceFormat = (this.fileFormats.FirstOrDefault((IFileFormat a) => a is ResourceItemFormat) as ResourceItemFormat);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000057D8 File Offset: 0x000039D8
		private void CollectFileFormat()
		{
			IFileFormat[] extensionObjects = AddinManager.GetExtensionObjects<IFileFormat>();
			this.fileFormats.AddRange(extensionObjects);
			AddinManager.AddExtensionNodeHandler(typeof(IFileFormat), new ExtensionNodeEventHandler(this.OnFileFormatExtensionChanged));
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005834 File Offset: 0x00003A34
		private void OnFileFormatExtensionChanged(object sender, ExtensionNodeEventArgs args)
		{
			TypeExtensionNode extension = args.ExtensionNode as TypeExtensionNode;
			if (!this.fileFormats.Any((IFileFormat a) => a.GetType().Equals(extension.Type)))
			{
				this.fileFormats.Add(Activator.CreateInstance(extension.Type) as IFileFormat);
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005894 File Offset: 0x00003A94
		public List<FileFormat> GetFileFormats(string fileName, Type expectedType)
		{
			List<FileFormat> list = new List<FileFormat>();
			foreach (IFileFormat fileFormat in this.fileFormats)
			{
				FileFormat fileFormat2 = (FileFormat)fileFormat;
				if (fileFormat2.CanReadFile(fileName, expectedType))
				{
					list.Add(fileFormat2);
				}
			}
			return list;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005904 File Offset: 0x00003B04
		internal List<FileFormat> GetFileFormatsForObject(object obj)
		{
			List<FileFormat> list = new List<FileFormat>();
			foreach (IFileFormat fileFormat in this.fileFormats)
			{
				FileFormat fileFormat2 = (FileFormat)fileFormat;
				if (fileFormat2.CanWriteFile(obj))
				{
					list.Add(fileFormat2);
				}
			}
			return list;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000596C File Offset: 0x00003B6C
		internal FileFormat GetDefaultResourceFormat()
		{
			return this.defaultResourceFormat;
		}

		// Token: 0x04000049 RID: 73
		private List<IFileFormat> fileFormats = new List<IFileFormat>();

		// Token: 0x0400004A RID: 74
		private ResourceItemFormat defaultResourceFormat;
	}
}
