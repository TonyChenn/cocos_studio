using System;
using CocoStudio.Core.View;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core
{
	// Token: 0x0200003C RID: 60
	public class DocumentExtend : Document, IEditableDocument
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000A09C File Offset: 0x0000829C
		public CocosItem File
		{
			get
			{
				return (this.DocumentWindow == null) ? null : this.DocumentWindow.ContentExtend.File;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000A0CC File Offset: 0x000082CC
		private DocumentWindow DocumentWindow
		{
			get
			{
				return this.window as DocumentWindow;
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000A0E9 File Offset: 0x000082E9
		public DocumentExtend(IDocumentWindow window)
		{
			this.window = window;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A0FC File Offset: 0x000082FC
		internal void SetFile(CocosItem newFile)
		{
			if (this.File != null)
			{
				this.File.NameChanged -= this.OnFileNameChanged;
				this.File.ContentChanged -= this.OnFileContentChanged;
			}
			if (this.DocumentWindow != null && this.DocumentWindow.ContentExtend != null)
			{
				this.DocumentWindow.ContentExtend.File = newFile;
				if (newFile != null)
				{
					newFile.NameChanged += this.OnFileNameChanged;
					newFile.ContentChanged += this.OnFileContentChanged;
				}
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000A1B0 File Offset: 0x000083B0
		private void OnFileContentChanged(object sender, EventArgs e)
		{
			if (this.DocumentWindow != null && this.DocumentWindow.ContentExtend != null)
			{
				this.DocumentWindow.ViewContent.ContentName = this.File.Name;
				this.DocumentWindow.ContentExtend.Reload();
				Services.Workbench.DocumentReloaded(this);
				Services.TaskService.Clear(this);
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000A228 File Offset: 0x00008428
		private void OnFileNameChanged(object sender, EventArgs e)
		{
			if (this.File != null)
			{
				base.Window.ViewContent.ContentName = this.File.Name;
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000A25F File Offset: 0x0000845F
		internal override void DisposeDocument()
		{
			base.DisposeDocument();
			this.SetFile(null);
			GC.Collect();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000A278 File Offset: 0x00008478
		public Widget GetToolbar()
		{
			return this.DocumentWindow.ContentExtend.GetToolbar();
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000A29C File Offset: 0x0000849C
		public bool Close(bool force)
		{
			return ((SdiWorkspaceWindow)base.Window).CloseWindow(force, true);
		}
	}
}
