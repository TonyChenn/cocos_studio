using System;

namespace Cocos.Launcher.Core
{
	public interface IUpdateService
	{
		UpdateInfo StoreUpdateInfo { get; }

		UpdateInfo TutorialsUpdateInfo { get; }

		void Save();

		event EventHandler<EventArgs> UpdateChanged;
	}
}
