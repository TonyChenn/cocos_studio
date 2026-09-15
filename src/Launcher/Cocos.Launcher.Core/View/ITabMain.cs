using System;
using Gtk;

namespace Cocos.Launcher.Core.View
{
	internal interface ITabMain
	{
		Widget ContainerWidget { get; }

		void ChangeTabContent(ITabContent tabContent);
	}
}
