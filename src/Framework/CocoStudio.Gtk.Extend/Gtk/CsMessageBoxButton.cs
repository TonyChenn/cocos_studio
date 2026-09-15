using System;

namespace Gtk
{
	internal class CsMessageBoxButton : Button, IDialogButton
	{
		public EnumMainButton ButtonType { get; set; }

		public string Text
		{
			get
			{
				return base.Label;
			}
			set
			{
				base.Label = value;
			}
		}

		public new event EventHandler<ButtonReleaseEventArgs> Clicked;

		public Widget GetWidget()
		{
			return this;
		}

		public CsMessageBoxButton()
		{
			base.Name = "DefaultButton";
			base.HasFocus = false;
			base.CanFocus = false;
		}

		protected override void OnClicked()
		{
			base.OnClicked();
			if (this.Clicked != null)
			{
				this.Clicked(this, new ButtonReleaseEventArgs());
			}
		}
	}
}
