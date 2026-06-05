using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000149 RID: 329
	public class ConfigurationEventArgs : SolutionItemEventArgs
	{
		// Token: 0x06000C4B RID: 3147 RVA: 0x0002D781 File Offset: 0x0002B981
		public ConfigurationEventArgs(SolutionEntityItem entry, ItemConfiguration configuration) : base(entry)
		{
			this.configuration = configuration;
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x0002D791 File Offset: 0x0002B991
		public ItemConfiguration Configuration
		{
			get
			{
				return this.configuration;
			}
		}

		// Token: 0x040003A6 RID: 934
		private ItemConfiguration configuration;
	}
}
