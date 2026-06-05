using System;

namespace MonoDevelop.Projects
{
	/// <summary>
	/// Specifies a configuration to be used for solution and project operations
	/// </summary>
	// Token: 0x02000184 RID: 388
	public class ConfigurationSelector
	{
		/// <summary>
		/// Gets the configuration selected by this instance for a given target
		/// </summary>
		/// <returns>
		/// The configuration.
		/// </returns>
		/// <param name="target">
		/// A target
		/// </param>
		// Token: 0x06000F37 RID: 3895 RVA: 0x000391E7 File Offset: 0x000373E7
		public virtual ItemConfiguration GetConfiguration(IConfigurationTarget target)
		{
			return null;
		}

		/// <summary>
		/// A configuration selector which selects the default (active) project or solution configuration
		/// </summary>
		// Token: 0x04000466 RID: 1126
		public static readonly ConfigurationSelector Default = new DefaultConfigurationSelector();
	}
}
