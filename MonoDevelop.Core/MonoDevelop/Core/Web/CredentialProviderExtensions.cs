using System;
using System.Net;

namespace MonoDevelop.Core.Web
{
	// Token: 0x02000268 RID: 616
	internal static class CredentialProviderExtensions
	{
		// Token: 0x06001655 RID: 5717 RVA: 0x0005A45E File Offset: 0x0005865E
		internal static ICredentials GetCredentials(this ICredentialProvider provider, WebRequest request, CredentialType credentialType, bool retrying = false)
		{
			return provider.GetCredentials(request.RequestUri, request.Proxy, credentialType, retrying);
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x0005A474 File Offset: 0x00058674
		internal static ICredentials AsCredentialCache(this ICredentials credentials, Uri uri)
		{
			if (credentials == null)
			{
				return null;
			}
			if (credentials == CredentialCache.DefaultCredentials || credentials == CredentialCache.DefaultNetworkCredentials)
			{
				return credentials;
			}
			NetworkCredential networkCredential = credentials as NetworkCredential;
			if (networkCredential == null)
			{
				return credentials;
			}
			CredentialCache credentialCache = new CredentialCache();
			foreach (string authType in CredentialProviderExtensions._authenticationSchemes)
			{
				credentialCache.Add(uri, authType, networkCredential);
			}
			return credentialCache;
		}

		// Token: 0x040006C0 RID: 1728
		private static readonly string[] _authenticationSchemes = new string[]
		{
			"Basic",
			"NTLM",
			"Negotiate"
		};
	}
}
