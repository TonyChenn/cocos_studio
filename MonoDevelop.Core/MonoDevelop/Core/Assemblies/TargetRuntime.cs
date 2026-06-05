using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Mono.Addins;
using MonoDevelop.Core.AddIns;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.Instrumentation;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x0200009E RID: 158
	public abstract class TargetRuntime
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x0001189A File Offset: 0x0000FA9A
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x000118A2 File Offset: 0x0000FAA2
		private protected bool ShuttingDown { protected get; private set; }

		// Token: 0x0600052D RID: 1325 RVA: 0x000118B4 File Offset: 0x0000FAB4
		public TargetRuntime()
		{
			this.assemblyContext = new RuntimeAssemblyContext(this);
			this.composedAssemblyContext = new ComposedAssemblyContext();
			this.composedAssemblyContext.Add(Runtime.SystemAssemblyService.UserAssemblyContext);
			this.composedAssemblyContext.Add(this.assemblyContext);
			Runtime.ShuttingDown += delegate(object param0, EventArgs param1)
			{
				this.ShuttingDown = true;
			};
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00011954 File Offset: 0x0000FB54
		public bool IsInitialized
		{
			get
			{
				return this.initialized;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x0001195C File Offset: 0x0000FB5C
		protected object InitializationLock
		{
			get
			{
				return this.initLock;
			}
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00011964 File Offset: 0x0000FB64
		internal void StartInitialization()
		{
			this.backgroundInitialize = true;
			this.initializing = true;
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.BackgroundInitialize));
		}

		/// <summary>
		/// Display name of the runtime. For example "The Mono Runtime 2.6"
		/// </summary>
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00011986 File Offset: 0x0000FB86
		public virtual string DisplayName
		{
			get
			{
				if (string.IsNullOrEmpty(this.Version))
				{
					return this.DisplayRuntimeName;
				}
				return this.DisplayRuntimeName + " " + this.Version;
			}
		}

		/// <summary>
		/// Unique identifier of this runtime. For example "Mono 2.6".
		/// </summary>
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x000119B2 File Offset: 0x0000FBB2
		public string Id
		{
			get
			{
				if (string.IsNullOrEmpty(this.Version))
				{
					return this.RuntimeId;
				}
				return this.RuntimeId + " " + this.Version;
			}
		}

		/// <summary>
		/// Core display name of the runtime. For example "The Mono Runtime"
		/// </summary>
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x000119DE File Offset: 0x0000FBDE
		public virtual string DisplayRuntimeName
		{
			get
			{
				return this.RuntimeId;
			}
		}

		/// <summary>
		/// Core identifier the runtime. For example, if there are several
		/// versions of Mono installed, each of them will have "Mono" as RuntimeId
		/// </summary>
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000534 RID: 1332
		public abstract string RuntimeId { get; }

		/// <summary>
		/// Version of the runtime.
		/// This string is strictly for displaying to the user or logging. It should never be used for version checks.
		/// </summary>
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000535 RID: 1333
		public abstract string Version { get; }

		/// <summary>
		/// Returns 'true' if this runtime is the one currently running MonoDevelop.
		/// </summary>
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000536 RID: 1334
		public abstract bool IsRunning { get; }

		// Token: 0x06000537 RID: 1335 RVA: 0x00011A8C File Offset: 0x0000FC8C
		public virtual IEnumerable<FilePath> GetReferenceFrameworkDirectories()
		{
			yield break;
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00011AA9 File Offset: 0x0000FCA9
		public IEnumerable<TargetFramework> CustomFrameworks
		{
			get
			{
				return this.customFrameworks;
			}
		}

		// Token: 0x06000539 RID: 1337
		protected abstract void OnInitialize();

		/// <summary>
		/// Returns an IExecutionHandler which can be used to execute commands in this runtime.
		/// </summary>
		// Token: 0x0600053A RID: 1338
		public abstract IExecutionHandler GetExecutionHandler();

		/// <summary>
		/// Returns an IAssemblyContext which can be used to discover assemblies through this runtime.
		/// It includes assemblies from directories manually registered by the user.
		/// </summary>
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00011AB1 File Offset: 0x0000FCB1
		public IAssemblyContext AssemblyContext
		{
			get
			{
				this.EnsureInitialized();
				return this.composedAssemblyContext;
			}
		}

		/// <summary>
		/// Returns an IAssemblyContext which can be used to discover assemblies provided by this runtime
		/// </summary>
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x00011ABF File Offset: 0x0000FCBF
		public RuntimeAssemblyContext RuntimeAssemblyContext
		{
			get
			{
				this.EnsureInitialized();
				return this.assemblyContext;
			}
		}

		/// <summary>
		/// Given an assembly file name, returns the corresponding debug information file name.
		/// (.mdb for Mono, .pdb for MS.NET)
		/// </summary>
		// Token: 0x0600053D RID: 1341
		public abstract string GetAssemblyDebugInfoFile(string assemblyPath);

		/// <summary>
		/// Executes an assembly using this runtime
		/// </summary>
		// Token: 0x0600053E RID: 1342 RVA: 0x00011ACD File Offset: 0x0000FCCD
		public Process ExecuteAssembly(string file, string arguments)
		{
			return this.ExecuteAssembly(file, arguments, null);
		}

		/// <summary>
		/// Executes an assembly using this runtime and the specified framework.
		/// </summary>
		// Token: 0x0600053F RID: 1343 RVA: 0x00011AD8 File Offset: 0x0000FCD8
		public Process ExecuteAssembly(string file, string arguments, TargetFramework fx)
		{
			return this.ExecuteAssembly(new ProcessStartInfo(file, arguments)
			{
				UseShellExecute = false
			}, fx);
		}

		/// <summary>
		/// Executes an assembly using this runtime.
		/// </summary>
		/// <param name="pinfo">
		/// Information of the process to execute
		/// </param>
		/// <returns>
		/// The started process.
		/// </returns>
		// Token: 0x06000540 RID: 1344 RVA: 0x00011AFC File Offset: 0x0000FCFC
		public Process ExecuteAssembly(ProcessStartInfo pinfo)
		{
			return this.ExecuteAssembly(pinfo, null);
		}

		/// <summary>
		/// Executes an assembly using this runtime and the specified framework.
		/// </summary>
		/// <param name="pinfo">
		/// Information of the process to execute
		/// </param>
		/// <param name="fx">
		/// Framework on which the assembly has to be executed.
		/// </param>
		/// <returns>
		/// The started process.
		/// </returns>
		// Token: 0x06000541 RID: 1345 RVA: 0x00011B08 File Offset: 0x0000FD08
		public virtual Process ExecuteAssembly(ProcessStartInfo pinfo, TargetFramework fx)
		{
			if (fx == null)
			{
				TargetFrameworkMoniker targetFrameworkForAssembly = Runtime.SystemAssemblyService.GetTargetFrameworkForAssembly(this, pinfo.FileName);
				fx = Runtime.SystemAssemblyService.GetTargetFramework(targetFrameworkForAssembly);
				if (!this.IsInstalled(fx))
				{
					foreach (TargetFramework targetFramework in Runtime.SystemAssemblyService.GetTargetFrameworks())
					{
						if (this.IsInstalled(targetFramework) && targetFramework.CanReferenceAssembliesTargetingFramework(fx))
						{
							fx = targetFramework;
							break;
						}
					}
				}
				if (!this.IsInstalled(fx))
				{
					throw new InvalidOperationException(string.Format("No compatible framework found for assembly '{0}' (required framework: {1})", pinfo.FileName, targetFrameworkForAssembly));
				}
			}
			this.ConvertAssemblyProcessStartInfo(pinfo);
			return Process.Start(pinfo);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00011BC4 File Offset: 0x0000FDC4
		protected virtual void ConvertAssemblyProcessStartInfo(ProcessStartInfo pinfo)
		{
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00011BC8 File Offset: 0x0000FDC8
		protected TargetFrameworkBackend GetBackend(TargetFramework fx)
		{
			this.EnsureInitialized();
			TargetFrameworkBackend result;
			lock (this.frameworkBackends)
			{
				TargetFrameworkBackend targetFrameworkBackend;
				if (this.frameworkBackends.TryGetValue(fx.Id, out targetFrameworkBackend))
				{
					result = targetFrameworkBackend;
				}
				else
				{
					targetFrameworkBackend = fx.CreateBackendForRuntime(this);
					if (targetFrameworkBackend == null)
					{
						targetFrameworkBackend = this.CreateBackend(fx);
						if (targetFrameworkBackend == null)
						{
							targetFrameworkBackend = new NotSupportedFrameworkBackend();
						}
					}
					targetFrameworkBackend.Initialize(this, fx);
					this.frameworkBackends[fx.Id] = targetFrameworkBackend;
					result = targetFrameworkBackend;
				}
			}
			return result;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00011C5C File Offset: 0x0000FE5C
		protected virtual TargetFrameworkBackend CreateBackend(TargetFramework fx)
		{
			return null;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00011C5F File Offset: 0x0000FE5F
		protected internal virtual IEnumerable<string> GetFrameworkFolders(TargetFramework fx)
		{
			return this.GetBackend(fx).GetFrameworkFolders();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00011C70 File Offset: 0x0000FE70
		public IEnumerable<string> FindFacadeAssembliesForPCL(TargetFramework tx)
		{
			foreach (string path in this.GetFrameworkFolders(tx))
			{
				string path2 = Path.Combine(path, "Facades");
				if (Directory.Exists(path2))
				{
					return Directory.GetFiles(path2, "*.dll");
				}
			}
			return new string[0];
		}

		/// <summary>
		/// Returns a list of environment variables that should be set when running tools using this runtime
		/// </summary>
		// Token: 0x06000547 RID: 1351 RVA: 0x00011CE4 File Offset: 0x0000FEE4
		public virtual ExecutionEnvironment GetToolsExecutionEnvironment(TargetFramework fx)
		{
			return new ExecutionEnvironment(this.GetBackend(fx).GetToolsEnvironmentVariables());
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00011CF7 File Offset: 0x0000FEF7
		public virtual ExecutionEnvironment GetToolsExecutionEnvironment()
		{
			return new ExecutionEnvironment();
		}

		/// <summary>
		/// Looks for the specified tool in this runtime. The name can be a script or a .exe.
		/// </summary>
		// Token: 0x06000549 RID: 1353 RVA: 0x00011CFE File Offset: 0x0000FEFE
		public virtual string GetToolPath(TargetFramework fx, string toolName)
		{
			return this.GetBackend(fx).GetToolPath(toolName);
		}

		/// <summary>
		/// Returns a list of paths which can contain tools for this runtime.
		/// </summary>
		// Token: 0x0600054A RID: 1354 RVA: 0x00011D0D File Offset: 0x0000FF0D
		public virtual IEnumerable<string> GetToolsPaths(TargetFramework fx)
		{
			return this.GetBackend(fx).GetToolsPaths();
		}

		/// <summary>
		/// Returns the MSBuild bin path for this runtime.
		/// </summary>
		// Token: 0x0600054B RID: 1355
		public abstract string GetMSBuildBinPath(string toolsVersion);

		/// <summary>
		/// Returns the MSBuild extensions path.
		/// </summary>
		// Token: 0x0600054C RID: 1356
		public abstract string GetMSBuildExtensionsPath();

		/// <summary>
		/// Returns all GAC locations for this runtime.
		/// </summary>
		// Token: 0x0600054D RID: 1357
		protected internal abstract IEnumerable<string> GetGacDirectories();

		/// <summary>
		/// This event is fired when the runtime has finished initializing. Runtimes are initialized
		/// in a background thread, so they are not guaranteed to be ready just after the IDE has
		/// finished loading. If the runtime is already initialized when the event is subscribed, then the
		/// subscribed handler will be automatically invoked.
		/// </summary>
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x0600054E RID: 1358 RVA: 0x00011D1C File Offset: 0x0000FF1C
		// (remove) Token: 0x0600054F RID: 1359 RVA: 0x00011D8C File Offset: 0x0000FF8C
		public event EventHandler Initialized
		{
			add
			{
				lock (this.initEventLock)
				{
					if (this.initialized)
					{
						if (!this.ShuttingDown)
						{
							value(this, EventArgs.Empty);
						}
					}
					else
					{
						this.initializedEvent = (EventHandler)Delegate.Combine(this.initializedEvent, value);
					}
				}
			}
			remove
			{
				lock (this.initEventLock)
				{
					this.initializedEvent = (EventHandler)Delegate.Remove(this.initializedEvent, value);
				}
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00011DE0 File Offset: 0x0000FFE0
		internal void EnsureInitialized()
		{
			lock (this.initLock)
			{
				if (!this.initialized && !this.initializing)
				{
					if (this.backgroundInitialize)
					{
						throw new InvalidOperationException("Runtime intialization not started");
					}
					this.initializing = true;
					this.BackgroundInitialize(null);
				}
				if (!this.extensionInitialized && !this.initializing)
				{
					this.extensionInitialized = true;
					AddinManager.AddExtensionNodeHandler("/MonoDevelop/Core/SupportPackages", new ExtensionNodeEventHandler(this.OnPackagesChanged));
				}
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00011E7C File Offset: 0x0001007C
		private void BackgroundInitialize(object state)
		{
			this.timer = Counters.TargetRuntimesLoading.BeginTiming("Initializing Runtime " + this.Id);
			lock (this.initLock)
			{
				try
				{
					this.RunInitialization();
				}
				catch (Exception ex)
				{
					LoggingService.LogInternalError("Unhandled exception in SystemAssemblyService background initialisation thread.", ex);
				}
				finally
				{
					lock (this.initEventLock)
					{
						this.initializing = false;
						this.initialized = true;
						try
						{
							if (this.initializedEvent != null && !this.ShuttingDown)
							{
								this.initializedEvent(this, EventArgs.Empty);
							}
						}
						catch (Exception ex2)
						{
							LoggingService.LogError("Error while initializing the runtime: " + this.Id, ex2);
						}
					}
					this.timer.End();
				}
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00011F98 File Offset: 0x00010198
		private void RunInitialization()
		{
			if (this.ShuttingDown)
			{
				return;
			}
			this.timer.Trace("Finding custom frameworks");
			List<TargetFramework> list = new List<TargetFramework>();
			try
			{
				foreach (FilePath filePath in this.GetReferenceFrameworkDirectories())
				{
					if (!string.IsNullOrEmpty(filePath) && Directory.Exists(filePath))
					{
						list.AddRange(TargetRuntime.FindTargetFrameworks(filePath));
					}
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error finding custom frameworks", ex);
			}
			this.customFrameworks = list.ToArray();
			this.timer.Trace("Creating frameworks");
			this.CreateFrameworks();
			if (this.ShuttingDown)
			{
				return;
			}
			this.timer.Trace("Initializing frameworks");
			this.OnInitialize();
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00012080 File Offset: 0x00010280
		private void OnPackagesChanged(object s, ExtensionNodeEventArgs args)
		{
			PackageExtensionNode packageExtensionNode = (PackageExtensionNode)args.ExtensionNode;
			SystemPackageInfo packageInfo = packageExtensionNode.GetPackageInfo();
			if (args.Change == ExtensionChange.Add)
			{
				SystemPackage packageInternal = this.assemblyContext.GetPackageInternal(packageInfo.Name);
				if (packageInternal == null || !packageInternal.IsFrameworkPackage || packageInfo.IsFrameworkPackage)
				{
					this.RegisterPackage(packageInfo, packageExtensionNode.Assemblies);
					return;
				}
			}
			else
			{
				SystemPackage package = this.assemblyContext.GetPackage(packageInfo.Name, packageInfo.Version);
				if (package.IsInternalPackage)
				{
					this.assemblyContext.UnregisterPackage(packageInfo.Name, packageInfo.Version);
				}
			}
		}

		/// <summary>
		/// Registers a package. It can be used by add-ins to register a package for a set of assemblies
		/// they provide.
		/// </summary>
		/// <param name="pinfo">
		/// Information about the package.
		/// </param>
		/// <param name="assemblyFiles">
		/// Assemblies that belong to the package
		/// </param>
		/// <returns>
		/// The registered package
		/// </returns>
		// Token: 0x06000554 RID: 1364 RVA: 0x00012113 File Offset: 0x00010313
		public SystemPackage RegisterPackage(SystemPackageInfo pinfo, params string[] assemblyFiles)
		{
			return this.RegisterPackage(pinfo, true, assemblyFiles);
		}

		/// <summary>
		/// Registers a package.
		/// </summary>
		/// <param name="pinfo">
		/// Information about the package.
		/// </param>
		/// <param name="isInternal">
		/// Set to true if this package is provided by an add-in and is not installed in the system.
		/// </param>
		/// <param name="assemblyFiles">
		/// The assemblies of the package.
		/// </param>
		/// <returns>
		/// A <see cref="T:MonoDevelop.Core.Assemblies.SystemPackage" />
		/// </returns>
		// Token: 0x06000555 RID: 1365 RVA: 0x0001211E File Offset: 0x0001031E
		public SystemPackage RegisterPackage(SystemPackageInfo pinfo, bool isInternal, params string[] assemblyFiles)
		{
			this.EnsureInitialized();
			return this.assemblyContext.RegisterPackage(pinfo, isInternal, assemblyFiles);
		}

		/// <summary>
		/// Checks if a framework is installed in this runtime.
		/// </summary>
		/// <param name="fx">
		/// The runtime to check.
		/// </param>
		/// <returns>
		/// True if the framework is installed
		/// </returns>
		// Token: 0x06000556 RID: 1366 RVA: 0x00012134 File Offset: 0x00010334
		public bool IsInstalled(TargetFramework fx)
		{
			return this.GetBackend(fx).IsInstalled;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00012144 File Offset: 0x00010344
		private void CreateFrameworks()
		{
			HashSet<TargetFrameworkMoniker> hashSet = new HashSet<TargetFrameworkMoniker>();
			foreach (TargetFramework targetFramework in Runtime.SystemAssemblyService.GetKnownFrameworks())
			{
				if (hashSet.Add(targetFramework.Id) && this.IsInstalled(targetFramework))
				{
					this.timer.Trace("Registering assemblies for framework " + targetFramework.Id);
					this.RegisterSystemAssemblies(targetFramework);
				}
			}
			foreach (TargetFramework targetFramework2 in this.CustomFrameworks)
			{
				if (hashSet.Add(targetFramework2.Id) && this.IsInstalled(targetFramework2))
				{
					this.timer.Trace("Registering assemblies for framework " + targetFramework2.Id);
					this.RegisterSystemAssemblies(targetFramework2);
				}
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00012240 File Offset: 0x00010440
		protected bool IsCorePackage(string pname)
		{
			return this.corePackages.Contains(pname);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00012250 File Offset: 0x00010450
		private void RegisterSystemAssemblies(TargetFramework fx)
		{
			Dictionary<string, List<SystemAssembly>> dictionary = new Dictionary<string, List<SystemAssembly>>();
			Dictionary<string, SystemPackage> dictionary2 = new Dictionary<string, SystemPackage>();
			IEnumerable<string> frameworkFolders = this.GetFrameworkFolders(fx);
			foreach (AssemblyInfo assemblyInfo in fx.Assemblies)
			{
				foreach (string path in frameworkFolders)
				{
					string text = Path.Combine(path, assemblyInfo.Name) + ".dll";
					if (File.Exists(text))
					{
						if (assemblyInfo.Version == null && this.IsRunning)
						{
							try
							{
								AssemblyName assemblyNameObj = SystemAssemblyService.GetAssemblyNameObj(text);
								assemblyInfo.Update(assemblyNameObj);
							}
							catch
							{
							}
						}
						string key = assemblyInfo.Package ?? string.Empty;
						SystemPackage package;
						if (!dictionary2.TryGetValue(key, out package))
						{
							package = (dictionary2[key] = new SystemPackage());
							dictionary[key] = new List<SystemAssembly>();
						}
						List<SystemAssembly> list = dictionary[key];
						list.Add(this.assemblyContext.AddAssembly(text, assemblyInfo, package));
						break;
					}
				}
			}
			foreach (string text2 in dictionary2.Keys)
			{
				SystemPackage systemPackage = dictionary2[text2];
				List<SystemAssembly> list2 = dictionary[text2];
				SystemPackageInfo frameworkPackageInfo = this.GetFrameworkPackageInfo(fx, text2);
				if (!frameworkPackageInfo.IsCorePackage)
				{
					this.corePackages.Add(frameworkPackageInfo.Name);
				}
				systemPackage.Initialize(frameworkPackageInfo, list2.ToArray(), false);
				this.assemblyContext.InternalAddPackage(systemPackage);
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00012424 File Offset: 0x00010624
		protected virtual SystemPackageInfo GetFrameworkPackageInfo(TargetFramework fx, string packageName)
		{
			return this.GetBackend(fx).GetFrameworkPackageInfo(packageName);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00012433 File Offset: 0x00010633
		protected static IEnumerable<TargetFramework> FindTargetFrameworks(FilePath frameworksDirectory)
		{
			return TargetRuntime.FindTargetFrameworks(frameworksDirectory, false);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001285C File Offset: 0x00010A5C
		protected static IEnumerable<TargetFramework> FindTargetFrameworks(FilePath frameworksDirectory, bool rescanKnownFrameworks)
		{
			string[] directories = Directory.GetDirectories(frameworksDirectory);
			for (int i = 0; i < directories.Length; i++)
			{
				FilePath idDir = directories[i];
				FilePath filePath = idDir;
				string id = filePath.FileName;
				string[] directories2 = Directory.GetDirectories(idDir);
				for (int j = 0; j < directories2.Length; j++)
				{
					FilePath versionDir = directories2[j];
					FilePath filePath2 = versionDir;
					string version = filePath2.FileName;
					TargetFrameworkMoniker moniker = new TargetFrameworkMoniker(id, version);
					if (rescanKnownFrameworks || !Runtime.SystemAssemblyService.IsKnownFramework(moniker))
					{
						TargetFramework fx = TargetRuntime.ReadTargetFramework(moniker, versionDir);
						if (fx != null)
						{
							yield return fx;
						}
					}
					FilePath filePath3 = versionDir;
					FilePath profileListDir = filePath3.Combine(new string[]
					{
						"Profile"
					});
					if (Directory.Exists(profileListDir))
					{
						string[] directories3 = Directory.GetDirectories(profileListDir);
						for (int k = 0; k < directories3.Length; k++)
						{
							FilePath profileDir = directories3[k];
							FilePath filePath4 = profileDir;
							string profile = filePath4.FileName;
							moniker = new TargetFrameworkMoniker(id, version, profile);
							if (rescanKnownFrameworks || !Runtime.SystemAssemblyService.IsKnownFramework(moniker))
							{
								TargetFramework fx2 = TargetRuntime.ReadTargetFramework(moniker, profileDir);
								if (fx2 != null)
								{
									yield return fx2;
								}
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00012880 File Offset: 0x00010A80
		private static TargetFramework ReadTargetFramework(TargetFrameworkMoniker moniker, FilePath directory)
		{
			try
			{
				return TargetFramework.FromFrameworkDirectory(moniker, directory);
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error reading framework definition '" + directory + "'", ex);
			}
			return null;
		}

		// Token: 0x040001B7 RID: 439
		private HashSet<string> corePackages = new HashSet<string>();

		// Token: 0x040001B8 RID: 440
		private object initLock = new object();

		// Token: 0x040001B9 RID: 441
		private object initEventLock = new object();

		// Token: 0x040001BA RID: 442
		private bool initialized;

		// Token: 0x040001BB RID: 443
		private bool initializing;

		// Token: 0x040001BC RID: 444
		private bool backgroundInitialize;

		// Token: 0x040001BD RID: 445
		private bool extensionInitialized;

		// Token: 0x040001BE RID: 446
		private Dictionary<TargetFrameworkMoniker, TargetFrameworkBackend> frameworkBackends = new Dictionary<TargetFrameworkMoniker, TargetFrameworkBackend>();

		// Token: 0x040001BF RID: 447
		private RuntimeAssemblyContext assemblyContext;

		// Token: 0x040001C0 RID: 448
		private ComposedAssemblyContext composedAssemblyContext;

		// Token: 0x040001C1 RID: 449
		private ITimeTracker timer;

		// Token: 0x040001C2 RID: 450
		private TargetFramework[] customFrameworks = new TargetFramework[0];

		// Token: 0x040001C3 RID: 451
		private EventHandler initializedEvent;
	}
}
