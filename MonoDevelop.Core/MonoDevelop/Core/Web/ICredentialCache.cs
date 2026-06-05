using System;
using System.Net;

namespace MonoDevelop.Core.Web
{
	// Token: 0x02000258 RID: 600
	internal interface ICredentialCache
	{
		// Token: 0x06001602 RID: 5634
		void Add(Uri uri, ICredentials credentials);

		// Token: 0x06001603 RID: 5635
		ICredentials GetCredentials(Uri uri);
	}
}
