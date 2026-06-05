using System;
using System.Diagnostics;

namespace MonoDevelop.Core
{
	// Token: 0x020000EB RID: 235
	[DebuggerDisplay("{id}")]
	public struct IconId : IEquatable<IconId>
	{
		// Token: 0x0600083C RID: 2108 RVA: 0x000214E4 File Offset: 0x0001F6E4
		public IconId(string id)
		{
			this.id = id;
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x000214ED File Offset: 0x0001F6ED
		public bool IsNull
		{
			get
			{
				return this.id == null;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x000214F8 File Offset: 0x0001F6F8
		public string Name
		{
			get
			{
				if (IconId.IconNameRequestHandler != null)
				{
					IconId.IconNameRequestHandler(this.id);
				}
				return this.id;
			}
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00021517 File Offset: 0x0001F717
		public static implicit operator IconId(string name)
		{
			return new IconId(name);
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0002151F File Offset: 0x0001F71F
		public static implicit operator string(IconId icon)
		{
			return icon.Name;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00021528 File Offset: 0x0001F728
		public static bool operator ==(IconId name1, IconId name2)
		{
			return name1.id == name2.id;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0002153D File Offset: 0x0001F73D
		public static bool operator !=(IconId name1, IconId name2)
		{
			return name1.id != name2.id;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00021552 File Offset: 0x0001F752
		public static bool operator ==(IconId name1, string name2)
		{
			return name1.id == name2;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00021561 File Offset: 0x0001F761
		public static bool operator !=(IconId name1, string name2)
		{
			return name1.id != name2;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00021570 File Offset: 0x0001F770
		public override bool Equals(object obj)
		{
			if (!(obj is IconId))
			{
				return false;
			}
			IconId iconId = (IconId)obj;
			return this.id == iconId.id;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x000215A0 File Offset: 0x0001F7A0
		public override int GetHashCode()
		{
			if (this.id == null)
			{
				return 0;
			}
			return this.id.GetHashCode();
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000215B7 File Offset: 0x0001F7B7
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x000215BF File Offset: 0x0001F7BF
		bool IEquatable<IconId>.Equals(IconId other)
		{
			return this.id == other.id;
		}

		// Token: 0x040002A3 RID: 675
		private string id;

		// Token: 0x040002A4 RID: 676
		public static readonly IconId Null = new IconId(null);

		// Token: 0x040002A5 RID: 677
		public static IconNameRequestHandler IconNameRequestHandler;
	}
}
