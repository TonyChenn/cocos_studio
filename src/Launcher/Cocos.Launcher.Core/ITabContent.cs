using System;
using Gtk;
using Mono.Addins;

namespace Cocos.Launcher.Core
{
	[TypeExtensionPoint]
	public interface ITabContent
	{
		int Order { get; }

		Widget Content { get; }

		void Initialize(ITabHead tabHead);

		void Activated(SwitchTabInfo switchTabInfo);

		void Deactivated();

		void Search(string text);
	}
}
