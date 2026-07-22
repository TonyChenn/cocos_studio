using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Gtk;
using Mono.Addins;

namespace CocoStudio.Model.Interface
{
	// Token: 0x020000AD RID: 173
	[TypeExtensionPoint]
	public interface IDragOperation
	{
		// Token: 0x06000591 RID: 1425
		void DragEnter(DragMotionArgs e, VisualObject target);

		// Token: 0x06000592 RID: 1426
		void DragLeave(DragMotionArgs e, VisualObject target);

		// Token: 0x06000593 RID: 1427
		bool DragOver(DragMotionArgs e, VisualObject target);

		// Token: 0x06000594 RID: 1428
		void DragDrop(DragDropArgs e, VisualObject target);

		// Token: 0x06000595 RID: 1429
		bool CanHandle(CocosItem project);
	}
}
