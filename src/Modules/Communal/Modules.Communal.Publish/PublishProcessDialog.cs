using System;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Ide;
using Stetic;

namespace Modules.Communal.Publish
{
	// Token: 0x0200000C RID: 12
	public class PublishProcessDialog : Dialog
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00004FA3 File Offset: 0x000031A3
		public PublishProcessDialog()
		{
			this.Build();
			base.Title = LanguageInfo.Menu_Project_Publish;
			this.label_desc.Text = LanguageInfo.Run_NowRunning;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004FCC File Offset: 0x000031CC
		public void StartRunning(CocosMonitor monitor)
		{
			bool modal = false;
			Gtk.Window defaultModalParent = MessageService.GetDefaultModalParent();
			if (defaultModalParent != null)
			{
				modal = defaultModalParent.Modal;
				defaultModalParent.Modal = false;
			}
			this.SetToDialogStyle(defaultModalParent, false, true, true);
			GLib.Timeout.Add(20U, () => this.RefreshUI(monitor));
			base.Run();
			this.hasStopped = true;
			if (defaultModalParent != null)
			{
				defaultModalParent.Modal = modal;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000503C File Offset: 0x0000323C
		private bool RefreshUI(CocosMonitor monitor)
		{
			if (this.hasStopped)
			{
				return false;
			}
			if (monitor == null || !monitor.HasStarted)
			{
				return true;
			}
			if (monitor.IsProcessing)
			{
				this.progressbar_main.Pulse();
				return true;
			}
			this.progressbar_main.Fraction = 1.0;
			base.Respond(ResponseType.Close);
			return false;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00005094 File Offset: 0x00003294
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 300;
			base.Name = "Modules.Communal.Publish.PublishProcessDialog";
			base.Title = Catalog.GetString("发布项目");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.BorderWidth = 15U;
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 6;
			this.label_desc = new Label();
			this.label_desc.Name = "label_desc";
			this.label_desc.Xalign = 0f;
			this.label_desc.LabelProp = Catalog.GetString("正在进行发布");
			this.vbox_main.Add(this.label_desc);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.label_desc];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.progressbar_main = new ProgressBar();
			this.progressbar_main.Name = "progressbar_main";
			this.progressbar_main.PulseStep = 0.02;
			this.vbox_main.Add(this.progressbar_main);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_main[this.progressbar_main];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.alignment_main.Add(this.vbox_main);
			vbox.Add(this.alignment_main);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox[this.alignment_main];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog_ActionArea";
			actionArea.Spacing = 10;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.alignment_dummyBtn = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_dummyBtn.Name = "alignment_dummyBtn";
			actionArea.Add(this.alignment_dummyBtn);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.alignment_dummyBtn];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 300;
			base.DefaultHeight = 93;
			actionArea.Hide();
			base.Hide();
		}

		// Token: 0x0400003F RID: 63
		private bool hasStopped;

		// Token: 0x04000040 RID: 64
		private Alignment alignment_main;

		// Token: 0x04000041 RID: 65
		private VBox vbox_main;

		// Token: 0x04000042 RID: 66
		private Label label_desc;

		// Token: 0x04000043 RID: 67
		private ProgressBar progressbar_main;

		// Token: 0x04000044 RID: 68
		private Alignment alignment_dummyBtn;
	}
}
