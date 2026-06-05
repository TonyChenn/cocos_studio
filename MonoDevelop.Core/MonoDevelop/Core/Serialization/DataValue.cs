using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200006F RID: 111
	[Serializable]
	public class DataValue : DataNode
	{
		// Token: 0x060003A6 RID: 934 RVA: 0x0000DE9B File Offset: 0x0000C09B
		public DataValue(string name, string value)
		{
			base.Name = name;
			this.value = value;
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000DEB1 File Offset: 0x0000C0B1
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000DEB9 File Offset: 0x0000C0B9
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x0000DEC1 File Offset: 0x0000C0C1
		internal bool StoreAsAttribute
		{
			get
			{
				return this.storeAsAttribute;
			}
			set
			{
				this.storeAsAttribute = value;
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000DECA File Offset: 0x0000C0CA
		public override string ToString()
		{
			return this.ToString(0);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000DED4 File Offset: 0x0000C0D4
		internal override string ToString(int indent)
		{
			return string.Concat(new string[]
			{
				new string(' ', indent),
				"[",
				base.Name,
				" = '",
				this.value,
				"']"
			});
		}

		// Token: 0x0400013F RID: 319
		private string value;

		// Token: 0x04000140 RID: 320
		private bool storeAsAttribute;
	}
}
