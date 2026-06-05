using System;
using System.Collections.Generic;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x0200024D RID: 589
	public class UnknownProject : Project
	{
		// Token: 0x060015A0 RID: 5536 RVA: 0x00057BDB File Offset: 0x00055DDB
		public UnknownProject()
		{
			this.NeedsReload = false;
			this.loadError = GettextCatalog.GetString("Unknown project type");
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00057C05 File Offset: 0x00055E05
		public UnknownProject(FilePath file, string loadError) : this()
		{
			this.NeedsReload = false;
			this.FileName = file;
			this.loadError = loadError;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x00057C22 File Offset: 0x00055E22
		public override bool SupportsConfigurations()
		{
			return true;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x00057CF0 File Offset: 0x00055EF0
		public override IEnumerable<string> GetProjectTypes()
		{
			yield return "";
			yield break;
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x00057D0D File Offset: 0x00055F0D
		// (set) Token: 0x060015A5 RID: 5541 RVA: 0x00057D15 File Offset: 0x00055F15
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

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x00057D37 File Offset: 0x00055F37
		// (set) Token: 0x060015A7 RID: 5543 RVA: 0x00057D52 File Offset: 0x00055F52
		public string LoadError
		{
			get
			{
				if (!this.unloaded)
				{
					return this.loadError;
				}
				return GettextCatalog.GetString("Unavailable");
			}
			set
			{
				this.loadError = value;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x00057D5B File Offset: 0x00055F5B
		// (set) Token: 0x060015A9 RID: 5545 RVA: 0x00057D63 File Offset: 0x00055F63
		public bool UnloadedEntry
		{
			get
			{
				return this.unloaded;
			}
			set
			{
				this.unloaded = value;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060015AA RID: 5546 RVA: 0x00057D6C File Offset: 0x00055F6C
		// (set) Token: 0x060015AB RID: 5547 RVA: 0x00057DA2 File Offset: 0x00055FA2
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

		// Token: 0x060015AC RID: 5548 RVA: 0x00057DA4 File Offset: 0x00055FA4
		protected internal override bool OnGetSupportsTarget(string target)
		{
			return false;
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00057DA7 File Offset: 0x00055FA7
		protected override void OnClean(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00057DAC File Offset: 0x00055FAC
		protected override BuildResult OnBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			BuildResult buildResult = new BuildResult();
			buildResult.AddError(this.loadError);
			return buildResult;
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00057DCC File Offset: 0x00055FCC
		protected internal override void OnExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00057DCE File Offset: 0x00055FCE
		protected internal override bool OnGetNeedsBuilding(ConfigurationSelector configuration)
		{
			return false;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00057DD1 File Offset: 0x00055FD1
		protected internal override void OnSetNeedsBuilding(bool value, ConfigurationSelector configuration)
		{
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00057DD3 File Offset: 0x00055FD3
		public override SolutionItemConfiguration CreateConfiguration(string name)
		{
			return new ProjectConfiguration(name);
		}

		// Token: 0x04000688 RID: 1672
		private string loadError = string.Empty;

		// Token: 0x04000689 RID: 1673
		private bool unloaded;

		// Token: 0x0400068A RID: 1674
		private FilePath fileName;
	}
}
