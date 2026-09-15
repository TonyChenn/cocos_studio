using System;
using System.ComponentModel;
using Gdk;
using Gtk;

namespace Cocos.Launcher.Control
{
	[ToolboxItem(true)]
	public class CheckboxView : EventBox
	{
		public event EventHandler<EventArgs> Clicked;

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

		public CheckboxView()
		{
			this.InitView();
		}

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

		public CheckboxView(string checkImagePath, string unCheckImagePath)
		{
			this.checkImagePath = checkImagePath;
			this.unCheckImagePath = unCheckImagePath;
			this.InitView();
		}

		public void SetLableColor(Color color)
		{
			this.label_checkbox.ModifyFg(StateType.Normal, color);
		}

		public void SetFontSize(double size)
		{
			this.label_checkbox.SetFontSize(size);
		}

		public void SetToolTips(string checkTip, string uncheckTip)
		{
			this.checkTip = checkTip;
			this.uncheckTip = uncheckTip;
		}

		private void InitEvent()
		{
			base.ButtonReleaseEvent += this.LauncherCheckbox_ButtonReleaseEvent;
			base.EnterNotifyEvent += this.LauncherCheckbox_EnterNotifyEvent;
			base.LeaveNotifyEvent += this.LauncherCheckbox_LeaveNotifyEvent;
		}

		private void LauncherCheckbox_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			base.GdkWindow.Cursor = null;
		}

		private void LauncherCheckbox_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			base.GdkWindow.Cursor = new Cursor(CursorType.Hand1);
		}

		private void LauncherCheckbox_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.Active = !this.Active;
			}
			base.ShowAll();
		}

		private HBox hbox2;

		private ImageBin image_checkBox;

		private Label label_checkbox;

		private string checkImagePath = "Cocos.Launcher.Resource.LauncherResource.checkBox2.png";

		private string unCheckImagePath = "Cocos.Launcher.Resource.LauncherResource.checkBox1.png";

		private string checkTip = string.Empty;

		private string uncheckTip = string.Empty;

		private bool active;

		private string label = string.Empty;
	}
}
