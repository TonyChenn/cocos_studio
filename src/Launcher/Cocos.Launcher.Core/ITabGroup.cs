using System;
using Cocos.Launcher.Core.View;
using Gtk;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000005 RID: 5
	public interface ITabGroup
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000014 RID: 20
		Widget Content { get; }

		// Token: 0x06000015 RID: 21
		void SwitchTab(SwitchTabInfo switchTabInfo);

		// Token: 0x06000016 RID: 22
		void AddTab(ITabContent tabContent);

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000017 RID: 23
		TabPage LastSelectedTabPage { get; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000018 RID: 24
		// (remove) Token: 0x06000019 RID: 25
		event EventHandler<EventArgs> SelectedTabChanged;
	}
}
