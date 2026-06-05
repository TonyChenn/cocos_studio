using System;

namespace Modules.Communal.MutualEditor
{
	// Token: 0x0200000B RID: 11
	public struct UDPPort
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002790 File Offset: 0x00000990
		public UDPPort(string ip, int port)
		{
			this.IP = ip;
			this.Port = port;
		}

		// Token: 0x0400000B RID: 11
		public string IP;

		// Token: 0x0400000C RID: 12
		public int Port;
	}
}
