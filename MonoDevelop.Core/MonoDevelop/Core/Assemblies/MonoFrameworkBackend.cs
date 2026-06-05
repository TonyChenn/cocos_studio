using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000BF RID: 191
	public class MonoFrameworkBackend : TargetFrameworkBackend<MonoTargetRuntime>
	{
		// Token: 0x06000681 RID: 1665 RVA: 0x00019038 File Offset: 0x00017238
		private string GetReferenceAssembliesFolder()
		{
			if (this.ref_assemblies_folder != null)
			{
				return this.ref_assemblies_folder;
			}
			string assemblyDirectoryName = this.framework.Id.GetAssemblyDirectoryName();
			foreach (FilePath filePath in ((MonoTargetRuntime)this.runtime).GetReferenceFrameworkDirectories())
			{
				FilePath filePath2 = filePath.Combine(new string[]
				{
					assemblyDirectoryName
				});
				FilePath filePath3 = filePath2.Combine(new string[]
				{
					"RedistList",
					"FrameworkList.xml"
				});
				if (File.Exists(filePath3))
				{
					using (XmlReader xmlReader = XmlReader.Create(filePath3))
					{
						if (xmlReader.ReadToDescendant("FileList") && xmlReader.MoveToAttribute("TargetFrameworkDirectory") && xmlReader.ReadAttributeValue())
						{
							string text = xmlReader.ReadContentAsString();
							if (!string.IsNullOrEmpty(text))
							{
								text = text.Replace('\\', Path.DirectorySeparatorChar);
								filePath2 = filePath3.ParentDirectory.Combine(new string[]
								{
									text
								}).FullPath;
							}
						}
					}
					this.ref_assemblies_folder = filePath2;
					return filePath2;
				}
			}
			return null;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00019428 File Offset: 0x00017628
		public override IEnumerable<string> GetFrameworkFolders()
		{
			string dir = this.GetReferenceAssembliesFolder();
			if (dir != null)
			{
				yield return dir;
			}
			if (!(this.framework.Id.Identifier != ".NETFramework"))
			{
				string version;
				string subdir;
				if ((version = this.framework.Id.Version) != null)
				{
					if (version == "1.1")
					{
						subdir = "1.0";
						goto IL_19F;
					}
					if (version == "3.0")
					{
						yield return Path.Combine(this.targetRuntime.MonoDirectory, "2.0");
						yield return Path.Combine(this.targetRuntime.MonoDirectory, "3.0");
						goto IL_1D0;
					}
					if (version == "3.5")
					{
						yield return Path.Combine(this.targetRuntime.MonoDirectory, "3.5");
						subdir = "2.0";
						goto IL_19F;
					}
				}
				subdir = this.framework.Id.Version;
				IL_19F:
				yield return Path.Combine(this.targetRuntime.MonoDirectory, subdir);
			}
			IL_1D0:
			yield break;
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00019448 File Offset: 0x00017648
		public override bool IsInstalled
		{
			get
			{
				if (!(this.framework.Id.Identifier == ".NETFramework") || !(this.framework.Id.Version == "3.0"))
				{
					return base.IsInstalled;
				}
				if (base.IsInstalled)
				{
					return true;
				}
				string text = Path.Combine(this.targetRuntime.MonoDirectory, "2.0");
				if (Directory.Exists(text))
				{
					string path = Path.Combine(text, "System.ServiceModel.dll");
					return File.Exists(path);
				}
				return false;
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x000194D0 File Offset: 0x000176D0
		private string GetOldMcsName(TargetFrameworkMoniker fx)
		{
			string identifier;
			if ((identifier = fx.Identifier) != null)
			{
				string version;
				if (!(identifier == ".NETFramework"))
				{
					if (identifier == "MonoAndroid" || identifier == "MonoTouch" || identifier == "Silverlight")
					{
						return "smcs";
					}
				}
				else if ((version = fx.Version) != null)
				{
					if (version == "1.1")
					{
						return "mcs";
					}
					if (version == "2.0" || version == "3.0" || version == "3.5")
					{
						return "gmcs";
					}
					if (version == "4.0")
					{
						return "dmcs";
					}
				}
			}
			return "mcs";
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001958C File Offset: 0x0001778C
		public override string GetToolPath(string toolName)
		{
			if (toolName == "csc" || toolName == "mcs")
			{
				if (this.targetRuntime.HasMultitargetingMcs)
				{
					toolName = "mcs";
				}
				else
				{
					toolName = this.GetOldMcsName(this.framework.Id);
				}
			}
			else if (toolName == "vbc")
			{
				toolName = "vbnc";
			}
			else if (toolName == "resgen")
			{
				if (this.framework.ClrVersion == ClrVersion.Net_1_1)
				{
					toolName = "resgen1";
				}
				else if (this.framework.ClrVersion == ClrVersion.Net_2_0)
				{
					toolName = "resgen2";
				}
			}
			else if (toolName == "msbuild")
			{
				toolName = "xbuild";
			}
			return base.GetToolPath(toolName);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001964C File Offset: 0x0001784C
		public override SystemPackageInfo GetFrameworkPackageInfo(string packageName)
		{
			SystemPackageInfo frameworkPackageInfo = base.GetFrameworkPackageInfo(packageName);
			if (this.framework.Id.Version == "3.0" && packageName == "olive")
			{
				frameworkPackageInfo.IsCorePackage = false;
			}
			if (string.IsNullOrEmpty(frameworkPackageInfo.Name))
			{
				frameworkPackageInfo.Name = "mono";
			}
			return frameworkPackageInfo;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001978C File Offset: 0x0001798C
		public override IEnumerable<string> GetToolsPaths()
		{
			yield return Path.Combine(this.targetRuntime.MonoRuntimeInfo.Prefix, "bin");
			yield break;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000197A9 File Offset: 0x000179A9
		public override Dictionary<string, string> GetToolsEnvironmentVariables()
		{
			return this.targetRuntime.EnvironmentVariables;
		}

		// Token: 0x0400022B RID: 555
		private string ref_assemblies_folder;
	}
}
