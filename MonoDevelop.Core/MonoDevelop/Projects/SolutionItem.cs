using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using MonoDevelop.Core;
using MonoDevelop.Core.Collections;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.Instrumentation;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Core.StringParsing;
using MonoDevelop.Projects.Extensions;
using MonoDevelop.Projects.Policies;

namespace MonoDevelop.Projects
{
	// Token: 0x0200010C RID: 268
	public abstract class SolutionItem : IBuildTarget, IWorkspaceObject, IExtendedDataItem, IFolderItem, IDisposable, ILoadController, IPolicyProvider
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x000261E2 File Offset: 0x000243E2
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x000261EA File Offset: 0x000243EA
		[ItemProperty("UseMSBuildEngine")]
		public bool? UseMSBuildEngine { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MonoDevelop.Projects.SolutionItem" /> class.
		/// </summary>
		// Token: 0x0600099E RID: 2462 RVA: 0x000261F3 File Offset: 0x000243F3
		public SolutionItem()
		{
			ProjectExtensionUtil.LoadControl(this);
		}

		/// <summary>
		/// Initializes a new instance of this item, using an xml element as template
		/// </summary>
		/// <param name="template">
		/// The template
		/// </param>
		// Token: 0x0600099F RID: 2463 RVA: 0x00026201 File Offset: 0x00024401
		public virtual void InitializeFromTemplate(XmlElement template)
		{
		}

		/// <summary>
		/// Gets the handler for this solution item
		/// </summary>
		/// <value>
		/// The solution item handler.
		/// </value>
		/// <exception cref="T:System.InvalidOperationException">
		/// Is thrown if there isn't a ISolutionItemHandler for this solution item
		/// </exception>
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x00026203 File Offset: 0x00024403
		protected internal ISolutionItemHandler ItemHandler
		{
			get
			{
				if (this.handler == null)
				{
					this.InitializeItemHandler();
					if (this.handler == null)
					{
						throw new InvalidOperationException("No handler found for solution item of type: " + base.GetType());
					}
				}
				return this.handler;
			}
		}

		/// <summary>
		/// Sets the handler for this solution item
		/// </summary>
		/// <param name="handler">
		/// A handler.
		/// </param>
		// Token: 0x060009A1 RID: 2465 RVA: 0x00026237 File Offset: 0x00024437
		internal virtual void SetItemHandler(ISolutionItemHandler handler)
		{
			if (this.handler != null)
			{
				this.handler.Dispose();
			}
			this.handler = handler;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00026253 File Offset: 0x00024453
		internal ISolutionItemHandler GetItemHandler()
		{
			return this.handler;
		}

		/// <summary>
		/// Gets the author information for this solution item, inherited from the solution and global settings.
		/// </summary>
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0002625B File Offset: 0x0002445B
		public AuthorInformation AuthorInformation
		{
			get
			{
				if (this.ParentSolution != null)
				{
					return this.ParentSolution.AuthorInformation;
				}
				return AuthorInformation.Default;
			}
		}

		/// <summary>
		/// Gets a service instance of a given type
		/// </summary>
		/// <returns>
		/// The service.
		/// </returns>
		/// <typeparam name="T">
		/// Type of the service
		/// </typeparam>
		/// <remarks>
		/// This method looks for an imlpementation of a service of the given type.
		/// </remarks>
		// Token: 0x060009A4 RID: 2468 RVA: 0x00026276 File Offset: 0x00024476
		public T GetService<T>() where T : class
		{
			return (T)((object)this.GetService(typeof(T)));
		}

		/// <summary>
		/// Gets a service instance of a given type
		/// </summary>
		/// <returns>
		/// The service.
		/// </returns>
		/// <param name="t">
		/// Type of the service
		/// </param>
		/// <remarks>
		/// This method looks for an imlpementation of a service of the given type.
		/// </remarks>
		// Token: 0x060009A5 RID: 2469 RVA: 0x0002628D File Offset: 0x0002448D
		public virtual object GetService(Type t)
		{
			return Services.ProjectService.GetExtensionChain(this).GetService(this, t);
		}

		/// <summary>
		/// Gets the solution to which this item belongs
		/// </summary>
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x000262A1 File Offset: 0x000244A1
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x000262BD File Offset: 0x000244BD
		public Solution ParentSolution
		{
			get
			{
				if (this.parentFolder != null)
				{
					return this.parentFolder.ParentSolution;
				}
				return this.parentSolution;
			}
			internal set
			{
				if (this.parentSolution != null && this.parentSolution != value)
				{
					this.NotifyUnboundFromSolution(true);
				}
				this.parentSolution = value;
				this.NotifyBoundToSolution(true);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this item is currently being loaded from a file
		/// </summary>
		/// <remarks>
		/// While an item is loading, some events such as project file change events may be fired.
		/// This flag can be used to check if change events are caused by data being loaded.
		/// </remarks>
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x000262E5 File Offset: 0x000244E5
		public bool Loading
		{
			get
			{
				return this.loading > 0;
			}
		}

		/// <summary>
		/// Saves the solution item
		/// </summary>
		/// <param name="monitor">
		/// A progress monitor.
		/// </param>
		// Token: 0x060009A9 RID: 2473
		public abstract void Save(IProgressMonitor monitor);

		/// <summary>
		/// Name of the solution item
		/// </summary>
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060009AA RID: 2474
		// (set) Token: 0x060009AB RID: 2475
		public abstract string Name { get; set; }

		/// <summary>
		/// Gets or sets the base directory of this solution item
		/// </summary>
		/// <value>
		/// The base directory.
		/// </value>
		/// <remarks>
		/// The base directory is the directory where files belonging to this project
		/// are placed. Notice that this directory may be different than the directory
		/// where the project file is placed.
		/// </remarks>
		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x000262F0 File Offset: 0x000244F0
		// (set) Token: 0x060009AD RID: 2477 RVA: 0x00026334 File Offset: 0x00024534
		public FilePath BaseDirectory
		{
			get
			{
				if (this.baseDirectory == null)
				{
					FilePath filePath = this.GetDefaultBaseDirectory();
					if (filePath.IsNullOrEmpty)
					{
						filePath = ".";
					}
					return filePath.FullPath;
				}
				return this.baseDirectory;
			}
			set
			{
				FilePath defaultBaseDirectory = this.GetDefaultBaseDirectory();
				if (value != FilePath.Null && defaultBaseDirectory != FilePath.Null && value.FullPath == defaultBaseDirectory.FullPath)
				{
					this.baseDirectory = null;
				}
				else if (string.IsNullOrEmpty(value))
				{
					this.baseDirectory = null;
				}
				else
				{
					this.baseDirectory = value.FullPath;
				}
				this.NotifyModified("BaseDirectory");
			}
		}

		/// <summary>
		/// Gets the directory where this solution item is placed
		/// </summary>
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x000263B4 File Offset: 0x000245B4
		public FilePath ItemDirectory
		{
			get
			{
				FilePath filePath = this.GetDefaultBaseDirectory();
				if (string.IsNullOrEmpty(filePath))
				{
					filePath = ".";
				}
				return filePath.FullPath;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x000263E7 File Offset: 0x000245E7
		internal bool HasCustomBaseDirectory
		{
			get
			{
				return this.baseDirectory != null;
			}
		}

		/// <summary>
		/// Gets the default base directory.
		/// </summary>
		/// <remarks>
		/// The base directory is the directory where files belonging to this project
		/// are placed. Notice that this directory may be different than the directory
		/// where the project file is placed.
		/// </remarks>
		// Token: 0x060009B0 RID: 2480 RVA: 0x000263F5 File Offset: 0x000245F5
		protected virtual FilePath GetDefaultBaseDirectory()
		{
			return this.ParentSolution.BaseDirectory;
		}

		/// <summary>
		/// Gets the identifier of this solution item
		/// </summary>
		/// <remarks>
		/// The identifier is unique inside the solution
		/// </remarks>
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x00026402 File Offset: 0x00024602
		public string ItemId
		{
			get
			{
				return this.ItemHandler.ItemId;
			}
		}

		/// <summary>
		/// Gets extended properties.
		/// </summary>
		/// <remarks>
		/// This dictionary can be used by add-ins to store arbitrary information about this solution item.
		/// Keys and values can be of any type.
		/// If a value implements IDisposable, the value will be disposed when this solution item is disposed.
		/// Values in this dictionary won't be serialized, unless they are registered as serializable using
		/// the /MonoDevelop/ProjectModel/ExtendedProperties extension point.
		/// </remarks>
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0002640F File Offset: 0x0002460F
		public IDictionary ExtendedProperties
		{
			get
			{
				return this.InternalGetExtendedProperties;
			}
		}

		/// <summary>
		/// Gets policies.
		/// </summary>
		/// <remarks>
		/// Returns a policy container which can be used to query policies specific for this
		/// solution item. If a policy is not defined for this item, the inherited value will be returned.
		/// </remarks>
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00026417 File Offset: 0x00024617
		// (set) Token: 0x060009B4 RID: 2484 RVA: 0x0002643E File Offset: 0x0002463E
		public PolicyBag Policies
		{
			get
			{
				if (this.policies == null)
				{
					this.policies = new PolicyBag();
				}
				this.policies.Owner = this;
				return this.policies;
			}
			internal set
			{
				this.policies = value;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x00026447 File Offset: 0x00024647
		PolicyContainer IPolicyProvider.Policies
		{
			get
			{
				return this.Policies;
			}
		}

		/// <summary>
		/// Gets solution item properties specific to the current user
		/// </summary>
		/// <remarks>
		/// These properties are not stored in the project file, but in a separate file which is not to be shared
		/// with other users.
		/// User properties are only loaded when the project is loaded inside the IDE.
		/// </remarks>
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0002644F File Offset: 0x0002464F
		public PropertyBag UserProperties
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

		/// <summary>
		/// Initializes the user properties of the item
		/// </summary>
		/// <param name="properties">
		/// Properties to be set
		/// </param>
		/// <exception cref="T:System.InvalidOperationException">
		/// The user properties have already been set
		/// </exception>
		/// <remarks>
		/// This method is used by the IDE to initialize the user properties when a project is loaded.
		/// </remarks>
		// Token: 0x060009B7 RID: 2487 RVA: 0x0002646A File Offset: 0x0002466A
		public void LoadUserProperties(PropertyBag properties)
		{
			if (this.userProperties != null)
			{
				throw new InvalidOperationException("User properties already loaded.");
			}
			this.userProperties = properties;
		}

		/// <summary>
		/// Gets the parent solution folder.
		/// </summary>
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x00026486 File Offset: 0x00024686
		// (set) Token: 0x060009B9 RID: 2489 RVA: 0x00026490 File Offset: 0x00024690
		public SolutionFolder ParentFolder
		{
			get
			{
				return this.parentFolder;
			}
			internal set
			{
				if (this.parentFolder != null && this.parentFolder.ParentSolution != null && (value == null || value.ParentSolution != this.parentFolder.ParentSolution))
				{
					this.NotifyUnboundFromSolution(false);
				}
				this.parentFolder = value;
				if (this.internalChildren != null)
				{
					this.internalChildren.ParentFolder = value;
				}
				if (value != null && value.ParentSolution != null)
				{
					this.NotifyBoundToSolution(false);
				}
			}
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x000264FC File Offset: 0x000246FC
		private void NotifyBoundToSolution(bool includeInternalChildren)
		{
			SolutionFolder solutionFolder = this as SolutionFolder;
			if (solutionFolder != null)
			{
				SolutionFolderItemCollection itemsWithoutCreating = solutionFolder.GetItemsWithoutCreating();
				if (itemsWithoutCreating != null)
				{
					foreach (SolutionItem solutionItem in itemsWithoutCreating)
					{
						solutionItem.NotifyBoundToSolution(true);
					}
				}
			}
			if (includeInternalChildren && this.internalChildren != null)
			{
				this.internalChildren.NotifyBoundToSolution(true);
			}
			this.OnBoundToSolution();
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00026578 File Offset: 0x00024778
		private void NotifyUnboundFromSolution(bool includeInternalChildren)
		{
			SolutionFolder solutionFolder = this as SolutionFolder;
			if (solutionFolder != null)
			{
				SolutionFolderItemCollection itemsWithoutCreating = solutionFolder.GetItemsWithoutCreating();
				if (itemsWithoutCreating != null)
				{
					foreach (SolutionItem solutionItem in itemsWithoutCreating)
					{
						solutionItem.NotifyUnboundFromSolution(true);
					}
				}
			}
			if (includeInternalChildren && this.internalChildren != null)
			{
				this.internalChildren.NotifyUnboundFromSolution(true);
			}
			this.OnUnboundFromSolution();
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:MonoDevelop.Projects.SolutionItem" /> has been disposed.
		/// </summary>
		/// <value>
		/// <c>true</c> if disposed; otherwise, <c>false</c>.
		/// </value>
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x000265F4 File Offset: 0x000247F4
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x000265FC File Offset: 0x000247FC
		protected internal bool Disposed { get; private set; }

		/// <summary>
		/// Releases all resource used by the <see cref="T:MonoDevelop.Projects.SolutionItem" /> object.
		/// </summary>
		/// <remarks>
		/// Call <see cref="M:MonoDevelop.Projects.SolutionItem.Dispose" /> when you are finished using the <see cref="T:MonoDevelop.Projects.SolutionItem" />. The
		/// <see cref="M:MonoDevelop.Projects.SolutionItem.Dispose" /> method leaves the <see cref="T:MonoDevelop.Projects.SolutionItem" /> in an unusable state.
		/// After calling <see cref="M:MonoDevelop.Projects.SolutionItem.Dispose" />, you must release all references to the
		/// <see cref="T:MonoDevelop.Projects.SolutionItem" /> so the garbage collector can reclaim the memory that the
		/// <see cref="T:MonoDevelop.Projects.SolutionItem" /> was occupying.
		/// </remarks>
		// Token: 0x060009BE RID: 2494 RVA: 0x00026608 File Offset: 0x00024808
		public virtual void Dispose()
		{
			if (this.Disposing != null)
			{
				this.Disposing(this, EventArgs.Empty);
			}
			this.Disposed = true;
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
				this.extendedProperties = null;
			}
			if (this.handler != null)
			{
				this.handler.Dispose();
			}
			if (this.userProperties != null)
			{
				((IDisposable)this.userProperties).Dispose();
				this.userProperties = null;
			}
		}

		/// <summary>
		/// Gets solution items referenced by this instance (items on which this item depends)
		/// </summary>
		/// <returns>
		/// The referenced items.
		/// </returns>
		/// <param name="configuration">
		/// Configuration for which to get the referenced items
		/// </param>
		// Token: 0x060009BF RID: 2495 RVA: 0x000266C8 File Offset: 0x000248C8
		public virtual IEnumerable<SolutionItem> GetReferencedItems(ConfigurationSelector configuration)
		{
			return new SolutionItem[0];
		}

		/// <summary>
		/// Runs a build or execution target.
		/// </summary>
		/// <returns>
		/// The result of the operation
		/// </returns>
		/// <param name="monitor">
		/// A progress monitor
		/// </param>
		/// <param name="target">
		/// Name of the target
		/// </param>
		/// <param name="configuration">
		/// Configuration to use to run the target
		/// </param>
		// Token: 0x060009C0 RID: 2496 RVA: 0x000266D0 File Offset: 0x000248D0
		public BuildResult RunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
		{
			return Services.ProjectService.GetExtensionChain(this).RunTarget(monitor, this, target, configuration);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x000266E6 File Offset: 0x000248E6
		public bool SupportsTarget(string target)
		{
			return Services.ProjectService.GetExtensionChain(this).SupportsTarget(this, target);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x000266FA File Offset: 0x000248FA
		public bool SupportsBuild()
		{
			return this.SupportsTarget("Build");
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00026707 File Offset: 0x00024907
		public bool SupportsExecute()
		{
			return Services.ProjectService.GetExtensionChain(this).SupportsExecute(this);
		}

		/// <summary>
		/// Cleans the files produced by this solution item
		/// </summary>
		/// <param name="monitor">
		/// A progress monitor
		/// </param>
		/// <param name="configuration">
		/// Configuration to use to clean the project
		/// </param>
		// Token: 0x060009C4 RID: 2500 RVA: 0x0002671C File Offset: 0x0002491C
		public void Clean(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			ITimeTracker timeTracker = Counters.CleanProjectTimer.BeginTiming("Cleaning " + this.Name, this.GetProjectEventMetadata());
			try
			{
				if (this is SolutionFolder)
				{
					this.RunTarget(monitor, "Clean", configuration);
				}
				else
				{
					try
					{
						SolutionEntityItem solutionEntityItem = this as SolutionEntityItem;
						SolutionItemConfiguration solutionItemConfiguration = (solutionEntityItem != null) ? solutionEntityItem.GetConfiguration(configuration) : null;
						string arg = (solutionItemConfiguration != null) ? solutionItemConfiguration.Id : configuration.ToString();
						monitor.BeginTask(GettextCatalog.GetString("Cleaning: {0} ({1})", this.Name, arg), 1);
						this.RunTarget(monitor, "Clean", configuration);
					}
					finally
					{
						monitor.EndTask();
					}
				}
			}
			finally
			{
				timeTracker.End();
			}
		}

		/// <summary>
		/// Builds the solution item
		/// </summary>
		/// <param name="monitor">
		/// A progress monitor
		/// </param>
		/// <param name="configuration">
		/// Configuration to use to build the project
		/// </param>
		// Token: 0x060009C5 RID: 2501 RVA: 0x000267E0 File Offset: 0x000249E0
		public BuildResult Build(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			return this.Build(monitor, configuration, false);
		}

		/// <summary>
		/// Builds the solution item
		/// </summary>
		/// <param name="monitor">
		/// A progress monitor
		/// </param>
		/// <param name="solutionConfiguration">
		/// Configuration to use to build the project
		/// </param>
		/// <param name="buildReferences">
		/// When set to <c>true</c>, the referenced items will be built before building this item
		/// </param>
		// Token: 0x060009C6 RID: 2502 RVA: 0x000267EC File Offset: 0x000249EC
		public BuildResult Build(IProgressMonitor monitor, ConfigurationSelector solutionConfiguration, bool buildReferences)
		{
			if (!buildReferences)
			{
				if (this is SolutionFolder)
				{
					return this.RunTarget(monitor, "Build", solutionConfiguration);
				}
				try
				{
					SolutionEntityItem solutionEntityItem = this as SolutionEntityItem;
					SolutionItemConfiguration solutionItemConfiguration = (solutionEntityItem != null) ? solutionEntityItem.GetConfiguration(solutionConfiguration) : null;
					string arg = (solutionItemConfiguration != null) ? solutionItemConfiguration.Id : solutionConfiguration.ToString();
					monitor.BeginTask(GettextCatalog.GetString("Building: {0} ({1})", this.Name, arg), 1);
					using (Counters.BuildProjectTimer.BeginTiming("Building " + this.Name, this.GetProjectEventMetadata()))
					{
						return this.RunTarget(monitor, "Build", solutionConfiguration);
					}
				}
				finally
				{
					monitor.EndTask();
				}
			}
			ITimeTracker timeTracker2 = Counters.BuildProjectAndReferencesTimer.BeginTiming("Building " + this.Name, this.GetProjectEventMetadata());
			BuildResult result;
			try
			{
				List<SolutionItem> list = new List<SolutionItem>();
				Set<SolutionItem> visited = new Set<SolutionItem>();
				this.GetBuildableReferencedItems(visited, list, this, solutionConfiguration);
				ReadOnlyCollection<SolutionItem> readOnlyCollection = SolutionItem.TopologicalSort<SolutionItem>(list, solutionConfiguration);
				BuildResult buildResult = new BuildResult();
				buildResult.BuildCount = 0;
				HashSet<SolutionItem> hashSet = new HashSet<SolutionItem>();
				monitor.BeginTask(null, readOnlyCollection.Count);
				foreach (SolutionItem solutionItem in readOnlyCollection)
				{
					if (!solutionItem.ContainsReferences(hashSet, solutionConfiguration))
					{
						BuildResult buildResult2 = solutionItem.Build(monitor, solutionConfiguration, false);
						buildResult.Append(buildResult2);
						if (buildResult2.ErrorCount > 0)
						{
							hashSet.Add(solutionItem);
						}
					}
					else
					{
						hashSet.Add(solutionItem);
					}
					monitor.Step(1);
					if (monitor.IsCancelRequested)
					{
						break;
					}
				}
				monitor.EndTask();
				result = buildResult;
			}
			finally
			{
				timeTracker2.End();
			}
			return result;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x000269CC File Offset: 0x00024BCC
		internal bool ContainsReferences(HashSet<SolutionItem> items, ConfigurationSelector conf)
		{
			foreach (SolutionItem item in this.GetReferencedItems(conf))
			{
				if (items.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the time of the last build
		/// </summary>
		/// <returns>
		/// The last build time.
		/// </returns>
		/// <param name="configuration">
		/// Configuration for which to get the last build time.
		/// </param>
		// Token: 0x060009C8 RID: 2504 RVA: 0x00026A24 File Offset: 0x00024C24
		public DateTime GetLastBuildTime(ConfigurationSelector configuration)
		{
			return this.OnGetLastBuildTime(configuration);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00026A30 File Offset: 0x00024C30
		private void GetBuildableReferencedItems(Set<SolutionItem> visited, List<SolutionItem> referenced, SolutionItem item, ConfigurationSelector configuration)
		{
			if (!visited.Add(item))
			{
				return;
			}
			referenced.Add(item);
			foreach (SolutionItem item2 in item.GetReferencedItems(configuration))
			{
				this.GetBuildableReferencedItems(visited, referenced, item2, configuration);
			}
		}

		/// <summary>
		/// Executes this solution item
		/// </summary>
		/// <param name="monitor">
		/// A progress monitor
		/// </param>
		/// <param name="context">
		/// An execution context
		/// </param>
		/// <param name="configuration">
		/// Configuration to use to execute the item
		/// </param>
		// Token: 0x060009CA RID: 2506 RVA: 0x00026A94 File Offset: 0x00024C94
		public void Execute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
			Services.ProjectService.GetExtensionChain(this).Execute(monitor, this, context, configuration);
		}

		/// <summary>
		/// Determines whether this solution item can be executed using the specified context and configuration.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can be executed; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="context">
		/// An execution context
		/// </param>
		/// <param name="configuration">
		/// Configuration to use to execute the item
		/// </param>
		// Token: 0x060009CB RID: 2507 RVA: 0x00026AAA File Offset: 0x00024CAA
		public bool CanExecute(ExecutionContext context, ConfigurationSelector configuration)
		{
			return this.SupportsExecute() && Services.ProjectService.GetExtensionChain(this).CanExecute(this, context, configuration);
		}

		/// <summary>
		/// Gets the execution targets.
		/// </summary>
		/// <returns>The execution targets.</returns>
		/// <param name="configuration">The configuration.</param>
		// Token: 0x060009CC RID: 2508 RVA: 0x00026AC9 File Offset: 0x00024CC9
		public IEnumerable<ExecutionTarget> GetExecutionTargets(ConfigurationSelector configuration)
		{
			return Services.ProjectService.GetExtensionChain(this).GetExecutionTargets(this, configuration);
		}

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x060009CD RID: 2509 RVA: 0x00026AE0 File Offset: 0x00024CE0
		// (remove) Token: 0x060009CE RID: 2510 RVA: 0x00026B18 File Offset: 0x00024D18
		public event EventHandler ExecutionTargetsChanged;

		// Token: 0x060009CF RID: 2511 RVA: 0x00026B4D File Offset: 0x00024D4D
		protected virtual void OnExecutionTargetsChanged()
		{
			if (this.ExecutionTargetsChanged != null)
			{
				this.ExecutionTargetsChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Checks if this solution item has modified files and has to be built
		/// </summary>
		/// <returns>
		/// <c>true</c> if the solution item has to be built
		/// </returns>
		/// <param name="configuration">
		/// Configuration for which to do the check
		/// </param>
		// Token: 0x060009D0 RID: 2512 RVA: 0x00026B68 File Offset: 0x00024D68
		[Obsolete("This method will be removed in future releases")]
		public bool NeedsBuilding(ConfigurationSelector configuration)
		{
			return true;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00026B6C File Offset: 0x00024D6C
		internal bool InternalCheckNeedsBuild(ConfigurationSelector configuration)
		{
			bool needsBuilding;
			using (Counters.NeedsBuildingTimer.BeginTiming("NeedsBuilding check for " + this.Name))
			{
				needsBuilding = Services.ProjectService.GetExtensionChain(this).GetNeedsBuilding(this, configuration);
			}
			return needsBuilding;
		}

		/// <summary>
		/// States whether this solution item needs to be built or not
		/// </summary>
		/// <param name="value">
		/// Whether this solution item needs to be built or not
		/// </param>
		/// <param name="configuration">
		/// Configuration for which to set the flag
		/// </param>
		// Token: 0x060009D2 RID: 2514 RVA: 0x00026BC4 File Offset: 0x00024DC4
		[Obsolete("This method will be removed in future releases")]
		public void SetNeedsBuilding(bool value, ConfigurationSelector configuration)
		{
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:MonoDevelop.Projects.SolutionItem" /> needs to be reload due to changes in project or solution file
		/// </summary>
		/// <value>
		/// <c>true</c> if needs reload; otherwise, <c>false</c>.
		/// </value>
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x00026BC6 File Offset: 0x00024DC6
		// (set) Token: 0x060009D4 RID: 2516 RVA: 0x00026BDD File Offset: 0x00024DDD
		public virtual bool NeedsReload
		{
			get
			{
				return this.ParentSolution != null && this.ParentSolution.NeedsReload;
			}
			set
			{
			}
		}

		/// <summary>
		/// Registers an internal child item.
		/// </summary>
		/// <param name="item">
		/// An item
		/// </param>
		/// <remarks>
		/// Some kind of projects may be composed of several child projects.
		/// By registering those child projects using this method, the child
		/// projects will be plugged into the parent solution infrastructure
		/// (so for example, the ParentSolution property for those projects
		/// will return the correct value)
		/// </remarks>
		// Token: 0x060009D5 RID: 2517 RVA: 0x00026BDF File Offset: 0x00024DDF
		protected void RegisterInternalChild(SolutionItem item)
		{
			if (this.internalChildren == null)
			{
				this.internalChildren = new SolutionFolder();
				this.internalChildren.ParentFolder = this.parentFolder;
			}
			this.internalChildren.Items.Add(item);
		}

		/// <summary>
		/// Unregisters an internal child item.
		/// </summary>
		/// <param name="item">
		/// The item
		/// </param>
		// Token: 0x060009D6 RID: 2518 RVA: 0x00026C16 File Offset: 0x00024E16
		protected void UnregisterInternalChild(SolutionItem item)
		{
			if (this.internalChildren != null)
			{
				this.internalChildren.Items.Remove(item);
			}
		}

		/// <summary>
		/// Gets the string tag model description for this solution item
		/// </summary>
		/// <returns>
		/// The string tag model description
		/// </returns>
		/// <param name="conf">
		/// Configuration for which to get the string tag model description
		/// </param>
		// Token: 0x060009D7 RID: 2519 RVA: 0x00026C34 File Offset: 0x00024E34
		public virtual StringTagModelDescription GetStringTagModelDescription(ConfigurationSelector conf)
		{
			StringTagModelDescription stringTagModelDescription = new StringTagModelDescription();
			stringTagModelDescription.Add(base.GetType());
			stringTagModelDescription.Add(typeof(Solution));
			return stringTagModelDescription;
		}

		/// <summary>
		/// Gets the string tag model for this solution item
		/// </summary>
		/// <returns>
		/// The string tag model
		/// </returns>
		/// <param name="conf">
		/// Configuration for which to get the string tag model
		/// </param>
		// Token: 0x060009D8 RID: 2520 RVA: 0x00026C64 File Offset: 0x00024E64
		public virtual StringTagModel GetStringTagModel(ConfigurationSelector conf)
		{
			StringTagModel stringTagModel = new StringTagModel();
			stringTagModel.Add(this);
			if (this.ParentSolution != null)
			{
				stringTagModel.Add(this.ParentSolution.GetStringTagModel());
			}
			return stringTagModel;
		}

		/// <summary>
		/// Sorts a collection of solution items, taking into account the dependencies between them
		/// </summary>
		/// <returns>
		/// The sorted collection of items
		/// </returns>
		/// <param name="items">
		/// Items to sort
		/// </param>
		/// <param name="configuration">
		/// A configuration
		/// </param>
		/// <remarks>
		/// This methods sorts a collection of items, ensuring that every item is placed after all the items
		/// on which it depends.
		/// </remarks>
		// Token: 0x060009D9 RID: 2521 RVA: 0x00026C98 File Offset: 0x00024E98
		public static ReadOnlyCollection<T> TopologicalSort<T>(IEnumerable<T> items, ConfigurationSelector configuration) where T : SolutionItem
		{
			IList<T> list = items as IList<T>;
			if (list == null)
			{
				list = new List<T>(items);
			}
			List<T> list2 = new List<T>();
			bool[] array = new bool[list.Count];
			bool[] triedToInsert = new bool[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				if (!array[i])
				{
					SolutionItem.Insert<T>(i, list, list2, array, triedToInsert, configuration);
				}
			}
			return list2.AsReadOnly();
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00026D04 File Offset: 0x00024F04
		private static void Insert<T>(int index, IList<T> allItems, List<T> sortedItems, bool[] inserted, bool[] triedToInsert, ConfigurationSelector solutionConfiguration) where T : SolutionItem
		{
			if (triedToInsert[index])
			{
				throw new CyclicDependencyException();
			}
			triedToInsert[index] = true;
			SolutionItem solutionItem = allItems[index];
			foreach (SolutionItem solutionItem2 in solutionItem.GetReferencedItems(solutionConfiguration))
			{
				int i = 0;
				while (i < allItems.Count)
				{
					SolutionItem solutionItem3 = allItems[i];
					if (solutionItem2 == solutionItem3)
					{
						if (!inserted[i])
						{
							SolutionItem.Insert<T>(i, allItems, sortedItems, inserted, triedToInsert, solutionConfiguration);
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			sortedItems.Add((T)((object)solutionItem));
			inserted[index] = true;
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x00026DB8 File Offset: 0x00024FB8
		internal virtual IDictionary InternalGetExtendedProperties
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

		// Token: 0x060009DC RID: 2524 RVA: 0x00026DD4 File Offset: 0x00024FD4
		public IDictionary<string, string> GetProjectEventMetadata()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			this.OnGetProjectEventMetadata(dictionary);
			return dictionary;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00026DEF File Offset: 0x00024FEF
		protected virtual void OnGetProjectEventMetadata(IDictionary<string, string> metadata)
		{
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00026DF1 File Offset: 0x00024FF1
		void ILoadController.BeginLoad()
		{
			this.loading++;
			this.OnBeginLoad();
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00026E07 File Offset: 0x00025007
		void ILoadController.EndLoad()
		{
			this.loading--;
			this.OnEndLoad();
		}

		/// <summary>
		/// Called when a load operation for this solution item has started
		/// </summary>
		// Token: 0x060009E0 RID: 2528 RVA: 0x00026E1D File Offset: 0x0002501D
		protected virtual void OnBeginLoad()
		{
		}

		/// <summary>
		/// Called when a load operation for this solution item has finished
		/// </summary>
		// Token: 0x060009E1 RID: 2529 RVA: 0x00026E1F File Offset: 0x0002501F
		protected virtual void OnEndLoad()
		{
		}

		/// <summary>
		/// Notifies that this solution item has been modified
		/// </summary>
		/// <param name="hint">
		/// Hint about which part of the solution item has been modified. This will typically be the property name.
		/// </param>
		// Token: 0x060009E2 RID: 2530 RVA: 0x00026E21 File Offset: 0x00025021
		protected internal void NotifyModified(string hint)
		{
			if (!this.Loading)
			{
				this.ItemHandler.OnModified(hint);
			}
			this.OnModified(new SolutionItemModifiedEventArgs(this, hint));
		}

		/// <summary>
		/// Raises the modified event.
		/// </summary>
		/// <param name="args">
		/// Arguments.
		/// </param>
		// Token: 0x060009E3 RID: 2531 RVA: 0x00026E44 File Offset: 0x00025044
		protected virtual void OnModified(SolutionItemModifiedEventArgs args)
		{
			if (this.Modified != null && !this.Disposed)
			{
				this.Modified(this, args);
			}
		}

		/// <summary>
		/// Raises the name changed event.
		/// </summary>
		/// <param name="e">
		/// Arguments.
		/// </param>
		// Token: 0x060009E4 RID: 2532 RVA: 0x00026E63 File Offset: 0x00025063
		protected virtual void OnNameChanged(SolutionItemRenamedEventArgs e)
		{
			this.NotifyModified("Name");
			if (this.NameChanged != null && !this.Disposed)
			{
				this.NameChanged(this, e);
			}
		}

		/// <summary>
		/// Initializes the item handler.
		/// </summary>
		/// <remarks>
		/// This method is called the first time an item handler is requested.
		/// Subclasses should override this method use SetItemHandler to
		/// assign a handler to this item.
		/// </remarks>
		// Token: 0x060009E5 RID: 2533 RVA: 0x00026E8D File Offset: 0x0002508D
		protected virtual void InitializeItemHandler()
		{
		}

		/// <summary>
		/// Runs a build or execution target.
		/// </summary>
		/// <returns>
		/// The result of the operation
		/// </returns>
		/// <param name="monitor">
		/// A progress monitor
		/// </param>
		/// <param name="target">
		/// Name of the target
		/// </param>
		/// <param name="configuration">
		/// Configuration to use to run the target
		/// </param>
		/// <remarks>
		/// Subclasses can override this method to provide a custom implementation of project operations such as
		/// build or clean. The default implementation delegates the execution to the more specific OnBuild
		/// and OnClean methods, or to the item handler for other targets.
		/// </remarks>
		// Token: 0x060009E6 RID: 2534 RVA: 0x00026E90 File Offset: 0x00025090
		protected internal virtual BuildResult OnRunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
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
			return this.ItemHandler.RunTarget(monitor, target, configuration) ?? new BuildResult();
		}

		/// <summary>
		/// Cleans the files produced by this solution item
		/// </summary>
		/// <param name="monitor">
		/// A progress monitor
		/// </param>
		/// <param name="configuration">
		/// Configuration to use to clean the project
		/// </param>
		// Token: 0x060009E7 RID: 2535
		protected abstract void OnClean(IProgressMonitor monitor, ConfigurationSelector configuration);

		/// <summary>
		/// Builds the solution item
		/// </summary>
		/// <param name="monitor">
		/// A progress monitor
		/// </param>
		/// <param name="configuration">
		/// Configuration to use to build the project
		/// </param>
		// Token: 0x060009E8 RID: 2536
		protected abstract BuildResult OnBuild(IProgressMonitor monitor, ConfigurationSelector configuration);

		/// <summary>
		/// Executes this solution item
		/// </summary>
		/// <param name="monitor">
		/// A progress monitor
		/// </param>
		/// <param name="context">
		/// An execution context
		/// </param>
		/// <param name="configuration">
		/// Configuration to use to execute the item
		/// </param>
		// Token: 0x060009E9 RID: 2537
		protected internal abstract void OnExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration);

		/// <summary>
		/// Checks if this solution item has modified files and has to be built
		/// </summary>
		/// <returns>
		/// <c>true</c> if the solution item has to be built
		/// </returns>
		/// <param name="configuration">
		/// Configuration for which to do the check
		/// </param>
		// Token: 0x060009EA RID: 2538 RVA: 0x00026EE5 File Offset: 0x000250E5
		protected internal virtual bool OnGetNeedsBuilding(ConfigurationSelector configuration)
		{
			return true;
		}

		/// <summary>
		/// States whether this solution item needs to be built or not
		/// </summary>
		/// <param name="val">
		/// Whether this solution item needs to be built or not
		/// </param>
		/// <param name="configuration">
		/// Configuration for which to set the flag
		/// </param>
		// Token: 0x060009EB RID: 2539 RVA: 0x00026EE8 File Offset: 0x000250E8
		protected internal virtual void OnSetNeedsBuilding(bool val, ConfigurationSelector configuration)
		{
		}

		/// <summary>
		/// Gets the time of the last build
		/// </summary>
		/// <returns>
		/// The last build time.
		/// </returns>
		/// <param name="configuration">
		/// Configuration for which to get the last build time.
		/// </param>
		// Token: 0x060009EC RID: 2540 RVA: 0x00026EEA File Offset: 0x000250EA
		protected internal virtual DateTime OnGetLastBuildTime(ConfigurationSelector configuration)
		{
			return DateTime.MinValue;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00026EF1 File Offset: 0x000250F1
		protected internal virtual bool OnGetSupportsTarget(string target)
		{
			return true;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00026EF4 File Offset: 0x000250F4
		protected internal virtual bool OnGetSupportsExecute()
		{
			return true;
		}

		/// <summary>
		/// Determines whether this solution item can be executed using the specified context and configuration.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can be executed; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="context">
		/// An execution context
		/// </param>
		/// <param name="configuration">
		/// Configuration to use to execute the item
		/// </param>
		// Token: 0x060009EF RID: 2543 RVA: 0x00026EF7 File Offset: 0x000250F7
		protected internal virtual bool OnGetCanExecute(ExecutionContext context, ConfigurationSelector configuration)
		{
			return false;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00026F9C File Offset: 0x0002519C
		protected internal virtual IEnumerable<ExecutionTarget> OnGetExecutionTargets(ConfigurationSelector configuration)
		{
			yield break;
		}

		/// <summary>
		/// Called just after this item is bound to a solution
		/// </summary>
		// Token: 0x060009F1 RID: 2545 RVA: 0x00026FB9 File Offset: 0x000251B9
		protected virtual void OnBoundToSolution()
		{
		}

		/// <summary>
		/// Called just before this item is removed from a solution (ParentSolution is still valid when this method is called)
		/// </summary>
		// Token: 0x060009F2 RID: 2546 RVA: 0x00026FBB File Offset: 0x000251BB
		protected virtual void OnUnboundFromSolution()
		{
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00026FBD File Offset: 0x000251BD
		protected internal virtual object OnGetService(Type t)
		{
			return this.ItemHandler.GetService(t);
		}

		/// <summary>
		/// Occurs when the name of the item changes
		/// </summary>
		// Token: 0x1400002E RID: 46
		// (add) Token: 0x060009F4 RID: 2548 RVA: 0x00026FCC File Offset: 0x000251CC
		// (remove) Token: 0x060009F5 RID: 2549 RVA: 0x00027004 File Offset: 0x00025204
		public event SolutionItemRenamedEventHandler NameChanged;

		/// <summary>
		/// Occurs when the item is modified.
		/// </summary>
		// Token: 0x1400002F RID: 47
		// (add) Token: 0x060009F6 RID: 2550 RVA: 0x0002703C File Offset: 0x0002523C
		// (remove) Token: 0x060009F7 RID: 2551 RVA: 0x00027074 File Offset: 0x00025274
		public event SolutionItemModifiedEventHandler Modified;

		/// <summary>
		/// Occurs when the object is being disposed
		/// </summary>
		// Token: 0x14000030 RID: 48
		// (add) Token: 0x060009F8 RID: 2552 RVA: 0x000270AC File Offset: 0x000252AC
		// (remove) Token: 0x060009F9 RID: 2553 RVA: 0x000270E4 File Offset: 0x000252E4
		public event EventHandler Disposing;

		// Token: 0x04000302 RID: 770
		private SolutionFolder parentFolder;

		// Token: 0x04000303 RID: 771
		private Solution parentSolution;

		// Token: 0x04000304 RID: 772
		private ISolutionItemHandler handler;

		// Token: 0x04000305 RID: 773
		private int loading;

		// Token: 0x04000306 RID: 774
		private SolutionFolder internalChildren;

		// Token: 0x04000307 RID: 775
		[ProjectPathItemProperty("BaseDirectory", DefaultValue = null)]
		private string baseDirectory;

		// Token: 0x04000308 RID: 776
		private Hashtable extendedProperties;

		// Token: 0x04000309 RID: 777
		[ItemProperty("Policies", IsExternal = true, SkipEmpty = true)]
		private PolicyBag policies;

		// Token: 0x0400030A RID: 778
		private PropertyBag userProperties;
	}
}
