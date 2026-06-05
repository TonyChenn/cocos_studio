using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200006A RID: 106
	[Serializable]
	public class DataNode
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0000DB8D File Offset: 0x0000BD8D
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0000DB95 File Offset: 0x0000BD95
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

		// Token: 0x0600037E RID: 894 RVA: 0x0000DB9E File Offset: 0x0000BD9E
		internal virtual string ToString(int indent)
		{
			return "";
		}

		// Token: 0x04000138 RID: 312
		private string name;
	}
}
