using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000186 RID: 390
	public class SolutionConfigurationSelector : ConfigurationSelector
	{
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x00039241 File Offset: 0x00037441
		// (set) Token: 0x06000F41 RID: 3905 RVA: 0x00039249 File Offset: 0x00037449
		public string Id { get; private set; }

		// Token: 0x06000F42 RID: 3906 RVA: 0x00039252 File Offset: 0x00037452
		public SolutionConfigurationSelector(string id)
		{
			this.Id = id;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00039261 File Offset: 0x00037461
		public override string ToString()
		{
			return this.Id;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00039269 File Offset: 0x00037469
		public static explicit operator SolutionConfigurationSelector(string id)
		{
			return new SolutionConfigurationSelector(id);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x00039274 File Offset: 0x00037474
		public override ItemConfiguration GetConfiguration(IConfigurationTarget target)
		{
			if (target is SolutionEntityItem)
			{
				SolutionEntityItem solutionEntityItem = (SolutionEntityItem)target;
				if (solutionEntityItem.ParentSolution != null)
				{
					SolutionConfiguration solutionConfiguration = solutionEntityItem.ParentSolution.Configurations[this.Id];
					if (solutionConfiguration != null)
					{
						string mappedConfiguration = solutionConfiguration.GetMappedConfiguration(solutionEntityItem);
						if (mappedConfiguration != null)
						{
							ItemConfiguration itemConfiguration = solutionEntityItem.Configurations[mappedConfiguration];
							if (itemConfiguration != null)
							{
								return itemConfiguration;
							}
						}
					}
				}
			}
			return target.Configurations[this.Id];
		}
	}
}
