using System;
using System.Net;

namespace MonoDevelop.Core.Web
{
	/// <summary>Proxy authentication handler.</summary>
	// Token: 0x02000257 RID: 599
	[Obsolete]
	public interface IProxyAuthenticationHandler
	{
		/// <summary>
		/// Adds a proxy to the cache.
		/// </summary>
		/// <param name="proxy">Proxy.</param>
		// Token: 0x060015FD RID: 5629
		void AddProxyToCache(IWebProxy proxy);

		/// <summary>
		/// Gets a cached proxy for the Url, if available.
		/// </summary>
		/// <returns>The cached proxy.</returns>
		/// <param name="uri">URI for which the proxy will be used.</param>
		// Token: 0x060015FE RID: 5630
		IWebProxy GetCachedProxy(Uri uri);

		/// <summary>
		/// Adds credentials to the cache.
		/// </summary>
		/// <param name="uri">URI for which the credentials are valid.</param>
		/// <param name="credentials">Credentials.</param>
		// Token: 0x060015FF RID: 5631
		void AddCredentialsToCache(Uri uri, ICredentials credentials);

		/// <summary>
		/// Gets cached credentials, if available.
		/// </summary>
		/// <returns>The cached credentials.</returns>
		/// <param name="uri">URI for which the credentials will be used.</param>
		// Token: 0x06001600 RID: 5632
		ICredentials GetCachedCredentials(Uri uri);

		/// <summary>
		/// Gets credentials from user.
		/// </summary>
		/// <returns>The credentials from user.</returns>
		/// <param name="request">Request for which the credentials will be used.</param>
		/// <param name="credentialType">Type of the credentials.</param>
		/// <param name="retrying">Whether retrying.</param>
		// Token: 0x06001601 RID: 5633
		ICredentials GetCredentialsFromUser(HttpWebRequest request, CredentialType credentialType, bool retrying);
	}
}
