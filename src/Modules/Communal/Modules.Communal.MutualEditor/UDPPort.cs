using System;

namespace Modules.Communal.MutualEditor
{
	public struct UDPPort
	{
		public UDPPort(string ip, int port)
		{
			this.IP = ip;
			this.Port = port;
		}

		public string IP;

		public int Port;
	}
}
