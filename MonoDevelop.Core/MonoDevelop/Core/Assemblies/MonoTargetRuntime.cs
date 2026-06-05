using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Mono.PkgConfig;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x0200009F RID: 159
	public class MonoTargetRuntime : TargetRuntime
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x000128C8 File Offset: 0x00010AC8
		internal MonoTargetRuntime(MonoRuntimeInfo info)
		{
			this.monoVersion = info.MonoVersion;
			this.monoDir = Path.Combine(Path.Combine(info.Prefix, "lib"), "mono");
			this.environmentVariables = info.GetEnvironmentVariables();
			this.monoRuntimeInfo = info;
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001291A File Offset: 0x00010B1A
		public MonoRuntimeInfo MonoRuntimeInfo
		{
			get
			{
				return this.monoRuntimeInfo;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x00012922 File Offset: 0x00010B22
		public string Prefix
		{
			get
			{
				return this.monoRuntimeInfo.Prefix;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0001292F File Offset: 0x00010B2F
		public string MonoDirectory
		{
			get
			{
				return this.monoDir;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x00012937 File Offset: 0x00010B37
		public Dictionary<string, string> EnvironmentVariables
		{
			get
			{
				return this.environmentVariables;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x0001293F File Offset: 0x00010B3F
		public override bool IsRunning
		{
			get
			{
				return this.monoRuntimeInfo.IsRunning;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0001294C File Offset: 0x00010B4C
		public override string RuntimeId
		{
			get
			{
				return "Mono";
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00012953 File Offset: 0x00010B53
		public override string Version
		{
			get
			{
				return this.monoVersion;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0001295B File Offset: 0x00010B5B
		public override string DisplayName
		{
			get
			{
				if (!this.IsRunning)
				{
					return base.DisplayName + " (" + this.Prefix + ")";
				}
				return base.DisplayName;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00012987 File Offset: 0x00010B87
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x0001298F File Offset: 0x00010B8F
		public bool HasMultitargetingMcs { get; private set; }

		// Token: 0x0600056A RID: 1386 RVA: 0x00012998 File Offset: 0x00010B98
		public override IEnumerable<FilePath> GetReferenceFrameworkDirectories()
		{
			return this.GetReferenceFrameworkDirectories(base.IsInitialized || this.IsRunning);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00012BFC File Offset: 0x00010DFC
		private IEnumerable<FilePath> GetReferenceFrameworkDirectories(bool includeGlobalDirectories)
		{
			string env;
			if (this.environmentVariables.TryGetValue("XBUILD_FRAMEWORK_FOLDERS_PATH", out env) && !string.IsNullOrEmpty(env))
			{
				foreach (string dir in env.Split(new char[]
				{
					Path.PathSeparator
				}, StringSplitOptions.RemoveEmptyEntries))
				{
					yield return dir;
				}
			}
			if (includeGlobalDirectories && Platform.IsMac)
			{
				yield return "/Library/Frameworks/Mono.framework/External/xbuild-frameworks";
			}
			yield return Path.Combine(this.monoDir, "xbuild-frameworks");
			yield break;
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00012C20 File Offset: 0x00010E20
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x00012C28 File Offset: 0x00010E28
		public bool UserDefined { get; internal set; }

		// Token: 0x0600056E RID: 1390 RVA: 0x00012C31 File Offset: 0x00010E31
		public override string GetAssemblyDebugInfoFile(string assemblyPath)
		{
			return assemblyPath + ".mdb";
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00012C3E File Offset: 0x00010E3E
		protected override TargetFrameworkBackend CreateBackend(TargetFramework fx)
		{
			return new MonoFrameworkBackend();
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00012C48 File Offset: 0x00010E48
		public override IExecutionHandler GetExecutionHandler()
		{
			if (this.execHandler == null)
			{
				string monoPath = Path.Combine(Path.Combine(this.MonoRuntimeInfo.Prefix, "bin"), "mono");
				this.execHandler = new MonoPlatformExecutionHandler(monoPath, this.environmentVariables);
			}
			return this.execHandler;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00012C98 File Offset: 0x00010E98
		protected override void ConvertAssemblyProcessStartInfo(ProcessStartInfo pinfo)
		{
			pinfo.Arguments = "\"" + pinfo.FileName + "\" " + pinfo.Arguments;
			pinfo.FileName = Path.Combine(Path.Combine(this.MonoRuntimeInfo.Prefix, "bin"), "mono");
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00012CEB File Offset: 0x00010EEB
		public override string GetToolPath(TargetFramework fx, string toolName)
		{
			if (fx.ClrVersion == ClrVersion.Net_2_0 && toolName == "al")
			{
				toolName = "al2";
			}
			return base.GetToolPath(fx, toolName);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00012F10 File Offset: 0x00011110
		protected internal override IEnumerable<string> GetGacDirectories()
		{
			yield return Path.Combine(this.monoDir, "gac");
			string gacs;
			if (this.environmentVariables.TryGetValue("MONO_GAC_PREFIX", out gacs) && !string.IsNullOrEmpty(gacs))
			{
				foreach (string path in gacs.Split(new char[]
				{
					Path.PathSeparator
				}, StringSplitOptions.RemoveEmptyEntries))
				{
					yield return path;
				}
			}
			yield break;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00012F30 File Offset: 0x00011130
		public override string GetMSBuildBinPath(string toolsVersion)
		{
			string text = Path.Combine(this.monoDir, toolsVersion);
			if (File.Exists(Path.Combine(text, "xbuild.exe")))
			{
				return text;
			}
			if (toolsVersion == "4.0")
			{
				return this.GetMSBuildBinPath("4.5");
			}
			return null;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00012F78 File Offset: 0x00011178
		public override string GetMSBuildExtensionsPath()
		{
			return Path.Combine(this.monoDir, "xbuild");
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x00012F8A File Offset: 0x0001118A
		public IEnumerable<string> PkgConfigDirs
		{
			get
			{
				return this.GetPkgConfigDirs(base.IsInitialized || this.IsRunning);
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00013170 File Offset: 0x00011370
		private IEnumerable<string> GetPkgConfigDirs(bool includeGlobalDirectories)
		{
			foreach (string s in this.PkgConfigPath.Split(new char[]
			{
				Path.PathSeparator
			}))
			{
				yield return s;
			}
			if (includeGlobalDirectories && Platform.IsMac)
			{
				yield return "/Library/Frameworks/Mono.framework/External/pkgconfig";
			}
			yield break;
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00013194 File Offset: 0x00011394
		public string PkgConfigPath
		{
			get
			{
				return this.environmentVariables["PKG_CONFIG_PATH"];
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001344C File Offset: 0x0001164C
		public IEnumerable<string> GetAllPkgConfigFiles()
		{
			HashSet<string> packageNames = new HashSet<string>();
			foreach (string pcdir in this.PkgConfigDirs)
			{
				if (Directory.Exists(pcdir))
				{
					string[] files;
					try
					{
						files = Directory.GetFiles(pcdir, "*.pc");
					}
					catch (Exception ex)
					{
						LoggingService.LogError(string.Format("Runtime '{0}' error in pc file scan of directory '{1}'", this.DisplayName, pcdir), ex);
						continue;
					}
					foreach (string pcfile in files)
					{
						if (packageNames.Add(Path.GetFileNameWithoutExtension(pcfile)))
						{
							yield return pcfile;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001346C File Offset: 0x0001166C
		protected override void OnInitialize()
		{
			if (!this.monoRuntimeInfo.IsValidRuntime)
			{
				return;
			}
			foreach (string text in this.GetAllPkgConfigFiles())
			{
				try
				{
					this.ParsePCFile(FileService.ResolveFullPath(text));
					if (base.ShuttingDown)
					{
						return;
					}
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Could not parse file '" + text + "'", ex);
				}
			}
			this.HasMultitargetingMcs = File.Exists(Path.Combine(this.monoDir, "4.5", "mscorlib.dll"));
			MonoTargetRuntime.PcFileCache.Save();
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00013534 File Offset: 0x00011734
		private void ParsePCFile(string pcfile)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pcfile);
			if (base.RuntimeAssemblyContext.GetPackageInternal(fileNameWithoutExtension) != null || base.IsCorePackage(fileNameWithoutExtension))
			{
				return;
			}
			LibraryPackageInfo packageInfo = MonoTargetRuntime.PcFileCache.GetPackageInfo(pcfile);
			if (packageInfo.IsValidPackage)
			{
				base.RuntimeAssemblyContext.RegisterPackage(packageInfo, false);
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00013582 File Offset: 0x00011782
		public static TargetRuntime RegisterRuntime(MonoRuntimeInfo info)
		{
			return MonoTargetRuntimeFactory.RegisterRuntime(info);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001358A File Offset: 0x0001178A
		public static void UnregisterRuntime(MonoTargetRuntime runtime)
		{
			MonoTargetRuntimeFactory.UnregisterRuntime(runtime);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00013592 File Offset: 0x00011792
		public override ExecutionEnvironment GetToolsExecutionEnvironment()
		{
			return new ExecutionEnvironment(this.EnvironmentVariables);
		}

		// Token: 0x040001C5 RID: 453
		private readonly string monoVersion;

		// Token: 0x040001C6 RID: 454
		private readonly string monoDir;

		// Token: 0x040001C7 RID: 455
		private MonoPlatformExecutionHandler execHandler;

		// Token: 0x040001C8 RID: 456
		private readonly Dictionary<string, string> environmentVariables;

		// Token: 0x040001C9 RID: 457
		internal static LibraryPcFileCache PcFileCache = new LibraryPcFileCache(new PcFileCacheContext());

		// Token: 0x040001CA RID: 458
		private readonly MonoRuntimeInfo monoRuntimeInfo;
	}
}
