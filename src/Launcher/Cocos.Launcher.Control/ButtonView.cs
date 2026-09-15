using System;
using System.ComponentModel;
using Gdk;
using Gtk;

namespace Cocos.Launcher.Control
{
	[ToolboxItem(true)]
	public class ButtonView : EventBox
	{
		public bool IsSensitive
		{
			get
			{
				return this.isSensitive;
			}
			set
			{
				if (this.isSensitive == value)
				{
					return;
				}
				this.isSensitive = value;
				if (this.normalNotifyPath != null && this.isEditNotifyPath != null)
				{
					if (value)
					{
						this.image_button.SetImageView(ImageIcon.GetIcon(this.normalNotifyPath));
					}
					else
					{
						this.image_button.SetImageView(ImageIcon.GetIcon(this.isEditNotifyPath));
					}
					base.Sensitive = value;
				}
			}
		}

		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
			set
			{
				if (this.isSelected == value)
				{
					return;
				}
				this.isSelected = value;
				if (this.normalNotifyPath != null && this.selectNotifyPath != null)
				{
					if (value)
					{
						this.image_button.SetImageView(ImageIcon.GetIcon(this.selectNotifyPath));
						this.label_buttonText.ModifyFg(StateType.Normal, this.selectColor);
					}
					else
					{
						this.image_button.SetImageView(ImageIcon.GetIcon(this.normalNotifyPath));
						this.label_buttonText.ModifyFg(StateType.Normal, this.normalColor);
					}
					this.IsSelected = value;
				}
			}
		}

		public int SelectedIndex { get; set; }

		public ButtonView()
		{
			base.VisibleWindow = false;
			this.fixed1 = new Fixed();
			this.fixed1.HasWindow = false;
			this.image_button = new ImageBin();
			this.fixed1.Add(this.image_button);
			this.label_buttonText = new Label();
			this.label_buttonText.Name = "label_buttonText";
			this.fixed1.Add(this.label_buttonText);
			base.Add(this.fixed1);
			this.IsSensitive = true;
			this.InitEvent();
		}

		private void InitEvent()
		{
			base.EnterNotifyEvent += this.LauncherButton_EnterNotifyEvent;
			base.LeaveNotifyEvent += this.LauncherButton_LeaveNotifyEvent;
			base.ButtonPressEvent += this.LauncherButton_ButtonPressEvent;
		}

		public void SetSize(int width, int height)
		{
			base.SetSizeRequest(width, height);
			this.fixed1.SetSizeRequest(width, height);
			this.image_button.SetSizeRequest(width, height);
			this.label_buttonText.SetSizeRequest(width, height);
		}

		public void SetBackGroundColor(Color bgColor)
		{
			base.ModifyBg(StateType.Normal, bgColor);
		}

		public void SetLabelText(string text)
		{
			this.label_buttonText.Text = text;
		}

		public void SetLabelBoldText(string text)
		{
			this.label_buttonText.LabelProp = "<b>" + text + "</b>";
		}

		public void SetLabelUseMarkup(bool isUse)
		{
			this.label_buttonText.UseMarkup = isUse;
		}

		public void SetLabelAlign(float x, float y)
		{
			this.label_buttonText.Xalign = x;
			this.label_buttonText.Yalign = y;
		}

		public void SetNormalBack(string path)
		{
			this.normalNotifyPath = path;
			this.image_button.SetImageView(ImageIcon.GetIcon(path));
		}

		public void SetIsEditBack(string path)
		{
			this.isEditNotifyPath = path;
		}

		public void SetMoveBack(string path)
		{
			this.enterNotifyPath = path;
		}

		public void SetPressBack(string path)
		{
			this.pressNotifyPath = path;
		}

		public void SetSelectBack(string path)
		{
			this.selectNotifyPath = path;
		}

		public void SetLableNormalColor(Color color)
		{
			this.normalColor = color;
			this.label_buttonText.ModifyFg(StateType.Normal, color);
		}

		public void SetLableSelectColor(Color color)
		{
			this.selectColor = color;
		}

		public void SetLableFontSize(double fontSize)
		{
			this.label_buttonText.SetFontSize(fontSize);
		}

		public string GetLableText()
		{
			return this.label_buttonText.Text;
		}

		private void LauncherButton_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			if (!string.IsNullOrEmpty(this.normalNotifyPath) && !this.IsSelected)
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.normalNotifyPath));
			}
		}

		private void LauncherButton_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			if (!string.IsNullOrEmpty(this.enterNotifyPath) && !this.IsSelected)
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.enterNotifyPath));
			}
		}

		private void LauncherButton_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (!string.IsNullOrEmpty(this.pressNotifyPath) && !this.IsSelected)
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.pressNotifyPath));
			}
		}

		private Fixed fixed1;

		private ImageBin image_button;

		private Label label_buttonText;

		private string normalNotifyPath;

		private string enterNotifyPath;

		private string isEditNotifyPath;

		private string pressNotifyPath;

		private Color normalColor;

		private bool isSensitive;

		private Color selectColor;

		private string selectNotifyPath;

		private bool isSelected;
	}
}
