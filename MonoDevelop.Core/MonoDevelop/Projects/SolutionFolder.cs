using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Formats.MSBuild;

namespace MonoDevelop.Projects
{
	// Token: 0x02000165 RID: 357
	[DataInclude(typeof(SolutionConfiguration))]
	public class SolutionFolder : SolutionItem
	{
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x00032CA4 File Offset: 0x00030EA4
		public SolutionFolderItemCollection Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new SolutionFolderItemCollection(this);
				}
				return this.items;
			}
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00032CC0 File Offset: 0x00030EC0
		internal SolutionFolderItemCollection GetItemsWithoutCreating()
		{
			return this.items;
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x00032CC8 File Offset: 0x00030EC8
		[ItemProperty]
		[ProjectPathItemProperty("File", Scope = "*")]
		public SolutionFolderFileCollection Files
		{
			get
			{
				if (this.files == null)
				{
					this.files = new SolutionFolderFileCollection(this);
				}
				return this.files;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x00032CE4 File Offset: 0x00030EE4
		public virtual bool IsRoot
		{
			get
			{
				return base.ParentFolder == null;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x00032CEF File Offset: 0x00030EEF
		// (set) Token: 0x06000DDB RID: 3547 RVA: 0x00032D14 File Offset: 0x00030F14
		public override string Name
		{
			get
			{
				if (base.ParentFolder == null && base.ParentSolution != null)
				{
					return base.ParentSolution.Name;
				}
				return this.name;
			}
			set
			{
				if (value != this.name)
				{
					string oldName = this.name;
					this.name = value;
					this.OnNameChanged(new SolutionItemRenamedEventArgs(this, oldName, this.name));
				}
			}
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00032D50 File Offset: 0x00030F50
		protected override FilePath GetDefaultBaseDirectory()
		{
			if (base.ParentSolution == null)
			{
				return FilePath.Null;
			}
			if (base.ParentFolder == null)
			{
				return base.ParentSolution.BaseDirectory;
			}
			FilePath filePath = this.GetCommonPathRoot();
			if (!string.IsNullOrEmpty(filePath))
			{
				return filePath;
			}
			SolutionFolder solutionFolder = this;
			filePath = FilePath.Empty;
			do
			{
				filePath = filePath.Combine(new string[]
				{
					solutionFolder.Name
				});
				solutionFolder = solutionFolder.ParentFolder;
			}
			while (solutionFolder.ParentFolder != null);
			filePath = base.ParentSolution.BaseDirectory.Combine(new FilePath[]
			{
				filePath
			});
			if (!Directory.Exists(filePath))
			{
				return base.ParentFolder.BaseDirectory;
			}
			return filePath;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00032E0C File Offset: 0x0003100C
		private FilePath GetCommonPathRoot()
		{
			FilePath filePath = null;
			foreach (SolutionItem solutionItem in this.Items)
			{
				FilePath filePath2;
				if (solutionItem is SolutionFolder)
				{
					SolutionFolder solutionFolder = (SolutionFolder)solutionItem;
					if (solutionFolder.HasCustomBaseDirectory)
					{
						filePath2 = solutionFolder.BaseDirectory;
					}
					else
					{
						filePath2 = solutionFolder.GetCommonPathRoot();
					}
				}
				else
				{
					filePath2 = solutionItem.BaseDirectory;
				}
				if (filePath2.IsNullOrEmpty)
				{
					return FilePath.Null;
				}
				if (!filePath.IsNull)
				{
					filePath = this.GetCommonPathRoot(filePath, filePath2);
					if (filePath.IsNullOrEmpty)
					{
						break;
					}
				}
				else
				{
					filePath = filePath2;
				}
			}
			return filePath;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00032ED8 File Offset: 0x000310D8
		private string GetCommonPathRoot(string path1, string path2)
		{
			path1 = Path.GetFullPath(path1);
			path2 = Path.GetFullPath(path2);
			if (path1 == path2)
			{
				return path1;
			}
			path1 += Path.DirectorySeparatorChar;
			path2 += Path.DirectorySeparatorChar;
			int num = -1;
			int num2 = 0;
			while (num2 < path1.Length && num2 < path2.Length && path1[num2] == path2[num2])
			{
				if (path1[num2] == Path.DirectorySeparatorChar)
				{
					num = num2;
				}
				num2++;
			}
			if (num > 0)
			{
				return path1.Substring(0, num);
			}
			return null;
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x00032F6E File Offset: 0x0003116E
		internal override IDictionary InternalGetExtendedProperties
		{
			get
			{
				if (base.ParentSolution != null && base.ParentFolder == null)
				{
					return base.ParentSolution.ExtendedProperties;
				}
				return base.InternalGetExtendedProperties;
			}
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00032F92 File Offset: 0x00031192
		protected override void InitializeItemHandler()
		{
			this.SetItemHandler(new DummySolutionFolderHandler(this));
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00032FA0 File Offset: 0x000311A0
		public override void Dispose()
		{
			if (this.items != null)
			{
				foreach (SolutionItem solutionItem in this.items)
				{
					solutionItem.Dispose();
				}
				this.items = null;
			}
			this.files = null;
			base.Dispose();
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00033028 File Offset: 0x00031228
		public SolutionItem ReloadItem(IProgressMonitor monitor, SolutionItem sitem)
		{
			if (this.Items.IndexOf(sitem) == -1)
			{
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"Solution item '",
					sitem.Name,
					"' does not belong to folder '",
					this.Name,
					"'"
				}));
			}
			SolutionEntityItem item = sitem as SolutionEntityItem;
			if (item == null)
			{
				return sitem;
			}
			SolutionEntityItem solutionEntityItem;
			try
			{
				if (base.ParentSolution.IsSolutionItemEnabled(item.FileName))
				{
					solutionEntityItem = Services.ProjectService.ReadSolutionItem(monitor, item.FileName);
				}
				else
				{
					UnknownSolutionItem unknownSolutionItem = new UnloadedSolutionItem
					{
						FileName = item.FileName
					};
					MSBuildHandler msbuildHandler = item.GetItemHandler() as MSBuildHandler;
					if (msbuildHandler != null)
					{
						MSBuildHandler itemHandler = new MSBuildHandler(msbuildHandler.TypeGuid, msbuildHandler.ItemId)
						{
							Item = unknownSolutionItem
						};
						unknownSolutionItem.SetItemHandler(itemHandler);
					}
					solutionEntityItem = unknownSolutionItem;
				}
			}
			catch (Exception ex)
			{
				solutionEntityItem = new UnknownSolutionItem
				{
					LoadError = ex.Message,
					FileName = item.FileName
				};
			}
			if (!this.Items.Contains(item))
			{
				solutionEntityItem.Dispose();
				return this.Items.OfType<SolutionEntityItem>().FirstOrDefault((SolutionEntityItem it) => it.FileName == item.FileName);
			}
			this.Items.Replace(item, solutionEntityItem);
			this.DisconnectChildEntryEvents(item);
			this.ConnectChildEntryEvents(solutionEntityItem);
			base.NotifyModified("Items");
			this.OnItemRemoved(new SolutionItemChangeEventArgs(item, base.ParentSolution, true)
			{
				ReplacedItem = item
			}, true);
			this.OnItemAdded(new SolutionItemChangeEventArgs(solutionEntityItem, base.ParentSolution, true)
			{
				ReplacedItem = item
			}, true);
			item.Dispose();
			return solutionEntityItem;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00033258 File Offset: 0x00031458
		internal void NotifyItemAdded(SolutionItem item, bool newToSolution)
		{
			this.ConnectChildEntryEvents(item);
			base.NotifyModified("Items");
			this.OnItemAdded(new SolutionItemChangeEventArgs(item, base.ParentSolution, false), newToSolution);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00033280 File Offset: 0x00031480
		private void ConnectChildEntryEvents(SolutionItem item)
		{
			if (item is Project)
			{
				Project project = item as Project;
				project.FileRemovedFromProject += this.NotifyFileRemovedFromProject;
				project.FileAddedToProject += this.NotifyFileAddedToProject;
				project.FileChangedInProject += this.NotifyFileChangedInProject;
				project.FilePropertyChangedInProject += this.NotifyFilePropertyChangedInProject;
				project.FileRenamedInProject += this.NotifyFileRenamedInProject;
				if (item is DotNetProject)
				{
					((DotNetProject)project).ReferenceRemovedFromProject += this.NotifyReferenceRemovedFromProject;
					((DotNetProject)project).ReferenceAddedToProject += this.NotifyReferenceAddedToProject;
				}
			}
			if (item is SolutionFolder)
			{
				SolutionFolder solutionFolder = item as SolutionFolder;
				solutionFolder.FileRemovedFromProject += this.NotifyFileRemovedFromProject;
				solutionFolder.FileAddedToProject += this.NotifyFileAddedToProject;
				solutionFolder.FileChangedInProject += this.NotifyFileChangedInProject;
				solutionFolder.FilePropertyChangedInProject += this.NotifyFilePropertyChangedInProject;
				solutionFolder.FileRenamedInProject += this.NotifyFileRenamedInProject;
				solutionFolder.ReferenceRemovedFromProject += this.NotifyReferenceRemovedFromProject;
				solutionFolder.ReferenceAddedToProject += this.NotifyReferenceAddedToProject;
			}
			if (item is SolutionEntityItem)
			{
				((SolutionEntityItem)item).Saved += this.NotifyItemSaved;
			}
			item.Modified += this.NotifyItemModified;
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x000333F0 File Offset: 0x000315F0
		public override void Save(IProgressMonitor monitor)
		{
			foreach (SolutionItem solutionItem in this.Items)
			{
				solutionItem.Save(monitor);
			}
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00033440 File Offset: 0x00031640
		public SolutionEntityItem AddItem(IProgressMonitor monitor, string filename)
		{
			return this.AddItem(monitor, filename, false);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0003344C File Offset: 0x0003164C
		public SolutionEntityItem AddItem(IProgressMonitor monitor, string filename, bool createSolutionConfigurations)
		{
			if (monitor == null)
			{
				monitor = new NullProgressMonitor();
			}
			SolutionEntityItem solutionEntityItem = Services.ProjectService.ReadSolutionItem(monitor, filename);
			this.AddItem(solutionEntityItem, createSolutionConfigurations);
			return solutionEntityItem;
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00033479 File Offset: 0x00031679
		public void AddItem(SolutionItem item)
		{
			this.AddItem(item, false);
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00033484 File Offset: 0x00031684
		public void AddItem(SolutionItem item, bool createSolutionConfigurations)
		{
			this.Items.Add(item);
			SolutionEntityItem solutionEntityItem = item as SolutionEntityItem;
			if (solutionEntityItem != null && createSolutionConfigurations && solutionEntityItem.SupportsBuild())
			{
				foreach (ItemConfiguration itemConfiguration in solutionEntityItem.Configurations)
				{
					bool flag = false;
					foreach (SolutionConfiguration solutionConfiguration in base.ParentSolution.Configurations)
					{
						if (solutionConfiguration.Name == itemConfiguration.Name && (itemConfiguration.Platform == solutionConfiguration.Platform || itemConfiguration.Platform.Length == 0))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						SolutionConfiguration solutionConfiguration2 = new SolutionConfiguration(itemConfiguration.Id);
						foreach (SolutionEntityItem item2 in base.ParentSolution.GetAllSolutionItems<SolutionEntityItem>())
						{
							solutionConfiguration2.AddItem(item2);
						}
						base.ParentSolution.Configurations.Add(solutionConfiguration2);
					}
				}
			}
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x000335E4 File Offset: 0x000317E4
		internal void NotifyItemRemoved(SolutionItem item, bool removedFromSolution)
		{
			this.DisconnectChildEntryEvents(item);
			base.NotifyModified("Items");
			this.OnItemRemoved(new SolutionItemChangeEventArgs(item, base.ParentSolution, false), removedFromSolution);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0003360C File Offset: 0x0003180C
		private void DisconnectChildEntryEvents(SolutionItem entry)
		{
			if (entry is Project)
			{
				Project project = entry as Project;
				project.FileRemovedFromProject -= this.NotifyFileRemovedFromProject;
				project.FileAddedToProject -= this.NotifyFileAddedToProject;
				project.FileChangedInProject -= this.NotifyFileChangedInProject;
				project.FilePropertyChangedInProject -= this.NotifyFilePropertyChangedInProject;
				project.FileRenamedInProject -= this.NotifyFileRenamedInProject;
				if (project is DotNetProject)
				{
					((DotNetProject)project).ReferenceRemovedFromProject -= this.NotifyReferenceRemovedFromProject;
					((DotNetProject)project).ReferenceAddedToProject -= this.NotifyReferenceAddedToProject;
				}
			}
			if (entry is SolutionFolder)
			{
				SolutionFolder solutionFolder = entry as SolutionFolder;
				solutionFolder.FileRemovedFromProject -= this.NotifyFileRemovedFromProject;
				solutionFolder.FileAddedToProject -= this.NotifyFileAddedToProject;
				solutionFolder.FileChangedInProject -= this.NotifyFileChangedInProject;
				solutionFolder.FilePropertyChangedInProject -= this.NotifyFilePropertyChangedInProject;
				solutionFolder.FileRenamedInProject -= this.NotifyFileRenamedInProject;
				solutionFolder.ReferenceRemovedFromProject -= this.NotifyReferenceRemovedFromProject;
				solutionFolder.ReferenceAddedToProject -= this.NotifyReferenceAddedToProject;
			}
			if (entry is SolutionEntityItem)
			{
				((SolutionEntityItem)entry).Saved -= this.NotifyItemSaved;
			}
			entry.Modified -= this.NotifyItemModified;
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0003377C File Offset: 0x0003197C
		protected internal override void OnExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
		}

		/// <remarks>
		/// Returns a collection containing all entries in this folder and 
		/// undercombines
		/// </remarks>
		// Token: 0x06000DED RID: 3565 RVA: 0x0003377E File Offset: 0x0003197E
		public ReadOnlyCollection<SolutionItem> GetAllItems()
		{
			return this.GetAllItems<SolutionItem>();
		}

		/// <remarks>
		/// Returns a collection containing all entries of the given type in this folder and 
		/// undercombines
		/// </remarks>
		// Token: 0x06000DEE RID: 3566 RVA: 0x00033788 File Offset: 0x00031988
		public ReadOnlyCollection<T> GetAllItems<T>() where T : SolutionItem
		{
			List<T> list = new List<T>();
			this.GetAllItems<T>(list, this);
			return list.AsReadOnly();
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x000337AC File Offset: 0x000319AC
		public ReadOnlyCollection<T> GetAllItemsWithTopologicalSort<T>(ConfigurationSelector configuration) where T : SolutionItem
		{
			List<T> list = new List<T>();
			this.GetAllItems<T>(list, this);
			return SolutionItem.TopologicalSort<T>(list, configuration);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x000337D0 File Offset: 0x000319D0
		public ReadOnlyCollection<Project> GetAllProjects()
		{
			List<Project> list = new List<Project>();
			this.GetAllItems<Project>(list, this);
			return list.AsReadOnly();
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x000337F4 File Offset: 0x000319F4
		public ReadOnlyCollection<Project> GetAllProjectsWithTopologicalSort(ConfigurationSelector configuration)
		{
			List<Project> list = new List<Project>();
			this.GetAllItems<Project>(list, this);
			return SolutionItem.TopologicalSort<Project>(list, configuration);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00033818 File Offset: 0x00031A18
		private void GetAllItems<T>(List<T> list, SolutionItem item) where T : SolutionItem
		{
			if (item is T)
			{
				list.Add((T)((object)item));
			}
			if (item is SolutionFolder)
			{
				foreach (SolutionItem item2 in ((SolutionFolder)item).Items)
				{
					this.GetAllItems<T>(list, item2);
				}
			}
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00033888 File Offset: 0x00031A88
		public ReadOnlyCollection<SolutionItem> GetAllBuildableEntries(ConfigurationSelector configuration, bool topologicalSort, bool includeExternalReferences)
		{
			List<SolutionItem> list = new List<SolutionItem>();
			this.GetAllBuildableEntries(list, configuration, includeExternalReferences);
			if (topologicalSort)
			{
				return SolutionItem.TopologicalSort<SolutionItem>(list, configuration);
			}
			return list.AsReadOnly();
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x000338B5 File Offset: 0x00031AB5
		public ReadOnlyCollection<SolutionItem> GetAllBuildableEntries(ConfigurationSelector configuration)
		{
			return this.GetAllBuildableEntries(configuration, false, false);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x000338C0 File Offset: 0x00031AC0
		private void GetAllBuildableEntries(List<SolutionItem> list, ConfigurationSelector configuration, bool includeExternalReferences)
		{
			if (base.ParentSolution == null)
			{
				return;
			}
			SolutionConfiguration configuration2 = base.ParentSolution.GetConfiguration(configuration);
			if (configuration2 == null)
			{
				return;
			}
			foreach (SolutionItem solutionItem in this.Items)
			{
				if (solutionItem is SolutionFolder)
				{
					((SolutionFolder)solutionItem).GetAllBuildableEntries(list, configuration, includeExternalReferences);
				}
				else if (solutionItem is SolutionEntityItem && configuration2.BuildEnabledForItem((SolutionEntityItem)solutionItem) && solutionItem.SupportsBuild())
				{
					this.GetAllBuildableReferences(list, solutionItem, configuration, includeExternalReferences);
				}
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00033960 File Offset: 0x00031B60
		private void GetAllBuildableReferences(List<SolutionItem> list, SolutionItem item, ConfigurationSelector configuration, bool includeExternalReferences)
		{
			if (list.Contains(item))
			{
				return;
			}
			list.Add(item);
			if (includeExternalReferences)
			{
				foreach (SolutionItem item2 in item.GetReferencedItems(configuration))
				{
					this.GetAllBuildableReferences(list, item2, configuration, includeExternalReferences);
				}
			}
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x000339C8 File Offset: 0x00031BC8
		[Obsolete("Use GetProjectsContainingFile() (plural) instead")]
		public Project GetProjectContainingFile(string fileName)
		{
			ReadOnlyCollection<Project> allProjects = this.GetAllProjects();
			foreach (Project project in allProjects)
			{
				if (project.FileName == fileName || project.IsFileInProject(fileName))
				{
					return project;
				}
			}
			return null;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00033CC0 File Offset: 0x00031EC0
		public IEnumerable<Project> GetProjectsContainingFile(string fileName)
		{
			ReadOnlyCollection<Project> projects = this.GetAllProjects();
			Project mainProject = null;
			List<Project> projectsWithLinks = new List<Project>();
			foreach (Project project2 in projects)
			{
				if (project2.FileName == fileName || project2.IsFileInProject(fileName))
				{
					string directoryName = Path.GetDirectoryName(project2.FileName);
					if (fileName.StartsWith(directoryName))
					{
						mainProject = project2;
					}
					else
					{
						projectsWithLinks.Add(project2);
					}
				}
			}
			if (mainProject != null)
			{
				yield return mainProject;
			}
			foreach (Project project in projectsWithLinks)
			{
				yield return project;
			}
			yield break;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00033CE4 File Offset: 0x00031EE4
		public SolutionEntityItem FindSolutionItem(string fileName)
		{
			string fullPath = Path.GetFullPath(fileName);
			foreach (SolutionItem solutionItem in this.Items)
			{
				if (solutionItem is SolutionFolder)
				{
					SolutionEntityItem solutionEntityItem = ((SolutionFolder)solutionItem).FindSolutionItem(fileName);
					if (solutionEntityItem != null)
					{
						return solutionEntityItem;
					}
				}
				else if (solutionItem is SolutionEntityItem)
				{
					SolutionEntityItem solutionEntityItem2 = (SolutionEntityItem)solutionItem;
					if (!string.IsNullOrEmpty(solutionEntityItem2.FileName) && fullPath == Path.GetFullPath(solutionEntityItem2.FileName))
					{
						return (SolutionEntityItem)solutionItem;
					}
				}
			}
			return null;
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00033D9C File Offset: 0x00031F9C
		public Project FindProjectByName(string name)
		{
			foreach (SolutionItem solutionItem in this.Items)
			{
				if (solutionItem is SolutionFolder)
				{
					Project project = ((SolutionFolder)solutionItem).FindProjectByName(name);
					if (project != null)
					{
						return project;
					}
				}
				else if (solutionItem is Project && name == solutionItem.Name)
				{
					return (Project)solutionItem;
				}
			}
			return null;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00033E20 File Offset: 0x00032020
		protected internal override BuildResult OnRunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
		{
			if (target == "Build")
			{
				return this.OnBuild(monitor, configuration);
			}
			if (target == "Clean")
			{
				this.OnClean(monitor, configuration);
				return new BuildResult();
			}
			ReadOnlyCollection<SolutionItem> allBuildableEntries;
			try
			{
				allBuildableEntries = this.GetAllBuildableEntries(configuration, true, true);
			}
			catch (CyclicDependencyException)
			{
				monitor.ReportError(GettextCatalog.GetString("Cyclic dependencies are not supported."), null);
				return new BuildResult("", 1, 1);
			}
			BuildResult result;
			try
			{
				monitor.BeginTask(GettextCatalog.GetString("Building Solution: {0} ({1})", this.Name, configuration.ToString()), allBuildableEntries.Count);
				BuildResult buildResult = new BuildResult();
				buildResult.BuildCount = 0;
				HashSet<SolutionItem> hashSet = new HashSet<SolutionItem>();
				foreach (SolutionItem solutionItem in allBuildableEntries)
				{
					if (monitor.IsCancelRequested)
					{
						break;
					}
					if (!solutionItem.ContainsReferences(hashSet, configuration))
					{
						BuildResult buildResult2 = solutionItem.RunTarget(monitor, target, configuration);
						if (buildResult2 != null)
						{
							buildResult.Append(buildResult2);
							if (buildResult2.ErrorCount > 0)
							{
								hashSet.Add(solutionItem);
							}
						}
					}
					else
					{
						hashSet.Add(solutionItem);
					}
					monitor.Step(1);
				}
				result = buildResult;
			}
			finally
			{
				monitor.EndTask();
			}
			return result;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00033F70 File Offset: 0x00032170
		protected override void OnClean(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			if (base.ParentSolution == null)
			{
				return;
			}
			SolutionConfiguration configuration2 = base.ParentSolution.GetConfiguration(configuration);
			if (configuration2 == null)
			{
				return;
			}
			try
			{
				monitor.BeginTask(GettextCatalog.GetString("Cleaning Solution: {0} ({1})", this.Name, configuration.ToString()), this.Items.Count);
				foreach (SolutionItem solutionItem in this.Items)
				{
					if (solutionItem is SolutionFolder)
					{
						solutionItem.Clean(monitor, configuration);
					}
					else if (solutionItem is SolutionEntityItem)
					{
						SolutionEntityItem solutionEntityItem = (SolutionEntityItem)solutionItem;
						SolutionConfigurationEntry entryForItem = configuration2.GetEntryForItem(solutionEntityItem);
						if (entryForItem != null && entryForItem.Build)
						{
							solutionEntityItem.Clean(monitor, entryForItem.ItemConfigurationSelector);
						}
					}
					else
					{
						solutionItem.Clean(monitor, configuration);
					}
					monitor.Step(1);
				}
			}
			finally
			{
				monitor.EndTask();
			}
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00034064 File Offset: 0x00032264
		protected override BuildResult OnBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			ReadOnlyCollection<SolutionItem> allBuildableEntries;
			try
			{
				allBuildableEntries = this.GetAllBuildableEntries(configuration, true, true);
			}
			catch (CyclicDependencyException)
			{
				monitor.ReportError(GettextCatalog.GetString("Cyclic dependencies are not supported."), null);
				return new BuildResult("", 1, 1);
			}
			BuildResult result;
			try
			{
				List<SolutionItem> list = new List<SolutionItem>(allBuildableEntries);
				monitor.BeginTask(GettextCatalog.GetString("Building Solution: {0} ({1})", this.Name, configuration.ToString()), list.Count);
				BuildResult buildResult = new BuildResult();
				buildResult.BuildCount = 0;
				HashSet<SolutionItem> hashSet = new HashSet<SolutionItem>();
				foreach (SolutionItem solutionItem in list)
				{
					if (monitor.IsCancelRequested)
					{
						break;
					}
					if (!solutionItem.ContainsReferences(hashSet, configuration))
					{
						BuildResult buildResult2 = solutionItem.Build(monitor, configuration, false);
						if (buildResult2 != null)
						{
							buildResult.Append(buildResult2);
							if (buildResult2.ErrorCount > 0)
							{
								hashSet.Add(solutionItem);
							}
						}
					}
					else
					{
						hashSet.Add(solutionItem);
					}
					monitor.Step(1);
				}
				result = buildResult;
			}
			finally
			{
				monitor.EndTask();
			}
			return result;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00034194 File Offset: 0x00032394
		protected internal override DateTime OnGetLastBuildTime(ConfigurationSelector configuration)
		{
			DateTime dateTime = DateTime.MaxValue;
			foreach (SolutionItem solutionItem in this.Items)
			{
				DateTime lastBuildTime = solutionItem.GetLastBuildTime(configuration);
				if (lastBuildTime < dateTime)
				{
					dateTime = lastBuildTime;
				}
			}
			return dateTime;
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x000341F4 File Offset: 0x000323F4
		public void RemoveFileFromProjects(string fileName)
		{
			if (Directory.Exists(fileName))
			{
				this.RemoveAllInDirectory(fileName);
				return;
			}
			this.RemoveFileFromAllProjects(fileName);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00034210 File Offset: 0x00032410
		private void RemoveAllInDirectory(string dirName)
		{
			foreach (Project project in this.GetAllProjects())
			{
				foreach (ProjectFile item in project.Files.GetFilesInPath(dirName))
				{
					project.Files.Remove(item);
				}
			}
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0003428C File Offset: 0x0003248C
		private void RemoveFileFromAllProjects(string fileName)
		{
			foreach (SolutionFolder solutionFolder in this.GetAllItems<SolutionFolder>())
			{
				solutionFolder.Files.Remove(fileName);
			}
			foreach (Project project in this.GetAllProjects())
			{
				List<ProjectFile> list = new List<ProjectFile>();
				foreach (ProjectFile projectFile in project.Files)
				{
					if (projectFile.Name == fileName)
					{
						list.Add(projectFile);
					}
				}
				foreach (ProjectFile item in list)
				{
					project.Files.Remove(item);
				}
			}
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x000343C4 File Offset: 0x000325C4
		public void RenameFileInProjects(FilePath sourceFile, FilePath targetFile)
		{
			if (Directory.Exists(targetFile))
			{
				this.RenameDirectoryInAllProjects(sourceFile, targetFile);
				return;
			}
			this.RenameFileInAllProjects(sourceFile, targetFile);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x000343E4 File Offset: 0x000325E4
		private void RenameFileInAllProjects(FilePath oldName, FilePath newName)
		{
			foreach (Project project in this.GetAllProjects())
			{
				foreach (ProjectFile projectFile in project.Files)
				{
					if (projectFile.FilePath == oldName)
					{
						if (projectFile.BuildAction == project.GetDefaultBuildAction(oldName))
						{
							projectFile.BuildAction = project.GetDefaultBuildAction(newName);
						}
						projectFile.Name = newName;
					}
				}
			}
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x000344A4 File Offset: 0x000326A4
		private void RenameDirectoryInAllProjects(FilePath oldName, FilePath newName)
		{
			foreach (Project project in this.GetAllProjects())
			{
				foreach (ProjectFile projectFile in project.Files)
				{
					if (projectFile.FilePath == oldName)
					{
						projectFile.Name = newName;
					}
					else if (projectFile.FilePath.IsChildPathOf(oldName))
					{
						projectFile.Name = newName.Combine(new FilePath[]
						{
							projectFile.FilePath.ToRelative(oldName)
						});
					}
					else if (projectFile.IsLink)
					{
						FilePath filePath = project.BaseDirectory.Combine(new FilePath[]
						{
							projectFile.ProjectVirtualPath
						});
						if (filePath.IsChildPathOf(oldName))
						{
							projectFile.Link = newName.ToRelative(project.BaseDirectory).Combine(new FilePath[]
							{
								filePath.ToRelative(oldName)
							});
						}
					}
				}
			}
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00034630 File Offset: 0x00032830
		internal void NotifyFileRemovedFromProject(object sender, ProjectFileEventArgs e)
		{
			this.OnFileRemovedFromProject(e);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00034639 File Offset: 0x00032839
		internal void NotifyFileAddedToProject(object sender, ProjectFileEventArgs e)
		{
			this.OnFileAddedToProject(e);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00034642 File Offset: 0x00032842
		internal void NotifyFileChangedInProject(object sender, ProjectFileEventArgs e)
		{
			this.OnFileChangedInProject(e);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0003464B File Offset: 0x0003284B
		internal void NotifyFilePropertyChangedInProject(object sender, ProjectFileEventArgs e)
		{
			this.OnFilePropertyChangedInProject(e);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00034654 File Offset: 0x00032854
		internal void NotifyFileRenamedInProject(object sender, ProjectFileRenamedEventArgs e)
		{
			this.OnFileRenamedInProject(e);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0003465D File Offset: 0x0003285D
		internal void NotifyReferenceRemovedFromProject(object sender, ProjectReferenceEventArgs e)
		{
			this.OnReferenceRemovedFromProject(e);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00034666 File Offset: 0x00032866
		internal void NotifyReferenceAddedToProject(object sender, ProjectReferenceEventArgs e)
		{
			this.OnReferenceAddedToProject(e);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0003466F File Offset: 0x0003286F
		internal void NotifyItemModified(object sender, SolutionItemModifiedEventArgs e)
		{
			this.OnItemModified(e);
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00034678 File Offset: 0x00032878
		internal void NotifyItemSaved(object sender, SolutionItemEventArgs e)
		{
			this.OnItemSaved(e);
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00034684 File Offset: 0x00032884
		internal void NotifyItemAddedToFolder(object sender, SolutionItemChangeEventArgs e, bool newToSolution)
		{
			if (base.ParentFolder != null)
			{
				base.ParentFolder.NotifyItemAddedToFolder(sender, e, newToSolution);
			}
			else if (base.ParentSolution != null && newToSolution)
			{
				base.ParentSolution.OnSolutionItemAdded(e);
			}
			if (this.DescendantItemAdded != null)
			{
				this.DescendantItemAdded(sender, e);
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x000346D8 File Offset: 0x000328D8
		internal void NotifyItemRemovedFromFolder(object sender, SolutionItemChangeEventArgs e, bool removedFromSolution)
		{
			if (this.DescendantItemRemoved != null)
			{
				this.DescendantItemRemoved(sender, e);
			}
			if (base.ParentFolder != null)
			{
				base.ParentFolder.NotifyItemRemovedFromFolder(sender, e, removedFromSolution);
				return;
			}
			if (base.ParentSolution != null && removedFromSolution)
			{
				base.ParentSolution.OnSolutionItemRemoved(e);
			}
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00034728 File Offset: 0x00032928
		internal void NotifyFilesAdded(params FilePath[] files)
		{
			foreach (FilePath file in files)
			{
				this.OnSolutionItemFileAdded(new SolutionItemFileEventArgs(file));
			}
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00034760 File Offset: 0x00032960
		internal void NotifyFilesRemoved(params FilePath[] files)
		{
			foreach (FilePath file in files)
			{
				this.OnSolutionItemFileRemoved(new SolutionItemFileEventArgs(file));
			}
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00034796 File Offset: 0x00032996
		private void OnItemAdded(SolutionItemChangeEventArgs e, bool newToSolution)
		{
			this.NotifyItemAddedToFolder(this, e, newToSolution);
			this.OnItemAdded(e);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000347A8 File Offset: 0x000329A8
		protected virtual void OnItemAdded(SolutionItemChangeEventArgs e)
		{
			if (this.ItemAdded != null)
			{
				this.ItemAdded(this, e);
			}
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x000347BF File Offset: 0x000329BF
		private void OnItemRemoved(SolutionItemChangeEventArgs e, bool removedFromSolution)
		{
			this.OnItemRemoved(e);
			this.NotifyItemRemovedFromFolder(this, e, removedFromSolution);
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x000347D1 File Offset: 0x000329D1
		protected virtual void OnItemRemoved(SolutionItemChangeEventArgs e)
		{
			if (this.ItemRemoved != null)
			{
				this.ItemRemoved(this, e);
			}
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x000347E8 File Offset: 0x000329E8
		protected virtual void OnFileRemovedFromProject(ProjectFileEventArgs e)
		{
			if (base.ParentFolder == null && base.ParentSolution != null)
			{
				base.ParentSolution.OnFileRemovedFromProject(e);
			}
			if (this.FileRemovedFromProject != null)
			{
				this.FileRemovedFromProject(this, e);
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0003481B File Offset: 0x00032A1B
		protected virtual void OnFileChangedInProject(ProjectFileEventArgs e)
		{
			if (base.ParentFolder == null && base.ParentSolution != null)
			{
				base.ParentSolution.OnFileChangedInProject(e);
			}
			if (this.FileChangedInProject != null)
			{
				this.FileChangedInProject(this, e);
			}
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0003484E File Offset: 0x00032A4E
		protected virtual void OnFilePropertyChangedInProject(ProjectFileEventArgs e)
		{
			if (base.ParentFolder == null && base.ParentSolution != null)
			{
				base.ParentSolution.OnFilePropertyChangedInProject(e);
			}
			if (this.FilePropertyChangedInProject != null)
			{
				this.FilePropertyChangedInProject(this, e);
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00034881 File Offset: 0x00032A81
		protected virtual void OnFileAddedToProject(ProjectFileEventArgs e)
		{
			if (base.ParentFolder == null && base.ParentSolution != null)
			{
				base.ParentSolution.OnFileAddedToProject(e);
			}
			if (this.FileAddedToProject != null)
			{
				this.FileAddedToProject(this, e);
			}
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x000348B4 File Offset: 0x00032AB4
		protected virtual void OnFileRenamedInProject(ProjectFileRenamedEventArgs e)
		{
			if (base.ParentFolder == null && base.ParentSolution != null)
			{
				base.ParentSolution.OnFileRenamedInProject(e);
			}
			if (this.FileRenamedInProject != null)
			{
				this.FileRenamedInProject(this, e);
			}
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x000348E7 File Offset: 0x00032AE7
		protected virtual void OnReferenceRemovedFromProject(ProjectReferenceEventArgs e)
		{
			if (base.ParentFolder == null && base.ParentSolution != null)
			{
				base.ParentSolution.OnReferenceRemovedFromProject(e);
			}
			if (this.ReferenceRemovedFromProject != null)
			{
				this.ReferenceRemovedFromProject(this, e);
			}
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0003491A File Offset: 0x00032B1A
		protected virtual void OnReferenceAddedToProject(ProjectReferenceEventArgs e)
		{
			if (base.ParentFolder == null && base.ParentSolution != null)
			{
				base.ParentSolution.OnReferenceAddedToProject(e);
			}
			if (this.ReferenceAddedToProject != null)
			{
				this.ReferenceAddedToProject(this, e);
			}
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0003494D File Offset: 0x00032B4D
		protected virtual void OnItemModified(SolutionItemModifiedEventArgs e)
		{
			if (base.ParentFolder == null && base.ParentSolution != null)
			{
				base.ParentSolution.OnEntryModified(e);
			}
			if (this.ItemModified != null)
			{
				this.ItemModified(this, e);
			}
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00034980 File Offset: 0x00032B80
		protected virtual void OnItemSaved(SolutionItemEventArgs e)
		{
			if (base.ParentFolder == null && base.ParentSolution != null)
			{
				base.ParentSolution.OnEntrySaved(e);
			}
			if (this.ItemSaved != null)
			{
				this.ItemSaved(this, e);
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x000349B3 File Offset: 0x00032BB3
		protected virtual void OnSolutionItemFileAdded(SolutionItemFileEventArgs args)
		{
			if (this.SolutionItemFileAdded != null)
			{
				this.SolutionItemFileAdded(this, args);
			}
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x000349CA File Offset: 0x00032BCA
		protected virtual void OnSolutionItemFileRemoved(SolutionItemFileEventArgs args)
		{
			if (this.SolutionItemFileRemoved != null)
			{
				this.SolutionItemFileRemoved(this, args);
			}
		}

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x06000E21 RID: 3617 RVA: 0x000349E4 File Offset: 0x00032BE4
		// (remove) Token: 0x06000E22 RID: 3618 RVA: 0x00034A1C File Offset: 0x00032C1C
		public event SolutionItemChangeEventHandler ItemAdded;

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06000E23 RID: 3619 RVA: 0x00034A54 File Offset: 0x00032C54
		// (remove) Token: 0x06000E24 RID: 3620 RVA: 0x00034A8C File Offset: 0x00032C8C
		public event SolutionItemChangeEventHandler ItemRemoved;

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06000E25 RID: 3621 RVA: 0x00034AC4 File Offset: 0x00032CC4
		// (remove) Token: 0x06000E26 RID: 3622 RVA: 0x00034AFC File Offset: 0x00032CFC
		public event SolutionItemChangeEventHandler DescendantItemAdded;

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06000E27 RID: 3623 RVA: 0x00034B34 File Offset: 0x00032D34
		// (remove) Token: 0x06000E28 RID: 3624 RVA: 0x00034B6C File Offset: 0x00032D6C
		public event SolutionItemChangeEventHandler DescendantItemRemoved;

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06000E29 RID: 3625 RVA: 0x00034BA4 File Offset: 0x00032DA4
		// (remove) Token: 0x06000E2A RID: 3626 RVA: 0x00034BDC File Offset: 0x00032DDC
		public event ProjectFileEventHandler FileAddedToProject;

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06000E2B RID: 3627 RVA: 0x00034C14 File Offset: 0x00032E14
		// (remove) Token: 0x06000E2C RID: 3628 RVA: 0x00034C4C File Offset: 0x00032E4C
		public event ProjectFileEventHandler FileRemovedFromProject;

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x06000E2D RID: 3629 RVA: 0x00034C84 File Offset: 0x00032E84
		// (remove) Token: 0x06000E2E RID: 3630 RVA: 0x00034CBC File Offset: 0x00032EBC
		public event ProjectFileEventHandler FileChangedInProject;

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x06000E2F RID: 3631 RVA: 0x00034CF4 File Offset: 0x00032EF4
		// (remove) Token: 0x06000E30 RID: 3632 RVA: 0x00034D2C File Offset: 0x00032F2C
		public event ProjectFileEventHandler FilePropertyChangedInProject;

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x06000E31 RID: 3633 RVA: 0x00034D64 File Offset: 0x00032F64
		// (remove) Token: 0x06000E32 RID: 3634 RVA: 0x00034D9C File Offset: 0x00032F9C
		public event ProjectFileRenamedEventHandler FileRenamedInProject;

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06000E33 RID: 3635 RVA: 0x00034DD4 File Offset: 0x00032FD4
		// (remove) Token: 0x06000E34 RID: 3636 RVA: 0x00034E0C File Offset: 0x0003300C
		public event ProjectReferenceEventHandler ReferenceAddedToProject;

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x06000E35 RID: 3637 RVA: 0x00034E44 File Offset: 0x00033044
		// (remove) Token: 0x06000E36 RID: 3638 RVA: 0x00034E7C File Offset: 0x0003307C
		public event ProjectReferenceEventHandler ReferenceRemovedFromProject;

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x06000E37 RID: 3639 RVA: 0x00034EB4 File Offset: 0x000330B4
		// (remove) Token: 0x06000E38 RID: 3640 RVA: 0x00034EEC File Offset: 0x000330EC
		public event SolutionItemModifiedEventHandler ItemModified;

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06000E39 RID: 3641 RVA: 0x00034F24 File Offset: 0x00033124
		// (remove) Token: 0x06000E3A RID: 3642 RVA: 0x00034F5C File Offset: 0x0003315C
		public event SolutionItemEventHandler ItemSaved;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06000E3B RID: 3643 RVA: 0x00034F94 File Offset: 0x00033194
		// (remove) Token: 0x06000E3C RID: 3644 RVA: 0x00034FCC File Offset: 0x000331CC
		public event EventHandler<SolutionItemFileEventArgs> SolutionItemFileAdded;

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06000E3D RID: 3645 RVA: 0x00035004 File Offset: 0x00033204
		// (remove) Token: 0x06000E3E RID: 3646 RVA: 0x0003503C File Offset: 0x0003323C
		public event EventHandler<SolutionItemFileEventArgs> SolutionItemFileRemoved;

		// Token: 0x0400040B RID: 1035
		private SolutionFolderItemCollection items;

		// Token: 0x0400040C RID: 1036
		private SolutionFolderFileCollection files;

		// Token: 0x0400040D RID: 1037
		private string name;
	}
}
