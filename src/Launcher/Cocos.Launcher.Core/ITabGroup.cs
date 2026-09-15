using System;
using Cocos.Launcher.Core.View;
using Gtk;

namespace Cocos.Launcher.Core
{
	public interface ITabGroup
	{
		Widget Content { get; }

		void SwitchTab(SwitchTabInfo switchTabInfo);

		void AddTab(ITabContent tabContent);

		TabPage LastSelectedTabPage { get; }

		event EventHandler<EventArgs> SelectedTabChanged;
	}
}
