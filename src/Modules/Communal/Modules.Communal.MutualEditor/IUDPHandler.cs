using System;

namespace Modules.Communal.MutualEditor
{
	// Token: 0x02000002 RID: 2
	public interface IUDPHandler : IDisposable
	{
		// Token: 0x06000001 RID: 1
		void SendMessage(string data, Action action = Action.Show);
	}
}
