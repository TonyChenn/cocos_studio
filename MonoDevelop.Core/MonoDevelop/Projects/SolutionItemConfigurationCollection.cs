using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000147 RID: 327
	public class SolutionItemConfigurationCollection : ItemConfigurationCollection<SolutionItemConfiguration>
	{
		// Token: 0x06000C43 RID: 3139 RVA: 0x0002D735 File Offset: 0x0002B935
		public SolutionItemConfigurationCollection()
		{
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0002D73D File Offset: 0x0002B93D
		internal SolutionItemConfigurationCollection(SolutionEntityItem parentItem)
		{
			this.parentItem = parentItem;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0002D74C File Offset: 0x0002B94C
		protected override void OnItemAdded(SolutionItemConfiguration conf)
		{
			if (this.parentItem != null)
			{
				conf.SetParentItem(this.parentItem);
			}
			base.OnItemAdded(conf);
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x0002D769 File Offset: 0x0002B969
		protected override void OnItemRemoved(SolutionItemConfiguration conf)
		{
			if (this.parentItem != null)
			{
				conf.SetParentItem(null);
			}
			base.OnItemRemoved(conf);
		}

		// Token: 0x040003A5 RID: 933
		private SolutionEntityItem parentItem;
	}
}
