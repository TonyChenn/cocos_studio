using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Core.StringParsing;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects
{
	// Token: 0x0200015C RID: 348
	public abstract class WorkspaceItem : IBuildTarget, IWorkspaceFileObject, IWorkspaceObject, IExtendedDataItem, IFolderItem, IDisposable, IFileItem, ILoadController
	{
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0002EF7B File Offset: 0x0002D17B
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x0002EF83 File Offset: 0x0002D183
		public Workspace ParentWorkspace
		{
			get
			{
				return this.parentWorkspace;
			}
			internal set
			{
				this.parentWorkspace = value;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0002EF8C File Offset: 0x0002D18C
		public IDictionary ExtendedProperties
		{
			get
			{
				if (this.extendedProperties == null)
				{
					this.extendedProperties = new Hashtable();
				}
				return this.extendedProperties;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x0002EFA7 File Offset: 0x0002D1A7
		public virtual PropertyBag UserProperties
		{
			get
			{
				if (this.userProperties == null)
				{
					this.userProperties = new PropertyBag();
				}
				return this.userProperties;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0002EFC2 File Offset: 0x0002D1C2
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x0002EFE4 File Offset: 0x0002D1E4
		public virtual string Name
		{
			get
			{
				if (this.fileName.IsNullOrEmpty)
				{
					return string.Empty;
				}
				return this.fileName.FileNameWithoutExtension;
			}
			set
			{
				if (this.fileName.IsNullOrEmpty)
				{
					this.SetLocation(FilePath.Empty, value);
					return;
				}
				FilePath parentDirectory = this.fileName.ParentDirectory;
				string extension = this.fileName.Extension;
				this.FileName = parentDirectory.Combine(new string[]
				{
					value
				}) + extension;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0002F04C File Offset: 0x0002D24C
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x0002F054 File Offset: 0x0002D254
		public virtual FilePath FileName
		{
			get
			{
				return this.fileName;
			}
			set
			{
				string name = this.Name;
				this.fileName = value;
				if (this.FileFormat != null)
				{
					this.fileName = this.FileFormat.GetValidFileName(this, this.fileName);
				}
				if (name != this.Name)
				{
					this.OnNameChanged(new WorkspaceItemRenamedEventArgs(this, name, this.Name));
				}
				this.NotifyModified();
				if (this.Loading)
				{
					this.LoadUserProperties();
				}
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0002F0D0 File Offset: 0x0002D2D0
		public void SetLocation(FilePath baseDirectory, string name)
		{
			this.FileName = baseDirectory.Combine(new string[]
			{
				name
			}) + ".x";
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0002F10C File Offset: 0x0002D30C
		// (set) Token: 0x06000CCF RID: 3279 RVA: 0x0002F144 File Offset: 0x0002D344
		public FilePath BaseDirectory
		{
			get
			{
				if (this.baseDirectory.IsNull)
				{
					return this.FileName.ParentDirectory.FullPath;
				}
				return this.baseDirectory;
			}
			set
			{
				if (!value.IsNull && !this.FileName.IsNull && this.FileName.ParentDirectory.FullPath == value.FullPath)
				{
					this.baseDirectory = null;
				}
				else if (value.IsNullOrEmpty)
				{
					this.baseDirectory = null;
				}
				else
				{
					this.baseDirectory = value.FullPath;
				}
				this.NotifyModified();
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0002F1C8 File Offset: 0x0002D3C8
		public FilePath ItemDirectory
		{
			get
			{
				return this.FileName.ParentDirectory.FullPath;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0002F1EB File Offset: 0x0002D3EB
		protected bool Loading
		{
			get
			{
				return this.loading > 0;
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0002F1F6 File Offset: 0x0002D3F6
		public WorkspaceItem()
		{
			ProjectExtensionUtil.LoadControl(this);
			this.fileStatusTracker = new FileStatusTracker<WorkspaceItemEventArgs>(this, new Action<WorkspaceItemEventArgs>(this.OnReloadRequired), new WorkspaceItemEventArgs(this));
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0002F223 File Offset: 0x0002D423
		public T GetService<T>() where T : class
		{
			return (T)((object)this.GetService(typeof(T)));
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0002F23A File Offset: 0x0002D43A
		public virtual object GetService(Type t)
		{
			return Services.ProjectService.GetExtensionChain(this).GetService(this, t);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0002F250 File Offset: 0x0002D450
		public virtual List<FilePath> GetItemFiles(bool includeReferencedFiles)
		{
			List<FilePath> itemFiles = this.FileFormat.Format.GetItemFiles(this);
			if (!string.IsNullOrEmpty(this.FileName) && !itemFiles.Contains(this.FileName))
			{
				itemFiles.Add(this.FileName);
			}
			return itemFiles;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0002F29C File Offset: 0x0002D49C
		public virtual SolutionEntityItem FindSolutionItem(string fileName)
		{
			return null;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0002F29F File Offset: 0x0002D49F
		public virtual bool ContainsItem(IWorkspaceObject obj)
		{
			return this == obj;
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0002F2A5 File Offset: 0x0002D4A5
		public ReadOnlyCollection<SolutionItem> GetAllSolutionItems()
		{
			return this.GetAllSolutionItems<SolutionItem>();
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0002F2AD File Offset: 0x0002D4AD
		public virtual ReadOnlyCollection<T> GetAllSolutionItems<T>() where T : SolutionItem
		{
			return new List<T>().AsReadOnly();
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0002F2B9 File Offset: 0x0002D4B9
		public ReadOnlyCollection<Project> GetAllProjects()
		{
			return this.GetAllSolutionItems<Project>();
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0002F2C1 File Offset: 0x0002D4C1
		public virtual ReadOnlyCollection<Solution> GetAllSolutions()
		{
			return this.GetAllItems<Solution>();
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0002F2C9 File Offset: 0x0002D4C9
		public ReadOnlyCollection<WorkspaceItem> GetAllItems()
		{
			return this.GetAllItems<WorkspaceItem>();
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0002F2D4 File Offset: 0x0002D4D4
		public virtual ReadOnlyCollection<T> GetAllItems<T>() where T : WorkspaceItem
		{
			List<T> list = new List<T>();
			if (this is T)
			{
				list.Add((T)((object)this));
			}
			return list.AsReadOnly();
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0002F301 File Offset: 0x0002D501
		[Obsolete("Use GetProjectsContainingFile() (plural) instead")]
		public virtual Project GetProjectContainingFile(FilePath fileName)
		{
			return null;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0002F3A4 File Offset: 0x0002D5A4
		public virtual IEnumerable<Project> GetProjectsContainingFile(FilePath fileName)
		{
			yield break;
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0002F3C1 File Offset: 0x0002D5C1
		public virtual ReadOnlyCollection<string> GetConfigurations()
		{
			return new ReadOnlyCollection<string>(new string[0]);
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0002F3CE File Offset: 0x0002D5CE
		protected internal virtual void OnSave(IProgressMonitor monitor)
		{
			Services.ProjectService.InternalWriteWorkspaceItem(monitor, this.FileName, this);
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0002F3E2 File Offset: 0x0002D5E2
		internal void SetParentWorkspace(Workspace workspace)
		{
			this.parentWorkspace = workspace;
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0002F3EB File Offset: 0x0002D5EB
		public BuildResult RunTarget(IProgressMonitor monitor, string target, string configuration)
		{
			return this.RunTarget(monitor, target, (SolutionConfigurationSelector)configuration);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0002F3FB File Offset: 0x0002D5FB
		public BuildResult RunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
		{
			return Services.ProjectService.GetExtensionChain(this).RunTarget(monitor, this, target, configuration);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0002F411 File Offset: 0x0002D611
		public bool SupportsBuild()
		{
			return this.SupportsTarget("Build");
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0002F41E File Offset: 0x0002D61E
		public void Clean(IProgressMonitor monitor, string configuration)
		{
			this.Clean(monitor, (SolutionConfigurationSelector)configuration);
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0002F42D File Offset: 0x0002D62D
		public void Clean(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			Services.ProjectService.GetExtensionChain(this).RunTarget(monitor, this, "Clean", configuration);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0002F448 File Offset: 0x0002D648
		public bool SupportsTarget(string target)
		{
			return Services.ProjectService.GetExtensionChain(this).SupportsTarget(this, target);
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0002F45C File Offset: 0x0002D65C
		public bool SupportsExecute()
		{
			return Services.ProjectService.GetExtensionChain(this).SupportsExecute(this);
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0002F46F File Offset: 0x0002D66F
		public BuildResult Build(IProgressMonitor monitor, string configuration)
		{
			return this.InternalBuild(monitor, (SolutionConfigurationSelector)configuration);
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0002F47E File Offset: 0x0002D67E
		public BuildResult Build(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			return this.InternalBuild(monitor, configuration);
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0002F488 File Offset: 0x0002D688
		public void Execute(IProgressMonitor monitor, ExecutionContext context, string configuration)
		{
			this.Execute(monitor, context, (SolutionConfigurationSelector)configuration);
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0002F498 File Offset: 0x0002D698
		public void Execute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
			Services.ProjectService.GetExtensionChain(this).Execute(monitor, this, context, configuration);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0002F4AE File Offset: 0x0002D6AE
		public bool CanExecute(ExecutionContext context, string configuration)
		{
			return this.CanExecute(context, (SolutionConfigurationSelector)configuration);
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0002F4BD File Offset: 0x0002D6BD
		public bool CanExecute(ExecutionContext context, ConfigurationSelector configuration)
		{
			return Services.ProjectService.GetExtensionChain(this).CanExecute(this, context, configuration);
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0002F4D2 File Offset: 0x0002D6D2
		public IEnumerable<ExecutionTarget> GetExecutionTargets(string configuration)
		{
			return this.GetExecutionTargets((SolutionConfigurationSelector)configuration);
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0002F4E0 File Offset: 0x0002D6E0
		public IEnumerable<ExecutionTarget> GetExecutionTargets(ConfigurationSelector configuration)
		{
			return Services.ProjectService.GetExtensionChain(this).GetExecutionTargets(this, configuration);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0002F4F4 File Offset: 0x0002D6F4
		[Obsolete("This method will be removed in future releases")]
		public bool NeedsBuilding(string configuration)
		{
			return true;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0002F4F7 File Offset: 0x0002D6F7
		[Obsolete("This method will be removed in future releases")]
		public bool NeedsBuilding(ConfigurationSelector configuration)
		{
			return true;
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0002F4FA File Offset: 0x0002D6FA
		[Obsolete("This method will be removed in future releases")]
		public void SetNeedsBuilding(bool value)
		{
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0002F4FC File Offset: 0x0002D6FC
		[Obsolete("This method will be removed in future releases")]
		public void SetNeedsBuilding(bool needsBuilding, string configuration)
		{
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0002F4FE File Offset: 0x0002D6FE
		[Obsolete("This method will be removed in future releases")]
		public void SetNeedsBuilding(bool needsBuilding, ConfigurationSelector configuration)
		{
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0002F500 File Offset: 0x0002D700
		public virtual FileFormat FileFormat
		{
			get
			{
				if (this.format == null)
				{
					this.format = Services.ProjectService.GetDefaultFormat(this);
				}
				return this.format;
			}
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0002F521 File Offset: 0x0002D721
		public virtual bool SupportsFormat(FileFormat format)
		{
			return true;
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0002F524 File Offset: 0x0002D724
		public virtual void ConvertToFormat(FileFormat format, bool convertChildren)
		{
			this.FormatSet = true;
			this.format = format;
			if (!string.IsNullOrEmpty(this.FileName))
			{
				this.FileName = format.GetValidFileName(this, this.FileName);
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0002F563 File Offset: 0x0002D763
		internal virtual BuildResult InternalBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			return Services.ProjectService.GetExtensionChain(this).RunTarget(monitor, this, "Build", configuration);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0002F57D File Offset: 0x0002D77D
		protected virtual void OnConfigurationsChanged()
		{
			if (this.ConfigurationsChanged != null)
			{
				this.ConfigurationsChanged(this, EventArgs.Empty);
			}
			if (this.ParentWorkspace != null)
			{
				this.ParentWorkspace.OnConfigurationsChanged();
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0002F5AB File Offset: 0x0002D7AB
		public void Save(FilePath fileName, IProgressMonitor monitor)
		{
			this.FileName = fileName;
			this.Save(monitor);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0002F5BC File Offset: 0x0002D7BC
		public void Save(IProgressMonitor monitor)
		{
			try
			{
				this.fileStatusTracker.BeginSave();
				Services.ProjectService.GetExtensionChain(this).Save(monitor, this);
				this.SaveUserProperties();
				this.OnSaved(new WorkspaceItemEventArgs(this));
			}
			finally
			{
				this.fileStatusTracker.EndSave();
			}
			FileService.NotifyFileChanged(this.FileName);
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0002F624 File Offset: 0x0002D824
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x0002F631 File Offset: 0x0002D831
		public virtual bool NeedsReload
		{
			get
			{
				return this.fileStatusTracker.NeedsReload;
			}
			set
			{
				this.fileStatusTracker.NeedsReload = value;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0002F63F File Offset: 0x0002D83F
		public virtual bool ItemFilesChanged
		{
			get
			{
				return this.fileStatusTracker.ItemFilesChanged;
			}
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0002F64C File Offset: 0x0002D84C
		protected internal virtual BuildResult OnRunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
		{
			if (target == "Build")
			{
				return this.OnBuild(monitor, configuration);
			}
			if (target == "Clean")
			{
				this.OnClean(monitor, configuration);
				return null;
			}
			return null;
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0002F67C File Offset: 0x0002D87C
		protected internal virtual bool OnGetSupportsTarget(string target)
		{
			return true;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0002F67F File Offset: 0x0002D87F
		protected internal virtual bool OnGetSupportsExecute()
		{
			return true;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0002F682 File Offset: 0x0002D882
		protected virtual void OnClean(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0002F684 File Offset: 0x0002D884
		protected virtual BuildResult OnBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			return null;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0002F687 File Offset: 0x0002D887
		protected internal virtual void OnExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0002F689 File Offset: 0x0002D889
		protected internal virtual bool OnGetCanExecute(ExecutionContext context, ConfigurationSelector configuration)
		{
			return true;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0002F72C File Offset: 0x0002D92C
		protected internal virtual IEnumerable<ExecutionTarget> OnGetExecutionTargets(ConfigurationSelector configuration)
		{
			yield break;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0002F749 File Offset: 0x0002D949
		protected internal virtual bool OnGetNeedsBuilding(ConfigurationSelector configuration)
		{
			return true;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0002F74C File Offset: 0x0002D94C
		protected internal virtual void OnSetNeedsBuilding(bool val, ConfigurationSelector configuration)
		{
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0002F74E File Offset: 0x0002D94E
		void ILoadController.BeginLoad()
		{
			this.loading++;
			this.OnBeginLoad();
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0002F764 File Offset: 0x0002D964
		void ILoadController.EndLoad()
		{
			this.loading--;
			this.fileStatusTracker.ResetLoadTimes();
			this.OnEndLoad();
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0002F785 File Offset: 0x0002D985
		protected virtual void OnBeginLoad()
		{
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0002F787 File Offset: 0x0002D987
		protected virtual void OnEndLoad()
		{
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0002F78C File Offset: 0x0002D98C
		public virtual void LoadUserProperties()
		{
			if (this.userProperties != null)
			{
				this.userProperties.Dispose();
			}
			this.userProperties = null;
			string preferencesFileName = this.GetPreferencesFileName();
			if (!File.Exists(preferencesFileName))
			{
				return;
			}
			XmlTextReader xmlTextReader = new XmlTextReader(preferencesFileName);
			try
			{
				xmlTextReader.MoveToContent();
				if (!(xmlTextReader.LocalName != "Properties"))
				{
					this.userProperties = (PropertyBag)new XmlDataSerializer(new DataContext())
					{
						SerializationContext = 
						{
							BaseFile = preferencesFileName
						}
					}.Deserialize(xmlTextReader, typeof(PropertyBag));
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Exception while loading user solution preferences.", ex);
			}
			finally
			{
				xmlTextReader.Close();
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0002F850 File Offset: 0x0002DA50
		public virtual void SaveUserProperties()
		{
			string preferencesFileName = this.GetPreferencesFileName();
			if (this.userProperties == null || this.userProperties.IsEmpty)
			{
				if (File.Exists(preferencesFileName))
				{
					File.Delete(preferencesFileName);
				}
				return;
			}
			XmlTextWriter xmlTextWriter = null;
			try
			{
				xmlTextWriter = new XmlTextWriter(preferencesFileName, Encoding.UTF8);
				xmlTextWriter.Formatting = Formatting.Indented;
				new XmlDataSerializer(new DataContext())
				{
					SerializationContext = 
					{
						BaseFile = preferencesFileName
					}
				}.Serialize(xmlTextWriter, this.userProperties, typeof(PropertyBag));
			}
			catch (Exception ex)
			{
				LoggingService.LogWarning("Could not save solution preferences: " + this.GetPreferencesFileName(), ex);
			}
			finally
			{
				if (xmlTextWriter != null)
				{
					xmlTextWriter.Close();
				}
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0002F910 File Offset: 0x0002DB10
		private string GetPreferencesFileName()
		{
			return this.FileName.ChangeExtension(".userprefs");
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0002F938 File Offset: 0x0002DB38
		public virtual StringTagModelDescription GetStringTagModelDescription()
		{
			StringTagModelDescription stringTagModelDescription = new StringTagModelDescription();
			stringTagModelDescription.Add(base.GetType());
			return stringTagModelDescription;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0002F958 File Offset: 0x0002DB58
		public virtual StringTagModel GetStringTagModel()
		{
			StringTagModel stringTagModel = new StringTagModel();
			stringTagModel.Add(this);
			return stringTagModel;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0002F973 File Offset: 0x0002DB73
		public FilePath GetAbsoluteChildPath(FilePath relPath)
		{
			return relPath.ToAbsolute(this.BaseDirectory);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0002F982 File Offset: 0x0002DB82
		public FilePath GetRelativeChildPath(FilePath absPath)
		{
			return absPath.ToRelative(this.BaseDirectory);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0002F994 File Offset: 0x0002DB94
		public virtual void Dispose()
		{
			if (this.extendedProperties != null)
			{
				foreach (object obj in this.extendedProperties.Values)
				{
					IDisposable disposable = obj as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			if (this.userProperties != null)
			{
				this.userProperties.Dispose();
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0002FA14 File Offset: 0x0002DC14
		protected virtual void OnNameChanged(WorkspaceItemRenamedEventArgs e)
		{
			this.fileStatusTracker.ResetLoadTimes();
			this.NotifyModified();
			if (this.NameChanged != null)
			{
				this.NameChanged(this, e);
			}
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0002FA3C File Offset: 0x0002DC3C
		protected internal virtual object OnGetService(Type t)
		{
			return null;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0002FA3F File Offset: 0x0002DC3F
		protected void NotifyModified()
		{
			this.OnModified(new WorkspaceItemEventArgs(this));
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0002FA4D File Offset: 0x0002DC4D
		protected virtual void OnModified(WorkspaceItemEventArgs args)
		{
			if (this.Modified != null)
			{
				this.Modified(this, args);
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0002FA64 File Offset: 0x0002DC64
		protected virtual void OnSaved(WorkspaceItemEventArgs args)
		{
			if (this.Saved != null)
			{
				this.Saved(this, args);
			}
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0002FA7B File Offset: 0x0002DC7B
		protected virtual void OnReloadRequired(WorkspaceItemEventArgs args)
		{
			this.fileStatusTracker.FireReloadRequired(args);
		}

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06000D1D RID: 3357 RVA: 0x0002FA8C File Offset: 0x0002DC8C
		// (remove) Token: 0x06000D1E RID: 3358 RVA: 0x0002FAC4 File Offset: 0x0002DCC4
		public event EventHandler ConfigurationsChanged;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06000D1F RID: 3359 RVA: 0x0002FAFC File Offset: 0x0002DCFC
		// (remove) Token: 0x06000D20 RID: 3360 RVA: 0x0002FB34 File Offset: 0x0002DD34
		public event EventHandler<WorkspaceItemRenamedEventArgs> NameChanged;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06000D21 RID: 3361 RVA: 0x0002FB6C File Offset: 0x0002DD6C
		// (remove) Token: 0x06000D22 RID: 3362 RVA: 0x0002FBA4 File Offset: 0x0002DDA4
		public event EventHandler<WorkspaceItemEventArgs> Modified;

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06000D23 RID: 3363 RVA: 0x0002FBDC File Offset: 0x0002DDDC
		// (remove) Token: 0x06000D24 RID: 3364 RVA: 0x0002FC14 File Offset: 0x0002DE14
		public event EventHandler<WorkspaceItemEventArgs> Saved;

		// Token: 0x040003D2 RID: 978
		private Workspace parentWorkspace;

		// Token: 0x040003D3 RID: 979
		private FileFormat format;

		// Token: 0x040003D4 RID: 980
		internal bool FormatSet;

		// Token: 0x040003D5 RID: 981
		private Hashtable extendedProperties;

		// Token: 0x040003D6 RID: 982
		private FilePath fileName;

		// Token: 0x040003D7 RID: 983
		private int loading;

		// Token: 0x040003D8 RID: 984
		private PropertyBag userProperties;

		// Token: 0x040003D9 RID: 985
		private FileStatusTracker<WorkspaceItemEventArgs> fileStatusTracker;

		// Token: 0x040003DA RID: 986
		[ProjectPathItemProperty("BaseDirectory", DefaultValue = null)]
		private FilePath baseDirectory;
	}
}
