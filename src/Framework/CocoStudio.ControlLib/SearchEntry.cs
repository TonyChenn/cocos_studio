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
	[ToolboxItem(true)]
	public class SearchEntry : EventBox
	{
		private event EventHandler filter_changed;

		private event EventHandler entry_changed;

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

		public Entry Entry
		{
			get
			{
				return this.entry;
			}
		}

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

		public SearchEntry()
		{
			base.AppPaintable = true;
			this.BuildWidget();
			this.BuildMenu();
			base.NoShowAll = true;
		}

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

		private void NotifyActivated()
		{
			if (this.activated_event != null)
			{
				this.activated_event(this, EventArgs.Empty);
			}
		}

		private void BuildMenu()
		{
			this.menu = new Menu();
			this.menu.Deactivated += this.OnMenuDeactivated;
		}

		public void PopupFilterMenu()
		{
			this.ShowMenu(0U);
		}

		private void ShowMenu(uint time)
		{
			this.OnRequestMenu(EventArgs.Empty);
			if (this.menu.Children.Length > 0)
			{
				this.menu.Popup(null, null, new MenuPositionFunc(this.OnPositionMenu), 0U, time);
				this.menu.ShowAll();
			}
		}

		private void ShowHideButtons()
		{
			this.clear_button.Visible = (this.entry.Text.Length > 0);
			this.entryAlignment.RightPadding = ((!this.clear_button.Visible && this.roundedShape) ? 6U : 0U);
			this.filter_button.Visible = (this.ForceFilterButtonVisible || (this.menu != null && this.menu.Children.Length > 0));
			this.entryAlignment.LeftPadding = ((!this.filter_button.Visible && this.roundedShape) ? 6U : 0U);
		}

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

		private void OnMenuDeactivated(object o, EventArgs args)
		{
			this.filter_button.QueueDraw();
		}

		public bool IsCheckMenu { get; set; }

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

		private bool OnChangedTimeout()
		{
			this.OnChanged();
			return false;
		}

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

		private void OnInnerEntryStyleSet(object o, StyleSetArgs args)
		{
			this.UpdateStyle();
		}

		private void OnInnerEntryStateChanged(object o, EventArgs args)
		{
			this.UpdateStyle();
		}

		private void OnInnerEntryFocusEvent(object o, EventArgs args)
		{
			base.QueueDraw();
		}

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

		protected virtual void OnRequestMenu(EventArgs e)
		{
			EventHandler requestMenu = this.RequestMenu;
			if (requestMenu != null)
			{
				requestMenu(this, e);
			}
		}

		public event EventHandler RequestMenu;

		public void GrabFocusEntry()
		{
			this.entry.GrabFocus();
		}

		private void OnClearButtonClicked(object o, EventArgs args)
		{
			this.active_filter_id = 0;
			this.entry.Text = string.Empty;
			this.NotifyActivated();
		}

		protected override void OnDestroyed()
		{
			if (this.menu != null)
			{
				this.menu.Destroy();
				this.menu = null;
			}
			base.OnDestroyed();
		}

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

		private static void RoundBorder(Cairo.Context ctx, double x, double y, double w, double h)
		{
			double num = h / 2.0;
			ctx.Arc(x + num, y + num, num, 1.5707963267948966, 4.71238898038469);
			ctx.LineTo(x + w - num, y);
			ctx.Arc(x + w - num, y + num, num, 4.71238898038469, 7.853981633974483);
			ctx.LineTo(x + num, y + h);
			ctx.ClosePath();
		}

		protected override void OnShown()
		{
			base.OnShown();
			this.ShowHideButtons();
		}

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

		public MenuItem AddMenuItem(string label)
		{
			MenuItem menuItem = new MenuItem(label);
			this.menu.Append(menuItem);
			return menuItem;
		}

		public void AddFilterSeparator()
		{
			this.menu.Append(new SeparatorMenuItem());
		}

		public void RemoveFilterOption(int id)
		{
			SearchEntry.FilterMenuItem filterMenuItem = this.FindFilterMenuItem(id);
			if (filterMenuItem != null)
			{
				this.menu.Remove(filterMenuItem);
			}
		}

		public void ActivateFilter(int id)
		{
			SearchEntry.FilterMenuItem filterMenuItem = this.FindFilterMenuItem(id);
			if (filterMenuItem != null)
			{
				filterMenuItem.Toggle();
			}
		}

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

		public void CancelSearch()
		{
			this.entry.Text = string.Empty;
			this.ActivateFilter(0);
		}

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

		public bool IsQueryAvailable
		{
			get
			{
				return this.Query != null && this.Query != string.Empty;
			}
		}

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

		public Entry InnerEntry
		{
			get
			{
				return this.entry;
			}
		}

		protected override void OnStateChanged(StateType previous_state)
		{
			base.OnStateChanged(previous_state);
			this.entry.Sensitive = (base.State != StateType.Insensitive);
			this.filter_button.Sensitive = (base.State != StateType.Insensitive);
			this.clear_button.Sensitive = (base.State != StateType.Insensitive);
		}

		private Gtk.Alignment alignment;

		private Gtk.Alignment entryAlignment;

		private HBox box;

		private Entry entry;

		private IconButton filter_button;

		private IconButton clear_button;

		private Menu menu;

		private int active_filter_id = -1;

		private uint changed_timeout_id = 0U;

		private string empty_message;

		private bool ready = false;

		private EventHandler activated_event;

		private bool roundedShape;

		private bool hasFrame = true;

		private bool customRoundedShapeDrawing = false;

		private bool forceFilterButtonVisible = true;

		private EventBox statusLabelEventBox;

		private bool toggling = false;

		private class FilterMenuItem : CheckMenuItem
		{
			public FilterMenuItem(int id, string label) : base(label)
			{
				this.id = id;
				this.label = label;
				base.DrawAsRadio = true;
			}

			public int ID
			{
				get
				{
					return this.id;
				}
			}

			public string Label
			{
				get
				{
					return this.label;
				}
			}

			public new event EventHandler Toggled;

			protected override void OnActivated()
			{
				base.OnActivated();
				if (this.Toggled != null)
				{
					this.Toggled(this, EventArgs.Empty);
				}
			}

			private int id;

			private string label;
		}

		private class FramelessEntry : Entry
		{
			public FramelessEntry(SearchEntry parent)
			{
				this.parent = parent;
				base.HasFrame = false;
				parent.StyleSet += new StyleSetHandler(this.OnParentStyleSet);
				base.WidthChars = 1;
			}

			private void OnParentStyleSet(object o, EventArgs args)
			{
				this.RefreshGC();
				base.QueueDraw();
			}

			private void RefreshGC()
			{
				this.text_gc = null;
			}

			protected override void OnDestroyed()
			{
				this.parent.StyleSet -= new StyleSetHandler(this.OnParentStyleSet);
				base.OnDestroyed();
			}

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

			protected override bool OnButtonPressEvent(EventButton evnt)
			{
				return evnt.Button != 3U && base.OnButtonPressEvent(evnt);
			}

			private SearchEntry parent;

			private Pango.Layout layout;

			private Gdk.GC text_gc;
		}
	}
}
