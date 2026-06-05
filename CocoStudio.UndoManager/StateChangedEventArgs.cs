using System;

namespace CocoStudio.UndoManager
{
	// Token: 0x0200001E RID: 30
	public class StateChangedEventArgs : EventArgs
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00004A84 File Offset: 0x00002C84
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00004A9B File Offset: 0x00002C9B
		public string PropertyName { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00004AA4 File Offset: 0x00002CA4
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00004ABB File Offset: 0x00002CBB
		public object OldValue { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00004AC4 File Offset: 0x00002CC4
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00004ADB File Offset: 0x00002CDB
		public object NewValue { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00004AE4 File Offset: 0x00002CE4
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00004AFB File Offset: 0x00002CFB
		public bool IsProvideValue { get; private set; }

		// Token: 0x060000E4 RID: 228 RVA: 0x00004B04 File Offset: 0x00002D04
		public StateChangedEventArgs(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004B17 File Offset: 0x00002D17
		public StateChangedEventArgs(string propertyName, object oldValue, object newValue)
		{
			this.PropertyName = propertyName;
			this.OldValue = oldValue;
			this.NewValue = newValue;
			this.IsProvideValue = true;
		}
	}
}
