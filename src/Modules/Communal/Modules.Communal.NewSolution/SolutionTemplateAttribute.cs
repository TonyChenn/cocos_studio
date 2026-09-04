using System;
using Mono.Addins;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000009 RID: 9
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	internal class SolutionTemplateAttribute : Attribute
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002B19 File Offset: 0x00000D19
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002B21 File Offset: 0x00000D21
		[NodeAttribute]
		public bool IsDefault { get; private set; }

		// Token: 0x0600003E RID: 62 RVA: 0x00002B2A File Offset: 0x00000D2A
		public SolutionTemplateAttribute()
		{
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002B32 File Offset: 0x00000D32
		internal SolutionTemplateAttribute([NodeAttribute("IsDefault")] bool isDefault)
		{
			this.IsDefault = isDefault;
		}
	}
}
