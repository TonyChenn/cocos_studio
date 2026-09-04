using System;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Event
{
	// Token: 0x020000AA RID: 170
	public class RenderEngineLoadedEventArgs
	{
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x000184F0 File Offset: 0x000166F0
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x00018507 File Offset: 0x00016707
		public GameWindow GameWindow { get; private set; }

		// Token: 0x06000584 RID: 1412 RVA: 0x00018510 File Offset: 0x00016710
		public RenderEngineLoadedEventArgs(GameWindow gameWindow)
		{
			this.GameWindow = gameWindow;
		}
	}
}
