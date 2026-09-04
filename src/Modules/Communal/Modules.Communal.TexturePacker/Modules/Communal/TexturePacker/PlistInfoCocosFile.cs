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
	// Token: 0x0200000C RID: 12
	[DataItem("PlistInfoProjectFile")]
	public class PlistInfoCocosFile : CocosFile
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00004724 File Offset: 0x00002924
		// (set) Token: 0x0600008E RID: 142 RVA: 0x0000472C File Offset: 0x0000292C
		public PlistInfoModel PlistInfoModel { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00004735 File Offset: 0x00002935
		public PlistInfoCocosItem PlistInfoCocosItem
		{
			get
			{
				return base.CocosItem as PlistInfoCocosItem;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004742 File Offset: 0x00002942
		public PlistInfoData PlistInfoData
		{
			get
			{
				return base.Content as PlistInfoData;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000474F File Offset: 0x0000294F
		protected PlistInfoCocosFile()
		{
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004757 File Offset: 0x00002957
		public PlistInfoCocosFile(FilePath file) : base(file)
		{
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004760 File Offset: 0x00002960
		public PlistInfoCocosFile(CocosItemCreateInfo info) : base(info)
		{
			this.Type = info.ContentType;
			this.PlistInfoModel = new PlistInfoModel(true);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004784 File Offset: 0x00002984
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

		// Token: 0x06000095 RID: 149 RVA: 0x00004800 File Offset: 0x00002A00
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

		// Token: 0x06000096 RID: 150 RVA: 0x000048F8 File Offset: 0x00002AF8
		protected override void OnLoad(IProgressMonitor monitor)
		{
			base.OnLoad(monitor);
			PlistInfoData plistInfoData = this.PlistInfoData;
			this.PlistInfoModel = new PlistInfoModel(this.PlistInfoData);
			this.PlistInfoModel.CocosItem = this.PlistInfoCocosItem;
			this.PlistInfoModel.CalculateItemPosition();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004935 File Offset: 0x00002B35
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

		// Token: 0x06000098 RID: 152 RVA: 0x0000495E File Offset: 0x00002B5E
		protected override HashSet<ResourceData> OnGetUsedResources(IProgressMonitor monitor)
		{
			return null;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000049C4 File Offset: 0x00002BC4
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
