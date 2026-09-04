using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using CocoStudio.Projects.ExtensionModel;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Ide;

namespace CocoStudio.Projects
{
	// Token: 0x0200005E RID: 94
	[DataInclude(typeof(CocosFile))]
	[DataItem("Project")]
	public class CocosItem : ResourceFile, ICocosFile, IInitialize, ICocosItem, IPublish, ICustomDataItem
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000A5D8 File Offset: 0x000087D8
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x0000A606 File Offset: 0x00008806
		[ItemProperty("Type")]
		public string ContentType
		{
			get
			{
				if (string.IsNullOrEmpty(this.contentType) && this.CocosFile != null)
				{
					this.contentType = this.CocosFile.Type;
				}
				return this.contentType;
			}
			set
			{
				this.contentType = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000A60F File Offset: 0x0000880F
		public CodeFileCollection SourceFiles
		{
			get
			{
				if (this.sourceFiles == null)
				{
					this.sourceFiles = new CodeFileCollection(this);
				}
				return this.sourceFiles;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000A62B File Offset: 0x0000882B
		public CocosFile CocosFile
		{
			get
			{
				if (!this.IsInitialized)
				{
					this.Initialize(ProjectsService.Instance.DefaultMonitor);
				}
				return this.cocosFile;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000A64C File Offset: 0x0000884C
		public UserData UserData
		{
			get
			{
				if (this._userData == null)
				{
					string filePath = UserData.GetFilePath(this.FileName);
					this._userData = UserData.Load(filePath);
					if (this._userData == null)
					{
						this._userData = new UserData(filePath);
					}
				}
				return this._userData;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000A693 File Offset: 0x00008893
		public virtual bool IsAutoInitialize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000A696 File Offset: 0x00008896
		// (set) Token: 0x060002AB RID: 683 RVA: 0x0000A69E File Offset: 0x0000889E
		public bool IsInitialized { get; private set; }

		// Token: 0x060002AC RID: 684 RVA: 0x0000A6A7 File Offset: 0x000088A7
		protected CocosItem()
		{
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000A6AF File Offset: 0x000088AF
		public CocosItem(FilePath file) : base(file)
		{
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000A6B8 File Offset: 0x000088B8
		public CocosItem(FilePath file, CocosFile cocosFile) : this(file)
		{
			this.cocosFile = cocosFile;
			this.cocosFile.CocosItem = this;
			this.contentType = cocosFile.Type;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000A6E0 File Offset: 0x000088E0
		protected bool CheckInitialize(IProgressMonitor monitor)
		{
			if (this.cocosFile == null)
			{
				this.Initialize(monitor);
			}
			return this.cocosFile != null;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000A6FD File Offset: 0x000088FD
		public bool IsLoaded
		{
			get
			{
				return this.cocosFile != null && this.cocosFile.IsLoaded;
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000A714 File Offset: 0x00008914
		public void ReloadReferencedItem(IProgressMonitor monitor)
		{
			if (this.cocosFile != null)
			{
				this.cocosFile.ReloadReferencedItem(monitor);
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000A72A File Offset: 0x0000892A
		public bool HasReferencedItem(CocosItem item)
		{
			return this == item || (this.cocosFile != null && this.cocosFile.HasReferencedItem(item));
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000A748 File Offset: 0x00008948
		public void Load(IProgressMonitor monitor)
		{
			this.CheckInitialize(monitor);
			if (this.cocosFile != null)
			{
				this.cocosFile.Load(monitor);
				if (!monitor.AsyncOperation.Success)
				{
					NullProgressMonitor nullProgressMonitor = monitor as NullProgressMonitor;
					string message = string.Empty;
					if (nullProgressMonitor != null)
					{
						message = nullProgressMonitor.Errors[0].Message;
					}
					this.dataError = new DataError(message);
				}
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000A7A8 File Offset: 0x000089A8
		public bool UpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourceCollection)
		{
			if (!this.CheckInitialize(monitor))
			{
				return false;
			}
			if (this.cocosFile != null)
			{
				bool flag = this.cocosFile.UpdateUsedResources(monitor, changedResourceCollection);
				if (flag)
				{
					this.RefreshFileInfo();
				}
				return flag;
			}
			return false;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000A7E2 File Offset: 0x000089E2
		public void Save(IProgressMonitor monitor)
		{
			if (!this.CheckInitialize(monitor))
			{
				return;
			}
			this.cocosFile.Save(monitor);
			this.SaveUserData();
			this.RefreshFileInfo();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000A806 File Offset: 0x00008A06
		private void RefreshFileInfo()
		{
			this.lastWriteTime = ResourceItem.GetLastWriteTime(this.FullPath);
			this.fileSize = ResourceItem.GetFileSize(this.FullPath);
			this.dataError = null;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000A838 File Offset: 0x00008A38
		public void Initialize(IProgressMonitor monitor)
		{
			if (!File.Exists(this.FileName))
			{
				return;
			}
			this.dataError = null;
			CocosFile cocosFile = null;
			if (!this.IsInitialized)
			{
				try
				{
					this.lastWriteTime = ResourceItem.GetLastWriteTime(this.FileName);
					this.fileSize = ResourceItem.GetFileSize(this.FileName);
					cocosFile = CocosItem.ReadFile(monitor, this.FileName);
					cocosFile.CocosItem = this;
					cocosFile.Initialize(monitor);
				}
				catch (Exception exception)
				{
					string message = string.Format("CocosFile initialize failed. File is {0}.", this.FileName);
					monitor.ReportError(message, exception);
					this.dataError = new DataError(LanguageInfo.DataError5_CsdAnalysisFailed);
				}
				this.IsInitialized = true;
			}
			if (cocosFile != null)
			{
				this.cocosFile = cocosFile;
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000A908 File Offset: 0x00008B08
		public void UnLoad(IProgressMonitor monitor)
		{
			if (this.cocosFile != null)
			{
				this.cocosFile.UnLoad(monitor);
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000A91E File Offset: 0x00008B1E
		public void Reload(IProgressMonitor monitor)
		{
			this.IsInitialized = false;
			this.Initialize(monitor);
			this.Load(monitor);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000A935 File Offset: 0x00008B35
		private static CocosFile ReadFile(IProgressMonitor monitor, string filePath)
		{
			FileUpgraderManager.Upgrade(filePath);
			return ProjectsService.Instance.InternalReadCocosFile(monitor, filePath);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000A94A File Offset: 0x00008B4A
		public HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor)
		{
			return this.GetUsedResources(monitor, false);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000A954 File Offset: 0x00008B54
		protected override DataError OnCheckDataError()
		{
			this.dataError = base.OnCheckDataError();
			if (this.dataError != null)
			{
				return this.dataError;
			}
			try
			{
				CocosItem.ReadFile(ProjectsService.Instance.DefaultMonitor, this.FileName);
			}
			catch (Exception exception)
			{
				string message = string.Format("Project initialize failed. File is {0}.", this.FileName);
				this.dataError = new DataError(LanguageInfo.DataError5_CsdAnalysisFailed);
				LogConfig.Logger.Debug(message, exception);
			}
			return this.dataError;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000A9E4 File Offset: 0x00008BE4
		public HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor, bool isSearchReferenceProjects)
		{
			this.CheckInitialize(monitor);
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			if (!isSearchReferenceProjects)
			{
				HashSet<ResourceData> usedResources = this.cocosFile.GetUsedResources(monitor);
				if (usedResources != null)
				{
					hashSet.UnionWith(usedResources);
				}
			}
			else
			{
				HashSet<CocosItem> searchedItem = new HashSet<CocosItem>();
				CocosItem.ScanFile(monitor, this, hashSet, searchedItem);
			}
			return hashSet;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000AA2C File Offset: 0x00008C2C
		private static void ScanFile(IProgressMonitor monitor, CocosItem referencedItem, HashSet<ResourceData> resourceItems, HashSet<CocosItem> searchedItem)
		{
			HashSet<ResourceData> usedResources = referencedItem.CocosFile.GetUsedResources(monitor);
			HashSet<CocosItem> hashSet = new HashSet<CocosItem>();
			foreach (ResourceData resourceData in usedResources)
			{
				string text = ProjectsService.Instance.ToAbsolute(resourceData);
				if (ProjectsService.Instance.IsCocosFile(text))
				{
					CocosItem cocosItem = ProjectsService.Instance.CurrentResourceGroup.FindResourceItem(text) as CocosItem;
					if (cocosItem != null)
					{
						hashSet.Add(cocosItem);
					}
				}
			}
			resourceItems.UnionWith(usedResources);
			foreach (CocosItem cocosItem2 in hashSet)
			{
				searchedItem.Add(cocosItem2);
				CocosItem.ScanFile(monitor, cocosItem2, resourceItems, searchedItem);
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000AB18 File Offset: 0x00008D18
		protected override void OnRefresh()
		{
			if (this.IsLoaded)
			{
				if (File.Exists(this.FileName))
				{
					this.UnLoad(ProjectsService.Instance.DefaultMonitor);
					this.Reload(ProjectsService.Instance.DefaultMonitor);
				}
			}
			else
			{
				this.IsInitialized = false;
				this.Initialize(ProjectsService.Instance.DefaultMonitor);
			}
			base.OnRefresh();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000AB7E File Offset: 0x00008D7E
		protected override void OnDelete(IProgressMonitor monitor)
		{
			this.DeleteUserData();
			base.OnDelete(monitor);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000AB90 File Offset: 0x00008D90
		public void Publish(IProgressMonitor monitor, PublishInfo info)
		{
			try
			{
				this.OnPublish(monitor, info);
			}
			catch (Exception ex)
			{
				string message = string.Format("Project {0} publish failed, the error is {1}", this.FullPath, ex.Message);
				monitor.ReportError(message, ex);
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000ABDC File Offset: 0x00008DDC
		protected virtual void OnPublish(IProgressMonitor monitor, PublishInfo info)
		{
			if (this.CheckInitialize(monitor))
			{
				if (!this.CheckDestinationDirectory(monitor, info))
				{
					return;
				}
				info.SourceFilePath = this.FileName;
				if (this.FileName.HasExtension(".csd"))
				{
					string text = ProjectsService.Instance.SerializeGameFile(info, this.CocosFile as GameFile);
					if (!string.IsNullOrWhiteSpace(text))
					{
						monitor.ReportError(text, null);
					}
				}
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000AC4C File Offset: 0x00008E4C
		protected bool CheckDestinationDirectory(IProgressMonitor monitor, PublishInfo info)
		{
			FilePath filePath = this.FileName.ToRelative(ProjectsService.Instance.CurrentSolution.ItemDirectory).ToAbsolute(info.PublishDirectory);
			info.DestinationFilePath = filePath;
			string text = filePath.ParentDirectory;
			if (!Directory.Exists(text))
			{
				try
				{
					Directory.CreateDirectory(text);
				}
				catch (Exception exception)
				{
					string message = string.Format("Create directory {0} failed.", text);
					monitor.ReportError(message, exception);
					return false;
				}
				return true;
			}
			return true;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000ACE8 File Offset: 0x00008EE8
		protected override void OnSetLocation(FilePath newFilePath, bool isRename = true)
		{
			this.RenameUserData(newFilePath);
			if (this.CheckInitialize(ProjectsService.Instance.DefaultMonitor))
			{
				this.cocosFile.SetLocation(newFilePath);
			}
			base.OnSetLocation(newFilePath, isRename);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000AD17 File Offset: 0x00008F17
		protected override void OnMove(FilePath newMovePath)
		{
			this.MoveUserData(newMovePath);
			if (this.CheckInitialize(ProjectsService.Instance.DefaultMonitor))
			{
				this.cocosFile.SetLocation(newMovePath);
			}
			base.OnMove(newMovePath);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000AD45 File Offset: 0x00008F45
		public void SaveUserData()
		{
			if (this.UserData.Properties.Count > 0)
			{
				this.UserData.Save();
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000AD68 File Offset: 0x00008F68
		private void RenameUserData(FilePath newFilePath)
		{
			string filePath = this.UserData.FilePath;
			if (File.Exists(filePath))
			{
				string text = newFilePath.ChangeExtension(".udf");
				FileService.RenameFile(filePath, text);
				this.UserData.ChangeFilePath(text);
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000ADB0 File Offset: 0x00008FB0
		private void MoveUserData(FilePath newMovePath)
		{
			string filePath = this.UserData.FilePath;
			if (File.Exists(filePath))
			{
				string text = newMovePath.ChangeExtension(".udf");
				FileService.MoveFile(filePath, text);
				this.UserData.ChangeFilePath(text);
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000ADF8 File Offset: 0x00008FF8
		private void DeleteUserData()
		{
			try
			{
				string filePath = this.UserData.FilePath;
				if (File.Exists(filePath))
				{
					DesktopService.PlatformService.DeleteToTrash(filePath);
				}
			}
			catch (Exception arg)
			{
				LogConfig.Logger.Error("删除参考线失败: " + arg);
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000AE50 File Offset: 0x00009050
		public DataCollection Serialize(ITypeSerializer handler)
		{
			DataCollection dataCollection = handler.Serialize(this);
			string baseFile = handler.SerializationContext.BaseFile;
			foreach (CodeFile codeFile in this.SourceFiles)
			{
				UnknowCodeFile unknowCodeFile = codeFile as UnknowCodeFile;
				if (unknowCodeFile != null)
				{
					dataCollection.Add(unknowCodeFile.DataItem);
				}
				else
				{
					DataNode entry = handler.SerializationContext.Serializer.Serialize(codeFile);
					dataCollection.Add(entry);
				}
			}
			handler.SerializationContext.BaseFile = baseFile;
			return dataCollection;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000AEF0 File Offset: 0x000090F0
		public void Deserialize(ITypeSerializer handler, DataCollection data)
		{
			string baseFile = handler.SerializationContext.BaseFile;
			handler.Deserialize(this, data);
			handler.SerializationContext.BaseFile = this.fileName.ParentDirectory.Combine(new string[]
			{
				"TempAppendFileName.Project"
			});
			foreach (object obj in data)
			{
				DataNode dataNode = (DataNode)obj;
				DataItem dataItem = dataNode as DataItem;
				if (dataItem != null)
				{
					DataType configurationDataType = handler.SerializationContext.Serializer.DataContext.GetConfigurationDataType(dataNode.Name);
					CodeFile codeFile;
					if (configurationDataType != null)
					{
						codeFile = (configurationDataType.Deserialize(handler.SerializationContext, null, dataItem) as CodeFile);
					}
					else
					{
						codeFile = new UnknowCodeFile(this.FileName.ParentDirectory, dataItem);
					}
					if (codeFile != null)
					{
						this.SourceFiles.Add(codeFile);
					}
				}
			}
			base.ExtendedProperties.Clear();
			handler.SerializationContext.BaseFile = baseFile;
		}

		// Token: 0x040000A6 RID: 166
		public const string tempAppendFileName = "TempAppendFileName.Project";

		// Token: 0x040000A7 RID: 167
		public const string FileSuffix = ".csd";

		// Token: 0x040000A8 RID: 168
		private string contentType;

		// Token: 0x040000A9 RID: 169
		private CodeFileCollection sourceFiles;

		// Token: 0x040000AA RID: 170
		private CocosFile cocosFile;

		// Token: 0x040000AB RID: 171
		private UserData _userData;
	}
}
