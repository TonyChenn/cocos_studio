using System;
using Gtk;

namespace Cocos.Launcher.Core.View
{
	// Token: 0x02000048 RID: 72
	internal interface ITabMain
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600026B RID: 619
		Widget ContainerWidget { get; }

		// Token: 0x0600026C RID: 620
		void ChangeTabContent(ITabContent tabContent);
	}
}
