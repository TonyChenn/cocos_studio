using System;

namespace MonoDevelop.Projects
{
	// Token: 0x0200016E RID: 366
	public class SolutionConfigurationCollection : ItemConfigurationCollection<SolutionConfiguration>
	{
		// Token: 0x06000E62 RID: 3682 RVA: 0x000353C3 File Offset: 0x000335C3
		public SolutionConfigurationCollection()
		{
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x000353CB File Offset: 0x000335CB
		internal SolutionConfigurationCollection(Solution parentSolution)
		{
			this.parentSolution = parentSolution;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x000353DA File Offset: 0x000335DA
		protected override void OnItemAdded(SolutionConfiguration conf)
		{
			if (this.parentSolution != null)
			{
				conf.ParentSolution = this.parentSolution;
				this.parentSolution.NotifyConfigurationsChanged();
			}
			base.OnItemAdded(conf);
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00035402 File Offset: 0x00033602
		protected override void OnItemRemoved(SolutionConfiguration conf)
		{
			if (this.parentSolution != null)
			{
				conf.ParentSolution = null;
				this.parentSolution.NotifyConfigurationsChanged();
			}
			base.OnItemRemoved(conf);
		}

		// Token: 0x04000422 RID: 1058
		private Solution parentSolution;
	}
}
