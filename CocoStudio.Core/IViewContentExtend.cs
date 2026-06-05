using System;
using CocoStudio.Core.View;
using CocoStudio.Projects;
using Gtk;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core
{
	// Token: 0x0200003E RID: 62
	public interface IViewContentExtend : IViewContent, IBaseViewContent, IDisposable
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000228 RID: 552
		IDocumentWindow DocumentWindow { get; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000229 RID: 553
		// (set) Token: 0x0600022A RID: 554
		CocosItem File { get; set; }

		// Token: 0x0600022B RID: 555
		void Activated();

		// Token: 0x0600022C RID: 556
		void AfterActivated();

		// Token: 0x0600022D RID: 557
		void Deactivated();

		// Token: 0x0600022E RID: 558
		void Closing();

		// Token: 0x0600022F RID: 559
		void Closed();

		// Token: 0x06000230 RID: 560
		void Reload();

		// Token: 0x06000231 RID: 561
		Widget GetToolbar();
	}
}
