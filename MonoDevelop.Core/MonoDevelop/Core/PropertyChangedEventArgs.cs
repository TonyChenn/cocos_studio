using System;

namespace MonoDevelop.Core
{
	// Token: 0x0200004A RID: 74
	public class PropertyChangedEventArgs : EventArgs
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600025C RID: 604 RVA: 0x000099E0 File Offset: 0x00007BE0
		public string Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600025D RID: 605 RVA: 0x000099E8 File Offset: 0x00007BE8
		public object NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600025E RID: 606 RVA: 0x000099F0 File Offset: 0x00007BF0
		public object OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000099F8 File Offset: 0x00007BF8
		public PropertyChangedEventArgs(string key, object oldValue, object newValue)
		{
			this.key = key;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		// Token: 0x040000DB RID: 219
		private string key;

		// Token: 0x040000DC RID: 220
		private object newValue;

		// Token: 0x040000DD RID: 221
		private object oldValue;
	}
}
