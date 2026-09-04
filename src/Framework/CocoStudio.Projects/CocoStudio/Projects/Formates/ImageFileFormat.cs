using System;
using System.Collections.Generic;
using CocoStudio.Model;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000033 RID: 51
	[Extension(typeof(IPublishProcesser))]
	[Extension(typeof(IFileFormat))]
	internal class ImageFileFormat : FileFormat, IPublishProcesser
	{
		// Token: 0x0600012A RID: 298 RVA: 0x0000597C File Offset: 0x00003B7C
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is ImageFile;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005988 File Offset: 0x00003B88
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

		// Token: 0x0600012C RID: 300 RVA: 0x000059F0 File Offset: 0x00003BF0
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new ImageFile(file);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000059F8 File Offset: 0x00003BF8
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

		// Token: 0x0600012E RID: 302 RVA: 0x00005A4B File Offset: 0x00003C4B
		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			return null;
		}
	}
}
