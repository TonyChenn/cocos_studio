using System;
using Gtk;
using Mono.Addins;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000006 RID: 6
	[TypeExtensionPoint]
	public interface ITabContent
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001A RID: 26
		int Order { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001B RID: 27
		Widget Content { get; }

		// Token: 0x0600001C RID: 28
		void Initialize(ITabHead tabHead);

		// Token: 0x0600001D RID: 29
		void Activated(SwitchTabInfo switchTabInfo);

		// Token: 0x0600001E RID: 30
		void Deactivated();

		// Token: 0x0600001F RID: 31
		void Search(string text);
	}
}
