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
	[DataInclude(typeof(GameFile))]
	[JsonObject(MemberSerialization.OptIn)]
	public abstract class CocosFile : ICocosFile, IInitialize, ICocosItem, IExtendedDataItem, IFileItem
	{
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

		public CocosItem CocosItem { get; set; }

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

		public FilePath FileName { get; set; }

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

		[ItemProperty]
		[JsonProperty]
		public ICocosFileContent Content { get; set; }

		public bool IsLoaded { get; private set; }

		[ItemProperty("PropertyGroup/Type")]
		[JsonProperty(PropertyName = "Type")]
		public virtual string Type { get; set; }

		protected CocosFile()
		{
			this.id = Guid.NewGuid().ToString();
		}

		public CocosFile(FilePath file) : this()
		{
			this.FileName = file;
		}

		public CocosFile(CocosItemCreateInfo info) : this(info.FileName)
		{
		}

		bool IInitialize.IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

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

		private void UpgradeFileVersion(IProgressMonitor monitor)
		{
			Version version = new Version(this.Version);
			if (version.CompareTo(Option.EditorVersion) < 0)
			{
				this.Version = Option.EditorVersion.ToString();
				this.WriteFile(monitor);
			}
		}

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

		protected virtual void OnSave(IProgressMonitor monitor)
		{
			if (this.Content != null)
			{
				this.Content.Save(monitor);
			}
			this.WriteFile(monitor);
		}

		public void WriteFile(IProgressMonitor monitor)
		{
			string directoryName = Path.GetDirectoryName(this.FileName);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			ProjectsService.Instance.InternalWriteCocosFile(monitor, this.FileName, this);
		}

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

		protected virtual void OnLoad(IProgressMonitor monitor)
		{
			if (this.Content != null)
			{
				this.Content.Load(monitor);
			}
		}

		public void UnLoad(IProgressMonitor monitor)
		{
			this.IsLoaded = false;
			this.OnUnLoad(monitor);
		}

		protected virtual void OnUnLoad(IProgressMonitor monitor)
		{
			if (this.Content != null)
			{
				this.Content.UnLoad(monitor);
			}
		}

		public HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor)
		{
			return this.OnGetUsedResources(monitor);
		}

		protected virtual HashSet<ResourceData> OnGetUsedResources(IProgressMonitor monitor)
		{
			if (this.Content != null)
			{
				return this.Content.GetUsedResources(monitor);
			}
			return null;
		}

		public bool UpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourcesCollection)
		{
			bool flag = this.OnUpdateUsedResources(monitor, changedResourcesCollection);
			if (flag)
			{
				this.WriteFile(monitor);
			}
			return flag;
		}

		protected virtual bool OnUpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourcesCollection)
		{
			return this.Content != null && this.Content.UpdateUsedResources(monitor, changedResourcesCollection);
		}

		public void ReloadReferencedItem(IProgressMonitor monitor)
		{
			this.OnReloadReferencedItem(monitor);
		}

		protected virtual void OnReloadReferencedItem(IProgressMonitor monitor)
		{
			if (this.Content != null)
			{
				this.Content.ReloadReferencedItem(monitor);
			}
		}

		public bool HasReferencedItem(CocosItem item)
		{
			return this.OnHasReferencedItem(item);
		}

		protected virtual bool OnHasReferencedItem(CocosItem item)
		{
			return this.Content != null && this.Content.HasReferencedItem(item);
		}

		internal void SetLocation(FilePath newFilePath)
		{
			this.OnSetLocation(newFilePath);
		}

		protected virtual void OnSetLocation(FilePath newFilePath)
		{
			this.FileName = newFilePath;
		}

		private Hashtable hashTable;

		private FileFormat format;

		[ItemProperty("PropertyGroup/ID")]
		[JsonProperty(PropertyName = "ID")]
		private string id;

		[ItemProperty("PropertyGroup/Version")]
		[JsonProperty(PropertyName = "Version")]
		private string version = Option.EditorVersion.ToString();
	}
}
