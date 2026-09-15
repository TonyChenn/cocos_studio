using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocoStudio.Model;
using CocoStudio.Projects;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.TexturePacker
{
	[DataItem("PlistInfoProjectFile")]
	public class PlistInfoCocosFile : CocosFile
	{
		public PlistInfoModel PlistInfoModel { get; private set; }

		public PlistInfoCocosItem PlistInfoCocosItem
		{
			get
			{
				return base.CocosItem as PlistInfoCocosItem;
			}
		}

		public PlistInfoData PlistInfoData
		{
			get
			{
				return base.Content as PlistInfoData;
			}
		}

		protected PlistInfoCocosFile()
		{
		}

		public PlistInfoCocosFile(FilePath file) : base(file)
		{
		}

		public PlistInfoCocosFile(CocosItemCreateInfo info) : base(info)
		{
			this.Type = info.ContentType;
			this.PlistInfoModel = new PlistInfoModel(true);
		}

		protected override void OnInitialize(IProgressMonitor monitor)
		{
			base.OnInitialize(monitor);
			PlistInfoData plistInfoData = this.PlistInfoData;
			plistInfoData.Initialize();
			foreach (FilePathData filePathData in this.PlistInfoData.ImageFiles)
			{
				(filePathData.File as ImageFile).InitializePack(base.CocosItem);
			}
		}

		protected override void OnSave(IProgressMonitor monitor)
		{
			if (this.PlistInfoModel != null)
			{
				PlistInfoData plistInfoData = this.PlistInfoData;
				plistInfoData.AllowAnySize = this.PlistInfoModel.AllowAnySize;
				plistInfoData.AllowRotation = this.PlistInfoModel.AllowRotation;
				plistInfoData.AllowTrim = this.PlistInfoModel.AllowTrim;
				plistInfoData.ExportType = this.PlistInfoModel.ExportType;
				plistInfoData.MaxSize = this.PlistInfoModel.MaxSize;
				plistInfoData.SortAlgorithm = this.PlistInfoModel.SortAlgorithm;
				plistInfoData.PicturePadding = this.PlistInfoModel.PicturePadding;
				plistInfoData.ImageFiles.Clear();
				foreach (ImageFile file in this.PlistInfoModel.ImageFiles)
				{
					plistInfoData.ImageFiles.Add(file);
				}
			}
			base.OnSave(monitor);
		}

		protected override void OnLoad(IProgressMonitor monitor)
		{
			base.OnLoad(monitor);
			PlistInfoData plistInfoData = this.PlistInfoData;
			this.PlistInfoModel = new PlistInfoModel(this.PlistInfoData);
			this.PlistInfoModel.CocosItem = this.PlistInfoCocosItem;
			this.PlistInfoModel.CalculateItemPosition();
		}

		protected override void OnUnLoad(IProgressMonitor monitor)
		{
			if (this.PlistInfoModel == null)
			{
				return;
			}
			this.PlistInfoModel.Dispose();
			this.PlistInfoModel.CocosItem = null;
			this.PlistInfoModel = null;
		}

		protected override HashSet<ResourceData> OnGetUsedResources(IProgressMonitor monitor)
		{
			return null;
		}

		protected override bool OnUpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourcesCollection)
		{
			if (this.PlistInfoData == null)
			{
				return false;
			}
			if (!File.Exists(base.FileName.FullPath))
			{
				return false;
			}
			bool result = false;
			using (Dictionary<ResourceData, ResourceFile>.Enumerator enumerator = changedResourcesCollection.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ResourceData, ResourceFile> item = enumerator.Current;
					FilePathData filePathData = this.PlistInfoData.ImageFiles.FirstOrDefault(delegate(FilePathData a)
					{
						KeyValuePair<ResourceData, ResourceFile> item2 = item;
						if (item2.Value == null)
						{
							string relativePath = a.File.RelativePath;
							KeyValuePair<ResourceData, ResourceFile> item3 = item;
							return relativePath == item3.Key.Path;
						}
						ResourceFile file = a.File;
						KeyValuePair<ResourceData, ResourceFile> item4 = item;
						return file == item4.Value;
					});
					if (filePathData != null)
					{
						KeyValuePair<ResourceData, ResourceFile> item5 = item;
						if (item5.Value == null)
						{
							this.PlistInfoData.ImageFiles.Remove(filePathData);
						}
						result = true;
					}
				}
			}
			return result;
		}
	}
}
