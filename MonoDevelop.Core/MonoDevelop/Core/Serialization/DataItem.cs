using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200006B RID: 107
	[Serializable]
	public class DataItem : DataNode
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000380 RID: 896 RVA: 0x0000DBAD File Offset: 0x0000BDAD
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0000DBC8 File Offset: 0x0000BDC8
		public DataCollection ItemData
		{
			get
			{
				if (this.data == null)
				{
					this.data = new DataCollection();
				}
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0000DBD1 File Offset: 0x0000BDD1
		public bool HasItemData
		{
			get
			{
				return this.data != null && this.data.Count > 0;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0000DBEB File Offset: 0x0000BDEB
		// (set) Token: 0x06000384 RID: 900 RVA: 0x0000DBF3 File Offset: 0x0000BDF3
		public bool UniqueNames
		{
			get
			{
				return this.uniqueNames;
			}
			set
			{
				this.uniqueNames = value;
			}
		}

		// Token: 0x170000AC RID: 172
		public DataNode this[string name]
		{
			get
			{
				if (this.data == null)
				{
					return null;
				}
				return this.data[name];
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000DC14 File Offset: 0x0000BE14
		public DataNode Extract(string name)
		{
			if (this.data == null)
			{
				return null;
			}
			return this.data.Extract(name);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000DC2C File Offset: 0x0000BE2C
		public override string ToString()
		{
			return this.ToString(0);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000DC38 File Offset: 0x0000BE38
		internal override string ToString(int indent)
		{
			string text = new string(' ', indent);
			string text2 = text + "[" + base.Name + "]\n";
			foreach (object obj in this.ItemData)
			{
				DataNode dataNode = (DataNode)obj;
				text2 = text2 + dataNode.ToString(indent + 4) + "\n";
			}
			string text3 = text2;
			text2 = string.Concat(new string[]
			{
				text3,
				text,
				"[/",
				base.Name,
				"]"
			});
			return text2;
		}

		// Token: 0x04000139 RID: 313
		private DataCollection data;

		// Token: 0x0400013A RID: 314
		private bool uniqueNames = true;
	}
}
