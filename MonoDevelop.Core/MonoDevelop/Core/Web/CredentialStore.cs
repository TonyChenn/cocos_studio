using System;
using System.Collections.Concurrent;
using System.Net;

namespace MonoDevelop.Core.Web
{
	// Token: 0x02000259 RID: 601
	internal class CredentialStore : ICredentialCache
	{
		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x000592F0 File Offset: 0x000574F0
		public static CredentialStore Instance
		{
			get
			{
				return CredentialStore._instance;
			}
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x000592F8 File Offset: 0x000574F8
		public ICredentials GetCredentials(Uri uri)
		{
			Uri rootUri = CredentialStore.GetRootUri(uri);
			ICredentials result;
			if (this._credentialCache.TryGetValue(uri, out result) || this._credentialCache.TryGetValue(rootUri, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x00059340 File Offset: 0x00057540
		public void Add(Uri uri, ICredentials credentials)
		{
			Uri rootUri = CredentialStore.GetRootUri(uri);
			this._credentialCache.TryAdd(uri, credentials);
			this._credentialCache.AddOrUpdate(rootUri, credentials, (Uri u, ICredentials c) => credentials);
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x00059393 File Offset: 0x00057593
		internal static Uri GetRootUri(Uri uri)
		{
			return new Uri(uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped));
		}

		// Token: 0x040006A0 RID: 1696
		private readonly ConcurrentDictionary<Uri, ICredentials> _credentialCache = new ConcurrentDictionary<Uri, ICredentials>();

		// Token: 0x040006A1 RID: 1697
		private static readonly CredentialStore _instance = new CredentialStore();
	}
}
