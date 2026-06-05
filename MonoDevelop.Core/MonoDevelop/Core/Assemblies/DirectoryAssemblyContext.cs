using System;
using System.Collections.Generic;
using System.IO;
using Mono.PkgConfig;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000D8 RID: 216
	public class DirectoryAssemblyContext : AssemblyContext
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x0001F3CE File Offset: 0x0001D5CE
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x0001F3D8 File Offset: 0x0001D5D8
		public IEnumerable<string> Directories
		{
			get
			{
				return this.directories;
			}
			set
			{
				lock (this.updatesLock)
				{
					this.directories = new List<string>(value);
					if (!this.pendingUpdate)
					{
						this.pendingUpdate = true;
						Runtime.SystemAssemblyService.CurrentRuntime.Initialized += this.OnUpdatePackages;
					}
					base.NotifyChanged();
				}
			}
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001F450 File Offset: 0x0001D650
		private void OnUpdatePackages(object o, EventArgs a)
		{
			lock (this.updatesLock)
			{
				Runtime.SystemAssemblyService.CurrentRuntime.Initialized -= this.OnUpdatePackages;
				foreach (SystemPackage p in this.packages)
				{
					base.UnregisterPackage(p);
				}
				this.packages.Clear();
				foreach (string text in this.directories)
				{
					if (Directory.Exists(text))
					{
						try
						{
							foreach (string text2 in Directory.GetFiles(text))
							{
								string extension = Path.GetExtension(text2);
								if (extension == ".dll")
								{
									this.RegisterAssembly(text2);
								}
								else if (extension == ".pc")
								{
									this.RegisterPcFile(text2);
								}
							}
							continue;
						}
						catch (Exception ex)
						{
							LoggingService.LogError("Error while updating assemblies from directory: " + text, ex);
							continue;
						}
					}
					LoggingService.LogWarning("User defined assembly forlder doesn't exist: " + text);
				}
				this.pendingUpdate = false;
			}
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001F600 File Offset: 0x0001D800
		private void RegisterAssembly(string file)
		{
			SystemPackage item = base.RegisterPackage(new SystemPackageInfo
			{
				Name = file,
				Description = Path.GetDirectoryName(file),
				Version = "",
				IsGacPackage = false,
				TargetFramework = Runtime.SystemAssemblyService.GetTargetFrameworkForAssembly(Runtime.SystemAssemblyService.CurrentRuntime, file)
			}, true, new string[]
			{
				file
			});
			this.packages.Add(item);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001F674 File Offset: 0x0001D874
		private void RegisterPcFile(string file)
		{
			LibraryPackageInfo packageInfo = MonoTargetRuntime.PcFileCache.GetPackageInfo(file);
			if (packageInfo.IsValidPackage)
			{
				SystemPackage item = base.RegisterPackage(packageInfo, true);
				this.packages.Add(item);
			}
		}

		// Token: 0x04000278 RID: 632
		private List<SystemPackage> packages = new List<SystemPackage>();

		// Token: 0x04000279 RID: 633
		private List<string> directories = new List<string>();

		// Token: 0x0400027A RID: 634
		private object updatesLock = new object();

		// Token: 0x0400027B RID: 635
		private bool pendingUpdate;
	}
}
