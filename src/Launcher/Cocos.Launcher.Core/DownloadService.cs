using System;

namespace Cocos.Launcher.Core
{
	internal class DownloadService : IDownloadService
	{
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

		public PageManager AssetManager
		{
			get
			{
				return PageManager.Instance;
			}
		}

		public void Download(string xmlUrl, int x, int y)
		{
			this.AssetManager.DownloadItem(xmlUrl, x, y);
		}

		public void Download(string xmlUrl)
		{
			this.AssetManager.DownloadItem(xmlUrl);
		}

		public void SwitchPageOne(bool switchPage)
		{
			this.DownloadWidget.SwitchPageOne(switchPage);
		}

		private IDownloadView downloadWidget;

		private static DownloadService instance;
	}
}
