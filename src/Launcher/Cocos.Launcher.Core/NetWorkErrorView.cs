using System;
using Cocos.Launcher.Control;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	public class NetWorkErrorView : EventBox
	{
		public NetWorkErrorView(string imagePath)
		{
			this.Initialize(imagePath);
			base.ShowAll();
		}

		private void Initialize(string imagePath)
		{
			Fixed @fixed = new Fixed();
			@fixed.HasWindow = false;
			base.Add(@fixed);
			ImageBin imageBin = new ImageBin();
			imageBin.SetImageView(ImageIcon.GetIcon(imagePath));
			@fixed.Add(imageBin);
			ButtonView buttonView = new ButtonView();
			buttonView.SetNormalBack("Cocos.Launcher.Resource.LauncherResource.newNormal.png");
			buttonView.SetMoveBack("Cocos.Launcher.Resource.LauncherResource.newMove.png");
			buttonView.SetPressBack("Cocos.Launcher.Resource.LauncherResource.newPress.png");
			buttonView.SetLableNormalColor(ConstantConfig.Colors.MainLeftColor);
			buttonView.SetSize(80, 26);
			buttonView.SetLabelText(LanguageInfo.Launcher_ClickRefresh);
			buttonView.SetLableFontSize(14.0);
			@fixed.Add(buttonView);
			Fixed.FixedChild fixedChild = (Fixed.FixedChild)@fixed[buttonView];
			fixedChild.X = 435;
			fixedChild.Y = 515;
			buttonView.ButtonReleaseEvent += this.button_ButtonReleaseEvent;
		}

		private void button_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			CocoStudio.Core.Services.NetworkService.TryRequest();
		}
	}
}
