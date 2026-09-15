using System;
using CocoStudio.Core.View;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core
{
	public class DocumentExtend : Document, IEditableDocument
	{
		public CocosItem File
		{
			get
			{
				return (this.DocumentWindow == null) ? null : this.DocumentWindow.ContentExtend.File;
			}
		}

		private DocumentWindow DocumentWindow
		{
			get
			{
				return this.window as DocumentWindow;
			}
		}

		public DocumentExtend(IDocumentWindow window)
		{
			this.window = window;
		}

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

		private void OnFileNameChanged(object sender, EventArgs e)
		{
			if (this.File != null)
			{
				base.Window.ViewContent.ContentName = this.File.Name;
			}
		}

		internal override void DisposeDocument()
		{
			base.DisposeDocument();
			this.SetFile(null);
			GC.Collect();
		}

		public Widget GetToolbar()
		{
			return this.DocumentWindow.ContentExtend.GetToolbar();
		}

		public bool Close(bool force)
		{
			return ((SdiWorkspaceWindow)base.Window).CloseWindow(force, true);
		}
	}
}
