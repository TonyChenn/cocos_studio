using System;
using System.Collections.Generic;
using CocoStudio.Model;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IPublishProcesser))]
	[Extension(typeof(IFileFormat))]
	internal class ImageFileFormat : FileFormat, IPublishProcesser
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is ImageFile;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return FileFormat.CheckFileSuffix(file, new string[]
			{
				".png",
				".jpg",
				".jpeg ",
				".pvr"
			}) && (expectedObjectType.Equals(typeof(ImageFile)) || expectedObjectType.Equals(typeof(ResourceItem)));
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new ImageFile(file);
		}

		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			if (resourceData.Type == EnumResourceType.MarkedSubImage)
			{
				return true;
			}
			if (resourceData.Type == EnumResourceType.Normal && ProjectsService.Instance.IsImageFile(resourceData))
			{
				ImageFile imageFile = ProjectsService.Instance.CurrentResourceGroup.FindResourceItem(resourceData) as ImageFile;
				return imageFile != null && imageFile.IsPacked();
			}
			return false;
		}

		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			return null;
		}
	}
}
