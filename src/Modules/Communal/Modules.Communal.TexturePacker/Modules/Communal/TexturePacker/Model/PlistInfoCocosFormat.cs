using System;
using CocoStudio.Projects;
using CocoStudio.Projects.Formates;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.TexturePacker.Model
{
	[Extension(typeof(IFileFormat))]
	internal class PlistInfoCocosFormat : FileFormat
	{
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return FileFormat.CheckFileSuffix(file, new string[]
			{
				".csi"
			}) && (expectedObjectType.Equals(typeof(CocosItem)) || expectedObjectType.Equals(typeof(ResourceItem)));
		}

		protected override bool OnCanWriteFile(object obj)
		{
			return obj.GetType().IsSubclassOf(typeof(PlistInfoCocosItem));
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new PlistInfoCocosItem(file);
		}

		protected override void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
		}
	}
}
