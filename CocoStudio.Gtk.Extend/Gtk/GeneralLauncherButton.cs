using System;
using Gdk;

namespace Gtk
{
	// Token: 0x0200006F RID: 111
	public sealed class GeneralLauncherButton : ImageLabelButton, IDialogButton
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000A6AC File Offset: 0x000088AC
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000A6C3 File Offset: 0x000088C3
		public EnumMainButton ButtonType { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000A6CC File Offset: 0x000088CC
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000A6E4 File Offset: 0x000088E4
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

		// Token: 0x06000288 RID: 648 RVA: 0x0000A6F0 File Offset: 0x000088F0
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

		// Token: 0x06000289 RID: 649 RVA: 0x0000A764 File Offset: 0x00008964
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

		// Token: 0x0600028A RID: 650 RVA: 0x0000A81F File Offset: 0x00008A1F
		private void SetTextColor(Color color)
		{
			base.Label.ModifyFg(StateType.Normal, color);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000A830 File Offset: 0x00008A30
		public Widget GetWidget()
		{
			return this;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000A844 File Offset: 0x00008A44
		protected override void OnRefreshUI()
		{
			base.OnRefreshUI();
			if (!base.Sensitive)
			{
				this.SetTextColor(new Color(41, 41, 41));
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000A878 File Offset: 0x00008A78
		private void HandleKeyPressedEvent(object o, KeyPressEventArgs args)
		{
			if (KeyboardExtend.IsEnterKey(args.Event.Key) || args.Event.Key == Gdk.Key.space)
			{
				base.RaiseClickedEvent(new ButtonReleaseEventArgs());
			}
		}

		// Token: 0x04000331 RID: 817
		private bool isMainButton = false;
	}
}
