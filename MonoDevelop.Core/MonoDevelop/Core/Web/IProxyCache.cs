using System;
using System.Net;

namespace MonoDevelop.Core.Web
{
	// Token: 0x0200025C RID: 604
	internal interface IProxyCache
	{
		// Token: 0x0600160E RID: 5646
		void Add(IWebProxy proxy);

		// Token: 0x0600160F RID: 5647
		IWebProxy GetProxy(Uri uri);
	}
}
