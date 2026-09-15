using System;
using Gdk;

namespace Gtk
{
	public sealed class GeneralLauncherButton : ImageLabelButton, IDialogButton
	{
		public EnumMainButton ButtonType { get; set; }

		public string Text
		{
			get
			{
				return base.LabelText;
			}
			set
			{
				base.LabelText = value;
			}
		}

		public GeneralLauncherButton()
		{
			this.SetButtonStyle(false);
			base.DisabledImage = ImageIcon.GetIcon("Gtk.Resource.Button.launcherBtn_disable.png");
			base.WidthRequest = 80;
			base.HeightRequest = 26;
			this.ButtonType = EnumMainButton.Yes;
			base.HasFocus = false;
			base.CanFocus = false;
			base.KeyPressEvent += this.HandleKeyPressedEvent;
		}

		public void SetButtonStyle(bool isMainBtn)
		{
			this.isMainButton = isMainBtn;
			if (this.isMainButton)
			{
				base.NormalImage = ImageIcon.GetIcon("Gtk.Resource.Button.launcherBtn_blue_normal.png");
				base.HoverImage = ImageIcon.GetIcon("Gtk.Resource.Button.launcherBtn_blue_hover.png");
				base.PressedImage = ImageIcon.GetIcon("Gtk.Resource.Button.launcherBtn_blue_pressed.png");
				this.SetTextColor(new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue));
			}
			else
			{
				base.NormalImage = ImageIcon.GetIcon("Gtk.Resource.Button.launcherBtn_white_normal.png");
				base.HoverImage = ImageIcon.GetIcon("Gtk.Resource.Button.launcherBtn_white_hover.png");
				base.PressedImage = ImageIcon.GetIcon("Gtk.Resource.Button.launcherBtn_white_pressed.png");
				this.SetTextColor(new Color(41, 41, 41));
			}
		}

		private void SetTextColor(Color color)
		{
			base.Label.ModifyFg(StateType.Normal, color);
		}

		public Widget GetWidget()
		{
			return this;
		}

		protected override void OnRefreshUI()
		{
			base.OnRefreshUI();
			if (!base.Sensitive)
			{
				this.SetTextColor(new Color(41, 41, 41));
			}
		}

		private void HandleKeyPressedEvent(object o, KeyPressEventArgs args)
		{
			if (KeyboardExtend.IsEnterKey(args.Event.Key) || args.Event.Key == Gdk.Key.space)
			{
				base.RaiseClickedEvent(new ButtonReleaseEventArgs());
			}
		}

		private bool isMainButton = false;
	}
}
