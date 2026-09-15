using System;
using System.Text.RegularExpressions;
using CocoStudio.Model.Editor;
using Gdk;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Editor
{
	internal class SpriteSheetSizeEditor : BaseEditor
	{
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

		protected override void OnSetControl()
		{
			SizeValue value = base.PropertyItem.GetValue<SizeValue>(0);
			this.comboBoxWidth.Entry.Text = value.Width.ToString();
			this.comboBoxHeight.Entry.Text = value.Height.ToString();
			this.oldWidth = value.Width;
			this.oldHeight = value.Height;
		}

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

		private int ConvertToInt(string text)
		{
			if (string.IsNullOrEmpty(text) || !Regex.IsMatch(text, "^[0-9]+$"))
			{
				return -1;
			}
			return Convert.ToInt32(text);
		}

		private void ComboBoxChangedHandler(object sender, EventArgs e)
		{
			ComboBoxEntry comboBoxEntry = sender as ComboBoxEntry;
			if (comboBoxEntry.Active != -1)
			{
				comboBoxEntry.Entry.Text = this.comboxList[comboBoxEntry.Active].ToString();
			}
		}

		private void WidthEntryChangedHandler(object sender, EventArgs e)
		{
			if (!this.comboBoxWidth.Entry.IsFocus)
			{
				this.SetWidth();
			}
		}

		private void HeightEntryChangedHandler(object sender, EventArgs e)
		{
			if (!this.comboBoxHeight.Entry.IsFocus)
			{
				this.SetHeight();
			}
		}

		private void WidthEntryKeyReleaseHandler(object o, KeyReleaseEventArgs args)
		{
			this.isKeyPress = true;
			if (KeyboardExtend.IsEnterKey(args.Event.Key) && this.comboBoxWidth.Entry.IsFocus)
			{
				this.SetWidth();
				this.isKeyPress = false;
			}
		}

		private void HeightEntryKeyReleaseHandler(object o, KeyReleaseEventArgs args)
		{
			this.isKeyPress = true;
			if (KeyboardExtend.IsEnterKey(args.Event.Key) && this.comboBoxHeight.Entry.IsFocus)
			{
				this.SetHeight();
				this.isKeyPress = false;
			}
		}

		private void WidthEntryFocusOutHandler(object o, FocusOutEventArgs args)
		{
			if (this.isKeyPress)
			{
				this.SetWidth();
			}
			this.isKeyPress = false;
		}

		private void HeightEntryFocusOutHandler(object o, FocusOutEventArgs args)
		{
			if (this.isKeyPress)
			{
				this.SetHeight();
			}
			this.isKeyPress = false;
		}

		private bool isKeyPress;

		private int maxValue = 1000;

		private int minValue;

		private int oldWidth;

		private int oldHeight;

		private ComboBoxEntry comboBoxWidth;

		private ComboBoxEntry comboBoxHeight;

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
