using System;

namespace Cocos.Launcher.Core
{
	public interface ILoginService
	{
		bool IsLoginSuccessed { get; }

		LoginInfo LoginInfo { get; }

		void ShowLoginWindow();

		event EventHandler<LoginChangedEventArgs> LoginChanged;
	}
}
