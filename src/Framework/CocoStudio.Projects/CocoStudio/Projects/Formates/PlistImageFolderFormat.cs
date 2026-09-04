using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Model;
using Modules.Communal.Packer.PlistReader;
using Modules.Communal.PList;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x0200002D RID: 45
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(ICompositeResourceProcesser))]
	[Extension(typeof(IPublishProcesser))]
	internal class PlistImageFolderFormat : CompositeFormat, IPublishProcesser
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x00004D67 File Offset: 0x00002F67
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is PlistImageFolder;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004D74 File Offset: 0x00002F74
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			try
			{
				if (!FileFormat.CheckFileSuffix(file, new string[]
				{
					".plist"
				}))
				{
					return false;
				}
				if (!expectedObjectType.Equals(typeof(ResourceItem)))
				{
					return false;
				}
				return PlistImageFormatFactory.CreatePlistFormat(file) != null;
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004DE0 File Offset: 0x00002FE0
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new PlistImageFolder(file);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004DE8 File Offset: 0x00002FE8
		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			if (resourceData.Type == EnumResourceType.MarkedSubImage || resourceData.Type == EnumResourceType.PlistSubImage || resourceData.Type == EnumResourceType.Addin)
			{
				return false;
			}
			FilePath fullPath = ProjectsService.Instance.GetFullPath(resourceData);
			return base.CanReadFile(fullPath, typeof(ResourceItem));
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004E30 File Offset: 0x00003030
		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			PlistImageFolder plistImageFolder = ProjectsService.Instance.CurrentResourceGroup.FindResourceItem(resourceData) as PlistImageFolder;
			if (plistImageFolder == null)
			{
				throw new Exception("Can't find file " + resourceData.Plist + " in current solution.");
			}
			List<string> imageFiles = new List<string>
			{
				plistImageFolder.PreviewImagePath
			};
			return CompositeResourceHelp.GetResourcesIncludeImage(resourceData, imageFiles);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004E8C File Offset: 0x0000308C
		public override bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004EA4 File Offset: 0x000030A4
		public override List<string> GetFiles(string filePath)
		{
			PListRoot plistRoot = PListRoot.Load(filePath);
			PListDict plistDict = plistRoot.Root as PListDict;
			PlistImageFormat plistImageFormat = PlistImageFormatFactory.CreatePlistFormat(plistDict);
			if (plistImageFormat == null)
			{
				return null;
			}
			string imageFileName = plistImageFormat.GetImageFileName(plistDict);
			if (imageFileName == null)
			{
				return null;
			}
			string item = Path.Combine(Path.GetDirectoryName(filePath), imageFileName);
			return new List<string>
			{
				item
			};
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004F04 File Offset: 0x00003104
		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".plist"
			};
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004F24 File Offset: 0x00003124
		public override List<string> GetFilterTypes()
		{
			return new List<string>
			{
				"_PList.Dir"
			};
		}
	}
}
