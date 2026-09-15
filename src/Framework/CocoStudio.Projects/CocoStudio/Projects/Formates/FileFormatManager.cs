using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Addins;

namespace CocoStudio.Projects.Formates
{
	internal class FileFormatManager
	{
		internal List<IFileFormat> FileFormats
		{
			get
			{
				return this.fileFormats;
			}
		}

		public FileFormatManager()
		{
			this.CollectFileFormat();
			this.defaultResourceFormat = (this.fileFormats.FirstOrDefault((IFileFormat a) => a is ResourceItemFormat) as ResourceItemFormat);
		}

		private void CollectFileFormat()
		{
			IFileFormat[] extensionObjects = AddinManager.GetExtensionObjects<IFileFormat>();
			this.fileFormats.AddRange(extensionObjects);
			AddinManager.AddExtensionNodeHandler(typeof(IFileFormat), new ExtensionNodeEventHandler(this.OnFileFormatExtensionChanged));
		}

		private void OnFileFormatExtensionChanged(object sender, ExtensionNodeEventArgs args)
		{
			TypeExtensionNode extension = args.ExtensionNode as TypeExtensionNode;
			if (!this.fileFormats.Any((IFileFormat a) => a.GetType().Equals(extension.Type)))
			{
				this.fileFormats.Add(Activator.CreateInstance(extension.Type) as IFileFormat);
			}
		}

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

		internal FileFormat GetDefaultResourceFormat()
		{
			return this.defaultResourceFormat;
		}

		private List<IFileFormat> fileFormats = new List<IFileFormat>();

		private ResourceItemFormat defaultResourceFormat;
	}
}
