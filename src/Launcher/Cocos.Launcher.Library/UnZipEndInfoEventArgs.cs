using System;

namespace Cocos.Launcher.Library
{
	public class UnZipEndInfoEventArgs : EventArgs
	{
		public string Error { get; private set; }

		public bool IsSucceed { get; private set; }

		public string ZipFilePath { get; private set; }

		public UnZipEndInfoEventArgs(bool isSucceed, string error, string targetPath)
		{
			this.IsSucceed = isSucceed;
			this.Error = error;
			this.ZipFilePath = targetPath;
		}
	}
}
