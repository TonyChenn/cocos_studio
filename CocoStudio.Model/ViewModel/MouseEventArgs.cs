using System;
using CocoStudio.Model.ViewModel.HitTest;
using Gtk;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000125 RID: 293
	public class MouseEventArgs : EventArgs
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x0002B758 File Offset: 0x00029958
		// (set) Token: 0x06000B04 RID: 2820 RVA: 0x0002B76F File Offset: 0x0002996F
		public HitTestResult HitResult { get; private set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0002B778 File Offset: 0x00029978
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x0002B78F File Offset: 0x0002998F
		public PointF Point { get; private set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0002B798 File Offset: 0x00029998
		// (set) Token: 0x06000B08 RID: 2824 RVA: 0x0002B7AF File Offset: 0x000299AF
		public MouseButton Button { get; private set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x0002B7B8 File Offset: 0x000299B8
		// (set) Token: 0x06000B0A RID: 2826 RVA: 0x0002B7CF File Offset: 0x000299CF
		public bool Handled { get; set; }

		// Token: 0x06000B0B RID: 2827 RVA: 0x0002B7D8 File Offset: 0x000299D8
		public MouseEventArgs(PointF point, MouseButton button, HitTestResult result)
		{
			this.Point = point;
			this.Button = button;
			this.HitResult = result;
		}
	}
}
