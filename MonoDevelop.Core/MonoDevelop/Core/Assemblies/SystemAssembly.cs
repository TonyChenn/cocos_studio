using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x0200009B RID: 155
	public class SystemAssembly
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00011194 File Offset: 0x0000F394
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x0001119C File Offset: 0x0000F39C
		public string FullName { get; internal set; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x000111A5 File Offset: 0x0000F3A5
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x000111AD File Offset: 0x0000F3AD
		public string Location { get; private set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x000111B6 File Offset: 0x0000F3B6
		public AssemblyName AssemblyName
		{
			get
			{
				if (this.aname == null)
				{
					this.aname = AssemblyContext.ParseAssemblyName(this.FullName);
				}
				return this.aname;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x000111D7 File Offset: 0x0000F3D7
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x000111DF File Offset: 0x0000F3DF
		public SystemPackage Package { get; internal set; }

		// Token: 0x060004FC RID: 1276 RVA: 0x000111E8 File Offset: 0x0000F3E8
		public SystemAssembly(string file, string name)
		{
			this.FullName = name;
			this.Location = file;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x000111FE File Offset: 0x0000F3FE
		internal static SystemAssembly FromFile(string file)
		{
			return new SystemAssembly(file, SystemAssemblyService.GetAssemblyName(file));
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001120C File Offset: 0x0000F40C
		internal static SystemAssembly FromFile(string file, AssemblyInfo ainfo)
		{
			if (ainfo == null || ainfo.Version == null)
			{
				return SystemAssembly.FromFile(file);
			}
			string text = (string.IsNullOrEmpty(ainfo.PublicKeyToken) || ainfo.PublicKeyToken == "null") ? string.Empty : (", PublicKeyToken=" + ainfo.PublicKeyToken);
			string name = string.Concat(new string[]
			{
				ainfo.Name,
				", Version=",
				ainfo.Version,
				", Culture=neutral",
				text
			});
			return new SystemAssembly(file, name);
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x000112A0 File Offset: 0x0000F4A0
		public string Name
		{
			get
			{
				int num = this.FullName.IndexOf(',');
				if (num != -1)
				{
					return this.FullName.Substring(0, num).Trim();
				}
				return this.FullName;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x000112D8 File Offset: 0x0000F4D8
		public string Version
		{
			get
			{
				int num = this.FullName.IndexOf("Version=");
				if (num == -1)
				{
					return string.Empty;
				}
				num += 8;
				int num2 = this.FullName.IndexOf(',', num);
				if (num2 == -1)
				{
					num2 = this.FullName.Length;
				}
				return this.FullName.Substring(num, num2 - num);
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00011420 File Offset: 0x0000F620
		internal IEnumerable<SystemAssembly> AllSameName()
		{
			SystemAssembly asm = this;
			do
			{
				yield return asm;
				asm = asm.NextSameName;
			}
			while (asm != null);
			yield break;
		}

		// Token: 0x0400019A RID: 410
		private AssemblyName aname;

		// Token: 0x0400019B RID: 411
		internal SystemAssembly NextSameName;

		// Token: 0x0400019C RID: 412
		internal SystemAssembly NextSamePackage;
	}
}
