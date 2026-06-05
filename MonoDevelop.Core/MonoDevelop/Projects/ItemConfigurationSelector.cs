using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000185 RID: 389
	public class ItemConfigurationSelector : ConfigurationSelector
	{
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x000391FE File Offset: 0x000373FE
		// (set) Token: 0x06000F3B RID: 3899 RVA: 0x00039206 File Offset: 0x00037406
		public string Id { get; private set; }

		// Token: 0x06000F3C RID: 3900 RVA: 0x0003920F File Offset: 0x0003740F
		public ItemConfigurationSelector(string id)
		{
			this.Id = id;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0003921E File Offset: 0x0003741E
		public static explicit operator ItemConfigurationSelector(string id)
		{
			return new ItemConfigurationSelector(id);
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x00039226 File Offset: 0x00037426
		public override ItemConfiguration GetConfiguration(IConfigurationTarget target)
		{
			return target.Configurations[this.Id];
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00039239 File Offset: 0x00037439
		public override string ToString()
		{
			return this.Id;
		}
	}
}
