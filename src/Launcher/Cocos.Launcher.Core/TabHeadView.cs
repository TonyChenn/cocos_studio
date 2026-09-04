using System;
using Cocos.Launcher.Control;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000050 RID: 80
	public class TabHeadView : EventBox, ITabHead
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000A847 File Offset: 0x00008A47
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x0000A854 File Offset: 0x00008A54
		public string HeadName
		{
			get
			{
				return this.label.Text;
			}
			set
			{
				this.label.Text = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000A862 File Offset: 0x00008A62
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x0000A86A File Offset: 0x00008A6A
		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
			set
			{
				this.SetSelecetItemStyle(value);
				this.isSelected = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000A87A File Offset: 0x00008A7A
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0000A882 File Offset: 0x00008A82
		public bool IsShowRed
		{
			get
			{
				return this.isShowRed;
			}
			set
			{
				this.isShowRed = value;
				this.ShowRed(value);
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000A894 File Offset: 0x00008A94
		public TabHeadView()
		{
			base.WidthRequest = 140;
			base.HeightRequest = 40;
			base.CanFocus = true;
			base.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainLeftColor);
			HBox hbox = new HBox();
			hbox.Spacing = 0;
			EventBox eventBox = new EventBox();
			eventBox.WidthRequest = 4;
			eventBox.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainLeftColor);
			hbox.PackStart(eventBox, false, false, 0U);
			Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
			if (LanguageOption.CurrentLanguage == LanguageType.Chinese || LanguageOption.CurrentLanguage == LanguageType.Traditional)
			{
				alignment.WidthRequest = 48;
			}
			else
			{
				alignment.WidthRequest = 10;
			}
			hbox.PackStart(alignment, false, false, 0U);
			this.label = new Label();
			this.label.ModifyFg(StateType.Normal, ConstantConfig.Colors.ContentLabelColor1);
			this.label.SetFontSize(16.0);
			hbox.PackStart(this.label, false, false, 0U);
			hbox.PackStart(new Alignment(0.5f, 0.5f, 1f, 1f)
			{
				TopPadding = 8U,
				BottomPadding = 26U
			}, false, false, 0U);
			base.Add(hbox);
			base.ShowAll();
			eventBox.EnterNotifyEvent += this.eventbox_EnterNotifyEvent;
			eventBox.LeaveNotifyEvent += this.eventbox_LeaveNotifyEvent;
			base.EnterNotifyEvent += this.eventbox_EnterNotifyEvent;
			base.LeaveNotifyEvent += this.eventbox_LeaveNotifyEvent;
			base.ButtonPressEvent += this.TabHeadView_ButtonPressEvent;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000AA2C File Offset: 0x00008C2C
		private void ShowRed(bool isShow)
		{
			if (this.image == null)
			{
				this.image = new ImageBin();
				this.image.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.redDot.png"));
				this.image.ShowAll();
			}
			Alignment alignment = (base.Children[0] as HBox).Children[3] as Alignment;
			if (isShow && alignment.Children.Length == 0)
			{
				alignment.Add(this.image);
				return;
			}
			if (!isShow && alignment.Children.Length != 0)
			{
				alignment.Remove(alignment.Children[0]);
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000AABC File Offset: 0x00008CBC
		public void SetNumber(int number)
		{
			if (this.numberButton == null)
			{
				this.ShowPluginLogo(true);
			}
			if (number <= 0)
			{
				this.ShowPluginLogo(false);
				return;
			}
			if (number < 10)
			{
				this.numberButton.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.pluginNum_1.png");
				this.numberButton.SetSize(14, 14);
			}
			else
			{
				this.numberButton.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.pluginNum_2.png");
				this.numberButton.SetSize(20, 14);
			}
			this.numberButton.SetLabelBoldText(number.ToString());
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000AB3C File Offset: 0x00008D3C
		private void ChangeBgColor(Color color)
		{
			if (this.IsSelected)
			{
				return;
			}
			base.ModifyBg(StateType.Normal, color);
			EventBox eventBox = (base.Children[0] as HBox).Children[0] as EventBox;
			eventBox.ModifyBg(StateType.Normal, color);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000AB7C File Offset: 0x00008D7C
		private void SetSelecetItemStyle(bool isSelected)
		{
			Color color;
			Color color2;
			Color color3;
			if (isSelected)
			{
				color = ConstantConfig.Colors.MainContentColor;
				color2 = ConstantConfig.Colors.TabFontPressColor;
				color3 = ConstantConfig.Colors.TabFontPressColor;
			}
			else
			{
				color = ConstantConfig.Colors.MainLeftColor;
				color2 = ConstantConfig.Colors.MainLeftColor;
				color3 = ConstantConfig.Colors.ContentLabelColor1;
			}
			base.ModifyBg(StateType.Normal, color);
			EventBox eventBox = (base.Children[0] as HBox).Children[0] as EventBox;
			eventBox.ModifyBg(StateType.Normal, color2);
			Label label = (base.Children[0] as HBox).Children[2] as Label;
			label.ModifyFg(StateType.Normal, color3);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000AC20 File Offset: 0x00008E20
		private void ShowPluginLogo(bool isShow)
		{
			if (this.numberButton == null)
			{
				this.numberButton = new ButtonView();
				this.numberButton.SetLableFontSize(11.0);
				this.numberButton.SetLableNormalColor(ConstantConfig.Colors.MainContentColor);
				this.numberButton.SetLabelUseMarkup(true);
				this.numberButton.SetLabelBoldText("0");
				this.numberButton.SetSize(14, 14);
				this.numberButton.SetNormalBack("Cocos.Launcher.Image.pluginNum_1.png");
			}
			Alignment alignment = (base.Children[0] as HBox).Children[3] as Alignment;
			alignment.LeftPadding = 4U;
			if (isShow && alignment.Children.Length == 0)
			{
				alignment.Add(this.numberButton);
			}
			else if (!isShow && alignment.Children.Length != 0)
			{
				alignment.Remove(alignment.Children[0]);
				this.numberButton = null;
			}
			base.ShowAll();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000AD08 File Offset: 0x00008F08
		private void eventbox_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			this.ChangeBgColor(ConstantConfig.Colors.MainLeftColor);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000AD1A File Offset: 0x00008F1A
		private void eventbox_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			this.ChangeBgColor(ConstantConfig.Colors.TabEnterBgColor);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000AD2C File Offset: 0x00008F2C
		private void TabHeadView_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			base.HasFocus = true;
			if (this.IsSelected)
			{
				return;
			}
			this.RaiseSelectedChanged();
			args.RetVal = true;
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060002B0 RID: 688 RVA: 0x0000AD50 File Offset: 0x00008F50
		// (remove) Token: 0x060002B1 RID: 689 RVA: 0x0000AD88 File Offset: 0x00008F88
		public event EventHandler<SelectedChangingEventArgs> SelectedChanging;

		// Token: 0x060002B2 RID: 690 RVA: 0x0000ADBD File Offset: 0x00008FBD
		private void RaiseSelectedChanged()
		{
			if (this.SelectedChanging != null)
			{
				this.SelectedChanging(this, new SelectedChangingEventArgs(true));
			}
		}

		// Token: 0x04000101 RID: 257
		private Label label;

		// Token: 0x04000102 RID: 258
		private bool isSelected;

		// Token: 0x04000103 RID: 259
		private ImageBin image;

		// Token: 0x04000104 RID: 260
		private ButtonView numberButton;

		// Token: 0x04000105 RID: 261
		private bool isShowRed;
	}
}
