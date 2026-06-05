using System;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x0200000D RID: 13
	public class DataEventArgs<TData> : EventArgs
	{
		// Token: 0x06000029 RID: 41 RVA: 0x00002A68 File Offset: 0x00000C68
		public DataEventArgs(TData value)
		{
			this._value = value;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002A7C File Offset: 0x00000C7C
		public TData Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x04000012 RID: 18
		private readonly TData _value;
	}
}
