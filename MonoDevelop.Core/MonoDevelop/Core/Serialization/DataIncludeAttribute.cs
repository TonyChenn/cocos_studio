using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000069 RID: 105
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public class DataIncludeAttribute : Attribute
	{
		// Token: 0x0600037A RID: 890 RVA: 0x0000DB76 File Offset: 0x0000BD76
		public DataIncludeAttribute(Type type)
		{
			this.type = type;
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0000DB85 File Offset: 0x0000BD85
		public Type Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x04000137 RID: 311
		private Type type;
	}
}
