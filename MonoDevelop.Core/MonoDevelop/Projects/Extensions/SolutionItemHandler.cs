using System;
using System.CodeDom.Compiler;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x0200019A RID: 410
	public abstract class SolutionItemHandler : ISolutionItemHandler, IDisposable
	{
		// Token: 0x06000FB2 RID: 4018 RVA: 0x0003A79B File Offset: 0x0003899B
		public SolutionItemHandler(SolutionItem item)
		{
			this.item = item;
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x0003A7AA File Offset: 0x000389AA
		public virtual bool SyncFileName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0003A7AD File Offset: 0x000389AD
		public SolutionItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0003A7B8 File Offset: 0x000389B8
		public virtual BuildResult RunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
		{
			if (target != null)
			{
				if (target == "Build")
				{
					return this.OnBuild(monitor, configuration);
				}
				if (target == "Clean")
				{
					return this.OnClean(monitor, configuration);
				}
			}
			return new BuildResult(new CompilerResults(null), "");
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0003A808 File Offset: 0x00038A08
		protected virtual BuildResult OnBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			return null;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0003A80B File Offset: 0x00038A0B
		protected virtual BuildResult OnClean(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			return null;
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0003A80E File Offset: 0x00038A0E
		public virtual void Dispose()
		{
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000FB9 RID: 4025
		public abstract string ItemId { get; }

		// Token: 0x06000FBA RID: 4026
		public abstract void Save(IProgressMonitor monitor);

		// Token: 0x06000FBB RID: 4027 RVA: 0x0003A810 File Offset: 0x00038A10
		public virtual void OnModified(string hint)
		{
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0003A812 File Offset: 0x00038A12
		public virtual object GetService(Type t)
		{
			return null;
		}

		// Token: 0x04000488 RID: 1160
		private SolutionItem item;
	}
}
