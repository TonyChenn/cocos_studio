using System;

namespace Cocos.Launcher.Core
{
	public class DownloadSucceedEventArgs : EventArgs
	{
		public string TargetPath { get; private set; }

		public string FileName { get; private set; }

		public DownloadSucceedEventArgs(string downloadPath, string filename)
		{
			this.TargetPath = downloadPath;
			this.FileName = filename;
		}
	}
}
