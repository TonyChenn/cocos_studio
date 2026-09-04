using System;
using Gdk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace Gtk
{
	// Token: 0x0200006D RID: 109
	public class StatusImageButton : EventBox
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000A308 File Offset: 0x00008508
		// (set) Token: 0x0600026B RID: 619 RVA: 0x0000A33C File Offset: 0x0000853C
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

		// Token: 0x0600026C RID: 620 RVA: 0x0000A36C File Offset: 0x0000856C
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

		// Token: 0x0600026D RID: 621 RVA: 0x0000A4C4 File Offset: 0x000086C4
		private void StatusImageButton_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (this.Sensitive)
			{
				this.eventBox.ModifyBg(StateType.Normal, new Gdk.Color(40, 40, 42));
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000A4F8 File Offset: 0x000086F8
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

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000A54C File Offset: 0x0000874C
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000A563 File Offset: 0x00008763
		public Xwt.Drawing.Image FirImage { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000A56C File Offset: 0x0000876C
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000A583 File Offset: 0x00008783
		public Xwt.Drawing.Image SecImage { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000A58C File Offset: 0x0000878C
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000A5A3 File Offset: 0x000087A3
		public string FirText { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000A5AC File Offset: 0x000087AC
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000A5C3 File Offset: 0x000087C3
		public string SecText { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000A5CC File Offset: 0x000087CC
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000A5E3 File Offset: 0x000087E3
		public string FirTooltipText { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000A5EC File Offset: 0x000087EC
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000A603 File Offset: 0x00008803
		public string SecTooltipText { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000A60C File Offset: 0x0000880C
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000A624 File Offset: 0x00008824
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

		// Token: 0x04000326 RID: 806
		private HBox hbox;

		// Token: 0x04000327 RID: 807
		private ImageView imageView;

		// Token: 0x04000328 RID: 808
		private Label textLabel;

		// Token: 0x04000329 RID: 809
		private EventBox eventBox;

		// Token: 0x0400032A RID: 810
		private int _currentChoice = 0;
	}
}
