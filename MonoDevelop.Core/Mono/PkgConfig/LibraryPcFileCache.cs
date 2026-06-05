using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Mono.PkgConfig
{
	// Token: 0x020000DA RID: 218
	internal class LibraryPcFileCache : PcFileCache<LibraryPackageInfo>
	{
		// Token: 0x060007C3 RID: 1987 RVA: 0x0001F9C4 File Offset: 0x0001DBC4
		public LibraryPcFileCache(IPcFileCacheContext<LibraryPackageInfo> ctx) : base(ctx)
		{
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0001F9D0 File Offset: 0x0001DBD0
		protected override string CacheDirectory
		{
			get
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				return Path.Combine(folderPath, "xbuild");
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001F9F2 File Offset: 0x0001DBF2
		public PackageAssemblyInfo GetAssemblyLocation(string fullName)
		{
			return this.GetAssemblyLocation(fullName, null);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0001F9FC File Offset: 0x0001DBFC
		public PackageAssemblyInfo GetAssemblyLocation(string fullName, IEnumerable<string> searchPaths)
		{
			lock (base.SyncRoot)
			{
				if (this.assemblyLocations == null)
				{
					this.assemblyLocations = new Dictionary<string, PackageAssemblyInfo>();
					foreach (LibraryPackageInfo libraryPackageInfo in base.GetPackages(searchPaths))
					{
						if (libraryPackageInfo.IsValidPackage)
						{
							foreach (PackageAssemblyInfo packageAssemblyInfo in libraryPackageInfo.Assemblies)
							{
								this.assemblyLocations[LibraryPcFileCache.NormalizeAsmName(packageAssemblyInfo.FullName)] = packageAssemblyInfo;
							}
						}
					}
				}
			}
			PackageAssemblyInfo result;
			this.assemblyLocations.TryGetValue(LibraryPcFileCache.NormalizeAsmName(fullName), out result);
			return result;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001FAFC File Offset: 0x0001DCFC
		public IEnumerable<PackageAssemblyInfo> ResolveAssemblyName(string name)
		{
			return this.ResolveAssemblyName(name, null);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0001FD60 File Offset: 0x0001DF60
		public IEnumerable<PackageAssemblyInfo> ResolveAssemblyName(string name, IEnumerable<string> searchPaths)
		{
			foreach (LibraryPackageInfo pinfo in base.GetPackages(searchPaths))
			{
				if (pinfo.IsValidPackage)
				{
					foreach (PackageAssemblyInfo asm in pinfo.Assemblies)
					{
						if (asm.Name == name)
						{
							yield return asm;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0001FD8C File Offset: 0x0001DF8C
		protected override void WritePackageContent(XmlTextWriter tw, string file, LibraryPackageInfo pinfo)
		{
			foreach (PackageAssemblyInfo packageAssemblyInfo in pinfo.Assemblies)
			{
				tw.WriteStartElement("Assembly");
				tw.WriteAttributeString("name", packageAssemblyInfo.Name);
				tw.WriteAttributeString("version", packageAssemblyInfo.Version);
				tw.WriteAttributeString("culture", packageAssemblyInfo.Culture);
				tw.WriteAttributeString("publicKeyToken", packageAssemblyInfo.PublicKeyToken);
				tw.WriteAttributeString("file", packageAssemblyInfo.File);
				tw.WriteEndElement();
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001FE40 File Offset: 0x0001E040
		protected override void ReadPackageContent(XmlReader tr, LibraryPackageInfo pinfo)
		{
			while (tr.NodeType == XmlNodeType.Element)
			{
				PackageAssemblyInfo packageAssemblyInfo = new PackageAssemblyInfo();
				packageAssemblyInfo.Name = tr.GetAttribute("name");
				packageAssemblyInfo.Version = tr.GetAttribute("version");
				packageAssemblyInfo.Culture = tr.GetAttribute("culture");
				packageAssemblyInfo.PublicKeyToken = tr.GetAttribute("publicKeyToken");
				packageAssemblyInfo.File = tr.GetAttribute("file");
				if (pinfo.Assemblies == null)
				{
					pinfo.Assemblies = new List<PackageAssemblyInfo>();
				}
				packageAssemblyInfo.ParentPackage = pinfo;
				pinfo.Assemblies.Add(packageAssemblyInfo);
				tr.Read();
				tr.MoveToContent();
			}
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001FEF0 File Offset: 0x0001E0F0
		protected override void ParsePackageInfo(PcFile file, LibraryPackageInfo pinfo)
		{
			List<string> list = null;
			bool flag = false;
			if (file.Libs != null && file.Libs.IndexOf(".dll") != -1)
			{
				if (file.Libs.IndexOf("-lib:") != -1 || file.Libs.IndexOf("/lib:") != -1)
				{
					list = this.GetAssembliesWithLibInfo(file.Libs);
				}
				else
				{
					list = this.GetAssembliesWithoutLibInfo(file.Libs);
				}
			}
			string text = file.GetVariable("Libraries");
			if (!string.IsNullOrEmpty(text))
			{
				list = this.GetAssembliesFromLibrariesVar(text);
			}
			text = file.GetVariable("GacPackage");
			if (text != null)
			{
				text = text.ToLower();
				pinfo.IsGacPackage = (text == "yes" || text == "true");
				flag = true;
			}
			if (list == null)
			{
				return;
			}
			string directoryName = Path.GetDirectoryName(file.FilePath);
			string text2 = Path.GetDirectoryName(Path.GetDirectoryName(directoryName));
			text2 = Path.GetFullPath(string.Concat(new object[]
			{
				text2,
				Path.DirectorySeparatorChar,
				"lib",
				Path.DirectorySeparatorChar,
				"mono",
				Path.DirectorySeparatorChar
			}));
			List<PackageAssemblyInfo> list2 = new List<PackageAssemblyInfo>();
			foreach (string text3 in list)
			{
				string text4;
				if (Path.IsPathRooted(text3))
				{
					text4 = Path.GetFullPath(text3);
				}
				else if (Path.GetDirectoryName(text3).Length == 0)
				{
					text4 = text3;
				}
				else
				{
					text4 = Path.GetFullPath(Path.Combine(directoryName, text3));
				}
				if (File.Exists(text4))
				{
					PackageAssemblyInfo packageAssemblyInfo = new PackageAssemblyInfo();
					packageAssemblyInfo.File = text4;
					packageAssemblyInfo.ParentPackage = pinfo;
					packageAssemblyInfo.UpdateFromFile(packageAssemblyInfo.File);
					list2.Add(packageAssemblyInfo);
					if (!flag && !text4.StartsWith(text2) && Path.IsPathRooted(text4))
					{
						flag = true;
						pinfo.IsGacPackage = false;
					}
				}
			}
			pinfo.Assemblies = list2;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00020108 File Offset: 0x0001E308
		private List<string> GetAssembliesWithLibInfo(string line)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			foreach (string text in line.Split(new char[]
			{
				' '
			}))
			{
				if (text.ToLower().Trim().StartsWith("/r:") || text.ToLower().Trim().StartsWith("-r:"))
				{
					list.Add(text.Substring(3).Trim());
				}
				else if (text.ToLower().Trim().StartsWith("/lib:") || text.ToLower().Trim().StartsWith("-lib:"))
				{
					list2.Add(text.Substring(5).Trim());
				}
			}
			foreach (string arg in list)
			{
				foreach (string arg2 in list2)
				{
					if (File.Exists(arg2 + Path.DirectorySeparatorChar + arg))
					{
						list3.Add(arg2 + Path.DirectorySeparatorChar + arg);
					}
				}
			}
			return list3;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00020288 File Offset: 0x0001E488
		private List<string> GetAssembliesFromLibrariesVar(string line)
		{
			List<string> list = new List<string>();
			foreach (string text in line.Split(new char[]
			{
				' '
			}))
			{
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(text);
				}
			}
			return list;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x000202D8 File Offset: 0x0001E4D8
		private List<string> GetAssembliesWithoutLibInfo(string line)
		{
			List<string> list = new List<string>();
			foreach (string text in line.Split(new char[]
			{
				' '
			}))
			{
				if (text.ToLower().Trim().StartsWith("/r:") || text.ToLower().Trim().StartsWith("-r:"))
				{
					string item = text.Substring(3).Trim();
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0002035C File Offset: 0x0001E55C
		public static string NormalizeAsmName(string name)
		{
			int num = name.ToLower().IndexOf(", publickeytoken=null");
			if (num != -1)
			{
				name = name.Substring(0, num).Trim();
			}
			num = name.ToLower().IndexOf(", processorarchitecture=");
			if (num != -1)
			{
				name = name.Substring(0, num).Trim();
			}
			return name;
		}

		// Token: 0x0400027D RID: 637
		private Dictionary<string, PackageAssemblyInfo> assemblyLocations;
	}
}
