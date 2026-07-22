using System;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000005 RID: 5
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public sealed class ValueRangeAttribute : Attribute
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000020EC File Offset: 0x000002EC
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002103 File Offset: 0x00000303
		public int MaxValue { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000210C File Offset: 0x0000030C
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002123 File Offset: 0x00000323
		public int MinValue { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000212C File Offset: 0x0000032C
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002143 File Offset: 0x00000343
		public float Step { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000214C File Offset: 0x0000034C
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002163 File Offset: 0x00000363
		public float PageStep { get; set; }

		// Token: 0x06000012 RID: 18 RVA: 0x0000216C File Offset: 0x0000036C
		public ValueRangeAttribute(int min = 0, int max = 2147483647, float step = 1f, float pageStep = 10f)
		{
			this.MaxValue = max;
			this.MinValue = min;
			this.Step = step;
			this.PageStep = pageStep;
		}
	}
}
