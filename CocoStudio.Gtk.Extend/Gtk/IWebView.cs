using System;

namespace Gtk
{
	// Token: 0x02000070 RID: 112
	public interface IWebView
	{
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x0600028E RID: 654
		// (remove) Token: 0x0600028F RID: 655
		event EventHandler<WebNewWindowEventArgs> WebNewWindow;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000290 RID: 656
		// (remove) Token: 0x06000291 RID: 657
		event EventHandler<WebNavigatingEventArgs> WebNavigating;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000292 RID: 658
		// (remove) Token: 0x06000293 RID: 659
		event EventHandler<WebJavaScriptCallEventArgs> WebJavaScriptCalled;

		// Token: 0x06000294 RID: 660
		void Activated(Widget parentWidget);

		// Token: 0x06000295 RID: 661
		void Deactivated();

		// Token: 0x06000296 RID: 662
		void PostData(string postData);

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000297 RID: 663
		// (set) Token: 0x06000298 RID: 664
		bool IsMenuEnabled { get; set; }
	}
}
