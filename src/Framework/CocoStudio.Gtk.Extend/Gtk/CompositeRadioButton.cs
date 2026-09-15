using System;
using System.Collections.Generic;
using System.Linq;
using GLib;

namespace Gtk
{
	public class CompositeRadioButton : IconRadioButton
	{
		public RadioItem SelectedItem { get; private set; }

		public event EventHandler SelectedRadioItemChanged;

		public CompositeRadioButton(List<RadioItem> itemList, int index = 0) : base(itemList[index].Icon)
		{
			this.radioItemList = itemList;
			this.SelectedItem = itemList[index];
			base.TooltipText = this.SelectedItem.Tooltip;
			this.bgBox.ButtonPressEvent += this.RightButtonPressEventHandler;
		}

		public void SwitchToNext()
		{
			if (this.radioItemList.Count > 1)
			{
				int num = this.radioItemList.IndexOf(this.SelectedItem);
				num++;
				if (num >= this.radioItemList.Count)
				{
					num -= this.radioItemList.Count;
				}
				this.ChangeCurrentItem(this.radioItemList[num]);
			}
		}

		private void ShowSwitchMenu(uint? activeTime = null)
		{
			if (!this.isPoppingMenu)
			{
				this.isPoppingMenu = true;
				Menu menu = new Menu();
				foreach (RadioItem radioItem in this.radioItemList)
				{
					CompositeRadioButton.CheckMenuItemWithID checkMenuItemWithID = new CompositeRadioButton.CheckMenuItemWithID(radioItem.DisplayName, radioItem.ID);
					checkMenuItemWithID.DrawAsRadio = true;
					if (radioItem == this.SelectedItem)
					{
						checkMenuItemWithID.Active = true;
					}
					checkMenuItemWithID.Activated += this.MenuItemActivatedHandler;
					menu.Add(checkMenuItemWithID);
				}
				menu.ShowAll();
				MenuPositionFunc func = delegate(Menu m, out int x, out int y, out bool pushIn)
				{
					int num;
					int num2;
					base.GdkWindow.GetOrigin(out num, out num2);
					x = num + base.WidthRequest;
					y = num2;
					pushIn = false;
				};
				menu.Popup(null, null, func, 0U, activeTime ?? Global.CurrentEventTime);
				this.isPoppingMenu = false;
			}
		}

		private void RightButtonPressEventHandler(object o, ButtonPressEventArgs args)
		{
			if (args.Event.Button == 3U)
			{
				base.MousePressedHandler(o, args);
				this.ShowSwitchMenu(null);
			}
		}

		protected override void OnMousePressed(ButtonPressEventArgs args)
		{
			base.OnMousePressed(args);
			if (args.Event.Button == 1U)
			{
				uint time = Global.CurrentEventTime + 250U;
				this.glibID = Timeout.Add(250U, delegate
				{
					if (this.isMousePressed)
					{
						this.ShowSwitchMenu(new uint?(time));
					}
					return false;
				});
			}
		}

		protected override void OnMouseReleased(ButtonReleaseEventArgs args)
		{
			Source.Remove(this.glibID);
			base.OnMouseReleased(args);
		}

		private void MenuItemActivatedHandler(object sender, EventArgs e)
		{
			CompositeRadioButton.CheckMenuItemWithID menuItem = sender as CompositeRadioButton.CheckMenuItemWithID;
			RadioItem item = this.radioItemList.First((RadioItem w) => w.ID.Equals(menuItem.ID));
			this.ChangeCurrentItem(item);
		}

		private void ChangeCurrentItem(RadioItem item)
		{
			if (this.radioItemList.Contains(item))
			{
				if (item != this.SelectedItem)
				{
					this.SelectedItem = item;
					base.ChangeImage(this.SelectedItem.Icon);
					base.TooltipText = this.SelectedItem.Tooltip;
					if (this.SelectedRadioItemChanged != null)
					{
						this.SelectedRadioItemChanged(this, new EventArgs());
					}
				}
			}
		}

		private uint glibID = 0U;

		private bool isPoppingMenu = false;

		private List<RadioItem> radioItemList;

		private class CheckMenuItemWithID : CheckMenuItem
		{
			public string ID { get; private set; }

			public CheckMenuItemWithID(string label, string id) : base(label)
			{
				this.ID = id;
			}
		}
	}
}
