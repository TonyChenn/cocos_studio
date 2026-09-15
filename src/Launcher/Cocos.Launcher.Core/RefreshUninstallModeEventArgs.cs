using System;

namespace Cocos.Launcher.Core
{
	public class RefreshUninstallModeEventArgs : EventArgs
	{
		public UninstallModeEnum UninstallMode { get; private set; }

		public RefreshUninstallModeEventArgs(UninstallModeEnum uninstallMode)
		{
			this.UninstallMode = uninstallMode;
		}
	}
}
