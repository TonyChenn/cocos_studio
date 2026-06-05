using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200006D RID: 109
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public class DataItemAttribute : Attribute, IDataItemAttribute
	{
		// Token: 0x0600038C RID: 908 RVA: 0x0000DD0F File Offset: 0x0000BF0F
		public DataItemAttribute()
		{
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000DD17 File Offset: 0x0000BF17
		public DataItemAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000DD26 File Offset: 0x0000BF26
		// (set) Token: 0x0600038F RID: 911 RVA: 0x0000DD2E File Offset: 0x0000BF2E
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0000DD37 File Offset: 0x0000BF37
		// (set) Token: 0x06000391 RID: 913 RVA: 0x0000DD3F File Offset: 0x0000BF3F
		public Type FallbackType
		{
			get
			{
				return this.fallbackType;
			}
			set
			{
				this.fallbackType = value;
			}
		}

		// Token: 0x0400013B RID: 315
		private string name;

		// Token: 0x0400013C RID: 316
		private Type fallbackType;
	}
}
