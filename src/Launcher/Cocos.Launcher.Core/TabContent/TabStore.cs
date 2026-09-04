using System;
using Cocos.Launcher.Control;
using Cocos.Launcher.Core.ExtensionModel;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Cocos.Launcher.Core.TabContent
{
	// Token: 0x02000047 RID: 71
	[Extension(Type = typeof(ITabContent))]
	internal class TabStore : BaseWebTabContent
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00009EA1 File Offset: 0x000080A1
		public override int Order
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009EA4 File Offset: 0x000080A4
		protected override void OnInitialize(ITabHead tabHead)
		{
			this.tabHead = tabHead;
			this.tabHead.HeadName = LanguageInfo.Launcher_CocosStore;
			this.netWorkErrorView = new NetWorkErrorView(ConstantConfig.Constant.Store404);
			this.initialUrl = ConstantConfig.Constant.StoreUrl;
			Services.UpdateService.UpdateChanged += this.UpdateService_UpdateChanged;
			Services.LoginService.LoginChanged += this.LoginService_LoginChanged;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009F19 File Offset: 0x00008119
		private void UpdateService_UpdateChanged(object sender, EventArgs e)
		{
			this.updateInfo = Services.UpdateService.StoreUpdateInfo;
			this.tabHead.IsShowRed = this.updateInfo.IsUpdate;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009F41 File Offset: 0x00008141
		protected override void OnActivated(SwitchTabInfo switchTabInfo)
		{
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.SetPostData();
			}
			base.OnActivated(switchTabInfo);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009F5C File Offset: 0x0000815C
		private void SetPostData()
		{
			string postDataToString = UserJsonInfo.Instance.GetPostDataToString();
			if (postDataToString != null)
			{
				base.WebView.PostData(postDataToString);
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00009F83 File Offset: 0x00008183
		private void LoginService_LoginChanged(object sender, LoginChangedEventArgs e)
		{
			if (this.tabHead.IsSelected)
			{
				this.SetPostData();
				base.WebView.Url = this.initialUrl;
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00009FA9 File Offset: 0x000081A9
		protected override void OnWebNewWindow(object sender, WebNewWindowEventArgs e)
		{
			Services.DownloadService.Download(e.Url, e.X, e.Y);
		}
	}
}
