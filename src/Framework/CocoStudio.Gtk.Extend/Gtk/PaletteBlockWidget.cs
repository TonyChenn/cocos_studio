using System;
using System.ComponentModel;
using Gdk;
using Modules.Communal.MultiLanguage;
using Mono.TextEditor;
using Stetic;

namespace Gtk
{
	[ToolboxItem(true)]
	public class PaletteBlockWidget : Bin
	{
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

		private void DragDataReceivedHandler(object o, DragDataReceivedArgs args)
		{
			Color? currentColor = args.SelectionData.GetCurrentColor();
			if (currentColor != null)
			{
				this.Color = currentColor.Value;
			}
		}

		private void DragDropHandler(object o, DragDropArgs args)
		{
			Drag.GetData(this.eventbox_border, args.Context, null, args.Time);
		}

		private void FocusInEventHandler(object o, FocusInEventArgs args)
		{
			this.eventbox_border.BorderWidth = 1U;
		}

		private void FocusOutEventHandler(object o, FocusOutEventArgs args)
		{
			this.eventbox_border.BorderWidth = 0U;
		}

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

		private void MenuItemActivatedHandler(object sender, EventArgs e)
		{
			this.Color = this.colorSelection.CurrentColor;
		}

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

		private Color curColor;

		private ColorSelection colorSelection;

		private EventBox eventbox_bg;

		private EventBox eventbox_border;
	}
}
