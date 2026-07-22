using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200004D RID: 77
	public class SelectedChangingEventArgs : EventArgs
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000A671 File Offset: 0x00008871
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000A679 File Offset: 0x00008879
		public bool IsSelected { get; private set; }

		// Token: 0x06000298 RID: 664 RVA: 0x0000A682 File Offset: 0x00008882
		public SelectedChangingEventArgs(bool isSelected)
		{
			this.IsSelected = isSelected;
		}
	}
}
