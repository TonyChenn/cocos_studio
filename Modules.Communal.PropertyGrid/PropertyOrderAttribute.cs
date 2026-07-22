using System;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000004 RID: 4
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class PropertyOrderAttribute : Attribute
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020B8 File Offset: 0x000002B8
		// (set) Token: 0x06000008 RID: 8 RVA: 0x000020CF File Offset: 0x000002CF
		public int Order { get; set; }

		// Token: 0x06000009 RID: 9 RVA: 0x000020D8 File Offset: 0x000002D8
		public PropertyOrderAttribute(int order)
		{
			this.Order = order;
		}
	}
}
