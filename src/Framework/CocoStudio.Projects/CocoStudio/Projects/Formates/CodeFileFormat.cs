using System;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x0200001A RID: 26
	[Extension(typeof(IFileFormat))]
	internal class CodeFileFormat : FileFormat
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00003776 File Offset: 0x00001976
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is CodeFile;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003784 File Offset: 0x00001984
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return FileFormat.CheckFileSuffix(file, new string[]
			{
				".lua",
				".js"
			}) && (expectedObjectType.Equals(typeof(CodeFile)) || expectedObjectType.Equals(typeof(ResourceItem)));
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000037DC File Offset: 0x000019DC
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			if (FileFormat.CheckFileSuffix(file, new string[]
			{
				".lua"
			}))
			{
				return new LuaFile(file);
			}
			return null;
		}
	}
}
