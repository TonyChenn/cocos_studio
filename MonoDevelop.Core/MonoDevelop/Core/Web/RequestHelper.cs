using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;

namespace MonoDevelop.Core.Web
{
	/// <summary>
	/// This class is used to keep sending requests until a response code that doesn't require
	/// authentication happens or if the request requires authentication and
	/// the user has stopped trying to enter them (i.e. they hit cancel when they are prompted).
	/// </summary>
	// Token: 0x02000261 RID: 609
	internal class RequestHelper
	{
		// Token: 0x06001625 RID: 5669 RVA: 0x0005970D File Offset: 0x0005790D
		public RequestHelper(Func<HttpWebRequest> createRequest, Action<HttpWebRequest> prepareRequest, IProxyCache proxyCache, ICredentialCache credentialCache, ICredentialProvider credentialProvider)
		{
			this._createRequest = createRequest;
			this._prepareRequest = prepareRequest;
			this._proxyCache = proxyCache;
			this._credentialCache = credentialCache;
			this._credentialProvider = credentialProvider;
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0005973A File Offset: 0x0005793A
		private static void MakeCancelable(HttpWebRequest request, CancellationToken token)
		{
			if (token.CanBeCanceled)
			{
				token.Register(new Action(request.Abort));
			}
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0005975C File Offset: 0x0005795C
		public HttpWebResponse GetResponse(CancellationToken token)
		{
			this._previousRequest = null;
			this._previousResponse = null;
			this._previousStatusCode = null;
			this._usingSTSAuth = false;
			this._continueIfFailed = true;
			this._proxyCredentialsRetryCount = 0;
			this._credentialsRetryCount = 0;
			int num = 0;
			HttpWebResponse result;
			for (;;)
			{
				HttpWebRequest httpWebRequest = this._createRequest();
				RequestHelper.MakeCancelable(httpWebRequest, token);
				this.ConfigureRequest(httpWebRequest);
				try
				{
					string text = httpWebRequest.Headers["Authorization"];
					this._basicAuthIsUsedInPreviousRequest = (text != null && text.StartsWith("Basic ", StringComparison.Ordinal));
					this._prepareRequest(httpWebRequest);
					HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					this._proxyCache.Add(httpWebRequest.Proxy);
					ICredentials credentials = httpWebRequest.Credentials;
					this._credentialCache.Add(httpWebRequest.RequestUri, credentials);
					this._credentialCache.Add(httpWebResponse.ResponseUri, credentials);
					result = httpWebResponse;
				}
				catch (WebException ex)
				{
					num++;
					if (num >= 10)
					{
						throw;
					}
					using (IHttpWebResponse response = RequestHelper.GetResponse(ex.Response))
					{
						if (response == null && ex.Status != WebExceptionStatus.SecureChannelFailure)
						{
							throw;
						}
						if (ex.Status == WebExceptionStatus.SecureChannelFailure)
						{
							if (!this._continueIfFailed)
							{
								throw;
							}
							this._previousStatusCode = new HttpStatusCode?(HttpStatusCode.Unauthorized);
						}
						else
						{
							if (this._previousStatusCode == HttpStatusCode.ProxyAuthenticationRequired && response.StatusCode != HttpStatusCode.ProxyAuthenticationRequired)
							{
								this._proxyCache.Add(httpWebRequest.Proxy);
							}
							else if (this._previousStatusCode == HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Unauthorized)
							{
								this._credentialCache.Add(httpWebRequest.RequestUri, httpWebRequest.Credentials);
								this._credentialCache.Add(response.ResponseUri, httpWebRequest.Credentials);
							}
							this._usingSTSAuth = STSAuthHelper.TryRetrieveSTSToken(httpWebRequest.RequestUri, response);
							if (!RequestHelper.IsAuthenticationResponse(response) || !this._continueIfFailed)
							{
								throw;
							}
							this._previousRequest = httpWebRequest;
							this._previousResponse = response;
							this._previousStatusCode = new HttpStatusCode?(this._previousResponse.StatusCode);
						}
					}
					continue;
				}
				break;
			}
			return result;
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x000599D0 File Offset: 0x00057BD0
		private void ConfigureRequest(HttpWebRequest request)
		{
			request.Proxy = this._proxyCache.GetProxy(request.RequestUri);
			if (request.Proxy != null && request.Proxy.Credentials == null)
			{
				request.Proxy.Credentials = CredentialCache.DefaultCredentials;
			}
			if (this._previousResponse == null || RequestHelper.ShouldKeepAliveBeUsedInRequest(this._previousRequest, this._previousResponse))
			{
				request.Credentials = this._credentialCache.GetCredentials(request.RequestUri);
				if (request.Credentials == null)
				{
					request.UseDefaultCredentials = true;
				}
			}
			else if (this._previousStatusCode == HttpStatusCode.ProxyAuthenticationRequired)
			{
				request.Proxy.Credentials = this._credentialProvider.GetCredentials(request, CredentialType.ProxyCredentials, this._proxyCredentialsRetryCount > 0);
				this._continueIfFailed = (request.Proxy.Credentials != null);
				this._proxyCredentialsRetryCount++;
			}
			else if (this._previousStatusCode == HttpStatusCode.Unauthorized)
			{
				this.SetCredentialsOnAuthorizationError(request);
			}
			RequestHelper.SetKeepAliveHeaders(request, this._previousResponse);
			if (this._usingSTSAuth)
			{
				STSAuthHelper.PrepareSTSRequest(request);
			}
			request.Credentials = request.Credentials.AsCredentialCache(request.RequestUri);
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00059B24 File Offset: 0x00057D24
		private void SetCredentialsOnAuthorizationError(HttpWebRequest request)
		{
			if (this._usingSTSAuth)
			{
				return;
			}
			bool flag = this._previousResponse.AuthType != null && this._previousResponse.AuthType.IndexOf("Basic", StringComparison.OrdinalIgnoreCase) != -1;
			if (flag && !this._basicAuthIsUsedInPreviousRequest)
			{
				request.Credentials = this._credentialCache.GetCredentials(request.RequestUri);
			}
			if (request.Credentials == null)
			{
				request.Credentials = this._credentialProvider.GetCredentials(request, CredentialType.RequestCredentials, this._credentialsRetryCount > 0);
			}
			if (flag)
			{
				NetworkCredential credential = request.Credentials.GetCredential(request.RequestUri, "Basic");
				if (credential != null)
				{
					string text = credential.UserName + ":" + credential.Password;
					text = Convert.ToBase64String(Encoding.Default.GetBytes(text));
					request.Headers["Authorization"] = "Basic " + text;
				}
			}
			this._continueIfFailed = (request.Credentials != null);
			this._credentialsRetryCount++;
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00059C30 File Offset: 0x00057E30
		private static IHttpWebResponse GetResponse(WebResponse response)
		{
			IHttpWebResponse httpWebResponse = response as IHttpWebResponse;
			if (httpWebResponse != null)
			{
				return httpWebResponse;
			}
			HttpWebResponse httpWebResponse2 = response as HttpWebResponse;
			if (httpWebResponse2 == null)
			{
				return null;
			}
			return new RequestHelper.HttpWebResponseWrapper(httpWebResponse2);
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00059C5B File Offset: 0x00057E5B
		private static bool IsAuthenticationResponse(IHttpWebResponse response)
		{
			return response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.ProxyAuthenticationRequired;
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00059C79 File Offset: 0x00057E79
		private static void SetKeepAliveHeaders(HttpWebRequest request, IHttpWebResponse previousResponse)
		{
			if (previousResponse == null || !RequestHelper.IsNtlmOrKerberos(previousResponse.AuthType))
			{
				request.KeepAlive = false;
				request.ProtocolVersion = HttpVersion.Version10;
			}
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x00059C9D File Offset: 0x00057E9D
		private static bool ShouldKeepAliveBeUsedInRequest(HttpWebRequest request, IHttpWebResponse response)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			return !request.KeepAlive && RequestHelper.IsNtlmOrKerberos(response.AuthType);
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x00059CD0 File Offset: 0x00057ED0
		private static bool IsNtlmOrKerberos(string authType)
		{
			return !string.IsNullOrEmpty(authType) && (authType.IndexOf("NTLM", StringComparison.OrdinalIgnoreCase) != -1 || authType.IndexOf("Kerberos", StringComparison.OrdinalIgnoreCase) != -1);
		}

		// Token: 0x040006AF RID: 1711
		private Func<HttpWebRequest> _createRequest;

		// Token: 0x040006B0 RID: 1712
		private Action<HttpWebRequest> _prepareRequest;

		// Token: 0x040006B1 RID: 1713
		private IProxyCache _proxyCache;

		// Token: 0x040006B2 RID: 1714
		private ICredentialCache _credentialCache;

		// Token: 0x040006B3 RID: 1715
		private ICredentialProvider _credentialProvider;

		// Token: 0x040006B4 RID: 1716
		private HttpWebRequest _previousRequest;

		// Token: 0x040006B5 RID: 1717
		private IHttpWebResponse _previousResponse;

		// Token: 0x040006B6 RID: 1718
		private HttpStatusCode? _previousStatusCode;

		// Token: 0x040006B7 RID: 1719
		private int _credentialsRetryCount;

		// Token: 0x040006B8 RID: 1720
		private bool _usingSTSAuth;

		// Token: 0x040006B9 RID: 1721
		private bool _continueIfFailed;

		// Token: 0x040006BA RID: 1722
		private int _proxyCredentialsRetryCount;

		// Token: 0x040006BB RID: 1723
		private bool _basicAuthIsUsedInPreviousRequest;

		// Token: 0x02000262 RID: 610
		private class HttpWebResponseWrapper : IHttpWebResponse, IDisposable
		{
			// Token: 0x0600162F RID: 5679 RVA: 0x00059CFF File Offset: 0x00057EFF
			public HttpWebResponseWrapper(HttpWebResponse response)
			{
				this._response = response;
			}

			// Token: 0x170004B0 RID: 1200
			// (get) Token: 0x06001630 RID: 5680 RVA: 0x00059D0E File Offset: 0x00057F0E
			public string AuthType
			{
				get
				{
					return this._response.Headers[HttpResponseHeader.WwwAuthenticate];
				}
			}

			// Token: 0x170004B1 RID: 1201
			// (get) Token: 0x06001631 RID: 5681 RVA: 0x00059D22 File Offset: 0x00057F22
			public HttpStatusCode StatusCode
			{
				get
				{
					return this._response.StatusCode;
				}
			}

			// Token: 0x170004B2 RID: 1202
			// (get) Token: 0x06001632 RID: 5682 RVA: 0x00059D2F File Offset: 0x00057F2F
			public Uri ResponseUri
			{
				get
				{
					return this._response.ResponseUri;
				}
			}

			// Token: 0x170004B3 RID: 1203
			// (get) Token: 0x06001633 RID: 5683 RVA: 0x00059D3C File Offset: 0x00057F3C
			public NameValueCollection Headers
			{
				get
				{
					return this._response.Headers;
				}
			}

			// Token: 0x06001634 RID: 5684 RVA: 0x00059D49 File Offset: 0x00057F49
			public void Dispose()
			{
				if (this._response != null)
				{
					this._response.Close();
				}
			}

			// Token: 0x040006BC RID: 1724
			private readonly HttpWebResponse _response;
		}
	}
}
