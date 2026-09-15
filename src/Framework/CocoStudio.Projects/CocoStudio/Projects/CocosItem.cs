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
	[DataInclude(typeof(CocosFile))]
	[DataItem("Project")]
	public class CocosItem : ResourceFile, ICocosFile, IInitialize, ICocosItem, IPublish, ICustomDataItem
	{
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

		public virtual bool IsAutoInitialize
		{
			get
			{
				return false;
			}
		}

		public bool IsInitialized { get; private set; }

		protected CocosItem()
		{
		}

		public CocosItem(FilePath file) : base(file)
		{
		}

		public CocosItem(FilePath file, CocosFile cocosFile) : this(file)
		{
			this.cocosFile = cocosFile;
			this.cocosFile.CocosItem = this;
			this.contentType = cocosFile.Type;
		}

		protected bool CheckInitialize(IProgressMonitor monitor)
		{
			if (this.cocosFile == null)
			{
				this.Initialize(monitor);
			}
			return this.cocosFile != null;
		}

		public bool IsLoaded
		{
			get
			{
				return this.cocosFile != null && this.cocosFile.IsLoaded;
			}
		}

		public void ReloadReferencedItem(IProgressMonitor monitor)
		{
			if (this.cocosFile != null)
			{
				this.cocosFile.ReloadReferencedItem(monitor);
			}
		}

		public bool HasReferencedItem(CocosItem item)
		{
			return this == item || (this.cocosFile != null && this.cocosFile.HasReferencedItem(item));
		}

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

		private void RefreshFileInfo()
		{
			this.lastWriteTime = ResourceItem.GetLastWriteTime(this.FullPath);
			this.fileSize = ResourceItem.GetFileSize(this.FullPath);
			this.dataError = null;
		}

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

		public void UnLoad(IProgressMonitor monitor)
		{
			if (this.cocosFile != null)
			{
				this.cocosFile.UnLoad(monitor);
			}
		}

		public void Reload(IProgressMonitor monitor)
		{
			this.IsInitialized = false;
			this.Initialize(monitor);
			this.Load(monitor);
		}

		private static CocosFile ReadFile(IProgressMonitor monitor, string filePath)
		{
			FileUpgraderManager.Upgrade(filePath);
			return ProjectsService.Instance.InternalReadCocosFile(monitor, filePath);
		}

		public HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor)
		{
			return this.GetUsedResources(monitor, false);
		}

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

		protected override void OnDelete(IProgressMonitor monitor)
		{
			this.DeleteUserData();
			base.OnDelete(monitor);
		}

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

		protected override void OnSetLocation(FilePath newFilePath, bool isRename = true)
		{
			this.RenameUserData(newFilePath);
			if (this.CheckInitialize(ProjectsService.Instance.DefaultMonitor))
			{
				this.cocosFile.SetLocation(newFilePath);
			}
			base.OnSetLocation(newFilePath, isRename);
		}

		protected override void OnMove(FilePath newMovePath)
		{
			this.MoveUserData(newMovePath);
			if (this.CheckInitialize(ProjectsService.Instance.DefaultMonitor))
			{
				this.cocosFile.SetLocation(newMovePath);
			}
			base.OnMove(newMovePath);
		}

		public void SaveUserData()
		{
			if (this.UserData.Properties.Count > 0)
			{
				this.UserData.Save();
			}
		}

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

		public const string tempAppendFileName = "TempAppendFileName.Project";

		public const string FileSuffix = ".csd";

		private string contentType;

		private CodeFileCollection sourceFiles;

		private CocosFile cocosFile;

		private UserData _userData;
	}
}
