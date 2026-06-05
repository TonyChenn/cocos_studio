using System;
using Mono.Addins;
using MonoDevelop.Core.Assemblies;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x02000043 RID: 67
	[ExtensionNodeChild(typeof(AssemblyExtensionNode))]
	[ExtensionNode("Package")]
	internal class PackageExtensionNode : TypeExtensionNode
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00008DE0 File Offset: 0x00006FE0
		public string[] Assemblies
		{
			get
			{
				lock (this)
				{
					if (this.assemblies == null)
					{
						this.assemblies = new string[base.ChildNodes.Count];
						for (int i = 0; i < base.ChildNodes.Count; i++)
						{
							string text = ((AssemblyExtensionNode)base.ChildNodes[i]).FileName;
							text = base.Addin.GetFilePath(text);
							this.assemblies[i] = text;
						}
					}
				}
				return this.assemblies;
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00008E7C File Offset: 0x0000707C
		public SystemPackageInfo GetPackageInfo()
		{
			SystemPackageInfo systemPackageInfo = new SystemPackageInfo();
			systemPackageInfo.Name = this.name;
			systemPackageInfo.Version = this.version;
			if (this.fxVersion != null)
			{
				systemPackageInfo.TargetFramework = TargetFrameworkMoniker.Parse(this.fxVersion);
			}
			else if (this.clrVersion == ClrVersion.Net_1_1)
			{
				systemPackageInfo.TargetFramework = TargetFrameworkMoniker.NET_1_1;
			}
			else if (this.clrVersion == ClrVersion.Net_2_0)
			{
				systemPackageInfo.TargetFramework = TargetFrameworkMoniker.NET_2_0;
			}
			if (this.hasGacRoot)
			{
				systemPackageInfo.GacRoot = base.Addin.GetFilePath(".");
			}
			return systemPackageInfo;
		}

		// Token: 0x040000C4 RID: 196
		[NodeAttribute("name", Required = true)]
		private string name;

		// Token: 0x040000C5 RID: 197
		[NodeAttribute("version", Required = true)]
		private string version;

		// Token: 0x040000C6 RID: 198
		[NodeAttribute("targetFramework")]
		private string fxVersion;

		// Token: 0x040000C7 RID: 199
		[NodeAttribute("clrVersion")]
		private ClrVersion clrVersion;

		// Token: 0x040000C8 RID: 200
		[NodeAttribute("gacRoot")]
		private bool hasGacRoot;

		// Token: 0x040000C9 RID: 201
		private string[] assemblies;
	}
}
