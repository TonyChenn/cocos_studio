using System;
using System.ComponentModel;
using Gdk;
using MonoDevelop.Core;

namespace Gtk
{
	[ToolboxItem(true)]
	public class LabelLinkButton : BaseButton
	{
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

		public Label Label
		{
			get
			{
				return this.label_display;
			}
		}

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

		private Color? normalColor = new Color?(LabelStyleSetting.LabelNormalColor);

		private Color? hoverColor = new Color?(LabelStyleSetting.LabelHoverColor);

		private Color? pressedColor = new Color?(LabelStyleSetting.LabelPressedColor);

		private Color? disabledColor = new Color?(LabelStyleSetting.LabelDisabledColor);

		private Label label_display;
	}
}
