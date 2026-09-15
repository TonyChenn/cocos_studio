using System;

namespace Cocos.Launcher.Library
{
	public class DownloadFinishedEventArgs : EventArgs
	{
		public string Error { get; private set; }

		public bool IsSuccessed { get; private set; }

		public string DownloadPath { get; private set; }

		public DownloadFinishedEventArgs(bool isSuccessed, string error, string path)
		{
			this.IsSuccessed = isSuccessed;
			this.Error = error;
			this.DownloadPath = path;
		}
	}
}
