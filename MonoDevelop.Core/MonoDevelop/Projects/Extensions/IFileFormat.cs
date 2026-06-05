using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x02000194 RID: 404
	public interface IFileFormat
	{
		// Token: 0x06000F8F RID: 3983
		FilePath GetValidFormatName(object obj, FilePath fileName);

		// Token: 0x06000F90 RID: 3984
		bool CanReadFile(FilePath file, Type expectedObjectType);

		// Token: 0x06000F91 RID: 3985
		bool CanWriteFile(object obj);

		// Token: 0x06000F92 RID: 3986
		void ConvertToFormat(object obj);

		// Token: 0x06000F93 RID: 3987
		void WriteFile(FilePath file, object obj, IProgressMonitor monitor);

		// Token: 0x06000F94 RID: 3988
		object ReadFile(FilePath file, Type expectedType, IProgressMonitor monitor);

		// Token: 0x06000F95 RID: 3989
		List<FilePath> GetItemFiles(object obj);

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000F96 RID: 3990
		bool SupportsMixedFormats { get; }

		// Token: 0x06000F97 RID: 3991
		IEnumerable<string> GetCompatibilityWarnings(object obj);

		// Token: 0x06000F98 RID: 3992
		bool SupportsFramework(TargetFramework framework);
	}
}
