using System;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000CE RID: 206
	public class FrameChangeTimelineRenderArgs : EventArgs
	{
		// Token: 0x06000671 RID: 1649 RVA: 0x0001A025 File Offset: 0x00018225
		public FrameChangeTimelineRenderArgs(bool isUnRedoing, bool includeEasignPanel = false)
		{
			this.IsUnRedoing = isUnRedoing;
			this.IncludeEasingPanel = includeEasignPanel;
		}

		// Token: 0x040002C9 RID: 713
		public bool IncludeEasingPanel = false;

		// Token: 0x040002CA RID: 714
		public bool IsUnRedoing = false;
	}
}
