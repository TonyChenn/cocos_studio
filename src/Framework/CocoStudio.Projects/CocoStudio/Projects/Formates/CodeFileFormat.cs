using System;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IFileFormat))]
	internal class CodeFileFormat : FileFormat
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is CodeFile;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return FileFormat.CheckFileSuffix(file, new string[]
			{
				".lua",
				".js"
			}) && (expectedObjectType.Equals(typeof(CodeFile)) || expectedObjectType.Equals(typeof(ResourceItem)));
		}

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
