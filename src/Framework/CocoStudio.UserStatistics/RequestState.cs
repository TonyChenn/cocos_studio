using System;
using System.Net;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000011 RID: 17
	internal class RequestState
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000035E4 File Offset: 0x000017E4
		// (set) Token: 0x06000051 RID: 81 RVA: 0x000035FB File Offset: 0x000017FB
		public HttpWebRequest Request { get; private set; }

		// Token: 0x06000052 RID: 82 RVA: 0x00003604 File Offset: 0x00001804
		public RequestState(HttpWebRequest request)
		{
			this.Request = request;
		}
	}
}
