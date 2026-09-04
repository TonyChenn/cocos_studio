using System;
using CocoStudio.Basic;
using CocoStudio.Core.View;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core
{
	// Token: 0x02000052 RID: 82
	public class ViewContent : AbstractViewContent, IViewContentExtend, IViewContent, IBaseViewContent, IDisposable
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000E8D4 File Offset: 0x0000CAD4
		public IDocumentWindow DocumentWindow
		{
			get
			{
				return this.WorkbenchWindow as IDocumentWindow;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000E8F4 File Offset: 0x0000CAF4
		// (set) Token: 0x06000331 RID: 817 RVA: 0x0000E90C File Offset: 0x0000CB0C
		public virtual CocosItem File
		{
			get
			{
				return this.cocosItem;
			}
			set
			{
				this.cocosItem = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000E918 File Offset: 0x0000CB18
		// (set) Token: 0x06000333 RID: 819 RVA: 0x0000E92F File Offset: 0x0000CB2F
		public bool IsActivated { get; private set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000E938 File Offset: 0x0000CB38
		public override bool IsFile
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000E94B File Offset: 0x0000CB4B
		protected ViewContent()
		{
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000E956 File Offset: 0x0000CB56
		public ViewContent(Widget widget)
		{
			this.widget = widget;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000E968 File Offset: 0x0000CB68
		public override bool CanReuseView(string fileName)
		{
			return this.File.FileName.ToString().Equals(fileName);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000E999 File Offset: 0x0000CB99
		public override void Load(string fileName)
		{
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000E99C File Offset: 0x0000CB9C
		public override Widget Control
		{
			get
			{
				return this.widget;
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000E9B4 File Offset: 0x0000CBB4
		public override void Save(string fileName)
		{
			this.OnSave();
			this.IsDirty = false;
			this.OnSaved();
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000E9D0 File Offset: 0x0000CBD0
		protected virtual void OnSave()
		{
			if (this.File != null)
			{
				IProgressMonitor consoleProgressMonitor = Services.ProgressMonitors.GetConsoleProgressMonitor(false, true);
				this.File.Save(consoleProgressMonitor);
				string message = LanguageInfo.Output_Saved;
				if (!consoleProgressMonitor.AsyncOperation.Success)
				{
					message = LanguageInfo.Output_FailedToSaveFile;
				}
				LogConfig.Output.Info(message, true);
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000EA2F File Offset: 0x0000CC2F
		protected virtual void OnSaved()
		{
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000EA32 File Offset: 0x0000CC32
		public virtual void Reload()
		{
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000EA35 File Offset: 0x0000CC35
		public void Activated()
		{
			this.IsActivated = true;
			this.OnActivated();
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000EA47 File Offset: 0x0000CC47
		protected virtual void OnActivated()
		{
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000EA4A File Offset: 0x0000CC4A
		public virtual void AfterActivated()
		{
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000EA4D File Offset: 0x0000CC4D
		public void Deactivated()
		{
			this.IsActivated = false;
			this.OnDeactivated();
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000EA5F File Offset: 0x0000CC5F
		protected virtual void OnDeactivated()
		{
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000EA62 File Offset: 0x0000CC62
		public void Closing()
		{
			this.OnClosing();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000EA6C File Offset: 0x0000CC6C
		protected virtual void OnClosing()
		{
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000EA6F File Offset: 0x0000CC6F
		public void Closed()
		{
			this.OnClosed();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000EA79 File Offset: 0x0000CC79
		protected virtual void OnClosed()
		{
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000EA7C File Offset: 0x0000CC7C
		public virtual Widget GetToolbar()
		{
			return null;
		}

		// Token: 0x0400016D RID: 365
		protected Widget widget;

		// Token: 0x0400016E RID: 366
		private CocosItem cocosItem;
	}
}
