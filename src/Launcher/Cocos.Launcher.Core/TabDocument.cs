using System;
using Cocos.Launcher.Control;
using Cocos.Launcher.Core.ExtensionModel;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Cocos.Launcher.Core
{
	[Extension(Type = typeof(ITabContent))]
	internal class TabDocument : BaseWebTabContent
	{
		public override int Order
		{
			get
			{
				return 1;
			}
		}

		protected override void OnInitialize(ITabHead tabHead)
		{
			this.tabHead = tabHead;
			this.tabHead.HeadName = LanguageInfo.Launcher_Document;
			this.netWorkErrorView = new NetWorkErrorView(ConstantConfig.Constant.Document404);
			this.initialUrl = ConstantConfig.Constant.DocumentUrl;
			this.isMenuEnabled = true;
			Services.UpdateService.UpdateChanged += this.UpdateService_UpdateChanged;
		}

		private void UpdateService_UpdateChanged(object sender, EventArgs e)
		{
			this.updateInfo = Services.UpdateService.TutorialsUpdateInfo;
			this.tabHead.IsShowRed = this.updateInfo.IsUpdate;
		}
	}
}
