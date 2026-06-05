using System;
using CocoStudio.Core.View;
using Gtk;
using Mono.Addins;
using MonoDevelop.Components.DockNotebook;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core
{
	// Token: 0x0200004C RID: 76
	public class DocumentWindow : SdiWorkspaceWindow, IDocumentWindow, IWorkbenchWindow
	{
		// Token: 0x060002A6 RID: 678 RVA: 0x0000B784 File Offset: 0x00009984
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

		// Token: 0x060002A7 RID: 679 RVA: 0x0000B882 File Offset: 0x00009A82
		private void AddContent(IViewContent content)
		{
			this.box.PackStart(content.Control, true, true, 1U);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000B89A File Offset: 0x00009A9A
		protected override void OnClosed(WorkbenchWindowEventArgs e)
		{
			this.box.Remove(this.content.Control);
			this.ContentExtend.Closed();
			base.OnClosed(e);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000B8C8 File Offset: 0x00009AC8
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000B988 File Offset: 0x00009B88
		public IViewContentExtend ContentExtend
		{
			get
			{
				return this.content as IViewContentExtend;
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000B9A8 File Offset: 0x00009BA8
		internal new void OnActivated()
		{
			if (this.box.Children.Length == 0 && this.content.Control != null)
			{
				this.AddContent(this.content);
			}
			this.box.ShowAll();
			this.ContentExtend.Activated();
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000BA02 File Offset: 0x00009C02
		internal void AfterActivated()
		{
			this.ContentExtend.AfterActivated();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000BA14 File Offset: 0x00009C14
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
