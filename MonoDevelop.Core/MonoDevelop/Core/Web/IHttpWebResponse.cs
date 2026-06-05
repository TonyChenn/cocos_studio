using System;
using System.Collections.Specialized;
using System.Net;

namespace MonoDevelop.Core.Web
{
	// Token: 0x0200025B RID: 603
	internal interface IHttpWebResponse : IDisposable
	{
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x0600160A RID: 5642
		HttpStatusCode StatusCode { get; }

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x0600160B RID: 5643
		Uri ResponseUri { get; }

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x0600160C RID: 5644
		string AuthType { get; }

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x0600160D RID: 5645
		NameValueCollection Headers { get; }
	}
}
