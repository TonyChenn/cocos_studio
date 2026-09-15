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
	public class ViewContent : AbstractViewContent, IViewContentExtend, IViewContent, IBaseViewContent, IDisposable
	{
		public IDocumentWindow DocumentWindow
		{
			get
			{
				return this.WorkbenchWindow as IDocumentWindow;
			}
		}

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

		public bool IsActivated { get; private set; }

		public override bool IsFile
		{
			get
			{
				return false;
			}
		}

		protected ViewContent()
		{
		}

		public ViewContent(Widget widget)
		{
			this.widget = widget;
		}

		public override bool CanReuseView(string fileName)
		{
			return this.File.FileName.ToString().Equals(fileName);
		}

		public override void Load(string fileName)
		{
		}

		public override Widget Control
		{
			get
			{
				return this.widget;
			}
		}

		public override void Save(string fileName)
		{
			this.OnSave();
			this.IsDirty = false;
			this.OnSaved();
		}

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

		protected virtual void OnSaved()
		{
		}

		public virtual void Reload()
		{
		}

		public void Activated()
		{
			this.IsActivated = true;
			this.OnActivated();
		}

		protected virtual void OnActivated()
		{
		}

		public virtual void AfterActivated()
		{
		}

		public void Deactivated()
		{
			this.IsActivated = false;
			this.OnDeactivated();
		}

		protected virtual void OnDeactivated()
		{
		}

		public void Closing()
		{
			this.OnClosing();
		}

		protected virtual void OnClosing()
		{
		}

		public void Closed()
		{
			this.OnClosed();
		}

		protected virtual void OnClosed()
		{
		}

		public virtual Widget GetToolbar()
		{
			return null;
		}

		protected Widget widget;

		private CocosItem cocosItem;
	}
}
