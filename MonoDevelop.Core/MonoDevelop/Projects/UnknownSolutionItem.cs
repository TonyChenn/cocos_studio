using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x0200015A RID: 346
	public class UnknownSolutionItem : SolutionEntityItem
	{
		// Token: 0x06000CB3 RID: 3251 RVA: 0x0002EE87 File Offset: 0x0002D087
		public UnknownSolutionItem()
		{
			this.NeedsReload = false;
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0002EEA1 File Offset: 0x0002D0A1
		public override bool SupportsConfigurations()
		{
			return true;
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0002EEA4 File Offset: 0x0002D0A4
		// (set) Token: 0x06000CB6 RID: 3254 RVA: 0x0002EEAC File Offset: 0x0002D0AC
		public override FilePath FileName
		{
			get
			{
				return this.fileName;
			}
			set
			{
				if (this.fileName == FilePath.Null)
				{
					this.fileName = value;
					this.NeedsReload = false;
				}
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0002EECE File Offset: 0x0002D0CE
		// (set) Token: 0x06000CB8 RID: 3256 RVA: 0x0002EED6 File Offset: 0x0002D0D6
		public string LoadError
		{
			get
			{
				return this.loadError;
			}
			set
			{
				this.loadError = value;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x0002EEDF File Offset: 0x0002D0DF
		// (set) Token: 0x06000CBA RID: 3258 RVA: 0x0002EEE7 File Offset: 0x0002D0E7
		public bool UnloadedEntry
		{
			get
			{
				return this.unloaded;
			}
			set
			{
				this.unloaded = value;
				if (value)
				{
					this.loadError = GettextCatalog.GetString("Unavailable");
				}
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x0002EF04 File Offset: 0x0002D104
		// (set) Token: 0x06000CBC RID: 3260 RVA: 0x0002EF3A File Offset: 0x0002D13A
		public override string Name
		{
			get
			{
				if (!this.FileName.IsNullOrEmpty)
				{
					return this.FileName.FileNameWithoutExtension;
				}
				return GettextCatalog.GetString("Unknown entry");
			}
			set
			{
			}
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0002EF3C File Offset: 0x0002D13C
		protected internal override bool OnGetSupportsTarget(string target)
		{
			return false;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0002EF3F File Offset: 0x0002D13F
		protected override void OnClean(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0002EF44 File Offset: 0x0002D144
		protected override BuildResult OnBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			BuildResult buildResult = new BuildResult();
			buildResult.AddError("Project unavailable");
			return buildResult;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0002EF63 File Offset: 0x0002D163
		protected internal override void OnExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0002EF65 File Offset: 0x0002D165
		protected internal override bool OnGetNeedsBuilding(ConfigurationSelector configuration)
		{
			return false;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0002EF68 File Offset: 0x0002D168
		protected internal override void OnSetNeedsBuilding(bool value, ConfigurationSelector configuration)
		{
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0002EF6A File Offset: 0x0002D16A
		protected internal override void OnSave(IProgressMonitor monitor)
		{
		}

		// Token: 0x040003CF RID: 975
		private string loadError = string.Empty;

		// Token: 0x040003D0 RID: 976
		private bool unloaded;

		// Token: 0x040003D1 RID: 977
		private FilePath fileName;
	}
}
