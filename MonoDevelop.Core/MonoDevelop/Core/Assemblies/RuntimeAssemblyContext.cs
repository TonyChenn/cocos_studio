using System;
using System.Collections.Generic;
using System.IO;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000D9 RID: 217
	public class RuntimeAssemblyContext : AssemblyContext
	{
		// Token: 0x060007BE RID: 1982 RVA: 0x0001F6D3 File Offset: 0x0001D8D3
		public RuntimeAssemblyContext(TargetRuntime runtime)
		{
			this.runtime = runtime;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001F6E2 File Offset: 0x0001D8E2
		protected override void Initialize()
		{
			this.runtime.EnsureInitialized();
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001F6EF File Offset: 0x0001D8EF
		public override bool AssemblyIsInGac(string aname)
		{
			return this.GetGacFile(aname, false) != null;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001F700 File Offset: 0x0001D900
		public override string GetAssemblyLocation(string assemblyName, string package, TargetFramework fx)
		{
			string assemblyLocation = base.GetAssemblyLocation(assemblyName, package, fx);
			if (assemblyLocation != null)
			{
				return assemblyLocation;
			}
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			int num = assemblyName.IndexOf(',');
			string path;
			if (num == -1)
			{
				path = assemblyName;
			}
			else
			{
				path = assemblyName.Substring(0, num).Trim();
			}
			if (!string.IsNullOrEmpty(baseDirectory))
			{
				string text = Path.Combine(baseDirectory, path);
				if (File.Exists(text))
				{
					return text;
				}
			}
			foreach (string path2 in this.GetAssemblyDirectories())
			{
				string text2 = Path.Combine(path2, path);
				if (File.Exists(text2))
				{
					return text2;
				}
			}
			return this.GetGacFile(assemblyName, true);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0001F7C8 File Offset: 0x0001D9C8
		private string GetGacFile(string aname, bool allowPartialMatch)
		{
			string text;
			string text2;
			string text3;
			string text4;
			AssemblyContext.ParseAssemblyName(aname, out text, out text2, out text3, out text4);
			if (text == null)
			{
				return null;
			}
			if (!allowPartialMatch)
			{
				if (text == null || text2 == null || text3 == null || text4 == null)
				{
					return null;
				}
				using (IEnumerator<string> enumerator = this.runtime.GetGacDirectories().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string path = enumerator.Current;
						string text5 = Path.Combine(path, text);
						text5 = Path.Combine(text5, string.Concat(new string[]
						{
							text2,
							"_",
							text3,
							"_",
							text4
						}));
						text5 = Path.Combine(text5, text + ".dll");
						if (File.Exists(text5))
						{
							return text5;
						}
					}
					goto IL_1CC;
				}
			}
			string searchPattern = string.Concat(new string[]
			{
				text2 ?? "*",
				"_",
				text3 ?? "*",
				"_",
				text4 ?? "*"
			});
			foreach (string path2 in this.runtime.GetGacDirectories())
			{
				string path3 = Path.Combine(path2, text);
				if (Directory.Exists(path3))
				{
					foreach (string path4 in Directory.GetDirectories(path3, searchPattern))
					{
						string text6 = Path.Combine(path4, text + ".dll");
						if (File.Exists(text6))
						{
							return text6;
						}
						text6 = Path.Combine(path4, text + ".exe");
						if (File.Exists(text6))
						{
							return text6;
						}
					}
				}
			}
			IL_1CC:
			return null;
		}

		// Token: 0x0400027C RID: 636
		private TargetRuntime runtime;
	}
}
