using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x0200011D RID: 285
	[DataItem(FallbackType = typeof(UnknownConfiguration))]
	public class SolutionItemConfiguration : ItemConfiguration
	{
		// Token: 0x06000A8B RID: 2699 RVA: 0x00028358 File Offset: 0x00026558
		public SolutionItemConfiguration()
		{
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00028360 File Offset: 0x00026560
		public SolutionItemConfiguration(string id) : base(id)
		{
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x00028369 File Offset: 0x00026569
		public SolutionEntityItem ParentItem
		{
			get
			{
				return this.parentItem;
			}
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x000283D4 File Offset: 0x000265D4
		public virtual SolutionItemConfiguration FindBestMatch(SolutionItemConfigurationCollection configurations)
		{
			IEnumerable<SolutionItemConfiguration> source = configurations.Cast<SolutionItemConfiguration>();
			return source.FirstOrDefault((SolutionItemConfiguration c) => base.Name == c.Name && base.Platform == c.Platform) ?? source.FirstOrDefault((SolutionItemConfiguration c) => base.Name == c.Name && (c.Platform == "" || c.Platform == "Any CPU"));
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00028410 File Offset: 0x00026610
		internal void SetParentItem(SolutionEntityItem item)
		{
			this.parentItem = item;
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00028419 File Offset: 0x00026619
		public override ConfigurationSelector Selector
		{
			get
			{
				return new ItemConfigurationSelector(base.Id);
			}
		}

		// Token: 0x0400032E RID: 814
		private SolutionEntityItem parentItem;
	}
}
