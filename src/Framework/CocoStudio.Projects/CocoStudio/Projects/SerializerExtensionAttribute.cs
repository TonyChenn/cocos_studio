using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x0200006F RID: 111
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class SerializerExtensionAttribute : Attribute
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0000C871 File Offset: 0x0000AA71
		// (set) Token: 0x0600037C RID: 892 RVA: 0x0000C879 File Offset: 0x0000AA79
		[NodeAttribute]
		public bool IsDefault { get; private set; }

		// Token: 0x0600037D RID: 893 RVA: 0x0000C882 File Offset: 0x0000AA82
		public SerializerExtensionAttribute()
		{
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000C88A File Offset: 0x0000AA8A
		public SerializerExtensionAttribute([NodeAttribute("IsDefault")] bool isDefault)
		{
			this.IsDefault = isDefault;
		}
	}
}
