using System;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000003 RID: 3
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class EngineClassNameAttribute : Attribute
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002084 File Offset: 0x00000284
		// (set) Token: 0x06000005 RID: 5 RVA: 0x0000209B File Offset: 0x0000029B
		public string CocoType { get; set; }

		// Token: 0x06000006 RID: 6 RVA: 0x000020A4 File Offset: 0x000002A4
		public EngineClassNameAttribute(string cocoType)
		{
			this.CocoType = cocoType;
		}
	}
}
