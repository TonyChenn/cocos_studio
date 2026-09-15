using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Ide;

namespace CocoStudio.Projects
{
	[DataInclude(typeof(MaterialFolder))]
	[DataInclude(typeof(PlistImageFolder))]
	[DataItem(Name = "Folder")]
	public class ResourceFolder : ResourceItem, IFoldeItem, ICocosFile, IInitialize, ICustomDataItem
	{
		[ResourcePathItemProperty("Name")]
		public FilePath BaseDirectory { get; protected set; }

		public ResourceItemCollection Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new ResourceItemCollection(this);
				}
				return this.items;
			}
		}

		public override string Name
		{
			get
			{
				return this.BaseDirectory.FileName;
			}
			protected set
			{
				if (this.BaseDirectory.FileName != value)
				{
					FilePath baseDirectory = this.BaseDirectory.ParentDirectory.Combine(new string[]
					{
						value
					});
					this.BaseDirectory = baseDirectory;
				}
			}
		}

		public override string FullPath
		{
			get
			{
				return this.BaseDirectory.FullPath;
			}
		}

		bool ICocosFile.IsLoaded
		{
			get
			{
				return false;
			}
		}

		bool IInitialize.IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

		public ResourceFolder()
		{
		}

		public ResourceFolder(FilePath directoryPath) : this()
		{
			if (!directoryPath.IsAbsolute)
			{
				throw new ArgumentException("Must be absolute path.");
			}
			this.BaseDirectory = directoryPath;
		}

		protected override DataError OnCheckDataError()
		{
			if (!Directory.Exists(this.FullPath))
			{
				return new DataError(string.Format(LanguageInfo.DataError6_DirNotExist, this.FullPath));
			}
			return null;
		}

		public override ResourceData GetResourceData()
		{
			return this.CreateResourceData(this.BaseDirectory);
		}

		protected override void OnSetLocation(FilePath newFilePath, bool isRename = true)
		{
			if (!Directory.Exists(newFilePath))
			{
				Directory.CreateDirectory(newFilePath);
			}
			foreach (ResourceItem resourceItem in this.items)
			{
				string name = newFilePath.Combine(new string[]
				{
					resourceItem.Name
				});
				resourceItem.SetLocation(name, isRename);
			}
			if (Directory.Exists(this.BaseDirectory) && !isRename)
			{
				this.BaseDirectory.Delete();
			}
			this.BaseDirectory = newFilePath;
			base.OnSetLocation(newFilePath, isRename);
		}

		protected override void OnMove(FilePath newMovePath)
		{
			try
			{
				if (!Directory.Exists(newMovePath))
				{
					Directory.CreateDirectory(newMovePath);
				}
				foreach (ResourceItem resourceItem in this.items)
				{
					string name = newMovePath.Combine(new string[]
					{
						resourceItem.Name
					});
					resourceItem.Move(name);
				}
				if (Directory.Exists(this.BaseDirectory))
				{
					this.BaseDirectory.Delete();
				}
				this.BaseDirectory = newMovePath;
				base.OnMove(newMovePath);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("folder move failure", exception);
			}
		}

		public DataCollection Serialize(ITypeSerializer handler)
		{
			DataCollection dataCollection = handler.Serialize(this);
			string baseFile = handler.SerializationContext.BaseFile;
			handler.SerializationContext.BaseFile = this.BaseDirectory.Combine(new string[]
			{
				"TempAppendFileName.Folder"
			});
			foreach (ResourceItem resourceItem in this.Items)
			{
				UnknowResourceItem unknowResourceItem = resourceItem as UnknowResourceItem;
				if (unknowResourceItem != null)
				{
					dataCollection.Add(unknowResourceItem.DataItem);
				}
				else
				{
					DataNode entry = handler.SerializationContext.Serializer.Serialize(resourceItem);
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
			handler.SerializationContext.BaseFile = this.BaseDirectory.Combine(new string[]
			{
				"TempAppendFileName.Folder"
			});
			foreach (object obj in data)
			{
				DataNode dataNode = (DataNode)obj;
				DataItem dataItem = dataNode as DataItem;
				if (dataItem != null)
				{
					DataType configurationDataType = handler.SerializationContext.Serializer.DataContext.GetConfigurationDataType(dataNode.Name);
					ResourceItem item;
					if (configurationDataType != null)
					{
						item = (configurationDataType.Deserialize(handler.SerializationContext, null, dataItem) as ResourceItem);
					}
					else
					{
						item = new UnknowResourceItem(this.BaseDirectory, dataItem);
					}
					this.Items.Add(item);
				}
			}
			base.ExtendedProperties.Clear();
			handler.SerializationContext.BaseFile = baseFile;
		}

		protected override void OnDelete(IProgressMonitor monitor)
		{
			try
			{
				foreach (ResourceItem resourceItem in this.items)
				{
					resourceItem.Remove(monitor);
				}
				DesktopService.PlatformService.DeleteToTrash(this.BaseDirectory);
				base.OnDelete(monitor);
			}
			catch (IOException ex)
			{
				LogConfig.Output.Error(LanguageInfo.MessageBox259_ProcessCannotAccess, ex);
				monitor.ReportError(ex.Message, ex);
			}
			catch (Exception ex2)
			{
				LogConfig.Output.Error(LanguageInfo.MessageBox259_ProcessCannotAccess, ex2);
				monitor.ReportError(ex2.Message, ex2);
			}
		}

		protected override void OnRemove(IProgressMonitor monitor)
		{
			try
			{
				foreach (ResourceItem resourceItem in this.items)
				{
					resourceItem.Remove(monitor);
				}
				base.OnRemove(monitor);
			}
			catch (Exception ex)
			{
				LogConfig.Output.Error(LanguageInfo.MessageBox259_ProcessCannotAccess, ex);
				monitor.ReportError(ex.Message, ex);
			}
		}

		protected internal override bool IsNeedRefresh()
		{
			return true;
		}

		void IInitialize.Initialize(IProgressMonitor monitor)
		{
			this.OnInitialize(monitor);
		}

		void ICocosFile.Load(IProgressMonitor monitor)
		{
			foreach (ResourceItem resourceItem in this.Items)
			{
				ICocosFile cocosFile = resourceItem as ICocosFile;
				if (cocosFile != null)
				{
					cocosFile.Load(monitor);
				}
			}
		}

		void ICocosFile.Save(IProgressMonitor monitor)
		{
			foreach (ResourceItem resourceItem in this.Items)
			{
				ICocosFile cocosFile = resourceItem as ICocosFile;
				if (cocosFile != null)
				{
					cocosFile.Save(monitor);
				}
			}
		}

		void ICocosFile.UnLoad(IProgressMonitor monitor)
		{
			foreach (ResourceItem resourceItem in this.Items)
			{
				ICocosFile cocosFile = resourceItem as ICocosFile;
				if (cocosFile != null)
				{
					cocosFile.UnLoad(monitor);
				}
			}
		}

		protected virtual void OnInitialize(IProgressMonitor monitor)
		{
			foreach (ResourceItem resourceItem in this.Items)
			{
				ICocosFile cocosFile = resourceItem as ICocosFile;
				if (cocosFile != null && cocosFile.IsAutoInitialize)
				{
					cocosFile.Initialize(monitor);
				}
			}
		}

		HashSet<ResourceData> ICocosFile.GetUsedResources(IProgressMonitor monitor)
		{
			return this.GetUsedResources(monitor, false);
		}

		private HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor, bool isSearchReferenceProject)
		{
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			foreach (ResourceItem resourceItem in this.Items)
			{
				try
				{
					ICocosFile cocosFile = resourceItem as ICocosFile;
					if (cocosFile != null)
					{
						HashSet<ResourceData> usedResources;
						if (cocosFile is CocosItem)
						{
							usedResources = ((CocosItem)cocosFile).GetUsedResources(monitor, isSearchReferenceProject);
						}
						else
						{
							usedResources = cocosFile.GetUsedResources(monitor);
						}
						if (usedResources != null)
						{
							hashSet.UnionWith(usedResources);
						}
					}
				}
				catch (Exception ex)
				{
					string message = string.Format("Resource {0} publish failed, the error is {1}", resourceItem.RelativePath, ex.Message);
					monitor.ReportError(message, ex);
					LogConfig.Output.Error(message, ex);
				}
			}
			return hashSet;
		}

		bool ICocosFile.UpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourcesCollection)
		{
			foreach (ResourceItem resourceItem in this.Items)
			{
				ICocosFile cocosFile = resourceItem as ICocosFile;
				if (cocosFile != null)
				{
					cocosFile.UpdateUsedResources(monitor, changedResourcesCollection);
				}
			}
			return false;
		}

		protected override void OnRefresh()
		{
			foreach (ResourceItem resourceItem in this.Items)
			{
				resourceItem.Refresh();
			}
			base.OnRefresh();
		}

		public const string tempAppendFileName = "TempAppendFileName.Folder";

		private ResourceItemCollection items;
	}
}
