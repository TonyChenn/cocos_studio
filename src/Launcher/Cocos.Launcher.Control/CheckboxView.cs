using System;
using System.ComponentModel;
using Gdk;
using Gtk;

namespace Cocos.Launcher.Control
{
	// Token: 0x0200000C RID: 12
	[ToolboxItem(true)]
	public class CheckboxView : EventBox
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000075 RID: 117 RVA: 0x00002D4C File Offset: 0x00000F4C
		// (remove) Token: 0x06000076 RID: 118 RVA: 0x00002D84 File Offset: 0x00000F84
		public event EventHandler<EventArgs> Clicked;

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002DB9 File Offset: 0x00000FB9
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00002DC4 File Offset: 0x00000FC4
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
				if (this.active)
				{
					this.image_checkBox.SetImageView(ImageIcon.GetIcon(this.checkImagePath));
					base.TooltipText = this.checkTip;
				}
				else
				{
					this.image_checkBox.SetImageView(ImageIcon.GetIcon(this.unCheckImagePath));
					base.TooltipText = this.uncheckTip;
				}
				this.image_checkBox.ShowAll();
				if (this.Clicked != null)
				{
					this.Clicked(this, null);
				}
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002E46 File Offset: 0x00001046
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00002E4E File Offset: 0x0000104E
		public string Label
		{
			get
			{
				return this.label;
			}
			set
			{
				this.label = value;
				this.label_checkbox.Text = this.Label;
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002E68 File Offset: 0x00001068
		public CheckboxView()
		{
			this.InitView();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002EB8 File Offset: 0x000010B8
		private void InitView()
		{
			this.hbox2 = new HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			this.image_checkBox = new ImageBin();
			this.image_checkBox.Name = "image1";
			this.hbox2.Add(this.image_checkBox);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox2[this.image_checkBox];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.label_checkbox = new Label();
			this.label_checkbox.Name = "label1";
			this.hbox2.Add(this.label_checkbox);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox2[this.label_checkbox];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			base.Add(this.hbox2);
			this.InitEvent();
			base.ShowAll();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002FB4 File Offset: 0x000011B4
		public CheckboxView(string checkImagePath, string unCheckImagePath)
		{
			this.checkImagePath = checkImagePath;
			this.unCheckImagePath = unCheckImagePath;
			this.InitView();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003012 File Offset: 0x00001212
		public void SetLableColor(Color color)
		{
			this.label_checkbox.ModifyFg(StateType.Normal, color);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003021 File Offset: 0x00001221
		public void SetFontSize(double size)
		{
			this.label_checkbox.SetFontSize(size);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000302F File Offset: 0x0000122F
		public void SetToolTips(string checkTip, string uncheckTip)
		{
			this.checkTip = checkTip;
			this.uncheckTip = uncheckTip;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000303F File Offset: 0x0000123F
		private void InitEvent()
		{
			base.ButtonReleaseEvent += this.LauncherCheckbox_ButtonReleaseEvent;
			base.EnterNotifyEvent += this.LauncherCheckbox_EnterNotifyEvent;
			base.LeaveNotifyEvent += this.LauncherCheckbox_LeaveNotifyEvent;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003077 File Offset: 0x00001277
		private void LauncherCheckbox_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			base.GdkWindow.Cursor = null;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003085 File Offset: 0x00001285
		private void LauncherCheckbox_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			base.GdkWindow.Cursor = new Cursor(CursorType.Hand1);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003099 File Offset: 0x00001299
		private void LauncherCheckbox_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.Active = !this.Active;
			}
			base.ShowAll();
		}

		// Token: 0x0400003D RID: 61
		private HBox hbox2;

		// Token: 0x0400003E RID: 62
		private ImageBin image_checkBox;

		// Token: 0x0400003F RID: 63
		private Label label_checkbox;

		// Token: 0x04000041 RID: 65
		private string checkImagePath = "Cocos.Launcher.Resource.LauncherResource.checkBox2.png";

		// Token: 0x04000042 RID: 66
		private string unCheckImagePath = "Cocos.Launcher.Resource.LauncherResource.checkBox1.png";

		// Token: 0x04000043 RID: 67
		private string checkTip = string.Empty;

		// Token: 0x04000044 RID: 68
		private string uncheckTip = string.Empty;

		// Token: 0x04000045 RID: 69
		private bool active;

		// Token: 0x04000046 RID: 70
		private string label = string.Empty;
	}
}
