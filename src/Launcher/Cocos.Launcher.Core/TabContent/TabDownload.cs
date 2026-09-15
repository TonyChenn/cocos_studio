using System;
using Cocos.Launcher.Core.ExtensionModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Cocos.Launcher.Core.TabContent
{
	[Extension(Type = typeof(ITabContent))]
	internal class TabDownload : BaseTabContent
	{
		public override int Order
		{
			get
			{
				return 3;
			}
		}

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

		protected override void OnInitialize(ITabHead tabHead)
		{
			tabHead.HeadName = LanguageInfo.Launcher_Downloads;
			DownloadService.Instance.AssetManager.SetTabHead(tabHead);
			tabHead.SetNumber(DownloadService.Instance.AssetManager.GetDoingDownloadNum());
		}

		protected override void OnActivated(SwitchTabInfo switchTabInfo)
		{
			DownloadService.Instance.DownloadWidget.Refresh();
			base.OnActivated(switchTabInfo);
		}

		private Widget content;
	}
}
