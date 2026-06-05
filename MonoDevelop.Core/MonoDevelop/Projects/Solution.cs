using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Policies;

namespace MonoDevelop.Projects
{
	// Token: 0x02000163 RID: 355
	[ProjectModelDataItem]
	public class Solution : WorkspaceItem, IConfigurationTarget, IBuildTarget, IWorkspaceObject, IExtendedDataItem, IFolderItem, IDisposable, IPolicyProvider
	{
		// Token: 0x06000D5D RID: 3421 RVA: 0x00030DDC File Offset: 0x0002EFDC
		public Solution()
		{
			Counters.SolutionsLoaded = ++Counters.SolutionsLoaded;
			this.configurations = new SolutionConfigurationCollection(this);
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00030E2C File Offset: 0x0002F02C
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x00030E53 File Offset: 0x0002F053
		public SolutionFolder RootFolder
		{
			get
			{
				if (this.rootFolder == null)
				{
					this.rootFolder = new SolutionFolder();
					this.rootFolder.ParentSolution = this;
				}
				return this.rootFolder;
			}
			internal set
			{
				this.rootFolder = value;
			}
		}

		/// <summary>
		/// Folder where to add solution files, when none is created
		/// </summary>
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00030E70 File Offset: 0x0002F070
		public SolutionFolder DefaultSolutionFolder
		{
			get
			{
				SolutionFolder solutionFolder = (SolutionFolder)this.RootFolder.Items.FirstOrDefault((SolutionItem item) => item.Name == "Solution Items");
				if (solutionFolder == null)
				{
					solutionFolder = new SolutionFolder();
					solutionFolder.Name = "Solution Items";
					this.RootFolder.AddItem(solutionFolder);
				}
				return solutionFolder;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000D61 RID: 3425 RVA: 0x00030ED4 File Offset: 0x0002F0D4
		public ReadOnlyCollection<SolutionItem> Items
		{
			get
			{
				if (this.solutionItems == null)
				{
					List<SolutionItem> list = new List<SolutionItem>();
					foreach (SolutionItem solutionItem in base.GetAllSolutionItems())
					{
						if (!(solutionItem is SolutionFolder))
						{
							list.Add(solutionItem);
						}
					}
					this.solutionItems = list.AsReadOnly();
				}
				return this.solutionItems;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00030F54 File Offset: 0x0002F154
		// (set) Token: 0x06000D63 RID: 3427 RVA: 0x00030FD8 File Offset: 0x0002F1D8
		public SolutionEntityItem StartupItem
		{
			get
			{
				if (this.startItemFileName != null)
				{
					this.startupItem = this.FindSolutionItem(this.startItemFileName);
					this.startItemFileName = null;
					this.singleStartup = true;
				}
				if (this.startupItem == null && this.singleStartup)
				{
					ReadOnlyCollection<SolutionEntityItem> allSolutionItems = this.GetAllSolutionItems<SolutionEntityItem>();
					if (allSolutionItems.Count > 0)
					{
						this.startupItem = allSolutionItems.FirstOrDefault((SolutionEntityItem it) => it.SupportsExecute());
					}
				}
				return this.startupItem;
			}
			set
			{
				this.startupItem = value;
				this.startItemFileName = null;
				base.NotifyModified();
				this.OnStartupItemChanged(null);
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00030FF5 File Offset: 0x0002F1F5
		// (set) Token: 0x06000D65 RID: 3429 RVA: 0x00031014 File Offset: 0x0002F214
		public bool SingleStartup
		{
			get
			{
				return this.startItemFileName != null || (this.multiStartupItems == null && this.singleStartup);
			}
			set
			{
				if (this.SingleStartup == value)
				{
					return;
				}
				this.singleStartup = value;
				if (value)
				{
					if (this.MultiStartupItems.Count > 0)
					{
						this.startupItem = this.startupItems[0];
					}
				}
				else
				{
					this.MultiStartupItems.Clear();
					if (this.StartupItem != null)
					{
						this.MultiStartupItems.Add(this.StartupItem);
					}
				}
				base.NotifyModified();
				this.OnStartupItemChanged(null);
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x00031088 File Offset: 0x0002F288
		public List<SolutionEntityItem> MultiStartupItems
		{
			get
			{
				if (this.multiStartupItems != null)
				{
					this.startupItems = new List<SolutionEntityItem>();
					foreach (string fileName in this.multiStartupItems)
					{
						SolutionEntityItem solutionEntityItem = this.FindSolutionItem(fileName);
						if (solutionEntityItem != null)
						{
							this.startupItems.Add(solutionEntityItem);
						}
					}
					this.multiStartupItems = null;
					this.singleStartup = false;
				}
				else if (this.startupItems == null)
				{
					this.startupItems = new List<SolutionEntityItem>();
				}
				return this.startupItems;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000D67 RID: 3431 RVA: 0x00031128 File Offset: 0x0002F328
		// (set) Token: 0x06000D68 RID: 3432 RVA: 0x0003114C File Offset: 0x0002F34C
		[ProjectPathItemProperty("StartupItem", DefaultValue = null, ReadOnly = true)]
		internal string StartupItemFileName
		{
			get
			{
				if (this.SingleStartup && this.StartupItem != null)
				{
					return this.StartupItem.FileName;
				}
				return null;
			}
			set
			{
				this.startItemFileName = value;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000D69 RID: 3433 RVA: 0x00031158 File Offset: 0x0002F358
		// (set) Token: 0x06000D6A RID: 3434 RVA: 0x000311D8 File Offset: 0x0002F3D8
		[ItemProperty("StartupItems", ReadOnly = true)]
		[ProjectPathItemProperty("Item", Scope = "*")]
		internal List<string> MultiStartupItemFileNames
		{
			get
			{
				if (this.SingleStartup)
				{
					return null;
				}
				if (this.multiStartupItems != null)
				{
					return this.multiStartupItems;
				}
				List<string> list = new List<string>();
				foreach (SolutionEntityItem solutionEntityItem in this.MultiStartupItems)
				{
					list.Add(solutionEntityItem.FileName);
				}
				return list;
			}
			set
			{
				this.multiStartupItems = value;
			}
		}

		/// <summary>
		/// Gets the author information for this solution. If no specific information is set for this solution, it
		/// will return the author defined in the global settings.
		/// </summary>
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x000311E1 File Offset: 0x0002F3E1
		public AuthorInformation AuthorInformation
		{
			get
			{
				return this.LocalAuthorInformation ?? AuthorInformation.Default;
			}
		}

		/// <summary>
		/// Gets or sets the author information for this solution. It returns null if no specific information
		/// has been set for this solution.
		/// </summary>
		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x000311F2 File Offset: 0x0002F3F2
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x00031204 File Offset: 0x0002F404
		public AuthorInformation LocalAuthorInformation
		{
			get
			{
				return this.UserProperties.GetValue<AuthorInformation>("AuthorInfo");
			}
			set
			{
				if (value != null)
				{
					this.UserProperties.SetValue<AuthorInformation>("AuthorInfo", value);
					return;
				}
				this.UserProperties.RemoveValue("AuthorInfo");
			}
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0003122C File Offset: 0x0002F42C
		protected override void OnEndLoad()
		{
			base.OnEndLoad();
			this.LoadItemProperties(this.UserProperties, this.RootFolder, "MonoDevelop.Ide.ItemProperties");
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00031260 File Offset: 0x0002F460
		public override void LoadUserProperties()
		{
			base.LoadUserProperties();
			string value = this.UserProperties.GetValue<string>("StartupItem");
			if (!string.IsNullOrEmpty(value))
			{
				this.startItemFileName = base.GetAbsoluteChildPath(value);
			}
			string[] value2 = this.UserProperties.GetValue<string[]>("StartupItems");
			if (value2 != null && value2.Length > 0)
			{
				this.multiStartupItems = (from p in value2
				select base.GetAbsoluteChildPath(p)).ToList<string>();
			}
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x000312F0 File Offset: 0x0002F4F0
		public override void SaveUserProperties()
		{
			this.UserProperties.SetValue<string>("StartupItem", base.GetRelativeChildPath(this.StartupItemFileName));
			if (this.MultiStartupItemFileNames != null)
			{
				this.UserProperties.SetValue<string[]>("StartupItems", (from p in this.MultiStartupItemFileNames
				select base.GetRelativeChildPath(p)).ToArray<string>());
			}
			else
			{
				this.UserProperties.RemoveValue("StartupItems");
			}
			this.CollectItemProperties(this.UserProperties, this.RootFolder, "MonoDevelop.Ide.ItemProperties");
			base.SaveUserProperties();
			this.CleanItemProperties(this.UserProperties, this.RootFolder, "MonoDevelop.Ide.ItemProperties");
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x000313A8 File Offset: 0x0002F5A8
		private void CollectItemProperties(PropertyBag props, SolutionItem item, string path)
		{
			if (!item.UserProperties.IsEmpty && item.ParentFolder != null)
			{
				props.SetValue<PropertyBag>(path, item.UserProperties);
			}
			SolutionFolder solutionFolder = item as SolutionFolder;
			if (solutionFolder != null)
			{
				foreach (SolutionItem solutionItem in solutionFolder.Items)
				{
					this.CollectItemProperties(props, solutionItem, path + "." + solutionItem.Name);
				}
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00031434 File Offset: 0x0002F634
		private void CleanItemProperties(PropertyBag props, SolutionItem item, string path)
		{
			props.RemoveValue(path);
			SolutionFolder solutionFolder = item as SolutionFolder;
			if (solutionFolder != null)
			{
				foreach (SolutionItem solutionItem in solutionFolder.Items)
				{
					this.CleanItemProperties(props, solutionItem, path + "." + solutionItem.Name);
				}
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x000314A8 File Offset: 0x0002F6A8
		private void LoadItemProperties(PropertyBag props, SolutionItem item, string path)
		{
			PropertyBag value = props.GetValue<PropertyBag>(path);
			if (value != null)
			{
				item.LoadUserProperties(value);
				props.RemoveValue(path);
			}
			SolutionFolder solutionFolder = item as SolutionFolder;
			if (solutionFolder != null)
			{
				foreach (SolutionItem solutionItem in solutionFolder.Items)
				{
					this.LoadItemProperties(props, solutionItem, path + "." + solutionItem.Name);
				}
			}
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00031534 File Offset: 0x0002F734
		public void CreateDefaultConfigurations()
		{
			foreach (SolutionItem solutionItem in from it in this.Items
			where it.SupportsBuild()
			select it)
			{
				SolutionEntityItem solutionEntityItem = (SolutionEntityItem)solutionItem;
				foreach (ItemConfiguration itemConfiguration in solutionEntityItem.Configurations)
				{
					SolutionConfiguration solutionConfiguration = this.Configurations[itemConfiguration.Id];
					if (solutionConfiguration == null)
					{
						solutionConfiguration = new SolutionConfiguration(itemConfiguration.Id);
						this.Configurations.Add(solutionConfiguration);
					}
					solutionConfiguration.AddItem(solutionEntityItem);
				}
			}
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00031610 File Offset: 0x0002F810
		ItemConfiguration IConfigurationTarget.CreateConfiguration(string name)
		{
			return new SolutionConfiguration(name);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00031620 File Offset: 0x0002F820
		public SolutionConfiguration AddConfiguration(string name, bool createConfigForItems)
		{
			SolutionConfiguration solutionConfiguration = new SolutionConfiguration(name);
			foreach (SolutionItem solutionItem in from it in this.Items
			where it.SupportsBuild()
			select it)
			{
				SolutionEntityItem solutionEntityItem = (SolutionEntityItem)solutionItem;
				if (createConfigForItems && solutionEntityItem.GetConfiguration(new ItemConfigurationSelector(name)) == null)
				{
					SolutionItemConfiguration solutionItemConfiguration = solutionEntityItem.CreateConfiguration(name);
					if (solutionEntityItem.DefaultConfiguration != null)
					{
						solutionItemConfiguration.CopyFrom(solutionEntityItem.DefaultConfiguration);
					}
					solutionEntityItem.Configurations.Add(solutionItemConfiguration);
				}
				solutionConfiguration.AddItem(solutionEntityItem);
			}
			this.configurations.Add(solutionConfiguration);
			return solutionConfiguration;
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000316E4 File Offset: 0x0002F8E4
		public override ReadOnlyCollection<string> GetConfigurations()
		{
			List<string> list = new List<string>();
			foreach (SolutionConfiguration solutionConfiguration in this.Configurations)
			{
				list.Add(solutionConfiguration.Id);
			}
			return list.AsReadOnly();
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00031744 File Offset: 0x0002F944
		public virtual SolutionConfiguration GetConfiguration(ConfigurationSelector configuration)
		{
			return ((SolutionConfiguration)configuration.GetConfiguration(this)) ?? this.DefaultConfiguration;
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0003175C File Offset: 0x0002F95C
		public SolutionItem GetSolutionItem(string itemId)
		{
			foreach (SolutionItem solutionItem in this.Items)
			{
				if (solutionItem.ItemId == itemId)
				{
					return solutionItem;
				}
			}
			return null;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x000317B8 File Offset: 0x0002F9B8
		public override SolutionEntityItem FindSolutionItem(string fileName)
		{
			return this.RootFolder.FindSolutionItem(fileName);
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x000317C6 File Offset: 0x0002F9C6
		public Project FindProjectByName(string name)
		{
			return this.RootFolder.FindProjectByName(name);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x000317D4 File Offset: 0x0002F9D4
		public override ReadOnlyCollection<T> GetAllSolutionItems<T>()
		{
			return this.RootFolder.GetAllItems<T>();
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x000317E1 File Offset: 0x0002F9E1
		public ReadOnlyCollection<T> GetAllSolutionItemsWithTopologicalSort<T>(ConfigurationSelector configuration) where T : SolutionItem
		{
			return this.RootFolder.GetAllItemsWithTopologicalSort<T>(configuration);
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x000317EF File Offset: 0x0002F9EF
		public ReadOnlyCollection<Project> GetAllProjectsWithTopologicalSort(ConfigurationSelector configuration)
		{
			return this.RootFolder.GetAllProjectsWithTopologicalSort(configuration);
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x000317FD File Offset: 0x0002F9FD
		[Obsolete("Use GetProjectsContainingFile() (plural) instead")]
		public override Project GetProjectContainingFile(FilePath fileName)
		{
			return this.RootFolder.GetProjectContainingFile(fileName);
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00031810 File Offset: 0x0002FA10
		public override IEnumerable<Project> GetProjectsContainingFile(FilePath fileName)
		{
			return this.RootFolder.GetProjectsContainingFile(fileName);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00031824 File Offset: 0x0002FA24
		public override bool ContainsItem(IWorkspaceObject obj)
		{
			if (base.ContainsItem(obj))
			{
				return true;
			}
			foreach (SolutionItem solutionItem in this.GetAllSolutionItems<SolutionItem>())
			{
				if (solutionItem == obj)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x00031880 File Offset: 0x0002FA80
		// (set) Token: 0x06000D83 RID: 3459 RVA: 0x00031891 File Offset: 0x0002FA91
		public string Description
		{
			get
			{
				return this.description ?? string.Empty;
			}
			set
			{
				this.description = value;
				base.NotifyModified();
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x000318A0 File Offset: 0x0002FAA0
		// (set) Token: 0x06000D85 RID: 3461 RVA: 0x000318B7 File Offset: 0x0002FAB7
		public string OutputDirectory
		{
			get
			{
				if (this.outputdir == null)
				{
					return this.DefaultOutputDirectory;
				}
				return this.outputdir;
			}
			set
			{
				if (value == this.DefaultOutputDirectory)
				{
					this.outputdir = null;
				}
				else
				{
					this.outputdir = value;
				}
				base.NotifyModified();
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x000318E0 File Offset: 0x0002FAE0
		private string DefaultOutputDirectory
		{
			get
			{
				return (base.BaseDirectory != FilePath.Null) ? base.BaseDirectory.Combine(new string[]
				{
					"build",
					"bin"
				}) : FilePath.Null;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00031931 File Offset: 0x0002FB31
		public SolutionConfigurationCollection Configurations
		{
			get
			{
				return this.configurations;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x00031939 File Offset: 0x0002FB39
		// (set) Token: 0x06000D89 RID: 3465 RVA: 0x00031956 File Offset: 0x0002FB56
		public SolutionConfiguration DefaultConfiguration
		{
			get
			{
				if (this.DefaultConfigurationId != null)
				{
					return this.Configurations[this.DefaultConfigurationId];
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.DefaultConfigurationId = value.Id;
					return;
				}
				this.DefaultConfigurationId = null;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0003196F File Offset: 0x0002FB6F
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x000319A4 File Offset: 0x0002FBA4
		public string DefaultConfigurationId
		{
			get
			{
				if (this.defaultConfiguration == null && this.configurations.Count > 0)
				{
					this.DefaultConfigurationId = this.configurations[0].Id;
				}
				return this.defaultConfiguration;
			}
			set
			{
				this.defaultConfiguration = value;
				this.UpdateDefaultConfigurations();
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x000319B3 File Offset: 0x0002FBB3
		public ConfigurationSelector DefaultConfigurationSelector
		{
			get
			{
				if (this.defaultConfiguration == null && this.configurations.Count > 0)
				{
					this.DefaultConfigurationId = this.configurations[0].Id;
				}
				return new SolutionConfigurationSelector(this.DefaultConfigurationId);
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x000319ED File Offset: 0x0002FBED
		IItemConfigurationCollection IConfigurationTarget.Configurations
		{
			get
			{
				return this.Configurations;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x000319F5 File Offset: 0x0002FBF5
		// (set) Token: 0x06000D8F RID: 3471 RVA: 0x000319FD File Offset: 0x0002FBFD
		ItemConfiguration IConfigurationTarget.DefaultConfiguration
		{
			get
			{
				return this.DefaultConfiguration;
			}
			set
			{
				this.DefaultConfiguration = (SolutionConfiguration)value;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x00031A0B File Offset: 0x0002FC0B
		// (set) Token: 0x06000D91 RID: 3473 RVA: 0x00031A18 File Offset: 0x0002FC18
		[ItemProperty("Policies", IsExternal = true, SkipEmpty = true)]
		public PolicyBag Policies
		{
			get
			{
				return this.RootFolder.Policies;
			}
			internal set
			{
				this.RootFolder.Policies = value;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x00031A26 File Offset: 0x0002FC26
		PolicyContainer IPolicyProvider.Policies
		{
			get
			{
				return this.Policies;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x00031A2E File Offset: 0x0002FC2E
		// (set) Token: 0x06000D94 RID: 3476 RVA: 0x00031A40 File Offset: 0x0002FC40
		public string Version
		{
			get
			{
				return this.version ?? string.Empty;
			}
			set
			{
				this.version = value;
				foreach (SolutionEntityItem solutionEntityItem in this.GetAllSolutionItems<SolutionEntityItem>())
				{
					if (solutionEntityItem.SyncVersionWithSolution)
					{
						solutionEntityItem.Version = value;
					}
				}
			}
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00031A9C File Offset: 0x0002FC9C
		public override void Dispose()
		{
			base.Dispose();
			this.RootFolder.Dispose();
			Counters.SolutionsLoaded = --Counters.SolutionsLoaded;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00031AC0 File Offset: 0x0002FCC0
		internal bool IsSolutionItemEnabled(string solutionItemPath)
		{
			solutionItemPath = base.GetRelativeChildPath(Path.GetFullPath(solutionItemPath));
			List<string> value = this.UserProperties.GetValue<List<string>>("DisabledProjects");
			return value == null || !value.Contains(solutionItemPath);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00031B08 File Offset: 0x0002FD08
		public void SetSolutionItemEnabled(string solutionItemPath, bool enabled)
		{
			solutionItemPath = base.GetRelativeChildPath(Path.GetFullPath(solutionItemPath));
			List<string> list = this.UserProperties.GetValue<List<string>>("DisabledProjects");
			if (!enabled)
			{
				if (list == null)
				{
					list = new List<string>();
				}
				if (!list.Contains(solutionItemPath))
				{
					list.Add(solutionItemPath);
				}
				this.UserProperties.SetValue<List<string>>("DisabledProjects", list);
				return;
			}
			if (list != null)
			{
				list.Remove(solutionItemPath);
				if (list.Count == 0)
				{
					this.UserProperties.RemoveValue("DisabledProjects");
					return;
				}
				this.UserProperties.SetValue<List<string>>("DisabledProjects", list);
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00031BA4 File Offset: 0x0002FDA4
		internal void UpdateDefaultConfigurations()
		{
			if (this.DefaultConfiguration != null)
			{
				foreach (SolutionConfigurationEntry solutionConfigurationEntry in this.DefaultConfiguration.Configurations)
				{
					if (solutionConfigurationEntry.Item != null)
					{
						solutionConfigurationEntry.Item.DefaultConfigurationId = solutionConfigurationEntry.ItemConfiguration;
					}
				}
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00031C10 File Offset: 0x0002FE10
		protected override BuildResult OnBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			return this.RootFolder.Build(monitor, configuration);
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00031C1F File Offset: 0x0002FE1F
		protected override void OnClean(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			this.RootFolder.Clean(monitor, configuration);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00031C30 File Offset: 0x0002FE30
		protected internal override bool OnGetCanExecute(ExecutionContext context, ConfigurationSelector configuration)
		{
			if (this.SingleStartup)
			{
				return this.StartupItem != null && this.StartupItem.CanExecute(context, configuration);
			}
			foreach (SolutionEntityItem solutionEntityItem in this.MultiStartupItems)
			{
				if (solutionEntityItem.CanExecute(context, configuration))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00031D34 File Offset: 0x0002FF34
		protected internal override void OnExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
			if (!this.SingleStartup)
			{
				List<IAsyncOperation> list = new List<IAsyncOperation>();
				monitor.BeginTask("Executing projects", 1);
				SynchronizedProgressMonitor slaveMonitor = new SynchronizedProgressMonitor(monitor);
				foreach (SolutionEntityItem solutionEntityItem in this.MultiStartupItems)
				{
					if (solutionEntityItem.CanExecute(context, configuration))
					{
						AggregatedProgressMonitor mon = new AggregatedProgressMonitor();
						mon.AddSlaveMonitor(slaveMonitor, MonitorAction.ReportError | MonitorAction.ReportWarning | MonitorAction.SlaveCancel);
						list.Add(mon.AsyncOperation);
						SolutionEntityItem cit = solutionEntityItem;
						new Thread(delegate()
						{
							try
							{
								using (AggregatedProgressMonitor mon = mon)
								{
									cit.Execute(mon, context, configuration);
								}
							}
							catch (Exception ex)
							{
								LoggingService.LogError("Project execution failed", ex);
							}
						})
						{
							Name = "Project execution",
							IsBackground = true
						}.Start();
					}
				}
				foreach (IAsyncOperation asyncOperation in list)
				{
					asyncOperation.WaitForCompleted();
				}
				monitor.EndTask();
				return;
			}
			if (this.StartupItem == null)
			{
				monitor.ReportError(GettextCatalog.GetString("Startup item not set"), null);
				return;
			}
			this.StartupItem.Execute(monitor, context, configuration);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00031EC4 File Offset: 0x000300C4
		protected virtual void OnStartupItemChanged(EventArgs e)
		{
			if (this.StartupItemChanged != null)
			{
				this.StartupItemChanged(this, e);
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00031EDC File Offset: 0x000300DC
		public override void ConvertToFormat(FileFormat format, bool convertChildren)
		{
			base.ConvertToFormat(format, convertChildren);
			foreach (SolutionItem item in this.GetAllSolutionItems<SolutionItem>())
			{
				this.ConvertToSolutionFormat(item, convertChildren);
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00031F4C File Offset: 0x0003014C
		public override bool SupportsFormat(FileFormat format)
		{
			return base.SupportsFormat(format) && this.GetAllSolutionItems<SolutionEntityItem>().All((SolutionEntityItem p) => p.SupportsFormat(format));
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00031F90 File Offset: 0x00030190
		public override List<FilePath> GetItemFiles(bool includeReferencedFiles)
		{
			List<FilePath> itemFiles = base.GetItemFiles(includeReferencedFiles);
			if (includeReferencedFiles)
			{
				foreach (SolutionEntityItem solutionEntityItem in this.GetAllSolutionItems<SolutionEntityItem>())
				{
					itemFiles.AddRange(solutionEntityItem.GetItemFiles(true));
				}
			}
			return itemFiles;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00031FF0 File Offset: 0x000301F0
		protected internal virtual void OnSolutionItemAdded(SolutionItemChangeEventArgs args)
		{
			this.solutionItems = null;
			SolutionFolder solutionFolder = args.SolutionItem as SolutionFolder;
			if (solutionFolder != null)
			{
				using (IEnumerator<SolutionItem> enumerator = solutionFolder.GetAllItems<SolutionItem>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SolutionItem item = enumerator.Current;
						this.SetupNewItem(item, null);
					}
					goto IL_59;
				}
			}
			this.SetupNewItem(args.SolutionItem, args.ReplacedItem);
			IL_59:
			if (this.SolutionItemAdded != null)
			{
				this.SolutionItemAdded(this, args);
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0003207C File Offset: 0x0003027C
		private void SetupNewItem(SolutionItem item, SolutionItem replacedItem)
		{
			this.ConvertToSolutionFormat(item, false);
			SolutionEntityItem solutionEntityItem = item as SolutionEntityItem;
			if (solutionEntityItem != null)
			{
				solutionEntityItem.NeedsReload = false;
				if (solutionEntityItem.SupportsConfigurations() || replacedItem != null)
				{
					if (replacedItem == null)
					{
						foreach (SolutionConfiguration solutionConfiguration in this.Configurations)
						{
							solutionConfiguration.AddItem(solutionEntityItem);
						}
						if (!base.Loading && (this.StartupItem == null || !this.StartupItem.SupportsExecute()) && solutionEntityItem.SupportsExecute())
						{
							this.StartupItem = solutionEntityItem;
							return;
						}
					}
					else
					{
						foreach (SolutionConfiguration solutionConfiguration2 in this.Configurations)
						{
							solutionConfiguration2.ReplaceItem((SolutionEntityItem)replacedItem, solutionEntityItem);
						}
						if (this.StartupItem == replacedItem)
						{
							this.StartupItem = solutionEntityItem;
							return;
						}
						int num = this.MultiStartupItems.IndexOf((SolutionEntityItem)replacedItem);
						if (num != -1)
						{
							this.MultiStartupItems[num] = solutionEntityItem;
						}
					}
				}
			}
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x000321A8 File Offset: 0x000303A8
		private void ConvertToSolutionFormat(SolutionItem item, bool force)
		{
			SolutionEntityItem solutionEntityItem = item as SolutionEntityItem;
			if (force || !this.FileFormat.Format.SupportsMixedFormats || solutionEntityItem == null || !solutionEntityItem.IsSaved)
			{
				this.FileFormat.Format.ConvertToFormat(item);
				if (solutionEntityItem != null)
				{
					solutionEntityItem.InstallFormat(this.FileFormat);
				}
			}
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x000321FC File Offset: 0x000303FC
		protected internal virtual void OnSolutionItemRemoved(SolutionItemChangeEventArgs args)
		{
			this.solutionItems = null;
			SolutionFolder solutionFolder = args.SolutionItem as SolutionFolder;
			if (solutionFolder != null)
			{
				using (IEnumerator<SolutionEntityItem> enumerator = solutionFolder.GetAllItems<SolutionEntityItem>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SolutionEntityItem item = enumerator.Current;
						this.DetachItem(item, args.Reloading);
					}
					goto IL_68;
				}
			}
			SolutionEntityItem solutionEntityItem = args.SolutionItem as SolutionEntityItem;
			if (solutionEntityItem != null)
			{
				this.DetachItem(solutionEntityItem, args.Reloading);
			}
			IL_68:
			if (this.SolutionItemRemoved != null)
			{
				this.SolutionItemRemoved(this, args);
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00032298 File Offset: 0x00030498
		private void DetachItem(SolutionEntityItem item, bool reloading)
		{
			item.NeedsReload = false;
			if (!reloading)
			{
				foreach (SolutionConfiguration solutionConfiguration in this.Configurations)
				{
					solutionConfiguration.RemoveItem(item);
				}
				if (item is Project)
				{
					this.RemoveReferencesToProject((Project)item);
				}
				if (this.StartupItem == item)
				{
					this.StartupItem = null;
				}
				else
				{
					this.MultiStartupItems.Remove(item);
				}
			}
			item.FileName = item.FileName;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00032330 File Offset: 0x00030530
		private void RemoveReferencesToProject(Project projectToRemove)
		{
			if (projectToRemove == null)
			{
				return;
			}
			foreach (DotNetProject dotNetProject in this.GetAllSolutionItems<DotNetProject>())
			{
				if (dotNetProject != projectToRemove)
				{
					List<ProjectReference> list = new List<ProjectReference>();
					foreach (ProjectReference projectReference in dotNetProject.References)
					{
						if (projectReference.ReferenceType == ReferenceType.Project && projectReference.Reference == projectToRemove.Name)
						{
							list.Add(projectReference);
						}
					}
					foreach (ProjectReference item in list)
					{
						dotNetProject.References.Remove(item);
					}
				}
			}
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00032434 File Offset: 0x00030634
		internal void NotifyConfigurationsChanged()
		{
			this.OnConfigurationsChanged();
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0003243C File Offset: 0x0003063C
		protected internal virtual void OnFileAddedToProject(ProjectFileEventArgs args)
		{
			if (this.FileAddedToProject != null)
			{
				this.FileAddedToProject(this, args);
			}
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00032453 File Offset: 0x00030653
		protected internal virtual void OnFileRemovedFromProject(ProjectFileEventArgs args)
		{
			if (this.FileRemovedFromProject != null)
			{
				this.FileRemovedFromProject(this, args);
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0003246A File Offset: 0x0003066A
		protected internal virtual void OnFileChangedInProject(ProjectFileEventArgs args)
		{
			if (this.FileChangedInProject != null)
			{
				this.FileChangedInProject(this, args);
			}
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00032481 File Offset: 0x00030681
		protected internal virtual void OnFilePropertyChangedInProject(ProjectFileEventArgs args)
		{
			if (this.FilePropertyChangedInProject != null)
			{
				this.FilePropertyChangedInProject(this, args);
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00032498 File Offset: 0x00030698
		protected internal virtual void OnFileRenamedInProject(ProjectFileRenamedEventArgs args)
		{
			if (this.FileRenamedInProject != null)
			{
				this.FileRenamedInProject(this, args);
			}
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x000324AF File Offset: 0x000306AF
		protected internal virtual void OnReferenceAddedToProject(ProjectReferenceEventArgs args)
		{
			if (this.ReferenceAddedToProject != null)
			{
				this.ReferenceAddedToProject(this, args);
			}
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x000324C6 File Offset: 0x000306C6
		protected internal virtual void OnReferenceRemovedFromProject(ProjectReferenceEventArgs args)
		{
			if (this.ReferenceRemovedFromProject != null)
			{
				this.ReferenceRemovedFromProject(this, args);
			}
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x000324DD File Offset: 0x000306DD
		protected internal virtual void OnEntryModified(SolutionItemModifiedEventArgs args)
		{
			if (this.EntryModified != null)
			{
				this.EntryModified(this, args);
			}
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x000324F4 File Offset: 0x000306F4
		protected internal virtual void OnEntrySaved(SolutionItemEventArgs args)
		{
			if (this.EntrySaved != null)
			{
				this.EntrySaved(this, args);
			}
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0003250B File Offset: 0x0003070B
		protected internal virtual void OnItemReloadRequired(SolutionItemEventArgs args)
		{
			if (this.ItemReloadRequired != null)
			{
				this.ItemReloadRequired(this, args);
			}
		}

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06000DB2 RID: 3506 RVA: 0x00032524 File Offset: 0x00030724
		// (remove) Token: 0x06000DB3 RID: 3507 RVA: 0x0003255C File Offset: 0x0003075C
		public event EventHandler StartupItemChanged;

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06000DB4 RID: 3508 RVA: 0x00032594 File Offset: 0x00030794
		// (remove) Token: 0x06000DB5 RID: 3509 RVA: 0x000325CC File Offset: 0x000307CC
		public event SolutionItemChangeEventHandler SolutionItemAdded;

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06000DB6 RID: 3510 RVA: 0x00032604 File Offset: 0x00030804
		// (remove) Token: 0x06000DB7 RID: 3511 RVA: 0x0003263C File Offset: 0x0003083C
		public event SolutionItemChangeEventHandler SolutionItemRemoved;

		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06000DB8 RID: 3512 RVA: 0x00032674 File Offset: 0x00030874
		// (remove) Token: 0x06000DB9 RID: 3513 RVA: 0x000326AC File Offset: 0x000308AC
		public event ProjectFileEventHandler FileAddedToProject;

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06000DBA RID: 3514 RVA: 0x000326E4 File Offset: 0x000308E4
		// (remove) Token: 0x06000DBB RID: 3515 RVA: 0x0003271C File Offset: 0x0003091C
		public event ProjectFileEventHandler FileRemovedFromProject;

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06000DBC RID: 3516 RVA: 0x00032754 File Offset: 0x00030954
		// (remove) Token: 0x06000DBD RID: 3517 RVA: 0x0003278C File Offset: 0x0003098C
		public event ProjectFileEventHandler FileChangedInProject;

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x06000DBE RID: 3518 RVA: 0x000327C4 File Offset: 0x000309C4
		// (remove) Token: 0x06000DBF RID: 3519 RVA: 0x000327FC File Offset: 0x000309FC
		public event ProjectFileEventHandler FilePropertyChangedInProject;

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x06000DC0 RID: 3520 RVA: 0x00032834 File Offset: 0x00030A34
		// (remove) Token: 0x06000DC1 RID: 3521 RVA: 0x0003286C File Offset: 0x00030A6C
		public event ProjectFileRenamedEventHandler FileRenamedInProject;

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x06000DC2 RID: 3522 RVA: 0x000328A4 File Offset: 0x00030AA4
		// (remove) Token: 0x06000DC3 RID: 3523 RVA: 0x000328DC File Offset: 0x00030ADC
		public event ProjectReferenceEventHandler ReferenceAddedToProject;

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06000DC4 RID: 3524 RVA: 0x00032914 File Offset: 0x00030B14
		// (remove) Token: 0x06000DC5 RID: 3525 RVA: 0x0003294C File Offset: 0x00030B4C
		public event ProjectReferenceEventHandler ReferenceRemovedFromProject;

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06000DC6 RID: 3526 RVA: 0x00032984 File Offset: 0x00030B84
		// (remove) Token: 0x06000DC7 RID: 3527 RVA: 0x000329BC File Offset: 0x00030BBC
		public event SolutionItemModifiedEventHandler EntryModified;

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06000DC8 RID: 3528 RVA: 0x000329F4 File Offset: 0x00030BF4
		// (remove) Token: 0x06000DC9 RID: 3529 RVA: 0x00032A2C File Offset: 0x00030C2C
		public event SolutionItemEventHandler EntrySaved;

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x06000DCA RID: 3530 RVA: 0x00032A64 File Offset: 0x00030C64
		// (remove) Token: 0x06000DCB RID: 3531 RVA: 0x00032A9C File Offset: 0x00030C9C
		public event EventHandler<SolutionItemEventArgs> ItemReloadRequired;

		// Token: 0x040003ED RID: 1005
		internal object MemoryProbe = Counters.SolutionsInMemory.CreateMemoryProbe();

		// Token: 0x040003EE RID: 1006
		private SolutionFolder rootFolder;

		// Token: 0x040003EF RID: 1007
		private string defaultConfiguration;

		// Token: 0x040003F0 RID: 1008
		private SolutionEntityItem startupItem;

		// Token: 0x040003F1 RID: 1009
		private List<SolutionEntityItem> startupItems;

		// Token: 0x040003F2 RID: 1010
		private bool singleStartup = true;

		// Token: 0x040003F3 RID: 1011
		private List<string> multiStartupItems;

		// Token: 0x040003F4 RID: 1012
		private string startItemFileName;

		// Token: 0x040003F5 RID: 1013
		private ReadOnlyCollection<SolutionItem> solutionItems;

		// Token: 0x040003F6 RID: 1014
		private SolutionConfigurationCollection configurations;

		// Token: 0x040003F7 RID: 1015
		[ItemProperty("description", DefaultValue = "")]
		private string description;

		// Token: 0x040003F8 RID: 1016
		[ItemProperty("version", DefaultValue = "0.1")]
		private string version = "0.1";

		// Token: 0x040003F9 RID: 1017
		[ProjectPathItemProperty("outputpath")]
		private string outputdir;
	}
}
