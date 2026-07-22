using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200002C RID: 44
	public class PluginDownloadEventArgs : EventArgs
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000183 RID: 387 RVA: 0x000084B0 File Offset: 0x000066B0
		// (set) Token: 0x06000184 RID: 388 RVA: 0x000084B8 File Offset: 0x000066B8
		public Plugin PluginModel { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000185 RID: 389 RVA: 0x000084C1 File Offset: 0x000066C1
		// (set) Token: 0x06000186 RID: 390 RVA: 0x000084C9 File Offset: 0x000066C9
		public string OldDownloadUrl { get; private set; }

		// Token: 0x06000187 RID: 391 RVA: 0x000084D2 File Offset: 0x000066D2
		public PluginDownloadEventArgs(Plugin pluginModel, string url)
		{
			this.PluginModel = pluginModel;
			this.OldDownloadUrl = url;
		}
	}
}
