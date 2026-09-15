using System;

namespace Cocos.Launcher.Core
{
	public class PluginDownloadEventArgs : EventArgs
	{
		public Plugin PluginModel { get; private set; }

		public string OldDownloadUrl { get; private set; }

		public PluginDownloadEventArgs(Plugin pluginModel, string url)
		{
			this.PluginModel = pluginModel;
			this.OldDownloadUrl = url;
		}
	}
}
