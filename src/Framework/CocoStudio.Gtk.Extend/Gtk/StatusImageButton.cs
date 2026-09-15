using System;
using Gdk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace Gtk
{
	public class StatusImageButton : EventBox
	{
		public new bool Sensitive
		{
			get
			{
				return this.hbox != null && this.hbox.Sensitive;
			}
			set
			{
				if (this.hbox != null)
				{
					this.hbox.Sensitive = value;
				}
			}
		}

		public StatusImageButton(string firstText, string firstTooltipText, Xwt.Drawing.Image firstImage, string secondText, string SecondTooltipText, Xwt.Drawing.Image secondImage)
		{
			this.FirImage = firstImage;
			this.FirText = firstText;
			this.SecImage = secondImage;
			this.SecText = secondText;
			this.FirTooltipText = firstTooltipText;
			this.SecTooltipText = SecondTooltipText;
			base.WidthRequest = 80;
			base.ModifyBg(StateType.Normal, WindowStyle.CheckedDark);
			this.eventBox = new EventBox();
			base.Add(this.eventBox);
			this.eventBox.ModifyBg(StateType.Normal, WindowStyle.WindowPanelColor);
			this.eventBox.BorderWidth = 1U;
			this.hbox = new HBox();
			this.hbox.Spacing = 6;
			this.imageView = new ImageView();
			this.imageView.Image = firstImage;
			this.hbox.PackStart(this.imageView, false, false, 0U);
			this.textLabel = new Label();
			this.textLabel.Text = firstText;
			this.hbox.PackStart(this.textLabel, false, false, 0U);
			this.eventBox.Add(this.hbox);
			base.ShowAll();
			base.TooltipText = firstTooltipText;
			base.ButtonPressEvent += this.StatusImageButton_ButtonPressEvent;
			base.ButtonReleaseEvent += this.StatusImageButton_ButtonReleaseEvent;
		}

		private void StatusImageButton_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (this.Sensitive)
			{
				this.eventBox.ModifyBg(StateType.Normal, new Gdk.Color(40, 40, 42));
			}
		}

		private void StatusImageButton_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (this.Sensitive)
			{
				this.eventBox.ModifyBg(StateType.Normal, WindowStyle.WindowPanelColor);
				if (this.CurrentChoice == 0)
				{
					this.CurrentChoice = 1;
				}
				else
				{
					this.CurrentChoice = 0;
				}
			}
		}

		public Xwt.Drawing.Image FirImage { get; private set; }

		public Xwt.Drawing.Image SecImage { get; private set; }

		public string FirText { get; private set; }

		public string SecText { get; private set; }

		public string FirTooltipText { get; private set; }

		public string SecTooltipText { get; private set; }

		public int CurrentChoice
		{
			get
			{
				return this._currentChoice;
			}
			set
			{
				this._currentChoice = value;
				if (value == 1)
				{
					this.imageView.Image = this.SecImage;
					base.TooltipText = this.SecTooltipText;
					this.textLabel.Text = this.SecText;
				}
				else
				{
					this.imageView.Image = this.FirImage;
					this.textLabel.Text = this.FirText;
					base.TooltipText = this.FirTooltipText;
				}
			}
		}

		private HBox hbox;

		private ImageView imageView;

		private Label textLabel;

		private EventBox eventBox;

		private int _currentChoice = 0;
	}
}
