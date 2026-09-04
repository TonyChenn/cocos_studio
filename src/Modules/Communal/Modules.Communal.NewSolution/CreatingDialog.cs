using System;
using AppKit;
using CocoStudio.Basic;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Stetic;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000022 RID: 34
	public class CreatingDialog : Dialog
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00008494 File Offset: 0x00006694
		public CreatingDialog()
		{
			this.Build();
			this.parentWnd = MessageService.GetDefaultModalParent();
			this.isParentWndModal = this.parentWnd.Modal;
			this.parentWnd.Modal = false;
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				this.CenterToParentWindow(this.parentWnd);
				base.TransientFor = this.parentWnd;
				if (Platform.IsMac)
				{
					NSWindowStyle style = NSWindowStyle.Titled | NSWindowStyle.DocModal;
					if (base.GdkWindow == null)
					{
						base.Show();
					}
					NativeGdkMac.SetNSWindowStyle(base.GdkWindow, style);
				}
				else if (Platform.IsWindows)
				{
					base.Decorated = false;
					this.alignment_base.BorderWidth = 1U;
					base.ModifyBg(StateType.Normal, NewSolutionStyles.Launcher_WindowBorder);
					base.VBox.BorderWidth = 0U;
				}
			}
			else
			{
				this.SetToDialogStyle(this.parentWnd, false, true, true);
				base.Title = LanguageInfo.Menu_File_NewProject;
			}
			this.label_title.Text = LanguageInfo.NewSolution_NowCreating;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00008598 File Offset: 0x00006798
		public void StartRunning(CocosMonitor monitor)
		{
			GLib.Timeout.Add(20U, () => this.RefreshUI(monitor));
			base.Run();
			this.CloseWindow();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000085DC File Offset: 0x000067DC
		private bool RefreshUI(CocosMonitor monitor)
		{
			if (!monitor.HasStarted)
			{
				return true;
			}
			if (this.isDestroyed)
			{
				return false;
			}
			if (monitor.IsProcessing)
			{
				this.progressbar_main.Pulse();
				return true;
			}
			this.progressbar_main.Fraction = 1.0;
			base.Respond(-5);
			return false;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000862F File Offset: 0x0000682F
		public void CloseWindow()
		{
			this.isDestroyed = true;
			this.parentWnd.Modal = this.isParentWndModal;
			this.Destroy();
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00008650 File Offset: 0x00006850
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.WidthRequest = 360;
			base.Name = "Modules.Communal.NewSolution.CreatingDialog";
			base.Title = Catalog.GetString("新建项目");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			this.alignment_base = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_base.Name = "alignment_base";
			this.vbox_window = new VBox();
			this.vbox_window.Name = "vbox_window";
			this.evtbx_main = new EventBox();
			this.evtbx_main.HeightRequest = 90;
			this.evtbx_main.Name = "evtbx_main";
			this.vbox_main = new VBox();
			this.vbox_main.Name = "vbox_main";
			this.vbox_main.Spacing = 12;
			this.vbox_main.BorderWidth = 15U;
			this.alignment_title = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_title.Name = "alignment_title";
			this.alignment_title.TopPadding = 5U;
			this.label_title = new Label();
			this.label_title.Name = "label_title";
			this.label_title.Xalign = 0f;
			this.label_title.LabelProp = Catalog.GetString("正在创建新项目，请稍后");
			this.alignment_title.Add(this.label_title);
			this.vbox_main.Add(this.alignment_title);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_main[this.alignment_title];
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
			this.evtbx_main.Add(this.vbox_main);
			this.vbox_window.Add(this.evtbx_main);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox_window[this.evtbx_main];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.alignment_base.Add(this.vbox_window);
			vbox.Add(this.alignment_base);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox[this.alignment_base];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
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
			base.DefaultWidth = 360;
			base.DefaultHeight = 114;
			actionArea.Hide();
			base.Hide();
		}

		// Token: 0x040000C6 RID: 198
		private Gtk.Window parentWnd;

		// Token: 0x040000C7 RID: 199
		private bool isParentWndModal;

		// Token: 0x040000C8 RID: 200
		private bool isDestroyed;

		// Token: 0x040000C9 RID: 201
		private Alignment alignment_base;

		// Token: 0x040000CA RID: 202
		private VBox vbox_window;

		// Token: 0x040000CB RID: 203
		private EventBox evtbx_main;

		// Token: 0x040000CC RID: 204
		private VBox vbox_main;

		// Token: 0x040000CD RID: 205
		private Alignment alignment_title;

		// Token: 0x040000CE RID: 206
		private Label label_title;

		// Token: 0x040000CF RID: 207
		private ProgressBar progressbar_main;

		// Token: 0x040000D0 RID: 208
		private Alignment alignment_dummyBtn;
	}
}
