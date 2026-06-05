using System;
using System.Drawing;
using System.Text.RegularExpressions;
using Gdk;

namespace Gtk.Controls
{
	// Token: 0x02000014 RID: 20
	public class ColorEx : EventBox
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003FD4 File Offset: 0x000021D4
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003FEB File Offset: 0x000021EB
		public CcsColorButton ColorButton { get; private set; }

		// Token: 0x06000091 RID: 145 RVA: 0x00003FF4 File Offset: 0x000021F4
		public ColorEx()
		{
			this.ColorButton = new CcsColorButton();
			this.ColorButton.WidthRequest = 25;
			this.ColorButton.HeightRequest = 15;
			this.ColorButton.ColorSet += new EventHandler<ColorSetEventArgs>(this.color_ColorSet);
			this.entry = new TextEntry();
			this.entry.WidthRequest = 70;
			this.entry.HeightRequest = 23;
			this.entry.FocusOutEvent += this.entry_FocusOutEvent;
			this.entry.KeyReleaseEvent += this.entry_KeyReleaseEvent;
			TriangleComboButton triangleComboButton = new TriangleComboButton();
			triangleComboButton.Clicked += this.ButtonClickedHandler;
			EntryShell widget = EntryShellBuilder.CreateShell(this.entry, triangleComboButton);
			HBox hbox = new HBox();
			hbox.Spacing = 1;
			hbox.PackStart(this.ColorButton, false, false, 0U);
			hbox.PackStart(widget);
			base.Add(hbox);
			base.ShowAll();
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004108 File Offset: 0x00002308
		// (set) Token: 0x06000093 RID: 147 RVA: 0x0000411F File Offset: 0x0000231F
		public bool MultilShow { get; set; }

		// Token: 0x06000094 RID: 148 RVA: 0x00004128 File Offset: 0x00002328
		private void entry_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if ((args.Event.Key == Key.Return || args.Event.Key == Key.KP_Enter || args.Event.Key == Key.ISO_Enter) && this.entry.IsFocus)
			{
				this.SetColorText();
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000418B File Offset: 0x0000238B
		private void entry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.SetColorText();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004198 File Offset: 0x00002398
		private void SetColorText()
		{
			string text = this.entry.Text.Replace("#", "");
			if (Regex.IsMatch(text, "^[0-9A-Fa-f]+$"))
			{
				text = text.PadRight(6, '0');
				this.ColorButton.ColorValue = System.Drawing.Color.FromArgb(Convert.ToInt32(text.Substring(0, 2), 16), Convert.ToInt32(text.Substring(2, 2), 16), Convert.ToInt32(text.Substring(4, 2), 16));
				this.entry.Text = '#' + text.Substring(0, 6);
				if (this.ColorChanged != null)
				{
					this.ColorChanged(this, new ColorExEvent(this.ColorButton.ColorValue));
				}
				this.oldStr = this.entry.Text;
			}
			else
			{
				this.entry.Text = this.oldStr;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004293 File Offset: 0x00002493
		private void ButtonClickedHandler(object sender, EventArgs e)
		{
			this.ColorButton.ColorClick();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000042A4 File Offset: 0x000024A4
		private void color_ColorSet(object sender, EventArgs e)
		{
			this.MultilShow = false;
			this.Color = this.ColorButton.ColorValue;
			if (this.ColorChanged != null)
			{
				this.ColorChanged(this, new ColorExEvent(this.ColorButton.ColorValue));
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000042F8 File Offset: 0x000024F8
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00004318 File Offset: 0x00002518
		public System.Drawing.Color Color
		{
			get
			{
				return this.ColorButton.ColorValue;
			}
			set
			{
				if (this.MultilShow)
				{
					this.ColorButton.ColorValue = System.Drawing.Color.Transparent;
					this.entry.Text = "-";
				}
				else
				{
					this.ColorButton.ColorValue = value;
					string text = this.ColorButton.ColorValue.Name.ToUpper();
					this.entry.Text = string.Format("#{0}", text.Substring(2, text.Length - 2));
				}
				this.oldStr = this.entry.Text;
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000043B8 File Offset: 0x000025B8
		public void SetColor(Gdk.Color c)
		{
			this.ColorButton.CurrentColor = c;
			string text = this.ColorButton.ColorValue.Name.ToUpper();
			this.entry.Text = string.Format("#{0}", text.Substring(2, text.Length - 2));
			this.oldStr = this.entry.Text;
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600009C RID: 156 RVA: 0x00004424 File Offset: 0x00002624
		// (remove) Token: 0x0600009D RID: 157 RVA: 0x00004460 File Offset: 0x00002660
		public event EventHandler<ColorExEvent> ColorChanged;

		// Token: 0x04000038 RID: 56
		private TextEntry entry;

		// Token: 0x04000039 RID: 57
		private string oldStr = "";
	}
}
