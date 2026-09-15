using System;

namespace Cocos.Launcher.Core
{
	public class RefreshRunModeEventArgs : EventArgs
	{
		public RunModeEnum RunMode { get; private set; }

		public RefreshRunModeEventArgs(RunModeEnum runMode)
		{
			this.RunMode = runMode;
		}
	}
}
