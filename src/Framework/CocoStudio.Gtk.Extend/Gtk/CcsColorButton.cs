using System;
using System.ComponentModel;
using System.Drawing;
using Gdk;
using Stetic;

namespace Gtk
{
	[ToolboxItem(true)]
	public class CcsColorButton : Bin
	{
		public Gdk.Color CurrentColor
		{
			get
			{
				return this.currentColor;
			}
			set
			{
				this.currentColor = value;
				this.evtbx_color.ModifyBg(StateType.Normal, value);
			}
		}

		public System.Drawing.Color ColorValue
		{
			get
			{
				int red = (int)(255f * (float)this.CurrentColor.Red / 65535f);
				int green = (int)(255f * (float)this.CurrentColor.Green / 65535f);
				int blue = (int)(255f * (float)this.CurrentColor.Blue / 65535f);
				return System.Drawing.Color.FromArgb(255, red, green, blue);
			}
			set
			{
				this.CurrentColor = new Gdk.Color(value.R, value.G, value.B);
			}
		}

		public event EventHandler<ColorSetEventArgs> ColorSet;

		public CcsColorButton()
		{
			this.Build();
			this.CurrentColor = new Gdk.Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);
			this.SetButtonStyle(CcsColorButton.BtnState.Normal);
		}

		public CcsColorButton(Gdk.Color initColor)
		{
			this.Build();
			this.CurrentColor = initColor;
			this.SetButtonStyle(CcsColorButton.BtnState.Normal);
		}

		private void SetButtonStyle(CcsColorButton.BtnState state)
		{
			switch (state)
			{
			case CcsColorButton.BtnState.Normal:
				this.evtbx_border.ModifyBg(StateType.Normal, this.BlackNormalBorder);
				this.evtbx_bg.ModifyBg(StateType.Normal, this.GrayNormalBg);
				break;
			case CcsColorButton.BtnState.Hover:
				this.evtbx_border.ModifyBg(StateType.Normal, this.BlueHoverBorder);
				this.evtbx_bg.ModifyBg(StateType.Normal, this.GrayNormalBg);
				break;
			case CcsColorButton.BtnState.Pressed:
				this.evtbx_border.ModifyBg(StateType.Normal, this.BlackPressedBorder);
				this.evtbx_bg.ModifyBg(StateType.Normal, this.GrayPressedBg);
				break;
			}
		}

		protected void OnMouseEnter(object o, EnterNotifyEventArgs args)
		{
			this.isMouseIn = true;
			this.SetButtonStyle(CcsColorButton.BtnState.Hover);
		}

		protected void OnMouseLeave(object o, LeaveNotifyEventArgs args)
		{
			if (args.Event.Detail != NotifyType.Inferior)
			{
				this.isMouseIn = false;
				this.SetButtonStyle(CcsColorButton.BtnState.Normal);
			}
		}

		protected void OnButtonPress(object o, ButtonPressEventArgs args)
		{
			this.isInWhenPressed = true;
			this.SetButtonStyle(CcsColorButton.BtnState.Pressed);
		}

		protected void OnButtonRelease(object o, ButtonReleaseEventArgs args)
		{
			if (this.isMouseIn)
			{
				if (this.isInWhenPressed)
				{
					this.SetButtonStyle(CcsColorButton.BtnState.Normal);
					this.ColorClick();
				}
				else
				{
					this.SetButtonStyle(CcsColorButton.BtnState.Hover);
				}
			}
			else
			{
				this.SetButtonStyle(CcsColorButton.BtnState.Normal);
			}
			this.isInWhenPressed = false;
		}

		public void ColorClick()
		{
			ColorPickerDialog colorPickerDialog = new ColorPickerDialog(this.CurrentColor);
			if (colorPickerDialog.Run() == -5)
			{
				this.CurrentColor = colorPickerDialog.ColorPicker.CurrentColor;
				if (this.ColorSet != null)
				{
					this.ColorSet(this, new ColorSetEventArgs(this.CurrentColor));
				}
			}
			colorPickerDialog.Destroy();
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Gtk.CcsColorButton";
			this.evtbx_border = new EventBox();
			this.evtbx_border.Name = "evtbx_border";
			this.evtbx_bg = new EventBox();
			this.evtbx_bg.Name = "evtbx_bg";
			this.evtbx_bg.BorderWidth = 1U;
			this.evtbx_color = new EventBox();
			this.evtbx_color.Name = "evtbx_color";
			this.evtbx_color.BorderWidth = 3U;
			this.evtbx_bg.Add(this.evtbx_color);
			this.evtbx_border.Add(this.evtbx_bg);
			base.Add(this.evtbx_border);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.evtbx_border.EnterNotifyEvent += this.OnMouseEnter;
			this.evtbx_border.LeaveNotifyEvent += this.OnMouseLeave;
			this.evtbx_border.ButtonPressEvent += this.OnButtonPress;
			this.evtbx_border.ButtonReleaseEvent += this.OnButtonRelease;
		}

		private Gdk.Color BlackNormalBorder = new Gdk.Color(38, 38, 40);

		private Gdk.Color BlackPressedBorder = new Gdk.Color(37, 37, 38);

		private Gdk.Color BlueHoverBorder = new Gdk.Color(29, 75, 120);

		private Gdk.Color GrayNormalBg = new Gdk.Color(75, 75, 82);

		private Gdk.Color GrayPressedBg = new Gdk.Color(61, 61, 65);

		public bool isMouseIn = false;

		private bool isInWhenPressed = false;

		private Gdk.Color currentColor;

		private EventBox evtbx_border;

		private EventBox evtbx_bg;

		private EventBox evtbx_color;

		private enum BtnState
		{
			Normal,
			Hover,
			Pressed
		}
	}
}
