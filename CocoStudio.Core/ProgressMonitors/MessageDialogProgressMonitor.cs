using System;
using CocoStudio.ControlLib;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Ide;
using MonoDevelop.Ide.ProgressMonitoring;

namespace CocoStudio.Core.ProgressMonitors
{
	// Token: 0x0200002C RID: 44
	public class MessageDialogProgressMonitor : BaseProgressMonitor
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x00008388 File Offset: 0x00006588
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

		// Token: 0x060001A5 RID: 421 RVA: 0x00008458 File Offset: 0x00006658
		protected override void OnWriteLog(string text)
		{
			if (this.dialog != null)
			{
				this.dialog.WriteText(text);
				base.RunPendingEvents();
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000084CC File Offset: 0x000066CC
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

		// Token: 0x060001A7 RID: 423 RVA: 0x00008508 File Offset: 0x00006708
		public override void BeginTask(string name, int totalWork)
		{
			if (this.dialog != null)
			{
				this.dialog.BeginTask(name);
			}
			base.BeginTask(name, totalWork);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000853C File Offset: 0x0000673C
		public override void BeginStepTask(string name, int totalWork, int stepSize)
		{
			if (this.dialog != null)
			{
				this.dialog.BeginTask(name);
			}
			base.BeginStepTask(name, totalWork, stepSize);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00008570 File Offset: 0x00006770
		public override void EndTask()
		{
			if (this.dialog != null)
			{
				this.dialog.EndTask();
			}
			base.EndTask();
			base.RunPendingEvents();
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000085A8 File Offset: 0x000067A8
		public override void ReportWarning(string message)
		{
			base.ReportWarning(message);
			if (this.dialog != null)
			{
				this.dialog.WriteText(LanguageInfo.MessageBox_Notification + ": " + message + "\n");
				base.RunPendingEvents();
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000085F8 File Offset: 0x000067F8
		public override void ReportError(string message, Exception ex)
		{
			base.ReportError(message, ex);
			if (this.dialog != null)
			{
				this.dialog.WriteText(LanguageInfo.MessageBox_Error + ": " + base.Errors[base.Errors.Count - 1] + "\n");
				base.RunPendingEvents();
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000865D File Offset: 0x0000685D
		protected override void OnCompleted()
		{
			DispatchService.GuiDispatch(new MessageHandler(this.ShowDialogs));
			base.OnCompleted();
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000867C File Offset: 0x0000687C
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

		// Token: 0x060001AE RID: 430 RVA: 0x000086F0 File Offset: 0x000068F0
		public void CloseDialogs()
		{
			this.dialog.Destroy();
		}

		// Token: 0x040000EA RID: 234
		private ProgressDialog dialog;

		// Token: 0x040000EB RID: 235
		private bool hideWhenDone;

		// Token: 0x040000EC RID: 236
		private bool showDetails;
	}
}
