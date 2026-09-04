using System;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000010 RID: 16
	[TypeExtensionPoint]
	public interface IFileFormat
	{
		// Token: 0x0600003E RID: 62
		bool CanReadFile(FilePath file, Type expectedObjectType);

		// Token: 0x0600003F RID: 63
		bool CanWriteFile(object obj);

		// Token: 0x06000040 RID: 64
		void WriteFile(FilePath file, object obj, IProgressMonitor monitor);

		// Token: 0x06000041 RID: 65
		object ReadFile(FilePath file, Type expectedType, IProgressMonitor monitor);
	}
}
