using System;
using Cocos.Launcher.Control;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000037 RID: 55
	public class NetWorkErrorView : EventBox
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x00008FA2 File Offset: 0x000071A2
		public NetWorkErrorView(string imagePath)
		{
			this.Initialize(imagePath);
			base.ShowAll();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008FB8 File Offset: 0x000071B8
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

		// Token: 0x060001F5 RID: 501 RVA: 0x00009089 File Offset: 0x00007289
		private void button_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			CocoStudio.Core.Services.NetworkService.TryRequest();
		}
	}
}
