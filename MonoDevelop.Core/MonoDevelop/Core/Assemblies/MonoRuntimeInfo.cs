using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000A9 RID: 169
	public class MonoRuntimeInfo
	{
		// Token: 0x060005D9 RID: 1497 RVA: 0x00015E0F File Offset: 0x0001400F
		internal MonoRuntimeInfo()
		{
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00015E2D File Offset: 0x0001402D
		public MonoRuntimeInfo(string prefix)
		{
			this.prefix = prefix;
			this.Initialize();
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x00015E58 File Offset: 0x00014058
		public string Prefix
		{
			get
			{
				return this.prefix;
			}
		}

		/// <summary>
		/// This string is strictly for displaying to the user or logging. It should never be used for version checks.
		/// </summary>
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x00015E60 File Offset: 0x00014060
		public string MonoVersion
		{
			get
			{
				this.Initialize();
				return this.monoVersion;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x00015E70 File Offset: 0x00014070
		public string DisplayName
		{
			get
			{
				return string.Concat(new string[]
				{
					"Mono ",
					this.MonoVersion,
					" (",
					this.prefix,
					")"
				});
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x00015EB4 File Offset: 0x000140B4
		public bool IsValidRuntime
		{
			get
			{
				this.Initialize();
				return this.isValidRuntime;
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00015EC2 File Offset: 0x000140C2
		private void Initialize()
		{
			if (!this.initialized)
			{
				this.initialized = true;
				this.isValidRuntime = this.InternalInitialize();
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00015EE0 File Offset: 0x000140E0
		private bool InternalInitialize()
		{
			string text = Path.Combine(this.prefix, "lib");
			if (!Directory.Exists(Path.Combine(text, "mono")))
			{
				return false;
			}
			string text2 = Path.Combine(this.prefix, "bin");
			this.envVars["PATH"] = string.Concat(new object[]
			{
				text,
				Path.PathSeparator,
				text2,
				Path.PathSeparator,
				Environment.GetEnvironmentVariable("PATH")
			});
			this.envVars["LD_LIBRARY_PATH"] = text + Path.PathSeparator + Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
			this.envVars["MONO_PATH"] = string.Empty;
			StringWriter stringWriter = new StringWriter();
			try
			{
				string text3 = Path.Combine(this.prefix, "bin");
				text3 = Path.Combine(text3, "mono");
				ProcessStartInfo processStartInfo = new ProcessStartInfo(text3, "--version");
				processStartInfo.UseShellExecute = false;
				processStartInfo.RedirectStandardOutput = true;
				foreach (KeyValuePair<string, string> keyValuePair in this.envVars)
				{
					processStartInfo.EnvironmentVariables[keyValuePair.Key] = keyValuePair.Value;
				}
				ProcessWrapper processWrapper = Runtime.ProcessService.StartProcess(processStartInfo, stringWriter, null, null);
				processWrapper.WaitForOutput();
			}
			catch
			{
				return false;
			}
			this.SetupPkgconfigPaths(null, null);
			string text4 = stringWriter.ToString();
			int num = text4.IndexOf("version", StringComparison.Ordinal);
			if (num == -1)
			{
				return false;
			}
			num += 8;
			int num2 = text4.IndexOf(' ', num);
			if (num2 == -1)
			{
				return false;
			}
			this.monoVersion = text4.Substring(num, num2 - num);
			num = text4.IndexOf('(');
			if (num != -1)
			{
				num++;
				num2 = text4.IndexOf(' ', num);
				if (num2 == -1)
				{
					num2 = text4.IndexOf(')', num);
				}
				if (num2 != -1)
				{
					string text5 = text4.Substring(num, num2 - num);
					num = text5.IndexOf('/');
					if (num != -1 && num + 1 < text5.Length)
					{
						text5 = text5.Substring(num + 1);
					}
					if (text5 != "tarball")
					{
						this.monoVersion = this.monoVersion + " (" + text5 + ")";
					}
				}
			}
			return true;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001617C File Offset: 0x0001437C
		internal Dictionary<string, string> GetEnvironmentVariables()
		{
			this.Initialize();
			return this.envVars;
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0001618A File Offset: 0x0001438A
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x00016192 File Offset: 0x00014392
		internal bool IsRunning { get; private set; }

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001619C File Offset: 0x0001439C
		public static MonoRuntimeInfo FromCurrentRuntime()
		{
			Type type = Type.GetType("Mono.Runtime");
			if (type == null)
			{
				return null;
			}
			MonoRuntimeInfo monoRuntimeInfo = new MonoRuntimeInfo();
			string text = (string)type.InvokeMember("GetDisplayName", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, null);
			int num = text.IndexOf("/branches/mono-");
			if (num != -1)
			{
				num += 15;
				int num2 = text.IndexOf('/', num);
				if (num2 != -1)
				{
					monoRuntimeInfo.monoVersion = text.Substring(num, num2 - num).Replace('-', '.');
				}
			}
			else if (text.StartsWith("/trunk/mono "))
			{
				monoRuntimeInfo.monoVersion = "Trunk";
			}
			if (monoRuntimeInfo.monoVersion == "Unknown")
			{
				num = text.IndexOf(' ');
				if (text.Length > num && text[num + 1] == '(')
				{
					monoRuntimeInfo.monoVersion = text.Substring(0, num);
				}
				else
				{
					monoRuntimeInfo.monoVersion = text.Substring(num + 1);
				}
			}
			monoRuntimeInfo.prefix = MonoRuntimeInfo.PathUp(typeof(int).Assembly.Location, 4);
			if (monoRuntimeInfo.prefix == null)
			{
				throw new SystemException("Could not detect Mono prefix");
			}
			monoRuntimeInfo.SetupPkgconfigPaths(Environment.GetEnvironmentVariable("PKG_CONFIG_PATH"), Environment.GetEnvironmentVariable("PKG_CONFIG_LIBDIR"));
			foreach (string text2 in new string[]
			{
				"PATH",
				"MONO_GAC_PREFIX",
				"XBUILD_FRAMEWORK_FOLDERS_PATH"
			})
			{
				monoRuntimeInfo.envVars[text2] = Environment.GetEnvironmentVariable(text2);
			}
			monoRuntimeInfo.IsRunning = true;
			monoRuntimeInfo.initialized = true;
			monoRuntimeInfo.isValidRuntime = true;
			return monoRuntimeInfo;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00016340 File Offset: 0x00014540
		private void SetupPkgconfigPaths(string pkgConfigPath, string pkgConfigLibdir)
		{
			IEnumerable<string> pkgconfigPaths = MonoTargetRuntime.PcFileCache.GetPkgconfigPaths(this.prefix, pkgConfigPath, pkgConfigLibdir);
			this.envVars["PKG_CONFIG_PATH"] = string.Join(Path.PathSeparator.ToString(), pkgconfigPaths.ToArray<string>());
			this.envVars["PKG_CONFIG_LIBDIR"] = "";
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x000163A0 File Offset: 0x000145A0
		private static string PathUp(string path, int up)
		{
			if (up == 0)
			{
				return path;
			}
			for (int i = path.Length - 1; i >= 0; i--)
			{
				if (path[i] == Path.DirectorySeparatorChar)
				{
					up--;
					if (up == 0)
					{
						return path.Substring(0, i);
					}
				}
			}
			return null;
		}

		// Token: 0x040001F9 RID: 505
		[ItemProperty]
		private string prefix;

		// Token: 0x040001FA RID: 506
		private string monoVersion = "Unknown";

		// Token: 0x040001FB RID: 507
		private Dictionary<string, string> envVars = new Dictionary<string, string>();

		// Token: 0x040001FC RID: 508
		private bool initialized;

		// Token: 0x040001FD RID: 509
		private bool isValidRuntime;
	}
}
