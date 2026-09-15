using System;
using Cocos.Launcher.Control;
using Cocos.Launcher.Core.ExtensionModel;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Cocos.Launcher.Core.TabContent
{
	[Extension(Type = typeof(ITabContent))]
	internal class TabStore : BaseWebTabContent
	{
		public override int Order
		{
			get
			{
				return 2;
			}
		}

		protected override void OnInitialize(ITabHead tabHead)
		{
			this.tabHead = tabHead;
			this.tabHead.HeadName = LanguageInfo.Launcher_CocosStore;
			this.netWorkErrorView = new NetWorkErrorView(ConstantConfig.Constant.Store404);
			this.initialUrl = ConstantConfig.Constant.StoreUrl;
			Services.UpdateService.UpdateChanged += this.UpdateService_UpdateChanged;
			Services.LoginService.LoginChanged += this.LoginService_LoginChanged;
		}

		private void UpdateService_UpdateChanged(object sender, EventArgs e)
		{
			this.updateInfo = Services.UpdateService.StoreUpdateInfo;
			this.tabHead.IsShowRed = this.updateInfo.IsUpdate;
		}

		protected override void OnActivated(SwitchTabInfo switchTabInfo)
		{
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.SetPostData();
			}
			base.OnActivated(switchTabInfo);
		}

		private void SetPostData()
		{
			string postDataToString = UserJsonInfo.Instance.GetPostDataToString();
			if (postDataToString != null)
			{
				base.WebView.PostData(postDataToString);
			}
		}

		private void LoginService_LoginChanged(object sender, LoginChangedEventArgs e)
		{
			if (this.tabHead.IsSelected)
			{
				this.SetPostData();
				base.WebView.Url = this.initialUrl;
			}
		}

		protected override void OnWebNewWindow(object sender, WebNewWindowEventArgs e)
		{
			Services.DownloadService.Download(e.Url, e.X, e.Y);
		}
	}
}
