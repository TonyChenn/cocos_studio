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
	// Token: 0x02000096 RID: 150
	internal class PropertyColorEditor : BaseEditor
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00016344 File Offset: 0x00014544
		public override bool CanCaching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00016358 File Offset: 0x00014558
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

		// Token: 0x06000525 RID: 1317 RVA: 0x0001663F File Offset: 0x0001483F
		private void color_ColorChanged(object sender, ColorExEvent e)
		{
			base.UpdatePropertyValue(e.Color, this.textColorProperty);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001665C File Offset: 0x0001485C
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

		// Token: 0x06000527 RID: 1319 RVA: 0x000166B4 File Offset: 0x000148B4
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

		// Token: 0x06000528 RID: 1320 RVA: 0x0001674C File Offset: 0x0001494C
		public Gdk.Color ConvertColor(System.Drawing.Color color)
		{
			return new Gdk.Color(color.R, color.G, color.B);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00016778 File Offset: 0x00014978
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

		// Token: 0x0600052A RID: 1322 RVA: 0x000167BC File Offset: 0x000149BC
		private void combox_Changed(object sender, EventArgs e)
		{
			ComboBoxEntry comboBoxEntry = sender as ComboBoxEntry;
			if (comboBoxEntry.Active != -1)
			{
				this.combox.Entry.Text = this.comboxList[comboBoxEntry.Active].ToString();
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00016808 File Offset: 0x00014A08
		private void Entry_Changed(object sender, EventArgs e)
		{
			if (!this.combox.Entry.IsFocus)
			{
				this.FontValue();
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00016834 File Offset: 0x00014A34
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

		// Token: 0x0600052D RID: 1325 RVA: 0x00016898 File Offset: 0x00014A98
		private void Entry_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			this.isKeyPress = true;
			Gdk.Key key = args.Event.Key;
			if ((key == Gdk.Key.Return || key == Gdk.Key.KP_Enter || key == Gdk.Key.ISO_Enter) && this.combox.Entry.IsFocus)
			{
				this.FontValue();
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000168F8 File Offset: 0x00014AF8
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

		// Token: 0x0600052F RID: 1327 RVA: 0x00016A28 File Offset: 0x00014C28
		protected override void OnSetControl()
		{
			this.SetColorValue();
			this.SetFontValue();
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00016A3C File Offset: 0x00014C3C
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

		// Token: 0x04000259 RID: 601
		private ColorEx color;

		// Token: 0x0400025A RID: 602
		private ComboBoxEntry combox;

		// Token: 0x0400025B RID: 603
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

		// Token: 0x0400025C RID: 604
		private string colorText = string.Empty;

		// Token: 0x0400025D RID: 605
		private PropertyInfo textColorProperty;

		// Token: 0x0400025E RID: 606
		private PropertyInfo fontSizeProperty;

		// Token: 0x0400025F RID: 607
		private bool isKeyPress = false;

		// Token: 0x04000260 RID: 608
		private int comboxOldValue = 5;
	}
}
