using System;
using System.Collections.Generic;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;

namespace Modules.Communal.Render.ExtensionModel
{
	// Token: 0x0200001B RID: 27
	public class ContextMenuShowingArgs : EventArgs
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00006E64 File Offset: 0x00005064
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00006E7B File Offset: 0x0000507B
		public bool Enable { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00006E84 File Offset: 0x00005084
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00006E9B File Offset: 0x0000509B
		public IEnumerable<VisualObject> SelectedParentObject { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00006EA4 File Offset: 0x000050A4
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00006EBB File Offset: 0x000050BB
		public PointF ClickPoint { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00006EC4 File Offset: 0x000050C4
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00006EDB File Offset: 0x000050DB
		public bool IsShowOnRenderArea { get; private set; }

		// Token: 0x060000FE RID: 254 RVA: 0x00006EE4 File Offset: 0x000050E4
		public ContextMenuShowingArgs()
		{
			this.Enable = true;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006EF7 File Offset: 0x000050F7
		public ContextMenuShowingArgs(IEnumerable<VisualObject> selectedParentObject, PointF clickPoint, bool isShowOnRenderArea) : this()
		{
			this.SelectedParentObject = selectedParentObject;
			this.ClickPoint = clickPoint;
			this.IsShowOnRenderArea = isShowOnRenderArea;
		}
	}
}
