using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;

namespace Mono.PkgConfig
{
	// Token: 0x020000B7 RID: 183
	internal abstract class PcFileCache<TP> where TP : PackageInfo, new()
	{
		// Token: 0x06000631 RID: 1585 RVA: 0x000172BC File Offset: 0x000154BC
		public PcFileCache(IPcFileCacheContext<TP> ctx)
		{
			this.ctx = ctx;
			try
			{
				string cacheDirectory = this.CacheDirectory;
				if (!Directory.Exists(cacheDirectory))
				{
					Directory.CreateDirectory(cacheDirectory);
				}
				this.cacheFile = Path.Combine(cacheDirectory, "pkgconfig-cache-2.xml");
				if (File.Exists(this.cacheFile))
				{
					this.Load();
				}
			}
			catch (Exception ex)
			{
				ctx.ReportError("pc file cache could not be loaded.", ex);
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000632 RID: 1586
		protected abstract string CacheDirectory { get; }

		// Token: 0x06000633 RID: 1587 RVA: 0x00017348 File Offset: 0x00015548
		public void Update()
		{
			this.Update(this.GetDefaultPaths());
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00017358 File Offset: 0x00015558
		public void Update(IEnumerable<string> pkgConfigDirs)
		{
			foreach (string path in pkgConfigDirs)
			{
				foreach (string file in Directory.GetFiles(path, "*.pc"))
				{
					this.GetPackageInfo(file);
				}
			}
			this.Save();
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000173CC File Offset: 0x000155CC
		public IEnumerable<TP> GetPackages()
		{
			return this.GetPackages(null);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00017634 File Offset: 0x00015834
		public IEnumerable<TP> GetPackages(IEnumerable<string> pkgConfigDirs)
		{
			if (pkgConfigDirs == null)
			{
				pkgConfigDirs = this.GetDefaultPaths();
			}
			foreach (string sp in pkgConfigDirs)
			{
				List<TP> list;
				if (this.filesByFolder.TryGetValue(Path.GetFullPath(sp), out list))
				{
					foreach (TP p in list)
					{
						yield return p;
					}
				}
			}
			yield break;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00017658 File Offset: 0x00015858
		public TP GetPackageInfoByName(string name)
		{
			return this.GetPackageInfoByName(name, null);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00017664 File Offset: 0x00015864
		public TP GetPackageInfoByName(string name, IEnumerable<string> pkgConfigDirs)
		{
			foreach (TP result in this.GetPackages(pkgConfigDirs))
			{
				if (result.Name == name)
				{
					return result;
				}
			}
			return default(TP);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x000176D0 File Offset: 0x000158D0
		public TP GetPackageInfo(string file)
		{
			TP replacedInfo = default(TP);
			file = Path.GetFullPath(file);
			DateTime lastWriteTime = File.GetLastWriteTime(file);
			TP tp;
			lock (this.infos)
			{
				if (this.infos.TryGetValue(file, out tp))
				{
					if (tp.LastWriteTime == lastWriteTime)
					{
						return tp;
					}
					replacedInfo = tp;
				}
			}
			try
			{
				tp = this.ParsePackageInfo(file);
			}
			catch (Exception ex)
			{
				this.ctx.ReportError("Error while parsing .pc file: " + file, ex);
				tp = Activator.CreateInstance<TP>();
			}
			lock (this.infos)
			{
				if (!tp.IsValidPackage)
				{
					tp = Activator.CreateInstance<TP>();
				}
				tp.LastWriteTime = lastWriteTime;
				this.Add(file, tp, replacedInfo);
				this.hasChanges = true;
			}
			return tp;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x000177F0 File Offset: 0x000159F0
		private void Add(string file, TP info, TP replacedInfo)
		{
			this.infos[file] = info;
			string fullPath = Path.GetFullPath(Path.GetDirectoryName(file));
			List<TP> list;
			if (!this.filesByFolder.TryGetValue(fullPath, out list))
			{
				list = new List<TP>();
				this.filesByFolder[fullPath] = list;
			}
			if (replacedInfo != null)
			{
				int num = list.IndexOf(replacedInfo);
				if (num != -1)
				{
					list[num] = info;
					return;
				}
			}
			list.Add(info);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00017860 File Offset: 0x00015A60
		private FileStream OpenFile(FileAccess access)
		{
			int i = 6;
			FileMode mode = (access == FileAccess.Read) ? FileMode.Open : FileMode.Create;
			Exception ex = null;
			while (i > 0)
			{
				try
				{
					return new FileStream(this.cacheFile, mode, access, FileShare.None);
				}
				catch (Exception ex2)
				{
					ex = ex2;
					Thread.Sleep(200);
					i--;
				}
			}
			this.ctx.ReportError("File could not be opened: " + this.cacheFile, ex);
			return null;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000178D8 File Offset: 0x00015AD8
		private void Load()
		{
			using (FileStream fileStream = this.OpenFile(FileAccess.Read))
			{
				if (fileStream != null)
				{
					XmlTextReader xmlTextReader = new XmlTextReader(fileStream);
					xmlTextReader.MoveToContent();
					xmlTextReader.ReadStartElement();
					xmlTextReader.MoveToContent();
					while (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						this.ReadPackage(xmlTextReader);
					}
				}
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001793C File Offset: 0x00015B3C
		public void Save()
		{
			lock (this.infos)
			{
				if (this.hasChanges)
				{
					using (FileStream fileStream = this.OpenFile(FileAccess.Write))
					{
						if (fileStream != null)
						{
							XmlTextWriter xmlTextWriter = new XmlTextWriter(new StreamWriter(fileStream));
							xmlTextWriter.Formatting = Formatting.Indented;
							xmlTextWriter.WriteStartElement("PcFileCache");
							foreach (KeyValuePair<string, TP> keyValuePair in this.infos)
							{
								this.WritePackage(xmlTextWriter, keyValuePair.Key, keyValuePair.Value);
							}
							xmlTextWriter.WriteEndElement();
							xmlTextWriter.Flush();
							this.hasChanges = false;
						}
					}
				}
			}
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00017A30 File Offset: 0x00015C30
		private void WritePackage(XmlTextWriter tw, string file, TP pinfo)
		{
			tw.WriteStartElement("File");
			tw.WriteAttributeString("path", file);
			tw.WriteAttributeString("lastWriteTime", XmlConvert.ToString(pinfo.LastWriteTime, XmlDateTimeSerializationMode.Local));
			if (pinfo.IsValidPackage)
			{
				if (pinfo.Name != null)
				{
					tw.WriteAttributeString("name", pinfo.Name);
				}
				if (pinfo.Version != null)
				{
					tw.WriteAttributeString("version", pinfo.Version);
				}
				if (!string.IsNullOrEmpty(pinfo.Description))
				{
					tw.WriteAttributeString("description", pinfo.Description);
				}
				if (!string.IsNullOrEmpty(pinfo.Requires))
				{
					tw.WriteAttributeString("requires", pinfo.Requires);
				}
				if (pinfo.CustomData != null)
				{
					foreach (KeyValuePair<string, string> keyValuePair in pinfo.CustomData)
					{
						tw.WriteAttributeString(keyValuePair.Key, keyValuePair.Value);
					}
				}
				this.WritePackageContent(tw, file, pinfo);
			}
			tw.WriteEndElement();
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00017BA4 File Offset: 0x00015DA4
		protected virtual void WritePackageContent(XmlTextWriter tw, string file, TP pinfo)
		{
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00017BA8 File Offset: 0x00015DA8
		private void ReadPackage(XmlReader tr)
		{
			TP tp = Activator.CreateInstance<TP>();
			string text = null;
			tr.MoveToFirstAttribute();
			for (;;)
			{
				string localName;
				if ((localName = tr.LocalName) == null)
				{
					goto IL_129;
				}
				if (<PrivateImplementationDetails>{F94756FF-069E-4CB1-B508-F981905EDA75}.$$method0x6000614-1 == null)
				{
					<PrivateImplementationDetails>{F94756FF-069E-4CB1-B508-F981905EDA75}.$$method0x6000614-1 = new Dictionary<string, int>(6)
					{
						{
							"path",
							0
						},
						{
							"lastWriteTime",
							1
						},
						{
							"name",
							2
						},
						{
							"version",
							3
						},
						{
							"description",
							4
						},
						{
							"requires",
							5
						}
					};
				}
				int num;
				if (!<PrivateImplementationDetails>{F94756FF-069E-4CB1-B508-F981905EDA75}.$$method0x6000614-1.TryGetValue(localName, out num))
				{
					goto IL_129;
				}
				switch (num)
				{
				case 0:
					text = tr.Value;
					break;
				case 1:
					tp.LastWriteTime = XmlConvert.ToDateTime(tr.Value, XmlDateTimeSerializationMode.Local);
					break;
				case 2:
					tp.Name = tr.Value;
					break;
				case 3:
					tp.Version = tr.Value;
					break;
				case 4:
					tp.Description = tr.Value;
					break;
				case 5:
					tp.Requires = tr.Value;
					break;
				default:
					goto IL_129;
				}
				IL_142:
				if (!tr.MoveToNextAttribute())
				{
					break;
				}
				continue;
				IL_129:
				tp.SetData(tr.LocalName, tr.Value);
				goto IL_142;
			}
			tr.MoveToElement();
			if (!tr.IsEmptyElement)
			{
				tr.ReadStartElement();
				tr.MoveToContent();
				this.ReadPackageContent(tr, tp);
				tr.MoveToContent();
				tr.ReadEndElement();
			}
			else
			{
				tr.Read();
			}
			tr.MoveToContent();
			if (!tp.IsValidPackage || this.ctx.IsCustomDataComplete(text, tp))
			{
				this.Add(text, tp, default(TP));
			}
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00017D73 File Offset: 0x00015F73
		protected virtual void ReadPackageContent(XmlReader tr, TP pinfo)
		{
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x00017D75 File Offset: 0x00015F75
		public object SyncRoot
		{
			get
			{
				return this.infos;
			}
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00017D80 File Offset: 0x00015F80
		private TP ParsePackageInfo(string pcfile)
		{
			PcFile pcFile = new PcFile();
			pcFile.Load(pcfile);
			TP tp = Activator.CreateInstance<TP>();
			tp.Name = Path.GetFileNameWithoutExtension(pcFile.FilePath);
			if (!pcFile.HasErrors)
			{
				tp.Version = pcFile.Version;
				tp.Description = pcFile.Description;
				tp.Requires = pcFile.Requires;
				this.ParsePackageInfo(pcFile, tp);
				if (tp.IsValidPackage)
				{
					this.ctx.StoreCustomData(pcFile, tp);
				}
			}
			return tp;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00017E1E File Offset: 0x0001601E
		protected virtual void ParsePackageInfo(PcFile file, TP pinfo)
		{
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00017E20 File Offset: 0x00016020
		private IEnumerable<string> GetDefaultPaths()
		{
			if (this.defaultPaths == null)
			{
				string environmentVariable = Environment.GetEnvironmentVariable("PKG_CONFIG_PATH");
				string environmentVariable2 = Environment.GetEnvironmentVariable("PKG_CONFIG_LIBDIR");
				this.defaultPaths = this.GetPkgconfigPaths(null, environmentVariable, environmentVariable2);
			}
			return this.defaultPaths;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00017E60 File Offset: 0x00016060
		public IEnumerable<string> GetPkgconfigPaths(string prefix, string pkgConfigPath, string pkgConfigLibdir)
		{
			char[] separator = new char[]
			{
				Path.PathSeparator
			};
			string[] array = null;
			if (!string.IsNullOrEmpty(pkgConfigPath))
			{
				array = pkgConfigPath.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 0)
				{
					array = null;
				}
			}
			string[] array2 = null;
			if (!string.IsNullOrEmpty(pkgConfigLibdir))
			{
				array2 = pkgConfigLibdir.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				if (array2.Length == 0)
				{
					array2 = null;
				}
			}
			if (prefix == null)
			{
				prefix = PcFileCache<TP>.PathUp(typeof(int).Assembly.Location, 4);
			}
			IEnumerable<string> unfilteredPkgConfigDirs = this.GetUnfilteredPkgConfigDirs(array, array2, new string[]
			{
				prefix
			});
			return this.NormaliseAndFilterPaths(unfilteredPkgConfigDirs, Environment.CurrentDirectory);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00018318 File Offset: 0x00016518
		private IEnumerable<string> GetUnfilteredPkgConfigDirs(IEnumerable<string> pkgConfigPaths, IEnumerable<string> pkgConfigLibdirs, IEnumerable<string> systemPrefixes)
		{
			if (pkgConfigPaths != null)
			{
				foreach (string dir in pkgConfigPaths)
				{
					yield return dir;
				}
			}
			if (pkgConfigLibdirs != null)
			{
				foreach (string dir2 in pkgConfigLibdirs)
				{
					yield return dir2;
				}
			}
			else if (systemPrefixes != null)
			{
				string[] suffixes = new string[]
				{
					Path.Combine("share", "pkgconfig"),
					Path.Combine("lib", "pkgconfig"),
					Path.Combine("lib64", "pkgconfig"),
					Path.Combine("libdata", "pkgconfig")
				};
				foreach (string prefix in systemPrefixes)
				{
					foreach (string suffix in suffixes)
					{
						yield return Path.Combine(prefix, suffix);
					}
				}
			}
			yield break;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x000185C4 File Offset: 0x000167C4
		private IEnumerable<string> NormaliseAndFilterPaths(IEnumerable<string> paths, string workingDirectory)
		{
			Dictionary<string, string> filtered = new Dictionary<string, string>();
			foreach (string p in paths)
			{
				string path = p;
				if (!Path.IsPathRooted(path))
				{
					path = Path.Combine(workingDirectory, path);
				}
				path = Path.GetFullPath(path);
				if (!filtered.ContainsKey(path))
				{
					filtered.Add(path, path);
					try
					{
						if (!Directory.Exists(path))
						{
							continue;
						}
					}
					catch (IOException ex)
					{
						this.ctx.ReportError("Error checking for directory '" + path + "'.", ex);
					}
					yield return path;
				}
			}
			yield break;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000185F0 File Offset: 0x000167F0
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

		// Token: 0x04000211 RID: 529
		private const string CACHE_VERSION = "2";

		// Token: 0x04000212 RID: 530
		private Dictionary<string, TP> infos = new Dictionary<string, TP>();

		// Token: 0x04000213 RID: 531
		private Dictionary<string, List<TP>> filesByFolder = new Dictionary<string, List<TP>>();

		// Token: 0x04000214 RID: 532
		private string cacheFile;

		// Token: 0x04000215 RID: 533
		private bool hasChanges;

		// Token: 0x04000216 RID: 534
		private IPcFileCacheContext<TP> ctx;

		// Token: 0x04000217 RID: 535
		private IEnumerable<string> defaultPaths;
	}
}
