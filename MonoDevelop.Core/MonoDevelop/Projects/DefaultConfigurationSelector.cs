using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000187 RID: 391
	public class DefaultConfigurationSelector : ConfigurationSelector
	{
		// Token: 0x06000F46 RID: 3910 RVA: 0x000392E0 File Offset: 0x000374E0
		public override ItemConfiguration GetConfiguration(IConfigurationTarget target)
		{
			return target.DefaultConfiguration;
		}
	}
}
