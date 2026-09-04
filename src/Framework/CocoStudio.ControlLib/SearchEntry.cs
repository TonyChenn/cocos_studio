using System;
using System.ComponentModel;
using Cairo;
using Gdk;
using GLib;
using Gtk;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using Pango;
using Xwt.GtkBackend;

namespace CocoStudio.ControlLib
{
	// Token: 0x0200000E RID: 14
	[ToolboxItem(true)]
	public class SearchEntry : EventBox
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000069 RID: 105 RVA: 0x00005734 File Offset: 0x00003934
		// (remove) Token: 0x0600006A RID: 106 RVA: 0x00005770 File Offset: 0x00003970
		private event EventHandler filter_changed;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600006B RID: 107 RVA: 0x000057AC File Offset: 0x000039AC
		// (remove) Token: 0x0600006C RID: 108 RVA: 0x000057E8 File Offset: 0x000039E8
		private event EventHandler entry_changed;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600006D RID: 109 RVA: 0x00005824 File Offset: 0x00003A24
		// (remove) Token: 0x0600006E RID: 110 RVA: 0x0000582F File Offset: 0x00003A2F
		public event EventHandler Changed
		{
			add
			{
				this.entry_changed += value;
			}
			remove
			{
				this.entry_changed -= value;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600006F RID: 111 RVA: 0x0000583A File Offset: 0x00003A3A
		// (remove) Token: 0x06000070 RID: 112 RVA: 0x00005854 File Offset: 0x00003A54
		public event EventHandler Activated
		{
			add
			{
				this.activated_event = (EventHandler)Delegate.Combine(this.activated_event, value);
			}
			remove
			{
				this.activated_event = (EventHandler)Delegate.Remove(this.activated_event, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000071 RID: 113 RVA: 0x0000586E File Offset: 0x00003A6E
		// (remove) Token: 0x06000072 RID: 114 RVA: 0x00005879 File Offset: 0x00003A79
		public event EventHandler FilterChanged
		{
			add
			{
				this.filter_changed += value;
			}
			remove
			{
				this.filter_changed -= value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00005884 File Offset: 0x00003A84
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000589C File Offset: 0x00003A9C
		public bool ForceFilterButtonVisible
		{
			get
			{
				return this.forceFilterButtonVisible;
			}
			set
			{
				this.forceFilterButtonVisible = value;
				this.ShowHideButtons();
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000058B0 File Offset: 0x00003AB0
		// (set) Token: 0x06000076 RID: 118 RVA: 0x000058C8 File Offset: 0x00003AC8
		public Menu Menu
		{
			get
			{
				return this.menu;
			}
			set
			{
				this.menu = value;
				this.menu.Deactivated += this.OnMenuDeactivated;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000058EC File Offset: 0x00003AEC
		public Entry Entry
		{
			get
			{
				return this.entry;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00005904 File Offset: 0x00003B04
		// (set) Token: 0x06000079 RID: 121 RVA: 0x0000591C File Offset: 0x00003B1C
		public bool HasFrame
		{
			get
			{
				return this.hasFrame;
			}
			set
			{
				this.hasFrame = value;
				base.QueueDraw();
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00005930 File Offset: 0x00003B30
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00005948 File Offset: 0x00003B48
		public bool RoundedShape
		{
			get
			{
				return this.roundedShape;
			}
			set
			{
				this.roundedShape = value;
				if (value)
				{
					this.entry.Name = "search-entry";
				}
				else
				{
					this.entry.Name = "";
				}
				this.ShowHideButtons();
				base.QueueDraw();
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00005998 File Offset: 0x00003B98
		public SearchEntry()
		{
			base.AppPaintable = true;
			this.BuildWidget();
			this.BuildMenu();
			base.NoShowAll = true;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00005A08 File Offset: 0x00003C08
		private void BuildWidget()
		{
			float yscale = 0f;
			if (MonoDevelop.Core.Platform.IsWindows)
			{
				yscale = (float)GtkWorkarounds.GetScaleFactor(this);
			}
			this.alignment = new Gtk.Alignment(0.5f, 0.5f, 1f, yscale);
			this.alignment.SetPadding(1U, 1U, 0U, 0U);
			base.VisibleWindow = false;
			this.box = new HBox();
			this.entry = new SearchEntry.FramelessEntry(this);
			this.filter_button = new IconButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.ResourcePanelResource.search.png"));
			this.clear_button = new IconButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.ResourcePanelResource.Close.png"));
			this.filter_button.SetSizeRequest(20, 20);
			this.clear_button.SetSizeRequest(20, 20);
			this.entryAlignment = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment.SetPadding(0U, 0U, 0U, 0U);
			this.entryAlignment.Add(this.entry);
			EventBox eventBox = new EventBox();
			EventBox eventBox2 = new EventBox();
			eventBox.Add(this.filter_button);
			eventBox2.Add(this.clear_button);
			eventBox.SetNormalBg(new Gdk.Color(50, 50, 54));
			eventBox2.SetNormalBg(new Gdk.Color(50, 50, 54));
			this.box.PackStart(eventBox, false, false, 0U);
			this.box.PackStart(this.entryAlignment, true, true, 0U);
			this.box.PackStart(eventBox2, false, false, 0U);
			this.alignment.Add(this.box);
			base.Add(this.alignment);
			this.alignment.ShowAll();
			this.entry.StyleSet += this.OnInnerEntryStyleSet;
			this.entry.StateChanged += new StateChangedHandler(this.OnInnerEntryStateChanged);
			this.entry.FocusInEvent += new FocusInEventHandler(this.OnInnerEntryFocusEvent);
			this.entry.FocusOutEvent += new FocusOutEventHandler(this.OnInnerEntryFocusEvent);
			this.entry.Changed += this.OnInnerEntryChanged;
			this.entry.Activated += delegate(object param0, EventArgs param1)
			{
				this.NotifyActivated();
			};
			this.filter_button.CanFocus = false;
			this.clear_button.CanFocus = false;
			this.filter_button.ButtonReleaseEvent += this.OnButtonReleaseEvent;
			this.clear_button.ButtonReleaseEvent += this.OnButtonReleaseEvent;
			this.clear_button.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.OnClearButtonClicked);
			this.ShowHideButtons();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005CA8 File Offset: 0x00003EA8
		protected override void OnSizeRequested(ref Requisition requisition)
		{
			if (base.HeightRequest != -1 && this.box.HeightRequest != base.HeightRequest)
			{
				this.box.HeightRequest = base.HeightRequest;
			}
			if (this.box.HeightRequest != -1 && base.HeightRequest == -1)
			{
				this.box.HeightRequest = -1;
			}
			base.OnSizeRequested(ref requisition);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005D24 File Offset: 0x00003F24
		public EventBox AddLabelWidget(Label label)
		{
			this.box.Remove(this.clear_button);
			this.statusLabelEventBox = new EventBox();
			this.statusLabelEventBox.Child = label;
			this.box.PackStart(this.statusLabelEventBox, false, false, 0U);
			this.box.PackStart(this.clear_button, false, false, 0U);
			this.UpdateStyle();
			this.box.ShowAll();
			return this.statusLabelEventBox;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005DA4 File Offset: 0x00003FA4
		private void NotifyActivated()
		{
			if (this.activated_event != null)
			{
				this.activated_event(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005DD1 File Offset: 0x00003FD1
		private void BuildMenu()
		{
			this.menu = new Menu();
			this.menu.Deactivated += this.OnMenuDeactivated;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005DF7 File Offset: 0x00003FF7
		public void PopupFilterMenu()
		{
			this.ShowMenu(0U);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005E04 File Offset: 0x00004004
		private void ShowMenu(uint time)
		{
			this.OnRequestMenu(EventArgs.Empty);
			if (this.menu.Children.Length > 0)
			{
				this.menu.Popup(null, null, new MenuPositionFunc(this.OnPositionMenu), 0U, time);
				this.menu.ShowAll();
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005E60 File Offset: 0x00004060
		private void ShowHideButtons()
		{
			this.clear_button.Visible = (this.entry.Text.Length > 0);
			this.entryAlignment.RightPadding = ((!this.clear_button.Visible && this.roundedShape) ? 6U : 0U);
			this.filter_button.Visible = (this.ForceFilterButtonVisible || (this.menu != null && this.menu.Children.Length > 0));
			this.entryAlignment.LeftPadding = ((!this.filter_button.Visible && this.roundedShape) ? 6U : 0U);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005F10 File Offset: 0x00004110
		private void OnPositionMenu(Menu menu, out int x, out int y, out bool push_in)
		{
			int num;
			int num2;
			this.filter_button.GdkWindow.GetOrigin(out num, out num2);
			int num3;
			base.GdkWindow.GetOrigin(out num2, out num3);
			x = num + this.filter_button.Allocation.X;
			y = num3 + base.Allocation.Y + base.SizeRequest().Height;
			push_in = true;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005F76 File Offset: 0x00004176
		private void OnMenuDeactivated(object o, EventArgs args)
		{
			this.filter_button.QueueDraw();
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00005F88 File Offset: 0x00004188
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00005F9F File Offset: 0x0000419F
		public bool IsCheckMenu { get; set; }

		// Token: 0x06000089 RID: 137 RVA: 0x00005FA8 File Offset: 0x000041A8
		private void OnMenuItemToggled(object o, EventArgs args)
		{
			if (!this.IsCheckMenu && !this.toggling && o is SearchEntry.FilterMenuItem)
			{
				this.toggling = true;
				SearchEntry.FilterMenuItem filterMenuItem = (SearchEntry.FilterMenuItem)o;
				foreach (object obj in this.menu)
				{
					MenuItem menuItem = (MenuItem)obj;
					if (menuItem is SearchEntry.FilterMenuItem)
					{
						SearchEntry.FilterMenuItem filterMenuItem2 = (SearchEntry.FilterMenuItem)menuItem;
						if (filterMenuItem2 != filterMenuItem)
						{
							filterMenuItem2.Active = false;
						}
					}
				}
				filterMenuItem.Active = true;
				this.ActiveFilterID = filterMenuItem.ID;
				this.toggling = false;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00006088 File Offset: 0x00004288
		private void OnInnerEntryChanged(object o, EventArgs args)
		{
			this.ShowHideButtons();
			if (this.changed_timeout_id > 0U)
			{
				Source.Remove(this.changed_timeout_id);
			}
			if (this.Ready)
			{
				this.changed_timeout_id = GLib.Timeout.Add(25U, new TimeoutHandler(this.OnChangedTimeout));
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000060E4 File Offset: 0x000042E4
		private bool OnChangedTimeout()
		{
			this.OnChanged();
			return false;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00006100 File Offset: 0x00004300
		private void UpdateStyle()
		{
			Gdk.Color color = this.entry.Style.Base(this.entry.State);
			if (this.statusLabelEventBox != null)
			{
				this.statusLabelEventBox.ModifyBg(this.entry.State, color);
			}
			this.box.BorderWidth = 0U;
			int num = this.entry.SizeRequest().Height + this.entry.Style.Ythickness * 2;
			int num2 = this.entry.SizeRequest().Height;
			num2 = Math.Max(num2, this.filter_button.SizeRequest().Height);
			num2 = Math.Max(num2, this.clear_button.SizeRequest().Height);
			int num3 = num - num2;
			if (num3 > 1)
			{
				this.box.BorderWidth = (uint)(num3 / 2);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000061E1 File Offset: 0x000043E1
		private void OnInnerEntryStyleSet(object o, StyleSetArgs args)
		{
			this.UpdateStyle();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000061EB File Offset: 0x000043EB
		private void OnInnerEntryStateChanged(object o, EventArgs args)
		{
			this.UpdateStyle();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000061F5 File Offset: 0x000043F5
		private void OnInnerEntryFocusEvent(object o, EventArgs args)
		{
			base.QueueDraw();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00006200 File Offset: 0x00004400
		private void OnButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.entry.HasFocus = true;
				if (o == this.filter_button)
				{
					this.ShowMenu(args.Event.Time);
				}
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00006254 File Offset: 0x00004454
		protected virtual void OnRequestMenu(EventArgs e)
		{
			EventHandler requestMenu = this.RequestMenu;
			if (requestMenu != null)
			{
				requestMenu(this, e);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000092 RID: 146 RVA: 0x0000627C File Offset: 0x0000447C
		// (remove) Token: 0x06000093 RID: 147 RVA: 0x000062B8 File Offset: 0x000044B8
		public event EventHandler RequestMenu;

		// Token: 0x06000094 RID: 148 RVA: 0x000062F4 File Offset: 0x000044F4
		public void GrabFocusEntry()
		{
			this.entry.GrabFocus();
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00006303 File Offset: 0x00004503
		private void OnClearButtonClicked(object o, EventArgs args)
		{
			this.active_filter_id = 0;
			this.entry.Text = string.Empty;
			this.NotifyActivated();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006328 File Offset: 0x00004528
		protected override void OnDestroyed()
		{
			if (this.menu != null)
			{
				this.menu.Destroy();
				this.menu = null;
			}
			base.OnDestroyed();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006360 File Offset: 0x00004560
		protected override bool OnKeyPressEvent(EventKey evnt)
		{
			bool result;
			if (evnt.Key == Gdk.Key.Escape)
			{
				this.active_filter_id = 0;
				this.entry.Text = string.Empty;
				this.NotifyActivated();
				result = true;
			}
			else
			{
				result = base.OnKeyPressEvent(evnt);
			}
			return result;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000063B4 File Offset: 0x000045B4
		protected override bool OnExposeEvent(EventExpose evnt)
		{
			Gdk.Rectangle rectangle = new Gdk.Rectangle(this.alignment.Allocation.X, this.box.Allocation.Y, this.alignment.Allocation.Width, this.box.Allocation.Height);
			if (this.hasFrame && (!this.roundedShape || (this.roundedShape && !this.customRoundedShapeDrawing)))
			{
				Gtk.Style.PaintShadow(this.entry.Style, base.GdkWindow, StateType.Normal, ShadowType.In, evnt.Area, this.entry, "entry", rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
			}
			else if (!this.roundedShape)
			{
				using (Cairo.Context context = Gdk.CairoHelper.Create(base.GdkWindow))
				{
					context.RoundedRectangle((double)rectangle.X + 0.5, (double)rectangle.Y + 0.5, (double)(rectangle.Width - 1), (double)(rectangle.Height - 1), 4.0);
					context.SetSourceColor(this.entry.Style.Base(StateType.Normal).ToCairoColor());
					context.Fill();
				}
			}
			else
			{
				using (Cairo.Context context = Gdk.CairoHelper.Create(base.GdkWindow))
				{
					SearchEntry.RoundBorder(context, (double)rectangle.X + 0.5, (double)rectangle.Y + 0.5, (double)(rectangle.Width - 1), (double)(rectangle.Height - 1));
					context.SetSourceColor(this.entry.Style.Base(StateType.Normal).ToCairoColor());
					context.Fill();
				}
			}
			base.PropagateExpose(base.Child, evnt);
			if (this.hasFrame && this.roundedShape && this.customRoundedShapeDrawing)
			{
				using (Cairo.Context context = Gdk.CairoHelper.Create(base.GdkWindow))
				{
					SearchEntry.RoundBorder(context, (double)rectangle.X + 0.5, (double)rectangle.Y + 0.5, (double)(rectangle.Width - 1), (double)(rectangle.Height - 1));
					context.SetSourceColor(Styles.WidgetBorderColor);
					context.LineWidth = 1.0;
					context.Stroke();
				}
			}
			return true;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00006690 File Offset: 0x00004890
		private static void RoundBorder(Cairo.Context ctx, double x, double y, double w, double h)
		{
			double num = h / 2.0;
			ctx.Arc(x + num, y + num, num, 1.5707963267948966, 4.71238898038469);
			ctx.LineTo(x + w - num, y);
			ctx.Arc(x + w - num, y + num, num, 4.71238898038469, 7.853981633974483);
			ctx.LineTo(x + num, y + h);
			ctx.ClosePath();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000670F File Offset: 0x0000490F
		protected override void OnShown()
		{
			base.OnShown();
			this.ShowHideButtons();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00006720 File Offset: 0x00004920
		protected virtual void OnChanged()
		{
			if (this.Ready)
			{
				EventHandler eventHandler = this.entry_changed;
				if (eventHandler != null)
				{
					eventHandler(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000675C File Offset: 0x0000495C
		protected virtual void OnFilterChanged()
		{
			EventHandler eventHandler = this.filter_changed;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
			if (this.IsQueryAvailable)
			{
				this.OnInnerEntryChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000067A4 File Offset: 0x000049A4
		public CheckMenuItem AddFilterOption(int id, string label)
		{
			if (id < 0)
			{
				throw new ArgumentException("id", "must be >= 0");
			}
			SearchEntry.FilterMenuItem filterMenuItem = new SearchEntry.FilterMenuItem(id, label);
			filterMenuItem.Toggled += this.OnMenuItemToggled;
			this.menu.Append(filterMenuItem);
			if (this.ActiveFilterID < 0)
			{
				filterMenuItem.Toggle();
			}
			this.filter_button.Visible = true;
			return filterMenuItem;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00006824 File Offset: 0x00004A24
		public MenuItem AddMenuItem(string label)
		{
			MenuItem menuItem = new MenuItem(label);
			this.menu.Append(menuItem);
			return menuItem;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000684B File Offset: 0x00004A4B
		public void AddFilterSeparator()
		{
			this.menu.Append(new SeparatorMenuItem());
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00006860 File Offset: 0x00004A60
		public void RemoveFilterOption(int id)
		{
			SearchEntry.FilterMenuItem filterMenuItem = this.FindFilterMenuItem(id);
			if (filterMenuItem != null)
			{
				this.menu.Remove(filterMenuItem);
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00006890 File Offset: 0x00004A90
		public void ActivateFilter(int id)
		{
			SearchEntry.FilterMenuItem filterMenuItem = this.FindFilterMenuItem(id);
			if (filterMenuItem != null)
			{
				filterMenuItem.Toggle();
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000068B8 File Offset: 0x00004AB8
		private SearchEntry.FilterMenuItem FindFilterMenuItem(int id)
		{
			foreach (object obj in this.menu)
			{
				MenuItem menuItem = (MenuItem)obj;
				if (menuItem is SearchEntry.FilterMenuItem && ((SearchEntry.FilterMenuItem)menuItem).ID == id)
				{
					return (SearchEntry.FilterMenuItem)menuItem;
				}
			}
			return null;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000694C File Offset: 0x00004B4C
		public string GetLabelForFilterID(int id)
		{
			SearchEntry.FilterMenuItem filterMenuItem = this.FindFilterMenuItem(id);
			string result;
			if (filterMenuItem == null)
			{
				result = null;
			}
			else
			{
				result = filterMenuItem.Label;
			}
			return result;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000697C File Offset: 0x00004B7C
		public void CancelSearch()
		{
			this.entry.Text = string.Empty;
			this.ActivateFilter(0);
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00006998 File Offset: 0x00004B98
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x000069B0 File Offset: 0x00004BB0
		public int ActiveFilterID
		{
			get
			{
				return this.active_filter_id;
			}
			set
			{
				if (value != this.active_filter_id)
				{
					this.active_filter_id = value;
					this.OnFilterChanged();
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000069E0 File Offset: 0x00004BE0
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00006A0D File Offset: 0x00004C0D
		public string EmptyMessage
		{
			get
			{
				return this.entry.Sensitive ? this.empty_message : string.Empty;
			}
			set
			{
				this.empty_message = value;
				this.entry.QueueDraw();
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00006A24 File Offset: 0x00004C24
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00006A46 File Offset: 0x00004C46
		public string Query
		{
			get
			{
				return this.entry.Text.Trim();
			}
			set
			{
				this.entry.Text = value.Trim();
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00006A5C File Offset: 0x00004C5C
		public bool IsQueryAvailable
		{
			get
			{
				return this.Query != null && this.Query != string.Empty;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00006A8C File Offset: 0x00004C8C
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00006AA4 File Offset: 0x00004CA4
		public bool Ready
		{
			get
			{
				return this.ready;
			}
			set
			{
				this.ready = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00006AB0 File Offset: 0x00004CB0
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00006ACD File Offset: 0x00004CCD
		public new bool HasFocus
		{
			get
			{
				return this.entry.HasFocus;
			}
			set
			{
				this.entry.HasFocus = true;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00006AE0 File Offset: 0x00004CE0
		public Entry InnerEntry
		{
			get
			{
				return this.entry;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00006AF8 File Offset: 0x00004CF8
		protected override void OnStateChanged(StateType previous_state)
		{
			base.OnStateChanged(previous_state);
			this.entry.Sensitive = (base.State != StateType.Insensitive);
			this.filter_button.Sensitive = (base.State != StateType.Insensitive);
			this.clear_button.Sensitive = (base.State != StateType.Insensitive);
		}

		// Token: 0x0400005B RID: 91
		private Gtk.Alignment alignment;

		// Token: 0x0400005C RID: 92
		private Gtk.Alignment entryAlignment;

		// Token: 0x0400005D RID: 93
		private HBox box;

		// Token: 0x0400005E RID: 94
		private Entry entry;

		// Token: 0x0400005F RID: 95
		private IconButton filter_button;

		// Token: 0x04000060 RID: 96
		private IconButton clear_button;

		// Token: 0x04000061 RID: 97
		private Menu menu;

		// Token: 0x04000062 RID: 98
		private int active_filter_id = -1;

		// Token: 0x04000063 RID: 99
		private uint changed_timeout_id = 0U;

		// Token: 0x04000064 RID: 100
		private string empty_message;

		// Token: 0x04000065 RID: 101
		private bool ready = false;

		// Token: 0x04000068 RID: 104
		private EventHandler activated_event;

		// Token: 0x04000069 RID: 105
		private bool roundedShape;

		// Token: 0x0400006A RID: 106
		private bool hasFrame = true;

		// Token: 0x0400006B RID: 107
		private bool customRoundedShapeDrawing = false;

		// Token: 0x0400006C RID: 108
		private bool forceFilterButtonVisible = true;

		// Token: 0x0400006D RID: 109
		private EventBox statusLabelEventBox;

		// Token: 0x0400006E RID: 110
		private bool toggling = false;

		// Token: 0x0200000F RID: 15
		private class FilterMenuItem : CheckMenuItem
		{
			// Token: 0x060000B3 RID: 179 RVA: 0x00006B56 File Offset: 0x00004D56
			public FilterMenuItem(int id, string label) : base(label)
			{
				this.id = id;
				this.label = label;
				base.DrawAsRadio = true;
			}

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x060000B4 RID: 180 RVA: 0x00006B78 File Offset: 0x00004D78
			public int ID
			{
				get
				{
					return this.id;
				}
			}

			// Token: 0x1700001B RID: 27
			// (get) Token: 0x060000B5 RID: 181 RVA: 0x00006B90 File Offset: 0x00004D90
			public string Label
			{
				get
				{
					return this.label;
				}
			}

			// Token: 0x1400000A RID: 10
			// (add) Token: 0x060000B6 RID: 182 RVA: 0x00006BA8 File Offset: 0x00004DA8
			// (remove) Token: 0x060000B7 RID: 183 RVA: 0x00006BE4 File Offset: 0x00004DE4
			public new event EventHandler Toggled;

			// Token: 0x060000B8 RID: 184 RVA: 0x00006C20 File Offset: 0x00004E20
			protected override void OnActivated()
			{
				base.OnActivated();
				if (this.Toggled != null)
				{
					this.Toggled(this, EventArgs.Empty);
				}
			}

			// Token: 0x04000071 RID: 113
			private int id;

			// Token: 0x04000072 RID: 114
			private string label;
		}

		// Token: 0x02000010 RID: 16
		private class FramelessEntry : Entry
		{
			// Token: 0x060000B9 RID: 185 RVA: 0x00006C56 File Offset: 0x00004E56
			public FramelessEntry(SearchEntry parent)
			{
				this.parent = parent;
				base.HasFrame = false;
				parent.StyleSet += new StyleSetHandler(this.OnParentStyleSet);
				base.WidthChars = 1;
			}

			// Token: 0x060000BA RID: 186 RVA: 0x00006C8B File Offset: 0x00004E8B
			private void OnParentStyleSet(object o, EventArgs args)
			{
				this.RefreshGC();
				base.QueueDraw();
			}

			// Token: 0x060000BB RID: 187 RVA: 0x00006C9C File Offset: 0x00004E9C
			private void RefreshGC()
			{
				this.text_gc = null;
			}

			// Token: 0x060000BC RID: 188 RVA: 0x00006CA6 File Offset: 0x00004EA6
			protected override void OnDestroyed()
			{
				this.parent.StyleSet -= new StyleSetHandler(this.OnParentStyleSet);
				base.OnDestroyed();
			}

			// Token: 0x060000BD RID: 189 RVA: 0x00006CC8 File Offset: 0x00004EC8
			public static Gdk.Color ColorBlend(Gdk.Color a, Gdk.Color b)
			{
				double num = 0.5;
				if (num < 0.0 || num > 1.0)
				{
					throw new ApplicationException("blend < 0.0 || blend > 1.0");
				}
				double num2 = 1.0 - num;
				int num3 = a.Red >> 8;
				int num4 = a.Green >> 8;
				int num5 = a.Blue >> 8;
				int num6 = b.Red >> 8;
				int num7 = b.Green >> 8;
				int num8 = b.Blue >> 8;
				double num9 = (double)(num3 + num6);
				double num10 = (double)(num4 + num7);
				double num11 = (double)(num5 + num8);
				double num12 = num9 * num2;
				double num13 = num10 * num2;
				double num14 = num11 * num2;
				Gdk.Color result = new Gdk.Color((byte)num12, (byte)num13, (byte)num14);
				Colormap.System.AllocColor(ref result, true, true);
				return result;
			}

			// Token: 0x060000BE RID: 190 RVA: 0x00006DB4 File Offset: 0x00004FB4
			protected override bool OnExposeEvent(EventExpose evnt)
			{
				bool result;
				if (evnt.Window == base.GdkWindow)
				{
					result = true;
				}
				else
				{
					bool flag = base.OnExposeEvent(evnt);
					if (this.text_gc == null)
					{
						this.text_gc = new Gdk.GC(evnt.Window);
						this.text_gc.Copy(base.Style.TextGC(StateType.Normal));
						Gdk.Color a = this.parent.Style.Base(StateType.Normal);
						Gdk.Color b = this.parent.Style.Text(StateType.Normal);
						this.text_gc.RgbFgColor = SearchEntry.FramelessEntry.ColorBlend(a, b);
					}
					if (base.Text.Length > 0 || base.HasFocus || this.parent.EmptyMessage == null)
					{
						result = flag;
					}
					else
					{
						if (this.layout == null)
						{
							this.layout = new Pango.Layout(base.PangoContext);
							this.layout.FontDescription = base.PangoContext.FontDescription.Copy();
						}
						this.layout.SetMarkup(this.parent.EmptyMessage);
						int num;
						int num2;
						this.layout.GetPixelSize(out num, out num2);
						evnt.Window.DrawLayout(this.text_gc, 2, (base.SizeRequest().Height - num2) / 2, this.layout);
						result = flag;
					}
				}
				return result;
			}

			// Token: 0x060000BF RID: 191 RVA: 0x00006F30 File Offset: 0x00005130
			protected override bool OnButtonPressEvent(EventButton evnt)
			{
				return evnt.Button != 3U && base.OnButtonPressEvent(evnt);
			}

			// Token: 0x04000074 RID: 116
			private SearchEntry parent;

			// Token: 0x04000075 RID: 117
			private Pango.Layout layout;

			// Token: 0x04000076 RID: 118
			private Gdk.GC text_gc;
		}
	}
}
