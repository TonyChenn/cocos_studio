using System;
using System.ComponentModel;
using Gdk;
using MonoDevelop.Core;

namespace Gtk
{
	// Token: 0x02000012 RID: 18
	[ToolboxItem(true)]
	public class LabelLinkButton : BaseButton
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003CEC File Offset: 0x00001EEC
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00003D04 File Offset: 0x00001F04
		public Color? NormalColor
		{
			get
			{
				return this.normalColor;
			}
			set
			{
				this.normalColor = value;
				base.RefreshUI();
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003D18 File Offset: 0x00001F18
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00003D30 File Offset: 0x00001F30
		public Color? HoverColor
		{
			get
			{
				return this.hoverColor;
			}
			set
			{
				this.hoverColor = value;
				base.RefreshUI();
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003D44 File Offset: 0x00001F44
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00003D5C File Offset: 0x00001F5C
		public Color? PressedColor
		{
			get
			{
				return this.pressedColor;
			}
			set
			{
				this.pressedColor = value;
				base.RefreshUI();
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003D70 File Offset: 0x00001F70
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00003D88 File Offset: 0x00001F88
		public Color? DisabledColor
		{
			get
			{
				return this.disabledColor;
			}
			set
			{
				this.disabledColor = value;
				base.RefreshUI();
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003D9C File Offset: 0x00001F9C
		public Label Label
		{
			get
			{
				return this.label_display;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003DB4 File Offset: 0x00001FB4
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00003DD1 File Offset: 0x00001FD1
		public string LabelText
		{
			get
			{
				return this.Label.Text;
			}
			set
			{
				this.Label.Text = value;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003DE4 File Offset: 0x00001FE4
		public LabelLinkButton(string label = null)
		{
			if (!string.IsNullOrEmpty(label))
			{
				this.label_display = new Label(label);
			}
			else
			{
				this.label_display = new Label();
			}
			if (Platform.IsWindows)
			{
				this.label_display.SetFontSize(13.0);
			}
			else
			{
				this.label_display.SetFontSize(12.0);
			}
			base.Add(this.label_display);
			this.label_display.Show();
			base.RefreshUI();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003EB8 File Offset: 0x000020B8
		protected override void OnRefreshUI()
		{
			Color? color = null;
			switch (base.CurrentState)
			{
			case ButtonState.Normal:
				color = this.NormalColor;
				if (base.GdkWindow != null)
				{
					base.GdkWindow.Cursor = null;
				}
				break;
			case ButtonState.Hover:
				color = this.HoverColor;
				if (base.GdkWindow != null)
				{
					base.GdkWindow.Cursor = new Cursor(CursorType.Hand1);
				}
				break;
			case ButtonState.Pressed:
				color = this.PressedColor;
				break;
			case ButtonState.Disabled:
				color = this.DisabledColor;
				break;
			}
			if (color != null)
			{
				this.label_display.ModifyFg(StateType.Normal, color.Value);
			}
		}

		// Token: 0x04000032 RID: 50
		private Color? normalColor = new Color?(LabelStyleSetting.LabelNormalColor);

		// Token: 0x04000033 RID: 51
		private Color? hoverColor = new Color?(LabelStyleSetting.LabelHoverColor);

		// Token: 0x04000034 RID: 52
		private Color? pressedColor = new Color?(LabelStyleSetting.LabelPressedColor);

		// Token: 0x04000035 RID: 53
		private Color? disabledColor = new Color?(LabelStyleSetting.LabelDisabledColor);

		// Token: 0x04000036 RID: 54
		private Label label_display;
	}
}
