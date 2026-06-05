using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000107 RID: 263
	public class SolutionConfigurationEntry
	{
		// Token: 0x06000983 RID: 2435 RVA: 0x000260A4 File Offset: 0x000242A4
		internal SolutionConfigurationEntry(SolutionConfiguration parentConfig, SolutionConfigurationEntry other)
		{
			this.parentConfig = parentConfig;
			this.itemId = other.itemId;
			this.configuration = other.configuration;
			this.build = other.build;
			this.deploy = other.deploy;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x000260F5 File Offset: 0x000242F5
		internal SolutionConfigurationEntry(SolutionConfiguration parentConfig, SolutionEntityItem item)
		{
			this.parentConfig = parentConfig;
			this.item = item;
			this.configuration = parentConfig.Id;
			this.itemId = item.ItemId;
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0002612A File Offset: 0x0002432A
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x00026132 File Offset: 0x00024332
		public string ItemConfiguration
		{
			get
			{
				return this.configuration;
			}
			set
			{
				this.configuration = value;
				if (this.parentConfig != null && this.parentConfig.ParentSolution != null)
				{
					this.parentConfig.ParentSolution.UpdateDefaultConfigurations();
				}
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x00026160 File Offset: 0x00024360
		public ConfigurationSelector ItemConfigurationSelector
		{
			get
			{
				return new ItemConfigurationSelector(this.ItemConfiguration);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0002616D File Offset: 0x0002436D
		// (set) Token: 0x06000989 RID: 2441 RVA: 0x00026175 File Offset: 0x00024375
		public bool Build
		{
			get
			{
				return this.build;
			}
			set
			{
				this.build = value;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0002617E File Offset: 0x0002437E
		// (set) Token: 0x0600098B RID: 2443 RVA: 0x00026186 File Offset: 0x00024386
		public bool Deploy
		{
			get
			{
				return this.deploy;
			}
			set
			{
				this.deploy = value;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x00026190 File Offset: 0x00024390
		// (set) Token: 0x0600098D RID: 2445 RVA: 0x000261D9 File Offset: 0x000243D9
		public SolutionEntityItem Item
		{
			get
			{
				if (this.item == null && this.parentConfig != null)
				{
					Solution parentSolution = this.parentConfig.ParentSolution;
					if (parentSolution != null)
					{
						this.item = (parentSolution.GetSolutionItem(this.itemId) as SolutionEntityItem);
					}
				}
				return this.item;
			}
			internal set
			{
				this.item = value;
			}
		}

		// Token: 0x040002FC RID: 764
		private SolutionEntityItem item;

		// Token: 0x040002FD RID: 765
		private SolutionConfiguration parentConfig;

		// Token: 0x040002FE RID: 766
		[ItemProperty("name")]
		private string itemId;

		// Token: 0x040002FF RID: 767
		[ItemProperty]
		private string configuration;

		// Token: 0x04000300 RID: 768
		[ItemProperty(DefaultValue = true)]
		private bool build = true;

		// Token: 0x04000301 RID: 769
		[ItemProperty(DefaultValue = false)]
		private bool deploy;
	}
}
