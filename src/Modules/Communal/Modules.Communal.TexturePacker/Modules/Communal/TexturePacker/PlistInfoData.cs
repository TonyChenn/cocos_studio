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
	// Token: 0x02000008 RID: 8
	[DataModelExtension]
	[DataInclude(typeof(SizeValue))]
	[DataInclude(typeof(ExportType))]
	[DataInclude(typeof(SortAlgorithm))]
	public class PlistInfoData : BaseObjectData, ICocosFileContent, ICocosFile, IInitialize, ICocosItem
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002902 File Offset: 0x00000B02
		// (set) Token: 0x06000024 RID: 36 RVA: 0x0000290A File Offset: 0x00000B0A
		[ItemProperty]
		public List<FilePathData> ImageFiles { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002913 File Offset: 0x00000B13
		// (set) Token: 0x06000026 RID: 38 RVA: 0x0000291B File Offset: 0x00000B1B
		[ItemProperty]
		public ExportType ExportType { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002924 File Offset: 0x00000B24
		// (set) Token: 0x06000028 RID: 40 RVA: 0x0000292C File Offset: 0x00000B2C
		[ItemProperty]
		public SortAlgorithm SortAlgorithm { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002935 File Offset: 0x00000B35
		// (set) Token: 0x0600002A RID: 42 RVA: 0x0000293D File Offset: 0x00000B3D
		[ItemProperty]
		public SizeValue MaxSize { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002946 File Offset: 0x00000B46
		// (set) Token: 0x0600002C RID: 44 RVA: 0x0000294E File Offset: 0x00000B4E
		[ItemProperty]
		public int PicturePadding { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002957 File Offset: 0x00000B57
		// (set) Token: 0x0600002E RID: 46 RVA: 0x0000295F File Offset: 0x00000B5F
		[ItemProperty]
		public bool AllowRotation { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002968 File Offset: 0x00000B68
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002970 File Offset: 0x00000B70
		[ItemProperty]
		public bool AllowAnySize { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002979 File Offset: 0x00000B79
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002981 File Offset: 0x00000B81
		[ItemProperty]
		public bool AllowTrim { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000033 RID: 51 RVA: 0x0000298A File Offset: 0x00000B8A
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002992 File Offset: 0x00000B92
		public CocosFile CocosFile { get; set; }

		// Token: 0x06000035 RID: 53 RVA: 0x0000299B File Offset: 0x00000B9B
		public PlistInfoData()
		{
			this.PicturePadding = 2;
			this.AllowRotation = true;
			this.MaxSize = new SizeValue(1024, 1024);
			this.ImageFiles = new List<FilePathData>();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000029D4 File Offset: 0x00000BD4
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

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002A50 File Offset: 0x00000C50
		public bool IsLoaded
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002A53 File Offset: 0x00000C53
		public bool IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002A56 File Offset: 0x00000C56
		public void Load(IProgressMonitor monitor)
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002A58 File Offset: 0x00000C58
		public void Save(IProgressMonitor monitor)
		{
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002A5A File Offset: 0x00000C5A
		public void UnLoad(IProgressMonitor monitor)
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002A5C File Offset: 0x00000C5C
		public HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor)
		{
			return null;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002A5F File Offset: 0x00000C5F
		public bool UpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourceCollection)
		{
			return true;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002A62 File Offset: 0x00000C62
		public void Initialize(IProgressMonitor monitor)
		{
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002A64 File Offset: 0x00000C64
		public void ReloadReferencedItem(IProgressMonitor monitor)
		{
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002A66 File Offset: 0x00000C66
		public void Publish(IProgressMonitor monitor, PublishInfo info)
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002A68 File Offset: 0x00000C68
		public bool HasReferencedItem(CocosItem cocosItem)
		{
			return false;
		}
	}
}
