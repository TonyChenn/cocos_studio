using System;
using CocoStudio.Basic;
using Gdk;
using MonoDevelop.Core;
using Stetic;

namespace Gtk
{
	public class CustomTitleDialog : Dialog
	{
		public CustomTitleDialog()
		{
			this.Build();
		}

		public void InitView(string title, Widget widget, Window parentWnd = null)
		{
			this.InitTitle(title);
			this.alignment_main.Add(widget);
			widget.Show();
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				if (parentWnd == null)
				{
					parentWnd = ApplicationCurrent.MainWindow;
				}
				this.CenterToParentWindow(parentWnd);
				base.TransientFor = parentWnd;
				this.RemoveWindowBorder();
				if (Platform.IsWindows)
				{
					this.evtbx_border.BorderWidth = 1U;
					base.ModifyBg(StateType.Normal, WindowStyle.LineDimColor);
				}
			}
			else
			{
				this.SetToDialogStyle(parentWnd, true, true, true);
			}
		}

		private void InitTitle(string title)
		{
			base.Title = title;
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				CustomTitleBar customTitleBar = new CustomTitleBar();
				customTitleBar.Title = title;
				customTitleBar.HeightRequest = 26;
				customTitleBar.SetParentWindow(this);
				customTitleBar.CloseClicked += this.HandleCustomTitleBarCloseClicked;
				this.alignment_title.Add(customTitleBar);
				customTitleBar.Show();
			}
		}

		private void HandleCustomTitleBarCloseClicked(object sender, EventArgs args)
		{
			base.Respond(ResponseType.Close);
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Gtk.CustomTitleDialog";
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
			VBox vbox = base.VBox;
			vbox.Name = "dialog_VBox";
			this.evtbx_border = new EventBox();
			this.evtbx_border.Name = "evtbx_border";
			this.vbox_window = new VBox();
			this.vbox_window.Name = "vbox_window";
			this.alignment_title = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_title.Name = "alignment_title";
			this.vbox_window.Add(this.alignment_title);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox_window[this.alignment_title];
			boxChild.Position = 0;
			boxChild.Expand = false;
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.vbox_window.Add(this.alignment_main);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox_window[this.alignment_main];
			boxChild2.Position = 1;
			this.evtbx_border.Add(this.vbox_window);
			vbox.Add(this.evtbx_border);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox[this.evtbx_border];
			boxChild3.Position = 0;
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
			base.DefaultWidth = 371;
			base.DefaultHeight = 278;
			actionArea.Hide();
			base.Hide();
		}

		private EventBox evtbx_border;

		private VBox vbox_window;

		private Alignment alignment_title;

		private Alignment alignment_main;

		private Alignment alignment_dummyBtn;
	}
}
