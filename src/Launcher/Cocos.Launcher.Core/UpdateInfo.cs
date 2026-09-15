using System;

namespace Cocos.Launcher.Core
{
	public class UpdateInfo
	{
		public string LocalTime { get; private set; }

		public string RemoteTime { get; private set; }

		public bool IsUpdate
		{
			get
			{
				return !string.Equals(this.LocalTime, this.RemoteTime);
			}
		}

		public void Update()
		{
			this.LocalTime = this.RemoteTime;
		}

		public UpdateInfo(string localTime, string remoteTime)
		{
			this.LocalTime = localTime;
			this.RemoteTime = remoteTime;
		}
	}
}
