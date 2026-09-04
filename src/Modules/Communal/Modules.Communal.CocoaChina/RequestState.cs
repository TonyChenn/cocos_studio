using System;
using System.IO;
using System.Net;
using System.Text;

namespace Modules.Communal.CocoaChina
{
	// Token: 0x02000004 RID: 4
	public class RequestState
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000023AC File Offset: 0x000005AC
		public RequestState()
		{
			this.BufferRead = new byte[10240];
			this.requestData = new StringBuilder("");
			this.request = null;
			this.streamResponse = null;
		}

		// Token: 0x0400000A RID: 10
		private const int BUFFER_SIZE = 10240;

		// Token: 0x0400000B RID: 11
		public StringBuilder requestData;

		// Token: 0x0400000C RID: 12
		public byte[] BufferRead;

		// Token: 0x0400000D RID: 13
		public HttpWebRequest request;

		// Token: 0x0400000E RID: 14
		public HttpWebResponse response;

		// Token: 0x0400000F RID: 15
		public Stream streamResponse;
	}
}
