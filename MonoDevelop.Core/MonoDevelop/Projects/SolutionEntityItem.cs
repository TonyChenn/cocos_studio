using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Core.StringParsing;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects
{
	// Token: 0x02000110 RID: 272
	[ProjectModelDataItem(FallbackType = typeof(UnknownSolutionItem))]
	public abstract class SolutionEntityItem : SolutionItem, IConfigurationTarget, IBuildTarget, IWorkspaceFileObject, IWorkspaceObject, IExtendedDataItem, IFolderItem, IDisposable, IFileItem
	{
		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06000A0B RID: 2571 RVA: 0x0002711C File Offset: 0x0002531C
		// (remove) Token: 0x06000A0C RID: 2572 RVA: 0x00027154 File Offset: 0x00025354
		public event EventHandler ConfigurationsChanged;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06000A0D RID: 2573 RVA: 0x0002718C File Offset: 0x0002538C
		// (remove) Token: 0x06000A0E RID: 2574 RVA: 0x000271C4 File Offset: 0x000253C4
		public event ConfigurationEventHandler DefaultConfigurationChanged;

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000A0F RID: 2575 RVA: 0x000271FC File Offset: 0x000253FC
		// (remove) Token: 0x06000A10 RID: 2576 RVA: 0x00027234 File Offset: 0x00025434
		public event ConfigurationEventHandler ConfigurationAdded;

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06000A11 RID: 2577 RVA: 0x0002726C File Offset: 0x0002546C
		// (remove) Token: 0x06000A12 RID: 2578 RVA: 0x000272A4 File Offset: 0x000254A4
		public event ConfigurationEventHandler ConfigurationRemoved;

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06000A13 RID: 2579 RVA: 0x000272DC File Offset: 0x000254DC
		// (remove) Token: 0x06000A14 RID: 2580 RVA: 0x00027314 File Offset: 0x00025514
		public event EventHandler<ProjectItemEventArgs> ProjectItemAdded;

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06000A15 RID: 2581 RVA: 0x0002734C File Offset: 0x0002554C
		// (remove) Token: 0x06000A16 RID: 2582 RVA: 0x00027384 File Offset: 0x00025584
		public event EventHandler<ProjectItemEventArgs> ProjectItemRemoved;

		// Token: 0x06000A17 RID: 2583 RVA: 0x000273BC File Offset: 0x000255BC
		public SolutionEntityItem()
		{
			this.items = new ProjectItemCollection(this);
			this.wildcardItems = new ProjectItemCollection(this);
			this.thisItemArgs = new SolutionItemEventArgs(this);
			this.configurations = new SolutionItemConfigurationCollection(this);
			this.configurations.ConfigurationAdded += this.OnConfigurationAddedToCollection;
			this.configurations.ConfigurationRemoved += this.OnConfigurationRemovedFromCollection;
			Counters.ItemsLoaded = ++Counters.ItemsLoaded;
			this.fileStatusTracker = new FileStatusTracker<SolutionItemEventArgs>(this, new Action<SolutionItemEventArgs>(this.OnReloadRequired), new SolutionItemEventArgs(this));
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00027488 File Offset: 0x00025688
		public override void Dispose()
		{
			if (base.Disposed)
			{
				return;
			}
			Counters.ItemsLoaded = --Counters.ItemsLoaded;
			foreach (ProjectItem projectItem in this.items.Concat(this.wildcardItems))
			{
				IDisposable disposable = projectItem as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			base.Dispose();
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00027508 File Offset: 0x00025708
		protected override void OnBoundToSolution()
		{
			base.OnBoundToSolution();
			base.ParentSolution.SolutionItemRemoved += this.HandleSolutionItemRemoved;
			base.ParentSolution.SolutionItemAdded += this.HandleSolutionItemAdded;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0002753E File Offset: 0x0002573E
		protected override void OnUnboundFromSolution()
		{
			base.OnUnboundFromSolution();
			base.ParentSolution.SolutionItemAdded -= this.HandleSolutionItemAdded;
			base.ParentSolution.SolutionItemRemoved -= this.HandleSolutionItemRemoved;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00027574 File Offset: 0x00025774
		private void HandleSolutionItemAdded(object sender, SolutionItemChangeEventArgs e)
		{
			if (e.Reloading && this.dependencies.Count > 0 && e.SolutionItem is SolutionEntityItem && e.ReplacedItem is SolutionEntityItem)
			{
				int num = this.dependencies.IndexOf((SolutionEntityItem)e.ReplacedItem);
				if (num != -1)
				{
					this.dependencies[num] = (SolutionEntityItem)e.SolutionItem;
				}
			}
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x000275E3 File Offset: 0x000257E3
		private void HandleSolutionItemRemoved(object sender, SolutionItemChangeEventArgs e)
		{
			if (!e.Reloading && e.SolutionItem is SolutionEntityItem)
			{
				this.dependencies.Remove((SolutionEntityItem)e.SolutionItem);
			}
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00027614 File Offset: 0x00025814
		internal override void SetItemHandler(ISolutionItemHandler handler)
		{
			string text = this.Name;
			string value = this.FileName;
			base.SetItemHandler(handler);
			this.Name = text;
			if (!string.IsNullOrEmpty(value))
			{
				this.FileName = value;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x00027656 File Offset: 0x00025856
		// (set) Token: 0x06000A1F RID: 2591 RVA: 0x0002765E File Offset: 0x0002585E
		public string Version
		{
			get
			{
				return this.releaseVersion;
			}
			set
			{
				this.releaseVersion = value;
				base.NotifyModified("Version");
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x00027672 File Offset: 0x00025872
		// (set) Token: 0x06000A21 RID: 2593 RVA: 0x0002767A File Offset: 0x0002587A
		public bool SyncVersionWithSolution
		{
			get
			{
				return this.syncReleaseVersion;
			}
			set
			{
				this.syncReleaseVersion = value;
				if (this.syncReleaseVersion && base.ParentSolution != null)
				{
					this.Version = base.ParentSolution.Version;
				}
				base.NotifyModified("SyncVersionWithSolution");
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x000276AF File Offset: 0x000258AF
		// (set) Token: 0x06000A23 RID: 2595 RVA: 0x000276C0 File Offset: 0x000258C0
		[ItemProperty("name")]
		public override string Name
		{
			get
			{
				return this.name ?? string.Empty;
			}
			set
			{
				if (this.name == value)
				{
					return;
				}
				string oldName = this.name;
				this.name = value;
				if (!base.Loading && base.ItemHandler.SyncFileName)
				{
					if (string.IsNullOrEmpty(this.fileName))
					{
						this.FileName = value;
					}
					else
					{
						string extension = this.fileName.Extension;
						this.FileName = this.fileName.ParentDirectory.Combine(new string[]
						{
							value
						}) + extension;
					}
				}
				this.OnNameChanged(new SolutionItemRenamedEventArgs(this, oldName, this.name));
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x00027773 File Offset: 0x00025973
		// (set) Token: 0x06000A25 RID: 2597 RVA: 0x0002777C File Offset: 0x0002597C
		public virtual FilePath FileName
		{
			get
			{
				return this.fileName;
			}
			set
			{
				this.fileName = value;
				if (this.FileFormat != null)
				{
					this.fileName = this.FileFormat.GetValidFileName(this, this.fileName);
				}
				if (base.ItemHandler.SyncFileName)
				{
					this.Name = this.fileName.FileNameWithoutExtension;
				}
				base.NotifyModified("FileName");
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x000277E3 File Offset: 0x000259E3
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x00027805 File Offset: 0x00025A05
		public bool Enabled
		{
			get
			{
				return base.ParentSolution == null || base.ParentSolution.IsSolutionItemEnabled(this.FileName);
			}
			set
			{
				if (base.ParentSolution != null)
				{
					base.ParentSolution.SetSolutionItemEnabled(this.FileName, value);
				}
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x00027828 File Offset: 0x00025A28
		// (set) Token: 0x06000A29 RID: 2601 RVA: 0x00027890 File Offset: 0x00025A90
		public FileFormat FileFormat
		{
			get
			{
				if (base.ParentSolution == null)
				{
					if (this.fileFormat == null)
					{
						this.fileFormat = Services.ProjectService.GetDefaultFormat(this);
					}
					return this.fileFormat;
				}
				if (base.ParentSolution.FileFormat.Format.SupportsMixedFormats && this.fileFormat != null)
				{
					return this.fileFormat;
				}
				return base.ParentSolution.FileFormat;
			}
			set
			{
				if (base.ParentSolution != null && !base.ParentSolution.FileFormat.Format.SupportsMixedFormats)
				{
					throw new InvalidOperationException("The file format can't be changed when the item belongs to a solution.");
				}
				this.InstallFormat(value);
				this.fileFormat.Format.ConvertToFormat(this);
				this.NeedsReload = false;
				base.NotifyModified("FileFormat");
			}
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x000278F1 File Offset: 0x00025AF1
		public virtual bool SupportsConfigurations()
		{
			return base.SupportsBuild();
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x000278F9 File Offset: 0x00025AF9
		public ProjectItemCollection Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x00027901 File Offset: 0x00025B01
		internal ProjectItemCollection WildcardItems
		{
			get
			{
				return this.wildcardItems;
			}
		}

		/// <summary>
		/// Projects that need to be built before building this one
		/// </summary>
		/// <value>The dependencies.</value>
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x00027909 File Offset: 0x00025B09
		public ItemCollection<SolutionEntityItem> ItemDependencies
		{
			get
			{
				return this.dependencies;
			}
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00027911 File Offset: 0x00025B11
		public override IEnumerable<SolutionItem> GetReferencedItems(ConfigurationSelector configuration)
		{
			return base.GetReferencedItems(configuration).Concat(this.dependencies);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00027925 File Offset: 0x00025B25
		void IWorkspaceFileObject.ConvertToFormat(FileFormat format, bool convertChildren)
		{
			this.FileFormat = format;
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0002792E File Offset: 0x00025B2E
		public virtual bool SupportsFormat(FileFormat format)
		{
			return true;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00027931 File Offset: 0x00025B31
		internal void InstallFormat(FileFormat format)
		{
			this.fileFormat = format;
			if (this.fileName != FilePath.Null)
			{
				this.fileName = this.fileFormat.GetValidFileName(this, this.fileName);
			}
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0002796E File Offset: 0x00025B6E
		protected override void InitializeItemHandler()
		{
			Services.ProjectService.GetDefaultFormat(this).Format.ConvertToFormat(this);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00027988 File Offset: 0x00025B88
		protected override FilePath GetDefaultBaseDirectory()
		{
			if (!this.FileName.IsNullOrEmpty)
			{
				return this.FileName.ParentDirectory;
			}
			return FilePath.Empty;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x000279B9 File Offset: 0x00025BB9
		public void Save(FilePath fileName, IProgressMonitor monitor)
		{
			this.FileName = fileName;
			this.Save(monitor);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x000279CC File Offset: 0x00025BCC
		public override void Save(IProgressMonitor monitor)
		{
			if (string.IsNullOrEmpty(this.FileName))
			{
				throw new InvalidOperationException("Project does not have a file name");
			}
			try
			{
				this.fileStatusTracker.BeginSave();
				Services.ProjectService.GetExtensionChain(this).Save(monitor, this);
				this.OnSaved(this.thisItemArgs);
			}
			finally
			{
				this.fileStatusTracker.EndSave();
			}
			FileService.NotifyFileChanged(this.FileName);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00027A48 File Offset: 0x00025C48
		protected override void OnEndLoad()
		{
			base.OnEndLoad();
			this.fileStatusTracker.ResetLoadTimes();
			if (this.syncReleaseVersion && base.ParentSolution != null)
			{
				this.releaseVersion = base.ParentSolution.Version;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00027A7C File Offset: 0x00025C7C
		internal bool IsSaved
		{
			get
			{
				return !string.IsNullOrEmpty(this.FileName) && File.Exists(this.FileName);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00027AA2 File Offset: 0x00025CA2
		// (set) Token: 0x06000A39 RID: 2617 RVA: 0x00027AAF File Offset: 0x00025CAF
		public override bool NeedsReload
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

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00027ABD File Offset: 0x00025CBD
		public virtual bool ItemFilesChanged
		{
			get
			{
				return this.fileStatusTracker.ItemFilesChanged;
			}
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00027ACC File Offset: 0x00025CCC
		protected internal override BuildResult OnRunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
		{
			if (target == "Build")
			{
				SolutionItemConfiguration configuration2 = this.GetConfiguration(configuration);
				if (configuration2 != null && configuration2.CustomCommands.HasCommands(CustomCommandType.Build))
				{
					configuration2.CustomCommands.ExecuteCommand(monitor, this, CustomCommandType.Build, configuration);
					return new BuildResult();
				}
			}
			else if (target == "Clean")
			{
				this.SetFastBuildCheckDirty();
				SolutionItemConfiguration configuration3 = this.GetConfiguration(configuration);
				if (configuration3 != null && configuration3.CustomCommands.HasCommands(CustomCommandType.Clean))
				{
					configuration3.CustomCommands.ExecuteCommand(monitor, this, CustomCommandType.Clean, configuration);
					return new BuildResult();
				}
			}
			BuildResult buildResult = base.OnRunTarget(monitor, target, configuration);
			if (!buildResult.Failed && target == "Build")
			{
				this.SetFastBuildCheckClean(configuration);
			}
			return buildResult;
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x00027B7C File Offset: 0x00025D7C
		protected internal virtual void OnSave(IProgressMonitor monitor)
		{
			base.ItemHandler.Save(monitor);
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00027B8A File Offset: 0x00025D8A
		[Obsolete("This method will be removed in future releases")]
		public void SetNeedsBuilding(bool value)
		{
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00027B8C File Offset: 0x00025D8C
		public FilePath GetAbsoluteChildPath(FilePath relPath)
		{
			return relPath.ToAbsolute(base.BaseDirectory);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00027B9B File Offset: 0x00025D9B
		public FilePath GetRelativeChildPath(FilePath absPath)
		{
			return absPath.ToRelative(base.BaseDirectory);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00027BAA File Offset: 0x00025DAA
		public List<FilePath> GetItemFiles(bool includeReferencedFiles)
		{
			return Services.ProjectService.GetExtensionChain(this).GetItemFiles(this, includeReferencedFiles);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00027BC0 File Offset: 0x00025DC0
		protected internal virtual List<FilePath> OnGetItemFiles(bool includeReferencedFiles)
		{
			List<FilePath> itemFiles = this.FileFormat.Format.GetItemFiles(this);
			if (!string.IsNullOrEmpty(this.FileName) && !itemFiles.Contains(this.FileName))
			{
				itemFiles.Add(this.FileName);
			}
			return itemFiles;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00027C0C File Offset: 0x00025E0C
		protected override void OnNameChanged(SolutionItemRenamedEventArgs e)
		{
			Solution parentSolution = base.ParentSolution;
			if (parentSolution != null)
			{
				foreach (DotNetProject dotNetProject in parentSolution.GetAllSolutionItems<DotNetProject>())
				{
					if (dotNetProject != this)
					{
						dotNetProject.RenameReferences(e.OldName, e.NewName);
					}
				}
			}
			this.fileStatusTracker.ResetLoadTimes();
			base.OnNameChanged(e);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00027C84 File Offset: 0x00025E84
		public virtual bool FastCheckNeedsBuild(ConfigurationSelector configuration)
		{
			if (this.disableFastUpToDateCheck || this.fastUpToDateCheckGoodConfig == null)
			{
				return true;
			}
			SolutionItemConfiguration configuration2 = this.GetConfiguration(configuration);
			return configuration2 == null || configuration2.Id != this.fastUpToDateCheckGoodConfig;
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00027CC1 File Offset: 0x00025EC1
		protected void SetFastBuildCheckDirty()
		{
			this.fastUpToDateCheckGoodConfig = null;
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00027CCC File Offset: 0x00025ECC
		private void SetFastBuildCheckClean(ConfigurationSelector configuration)
		{
			SolutionItemConfiguration configuration2 = this.GetConfiguration(configuration);
			this.fastUpToDateCheckGoodConfig = ((configuration2 != null) ? configuration2.Id : null);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00027CF3 File Offset: 0x00025EF3
		protected virtual void OnSaved(SolutionItemEventArgs args)
		{
			this.SetFastBuildCheckDirty();
			if (this.Saved != null)
			{
				this.Saved(this, args);
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00027D10 File Offset: 0x00025F10
		public virtual string[] SupportedPlatforms
		{
			get
			{
				return new string[0];
			}
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00027D18 File Offset: 0x00025F18
		public virtual SolutionItemConfiguration GetConfiguration(ConfigurationSelector configuration)
		{
			return ((SolutionItemConfiguration)configuration.GetConfiguration(this)) ?? this.DefaultConfiguration;
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x00027D30 File Offset: 0x00025F30
		// (set) Token: 0x06000A4A RID: 2634 RVA: 0x00027D38 File Offset: 0x00025F38
		ItemConfiguration IConfigurationTarget.DefaultConfiguration
		{
			get
			{
				return this.DefaultConfiguration;
			}
			set
			{
				this.DefaultConfiguration = (SolutionItemConfiguration)value;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x00027D46 File Offset: 0x00025F46
		// (set) Token: 0x06000A4C RID: 2636 RVA: 0x00027D71 File Offset: 0x00025F71
		public SolutionItemConfiguration DefaultConfiguration
		{
			get
			{
				if (this.activeConfiguration == null && this.configurations.Count > 0)
				{
					return this.configurations[0];
				}
				return this.activeConfiguration;
			}
			set
			{
				if (this.activeConfiguration != value)
				{
					this.activeConfiguration = value;
					base.NotifyModified("DefaultConfiguration");
					this.OnDefaultConfigurationChanged(new ConfigurationEventArgs(this, value));
				}
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x00027D9B File Offset: 0x00025F9B
		// (set) Token: 0x06000A4E RID: 2638 RVA: 0x00027DB2 File Offset: 0x00025FB2
		public string DefaultConfigurationId
		{
			get
			{
				if (this.DefaultConfiguration != null)
				{
					return this.DefaultConfiguration.Id;
				}
				return null;
			}
			set
			{
				this.DefaultConfiguration = this.GetConfiguration(new ItemConfigurationSelector(value));
			}
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00027DC8 File Offset: 0x00025FC8
		public virtual ReadOnlyCollection<string> GetConfigurations()
		{
			List<string> list = new List<string>();
			foreach (SolutionItemConfiguration solutionItemConfiguration in this.Configurations)
			{
				list.Add(solutionItemConfiguration.Id);
			}
			return list.AsReadOnly();
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x00027E28 File Offset: 0x00026028
		[ItemProperty("Configuration", ValueType = typeof(SolutionItemConfiguration), Scope = "*")]
		[ItemProperty("Configurations")]
		public SolutionItemConfigurationCollection Configurations
		{
			get
			{
				return this.configurations;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x00027E30 File Offset: 0x00026030
		IItemConfigurationCollection IConfigurationTarget.Configurations
		{
			get
			{
				return this.Configurations;
			}
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00027E38 File Offset: 0x00026038
		public SolutionItemConfiguration AddNewConfiguration(string name)
		{
			SolutionItemConfiguration solutionItemConfiguration = this.CreateConfiguration(name);
			this.Configurations.Add(solutionItemConfiguration);
			return solutionItemConfiguration;
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00027E5A File Offset: 0x0002605A
		ItemConfiguration IConfigurationTarget.CreateConfiguration(string name)
		{
			return this.CreateConfiguration(name);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00027E63 File Offset: 0x00026063
		public virtual SolutionItemConfiguration CreateConfiguration(string name)
		{
			return new SolutionItemConfiguration(name);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00027E6C File Offset: 0x0002606C
		private void OnConfigurationAddedToCollection(object ob, ConfigurationEventArgs args)
		{
			base.NotifyModified("Configurations");
			this.OnConfigurationAdded(new ConfigurationEventArgs(this, args.Configuration));
			if (this.ConfigurationsChanged != null)
			{
				this.ConfigurationsChanged(this, EventArgs.Empty);
			}
			if (this.activeConfiguration == null)
			{
				this.DefaultConfigurationId = args.Configuration.Id;
			}
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00027EC8 File Offset: 0x000260C8
		private void OnConfigurationRemovedFromCollection(object ob, ConfigurationEventArgs args)
		{
			if (this.activeConfiguration == args.Configuration)
			{
				if (this.Configurations.Count > 0)
				{
					this.DefaultConfiguration = this.Configurations[0];
				}
				else
				{
					this.DefaultConfiguration = null;
				}
			}
			base.NotifyModified("Configurations");
			this.OnConfigurationRemoved(new ConfigurationEventArgs(this, args.Configuration));
			if (this.ConfigurationsChanged != null)
			{
				this.ConfigurationsChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00027F44 File Offset: 0x00026144
		public override StringTagModelDescription GetStringTagModelDescription(ConfigurationSelector conf)
		{
			StringTagModelDescription stringTagModelDescription = base.GetStringTagModelDescription(conf);
			SolutionItemConfiguration configuration = this.GetConfiguration(conf);
			if (configuration != null)
			{
				stringTagModelDescription.Add(configuration.GetType());
			}
			else
			{
				stringTagModelDescription.Add(typeof(SolutionItemConfiguration));
			}
			return stringTagModelDescription;
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00027F84 File Offset: 0x00026184
		public override StringTagModel GetStringTagModel(ConfigurationSelector conf)
		{
			StringTagModel stringTagModel = base.GetStringTagModel(conf);
			SolutionItemConfiguration configuration = this.GetConfiguration(conf);
			if (configuration != null)
			{
				stringTagModel.Add(configuration);
			}
			return stringTagModel;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00027FB8 File Offset: 0x000261B8
		protected internal virtual void OnItemsAdded(IEnumerable<ProjectItem> objs)
		{
			base.NotifyModified("Items");
			ProjectItemEventArgs projectItemEventArgs = new ProjectItemEventArgs();
			projectItemEventArgs.AddRange(from pi in objs
			select new ProjectItemEventInfo(this, pi));
			if (this.ProjectItemAdded != null)
			{
				this.ProjectItemAdded(this, projectItemEventArgs);
			}
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0002800C File Offset: 0x0002620C
		protected internal virtual void OnItemsRemoved(IEnumerable<ProjectItem> objs)
		{
			base.NotifyModified("Items");
			ProjectItemEventArgs projectItemEventArgs = new ProjectItemEventArgs();
			projectItemEventArgs.AddRange(from pi in objs
			select new ProjectItemEventInfo(this, pi));
			if (this.ProjectItemRemoved != null)
			{
				this.ProjectItemRemoved(this, projectItemEventArgs);
			}
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00028057 File Offset: 0x00026257
		protected virtual void OnDefaultConfigurationChanged(ConfigurationEventArgs args)
		{
			if (this.DefaultConfigurationChanged != null)
			{
				this.DefaultConfigurationChanged(this, args);
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0002806E File Offset: 0x0002626E
		protected virtual void OnConfigurationAdded(ConfigurationEventArgs args)
		{
			if (this.ConfigurationAdded != null)
			{
				this.ConfigurationAdded(this, args);
			}
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00028085 File Offset: 0x00026285
		protected virtual void OnConfigurationRemoved(ConfigurationEventArgs args)
		{
			if (this.ConfigurationRemoved != null)
			{
				this.ConfigurationRemoved(this, args);
			}
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0002809C File Offset: 0x0002629C
		protected virtual void OnReloadRequired(SolutionItemEventArgs args)
		{
			this.fileStatusTracker.FireReloadRequired(args);
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06000A5F RID: 2655 RVA: 0x000280AC File Offset: 0x000262AC
		// (remove) Token: 0x06000A60 RID: 2656 RVA: 0x000280E4 File Offset: 0x000262E4
		public event SolutionItemEventHandler Saved;

		// Token: 0x04000311 RID: 785
		internal object MemoryProbe = Counters.ItemsInMemory.CreateMemoryProbe();

		// Token: 0x04000312 RID: 786
		private ProjectItemCollection items;

		// Token: 0x04000313 RID: 787
		private ProjectItemCollection wildcardItems;

		// Token: 0x04000314 RID: 788
		private ItemCollection<SolutionEntityItem> dependencies = new ItemCollection<SolutionEntityItem>();

		// Token: 0x04000315 RID: 789
		private SolutionItemEventArgs thisItemArgs;

		// Token: 0x04000316 RID: 790
		private FileStatusTracker<SolutionItemEventArgs> fileStatusTracker;

		// Token: 0x04000317 RID: 791
		private FilePath fileName;

		// Token: 0x04000318 RID: 792
		private string name;

		// Token: 0x04000319 RID: 793
		private FileFormat fileFormat;

		// Token: 0x0400031A RID: 794
		private SolutionItemConfiguration activeConfiguration;

		// Token: 0x0400031B RID: 795
		private SolutionItemConfigurationCollection configurations;

		// Token: 0x04000322 RID: 802
		[ItemProperty("ReleaseVersion", DefaultValue = "0.1")]
		private string releaseVersion = "0.1";

		// Token: 0x04000323 RID: 803
		[ItemProperty("SynchReleaseVersion", DefaultValue = true)]
		private bool syncReleaseVersion = true;

		// Token: 0x04000324 RID: 804
		[ItemProperty("DisableFastUpToDateCheck", DefaultValue = false)]
		private bool disableFastUpToDateCheck;

		// Token: 0x04000325 RID: 805
		private string fastUpToDateCheckGoodConfig;
	}
}
