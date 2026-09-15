using System;
using Cocos.Launcher.Control;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	public class TabHeadView : EventBox, ITabHead
	{
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

		private void eventbox_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			this.ChangeBgColor(ConstantConfig.Colors.MainLeftColor);
		}

		private void eventbox_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			this.ChangeBgColor(ConstantConfig.Colors.TabEnterBgColor);
		}

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

		public event EventHandler<SelectedChangingEventArgs> SelectedChanging;

		private void RaiseSelectedChanged()
		{
			if (this.SelectedChanging != null)
			{
				this.SelectedChanging(this, new SelectedChangingEventArgs(true));
			}
		}

		private Label label;

		private bool isSelected;

		private ImageBin image;

		private ButtonView numberButton;

		private bool isShowRed;
	}
}
