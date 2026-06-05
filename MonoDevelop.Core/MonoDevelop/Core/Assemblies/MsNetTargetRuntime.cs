using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000B3 RID: 179
	public class MsNetTargetRuntime : TargetRuntime
	{
		// Token: 0x0600061C RID: 1564 RVA: 0x00016902 File Offset: 0x00014B02
		private static string GetProgramFilesX86()
		{
			return Environment.GetFolderPath((IntPtr.Size == 8) ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00016918 File Offset: 0x00014B18
		public MsNetTargetRuntime(bool running)
		{
			this.winDir = Path.GetFullPath(Environment.SystemDirectory + "\\..");
			this.rootDir = this.winDir + "\\Microsoft.NET\\Framework";
			string programFilesX = MsNetTargetRuntime.GetProgramFilesX86();
			this.newFxDir = programFilesX + "\\Reference Assemblies\\Microsoft\\Framework";
			this.msbuildDir = programFilesX + "\\MSBuild";
			this.running = running;
			this.execHandler = new MsNetExecutionHandler();
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x000169A4 File Offset: 0x00014BA4
		public override string DisplayRuntimeName
		{
			get
			{
				return "Microsoft .NET";
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x000169AB File Offset: 0x00014BAB
		public override string RuntimeId
		{
			get
			{
				return "MS.NET";
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x000169B2 File Offset: 0x00014BB2
		public override string Version
		{
			get
			{
				return "";
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x000169B9 File Offset: 0x00014BB9
		public FilePath RootDirectory
		{
			get
			{
				return this.rootDir;
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00016A98 File Offset: 0x00014C98
		public override IEnumerable<FilePath> GetReferenceFrameworkDirectories()
		{
			yield return this.newFxDir;
			yield break;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00016AB5 File Offset: 0x00014CB5
		public override string GetAssemblyDebugInfoFile(string assemblyPath)
		{
			return Path.ChangeExtension(assemblyPath, ".pdb");
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00016AC4 File Offset: 0x00014CC4
		protected override void OnInitialize()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\AssemblyFolders", false);
			if (registryKey != null)
			{
				foreach (string text in registryKey.GetSubKeyNames())
				{
					if (base.ShuttingDown)
					{
						return;
					}
					if (!text.StartsWith("Microsoft .NET Framework", StringComparison.Ordinal))
					{
						RegistryKey registryKey2 = registryKey.OpenSubKey(text, false);
						string text2 = registryKey2.GetValue("") as string;
						string version = (registryKey2.GetValue("version") as string) ?? "";
						if (!string.IsNullOrEmpty(text2))
						{
							this.AddPackage(text, version, text2, null);
						}
						registryKey2.Close();
					}
				}
				registryKey.Close();
			}
			foreach (TargetFramework targetFramework in Runtime.SystemAssemblyService.GetKnownFrameworks())
			{
				if (!(targetFramework.Id.Identifier != ".NETFramework"))
				{
					if (base.ShuttingDown)
					{
						break;
					}
					RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\v" + targetFramework.Id.Version + "\\AssemblyFoldersEx", false);
					if (registryKey3 != null)
					{
						this.AddPackages(targetFramework, registryKey3);
						registryKey3.Close();
					}
					string clrVersion = MsNetFrameworkBackend.GetClrVersion(targetFramework.ClrVersion);
					if (clrVersion.StartsWith("v" + targetFramework.Id.Version, StringComparison.Ordinal))
					{
						registryKey3 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\" + clrVersion + "\\AssemblyFoldersEx", false);
						if (registryKey3 != null)
						{
							this.AddPackages(targetFramework, registryKey3);
							registryKey3.Close();
						}
					}
				}
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00016C88 File Offset: 0x00014E88
		private void AddPackages(TargetFramework fx, RegistryKey fxKey)
		{
			foreach (string name in fxKey.GetSubKeyNames())
			{
				if (base.ShuttingDown)
				{
					break;
				}
				RegistryKey registryKey = fxKey.OpenSubKey(name, false);
				string text = registryKey.GetValue("") as string;
				string version = (registryKey.GetValue("version") as string) ?? "";
				if (!string.IsNullOrEmpty(text))
				{
					this.AddPackage(name, version, text, fx);
				}
				registryKey.Close();
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00016D0C File Offset: 0x00014F0C
		public override string GetMSBuildBinPath(string toolsVersion)
		{
			string result;
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions\\" + toolsVersion, false))
			{
				if (registryKey != null)
				{
					string text = registryKey.GetValue("MSBuildToolsPath") as string;
					if (text != null && File.Exists(Path.Combine(text, "MSBuild.exe")))
					{
						return text;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00016D7C File Offset: 0x00014F7C
		public override string GetMSBuildExtensionsPath()
		{
			return this.msbuildDir;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00016D8C File Offset: 0x00014F8C
		private void AddPackage(string name, string version, string folder, TargetFramework fx)
		{
			SystemPackageInfo systemPackageInfo = new SystemPackageInfo();
			systemPackageInfo.Name = name;
			systemPackageInfo.Description = name;
			systemPackageInfo.Version = version;
			systemPackageInfo.TargetFramework = ((fx != null) ? fx.Id : null);
			try
			{
				if (Directory.Exists(folder))
				{
					base.RegisterPackage(systemPackageInfo, false, Directory.GetFiles(folder, "*.dll"));
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error while scanning assembly folder '" + folder + "'", ex);
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x00016E10 File Offset: 0x00015010
		public override bool IsRunning
		{
			get
			{
				return this.running;
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00017134 File Offset: 0x00015334
		protected internal override IEnumerable<string> GetGacDirectories()
		{
			FilePath gacDir = this.winDir + "\\assembly\\GAC";
			if (Directory.Exists(gacDir))
			{
				yield return gacDir;
			}
			if (Directory.Exists(gacDir + "_32"))
			{
				yield return gacDir + "_32";
			}
			if (Directory.Exists(gacDir + "_64"))
			{
				yield return gacDir + "_64";
			}
			if (Directory.Exists(gacDir + "_MSIL"))
			{
				yield return gacDir + "_MSIL";
			}
			gacDir = this.winDir + "\\Microsoft.NET\\assembly\\GAC";
			if (Directory.Exists(gacDir))
			{
				yield return gacDir;
			}
			if (Directory.Exists(gacDir + "_32"))
			{
				yield return gacDir + "_32";
			}
			if (Directory.Exists(gacDir + "_64"))
			{
				yield return gacDir + "_64";
			}
			if (Directory.Exists(gacDir + "_MSIL"))
			{
				yield return gacDir + "_MSIL";
			}
			yield break;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00017151 File Offset: 0x00015351
		public override IExecutionHandler GetExecutionHandler()
		{
			return this.execHandler;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00017159 File Offset: 0x00015359
		protected override TargetFrameworkBackend CreateBackend(TargetFramework fx)
		{
			return new MsNetFrameworkBackend();
		}

		// Token: 0x0400020B RID: 523
		private FilePath rootDir;

		// Token: 0x0400020C RID: 524
		private FilePath newFxDir;

		// Token: 0x0400020D RID: 525
		private FilePath msbuildDir;

		// Token: 0x0400020E RID: 526
		private bool running;

		// Token: 0x0400020F RID: 527
		private MsNetExecutionHandler execHandler;

		// Token: 0x04000210 RID: 528
		private string winDir;
	}
}
