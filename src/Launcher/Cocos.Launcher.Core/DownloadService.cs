using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200002A RID: 42
	internal class DownloadService : IDownloadService
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000840A File Offset: 0x0000660A
		public IDownloadView DownloadWidget
		{
			get
			{
				if (this.downloadWidget == null)
				{
					this.downloadWidget = new DownloadView();
				}
				return this.downloadWidget;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00008425 File Offset: 0x00006625
		public static DownloadService Instance
		{
			get
			{
				if (DownloadService.instance == null)
				{
					DownloadService.instance = new DownloadService();
				}
				return DownloadService.instance;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000179 RID: 377 RVA: 0x0000843D File Offset: 0x0000663D
		public PageManager AssetManager
		{
			get
			{
				return PageManager.Instance;
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008444 File Offset: 0x00006644
		public void Download(string xmlUrl, int x, int y)
		{
			this.AssetManager.DownloadItem(xmlUrl, x, y);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008454 File Offset: 0x00006654
		public void Download(string xmlUrl)
		{
			this.AssetManager.DownloadItem(xmlUrl);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008462 File Offset: 0x00006662
		public void SwitchPageOne(bool switchPage)
		{
			this.DownloadWidget.SwitchPageOne(switchPage);
		}

		// Token: 0x04000075 RID: 117
		private IDownloadView downloadWidget;

		// Token: 0x04000076 RID: 118
		private static DownloadService instance;
	}
}
