using System;
using System.Text.RegularExpressions;
using CocoStudio.Model.Editor;
using Gdk;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Editor
{
	// Token: 0x02000007 RID: 7
	internal class SpriteSheetSizeEditor : BaseEditor
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000022C8 File Offset: 0x000004C8
		protected override Widget OnCreateWidget()
		{
			ListStore listStore = new ListStore(new Type[]
			{
				typeof(string)
			});
			foreach (int num in this.comboxList)
			{
				listStore.AppendValues(new object[]
				{
					num.ToString()
				});
			}
			CellRendererText cell = new CellRendererText();
			this.comboBoxWidth = new ComboBoxEntry();
			this.comboBoxWidth.WidthRequest = 40;
			this.comboBoxWidth.HeightRequest = 25;
			this.comboBoxWidth.Model = listStore;
			this.comboBoxWidth.PackStart(cell, true);
			this.comboBoxWidth.AddAttribute(cell, "text", 0);
			this.comboBoxWidth.Entry.MaxLength = 8;
			this.comboBoxHeight = new ComboBoxEntry();
			this.comboBoxHeight.HeightRequest = 25;
			this.comboBoxHeight.WidthRequest = 40;
			this.comboBoxHeight.Model = listStore;
			this.comboBoxHeight.PackStart(cell, true);
			this.comboBoxHeight.AddAttribute(cell, "text", 0);
			this.comboBoxHeight.Entry.MaxLength = 8;
			HBox hbox = new HBox();
			hbox.Spacing = 6;
			hbox.PackStart(new Label("W"), false, false, 0U);
			hbox.PackStart(this.comboBoxWidth);
			hbox.PackStart(new Alignment(0.5f, 0.5f, 1f, 1f)
			{
				WidthRequest = 10
			}, false, false, 0U);
			hbox.PackStart(new Label("H"), false, false, 0U);
			hbox.PackStart(this.comboBoxHeight);
			hbox.ShowAll();
			ValueRangeAttribute valueRangeAttribute = base.PropertyItem.Attributes[typeof(ValueRangeAttribute)] as ValueRangeAttribute;
			if (valueRangeAttribute != null)
			{
				this.maxValue = valueRangeAttribute.MaxValue;
				this.minValue = valueRangeAttribute.MinValue;
			}
			base.SetControl();
			this.comboBoxWidth.Changed += this.ComboBoxChangedHandler;
			this.comboBoxWidth.Entry.Changed += this.WidthEntryChangedHandler;
			this.comboBoxWidth.Entry.KeyReleaseEvent += this.WidthEntryKeyReleaseHandler;
			this.comboBoxWidth.Entry.FocusOutEvent += this.WidthEntryFocusOutHandler;
			this.comboBoxHeight.Changed += this.ComboBoxChangedHandler;
			this.comboBoxHeight.Entry.Changed += this.HeightEntryChangedHandler;
			this.comboBoxHeight.Entry.KeyReleaseEvent += this.HeightEntryKeyReleaseHandler;
			this.comboBoxHeight.Entry.FocusOutEvent += this.HeightEntryFocusOutHandler;
			return hbox;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002594 File Offset: 0x00000794
		protected override void OnSetControl()
		{
			SizeValue value = base.PropertyItem.GetValue<SizeValue>(0);
			this.comboBoxWidth.Entry.Text = value.Width.ToString();
			this.comboBoxHeight.Entry.Text = value.Height.ToString();
			this.oldWidth = value.Width;
			this.oldHeight = value.Height;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002604 File Offset: 0x00000804
		private void SetWidth()
		{
			int num = this.ConvertToInt(this.comboBoxWidth.Entry.Text);
			if (num < 0)
			{
				this.comboBoxWidth.Entry.Text = this.oldWidth.ToString();
				return;
			}
			if (num > this.maxValue)
			{
				num = this.maxValue;
				this.comboBoxWidth.Entry.Text = num.ToString();
			}
			if (num < this.minValue)
			{
				num = this.minValue;
				this.comboBoxWidth.Entry.Text = num.ToString();
			}
			if (this.oldWidth == num)
			{
				return;
			}
			SizeValue value = base.PropertyItem.GetValue<SizeValue>(0);
			value.Width = num;
			base.UpdatePropertyValue(value, null);
			this.oldWidth = num;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026C4 File Offset: 0x000008C4
		private void SetHeight()
		{
			int num = this.ConvertToInt(this.comboBoxHeight.Entry.Text);
			if (num < 0)
			{
				this.comboBoxHeight.Entry.Text = this.oldHeight.ToString();
				return;
			}
			if (num > this.maxValue)
			{
				num = this.maxValue;
				this.comboBoxHeight.Entry.Text = num.ToString();
			}
			if (num < this.minValue)
			{
				num = this.minValue;
				this.comboBoxHeight.Entry.Text = num.ToString();
			}
			if (this.oldHeight == num)
			{
				return;
			}
			SizeValue value = base.PropertyItem.GetValue<SizeValue>(0);
			value.Height = num;
			base.UpdatePropertyValue(value, null);
			this.oldHeight = num;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002783 File Offset: 0x00000983
		private int ConvertToInt(string text)
		{
			if (string.IsNullOrEmpty(text) || !Regex.IsMatch(text, "^[0-9]+$"))
			{
				return -1;
			}
			return Convert.ToInt32(text);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000027A4 File Offset: 0x000009A4
		private void ComboBoxChangedHandler(object sender, EventArgs e)
		{
			ComboBoxEntry comboBoxEntry = sender as ComboBoxEntry;
			if (comboBoxEntry.Active != -1)
			{
				comboBoxEntry.Entry.Text = this.comboxList[comboBoxEntry.Active].ToString();
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027E2 File Offset: 0x000009E2
		private void WidthEntryChangedHandler(object sender, EventArgs e)
		{
			if (!this.comboBoxWidth.Entry.IsFocus)
			{
				this.SetWidth();
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000027FC File Offset: 0x000009FC
		private void HeightEntryChangedHandler(object sender, EventArgs e)
		{
			if (!this.comboBoxHeight.Entry.IsFocus)
			{
				this.SetHeight();
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002816 File Offset: 0x00000A16
		private void WidthEntryKeyReleaseHandler(object o, KeyReleaseEventArgs args)
		{
			this.isKeyPress = true;
			if (KeyboardExtend.IsEnterKey(args.Event.Key) && this.comboBoxWidth.Entry.IsFocus)
			{
				this.SetWidth();
				this.isKeyPress = false;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002850 File Offset: 0x00000A50
		private void HeightEntryKeyReleaseHandler(object o, KeyReleaseEventArgs args)
		{
			this.isKeyPress = true;
			if (KeyboardExtend.IsEnterKey(args.Event.Key) && this.comboBoxHeight.Entry.IsFocus)
			{
				this.SetHeight();
				this.isKeyPress = false;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000288A File Offset: 0x00000A8A
		private void WidthEntryFocusOutHandler(object o, FocusOutEventArgs args)
		{
			if (this.isKeyPress)
			{
				this.SetWidth();
			}
			this.isKeyPress = false;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000028A1 File Offset: 0x00000AA1
		private void HeightEntryFocusOutHandler(object o, FocusOutEventArgs args)
		{
			if (this.isKeyPress)
			{
				this.SetHeight();
			}
			this.isKeyPress = false;
		}

		// Token: 0x04000005 RID: 5
		private bool isKeyPress;

		// Token: 0x04000006 RID: 6
		private int maxValue = 1000;

		// Token: 0x04000007 RID: 7
		private int minValue;

		// Token: 0x04000008 RID: 8
		private int oldWidth;

		// Token: 0x04000009 RID: 9
		private int oldHeight;

		// Token: 0x0400000A RID: 10
		private ComboBoxEntry comboBoxWidth;

		// Token: 0x0400000B RID: 11
		private ComboBoxEntry comboBoxHeight;

		// Token: 0x0400000C RID: 12
		private int[] comboxList = new int[]
		{
			32,
			64,
			128,
			256,
			512,
			1024,
			2048,
			4096
		};
	}
}
