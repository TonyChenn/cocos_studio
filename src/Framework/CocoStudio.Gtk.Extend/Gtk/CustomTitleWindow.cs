using System;
using CocoStudio.Basic;
using Gdk;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Gtk
{
	public class CustomTitleWindow : Window
	{
		public CustomTitleWindow() : base(WindowType.Toplevel)
		{
			this.Build();
			base.KeyPressEvent += this.HandleKeyPressed;
		}

		public void InitView(string title, Widget widget)
		{
			this.InitTitle(title);
			this.alignment_main.Add(widget);
			widget.Show();
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				this.CenterToParentWindow(ApplicationCurrent.MainWindow);
				this.RemoveWindowBorder();
				if (Platform.IsWindows)
				{
					this.evtbx_border.BorderWidth = 1U;
					base.ModifyBg(StateType.Normal, WindowStyle.LineDimColor);
				}
			}
			else
			{
				this.SetToDialogStyle(null, true, true, true);
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
			this.Destroy();
		}

		private void HandleKeyPressed(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Escape)
			{
				this.Destroy();
			}
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Gtk.CustomTitleWindow";
			base.Title = Catalog.GetString("CustomTitleWindow");
			base.TypeHint = WindowTypeHint.Dialog;
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.Resizable = false;
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
			base.Add(this.evtbx_border);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 356;
			base.DefaultHeight = 235;
			base.Hide();
		}

		private EventBox evtbx_border;

		private VBox vbox_window;

		private Alignment alignment_title;

		private Alignment alignment_main;
	}
}
