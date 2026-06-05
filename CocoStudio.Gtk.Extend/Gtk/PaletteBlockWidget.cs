using System;
using System.ComponentModel;
using Gdk;
using Modules.Communal.MultiLanguage;
using Mono.TextEditor;
using Stetic;

namespace Gtk
{
	// Token: 0x0200009F RID: 159
	[ToolboxItem(true)]
	public class PaletteBlockWidget : Bin
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00010448 File Offset: 0x0000E648
		// (set) Token: 0x06000372 RID: 882 RVA: 0x00010460 File Offset: 0x0000E660
		public Color Color
		{
			get
			{
				return this.curColor;
			}
			set
			{
				this.curColor = value;
				this.eventbox_border.ModifyBg(StateType.Normal, value);
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00010478 File Offset: 0x0000E678
		public PaletteBlockWidget(Color color, ColorSelection selection)
		{
			this.Build();
			this.eventbox_bg.ModifyBg(StateType.Normal, new Color(0, 0, 0));
			this.Color = color;
			this.colorSelection = selection;
			base.CanFocus = true;
			base.FocusInEvent += this.FocusInEventHandler;
			base.FocusOutEvent += this.FocusOutEventHandler;
			TargetEntry[] targets = new TargetEntry[]
			{
				DragTargetType.ColorDropTarget
			};
			Drag.DestSet(this.eventbox_border, DestDefaults.All, targets, DragAction.Copy | DragAction.Move | DragAction.Link);
			this.eventbox_border.DragDrop += this.DragDropHandler;
			this.eventbox_border.DragDataReceived += this.DragDataReceivedHandler;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00010544 File Offset: 0x0000E744
		private void DragDataReceivedHandler(object o, DragDataReceivedArgs args)
		{
			Color? currentColor = args.SelectionData.GetCurrentColor();
			if (currentColor != null)
			{
				this.Color = currentColor.Value;
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001057A File Offset: 0x0000E77A
		private void DragDropHandler(object o, DragDropArgs args)
		{
			Drag.GetData(this.eventbox_border, args.Context, null, args.Time);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00010596 File Offset: 0x0000E796
		private void FocusInEventHandler(object o, FocusInEventArgs args)
		{
			this.eventbox_border.BorderWidth = 1U;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000105A6 File Offset: 0x0000E7A6
		private void FocusOutEventHandler(object o, FocusOutEventArgs args)
		{
			this.eventbox_border.BorderWidth = 0U;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000105B8 File Offset: 0x0000E7B8
		protected void ButtonReleaseHandler(object o, ButtonReleaseEventArgs args)
		{
			base.HasFocus = true;
			if (args.Event.Button == 1U)
			{
				this.colorSelection.CurrentColor = this.Color;
			}
			else if (args.Event.Button == 3U)
			{
				Menu menu = new Menu();
				MenuItem menuItem = new MenuItem(LanguageInfo.Property_SaveColorHere);
				menuItem.Activated += this.MenuItemActivatedHandler;
				menuItem.Show();
				menu.Append(menuItem);
				GtkWorkarounds.ShowContextMenu(menu, this, args.Event);
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00010651 File Offset: 0x0000E851
		private void MenuItemActivatedHandler(object sender, EventArgs e)
		{
			this.Color = this.colorSelection.CurrentColor;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00010668 File Offset: 0x0000E868
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.WidthRequest = 29;
			base.HeightRequest = 20;
			base.Name = "Gtk.PaletteBlockWidget";
			this.eventbox_bg = new EventBox();
			this.eventbox_bg.Name = "eventbox_bg";
			this.eventbox_border = new EventBox();
			this.eventbox_border.Name = "eventbox_border";
			this.eventbox_bg.Add(this.eventbox_border);
			base.Add(this.eventbox_bg);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.eventbox_bg.ButtonReleaseEvent += this.ButtonReleaseHandler;
		}

		// Token: 0x04000419 RID: 1049
		private Color curColor;

		// Token: 0x0400041A RID: 1050
		private ColorSelection colorSelection;

		// Token: 0x0400041B RID: 1051
		private EventBox eventbox_bg;

		// Token: 0x0400041C RID: 1052
		private EventBox eventbox_border;
	}
}
