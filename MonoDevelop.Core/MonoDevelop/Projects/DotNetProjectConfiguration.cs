using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Formats.MSBuild;

namespace MonoDevelop.Projects
{
	// Token: 0x02000122 RID: 290
	public class DotNetProjectConfiguration : ProjectConfiguration
	{
		// Token: 0x06000AB1 RID: 2737 RVA: 0x00028834 File Offset: 0x00026A34
		public DotNetProjectConfiguration()
		{
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00028847 File Offset: 0x00026A47
		public DotNetProjectConfiguration(string name) : base(name)
		{
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0002885B File Offset: 0x00026A5B
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x00028863 File Offset: 0x00026A63
		public bool SignAssembly
		{
			get
			{
				return this.signAssembly;
			}
			set
			{
				this.signAssembly = value;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0002886C File Offset: 0x00026A6C
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x00028874 File Offset: 0x00026A74
		public bool DelaySign
		{
			get
			{
				return this.delaySign;
			}
			set
			{
				this.delaySign = value;
			}
		}

		// Token: 0x17000239 RID: 569
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x0002887D File Offset: 0x00026A7D
		[ProjectPathItemProperty("AssemblyKeyFile", ReadOnly = true)]
		[MergeToProject]
		internal string OldAssemblyKeyFile
		{
			set
			{
				this.assemblyKeyFile = value;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00028886 File Offset: 0x00026A86
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x0002888E File Offset: 0x00026A8E
		public string AssemblyKeyFile
		{
			get
			{
				return this.assemblyKeyFile;
			}
			set
			{
				this.assemblyKeyFile = value;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00028897 File Offset: 0x00026A97
		// (set) Token: 0x06000ABB RID: 2747 RVA: 0x0002889F File Offset: 0x00026A9F
		public virtual string OutputAssembly
		{
			get
			{
				return this.assembly;
			}
			set
			{
				this.assembly = value;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x000288A8 File Offset: 0x00026AA8
		public virtual CompileTarget CompileTarget
		{
			get
			{
				DotNetProject parentItem = this.ParentItem;
				if (parentItem != null)
				{
					return parentItem.CompileTarget;
				}
				return CompileTarget.Library;
			}
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00028914 File Offset: 0x00026B14
		public override SolutionItemConfiguration FindBestMatch(SolutionItemConfigurationCollection configurations)
		{
			bool isDebug = this.compilationParameters.GetDefineSymbols().Contains("DEBUG");
			DotNetProjectConfiguration[] source = (from c in configurations.OfType<DotNetProjectConfiguration>()
			where c.CompilationParameters.GetDefineSymbols().Contains("DEBUG") == isDebug
			select c).ToArray<DotNetProjectConfiguration>();
			SolutionItemConfiguration result;
			if ((result = base.FindBestMatch(configurations)) == null && (result = source.FirstOrDefault((DotNetProjectConfiguration c) => base.Platform == c.Platform)) == null)
			{
				result = source.FirstOrDefault((DotNetProjectConfiguration c) => c.Platform == "");
			}
			return result;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x000289AC File Offset: 0x00026BAC
		public TargetFramework TargetFramework
		{
			get
			{
				DotNetProject parentItem = this.ParentItem;
				if (parentItem != null)
				{
					return parentItem.TargetFramework;
				}
				return Services.ProjectService.DefaultTargetFramework;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x000289D4 File Offset: 0x00026BD4
		public TargetRuntime TargetRuntime
		{
			get
			{
				DotNetProject parentItem = this.ParentItem;
				if (parentItem != null)
				{
					return parentItem.TargetRuntime;
				}
				return Runtime.SystemAssemblyService.DefaultRuntime;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x000289FC File Offset: 0x00026BFC
		public ClrVersion ClrVersion
		{
			get
			{
				return this.TargetFramework.ClrVersion;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x00028A09 File Offset: 0x00026C09
		// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x00028A11 File Offset: 0x00026C11
		[ItemProperty("CodeGeneration")]
		public ConfigurationParameters CompilationParameters
		{
			get
			{
				return this.compilationParameters;
			}
			set
			{
				this.compilationParameters = value;
				if (this.compilationParameters != null)
				{
					this.compilationParameters.ParentConfiguration = this;
				}
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00028A30 File Offset: 0x00026C30
		public ProjectParameters ProjectParameters
		{
			get
			{
				DotNetProject parentItem = this.ParentItem;
				if (parentItem != null)
				{
					return parentItem.LanguageParameters;
				}
				return null;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x00028A50 File Offset: 0x00026C50
		public FilePath CompiledOutputName
		{
			get
			{
				FilePath filePath = this.OutputDirectory.Combine(new string[]
				{
					this.OutputAssembly
				});
				if (this.OutputAssembly.EndsWith(".dll") || this.OutputAssembly.EndsWith(".exe"))
				{
					return filePath;
				}
				return filePath + ((this.CompileTarget == CompileTarget.Library) ? ".dll" : ".exe");
			}
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00028AC8 File Offset: 0x00026CC8
		public override void CopyFrom(ItemConfiguration configuration)
		{
			base.CopyFrom(configuration);
			DotNetProjectConfiguration dotNetProjectConfiguration = (DotNetProjectConfiguration)configuration;
			this.assembly = dotNetProjectConfiguration.assembly;
			this.sourcePath = dotNetProjectConfiguration.sourcePath;
			if (this.ParentItem == null)
			{
				base.SetParentItem(dotNetProjectConfiguration.ParentItem);
			}
			this.CompilationParameters = ((dotNetProjectConfiguration.compilationParameters != null) ? dotNetProjectConfiguration.compilationParameters.Clone() : null);
			this.signAssembly = dotNetProjectConfiguration.signAssembly;
			this.delaySign = dotNetProjectConfiguration.delaySign;
			this.assemblyKeyFile = dotNetProjectConfiguration.assemblyKeyFile;
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x00028B4F File Offset: 0x00026D4F
		public new DotNetProject ParentItem
		{
			get
			{
				return (DotNetProject)base.ParentItem;
			}
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00028B5C File Offset: 0x00026D5C
		public virtual IEnumerable<string> GetDefineSymbols()
		{
			if (this.CompilationParameters != null)
			{
				return this.CompilationParameters.GetDefineSymbols();
			}
			return new string[0];
		}

		// Token: 0x0400033C RID: 828
		[ItemProperty("AssemblyName")]
		[MergeToProject]
		private string assembly;

		// Token: 0x0400033D RID: 829
		private ConfigurationParameters compilationParameters;

		// Token: 0x0400033E RID: 830
		private string sourcePath;

		// Token: 0x0400033F RID: 831
		[MergeToProject]
		[ItemProperty("SignAssembly", DefaultValue = false)]
		private bool signAssembly;

		// Token: 0x04000340 RID: 832
		[ItemProperty("DelaySign", DefaultValue = false)]
		[MergeToProject]
		private bool delaySign;

		// Token: 0x04000341 RID: 833
		[ProjectPathItemProperty("AssemblyOriginatorKeyFile", DefaultValue = "")]
		[MergeToProject]
		private string assemblyKeyFile = "";
	}
}
