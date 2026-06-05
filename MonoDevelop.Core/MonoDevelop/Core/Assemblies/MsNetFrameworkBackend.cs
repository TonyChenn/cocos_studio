using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000C0 RID: 192
	public class MsNetFrameworkBackend : TargetFrameworkBackend<MsNetTargetRuntime>
	{
		// Token: 0x0600068A RID: 1674 RVA: 0x000197C0 File Offset: 0x000179C0
		private string GetReferenceAssembliesFolder()
		{
			string assemblyDirectoryName = this.framework.Id.GetAssemblyDirectoryName();
			foreach (FilePath filePath in this.runtime.GetReferenceFrameworkDirectories())
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
					return filePath2;
				}
			}
			return null;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00019B78 File Offset: 0x00017D78
		public override IEnumerable<string> GetFrameworkFolders()
		{
			string dir = this.GetReferenceAssembliesFolder();
			if (dir != null)
			{
				yield return dir;
			}
			string version;
			if (!(this.framework.Id.Identifier != ".NETFramework") && (version = this.framework.Id.Version) != null)
			{
				if (!(version == "1.1") && !(version == "2.0"))
				{
					if (!(version == "4.0") && !(version == "4.5"))
					{
						if (version == "3.0" || version == "3.5")
						{
							RegistryKey fxFolderKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\AssemblyFolders\\v" + this.framework.Id.Version, false);
							if (fxFolderKey != null)
							{
								string folder = fxFolderKey.GetValue("All Assemblies In") as string;
								fxFolderKey.Close();
								yield return folder;
							}
						}
					}
					else
					{
						FilePath fx40dir = this.targetRuntime.RootDirectory.Combine(new string[]
						{
							MsNetFrameworkBackend.GetClrVersion(this.framework.ClrVersion)
						});
						yield return fx40dir;
						yield return fx40dir.Combine(new string[]
						{
							"WPF"
						});
					}
				}
				else
				{
					yield return this.targetRuntime.RootDirectory.Combine(new string[]
					{
						MsNetFrameworkBackend.GetClrVersion(this.framework.ClrVersion)
					});
				}
			}
			yield break;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00019B98 File Offset: 0x00017D98
		public override Dictionary<string, string> GetToolsEnvironmentVariables()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in this.GetFrameworkToolsPaths())
			{
				stringBuilder.Append(value);
				stringBuilder.Append(Path.PathSeparator);
			}
			stringBuilder.Append(Environment.GetEnvironmentVariable("PATH"));
			dictionary["PATH"] = stringBuilder.ToString();
			return dictionary;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00019E98 File Offset: 0x00018098
		public override IEnumerable<string> GetToolsPaths()
		{
			foreach (string s in this.GetFrameworkToolsPaths())
			{
				yield return s;
			}
			foreach (string s2 in this.BaseGetToolsPaths())
			{
				yield return s2;
			}
			yield return PropertyService.EntryAssemblyPath;
			yield break;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00019EB5 File Offset: 0x000180B5
		private static string GetProgramFilesX86()
		{
			return Environment.GetFolderPath((IntPtr.Size == 8) ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001A1C8 File Offset: 0x000183C8
		private IEnumerable<string> GetFrameworkToolsPaths()
		{
			TargetFrameworkToolsVersion toolsVersion = this.framework.GetToolsVersion();
			string programFilesX86 = MsNetFrameworkBackend.GetProgramFilesX86();
			string sdkPath = Path.Combine(programFilesX86, "Microsoft SDKs", "Windows");
			switch (toolsVersion)
			{
			case TargetFrameworkToolsVersion.V1_1:
				yield return Path.Combine(sdkPath, "v6.0A\\bin");
				yield return this.targetRuntime.RootDirectory.Combine(new string[]
				{
					MsNetFrameworkBackend.GetClrVersion(ClrVersion.Net_1_1)
				});
				goto IL_26C;
			case TargetFrameworkToolsVersion.V2_0:
				break;
			case TargetFrameworkToolsVersion.V3_5:
				yield return Path.Combine(sdkPath, "v7.0A\\bin");
				yield return this.targetRuntime.RootDirectory.Combine(new string[]
				{
					"v3.5"
				});
				break;
			case TargetFrameworkToolsVersion.V4_0:
				yield return Path.Combine(sdkPath, "v7.0A\\bin\\NETFX 4.0 Tools");
				yield return this.targetRuntime.RootDirectory.Combine(new string[]
				{
					MsNetFrameworkBackend.GetClrVersion(ClrVersion.Net_4_0)
				});
				goto IL_26C;
			default:
				throw new Exception("Unknown ToolsVersion");
			}
			yield return Path.Combine(sdkPath, "v6.0A\\bin");
			yield return this.targetRuntime.RootDirectory.Combine(new string[]
			{
				MsNetFrameworkBackend.GetClrVersion(ClrVersion.Net_2_0)
			});
			IL_26C:
			yield break;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001A1E5 File Offset: 0x000183E5
		private IEnumerable<string> BaseGetToolsPaths()
		{
			return base.GetToolsPaths();
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001A1F0 File Offset: 0x000183F0
		internal static string GetClrVersion(ClrVersion v)
		{
			switch (v)
			{
			case ClrVersion.Net_1_1:
				return "v1.1.4322";
			case ClrVersion.Net_2_0:
				return "v2.0.50727";
			case ClrVersion.Net_4_0:
			case ClrVersion.Net_4_5:
				return "v4.0.30319";
			}
			return null;
		}
	}
}
