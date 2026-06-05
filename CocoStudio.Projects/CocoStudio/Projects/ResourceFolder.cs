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
	// Token: 0x02000048 RID: 72
	[DataInclude(typeof(MaterialFolder))]
	[DataInclude(typeof(PlistImageFolder))]
	[DataItem(Name = "Folder")]
	public class ResourceFolder : ResourceItem, IFoldeItem, ICocosFile, IInitialize, ICustomDataItem
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00007548 File Offset: 0x00005748
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00007550 File Offset: 0x00005750
		[ResourcePathItemProperty("Name")]
		public FilePath BaseDirectory { get; protected set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00007559 File Offset: 0x00005759
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00007578 File Offset: 0x00005778
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x00007594 File Offset: 0x00005794
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x000075E4 File Offset: 0x000057E4
		public override string FullPath
		{
			get
			{
				return this.BaseDirectory.FullPath;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00007604 File Offset: 0x00005804
		bool ICocosFile.IsLoaded
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00007607 File Offset: 0x00005807
		bool IInitialize.IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000760A File Offset: 0x0000580A
		public ResourceFolder()
		{
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00007612 File Offset: 0x00005812
		public ResourceFolder(FilePath directoryPath) : this()
		{
			if (!directoryPath.IsAbsolute)
			{
				throw new ArgumentException("Must be absolute path.");
			}
			this.BaseDirectory = directoryPath;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00007635 File Offset: 0x00005835
		protected override DataError OnCheckDataError()
		{
			if (!Directory.Exists(this.FullPath))
			{
				return new DataError(string.Format(LanguageInfo.DataError6_DirNotExist, this.FullPath));
			}
			return null;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000765B File Offset: 0x0000585B
		public override ResourceData GetResourceData()
		{
			return this.CreateResourceData(this.BaseDirectory);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000766C File Offset: 0x0000586C
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

		// Token: 0x060001F0 RID: 496 RVA: 0x0000772C File Offset: 0x0000592C
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

		// Token: 0x060001F1 RID: 497 RVA: 0x00007808 File Offset: 0x00005A08
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

		// Token: 0x060001F2 RID: 498 RVA: 0x000078DC File Offset: 0x00005ADC
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

		// Token: 0x060001F3 RID: 499 RVA: 0x000079EC File Offset: 0x00005BEC
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

		// Token: 0x060001F4 RID: 500 RVA: 0x00007AB0 File Offset: 0x00005CB0
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

		// Token: 0x060001F5 RID: 501 RVA: 0x00007B34 File Offset: 0x00005D34
		protected internal override bool IsNeedRefresh()
		{
			return true;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00007B37 File Offset: 0x00005D37
		void IInitialize.Initialize(IProgressMonitor monitor)
		{
			this.OnInitialize(monitor);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00007B40 File Offset: 0x00005D40
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

		// Token: 0x060001F8 RID: 504 RVA: 0x00007B98 File Offset: 0x00005D98
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

		// Token: 0x060001F9 RID: 505 RVA: 0x00007BF0 File Offset: 0x00005DF0
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

		// Token: 0x060001FA RID: 506 RVA: 0x00007C48 File Offset: 0x00005E48
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

		// Token: 0x060001FB RID: 507 RVA: 0x00007CA8 File Offset: 0x00005EA8
		HashSet<ResourceData> ICocosFile.GetUsedResources(IProgressMonitor monitor)
		{
			return this.GetUsedResources(monitor, false);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00007CB4 File Offset: 0x00005EB4
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

		// Token: 0x060001FD RID: 509 RVA: 0x00007D84 File Offset: 0x00005F84
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

		// Token: 0x060001FE RID: 510 RVA: 0x00007DE0 File Offset: 0x00005FE0
		protected override void OnRefresh()
		{
			foreach (ResourceItem resourceItem in this.Items)
			{
				resourceItem.Refresh();
			}
			base.OnRefresh();
		}

		// Token: 0x0400007C RID: 124
		public const string tempAppendFileName = "TempAppendFileName.Folder";

		// Token: 0x0400007D RID: 125
		private ResourceItemCollection items;
	}
}
