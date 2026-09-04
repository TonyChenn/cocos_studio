using System;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core.View
{
	// Token: 0x0200004B RID: 75
	public interface IDocumentWindow : IWorkbenchWindow
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002A5 RID: 677
		IViewContentExtend ContentExtend { get; }
	}
}
