using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;

namespace Modules.Communal.Render.ExtensionModel
{
	// Token: 0x0200001C RID: 28
	public class DragMenuShowingArgs
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00006F1C File Offset: 0x0000511C
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00006F33 File Offset: 0x00005133
		public bool Enable { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00006F3C File Offset: 0x0000513C
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00006F53 File Offset: 0x00005153
		public VisualObject DragObject { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00006F5C File Offset: 0x0000515C
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00006F73 File Offset: 0x00005173
		public ResourceItem DragResourceItem { get; set; }

		// Token: 0x06000106 RID: 262 RVA: 0x00006F7C File Offset: 0x0000517C
		public DragMenuShowingArgs(VisualObject dragObject, ResourceItem dragResourceItem)
		{
			this.Enable = true;
			this.DragObject = dragObject;
			this.DragResourceItem = dragResourceItem;
		}
	}
}
