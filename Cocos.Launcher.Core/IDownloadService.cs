using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000011 RID: 17
	public interface IDownloadService
	{
		// Token: 0x06000084 RID: 132
		void Download(string xmlUrl, int x, int y);

		// Token: 0x06000085 RID: 133
		void Download(string xmlUrl);

		// Token: 0x06000086 RID: 134
		void SwitchPageOne(bool switchPage);
	}
}
