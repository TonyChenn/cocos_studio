using System;

namespace Cocos.Launcher.Library
{
	public class ProgressChangedEventArgs : EventArgs
	{
		public float Fraction { get; private set; }

		public float FileSize { get; private set; }

		public float DownloadSpeed { get; private set; }

		public long RemainTime { get; private set; }

		public ProgressChangedEventArgs(float fraction, float filesize = 0f, float downladSpeed = 0f, long remainTime = -1L)
		{
			this.Fraction = fraction;
			this.FileSize = filesize;
			this.DownloadSpeed = downladSpeed;
			this.RemainTime = remainTime;
		}
	}
}
