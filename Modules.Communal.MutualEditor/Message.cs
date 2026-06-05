using System;

namespace Modules.Communal.MutualEditor
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class Message
	{
		// Token: 0x0400000D RID: 13
		public string SentIP = "127.0.0.1";

		// Token: 0x0400000E RID: 14
		public string Sentport;

		// Token: 0x0400000F RID: 15
		public string ReciveIP = "127.0.0.1";

		// Token: 0x04000010 RID: 16
		public string RecivePort;

		// Token: 0x04000011 RID: 17
		public Action Action;

		// Token: 0x04000012 RID: 18
		public string Data;
	}
}
