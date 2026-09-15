using System;
using System.Drawing;
using System.Text.RegularExpressions;
using Gdk;

namespace Gtk.Controls
{
	public class ColorEx : EventBox
	{
		public CcsColorButton ColorButton { get; private set; }

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

		public bool MultilShow { get; set; }

		private void entry_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if ((args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter || args.Event.Key == Gdk.Key.ISO_Enter) && this.entry.IsFocus)
			{
				this.SetColorText();
			}
		}

		private void entry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.SetColorText();
		}

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

		private void ButtonClickedHandler(object sender, EventArgs e)
		{
			this.ColorButton.ColorClick();
		}

		private void color_ColorSet(object sender, EventArgs e)
		{
			this.MultilShow = false;
			this.Color = this.ColorButton.ColorValue;
			if (this.ColorChanged != null)
			{
				this.ColorChanged(this, new ColorExEvent(this.ColorButton.ColorValue));
			}
		}

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

		public void SetColor(Gdk.Color c)
		{
			this.ColorButton.CurrentColor = c;
			string text = this.ColorButton.ColorValue.Name.ToUpper();
			this.entry.Text = string.Format("#{0}", text.Substring(2, text.Length - 2));
			this.oldStr = this.entry.Text;
		}

		public event EventHandler<ColorExEvent> ColorChanged;

		private TextEntry entry;

		private string oldStr = "";
	}
}
