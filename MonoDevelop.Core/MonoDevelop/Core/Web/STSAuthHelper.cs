using System;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;

namespace MonoDevelop.Core.Web
{
	// Token: 0x02000264 RID: 612
	internal static class STSAuthHelper
	{
		/// <summary>
		/// Adds the SAML token as a header to the request if it is already cached for this host.
		/// </summary>
		// Token: 0x06001636 RID: 5686 RVA: 0x00059D6C File Offset: 0x00057F6C
		public static void PrepareSTSRequest(WebRequest request)
		{
			string cacheKey = STSAuthHelper.GetCacheKey(request.RequestUri);
			string token;
			if (MemoryCache.Instance.TryGetValue<string>(cacheKey, out token))
			{
				request.Headers["X-MonoDevelop-STS-Token"] = STSAuthHelper.EncodeHeader(token);
			}
		}

		/// <summary>
		/// Attempts to retrieve a SAML token if the response indicates that server requires STS-based auth.
		/// </summary>
		/// <param name="requestUri">The feed URI we were connecting to.</param>
		/// <param name="response">The 401 response we receieved from the server.</param>
		/// <returns>True if we were able to successfully retrieve a SAML token from the STS specified in the response headers.</returns>
		// Token: 0x06001637 RID: 5687 RVA: 0x00059DCC File Offset: 0x00057FCC
		public static bool TryRetrieveSTSToken(Uri requestUri, IHttpWebResponse response)
		{
			if (response.StatusCode != HttpStatusCode.Unauthorized)
			{
				return false;
			}
			string endPoint = STSAuthHelper.GetSTSEndPoint(response);
			string realm = response.Headers["X-MonoDevelop-STS-Realm"];
			if (string.IsNullOrEmpty(endPoint) || string.IsNullOrEmpty(realm))
			{
				return false;
			}
			string cacheKey = STSAuthHelper.GetCacheKey(requestUri);
			MemoryCache.Instance.GetOrAdd<string>(cacheKey, () => STSAuthHelper.GetSTSToken(requestUri, endPoint, realm), TimeSpan.FromMinutes(30.0), true);
			return true;
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x00059E68 File Offset: 0x00058068
		private static string GetSTSToken(Uri requestUri, string endPoint, string appliesTo)
		{
			WIFTypeProvider wiftypes = WIFTypeProvider.GetWIFTypes();
			if (wiftypes == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Connection to feed '{0}' requires the Windows Identity Foundation runtime to be installed.", new object[]
				{
					requestUri
				}));
			}
			WS2007HttpBinding ws2007HttpBinding = new WS2007HttpBinding(SecurityMode.Transport);
			object arg = Activator.CreateInstance(wiftypes.ChannelFactory, new object[]
			{
				ws2007HttpBinding,
				endPoint
			});
			if (STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site4 == null)
			{
				STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site4 = CallSite<Func<CallSite, object, TrustVersion, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.SetMember(CSharpBinderFlags.None, "TrustVersion", typeof(STSAuthHelper), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
				}));
			}
			STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site4.Target(STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site4, arg, TrustVersion.WSTrust13);
			object obj = Activator.CreateInstance(wiftypes.RequestSecurityToken);
			if (STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site5 == null)
			{
				STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site5 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.SetMember(CSharpBinderFlags.None, "RequestType", typeof(STSAuthHelper), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
				}));
			}
			STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site5.Target(STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site5, obj, STSAuthHelper.GetFieldValue<string>(wiftypes.RequestTypes, "Issue"));
			if (STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site6 == null)
			{
				STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site6 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.SetMember(CSharpBinderFlags.None, "KeyType", typeof(STSAuthHelper), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
				}));
			}
			STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site6.Target(STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site6, obj, STSAuthHelper.GetFieldValue<string>(wiftypes.KeyTypes, "Bearer"));
			object arg2 = Activator.CreateInstance(wiftypes.EndPoint, new object[]
			{
				appliesTo
			});
			if (STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site7 == null)
			{
				STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site7 = CallSite<Action<CallSite, Type, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SetProperty", null, typeof(STSAuthHelper), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
				}));
			}
			STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site7.Target(STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site7, typeof(STSAuthHelper), obj, "AppliesTo", arg2);
			if (STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site8 == null)
			{
				STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site8 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "CreateChannel", null, typeof(STSAuthHelper), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			object arg3 = STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site8.Target(STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site8, arg);
			if (STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site9 == null)
			{
				STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site9 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "Issue", null, typeof(STSAuthHelper), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			object arg4 = STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site9.Target(STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Site9, arg3, obj);
			if (STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Sitea == null)
			{
				STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Sitea = CallSite<Func<CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(STSAuthHelper)));
			}
			Func<CallSite, object, string> target = STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Sitea.Target;
			CallSite <>p__Sitea = STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Sitea;
			if (STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Siteb == null)
			{
				STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Siteb = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "OuterXml", typeof(STSAuthHelper), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			Func<CallSite, object, object> target2 = STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Siteb.Target;
			CallSite <>p__Siteb = STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Siteb;
			if (STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Sitec == null)
			{
				STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Sitec = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "TokenXml", typeof(STSAuthHelper), new CSharpArgumentInfo[]
				{
					CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
				}));
			}
			return target(<>p__Sitea, target2(<>p__Siteb, STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Sitec.Target(STSAuthHelper.<GetSTSToken>o__SiteContainer3.<>p__Sitec, arg4)));
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0005A244 File Offset: 0x00058444
		private static void SetProperty(object instance, string propertyName, object value)
		{
			Type type = instance.GetType();
			PropertyInfo property = type.GetProperty(propertyName);
			MethodInfo setMethod = property.GetSetMethod();
			setMethod.Invoke(instance, new object[]
			{
				value
			});
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x0005A27B File Offset: 0x0005847B
		private static TVal GetFieldValue<TVal>(Type type, string fieldName)
		{
			return (TVal)((object)type.GetField(fieldName).GetValue(null));
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0005A28F File Offset: 0x0005848F
		private static string GetSTSEndPoint(IHttpWebResponse response)
		{
			return response.Headers["X-MonoDevelop-STS-EndPoint"].SafeTrim();
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x0005A2A6 File Offset: 0x000584A6
		private static string GetCacheKey(Uri requestUri)
		{
			return "X-MonoDevelop-STS-Token|" + requestUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped);
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x0005A2BB File Offset: 0x000584BB
		private static string EncodeHeader(string token)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
		}

		/// <summary>
		/// Response header that specifies the WSTrust13 Windows Transport endpoint.
		/// </summary>
		/// <remarks>
		/// TODO: Is there a way to discover this \ negotiate this endpoint?
		/// </remarks>
		// Token: 0x040006BD RID: 1725
		private const string STSEndPointHeader = "X-MonoDevelop-STS-EndPoint";

		/// <summary>
		/// Response header that specifies the realm to authenticate for. In most cases this would be the gallery we are going up against.
		/// </summary>
		// Token: 0x040006BE RID: 1726
		private const string STSRealmHeader = "X-MonoDevelop-STS-Realm";

		/// <summary>
		/// Request header that contains the SAML token.
		/// </summary>
		// Token: 0x040006BF RID: 1727
		private const string STSTokenHeader = "X-MonoDevelop-STS-Token";

		// Token: 0x02000318 RID: 792
		[CompilerGenerated]
		private static class <GetSTSToken>o__SiteContainer3
		{
			// Token: 0x04000A7E RID: 2686
			public static CallSite<Func<CallSite, object, TrustVersion, object>> <>p__Site4;

			// Token: 0x04000A7F RID: 2687
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site5;

			// Token: 0x04000A80 RID: 2688
			public static CallSite<Func<CallSite, object, string, object>> <>p__Site6;

			// Token: 0x04000A81 RID: 2689
			public static CallSite<Action<CallSite, Type, object, string, object>> <>p__Site7;

			// Token: 0x04000A82 RID: 2690
			public static CallSite<Func<CallSite, object, object>> <>p__Site8;

			// Token: 0x04000A83 RID: 2691
			public static CallSite<Func<CallSite, object, object, object>> <>p__Site9;

			// Token: 0x04000A84 RID: 2692
			public static CallSite<Func<CallSite, object, string>> <>p__Sitea;

			// Token: 0x04000A85 RID: 2693
			public static CallSite<Func<CallSite, object, object>> <>p__Siteb;

			// Token: 0x04000A86 RID: 2694
			public static CallSite<Func<CallSite, object, object>> <>p__Sitec;
		}
	}
}
