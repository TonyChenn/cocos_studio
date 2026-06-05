using System;
using System.Net;

namespace MonoDevelop.Core.Web
{
	// Token: 0x0200025F RID: 607
	internal class NullCredentialProvider : ICredentialProvider
	{
		// Token: 0x0600161D RID: 5661 RVA: 0x00059607 File Offset: 0x00057807
		public ICredentials GetCredentials(Uri uri, IWebProxy proxy, CredentialType credentialType, bool retrying)
		{
			return null;
		}
	}
}
