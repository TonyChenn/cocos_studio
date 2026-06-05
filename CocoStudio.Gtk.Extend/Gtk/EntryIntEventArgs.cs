using System;

namespace Gtk
{
	// Token: 0x0200001D RID: 29
	public class EntryIntEventArgs : EventArgs
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00005A14 File Offset: 0x00003C14
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00005A2B File Offset: 0x00003C2B
		public float Value { get; private set; }

		// Token: 0x060000EE RID: 238 RVA: 0x00005A34 File Offset: 0x00003C34
		public EntryIntEventArgs(float value)
		{
			this.Value = value;
		}
	}
}
