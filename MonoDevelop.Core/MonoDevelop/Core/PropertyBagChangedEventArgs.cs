using System;

namespace MonoDevelop.Core
{
	// Token: 0x020000C2 RID: 194
	public class PropertyBagChangedEventArgs : EventArgs
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0001A8F9 File Offset: 0x00018AF9
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x0001A901 File Offset: 0x00018B01
		public string PropertyName { get; private set; }

		// Token: 0x060006AB RID: 1707 RVA: 0x0001A90A File Offset: 0x00018B0A
		public PropertyBagChangedEventArgs(string name)
		{
			this.PropertyName = name;
		}
	}
}
