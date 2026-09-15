using System;
using CocoStudio.Core.View;
using Gtk;
using Mono.Addins;
using MonoDevelop.Components.DockNotebook;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core
{
	public class DocumentWindow : SdiWorkspaceWindow, IDocumentWindow, IWorkbenchWindow
	{
		public DocumentWindow(MainWindow workbench, IViewContentExtend content, DockNotebook tabControl, DockNotebookTab tabLabel)
		{
			this.workbench = workbench;
			this.tabControl = tabControl;
			this.content = content;
			this.tab = tabLabel;
			this.tabPage = content.Control;
			this.fileTypeCondition.SetFileName(content.ContentName ?? content.UntitledName);
			this.extensionContext = AddinManager.CreateExtensionContext();
			this.extensionContext.RegisterCondition("FileType", this.fileTypeCondition);
			this.box = new VBox();
			content.WorkbenchWindow = this;
			content.ContentNameChanged += base.SetTitleEvent;
			content.DirtyChanged += base.HandleDirtyChanged;
			content.BeforeSave += base.BeforeSave;
			content.ContentChanged += base.OnContentChanged;
			this.box.Show();
			base.Add(this.box);
			base.SetTitleEvent(null, null);
		}

		private void AddContent(IViewContent content)
		{
			this.box.PackStart(content.Control, true, true, 1U);
		}

		protected override void OnClosed(WorkbenchWindowEventArgs e)
		{
			this.box.Remove(this.content.Control);
			this.ContentExtend.Closed();
			base.OnClosed(e);
		}

		protected override void OnTitleChanged(EventArgs e)
		{
			this.tab.Text = base.Title;
			this.tab.Notify = this.show_notification;
			this.tab.Dirty = this.content.IsDirty;
			if (this.tab.Dirty)
			{
				DockNotebookTab tab = this.tab;
				tab.Text += "*";
			}
			if (this.content.ContentName != null && this.content.ContentName != "")
			{
				this.tab.Tooltip = this.content.ContentName;
			}
		}

		public IViewContentExtend ContentExtend
		{
			get
			{
				return this.content as IViewContentExtend;
			}
		}

		internal new void OnActivated()
		{
			if (this.box.Children.Length == 0 && this.content.Control != null)
			{
				this.AddContent(this.content);
			}
			this.box.ShowAll();
			this.ContentExtend.Activated();
		}

		internal void AfterActivated()
		{
			this.ContentExtend.AfterActivated();
		}

		internal new void OnDeactivated()
		{
			this.ContentExtend.Deactivated();
			if (this.box.Children.Length > 0 && this.content.Control != null)
			{
				this.box.Remove(this.content.Control);
			}
		}
	}
}
