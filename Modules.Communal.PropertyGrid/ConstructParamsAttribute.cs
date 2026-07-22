using System;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000002 RID: 2
	public class ConstructParamsAttribute : Attribute
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002067 File Offset: 0x00000267
		public object[] ConstructParams { get; private set; }

		// Token: 0x06000003 RID: 3 RVA: 0x00002070 File Offset: 0x00000270
		public ConstructParamsAttribute(params object[] args)
		{
			this.ConstructParams = args;
		}
	}
}
