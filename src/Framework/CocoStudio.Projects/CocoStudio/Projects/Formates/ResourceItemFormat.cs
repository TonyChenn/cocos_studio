using System;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IFileFormat))]
	internal class ResourceItemFormat : FileFormat
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return false;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return false;
		}

		protected override void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
			throw new InvalidOperationException("Simple resource file can't write. Only cocos file can write.");
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			if (file.IsDirectory)
			{
				return new ResourceFolder(file);
			}
			return new ResourceFile(file);
		}
	}
}
