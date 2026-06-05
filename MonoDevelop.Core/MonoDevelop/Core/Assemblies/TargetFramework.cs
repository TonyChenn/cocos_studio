using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Mono.Addins;
using MonoDevelop.Core.AddIns;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x020000A4 RID: 164
	public class TargetFramework
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x00013E70 File Offset: 0x00012070
		public static TargetFramework Default
		{
			get
			{
				return Runtime.SystemAssemblyService.GetTargetFramework(TargetFrameworkMoniker.Default);
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00013E81 File Offset: 0x00012081
		internal TargetFramework()
		{
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00013EA0 File Offset: 0x000120A0
		internal TargetFramework(TargetFrameworkMoniker id)
		{
			this.id = id;
			this.name = ((id.Profile == null) ? string.Format("{0} {1}", id.Identifier, id.Version) : string.Format("{0} {1} {2} Profile", id.Identifier, id.Version, id.Profile));
			this.clrVersion = ClrVersion.Default;
			this.Assemblies = new AssemblyInfo[0];
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x00013F25 File Offset: 0x00012125
		public bool Hidden
		{
			get
			{
				return this.hidden;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00013F30 File Offset: 0x00012130
		public string Name
		{
			get
			{
				if (!string.IsNullOrEmpty(this.name))
				{
					return this.name;
				}
				if (!string.IsNullOrEmpty(this.id.Profile))
				{
					return string.Format("{0} {1} ({2})", this.id.Identifier, this.id.Version, this.id.Profile);
				}
				return string.Format("{0} {1}", this.id.Identifier, this.id.Version);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x00013FAF File Offset: 0x000121AF
		public TargetFrameworkMoniker Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00013FB7 File Offset: 0x000121B7
		public ClrVersion ClrVersion
		{
			get
			{
				if (this.clrVersion == ClrVersion.Default)
				{
					return ClrVersion.Net_4_0;
				}
				return this.clrVersion;
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00013FCC File Offset: 0x000121CC
		internal TargetFrameworkToolsVersion GetToolsVersion()
		{
			if (this.toolsVersion != TargetFrameworkToolsVersion.Unspecified)
			{
				return this.toolsVersion;
			}
			string version;
			if (this.Id.Identifier == ".NETFramework" && (version = this.id.Version) != null)
			{
				if (version == "4.0")
				{
					return TargetFrameworkToolsVersion.V4_0;
				}
				if (version == "3.5")
				{
					return TargetFrameworkToolsVersion.V3_5;
				}
				if (version == "3.0" || version == "2.0")
				{
					return TargetFrameworkToolsVersion.V2_0;
				}
				if (version == "1.1")
				{
					return TargetFrameworkToolsVersion.V1_1;
				}
			}
			switch (this.clrVersion)
			{
			case ClrVersion.Net_1_1:
				return TargetFrameworkToolsVersion.V1_1;
			case ClrVersion.Net_2_0:
				return TargetFrameworkToolsVersion.V2_0;
			case ClrVersion.Net_4_0:
				return TargetFrameworkToolsVersion.V4_0;
			}
			return TargetFrameworkToolsVersion.V4_0;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00014084 File Offset: 0x00012284
		private static bool ProfileMatchesPattern(string profile, string pattern)
		{
			if (string.IsNullOrEmpty(pattern))
			{
				return string.IsNullOrEmpty(profile);
			}
			int num = pattern.IndexOf('*');
			if (num == -1)
			{
				return profile == pattern;
			}
			if (num == 0)
			{
				return true;
			}
			if (string.IsNullOrEmpty(profile))
			{
				return false;
			}
			string value = pattern.Substring(0, num);
			return profile.StartsWith(value, StringComparison.Ordinal);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x000140D8 File Offset: 0x000122D8
		public bool CanReferenceAssembliesTargetingFramework(TargetFrameworkMoniker fxId)
		{
			TargetFramework targetFramework = Runtime.SystemAssemblyService.GetTargetFramework(fxId);
			return targetFramework != null && this.CanReferenceAssembliesTargetingFramework(targetFramework);
		}

		/// <summary>
		/// Determines whether projects targeting this framework can reference assemblies targeting the framework specified by fx.
		/// </summary>
		/// <returns><c>true</c> if projects targeting this framework can reference assemblies targeting the framework specified by fx; otherwise, <c>false</c>.</returns>
		/// <param name="fx">The target framework</param>
		// Token: 0x06000599 RID: 1433 RVA: 0x00014100 File Offset: 0x00012300
		public bool CanReferenceAssembliesTargetingFramework(TargetFramework fx)
		{
			foreach (SupportedFramework supportedFramework in fx.SupportedFrameworks)
			{
				if (!(supportedFramework.Identifier != this.id.Identifier) && TargetFramework.ProfileMatchesPattern(this.id.Profile, supportedFramework.Profile))
				{
					Version v = new Version(this.id.Version);
					if (v >= supportedFramework.MinimumVersion && v <= supportedFramework.MaximumVersion)
					{
						return true;
					}
				}
			}
			return fx.Id.Identifier == this.id.Identifier && new Version(fx.Id.Version).CompareTo(new Version(this.id.Version)) <= 0;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x000141FC File Offset: 0x000123FC
		internal string GetCorlibVersion()
		{
			if (this.corlibVersion != null)
			{
				return this.corlibVersion;
			}
			foreach (AssemblyInfo assemblyInfo in this.Assemblies)
			{
				if (assemblyInfo.Name == "mscorlib")
				{
					return this.corlibVersion = assemblyInfo.Version;
				}
			}
			return this.corlibVersion = string.Empty;
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00014269 File Offset: 0x00012469
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x00014271 File Offset: 0x00012471
		internal TargetFrameworkNode FrameworkNode { get; set; }

		// Token: 0x0600059D RID: 1437 RVA: 0x0001427C File Offset: 0x0001247C
		internal TargetFrameworkBackend CreateBackendForRuntime(TargetRuntime runtime)
		{
			if (this.FrameworkNode == null)
			{
				return null;
			}
			lock (this.FrameworkNode)
			{
				if (this.FrameworkNode.ChildNodes == null)
				{
					return null;
				}
			}
			foreach (object obj in this.FrameworkNode.ChildNodes)
			{
				TypeExtensionNode typeExtensionNode = (TypeExtensionNode)obj;
				TargetFrameworkBackend targetFrameworkBackend = (TargetFrameworkBackend)typeExtensionNode.CreateInstance(typeof(TargetFrameworkBackend));
				if (targetFrameworkBackend.SupportsRuntime(runtime))
				{
					return targetFrameworkBackend;
				}
			}
			return null;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00014348 File Offset: 0x00012548
		public bool IncludesFramework(TargetFrameworkMoniker id)
		{
			return id == this.id || this.includedFrameworks.Contains(id);
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00014366 File Offset: 0x00012566
		internal List<TargetFrameworkMoniker> IncludedFrameworks
		{
			get
			{
				return this.includedFrameworks;
			}
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00014370 File Offset: 0x00012570
		internal TargetFrameworkMoniker GetIncludesFramework()
		{
			if (string.IsNullOrEmpty(this.includesFramework))
			{
				return null;
			}
			string text = (this.includesFramework[0] == 'v') ? this.includesFramework.Substring(1) : this.includesFramework;
			if (text.Length == 0)
			{
				throw new InvalidOperationException("Invalid include version in framework " + this.id);
			}
			return new TargetFrameworkMoniker(this.id.Identifier, text);
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x000143E0 File Offset: 0x000125E0
		public List<SupportedFramework> SupportedFrameworks
		{
			get
			{
				return this.supportedFrameworks;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x000143E8 File Offset: 0x000125E8
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x000143F0 File Offset: 0x000125F0
		[ItemProperty("Assembly", Scope = "*")]
		[ItemProperty]
		internal AssemblyInfo[] Assemblies { get; set; }

		// Token: 0x060005A4 RID: 1444 RVA: 0x000143FC File Offset: 0x000125FC
		public override string ToString()
		{
			return string.Format("[TargetFramework: Hidden={0}, Name={1}, Id={2}, ClrVersion={3}]", new object[]
			{
				this.Hidden,
				this.Name,
				this.Id,
				this.ClrVersion
			});
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001444C File Offset: 0x0001264C
		public static TargetFramework FromFrameworkDirectory(TargetFrameworkMoniker moniker, FilePath dir)
		{
			FilePath filePath = dir.Combine(new string[]
			{
				"RedistList",
				"FrameworkList.xml"
			});
			if (!File.Exists(filePath))
			{
				return null;
			}
			TargetFramework targetFramework = new TargetFramework(moniker);
			using (XmlReader xmlReader = XmlReader.Create(filePath))
			{
				if (!xmlReader.ReadToDescendant("FileList"))
				{
					throw new Exception("Missing FileList element");
				}
				if (xmlReader.MoveToAttribute("Name") && xmlReader.ReadAttributeValue())
				{
					targetFramework.name = xmlReader.ReadContentAsString();
				}
				if (xmlReader.MoveToAttribute("RuntimeVersion") && xmlReader.ReadAttributeValue())
				{
					string text = xmlReader.ReadContentAsString();
					string a;
					if ((a = text) != null)
					{
						if (a == "2.0")
						{
							targetFramework.clrVersion = ClrVersion.Net_2_0;
							goto IL_122;
						}
						if (a == "4.0")
						{
							targetFramework.clrVersion = ClrVersion.Net_4_0;
							goto IL_122;
						}
						if (a == "4.5" || a == "4.5.1")
						{
							targetFramework.clrVersion = ClrVersion.Net_4_5;
							goto IL_122;
						}
					}
					LoggingService.LogInfo("Framework {0} has unknown RuntimeVersion {1}", new object[]
					{
						moniker,
						text
					});
					return null;
				}
				IL_122:
				if (xmlReader.MoveToAttribute("ToolsVersion") && xmlReader.ReadAttributeValue())
				{
					string text2 = xmlReader.ReadContentAsString();
					string a2;
					if ((a2 = text2) != null)
					{
						if (a2 == "2.0")
						{
							targetFramework.toolsVersion = TargetFrameworkToolsVersion.V2_0;
							goto IL_1D1;
						}
						if (a2 == "3.5")
						{
							targetFramework.toolsVersion = TargetFrameworkToolsVersion.V3_5;
							goto IL_1D1;
						}
						if (a2 == "4.0")
						{
							targetFramework.toolsVersion = TargetFrameworkToolsVersion.V4_0;
							goto IL_1D1;
						}
						if (a2 == "4.5")
						{
							targetFramework.toolsVersion = TargetFrameworkToolsVersion.V4_5;
							goto IL_1D1;
						}
					}
					LoggingService.LogInfo("Framework {0} has unknown ToolsVersion {1}", new object[]
					{
						moniker,
						text2
					});
					return null;
				}
				IL_1D1:
				if (xmlReader.MoveToAttribute("IncludeFramework") && xmlReader.ReadAttributeValue())
				{
					string value = xmlReader.ReadContentAsString();
					if (!string.IsNullOrEmpty(value))
					{
						targetFramework.includesFramework = value;
					}
				}
				if (xmlReader.MoveToAttribute("TargetFrameworkDirectory") && xmlReader.ReadAttributeValue())
				{
					string text3 = xmlReader.ReadContentAsString();
					if (!string.IsNullOrEmpty(text3))
					{
						text3 = text3.Replace('\\', Path.DirectorySeparatorChar);
						dir = filePath.ParentDirectory.Combine(new string[]
						{
							text3
						}).FullPath;
					}
				}
				List<AssemblyInfo> list = new List<AssemblyInfo>();
				if (xmlReader.ReadToFollowing("File"))
				{
					for (;;)
					{
						AssemblyInfo assemblyInfo = new AssemblyInfo();
						list.Add(assemblyInfo);
						if (xmlReader.MoveToAttribute("AssemblyName") && xmlReader.ReadAttributeValue())
						{
							assemblyInfo.Name = xmlReader.ReadContentAsString();
						}
						if (string.IsNullOrEmpty(assemblyInfo.Name))
						{
							break;
						}
						if (xmlReader.MoveToAttribute("Version") && xmlReader.ReadAttributeValue())
						{
							assemblyInfo.Version = xmlReader.ReadContentAsString();
						}
						if (xmlReader.MoveToAttribute("PublicKeyToken") && xmlReader.ReadAttributeValue())
						{
							assemblyInfo.PublicKeyToken = xmlReader.ReadContentAsString();
						}
						if (xmlReader.MoveToAttribute("Culture") && xmlReader.ReadAttributeValue())
						{
							assemblyInfo.Culture = xmlReader.ReadContentAsString();
						}
						if (xmlReader.MoveToAttribute("ProcessorArchitecture") && xmlReader.ReadAttributeValue())
						{
							assemblyInfo.ProcessorArchitecture = (ProcessorArchitecture)Enum.Parse(typeof(ProcessorArchitecture), xmlReader.ReadContentAsString(), true);
						}
						if (xmlReader.MoveToAttribute("InGac") && xmlReader.ReadAttributeValue())
						{
							assemblyInfo.InGac = xmlReader.ReadContentAsBoolean();
						}
						if (!xmlReader.ReadToFollowing("File"))
						{
							goto Block_41;
						}
					}
					throw new Exception("Missing AssemblyName attribute");
					Block_41:;
				}
				else
				{
					string[] files = Directory.GetFiles(dir, "*.dll");
					foreach (string text4 in files)
					{
						try
						{
							AssemblyName assemblyNameObj = SystemAssemblyService.GetAssemblyNameObj(dir.Combine(new string[]
							{
								text4
							}));
							AssemblyInfo assemblyInfo2 = new AssemblyInfo();
							assemblyInfo2.Update(assemblyNameObj);
							list.Add(assemblyInfo2);
						}
						catch (Exception ex)
						{
							LoggingService.LogError("Error reading name for assembly '{0}' in framework '{1}':\n{2}", new object[]
							{
								text4,
								targetFramework.Id,
								ex.ToString()
							});
						}
					}
				}
				targetFramework.Assemblies = list.ToArray();
			}
			FilePath filePath2 = dir.Combine(new string[]
			{
				"SupportedFrameworks"
			});
			if (Directory.Exists(filePath2))
			{
				foreach (string text5 in Directory.GetFiles(filePath2))
				{
					targetFramework.SupportedFrameworks.Add(SupportedFramework.Load(targetFramework, text5));
				}
			}
			return targetFramework;
		}

		// Token: 0x040001D1 RID: 465
		[ItemProperty(SerializationDataType = typeof(TargetFrameworkMonikerDataType))]
		private TargetFrameworkMoniker id;

		// Token: 0x040001D2 RID: 466
		[ItemProperty("_name")]
		private string name;

		// Token: 0x040001D3 RID: 467
		[ItemProperty]
		private bool hidden;

		// Token: 0x040001D4 RID: 468
		[ItemProperty]
		private ClrVersion clrVersion;

		// Token: 0x040001D5 RID: 469
		private List<TargetFrameworkMoniker> includedFrameworks = new List<TargetFrameworkMoniker>();

		// Token: 0x040001D6 RID: 470
		private List<SupportedFramework> supportedFrameworks = new List<SupportedFramework>();

		// Token: 0x040001D7 RID: 471
		internal bool RelationsBuilt;

		// Token: 0x040001D8 RID: 472
		private string corlibVersion;

		// Token: 0x040001D9 RID: 473
		private TargetFrameworkToolsVersion toolsVersion;

		// Token: 0x040001DA RID: 474
		[ItemProperty(Name = "IncludesFramework")]
		private string includesFramework;
	}
}
