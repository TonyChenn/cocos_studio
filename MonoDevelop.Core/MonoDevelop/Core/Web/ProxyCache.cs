using System;
using System.Collections.Concurrent;
using System.Net;

namespace MonoDevelop.Core.Web
{
	// Token: 0x02000260 RID: 608
	internal class ProxyCache : IProxyCache
	{
		// Token: 0x0600161F RID: 5663 RVA: 0x00059614 File Offset: 0x00057814
		public IWebProxy GetProxy(Uri uri)
		{
			if (!ProxyCache.IsSystemProxySet(uri))
			{
				return null;
			}
			WebProxy systemProxy = ProxyCache.GetSystemProxy(uri);
			WebProxy result;
			if (!this.cache.TryGetValue(systemProxy.Address, out result))
			{
				return systemProxy;
			}
			return result;
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x0005964C File Offset: 0x0005784C
		public void Add(IWebProxy proxy)
		{
			WebProxy webProxy = proxy as WebProxy;
			if (webProxy != null)
			{
				this.cache.TryAdd(webProxy.Address, webProxy);
			}
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00059678 File Offset: 0x00057878
		private static WebProxy GetSystemProxy(Uri uri)
		{
			Uri proxy = ProxyCache.originalSystemProxy.GetProxy(uri);
			return new WebProxy(proxy);
		}

		/// <summary>
		/// Return true or false if connecting through a proxy server
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		// Token: 0x06001622 RID: 5666 RVA: 0x00059698 File Offset: 0x00057898
		private static bool IsSystemProxySet(Uri uri)
		{
			IWebProxy webProxy = WebRequest.DefaultWebProxy;
			if (webProxy != null)
			{
				Uri uri2 = new Uri(webProxy.GetProxy(uri).AbsoluteUri);
				if (string.Equals(uri2.AbsoluteUri, uri.AbsoluteUri))
				{
					return false;
				}
				if (webProxy.IsBypassed(uri))
				{
					return false;
				}
				webProxy = new WebProxy(uri2);
			}
			return webProxy != null;
		}

		/// <summary>
		/// Capture the default System Proxy so that it can be re-used by the IProxyFinder
		/// because we can't rely on WebRequest.DefaultWebProxy since someone can modify the DefaultWebProxy
		/// property and we can't tell if it was modified and if we are still using System Proxy Settings or not.
		/// </summary>
		// Token: 0x040006AD RID: 1709
		private static readonly IWebProxy originalSystemProxy = WebRequest.GetSystemWebProxy();

		// Token: 0x040006AE RID: 1710
		private readonly ConcurrentDictionary<Uri, WebProxy> cache = new ConcurrentDictionary<Uri, WebProxy>();
	}
}
