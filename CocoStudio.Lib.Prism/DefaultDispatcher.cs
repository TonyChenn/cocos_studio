using System;
using Gtk;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x0200000F RID: 15
	public class DefaultDispatcher : IDispatcherFacade
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002AC8 File Offset: 0x00000CC8
		public void BeginInvoke(Delegate method, object arg)
		{
			Application.Invoke(delegate(object param0, EventArgs param1)
			{
				method.DynamicInvoke(new object[]
				{
					arg
				});
			});
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002AFD File Offset: 0x00000CFD
		public void BeginInvoke(EventHandler method, EventArgs args)
		{
			throw new NotImplementedException();
		}
	}
}
