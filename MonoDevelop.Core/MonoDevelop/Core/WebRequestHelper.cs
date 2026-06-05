using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Mono.Addins;
using MonoDevelop.Core.Web;

namespace MonoDevelop.Core
{
	/// <summary>
	/// Helper for making web requests with support for authenticated proxies.
	/// </summary>
	// Token: 0x02000254 RID: 596
	public static class WebRequestHelper
	{
		// Token: 0x060015F1 RID: 5617 RVA: 0x00059028 File Offset: 0x00057228
		internal static void Initialize()
		{
			WebRequestHelper.proxyCache = new ProxyCache();
			WebRequestHelper.credentialProvider = AddinManager.GetExtensionObjects<ICredentialProvider>("/MonoDevelop/Core/WebCredentialProviders").FirstOrDefault<ICredentialProvider>();
			if (WebRequestHelper.credentialProvider != null)
			{
				WebRequestHelper.credentialProvider = new WebRequestHelper.CachingCredentialProvider(WebRequestHelper.credentialProvider);
				return;
			}
			LoggingService.LogWarning("No proxy credential provider was found");
			WebRequestHelper.credentialProvider = new NullCredentialProvider();
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060015F2 RID: 5618 RVA: 0x0005907E File Offset: 0x0005727E
		public static ICredentialProvider CredentialProvider
		{
			get
			{
				return WebRequestHelper.credentialProvider;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060015F3 RID: 5619 RVA: 0x00059085 File Offset: 0x00057285
		// (set) Token: 0x060015F4 RID: 5620 RVA: 0x0005908C File Offset: 0x0005728C
		[Obsolete]
		public static IProxyAuthenticationHandler ProxyAuthenticationHandler { get; internal set; }

		/// <summary>
		/// Gets the web response, using the <see cref="P:MonoDevelop.Core.WebRequestHelper.ProxyAuthenticationHandler" /> to handle proxy authentication
		/// if necessary.
		/// </summary>
		/// <returns>The response.</returns>
		/// <param name="createRequest">Callback for creating the request.</param>
		/// <param name="prepareRequest">Callback for preparing the request, e.g. writing the request stream.</param>
		/// <param name="token">Cancellation token.</param>
		/// <remarks>
		/// Keeps sending requests until a response code that doesn't require authentication happens or if the request
		/// requires authentication and the user has stopped trying to enter them (i.e. they hit cancel when they are prompted).
		/// </remarks>
		// Token: 0x060015F5 RID: 5621 RVA: 0x000590B8 File Offset: 0x000572B8
		public static Task<HttpWebResponse> GetResponseAsync(Func<HttpWebRequest> createRequest, Action<HttpWebRequest> prepareRequest = null, CancellationToken token = default(CancellationToken))
		{
			return Task.Factory.StartNew<HttpWebResponse>(() => WebRequestHelper.GetResponse(createRequest, prepareRequest, token), token);
		}

		/// <summary>
		/// Gets the web response, using the <see cref="P:MonoDevelop.Core.WebRequestHelper.ProxyAuthenticationHandler" /> to handle proxy authentication
		/// if necessary.
		/// </summary>
		/// <returns>The response.</returns>
		/// <param name="createRequest">Callback for creating the request.</param>
		/// <param name="prepareRequest">Callback for preparing the request, e.g. writing the request stream.</param>
		/// <param name="token">Cancellation token.</param>
		/// <remarks>
		/// Keeps sending requests until a response code that doesn't require authentication happens or if the request
		/// requires authentication and the user has stopped trying to enter them (i.e. they hit cancel when they are prompted).
		/// </remarks>
		// Token: 0x060015F6 RID: 5622 RVA: 0x00059100 File Offset: 0x00057300
		public static HttpWebResponse GetResponse(Func<HttpWebRequest> createRequest, Action<HttpWebRequest> prepareRequest = null, CancellationToken token = default(CancellationToken))
		{
			if (prepareRequest == null)
			{
				prepareRequest = delegate(HttpWebRequest r)
				{
				};
			}
			if (WebRequestHelper.credentialProvider == null)
			{
				HttpWebRequest httpWebRequest = createRequest();
				httpWebRequest.MakeCancelable(token);
				prepareRequest(httpWebRequest);
				return (HttpWebResponse)httpWebRequest.GetResponse();
			}
			RequestHelper requestHelper = new RequestHelper(createRequest, prepareRequest, WebRequestHelper.proxyCache, CredentialStore.Instance, WebRequestHelper.credentialProvider);
			return requestHelper.GetResponse(token);
		}

		/// <summary>
		/// Determines whether an error code is likely to have been caused by internet reachability problems.
		/// </summary>
		// Token: 0x060015F7 RID: 5623 RVA: 0x00059178 File Offset: 0x00057378
		public static bool IsCannotReachInternetError(this WebExceptionStatus status)
		{
			switch (status)
			{
			case WebExceptionStatus.NameResolutionFailure:
			case WebExceptionStatus.ConnectFailure:
			case WebExceptionStatus.SendFailure:
				break;
			case WebExceptionStatus.ReceiveFailure:
				return false;
			default:
				if (status != WebExceptionStatus.ConnectionClosed)
				{
					switch (status)
					{
					case WebExceptionStatus.Timeout:
					case WebExceptionStatus.ProxyNameResolutionFailure:
						break;
					default:
						return false;
					}
				}
				break;
			}
			return true;
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x000591E4 File Offset: 0x000573E4
		private static void MakeCancelable(this HttpWebRequest request, CancellationToken token)
		{
			if (!token.CanBeCanceled)
			{
				return;
			}
			token.Register(delegate()
			{
				HttpWebRequest request2 = request;
				if (request2 != null)
				{
					request2.Abort();
				}
			});
		}

		// Token: 0x04000699 RID: 1689
		private const string WebCredentialProvidersPath = "/MonoDevelop/Core/WebCredentialProviders";

		// Token: 0x0400069A RID: 1690
		private static ProxyCache proxyCache;

		// Token: 0x0400069B RID: 1691
		private static ICredentialProvider credentialProvider;

		// Token: 0x02000256 RID: 598
		private class CachingCredentialProvider : ICredentialProvider
		{
			// Token: 0x060015FB RID: 5627 RVA: 0x0005921C File Offset: 0x0005741C
			public CachingCredentialProvider(ICredentialProvider wrapped)
			{
				this.wrapped = wrapped;
			}

			// Token: 0x060015FC RID: 5628 RVA: 0x00059238 File Offset: 0x00057438
			public ICredentials GetCredentials(Uri uri, IWebProxy proxy, CredentialType credentialType, bool retrying)
			{
				if (credentialType != CredentialType.ProxyCredentials)
				{
					return this.wrapped.GetCredentials(uri, proxy, credentialType, retrying);
				}
				Uri proxy2 = proxy.GetProxy(uri);
				if (proxy2 == null)
				{
					return null;
				}
				if (!retrying)
				{
					ICredentials credentials = CredentialStore.Instance.GetCredentials(proxy2);
					if (credentials != null)
					{
						return credentials;
					}
				}
				ICredentials result;
				lock (this.locker)
				{
					if (!retrying)
					{
						ICredentials credentials2 = CredentialStore.Instance.GetCredentials(proxy2);
						if (credentials2 != null)
						{
							return credentials2;
						}
					}
					ICredentials credentials3 = this.wrapped.GetCredentials(uri, proxy, credentialType, retrying);
					if (credentials3 != null)
					{
						CredentialStore.Instance.Add(proxy2, credentials3);
					}
					result = credentials3;
				}
				return result;
			}

			// Token: 0x0400069E RID: 1694
			private readonly ICredentialProvider wrapped;

			// Token: 0x0400069F RID: 1695
			private readonly object locker = new object();
		}
	}
}
