using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.TexturePacker
{
	[DataModelExtension]
	[DataInclude(typeof(SizeValue))]
	[DataInclude(typeof(ExportType))]
	[DataInclude(typeof(SortAlgorithm))]
	public class PlistInfoData : BaseObjectData, ICocosFileContent, ICocosFile, IInitialize, ICocosItem
	{
		[ItemProperty]
		public List<FilePathData> ImageFiles { get; set; }

		[ItemProperty]
		public ExportType ExportType { get; set; }

		[ItemProperty]
		public SortAlgorithm SortAlgorithm { get; set; }

		[ItemProperty]
		public SizeValue MaxSize { get; set; }

		[ItemProperty]
		public int PicturePadding { get; set; }

		[ItemProperty]
		public bool AllowRotation { get; set; }

		[ItemProperty]
		public bool AllowAnySize { get; set; }

		[ItemProperty]
		public bool AllowTrim { get; set; }

		public CocosFile CocosFile { get; set; }

		public PlistInfoData()
		{
			this.PicturePadding = 2;
			this.AllowRotation = true;
			this.MaxSize = new SizeValue(1024, 1024);
			this.ImageFiles = new List<FilePathData>();
		}

		public void Initialize()
		{
			List<FilePathData> list = new List<FilePathData>();
			foreach (FilePathData filePathData in this.ImageFiles)
			{
				if (filePathData == null)
				{
					LogConfig.Logger.Error("PlistInfoData.ImageFiles has a null item. This is should be a bug.");
				}
				else if (filePathData.File != null)
				{
					list.Add(filePathData);
				}
			}
			this.ImageFiles = list;
		}

		public bool IsLoaded
		{
			get
			{
				return false;
			}
		}

		public bool IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

		public void Load(IProgressMonitor monitor)
		{
		}

		public void Save(IProgressMonitor monitor)
		{
		}

		public void UnLoad(IProgressMonitor monitor)
		{
		}

		public HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor)
		{
			return null;
		}

		public bool UpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourceCollection)
		{
			return true;
		}

		public void Initialize(IProgressMonitor monitor)
		{
		}

		public void ReloadReferencedItem(IProgressMonitor monitor)
		{
		}

		public void Publish(IProgressMonitor monitor, PublishInfo info)
		{
		}

		public bool HasReferencedItem(CocosItem cocosItem)
		{
			return false;
		}
	}
}
