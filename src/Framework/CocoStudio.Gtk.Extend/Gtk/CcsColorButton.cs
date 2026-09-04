using System;
using System.ComponentModel;
using System.Drawing;
using Gdk;
using Stetic;

namespace Gtk
{
	// Token: 0x02000097 RID: 151
	[ToolboxItem(true)]
	public class CcsColorButton : Bin
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000D5EC File Offset: 0x0000B7EC
		// (set) Token: 0x06000330 RID: 816 RVA: 0x0000D604 File Offset: 0x0000B804
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

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000D61C File Offset: 0x0000B81C
		// (set) Token: 0x06000332 RID: 818 RVA: 0x0000D689 File Offset: 0x0000B889
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

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000333 RID: 819 RVA: 0x0000D6B0 File Offset: 0x0000B8B0
		// (remove) Token: 0x06000334 RID: 820 RVA: 0x0000D6EC File Offset: 0x0000B8EC
		public event EventHandler<ColorSetEventArgs> ColorSet;

		// Token: 0x06000335 RID: 821 RVA: 0x0000D728 File Offset: 0x0000B928
		public CcsColorButton()
		{
			this.Build();
			this.CurrentColor = new Gdk.Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);
			this.SetButtonStyle(CcsColorButton.BtnState.Normal);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000D7CC File Offset: 0x0000B9CC
		public CcsColorButton(Gdk.Color initColor)
		{
			this.Build();
			this.CurrentColor = initColor;
			this.SetButtonStyle(CcsColorButton.BtnState.Normal);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000D85C File Offset: 0x0000BA5C
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

		// Token: 0x06000338 RID: 824 RVA: 0x0000D8F8 File Offset: 0x0000BAF8
		protected void OnMouseEnter(object o, EnterNotifyEventArgs args)
		{
			this.isMouseIn = true;
			this.SetButtonStyle(CcsColorButton.BtnState.Hover);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000D90C File Offset: 0x0000BB0C
		protected void OnMouseLeave(object o, LeaveNotifyEventArgs args)
		{
			if (args.Event.Detail != NotifyType.Inferior)
			{
				this.isMouseIn = false;
				this.SetButtonStyle(CcsColorButton.BtnState.Normal);
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000D93D File Offset: 0x0000BB3D
		protected void OnButtonPress(object o, ButtonPressEventArgs args)
		{
			this.isInWhenPressed = true;
			this.SetButtonStyle(CcsColorButton.BtnState.Pressed);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000D950 File Offset: 0x0000BB50
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

		// Token: 0x0600033C RID: 828 RVA: 0x0000D9AC File Offset: 0x0000BBAC
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

		// Token: 0x0600033D RID: 829 RVA: 0x0000DA1C File Offset: 0x0000BC1C
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

		// Token: 0x040003C3 RID: 963
		private Gdk.Color BlackNormalBorder = new Gdk.Color(38, 38, 40);

		// Token: 0x040003C4 RID: 964
		private Gdk.Color BlackPressedBorder = new Gdk.Color(37, 37, 38);

		// Token: 0x040003C5 RID: 965
		private Gdk.Color BlueHoverBorder = new Gdk.Color(29, 75, 120);

		// Token: 0x040003C6 RID: 966
		private Gdk.Color GrayNormalBg = new Gdk.Color(75, 75, 82);

		// Token: 0x040003C7 RID: 967
		private Gdk.Color GrayPressedBg = new Gdk.Color(61, 61, 65);

		// Token: 0x040003C8 RID: 968
		public bool isMouseIn = false;

		// Token: 0x040003C9 RID: 969
		private bool isInWhenPressed = false;

		// Token: 0x040003CA RID: 970
		private Gdk.Color currentColor;

		// Token: 0x040003CC RID: 972
		private EventBox evtbx_border;

		// Token: 0x040003CD RID: 973
		private EventBox evtbx_bg;

		// Token: 0x040003CE RID: 974
		private EventBox evtbx_color;

		// Token: 0x02000098 RID: 152
		private enum BtnState
		{
			// Token: 0x040003D0 RID: 976
			Normal,
			// Token: 0x040003D1 RID: 977
			Hover,
			// Token: 0x040003D2 RID: 978
			Pressed
		}
	}
}
