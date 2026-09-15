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
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(ICompositeResourceProcesser))]
	[Extension(typeof(IPublishProcesser))]
	internal class PlistImageFolderFormat : CompositeFormat, IPublishProcesser
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is PlistImageFolder;
		}

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

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new PlistImageFolder(file);
		}

		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			if (resourceData.Type == EnumResourceType.MarkedSubImage || resourceData.Type == EnumResourceType.PlistSubImage || resourceData.Type == EnumResourceType.Addin)
			{
				return false;
			}
			FilePath fullPath = ProjectsService.Instance.GetFullPath(resourceData);
			return base.CanReadFile(fullPath, typeof(ResourceItem));
		}

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

		public override bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

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

		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".plist"
			};
		}

		public override List<string> GetFilterTypes()
		{
			return new List<string>
			{
				"_PList.Dir"
			};
		}
	}
}
