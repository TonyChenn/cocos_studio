using System;
using System.ComponentModel;
using Gdk;
using Gtk;

namespace Cocos.Launcher.Control
{
	// Token: 0x0200000B RID: 11
	[ToolboxItem(true)]
	public class ButtonView : EventBox
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000029DD File Offset: 0x00000BDD
		// (set) Token: 0x0600005C RID: 92 RVA: 0x000029E8 File Offset: 0x00000BE8
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002A4E File Offset: 0x00000C4E
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002A58 File Offset: 0x00000C58
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002AE2 File Offset: 0x00000CE2
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002AEA File Offset: 0x00000CEA
		public int SelectedIndex { get; set; }

		// Token: 0x06000061 RID: 97 RVA: 0x00002AF4 File Offset: 0x00000CF4
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

		// Token: 0x06000062 RID: 98 RVA: 0x00002B86 File Offset: 0x00000D86
		private void InitEvent()
		{
			base.EnterNotifyEvent += this.LauncherButton_EnterNotifyEvent;
			base.LeaveNotifyEvent += this.LauncherButton_LeaveNotifyEvent;
			base.ButtonPressEvent += this.LauncherButton_ButtonPressEvent;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002BBE File Offset: 0x00000DBE
		public void SetSize(int width, int height)
		{
			base.SetSizeRequest(width, height);
			this.fixed1.SetSizeRequest(width, height);
			this.image_button.SetSizeRequest(width, height);
			this.label_buttonText.SetSizeRequest(width, height);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002BEF File Offset: 0x00000DEF
		public void SetBackGroundColor(Color bgColor)
		{
			base.ModifyBg(StateType.Normal, bgColor);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002BF9 File Offset: 0x00000DF9
		public void SetLabelText(string text)
		{
			this.label_buttonText.Text = text;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002C07 File Offset: 0x00000E07
		public void SetLabelBoldText(string text)
		{
			this.label_buttonText.LabelProp = "<b>" + text + "</b>";
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002C24 File Offset: 0x00000E24
		public void SetLabelUseMarkup(bool isUse)
		{
			this.label_buttonText.UseMarkup = isUse;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002C32 File Offset: 0x00000E32
		public void SetLabelAlign(float x, float y)
		{
			this.label_buttonText.Xalign = x;
			this.label_buttonText.Yalign = y;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002C4C File Offset: 0x00000E4C
		public void SetNormalBack(string path)
		{
			this.normalNotifyPath = path;
			this.image_button.SetImageView(ImageIcon.GetIcon(path));
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002C66 File Offset: 0x00000E66
		public void SetIsEditBack(string path)
		{
			this.isEditNotifyPath = path;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002C6F File Offset: 0x00000E6F
		public void SetMoveBack(string path)
		{
			this.enterNotifyPath = path;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002C78 File Offset: 0x00000E78
		public void SetPressBack(string path)
		{
			this.pressNotifyPath = path;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002C81 File Offset: 0x00000E81
		public void SetSelectBack(string path)
		{
			this.selectNotifyPath = path;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002C8A File Offset: 0x00000E8A
		public void SetLableNormalColor(Color color)
		{
			this.normalColor = color;
			this.label_buttonText.ModifyFg(StateType.Normal, color);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002CA0 File Offset: 0x00000EA0
		public void SetLableSelectColor(Color color)
		{
			this.selectColor = color;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002CA9 File Offset: 0x00000EA9
		public void SetLableFontSize(double fontSize)
		{
			this.label_buttonText.SetFontSize(fontSize);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002CB7 File Offset: 0x00000EB7
		public string GetLableText()
		{
			return this.label_buttonText.Text;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002CC4 File Offset: 0x00000EC4
		private void LauncherButton_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			if (!string.IsNullOrEmpty(this.normalNotifyPath) && !this.IsSelected)
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.normalNotifyPath));
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002CF1 File Offset: 0x00000EF1
		private void LauncherButton_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			if (!string.IsNullOrEmpty(this.enterNotifyPath) && !this.IsSelected)
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.enterNotifyPath));
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002D1E File Offset: 0x00000F1E
		private void LauncherButton_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (!string.IsNullOrEmpty(this.pressNotifyPath) && !this.IsSelected)
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.pressNotifyPath));
			}
		}

		// Token: 0x04000030 RID: 48
		private Fixed fixed1;

		// Token: 0x04000031 RID: 49
		private ImageBin image_button;

		// Token: 0x04000032 RID: 50
		private Label label_buttonText;

		// Token: 0x04000033 RID: 51
		private string normalNotifyPath;

		// Token: 0x04000034 RID: 52
		private string enterNotifyPath;

		// Token: 0x04000035 RID: 53
		private string isEditNotifyPath;

		// Token: 0x04000036 RID: 54
		private string pressNotifyPath;

		// Token: 0x04000037 RID: 55
		private Color normalColor;

		// Token: 0x04000038 RID: 56
		private bool isSensitive;

		// Token: 0x04000039 RID: 57
		private Color selectColor;

		// Token: 0x0400003A RID: 58
		private string selectNotifyPath;

		// Token: 0x0400003B RID: 59
		private bool isSelected;
	}
}
