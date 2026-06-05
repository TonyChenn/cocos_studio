using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000180 RID: 384
	public abstract class DotNetConfigurationParameters : ConfigurationParameters
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000F27 RID: 3879
		// (set) Token: 0x06000F28 RID: 3880
		public abstract bool NoStdLib { get; set; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x00038E47 File Offset: 0x00037047
		// (set) Token: 0x06000F2A RID: 3882 RVA: 0x00038E4E File Offset: 0x0003704E
		public virtual string DebugType
		{
			get
			{
				return "";
			}
			set
			{
			}
		}
	}
}
