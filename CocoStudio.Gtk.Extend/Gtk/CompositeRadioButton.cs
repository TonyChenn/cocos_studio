using System;
using System.Collections.Generic;
using System.Linq;
using GLib;

namespace Gtk
{
	// Token: 0x0200000B RID: 11
	public class CompositeRadioButton : IconRadioButton
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000030FC File Offset: 0x000012FC
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00003113 File Offset: 0x00001313
		public RadioItem SelectedItem { get; private set; }

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000055 RID: 85 RVA: 0x0000311C File Offset: 0x0000131C
		// (remove) Token: 0x06000056 RID: 86 RVA: 0x00003158 File Offset: 0x00001358
		public event EventHandler SelectedRadioItemChanged;

		// Token: 0x06000057 RID: 87 RVA: 0x00003194 File Offset: 0x00001394
		public CompositeRadioButton(List<RadioItem> itemList, int index = 0) : base(itemList[index].Icon)
		{
			this.radioItemList = itemList;
			this.SelectedItem = itemList[index];
			base.TooltipText = this.SelectedItem.Tooltip;
			this.bgBox.ButtonPressEvent += this.RightButtonPressEventHandler;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003204 File Offset: 0x00001404
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

		// Token: 0x06000059 RID: 89 RVA: 0x000032A0 File Offset: 0x000014A0
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

		// Token: 0x0600005A RID: 90 RVA: 0x000033A8 File Offset: 0x000015A8
		private void RightButtonPressEventHandler(object o, ButtonPressEventArgs args)
		{
			if (args.Event.Button == 3U)
			{
				base.MousePressedHandler(o, args);
				this.ShowSwitchMenu(null);
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003428 File Offset: 0x00001628
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

		// Token: 0x0600005C RID: 92 RVA: 0x0000348F File Offset: 0x0000168F
		protected override void OnMouseReleased(ButtonReleaseEventArgs args)
		{
			Source.Remove(this.glibID);
			base.OnMouseReleased(args);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000034D8 File Offset: 0x000016D8
		private void MenuItemActivatedHandler(object sender, EventArgs e)
		{
			CompositeRadioButton.CheckMenuItemWithID menuItem = sender as CompositeRadioButton.CheckMenuItemWithID;
			RadioItem item = this.radioItemList.First((RadioItem w) => w.ID.Equals(menuItem.ID));
			this.ChangeCurrentItem(item);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000351C File Offset: 0x0000171C
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

		// Token: 0x04000020 RID: 32
		private uint glibID = 0U;

		// Token: 0x04000021 RID: 33
		private bool isPoppingMenu = false;

		// Token: 0x04000022 RID: 34
		private List<RadioItem> radioItemList;

		// Token: 0x0200000C RID: 12
		private class CheckMenuItemWithID : CheckMenuItem
		{
			// Token: 0x17000012 RID: 18
			// (get) Token: 0x06000060 RID: 96 RVA: 0x0000359C File Offset: 0x0000179C
			// (set) Token: 0x06000061 RID: 97 RVA: 0x000035B3 File Offset: 0x000017B3
			public string ID { get; private set; }

			// Token: 0x06000062 RID: 98 RVA: 0x000035BC File Offset: 0x000017BC
			public CheckMenuItemWithID(string label, string id) : base(label)
			{
				this.ID = id;
			}
		}
	}
}
