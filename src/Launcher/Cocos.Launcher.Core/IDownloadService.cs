using System;

namespace Cocos.Launcher.Core
{
	public interface IDownloadService
	{
		void Download(string xmlUrl, int x, int y);

		void Download(string xmlUrl);

		void SwitchPageOne(bool switchPage);
	}
}
