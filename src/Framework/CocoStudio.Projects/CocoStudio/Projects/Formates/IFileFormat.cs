using System;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[TypeExtensionPoint]
	public interface IFileFormat
	{
		bool CanReadFile(FilePath file, Type expectedObjectType);

		bool CanWriteFile(object obj);

		void WriteFile(FilePath file, object obj, IProgressMonitor monitor);

		object ReadFile(FilePath file, Type expectedType, IProgressMonitor monitor);
	}
}
