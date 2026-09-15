using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using Gdk;
using Gtk;
using Gtk.Controls;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class PropertyColorEditor : BaseEditor
	{
		public override bool CanCaching
		{
			get
			{
				return false;
			}
		}

		protected override Widget OnCreateWidget()
		{
			this.color = new ColorEx();
			this.color.ColorChanged += this.color_ColorChanged;
			this.combox = new ComboBoxEntry();
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
			this.combox.Model = listStore;
			CellRendererText cell = new CellRendererText();
			this.combox.PackStart(cell, true);
			this.combox.AddAttribute(cell, "text", 0);
			object value = PropertyItem.FirstObject.GetType().GetProperty("FontSize").GetValue(PropertyItem.FirstObject, null);
			int num2 = this.IndexCombox((int)value);
			if (num2 == -1)
			{
				this.combox.Entry.Text = value.ToString();
				this.comboxOldValue = (int)value;
			}
			else
			{
				this.combox.Entry.Text = this.comboxList[num2].ToString();
				this.comboxOldValue = this.comboxList[num2];
			}
			this.combox.WidthRequest = 40;
			this.combox.Changed += this.combox_Changed;
			this.combox.Entry.KeyReleaseEvent += this.Entry_KeyReleaseEvent;
			this.combox.Entry.FocusOutEvent += this.Entry_FocusOutEvent;
			this.combox.Entry.Changed += this.Entry_Changed;
			this.combox.Entry.MaxLength = 3;
			this.textColorProperty = PropertyItem.FirstObject.GetType().GetProperty("TextColor");
			this.colorText = "TextColor";
			if (this.textColorProperty == null)
			{
				this.textColorProperty = PropertyItem.FirstObject.GetType().GetProperty("CColor");
				this.colorText = "CColor";
			}
			base.SetControl();
			VBox vbox = new VBox();
			vbox.Spacing = (int)PropertyPadStyle.mainRowSpacing;
			if (this.textColorProperty != null)
			{
				HBox hbox = new HBox();
				hbox.Spacing = (int)PropertyPadStyle.mainColumnSpacing;
				hbox.PackStart(this.color);
				hbox.PackEnd(this.combox);
				vbox.Add(hbox);
			}
			else
			{
				vbox.Add(this.combox);
			}
			vbox.ShowAll();
			return vbox;
		}

		private void color_ColorChanged(object sender, ColorExEvent e)
		{
			base.UpdatePropertyValue(e.Color, this.textColorProperty);
		}

		private void SetColorValue()
		{
			if (this.textColorProperty != null)
			{
				object value = this.textColorProperty.GetValue(PropertyItem.FirstObject, null);
				if (value != null)
				{
					this.color.SetColor(this.ConvertColor((System.Drawing.Color)value));
				}
			}
		}

		private void SetFontValue()
		{
			if (this.fontSizeProperty == null)
			{
				this.fontSizeProperty = PropertyItem.FirstObject.GetType().GetProperty("FontSize");
			}
			if (this.fontSizeProperty != null)
			{
				if (this.combox != null && this.combox.Entry != null)
				{
					this.combox.Entry.Text = this.fontSizeProperty.GetValue(PropertyItem.FirstObject, null).ToString();
				}
			}
		}

		public Gdk.Color ConvertColor(System.Drawing.Color color)
		{
			return new Gdk.Color(color.R, color.G, color.B);
		}

		private int IndexCombox(int num)
		{
			for (int i = 0; i < this.comboxList.Length; i++)
			{
				if (this.comboxList[i] == num)
				{
					return i;
				}
			}
			return -1;
		}

		private void combox_Changed(object sender, EventArgs e)
		{
			ComboBoxEntry comboBoxEntry = sender as ComboBoxEntry;
			if (comboBoxEntry.Active != -1)
			{
				this.combox.Entry.Text = this.comboxList[comboBoxEntry.Active].ToString();
			}
		}

		private void Entry_Changed(object sender, EventArgs e)
		{
			if (!this.combox.Entry.IsFocus)
			{
				this.FontValue();
			}
		}

		private void Entry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			string text = this.combox.Entry.Text;
			if (!Regex.IsMatch(text, "^[0-9]+$"))
			{
				this.combox.Entry.Text = "";
			}
			if (this.isKeyPress)
			{
				this.FontValue();
			}
			this.isKeyPress = false;
		}

		private void Entry_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			this.isKeyPress = true;
			Gdk.Key key = args.Event.Key;
			if ((key == Gdk.Key.Return || key == Gdk.Key.KP_Enter || key == Gdk.Key.ISO_Enter) && this.combox.Entry.IsFocus)
			{
				this.FontValue();
			}
		}

		private void FontValue()
		{
			if (this.combox.Entry == null || string.IsNullOrEmpty(this.combox.Entry.Text))
			{
				this.combox.Changed -= this.combox_Changed;
				this.combox.Entry.Text = this.comboxOldValue.ToString();
				this.combox.Changed += this.combox_Changed;
			}
			else
			{
				int num = this.comboxOldValue;
				if (!int.TryParse(this.combox.Entry.Text, out num))
				{
					this.combox.Entry.Text = this.comboxOldValue.ToString();
				}
				else if (num != this.comboxOldValue)
				{
					if (num > 100)
					{
						num = 100;
					}
					if (num < 5)
					{
						num = 5;
					}
					this.combox.Entry.Text = num.ToString();
					base.UpdatePropertyValue(num, this.fontSizeProperty);
					this.comboxOldValue = num;
				}
			}
		}

		protected override void OnSetControl()
		{
			this.SetColorValue();
			this.SetFontValue();
		}

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			if (propertyName != null)
			{
				if (propertyName == "TextColor" || propertyName == "CColor" || propertyName == "FontSize" || propertyName == "FontResource")
				{
					base.SetControl();
				}
			}
		}

		private ColorEx color;

		private ComboBoxEntry combox;

		private int[] comboxList = new int[]
		{
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			16,
			18,
			20,
			22,
			24,
			36,
			48,
			72
		};

		private string colorText = string.Empty;

		private PropertyInfo textColorProperty;

		private PropertyInfo fontSizeProperty;

		private bool isKeyPress = false;

		private int comboxOldValue = 5;
	}
}
