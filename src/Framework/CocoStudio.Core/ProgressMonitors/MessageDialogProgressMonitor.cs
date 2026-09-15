using System;
using CocoStudio.ControlLib;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Ide;
using MonoDevelop.Ide.ProgressMonitoring;

namespace CocoStudio.Core.ProgressMonitors
{
	public class MessageDialogProgressMonitor : BaseProgressMonitor
	{
		public MessageDialogProgressMonitor(bool showProgress = true, bool allowCancel = true, bool showDetails = true, bool hideWhenDone = false)
		{
			if (showProgress)
			{
				this.dialog = new ProgressDialog(MessageService.RootWindow, allowCancel, showDetails);
				this.dialog.SetToDialogStyle(MessageService.RootWindow, true, true, true);
				this.dialog.Message = "";
				this.dialog.Show();
				this.dialog.AsyncOperation = base.AsyncOperation;
				this.dialog.OperationCancelled += delegate(object param0, EventArgs param1)
				{
					this.OnCancelRequested();
				};
				this.dialog.DeleteEvent += delegate(object param0, DeleteEventArgs param1)
				{
					this.OnCancelRequested();
				};
				base.RunPendingEvents();
				this.hideWhenDone = hideWhenDone;
				this.showDetails = showDetails;
			}
		}

		protected override void OnWriteLog(string text)
		{
			if (this.dialog != null)
			{
				this.dialog.WriteText(text);
				base.RunPendingEvents();
			}
		}

		protected override void OnProgressChanged()
		{
			if (this.dialog != null)
			{
				GLib.Timeout.Add(0U, delegate
				{
					this.dialog.Message = base.CurrentTask;
					this.dialog.Progress = base.GlobalWork;
					base.RunPendingEvents();
					return false;
				});
			}
		}

		public override void BeginTask(string name, int totalWork)
		{
			if (this.dialog != null)
			{
				this.dialog.BeginTask(name);
			}
			base.BeginTask(name, totalWork);
		}

		public override void BeginStepTask(string name, int totalWork, int stepSize)
		{
			if (this.dialog != null)
			{
				this.dialog.BeginTask(name);
			}
			base.BeginStepTask(name, totalWork, stepSize);
		}

		public override void EndTask()
		{
			if (this.dialog != null)
			{
				this.dialog.EndTask();
			}
			base.EndTask();
			base.RunPendingEvents();
		}

		public override void ReportWarning(string message)
		{
			base.ReportWarning(message);
			if (this.dialog != null)
			{
				this.dialog.WriteText(LanguageInfo.MessageBox_Notification + ": " + message + "\n");
				base.RunPendingEvents();
			}
		}

		public override void ReportError(string message, Exception ex)
		{
			base.ReportError(message, ex);
			if (this.dialog != null)
			{
				this.dialog.WriteText(LanguageInfo.MessageBox_Error + ": " + base.Errors[base.Errors.Count - 1] + "\n");
				base.RunPendingEvents();
			}
		}

		protected override void OnCompleted()
		{
			DispatchService.GuiDispatch(new MessageHandler(this.ShowDialogs));
			base.OnCompleted();
		}

		private void ShowDialogs()
		{
			if (this.dialog != null)
			{
				this.dialog.ShowDone(base.Warnings.Count > 0, base.Errors.Count > 0);
				if (this.hideWhenDone)
				{
					this.dialog.Destroy();
				}
			}
			if (!this.showDetails)
			{
				base.ShowResultDialog();
			}
		}

		public void CloseDialogs()
		{
			this.dialog.Destroy();
		}

		private ProgressDialog dialog;

		private bool hideWhenDone;

		private bool showDetails;
	}
}
