using System;
using Cocos.Launcher.Control;
using Cocos.Launcher.Core.ExtensionModel;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000015 RID: 21
	[Extension(Type = typeof(ITabContent))]
	internal class TabDocument : BaseWebTabContent
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004EB3 File Offset: 0x000030B3
		public override int Order
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004EB8 File Offset: 0x000030B8
		protected override void OnInitialize(ITabHead tabHead)
		{
			this.tabHead = tabHead;
			this.tabHead.HeadName = LanguageInfo.Launcher_Document;
			this.netWorkErrorView = new NetWorkErrorView(ConstantConfig.Constant.Document404);
			this.initialUrl = ConstantConfig.Constant.DocumentUrl;
			this.isMenuEnabled = true;
			Services.UpdateService.UpdateChanged += this.UpdateService_UpdateChanged;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004F1E File Offset: 0x0000311E
		private void UpdateService_UpdateChanged(object sender, EventArgs e)
		{
			this.updateInfo = Services.UpdateService.TutorialsUpdateInfo;
			this.tabHead.IsShowRed = this.updateInfo.IsUpdate;
		}
	}
}
