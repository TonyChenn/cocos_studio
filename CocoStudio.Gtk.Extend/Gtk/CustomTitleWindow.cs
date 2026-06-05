using System;
using CocoStudio.Basic;
using Gdk;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Gtk
{
	// Token: 0x0200009D RID: 157
	public class CustomTitleWindow : Window
	{
		// Token: 0x06000362 RID: 866 RVA: 0x0000F59A File Offset: 0x0000D79A
		public CustomTitleWindow() : base(WindowType.Toplevel)
		{
			this.Build();
			base.KeyPressEvent += this.HandleKeyPressed;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
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

		// Token: 0x06000364 RID: 868 RVA: 0x0000F648 File Offset: 0x0000D848
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

		// Token: 0x06000365 RID: 869 RVA: 0x0000F6B5 File Offset: 0x0000D8B5
		private void HandleCustomTitleBarCloseClicked(object sender, EventArgs args)
		{
			this.Destroy();
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000F6C0 File Offset: 0x0000D8C0
		private void HandleKeyPressed(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Key.Escape)
			{
				this.Destroy();
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000F6F0 File Offset: 0x0000D8F0
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

		// Token: 0x040003FD RID: 1021
		private EventBox evtbx_border;

		// Token: 0x040003FE RID: 1022
		private VBox vbox_window;

		// Token: 0x040003FF RID: 1023
		private Alignment alignment_title;

		// Token: 0x04000400 RID: 1024
		private Alignment alignment_main;
	}
}
