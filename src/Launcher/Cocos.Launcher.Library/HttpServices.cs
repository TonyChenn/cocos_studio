using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CocoStudio.Basic;

namespace Cocos.Launcher.Library
{
	// Token: 0x02000005 RID: 5
	public static class HttpServices
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002828 File Offset: 0x00000A28
		static HttpServices()
		{
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback((object se, X509Certificate cert, X509Chain chain, SslPolicyErrors sslerror) => true));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002898 File Offset: 0x00000A98
		public static HttpWebRequest CreatRequest(this string url, string method = "get")
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.KeepAlive = false;
			ServicePointManager.DefaultConnectionLimit = 50;
			httpWebRequest.Method = method.ToUpper();
			httpWebRequest.AllowAutoRedirect = true;
			httpWebRequest.CookieContainer = HttpServices.CookieContainers;
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.UserAgent = HttpServices.IE7;
			httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
			httpWebRequest.Timeout = 10000;
			return httpWebRequest;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000290C File Offset: 0x00000B0C
		public static Stream GetResponseOfStream(this string url, string method = "get", string data = "")
		{
			Stream result;
			try
			{
				HttpWebRequest httpWebRequest = url.CreatRequest(method);
				if (method.ToUpper() == "POST" && data != null)
				{
					byte[] bytes = new ASCIIEncoding().GetBytes(data);
					httpWebRequest.ContentLength = (long)bytes.Length;
					Stream requestStream = httpWebRequest.GetRequestStream();
					requestStream.Write(bytes, 0, bytes.Length);
					requestStream.Close();
				}
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using (Stream responseStream = httpWebResponse.GetResponseStream())
				{
					MemoryStream memoryStream = new MemoryStream();
					responseStream.CopyTo(memoryStream);
					memoryStream.Position = 0L;
					httpWebResponse.Close();
					httpWebRequest.Abort();
					result = memoryStream;
				}
			}
			catch
			{
				LogConfig.Output.Error("读取服务器数据失败");
				result = null;
			}
			return result;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000029E4 File Offset: 0x00000BE4
		public static string GetResponseOfString(this string url, string method = "get", string data = "")
		{
			string result;
			try
			{
				HttpWebRequest httpWebRequest = url.CreatRequest(method);
				if (method.ToUpper() == "POST" && data != null)
				{
					byte[] bytes = new ASCIIEncoding().GetBytes(data);
					httpWebRequest.ContentLength = (long)bytes.Length;
					Stream requestStream = httpWebRequest.GetRequestStream();
					requestStream.Write(bytes, 0, bytes.Length);
					requestStream.Close();
				}
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				foreach (object obj in httpWebResponse.Cookies)
				{
					Cookie cookie = (Cookie)obj;
					HttpServices.Cookies[cookie.Name] = cookie.Value;
					HttpServices.CookieContainers.Add(cookie);
				}
				using (Stream responseStream = httpWebResponse.GetResponseStream())
				{
					string text = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
					httpWebResponse.Close();
					httpWebRequest.Abort();
					result = text;
				}
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x04000018 RID: 24
		public static string IE7 = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; InfoPath.2; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET4.0C; .NET4.0E; COCOS " + Option.EditorVersion.ToString() + ")";

		// Token: 0x04000019 RID: 25
		public static CookieContainer CookieContainers = new CookieContainer();

		// Token: 0x0400001A RID: 26
		public static Dictionary<string, string> Cookies = new Dictionary<string, string>();
	}
}
