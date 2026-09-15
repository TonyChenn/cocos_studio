using System;

namespace Modules.Communal.MutualEditor
{
	public interface IUDPHandler : IDisposable
	{
		void SendMessage(string data, Action action = Action.Show);
	}
}
