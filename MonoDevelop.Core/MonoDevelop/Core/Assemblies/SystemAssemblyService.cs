using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using IKVM.Reflection;
using Mono.Addins;
using MonoDevelop.Core.AddIns;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000A7 RID: 167
	public sealed class SystemAssemblyService
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00014A3A File Offset: 0x00012C3A
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00014A42 File Offset: 0x00012C42
		public TargetRuntime CurrentRuntime { get; private set; }

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x060005AD RID: 1453 RVA: 0x00014A4C File Offset: 0x00012C4C
		// (remove) Token: 0x060005AE RID: 1454 RVA: 0x00014A84 File Offset: 0x00012C84
		public event EventHandler DefaultRuntimeChanged;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060005AF RID: 1455 RVA: 0x00014ABC File Offset: 0x00012CBC
		// (remove) Token: 0x060005B0 RID: 1456 RVA: 0x00014AF4 File Offset: 0x00012CF4
		public event EventHandler RuntimesChanged;

		// Token: 0x060005B1 RID: 1457 RVA: 0x00014B34 File Offset: 0x00012D34
		internal void Initialize()
		{
			this.CreateFrameworks();
			this.runtimes = new List<TargetRuntime>();
			foreach (ITargetRuntimeFactory targetRuntimeFactory in AddinManager.GetExtensionObjects("/MonoDevelop/Core/Runtimes", typeof(ITargetRuntimeFactory)))
			{
				foreach (TargetRuntime targetRuntime in targetRuntimeFactory.CreateRuntimes())
				{
					this.runtimes.Add(targetRuntime);
					if (targetRuntime.IsRunning)
					{
						this.DefaultRuntime = (this.CurrentRuntime = targetRuntime);
					}
				}
			}
			foreach (TargetRuntime targetRuntime2 in this.runtimes)
			{
				targetRuntime2.Initialized += this.HandleRuntimeInitialized;
			}
			if (this.CurrentRuntime == null)
			{
				LoggingService.LogFatalError("Could not create runtime info for current runtime");
			}
			this.CurrentRuntime.StartInitialization();
			this.LoadUserAssemblyContext();
			this.userAssemblyContext.Changed += delegate(object param0, EventArgs param1)
			{
				this.SaveUserAssemblyContext();
			};
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00014C70 File Offset: 0x00012E70
		private void InitializeRuntime(TargetRuntime runtime)
		{
			runtime.Initialized += this.HandleRuntimeInitialized;
			runtime.StartInitialization();
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00014C8C File Offset: 0x00012E8C
		private void HandleRuntimeInitialized(object sender, EventArgs e)
		{
			TargetRuntime targetRuntime = (TargetRuntime)sender;
			if (targetRuntime.CustomFrameworks.Any<TargetFramework>())
			{
				this.UpdateFrameworks(targetRuntime.CustomFrameworks);
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00014CBC File Offset: 0x00012EBC
		private void UpdateFrameworks(IEnumerable<TargetFramework> toAdd)
		{
			lock (this.frameworkWriteLock)
			{
				Dictionary<TargetFrameworkMoniker, TargetFramework> dictionary = new Dictionary<TargetFrameworkMoniker, TargetFramework>(this.frameworks);
				bool flag2 = false;
				foreach (TargetFramework targetFramework in toAdd)
				{
					TargetFramework targetFramework2;
					if (!dictionary.TryGetValue(targetFramework.Id, out targetFramework2) || targetFramework2.Assemblies.Length == 0)
					{
						dictionary[targetFramework.Id] = targetFramework;
						flag2 = true;
					}
				}
				if (flag2)
				{
					SystemAssemblyService.BuildFrameworkRelations(dictionary);
					this.frameworks = dictionary;
				}
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00014D78 File Offset: 0x00012F78
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x00014D80 File Offset: 0x00012F80
		public TargetRuntime DefaultRuntime
		{
			get
			{
				return this.defaultRuntime;
			}
			set
			{
				this.defaultRuntime = value;
				if (this.DefaultRuntimeChanged != null)
				{
					this.DefaultRuntimeChanged(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00014DA2 File Offset: 0x00012FA2
		public DirectoryAssemblyContext UserAssemblyContext
		{
			get
			{
				return this.userAssemblyContext;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00014DAA File Offset: 0x00012FAA
		public IAssemblyContext DefaultAssemblyContext
		{
			get
			{
				return this.DefaultRuntime.AssemblyContext;
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00014DB7 File Offset: 0x00012FB7
		public void RegisterRuntime(TargetRuntime runtime)
		{
			runtime.Initialized += this.HandleRuntimeInitialized;
			this.runtimes.Add(runtime);
			if (this.RuntimesChanged != null)
			{
				this.RuntimesChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00014DF0 File Offset: 0x00012FF0
		public void UnregisterRuntime(TargetRuntime runtime)
		{
			if (runtime == this.CurrentRuntime)
			{
				return;
			}
			this.DefaultRuntime = this.CurrentRuntime;
			this.runtimes.Remove(runtime);
			runtime.Initialized -= this.HandleRuntimeInitialized;
			if (this.RuntimesChanged != null)
			{
				this.RuntimesChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00014E4B File Offset: 0x0001304B
		internal IEnumerable<TargetFramework> GetKnownFrameworks()
		{
			return this.frameworks.Values;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00014E58 File Offset: 0x00013058
		internal bool IsKnownFramework(TargetFrameworkMoniker moniker)
		{
			return this.frameworks.ContainsKey(moniker);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00014E66 File Offset: 0x00013066
		public IEnumerable<TargetFramework> GetTargetFrameworks()
		{
			return this.frameworks.Values;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00014E73 File Offset: 0x00013073
		public IEnumerable<TargetRuntime> GetTargetRuntimes()
		{
			return this.runtimes;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00014E7C File Offset: 0x0001307C
		public TargetRuntime GetTargetRuntime(string id)
		{
			foreach (TargetRuntime targetRuntime in this.runtimes)
			{
				if (targetRuntime.Id == id)
				{
					return targetRuntime;
				}
			}
			return null;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001508C File Offset: 0x0001328C
		public IEnumerable<TargetRuntime> GetTargetRuntimes(string runtimeId)
		{
			foreach (TargetRuntime r in this.runtimes)
			{
				if (r.RuntimeId == runtimeId)
				{
					yield return r;
				}
			}
			yield break;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000150B0 File Offset: 0x000132B0
		public TargetFramework GetTargetFramework(TargetFrameworkMoniker id)
		{
			TargetFramework result;
			if (this.frameworks.TryGetValue(id, out result))
			{
				return result;
			}
			LoggingService.LogDebug("Unregistered TargetFramework '{0}' is being requested from SystemAssemblyService, ensuring rutimes initialized and trying again", new object[]
			{
				id
			});
			foreach (TargetRuntime targetRuntime in this.runtimes)
			{
				targetRuntime.EnsureInitialized();
			}
			if (this.frameworks.TryGetValue(id, out result))
			{
				return result;
			}
			LoggingService.LogWarning("Unregistered TargetFramework '{0}' is being requested from SystemAssemblyService, returning empty TargetFramework", new object[]
			{
				id
			});
			this.UpdateFrameworks(new TargetFramework[]
			{
				new TargetFramework(id)
			});
			return this.frameworks[id];
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001517C File Offset: 0x0001337C
		public SystemPackage GetPackageFromPath(string assemblyPath)
		{
			foreach (TargetRuntime targetRuntime in this.runtimes)
			{
				SystemPackage packageFromPath = targetRuntime.AssemblyContext.GetPackageFromPath(assemblyPath);
				if (packageFromPath != null)
				{
					return packageFromPath;
				}
			}
			return null;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x000151E0 File Offset: 0x000133E0
		public static System.Reflection.AssemblyName ParseAssemblyName(string fullname)
		{
			System.Reflection.AssemblyName assemblyName = new System.Reflection.AssemblyName();
			int num = fullname.IndexOf(',');
			if (num == -1)
			{
				assemblyName.Name = fullname.Trim();
				return assemblyName;
			}
			assemblyName.Name = fullname.Substring(0, num).Trim();
			num = fullname.IndexOf("Version", num + 1, StringComparison.Ordinal);
			if (num == -1)
			{
				return assemblyName;
			}
			num = fullname.IndexOf('=', num);
			if (num == -1)
			{
				return assemblyName;
			}
			int num2 = fullname.IndexOf(',', num);
			if (num2 == -1)
			{
				assemblyName.Version = new Version(fullname.Substring(num + 1).Trim());
			}
			else
			{
				assemblyName.Version = new Version(fullname.Substring(num + 1, num2 - num - 1).Trim());
			}
			return assemblyName;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00015290 File Offset: 0x00013490
		internal static System.Reflection.AssemblyName GetAssemblyNameObj(string file)
		{
			lock (SystemAssemblyService.assemblyNameCache)
			{
				System.Reflection.AssemblyName assemblyName;
				if (SystemAssemblyService.assemblyNameCache.TryGetValue(file, out assemblyName))
				{
					return assemblyName;
				}
			}
			System.Reflection.AssemblyName result;
			try
			{
				System.Reflection.AssemblyName assemblyName = System.Reflection.AssemblyName.GetAssemblyName(file);
				lock (SystemAssemblyService.assemblyNameCache)
				{
					SystemAssemblyService.assemblyNameCache[file] = assemblyName;
				}
				result = assemblyName;
			}
			catch (FileNotFoundException)
			{
				foreach (string text in Directory.GetFiles(Path.GetDirectoryName(file), Path.GetFileName(file)))
				{
					if (text != file)
					{
						SystemAssemblyService.GetAssemblyNameObj(text);
						return SystemAssemblyService.assemblyNameCache[file];
					}
				}
				throw;
			}
			return result;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00015380 File Offset: 0x00013580
		public static string GetAssemblyName(string file)
		{
			return AssemblyContext.NormalizeAsmName(SystemAssemblyService.GetAssemblyNameObj(file).ToString());
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00015394 File Offset: 0x00013594
		private void CreateFrameworks()
		{
			this.frameworks = new Dictionary<TargetFrameworkMoniker, TargetFramework>();
			this.coreFrameworks = new List<TargetFrameworkMoniker>();
			foreach (object obj in AddinManager.GetExtensionNodes("/MonoDevelop/Core/Frameworks"))
			{
				TargetFrameworkNode targetFrameworkNode = (TargetFrameworkNode)obj;
				try
				{
					TargetFramework targetFramework = targetFrameworkNode.CreateFramework();
					if (this.frameworks.ContainsKey(targetFramework.Id))
					{
						LoggingService.LogError("Duplicate framework '" + targetFramework.Id + "'");
					}
					else
					{
						this.coreFrameworks.Add(targetFramework.Id);
						this.frameworks[targetFramework.Id] = targetFramework;
					}
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Could not load framework '" + targetFrameworkNode.Id + "'", ex);
				}
			}
			SystemAssemblyService.BuildFrameworkRelations(this.frameworks);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001549C File Offset: 0x0001369C
		private static void BuildFrameworkRelations(Dictionary<TargetFrameworkMoniker, TargetFramework> frameworks)
		{
			foreach (TargetFramework fx in frameworks.Values)
			{
				SystemAssemblyService.BuildFrameworkRelations(fx, frameworks);
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000154F0 File Offset: 0x000136F0
		private static void BuildFrameworkRelations(TargetFramework fx, Dictionary<TargetFrameworkMoniker, TargetFramework> frameworks)
		{
			if (fx.RelationsBuilt)
			{
				return;
			}
			TargetFrameworkMoniker includesFramework = fx.GetIncludesFramework();
			if (includesFramework != null)
			{
				fx.IncludedFrameworks.Add(includesFramework);
				TargetFramework targetFramework;
				if (frameworks.TryGetValue(includesFramework, out targetFramework))
				{
					SystemAssemblyService.BuildFrameworkRelations(targetFramework, frameworks);
					fx.IncludedFrameworks.AddRange(targetFramework.IncludedFrameworks);
				}
				else
				{
					LoggingService.LogWarning("TargetFramework '{0}' imports unknown framework '{0}'", new object[]
					{
						fx.Id,
						includesFramework
					});
				}
			}
			fx.RelationsBuilt = true;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00015580 File Offset: 0x00013780
		public static Universe CreateClosedUniverse()
		{
			Universe universe = new Universe(UniverseOptions.DisablePseudoCustomAttributeRetrieval | UniverseOptions.ResolveMissingMembers | UniverseOptions.SupressReferenceTypeIdentityConversion);
			universe.AssemblyResolve += ((object sender, IKVM.Reflection.ResolveEventArgs args) => ((Universe)sender).CreateMissingAssembly(args.Name));
			return universe;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000155D4 File Offset: 0x000137D4
		public TargetFrameworkMoniker GetTargetFrameworkForAssembly(TargetRuntime tr, string file)
		{
			Universe universe = SystemAssemblyService.CreateClosedUniverse();
			try
			{
				IKVM.Reflection.Assembly assembly = universe.LoadFile(file);
				IKVM.Reflection.CustomAttributeData customAttributeData = assembly.CustomAttributes.FirstOrDefault((IKVM.Reflection.CustomAttributeData a) => a.AttributeType.FullName == "System.Runtime.Versioning.TargetFrameworkAttribute");
				if (customAttributeData != null)
				{
					if (customAttributeData.ConstructorArguments.Count == 1)
					{
						string text = customAttributeData.ConstructorArguments[0].Value as string;
						TargetFrameworkMoniker result;
						if (text != null && TargetFrameworkMoniker.TryParse(text, out result))
						{
							return result;
						}
					}
					LoggingService.LogError("Invalid TargetFrameworkAttribute in assembly {0}", new object[]
					{
						file
					});
				}
				IKVM.Reflection.AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
				int i = 0;
				while (i < referencedAssemblies.Length)
				{
					IKVM.Reflection.AssemblyName assemblyName = referencedAssemblies[i];
					if (assemblyName.Name == "mscorlib")
					{
						TargetFramework targetFramework = null;
						foreach (TargetFramework targetFramework2 in this.GetKnownFrameworks())
						{
							if (targetFramework2.GetCorlibVersion() == assemblyName.Version.ToString())
							{
								targetFramework = targetFramework2;
								if (tr.IsInstalled(targetFramework2))
								{
									return targetFramework2.Id;
								}
							}
						}
						if (targetFramework != null)
						{
							return targetFramework.Id;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error determining target framework for assembly {0}: {1}", new object[]
				{
					file,
					ex
				});
				return TargetFrameworkMoniker.UNKNOWN;
			}
			finally
			{
				universe.Dispose();
			}
			LoggingService.LogError("Failed to determine target framework for assembly {0}", new object[]
			{
				file
			});
			return TargetFrameworkMoniker.UNKNOWN;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000157C8 File Offset: 0x000139C8
		private void SaveUserAssemblyContext()
		{
			List<string> val = new List<string>(this.userAssemblyContext.Directories);
			PropertyService.Set("MonoDevelop.Core.Assemblies.UserAssemblyContext", val);
			PropertyService.SaveProperties();
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000157F8 File Offset: 0x000139F8
		private void LoadUserAssemblyContext()
		{
			List<string> list = PropertyService.Get<List<string>>("MonoDevelop.Core.Assemblies.UserAssemblyContext");
			if (list != null)
			{
				this.userAssemblyContext.Directories = list;
			}
		}

		/// <summary>
		/// Simply get all assembly reference names from an assembly given it's file name.
		/// </summary>
		// Token: 0x060005CD RID: 1485 RVA: 0x00015A38 File Offset: 0x00013C38
		public static IEnumerable<string> GetAssemblyReferences(string fileName)
		{
			using (Universe universe = new Universe())
			{
				IKVM.Reflection.Assembly assembly;
				try
				{
					assembly = universe.LoadFile(fileName);
				}
				catch
				{
					yield break;
				}
				foreach (IKVM.Reflection.AssemblyName r in assembly.GetReferencedAssemblies())
				{
					yield return r.Name;
				}
			}
			yield break;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00015A58 File Offset: 0x00013C58
		public static bool ContainsReferenceToSystemRuntime(string fileName)
		{
			using (Universe universe = new Universe())
			{
				IKVM.Reflection.Assembly assembly;
				try
				{
					assembly = universe.LoadFile(fileName);
				}
				catch
				{
					return false;
				}
				foreach (IKVM.Reflection.AssemblyName assemblyName in assembly.GetReferencedAssemblies())
				{
					if (assemblyName.FullName.Equals("System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Simply get all assembly manifest resources from an assembly given it's file name.
		/// </summary>
		// Token: 0x060005CF RID: 1487 RVA: 0x00015D94 File Offset: 0x00013F94
		public static IEnumerable<SystemAssemblyService.ManifestResource> GetAssemblyManifestResources(string fileName)
		{
			using (Universe universe = new Universe())
			{
				IKVM.Reflection.Assembly assembly;
				try
				{
					assembly = universe.LoadFile(fileName);
				}
				catch
				{
					yield break;
				}
				string[] manifestResourceNames = assembly.GetManifestResourceNames();
				for (int i = 0; i < manifestResourceNames.Length; i++)
				{
					string _r = manifestResourceNames[i];
					string r = _r;
					yield return new SystemAssemblyService.ManifestResource(r, () => assembly.GetManifestResourceStream(r));
				}
			}
			yield break;
		}

		// Token: 0x040001EB RID: 491
		private object frameworkWriteLock = new object();

		// Token: 0x040001EC RID: 492
		private Dictionary<TargetFrameworkMoniker, TargetFramework> frameworks;

		// Token: 0x040001ED RID: 493
		private List<TargetFrameworkMoniker> coreFrameworks;

		// Token: 0x040001EE RID: 494
		private List<TargetRuntime> runtimes;

		// Token: 0x040001EF RID: 495
		private TargetRuntime defaultRuntime;

		// Token: 0x040001F0 RID: 496
		private DirectoryAssemblyContext userAssemblyContext = new DirectoryAssemblyContext();

		// Token: 0x040001F3 RID: 499
		private static readonly Dictionary<string, System.Reflection.AssemblyName> assemblyNameCache = new Dictionary<string, System.Reflection.AssemblyName>();

		// Token: 0x020000A8 RID: 168
		public class ManifestResource
		{
			// Token: 0x17000143 RID: 323
			// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00015DDB File Offset: 0x00013FDB
			// (set) Token: 0x060005D6 RID: 1494 RVA: 0x00015DE3 File Offset: 0x00013FE3
			public string Name { get; private set; }

			// Token: 0x060005D7 RID: 1495 RVA: 0x00015DEC File Offset: 0x00013FEC
			public Stream Open()
			{
				return this.streamCallback();
			}

			// Token: 0x060005D8 RID: 1496 RVA: 0x00015DF9 File Offset: 0x00013FF9
			public ManifestResource(string name, Func<Stream> streamCallback)
			{
				this.streamCallback = streamCallback;
				this.Name = name;
			}

			// Token: 0x040001F7 RID: 503
			private Func<Stream> streamCallback;
		}
	}
}
