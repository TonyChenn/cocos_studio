using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Modules.Communal.CocosCodeIDE
{
	// Token: 0x02000008 RID: 8
	[DataObject]
	internal class SendMessageInfo
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002D33 File Offset: 0x00000F33
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002D3B File Offset: 0x00000F3B
		public string command { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002D44 File Offset: 0x00000F44
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002D4C File Offset: 0x00000F4C
		[JsonProperty("params")]
		public string[] Params { get; set; }

		// Token: 0x0600002C RID: 44 RVA: 0x00002D55 File Offset: 0x00000F55
		public SendMessageInfo(string command, string[] message)
		{
			this.command = command;
			this.Params = message;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002D6B File Offset: 0x00000F6B
		public SendMessageInfo()
		{
		}
	}
}
