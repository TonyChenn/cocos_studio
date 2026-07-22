using System;
using Cocos.Launcher.Core.ExtensionModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Cocos.Launcher.Core.TabContent
{
	// Token: 0x02000042 RID: 66
	[Extension(Type = typeof(ITabContent))]
	internal class TabDownload : BaseTabContent
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00009AA0 File Offset: 0x00007CA0
		public override int Order
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00009AA3 File Offset: 0x00007CA3
		public override Widget Content
		{
			get
			{
				if (this.content == null)
				{
					this.content = (DownloadService.Instance.DownloadWidget as Widget);
					Services.DownloadService = DownloadService.Instance;
				}
				return this.content;
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00009AD2 File Offset: 0x00007CD2
		protected override void OnInitialize(ITabHead tabHead)
		{
			tabHead.HeadName = LanguageInfo.Launcher_Downloads;
			DownloadService.Instance.AssetManager.SetTabHead(tabHead);
			tabHead.SetNumber(DownloadService.Instance.AssetManager.GetDoingDownloadNum());
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00009B04 File Offset: 0x00007D04
		protected override void OnActivated(SwitchTabInfo switchTabInfo)
		{
			DownloadService.Instance.DownloadWidget.Refresh();
			base.OnActivated(switchTabInfo);
		}

		// Token: 0x040000E6 RID: 230
		private Widget content;
	}
}
