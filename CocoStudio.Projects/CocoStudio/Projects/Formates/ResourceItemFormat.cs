using System;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000036 RID: 54
	[Extension(typeof(IFileFormat))]
	internal class ResourceItemFormat : FileFormat
	{
		// Token: 0x06000140 RID: 320 RVA: 0x00005E17 File Offset: 0x00004017
		protected override bool OnCanWriteFile(object obj)
		{
			return false;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005E1A File Offset: 0x0000401A
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return false;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005E1D File Offset: 0x0000401D
		protected override void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
			throw new InvalidOperationException("Simple resource file can't write. Only cocos file can write.");
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005E29 File Offset: 0x00004029
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
