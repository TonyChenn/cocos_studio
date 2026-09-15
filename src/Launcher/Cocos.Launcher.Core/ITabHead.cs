using System;

namespace Cocos.Launcher.Core
{
	public interface ITabHead
	{
		string HeadName { get; set; }

		bool IsShowRed { get; set; }

		bool IsSelected { get; }

		void SetNumber(int number);

		event EventHandler<SelectedChangingEventArgs> SelectedChanging;
	}
}
