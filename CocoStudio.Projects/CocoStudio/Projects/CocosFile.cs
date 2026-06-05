using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using CocoStudio.Projects.ExtensionModel;
using CocoStudio.Projects.Formates;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Projects
{
	// Token: 0x0200006A RID: 106
	[DataInclude(typeof(GameFile))]
	[JsonObject(MemberSerialization.OptIn)]
	public abstract class CocosFile : ICocosFile, IInitialize, ICocosItem, IExtendedDataItem, IFileItem
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000B5A2 File Offset: 0x000097A2
		public IDictionary ExtendedProperties
		{
			get
			{
				if (this.hashTable == null)
				{
					this.hashTable = new Hashtable();
				}
				return this.hashTable;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0000B5BD File Offset: 0x000097BD
		// (set) Token: 0x06000312 RID: 786 RVA: 0x0000B5C5 File Offset: 0x000097C5
		public CocosItem CocosItem { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000B5CE File Offset: 0x000097CE
		internal virtual FileFormat FileFormat
		{
			get
			{
				if (this.format == null)
				{
					this.format = ProjectsService.Instance.GetDefaultFormat(this);
				}
				return this.format;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000B5EF File Offset: 0x000097EF
		// (set) Token: 0x06000315 RID: 789 RVA: 0x0000B5F7 File Offset: 0x000097F7
		public FilePath FileName { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0000B600 File Offset: 0x00009800
		// (set) Token: 0x06000317 RID: 791 RVA: 0x0000B60D File Offset: 0x0000980D
		public Guid ID
		{
			get
			{
				return new Guid(this.id);
			}
			internal set
			{
				this.id = value.ToString();
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000B622 File Offset: 0x00009822
		// (set) Token: 0x06000319 RID: 793 RVA: 0x0000B62A File Offset: 0x0000982A
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000B634 File Offset: 0x00009834
		// (set) Token: 0x0600031B RID: 795 RVA: 0x0000B64F File Offset: 0x0000984F
		[JsonProperty(PropertyName = "Name")]
		[ItemProperty("PropertyGroup/Name")]
		public string Name
		{
			get
			{
				return this.FileName.FileNameWithoutExtension;
			}
			private set
			{
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000B651 File Offset: 0x00009851
		// (set) Token: 0x0600031D RID: 797 RVA: 0x0000B659 File Offset: 0x00009859
		[ItemProperty]
		[JsonProperty]
		public ICocosFileContent Content { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0000B662 File Offset: 0x00009862
		// (set) Token: 0x0600031F RID: 799 RVA: 0x0000B66A File Offset: 0x0000986A
		public bool IsLoaded { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000B673 File Offset: 0x00009873
		// (set) Token: 0x06000321 RID: 801 RVA: 0x0000B67B File Offset: 0x0000987B
		[ItemProperty("PropertyGroup/Type")]
		[JsonProperty(PropertyName = "Type")]
		public virtual string Type { get; set; }

		// Token: 0x06000322 RID: 802 RVA: 0x0000B684 File Offset: 0x00009884
		protected CocosFile()
		{
			this.id = Guid.NewGuid().ToString();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000B6C0 File Offset: 0x000098C0
		public CocosFile(FilePath file) : this()
		{
			this.FileName = file;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000B6CF File Offset: 0x000098CF
		public CocosFile(CocosItemCreateInfo info) : this(info.FileName)
		{
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000B6DD File Offset: 0x000098DD
		bool IInitialize.IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000B6E0 File Offset: 0x000098E0
		public void Initialize(IProgressMonitor monitor)
		{
			try
			{
				this.OnInitialize(monitor);
			}
			catch (Exception exception)
			{
				monitor.ReportError("Project item initialize failed.", exception);
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000B718 File Offset: 0x00009918
		protected virtual void OnInitialize(IProgressMonitor monitor)
		{
			if (this.Content != null)
			{
				FileUpgraderManager.Upgrade(this);
				this.Content.CocosFile = this;
				this.Content.Initialize(monitor);
				this.UpgradeFileVersion(monitor);
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000B748 File Offset: 0x00009948
		private void UpgradeFileVersion(IProgressMonitor monitor)
		{
			Version version = new Version(this.Version);
			if (version.CompareTo(Option.EditorVersion) < 0)
			{
				this.Version = Option.EditorVersion.ToString();
				this.WriteFile(monitor);
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000B788 File Offset: 0x00009988
		public void Save(IProgressMonitor monitor)
		{
			try
			{
				this.OnSave(monitor);
			}
			catch (Exception exception)
			{
				monitor.ReportError("Save failed,", exception);
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000B7C0 File Offset: 0x000099C0
		protected virtual void OnSave(IProgressMonitor monitor)
		{
			if (this.Content != null)
			{
				this.Content.Save(monitor);
			}
			this.WriteFile(monitor);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000B7E0 File Offset: 0x000099E0
		public void WriteFile(IProgressMonitor monitor)
		{
			string directoryName = Path.GetDirectoryName(this.FileName);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			ProjectsService.Instance.InternalWriteCocosFile(monitor, this.FileName, this);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000B820 File Offset: 0x00009A20
		public void Load(IProgressMonitor monitor)
		{
			if (this.IsLoaded)
			{
				return;
			}
			try
			{
				this.IsLoaded = true;
				this.OnLoad(monitor);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error(LanguageInfo.DataError11_FileCorrupted, exception);
				monitor.ReportError(LanguageInfo.DataError11_FileCorrupted, exception);
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000B878 File Offset: 0x00009A78
		protected virtual void OnLoad(IProgressMonitor monitor)
		{
			if (this.Content != null)
			{
				this.Content.Load(monitor);
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000B88E File Offset: 0x00009A8E
		public void UnLoad(IProgressMonitor monitor)
		{
			this.IsLoaded = false;
			this.OnUnLoad(monitor);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000B89E File Offset: 0x00009A9E
		protected virtual void OnUnLoad(IProgressMonitor monitor)
		{
			if (this.Content != null)
			{
				this.Content.UnLoad(monitor);
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000B8B4 File Offset: 0x00009AB4
		public HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor)
		{
			return this.OnGetUsedResources(monitor);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000B8BD File Offset: 0x00009ABD
		protected virtual HashSet<ResourceData> OnGetUsedResources(IProgressMonitor monitor)
		{
			if (this.Content != null)
			{
				return this.Content.GetUsedResources(monitor);
			}
			return null;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000B8D8 File Offset: 0x00009AD8
		public bool UpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourcesCollection)
		{
			bool flag = this.OnUpdateUsedResources(monitor, changedResourcesCollection);
			if (flag)
			{
				this.WriteFile(monitor);
			}
			return flag;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000B8F9 File Offset: 0x00009AF9
		protected virtual bool OnUpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourcesCollection)
		{
			return this.Content != null && this.Content.UpdateUsedResources(monitor, changedResourcesCollection);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000B912 File Offset: 0x00009B12
		public void ReloadReferencedItem(IProgressMonitor monitor)
		{
			this.OnReloadReferencedItem(monitor);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000B91B File Offset: 0x00009B1B
		protected virtual void OnReloadReferencedItem(IProgressMonitor monitor)
		{
			if (this.Content != null)
			{
				this.Content.ReloadReferencedItem(monitor);
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000B931 File Offset: 0x00009B31
		public bool HasReferencedItem(CocosItem item)
		{
			return this.OnHasReferencedItem(item);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000B93A File Offset: 0x00009B3A
		protected virtual bool OnHasReferencedItem(CocosItem item)
		{
			return this.Content != null && this.Content.HasReferencedItem(item);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000B952 File Offset: 0x00009B52
		internal void SetLocation(FilePath newFilePath)
		{
			this.OnSetLocation(newFilePath);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000B95B File Offset: 0x00009B5B
		protected virtual void OnSetLocation(FilePath newFilePath)
		{
			this.FileName = newFilePath;
		}

		// Token: 0x040000BA RID: 186
		private Hashtable hashTable;

		// Token: 0x040000BB RID: 187
		private FileFormat format;

		// Token: 0x040000BC RID: 188
		[ItemProperty("PropertyGroup/ID")]
		[JsonProperty(PropertyName = "ID")]
		private string id;

		// Token: 0x040000BD RID: 189
		[ItemProperty("PropertyGroup/Version")]
		[JsonProperty(PropertyName = "Version")]
		private string version = Option.EditorVersion.ToString();
	}
}
