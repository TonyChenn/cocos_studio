using System;
using System.Collections.Generic;
using System.IO;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000BC RID: 188
	public abstract class TargetFrameworkBackend
	{
		// Token: 0x06000670 RID: 1648
		public abstract bool SupportsRuntime(TargetRuntime runtime);

		// Token: 0x06000671 RID: 1649 RVA: 0x00018ADC File Offset: 0x00016CDC
		protected internal virtual void Initialize(TargetRuntime runtime, TargetFramework framework)
		{
			this.runtime = runtime;
			this.framework = framework;
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00018AEC File Offset: 0x00016CEC
		public virtual bool IsInstalled
		{
			get
			{
				if (this.framework.Assemblies.Length == 0)
				{
					return false;
				}
				foreach (string text in this.GetFrameworkFolders())
				{
					if (Directory.Exists(text))
					{
						string path = Path.Combine(text, "RedistList", "FrameworkList.xml");
						if (File.Exists(path))
						{
							return true;
						}
						string path2 = Path.Combine(text, this.framework.Assemblies[0].Name) + ".dll";
						if (File.Exists(path2))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06000673 RID: 1651
		public abstract IEnumerable<string> GetFrameworkFolders();

		// Token: 0x06000674 RID: 1652 RVA: 0x00018BA0 File Offset: 0x00016DA0
		public virtual Dictionary<string, string> GetToolsEnvironmentVariables()
		{
			return new Dictionary<string, string>();
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00018BA8 File Offset: 0x00016DA8
		public virtual string GetToolPath(string toolName)
		{
			foreach (string path in this.runtime.GetToolsPaths(this.framework))
			{
				string text = Path.Combine(path, toolName);
				if (Platform.IsWindows && File.Exists(text + ".bat"))
				{
					return text + ".bat";
				}
				if (File.Exists(text))
				{
					return text;
				}
				if (File.Exists(text + ".exe"))
				{
					return text + ".exe";
				}
			}
			return null;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00018EA0 File Offset: 0x000170A0
		public virtual IEnumerable<string> GetToolsPaths()
		{
			string paths;
			if (this.runtime.GetToolsExecutionEnvironment(this.framework).Variables.TryGetValue("PATH", out paths))
			{
				foreach (string path in paths.Split(new char[]
				{
					Path.PathSeparator
				}, StringSplitOptions.RemoveEmptyEntries))
				{
					if (path.Length > 0 && path[0] == '"' && path[path.Length - 1] == '"')
					{
						yield return path.Substring(1, path.Length - 2);
					}
					else
					{
						yield return path;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00018F60 File Offset: 0x00017160
		public virtual IEnumerable<string> GetAssemblyDirectories()
		{
			yield break;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00018F80 File Offset: 0x00017180
		public virtual SystemPackageInfo GetFrameworkPackageInfo(string packageName)
		{
			string text = (!string.IsNullOrEmpty(packageName)) ? packageName : this.framework.Name;
			return new SystemPackageInfo
			{
				Name = text,
				Description = text,
				IsFrameworkPackage = true,
				IsCorePackage = true,
				IsGacPackage = true,
				Version = this.framework.Id.Version,
				TargetFramework = this.framework.Id
			};
		}

		// Token: 0x04000228 RID: 552
		protected TargetRuntime runtime;

		// Token: 0x04000229 RID: 553
		protected TargetFramework framework;
	}
}
