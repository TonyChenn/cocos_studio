using System;
using System.Collections.Generic;
using Cairo;
using GLib;
using Gdk;
using Gtk;
using Mono.TextEditor;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Pango;
using Xwt.Drawing;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	internal class ToolboxWidget : DrawingArea
	{
		private delegate void CategoryAction(Category category, Size categoryDimension);

		private delegate void ItemAction(Category curCategory, Item item, Size itemDimension);

		private class CustomTooltipWindow : TooltipWindow
		{
			private string tooltip;

			private Label label = new Label();

			public string Tooltip
			{
				get
				{
					return tooltip;
				}
				set
				{
					tooltip = value;
					label.Markup = tooltip;
				}
			}

			public CustomTooltipWindow()
			{
				label.Xalign = 0f;
				label.Xpad = 3;
				label.Ypad = 3;
				Add(label);
			}
		}

		private const uint animationTimeSpan = 10u;

		private const int animationStepSize = 35;

		private const int CategoryLeftPadding = 6;

		private const int CategoryRightPadding = 4;

		private const int CategoryTopBottomPadding = 6;

		private const int ItemTopBottomPadding = 3;

		private const int ItemLeftPadding = 4;

		private const int ItemIconTextItemSpacing = 4;

		private const int IconModePadding = 2;

		private const int TipTimer = 800;

		private List<Category> categories = new List<Category>();

		private bool showCategories = true;

		private bool listMode;

		private int mouseX;

		private int mouseY;

		private FontDescription desc;

		private Xwt.Drawing.Image discloseDown;

		private Xwt.Drawing.Image discloseUp;

		private Cursor handCursor;

		private Pango.Layout layout;

		private Pango.Layout headerLayout;

		private Size iconSize = new Size(24, 24);

		private static readonly Cairo.Color CategoryBackgroundGradientStartColor = new Cairo.Color(248.0 / 255.0, 248.0 / 255.0, 248.0 / 255.0);

		private static readonly Cairo.Color CategoryBackgroundGradientEndColor = new Cairo.Color(0.9411764705882353, 0.9411764705882353, 0.9411764705882353);

		private static readonly Cairo.Color CategoryBorderColor = new Cairo.Color(217.0 / 255.0, 217.0 / 255.0, 217.0 / 255.0);

		private static readonly Cairo.Color CategoryLabelColor = new Cairo.Color(128.0 / 255.0, 128.0 / 255.0, 128.0 / 255.0);

		private Item selectedItem;

		private Item mouseOverItem;

		private Adjustment hAdjustement;

		private Adjustment vAdjustement;

		private bool realSizeRequest;

		private CustomTooltipWindow tooltipWindow;

		private Item tipItem;

		private int tipX;

		private int tipY;

		private uint tipTimeoutId;

		public bool IsListMode
		{
			get
			{
				return listMode;
			}
			set
			{
				listMode = value;
				QueueResize();
				ScrollToSelectedItem();
			}
		}

		public bool CanIconizeToolboxCategories
		{
			get
			{
				foreach (Category category in categories)
				{
					if (category.CanIconizeItems)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool ShowCategories
		{
			get
			{
				return showCategories;
			}
			set
			{
				showCategories = value;
				QueueResize();
				ScrollToSelectedItem();
			}
		}

		public string CustomMessage { get; set; }

		private Size IconSize => iconSize;

		public IEnumerable<Category> Categories => categories;

		public IEnumerable<Item> AllItems
		{
			get
			{
				foreach (Category category in categories)
				{
					foreach (Item item in category.Items)
					{
						yield return item;
					}
				}
			}
		}

		public Action<EventButton> DoPopupMenu { get; set; }

		public Item SelectedItem
		{
			get
			{
				return selectedItem;
			}
			set
			{
				if (selectedItem != value)
				{
					selectedItem = value;
					ScrollToSelectedItem();
					OnSelectedItemChanged(EventArgs.Empty);
				}
			}
		}

		public event EventHandler SelectedItemChanged;

		public event EventHandler ActivateSelectedItem;

		internal void SetCustomFont(FontDescription desc)
		{
			this.desc = desc;
			if (layout != null)
			{
				layout.FontDescription = desc;
			}
			if (headerLayout != null)
			{
				headerLayout.FontDescription = desc;
			}
		}

		public void ClearCategories()
		{
			categories.Clear();
			iconSize = new Size(24, 24);
		}

		public void AddCategory(Category category)
		{
			categories.Add(category);
			foreach (Item item in category.Items)
			{
				if (item.Icon != null)
				{
					iconSize.Width = Math.Max(iconSize.Width, (int)item.Icon.Width);
					iconSize.Height = Math.Max(iconSize.Height, (int)item.Icon.Height);
				}
			}
		}

		public ToolboxWidget()
		{
			base.Events = EventMask.ExposureMask | EventMask.PointerMotionMask | EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | EventMask.KeyPressMask | EventMask.EnterNotifyMask | EventMask.LeaveNotifyMask;
			base.CanFocus = true;
			discloseDown = ImageService.GetIcon("md-disclose-arrow-down", Gtk.IconSize.Menu);
			discloseUp = ImageService.GetIcon("md-disclose-arrow-up", Gtk.IconSize.Menu);
			handCursor = new Cursor(CursorType.Hand1);
		}

		protected override void OnStyleSet(Gtk.Style previous_style)
		{
			if (layout != null)
			{
				layout.Dispose();
				layout = null;
			}
			if (headerLayout != null)
			{
				headerLayout.Dispose();
				headerLayout = null;
			}
			base.OnStyleSet(previous_style);
			layout = new Pango.Layout(base.PangoContext);
			headerLayout = new Pango.Layout(base.PangoContext);
			if (desc != null)
			{
				layout.FontDescription = desc;
				headerLayout.FontDescription = desc;
			}
			headerLayout.Attributes = new AttrList();
		}

		protected override void OnDestroyed()
		{
			HideTooltipWindow();
			if (layout != null)
			{
				layout.Dispose();
				layout = null;
			}
			if (headerLayout != null)
			{
				headerLayout.Dispose();
				headerLayout = null;
			}
			base.OnDestroyed();
			handCursor.Dispose();
		}

		private static Cairo.Color Convert(Gdk.Color color)
		{
			return new Cairo.Color((double)(int)color.Red / 65535.0, (double)(int)color.Green / 65535.0, (double)(int)color.Blue / 65535.0);
		}

		protected override bool OnExposeEvent(EventExpose e)
		{
			Cairo.Context cr = Gdk.CairoHelper.Create(e.Window);
			Gdk.Rectangle area = e.Area;
			if (categories.Count == 0 || !string.IsNullOrEmpty(CustomMessage))
			{
				Pango.Layout layout = new Pango.Layout(base.PangoContext);
				layout.Alignment = Pango.Alignment.Center;
				layout.Width = (int)((double)(base.Allocation.Width * 2 / 3) * Pango.Scale.PangoScale);
				if (!string.IsNullOrEmpty(CustomMessage))
				{
					layout.SetText(CustomMessage);
				}
				else
				{
					layout.SetText(GettextCatalog.GetString("There are no tools available for the current document."));
				}
				cr.MoveTo(base.Allocation.Width / 6, 12.0);
				cr.SetSourceColor(base.Style.Text(StateType.Normal).ToCairoColor());
				Pango.CairoHelper.ShowLayout(cr, layout);
				layout.Dispose();
				((IDisposable)cr).Dispose();
				return true;
			}
			Cairo.Color backColor = base.Style.Base(StateType.Normal).ToCairoColor();
			cr.SetSourceColor(backColor);
			cr.Rectangle(area.X, area.Y, area.Width, area.Height);
			cr.Fill();
			int xpos = ((hAdjustement != null) ? ((int)hAdjustement.Value) : 0);
			int num = ((vAdjustement != null) ? ((int)vAdjustement.Value) : 0);
			int ypos = -num;
			Category lastCategory = null;
			int lastCategoryYpos = 0;
			Iterate(ref xpos, ref ypos, delegate(Category category, Size itemDimension)
			{
				ProcessExpandAnimation(cr, lastCategory, lastCategoryYpos, backColor, area, ref ypos);
				cr.Rectangle(xpos, ypos, itemDimension.Width, itemDimension.Height);
				using (Cairo.LinearGradient linearGradient = new Cairo.LinearGradient(xpos, ypos, xpos, ypos + itemDimension.Height))
				{
					linearGradient.AddColorStop(0.0, CategoryBackgroundGradientStartColor);
					linearGradient.AddColorStop(1.0, CategoryBackgroundGradientEndColor);
					cr.SetSource(linearGradient);
					cr.Fill();
				}
				if (lastCategory == null || lastCategory.IsExpanded || lastCategory.AnimatingExpand)
				{
					cr.MoveTo(xpos, (double)ypos + 0.5);
					cr.LineTo(itemDimension.Width, (double)ypos + 0.5);
				}
				cr.MoveTo(0.0, (double)(ypos + itemDimension.Height) - 0.5);
				cr.LineTo(xpos + base.Allocation.Width, (double)(ypos + itemDimension.Height) - 0.5);
				cr.SetSourceColor(CategoryBorderColor);
				cr.LineWidth = 1.0;
				cr.Stroke();
				headerLayout.SetText(category.Text);
				cr.SetSourceColor(CategoryLabelColor);
				this.layout.GetPixelSize(out var _, out var height);
				cr.MoveTo(xpos + 6, (double)ypos + Math.Round((double)(itemDimension.Height - height) / 2.0));
				Pango.CairoHelper.ShowLayout(cr, headerLayout);
				Xwt.Drawing.Image image = (category.IsExpanded ? discloseUp : discloseDown);
				cr.DrawImage(this, image, (double)base.Allocation.Width - image.Width - 4.0, (double)ypos + Math.Round(((double)itemDimension.Height - image.Height) / 2.0));
				lastCategory = category;
				lastCategoryYpos = ypos + itemDimension.Height;
			}, delegate(Category curCategory, Item item, Size itemDimension)
			{
				if (item == SelectedItem)
				{
					cr.SetSourceColor(base.Style.Base(StateType.Selected).ToCairoColor());
					cr.Rectangle(xpos, ypos, itemDimension.Width, itemDimension.Height);
					cr.Fill();
				}
				if (listMode || !curCategory.CanIconizeItems)
				{
					cr.DrawImage(this, item.Icon, xpos + 4, (double)ypos + Math.Round(((double)itemDimension.Height - item.Icon.Height) / 2.0));
					this.layout.SetText(item.Text);
					this.layout.GetPixelSize(out var _, out var height);
					cr.SetSourceColor(base.Style.Text((item == SelectedItem) ? StateType.Selected : StateType.Normal).ToCairoColor());
					cr.MoveTo(xpos + 4 + IconSize.Width + 4, (double)ypos + Math.Round((double)(itemDimension.Height - height) / 2.0));
					Pango.CairoHelper.ShowLayout(cr, this.layout);
				}
				else
				{
					cr.DrawImage(this, item.Icon, (double)xpos + Math.Round(((double)itemDimension.Width - item.Icon.Width) / 2.0), (double)ypos + Math.Round(((double)itemDimension.Height - item.Icon.Height) / 2.0));
				}
				if (item == mouseOverItem)
				{
					cr.SetSourceColor(base.Style.Dark(StateType.Prelight).ToCairoColor());
					cr.Rectangle((double)xpos + 0.5, (double)ypos + 0.5, itemDimension.Width - 1, itemDimension.Height - 1);
					cr.Stroke();
				}
			});
			ProcessExpandAnimation(cr, lastCategory, lastCategoryYpos, backColor, area, ref ypos);
			if (lastCategory != null && lastCategory.AnimatingExpand)
			{
				cr.MoveTo(area.X, (double)ypos + 0.5);
				cr.RelLineTo(area.Width, 0.0);
				cr.SetSourceColor(CategoryBorderColor);
				cr.Stroke();
			}
			((IDisposable)cr).Dispose();
			return true;
		}

		private void ProcessExpandAnimation(Cairo.Context cr, Category lastCategory, int lastCategoryYpos, Cairo.Color backColor, Gdk.Rectangle area, ref int ypos)
		{
			if (lastCategory != null && lastCategory.AnimatingExpand)
			{
				int num = (lastCategory.IsExpanded ? (lastCategoryYpos + lastCategory.AnimationHeight) : (ypos + lastCategory.AnimationHeight));
				if (num < lastCategoryYpos)
				{
					num = lastCategoryYpos;
					StopExpandAnimation(lastCategory);
				}
				if (num > ypos)
				{
					num = ypos;
					StopExpandAnimation(lastCategory);
				}
				cr.SetSourceColor(backColor);
				cr.Rectangle(area.X, num, area.Width, ypos - lastCategoryYpos);
				cr.Fill();
				ypos = num;
			}
		}

		protected override bool OnKeyPressEvent(EventKey evnt)
		{
			if (evnt.Key == Gdk.Key.F1 && (evnt.State & ModifierType.ControlMask) == ModifierType.ControlMask)
			{
				if (SelectedItem != null)
				{
					int num = ((vAdjustement != null) ? ((int)vAdjustement.Value) : 0);
					Gdk.Rectangle itemExtends = GetItemExtends(SelectedItem);
					ShowTooltip(SelectedItem, 0u, itemExtends.X, itemExtends.Bottom - num);
				}
				return true;
			}
			switch (evnt.Key)
			{
			case Gdk.Key.Return:
			case Gdk.Key.KP_Enter:
				if (SelectedItem != null)
				{
					OnActivateSelectedItem(EventArgs.Empty);
				}
				return true;
			case Gdk.Key.Up:
			case Gdk.Key.KP_Up:
				if (listMode || SelectedItem is Category)
				{
					SelectedItem = GetPrevItem(SelectedItem);
				}
				else
				{
					Item itemAbove = GetItemAbove(SelectedItem);
					SelectedItem = ((itemAbove != SelectedItem) ? itemAbove : GetCategory(SelectedItem));
				}
				QueueDraw();
				return true;
			case Gdk.Key.Down:
			case Gdk.Key.KP_Down:
				if (listMode || SelectedItem is Category)
				{
					SelectedItem = GetNextItem(SelectedItem);
				}
				else
				{
					Item itemAbove = GetItemBelow(SelectedItem);
					if (itemAbove == SelectedItem)
					{
						Category category2 = GetCategory(SelectedItem);
						itemAbove = GetNextCategory(category2);
						if (itemAbove == category2)
						{
							itemAbove = SelectedItem;
						}
					}
					SelectedItem = itemAbove;
				}
				QueueDraw();
				return true;
			case Gdk.Key.Left:
			case Gdk.Key.KP_Left:
				if (SelectedItem is Category)
				{
					SetCategoryExpanded((Category)SelectedItem, expanded: false);
				}
				else if (listMode)
				{
					SelectedItem = GetCategory(SelectedItem);
				}
				else
				{
					SelectedItem = GetItemLeft(SelectedItem);
				}
				QueueDraw();
				return true;
			case Gdk.Key.Right:
			case Gdk.Key.KP_Right:
				if (SelectedItem is Category)
				{
					Category category = (Category)SelectedItem;
					if (category.IsExpanded)
					{
						if (category.ItemCount > 0)
						{
							SelectedItem = category.Items[0];
						}
					}
					else
					{
						SetCategoryExpanded(category, expanded: true);
					}
				}
				else if (!listMode)
				{
					SelectedItem = GetItemRight(SelectedItem);
				}
				QueueDraw();
				return true;
			default:
				return false;
			}
		}

		protected override void OnUnrealized()
		{
			HideTooltipWindow();
			base.OnUnrealized();
		}

		protected override bool OnLeaveNotifyEvent(EventCrossing evnt)
		{
			if (evnt.Mode == CrossingMode.Normal)
			{
				HideTooltipWindow();
				ClearMouseOverItem();
			}
			base.GdkWindow.Cursor = null;
			return base.OnLeaveNotifyEvent(evnt);
		}

		protected override bool OnScrollEvent(EventScroll evnt)
		{
			HideTooltipWindow();
			ClearMouseOverItem();
			return base.OnScrollEvent(evnt);
		}

		protected override bool OnButtonPressEvent(EventButton e)
		{
			GrabFocus();
			HideTooltipWindow();
			if (mouseOverItem is Category)
			{
				if (!e.TriggersContextMenu() && e.Button == 1 && e.Type == EventType.ButtonPress)
				{
					Category category = (Category)mouseOverItem;
					SetCategoryExpanded(category, !category.IsExpanded);
					return true;
				}
				SelectedItem = mouseOverItem;
				QueueResize();
			}
			else
			{
				SelectedItem = mouseOverItem;
				QueueDraw();
			}
			if (e.TriggersContextMenu())
			{
				if (DoPopupMenu != null)
				{
					DoPopupMenu(null);
					return true;
				}
			}
			else if (e.Type == EventType.TwoButtonPress && SelectedItem != null)
			{
				OnActivateSelectedItem(EventArgs.Empty);
				return true;
			}
			return base.OnButtonPressEvent(e);
		}

		private void SetCategoryExpanded(Category cat, bool expanded)
		{
			if (cat.IsExpanded != expanded)
			{
				cat.IsExpanded = expanded;
				if (cat.IsExpanded)
				{
					StartExpandAnimation(cat);
				}
				else
				{
					StartCollapseAnimation(cat);
				}
			}
		}

		private void StartExpandAnimation(Category cat)
		{
			if (cat.AnimatingExpand)
			{
				Source.Remove(cat.AnimationHandle);
			}
			cat.AnimationHeight = 0;
			cat.AnimatingExpand = true;
			cat.AnimationHandle = GLib.Timeout.Add(10u, delegate
			{
				cat.AnimationHeight += 35;
				QueueResize();
				return true;
			});
		}

		private void StartCollapseAnimation(Category cat)
		{
			if (cat.AnimatingExpand)
			{
				Source.Remove(cat.AnimationHandle);
			}
			cat.AnimationHeight = 0;
			cat.AnimatingExpand = true;
			cat.AnimationHandle = GLib.Timeout.Add(10u, delegate
			{
				cat.AnimationHeight -= 35;
				QueueResize();
				return true;
			});
		}

		private void StopExpandAnimation(Category cat)
		{
			if (cat.AnimatingExpand)
			{
				cat.AnimatingExpand = false;
				Source.Remove(cat.AnimationHandle);
			}
		}

		protected override bool OnPopupMenu()
		{
			if (DoPopupMenu != null)
			{
				DoPopupMenu(null);
				return true;
			}
			return base.OnPopupMenu();
		}

		protected override bool OnMotionNotifyEvent(EventMotion e)
		{
			int xpos = 0;
			int ypos = 0;
			HideTooltipWindow();
			Item item = mouseOverItem;
			mouseOverItem = null;
			mouseX = (int)e.X + (int)((hAdjustement != null) ? hAdjustement.Value : 0.0);
			mouseY = (int)e.Y + (int)((vAdjustement != null) ? vAdjustement.Value : 0.0);
			Iterate(ref xpos, ref ypos, delegate(Category category, Size itemDimension)
			{
				if (xpos <= mouseX && mouseX <= xpos + itemDimension.Width && ypos <= mouseY && mouseY <= ypos + itemDimension.Height)
				{
					mouseOverItem = category;
					base.GdkWindow.Cursor = handCursor;
					ShowTooltip(mouseOverItem, 800u, (int)e.X + 2, (int)e.Y + 16);
				}
			}, delegate(Category curCategory, Item item2, Size itemDimension)
			{
				if (xpos <= mouseX && mouseX <= xpos + itemDimension.Width && ypos <= mouseY && mouseY <= ypos + itemDimension.Height)
				{
					mouseOverItem = item2;
					base.GdkWindow.Cursor = null;
					ShowTooltip(mouseOverItem, 800u, (int)e.X + 2, (int)e.Y + 16);
				}
			});
			if (mouseOverItem == null)
			{
				base.GdkWindow.Cursor = null;
			}
			if (item != mouseOverItem)
			{
				QueueDraw();
			}
			return base.OnMotionNotifyEvent(e);
		}

		protected virtual void OnSelectedItemChanged(EventArgs args)
		{
			HideTooltipWindow();
			if (SelectedItemChanged != null)
			{
				SelectedItemChanged(this, args);
			}
		}

		protected virtual void OnActivateSelectedItem(EventArgs args)
		{
			if (ActivateSelectedItem != null)
			{
				ActivateSelectedItem(this, args);
			}
		}

		private void ClearMouseOverItem()
		{
			if (mouseOverItem != null)
			{
				mouseOverItem = null;
			}
			HideTooltipWindow();
			QueueDraw();
		}

		private Category GetCategory(Item item)
		{
			Category result = null;
			int xpos = 0;
			int ypos = 0;
			Iterate(ref xpos, ref ypos, delegate
			{
			}, delegate(Category curCategory, Item innerItem, Size itemDimension)
			{
				if (innerItem == item)
				{
					result = curCategory;
				}
			});
			return result;
		}

		private Category GetNextCategory(Category category)
		{
			Category result = category;
			Category last = null;
			int xpos = 0;
			int ypos = 0;
			Iterate(ref xpos, ref ypos, delegate(Category curCategory, Size itemDimension)
			{
				if (last == category)
				{
					result = curCategory;
				}
				last = curCategory;
			}, delegate
			{
			});
			return result;
		}

		private Item GetItemRight(Item item)
		{
			Item result = item;
			Gdk.Rectangle rect = GetItemExtends(item);
			int xpos = 0;
			int ypos = 0;
			Iterate(ref xpos, ref ypos, delegate
			{
			}, delegate(Category curCategory, Item curItem, Size itemDimension)
			{
				if (xpos > rect.X && ypos == rect.Y && result == item)
				{
					result = curItem;
				}
			});
			return result;
		}

		private Item GetItemLeft(Item item)
		{
			Item result = item;
			Gdk.Rectangle rect = GetItemExtends(item);
			int xpos = 0;
			int ypos = 0;
			Iterate(ref xpos, ref ypos, delegate
			{
			}, delegate(Category curCategory, Item curItem, Size itemDimension)
			{
				if (xpos < rect.X && ypos == rect.Y)
				{
					result = curItem;
				}
			});
			return result;
		}

		private Item GetItemBelow(Item item)
		{
			Category itemCategory = GetCategory(item);
			Item result = item;
			Gdk.Rectangle rect = GetItemExtends(item);
			int xpos = 0;
			int ypos = 0;
			Iterate(ref xpos, ref ypos, delegate
			{
			}, delegate(Category curCategory, Item curItem, Size itemDimension)
			{
				if (ypos > rect.Y && xpos == rect.X && result == item && curCategory == itemCategory)
				{
					result = curItem;
				}
			});
			return result;
		}

		private Item GetItemAbove(Item item)
		{
			Category itemCategory = GetCategory(item);
			Item result = item;
			Gdk.Rectangle rect = GetItemExtends(item);
			int xpos = 0;
			int ypos = 0;
			Iterate(ref xpos, ref ypos, delegate
			{
			}, delegate(Category curCategory, Item curItem, Size itemDimension)
			{
				if (ypos < rect.Y && xpos == rect.X && curCategory == itemCategory)
				{
					result = curItem;
				}
			});
			return result;
		}

		private Gdk.Rectangle GetItemExtends(Item item)
		{
			Gdk.Rectangle result = default(Gdk.Rectangle);
			ref Gdk.Rectangle reference = ref result;
			reference = new Gdk.Rectangle(0, 0, 0, 0);
			int xpos = 0;
			int ypos = 0;
			Iterate(ref xpos, ref ypos, delegate(Category category, Size itemDimension)
			{
				if (item == category)
				{
					ref Gdk.Rectangle reference2 = ref result;
					reference2 = new Gdk.Rectangle(xpos, ypos, itemDimension.Width, itemDimension.Height);
				}
			}, delegate(Category curCategory, Item curItem, Size itemDimension)
			{
				if (item == curItem)
				{
					ref Gdk.Rectangle reference2 = ref result;
					reference2 = new Gdk.Rectangle(xpos, ypos, itemDimension.Width, itemDimension.Height);
				}
			});
			return result;
		}

		private Item GetPrevItem(Item currentItem)
		{
			Item result = currentItem;
			Item lastItem = null;
			int xpos = 0;
			int ypos = 0;
			Iterate(ref xpos, ref ypos, delegate(Category category, Size itemDimension)
			{
				if (currentItem == category && lastItem != null)
				{
					result = lastItem;
				}
				lastItem = category;
			}, delegate(Category curCategory, Item item, Size itemDimension)
			{
				if (currentItem == item && lastItem != null)
				{
					result = lastItem;
				}
				lastItem = item;
			});
			return result;
		}

		private Item GetNextItem(Item currentItem)
		{
			Item result = currentItem;
			Item lastItem = null;
			int xpos = 0;
			int ypos = 0;
			Iterate(ref xpos, ref ypos, delegate(Category category, Size itemDimension)
			{
				if (lastItem == currentItem)
				{
					result = category;
				}
				lastItem = category;
			}, delegate(Category curCategory, Item item, Size itemDimension)
			{
				if (lastItem == currentItem)
				{
					result = item;
				}
				lastItem = item;
			});
			return result;
		}

		public void ScrollToSelectedItem()
		{
			if (SelectedItem != null && vAdjustement != null)
			{
				Gdk.Rectangle itemExtends = GetItemExtends(SelectedItem);
				if (vAdjustement.Value > (double)itemExtends.Top)
				{
					vAdjustement.Value = itemExtends.Top;
				}
				if (vAdjustement.Value + (double)base.Allocation.Height < (double)itemExtends.Bottom)
				{
					vAdjustement.Value = itemExtends.Bottom - base.Allocation.Height;
				}
			}
		}

		protected override void OnSetScrollAdjustments(Adjustment hAdjustement, Adjustment vAdjustement)
		{
			this.hAdjustement = hAdjustement;
			if (this.hAdjustement != null)
			{
				this.hAdjustement.ValueChanged += delegate
				{
					QueueDraw();
				};
			}
			this.vAdjustement = vAdjustement;
			if (this.vAdjustement != null)
			{
				this.vAdjustement.ValueChanged += delegate
				{
					QueueDraw();
				};
			}
		}

		private void IterateItems(Category category, ref int xpos, ref int ypos, ItemAction action)
		{
			if (listMode || !category.CanIconizeItems)
			{
				foreach (Item item in category.Items)
				{
					if (item.IsVisible)
					{
						layout.SetText(item.Text);
						layout.GetPixelSize(out var _, out var height);
						height = Math.Max(IconSize.Height, height);
						height += 6;
						xpos = 0;
						action?.Invoke(category, item, new Size(base.Allocation.Width, height));
						ypos += height;
					}
				}
				return;
			}
			foreach (Item item2 in category.Items)
			{
				if (item2.IsVisible)
				{
					if (xpos + IconSize.Width >= base.Allocation.Width)
					{
						xpos = 0;
						ypos += IconSize.Height;
					}
					action?.Invoke(category, item2, IconSize);
					xpos += IconSize.Width;
				}
			}
			ypos += IconSize.Height;
		}

		private void Iterate(ref int xpos, ref int ypos, CategoryAction catAction, ItemAction action)
		{
			foreach (Category category in categories)
			{
				if (category.IsVisible)
				{
					xpos = 0;
					if (showCategories)
					{
						layout.SetText(category.Text);
						layout.GetPixelSize(out var _, out var height);
						height += 12;
						catAction?.Invoke(category, new Size(base.Allocation.Width, height));
						ypos += height;
					}
					if (category.IsExpanded || category.AnimatingExpand || !showCategories)
					{
						IterateItems(category, ref xpos, ref ypos, action);
					}
				}
			}
		}

		protected override void OnSizeRequested(ref Requisition req)
		{
			if (!realSizeRequest)
			{
				req.Width = 50;
				req.Height = 0;
				return;
			}
			int xpos = 0;
			int ypos = 0;
			Iterate(ref xpos, ref ypos, null, null);
			req.Width = 50;
			req.Height = ypos;
			if (vAdjustement != null)
			{
				vAdjustement.SetBounds(0.0, ypos, 20.0, base.Allocation.Height, base.Allocation.Height);
				if (ypos < base.Allocation.Height)
				{
					vAdjustement.Value = 0.0;
				}
				if (vAdjustement.Value + vAdjustement.PageSize > vAdjustement.Upper)
				{
					vAdjustement.Value = vAdjustement.Upper - vAdjustement.PageSize;
				}
				if (vAdjustement.Value < 0.0)
				{
					vAdjustement.Value = 0.0;
				}
			}
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
			if (!realSizeRequest)
			{
				realSizeRequest = true;
				QueueResize();
			}
			else
			{
				realSizeRequest = false;
			}
		}

		public void HideTooltipWindow()
		{
			if (tipTimeoutId != 0)
			{
				Source.Remove(tipTimeoutId);
				tipTimeoutId = 0u;
			}
			if (tooltipWindow != null)
			{
				tooltipWindow.Destroy();
				tooltipWindow = null;
			}
		}

		private bool ShowTooltip()
		{
			HideTooltipWindow();
			tooltipWindow = new CustomTooltipWindow();
			tooltipWindow.Tooltip = tipItem.Tooltip;
			tooltipWindow.ParentWindow = base.GdkWindow;
			base.GdkWindow.GetOrigin(out var x, out var y);
			tooltipWindow.Move(Math.Max(0, x + tipX), y + tipY);
			tooltipWindow.ShowAll();
			return false;
		}

		public void ShowTooltip(Item item, uint timer, int x, int y)
		{
			HideTooltipWindow();
			if (!string.IsNullOrEmpty(item.Tooltip))
			{
				tipItem = item;
				tipX = x;
				tipY = y;
				tipTimeoutId = GLib.Timeout.Add(timer, ShowTooltip);
			}
		}
	}
}
