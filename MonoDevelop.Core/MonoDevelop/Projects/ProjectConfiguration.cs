using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Core.StringParsing;

namespace MonoDevelop.Projects
{
	// Token: 0x0200011E RID: 286
	public class ProjectConfiguration : SolutionItemConfiguration
	{
		// Token: 0x06000A93 RID: 2707 RVA: 0x00028428 File Offset: 0x00026628
		public ProjectConfiguration()
		{
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00028480 File Offset: 0x00026680
		public ProjectConfiguration(string name) : base(name)
		{
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x000284D8 File Offset: 0x000266D8
		// (set) Token: 0x06000A96 RID: 2710 RVA: 0x00028558 File Offset: 0x00026758
		public virtual FilePath IntermediateOutputDirectory
		{
			get
			{
				if (!this.intermediateOutputDirectory.IsNullOrEmpty)
				{
					return this.intermediateOutputDirectory;
				}
				if (!string.IsNullOrEmpty(base.Platform))
				{
					return this.ParentItem.BaseIntermediateOutputPath.Combine(new string[]
					{
						base.Platform,
						base.Name
					});
				}
				return this.ParentItem.BaseIntermediateOutputPath.Combine(new string[]
				{
					base.Name
				});
			}
			set
			{
				if (value.IsNullOrEmpty)
				{
					value = FilePath.Null;
				}
				if (this.intermediateOutputDirectory == value)
				{
					return;
				}
				this.intermediateOutputDirectory = value;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x00028580 File Offset: 0x00026780
		// (set) Token: 0x06000A98 RID: 2712 RVA: 0x00028588 File Offset: 0x00026788
		public virtual FilePath OutputDirectory
		{
			get
			{
				return this.outputDirectory;
			}
			set
			{
				this.outputDirectory = value;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x00028591 File Offset: 0x00026791
		// (set) Token: 0x06000A9A RID: 2714 RVA: 0x00028599 File Offset: 0x00026799
		public bool DebugMode
		{
			get
			{
				return this.debugMode;
			}
			set
			{
				this.debugMode = value;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x000285A2 File Offset: 0x000267A2
		// (set) Token: 0x06000A9C RID: 2716 RVA: 0x000285AA File Offset: 0x000267AA
		public bool PauseConsoleOutput
		{
			get
			{
				return this.pauseConsoleOutput;
			}
			set
			{
				this.pauseConsoleOutput = value;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x000285B3 File Offset: 0x000267B3
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x000285BB File Offset: 0x000267BB
		public bool ExternalConsole
		{
			get
			{
				return this.externalConsole;
			}
			set
			{
				this.externalConsole = value;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x000285C4 File Offset: 0x000267C4
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x000285CC File Offset: 0x000267CC
		public string CommandLineParameters
		{
			get
			{
				return this.commandLineParameters;
			}
			set
			{
				this.commandLineParameters = value;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x000285D5 File Offset: 0x000267D5
		public Dictionary<string, string> EnvironmentVariables
		{
			get
			{
				return this.environmentVariables;
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x000285E0 File Offset: 0x000267E0
		public Dictionary<string, string> GetParsedEnvironmentVariables()
		{
			if (this.ParentItem == null)
			{
				return this.environmentVariables;
			}
			StringTagModel stringTagModel = this.ParentItem.GetStringTagModel(this.Selector);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> keyValuePair in this.environmentVariables)
			{
				dictionary[keyValuePair.Key] = StringParserService.Parse(keyValuePair.Value, stringTagModel);
			}
			return dictionary;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x00028670 File Offset: 0x00026870
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x00028678 File Offset: 0x00026878
		public virtual bool RunWithWarnings
		{
			get
			{
				return this.runWithWarnings;
			}
			set
			{
				this.runWithWarnings = value;
			}
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00028684 File Offset: 0x00026884
		public override void CopyFrom(ItemConfiguration conf)
		{
			base.CopyFrom(conf);
			ProjectConfiguration projectConfiguration = conf as ProjectConfiguration;
			this.intermediateOutputDirectory = projectConfiguration.intermediateOutputDirectory;
			this.outputDirectory = projectConfiguration.outputDirectory;
			this.debugMode = projectConfiguration.debugMode;
			this.pauseConsoleOutput = projectConfiguration.pauseConsoleOutput;
			this.externalConsole = projectConfiguration.externalConsole;
			this.commandLineParameters = projectConfiguration.commandLineParameters;
			this.environmentVariables.Clear();
			foreach (KeyValuePair<string, string> keyValuePair in projectConfiguration.environmentVariables)
			{
				this.environmentVariables.Add(keyValuePair.Key, keyValuePair.Value);
			}
			this.runWithWarnings = projectConfiguration.runWithWarnings;
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x00028758 File Offset: 0x00026958
		public new Project ParentItem
		{
			get
			{
				return (Project)base.ParentItem;
			}
		}

		// Token: 0x0400032F RID: 815
		[ProjectPathItemProperty("IntermediateOutputPath")]
		private FilePath intermediateOutputDirectory;

		// Token: 0x04000330 RID: 816
		[ProjectPathItemProperty("OutputPath")]
		private FilePath outputDirectory = "." + Path.DirectorySeparatorChar;

		// Token: 0x04000331 RID: 817
		[ItemProperty("DebugSymbols", DefaultValue = false)]
		private bool debugMode;

		// Token: 0x04000332 RID: 818
		[ItemProperty("ConsolePause", DefaultValue = true)]
		private bool pauseConsoleOutput = true;

		// Token: 0x04000333 RID: 819
		[ItemProperty("Externalconsole", DefaultValue = false)]
		private bool externalConsole;

		// Token: 0x04000334 RID: 820
		[ItemProperty("Commandlineparameters", DefaultValue = "")]
		private string commandLineParameters = "";

		// Token: 0x04000335 RID: 821
		[ItemProperty("EnvironmentVariables", SkipEmpty = true)]
		[ItemProperty("value", Scope = "value")]
		[ItemProperty("name", Scope = "key")]
		[ItemProperty("Variable", Scope = "item")]
		private Dictionary<string, string> environmentVariables = new Dictionary<string, string>();

		// Token: 0x04000336 RID: 822
		[ItemProperty("RunWithWarnings", DefaultValue = true)]
		private bool runWithWarnings = true;
	}
}
