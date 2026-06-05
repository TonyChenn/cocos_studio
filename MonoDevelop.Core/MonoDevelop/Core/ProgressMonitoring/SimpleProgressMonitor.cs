using System;

namespace MonoDevelop.Core.ProgressMonitoring
{
	// Token: 0x020000EE RID: 238
	public class SimpleProgressMonitor : NullProgressMonitor
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x00021606 File Offset: 0x0001F806
		protected ProgressTracker Tracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00021621 File Offset: 0x0001F821
		public override void BeginTask(string name, int totalWork)
		{
			this.tracker.BeginTask(name, totalWork);
			this.OnProgressChanged();
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00021636 File Offset: 0x0001F836
		public override void BeginStepTask(string name, int totalWork, int stepSize)
		{
			this.tracker.BeginStepTask(name, totalWork, stepSize);
			this.OnProgressChanged();
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0002164C File Offset: 0x0001F84C
		public override void EndTask()
		{
			this.tracker.EndTask();
			this.OnProgressChanged();
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0002165F File Offset: 0x0001F85F
		public override void Step(int work)
		{
			this.tracker.Step(work);
			this.OnProgressChanged();
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00021673 File Offset: 0x0001F873
		protected override void OnCompleted()
		{
			base.OnCompleted();
			this.OnProgressChanged();
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00021681 File Offset: 0x0001F881
		protected virtual void OnProgressChanged()
		{
		}

		// Token: 0x040002A8 RID: 680
		private ProgressTracker tracker = new ProgressTracker();
	}
}
