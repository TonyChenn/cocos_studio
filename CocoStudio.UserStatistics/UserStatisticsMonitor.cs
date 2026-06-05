using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using CocoStudio.Basic;
using Newtonsoft.Json.Linq;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000010 RID: 16
	internal class UserStatisticsMonitor
	{
		// Token: 0x0600003D RID: 61
		[DllImport("zlib1.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int compress(byte[] dest, ref ulong destLen, byte[] source, ulong sourceLen);

		// Token: 0x0600003E RID: 62 RVA: 0x00002C10 File Offset: 0x00000E10
		public void Start()
		{
			this.SessionID = new Random().Next().ToString();
			this.localInfo = new LocalInfo();
			this.installID = this.Hash(this.localInfo.MACAddress);
			this.sessionStart = this.GetTimeStamp();
			this.GenDeviceInfo();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002C6B File Offset: 0x00000E6B
		public void Stop()
		{
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002C70 File Offset: 0x00000E70
		public void TrackSession(string actionName, string editorType, string isFromLaunch, string timeSpend = null, bool isAsync = true)
		{
			JObject jobject = new JObject();
			jobject.Add("action", actionName);
			jobject.Add("editor_name", editorType);
			jobject.Add("isStartFromLaunch", isFromLaunch);
			if (!string.IsNullOrEmpty(timeSpend))
			{
				jobject.Add("time_spent", timeSpend);
			}
			long timeStamp = this.GetTimeStamp();
			JObject eventInfo = this.GetEventInfo("Session");
			eventInfo.Add("u", "");
			eventInfo.Add("p", jobject);
			this.SendDataHandling(eventInfo, isAsync);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002D18 File Offset: 0x00000F18
		public void TrackException(Exception ex, string feedbackInfo)
		{
			JObject jobject = new JObject();
			jobject.Add("stack_trace", ex.GetAllStackTrace().Replace("\n", "<br>"));
			jobject.Add("exception_type", ex.Message);
			jobject.Add("context", ex.Source);
			if (!string.IsNullOrEmpty(feedbackInfo))
			{
				jobject.Add("feedbackinfo", feedbackInfo);
			}
			JObject eventInfo = this.GetEventInfo("Bug");
			eventInfo.Add("p", jobject);
			eventInfo.Add("u", "");
			this.SendDataHandling(eventInfo, false);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002DD4 File Offset: 0x00000FD4
		public void TrackFeature(string editorType, FeatureInfo feature, bool isAsync = true)
		{
			JObject jobject = new JObject();
			jobject.Add("editor_name", editorType);
			jobject.Add("region_name", feature.Region.ToString());
			jobject.Add("feature_name", feature.FeatureName);
			jobject.Add("method_name", feature.MethodName);
			jobject.Add("new_value", feature.NewValue);
			JObject eventInfo = this.GetEventInfo("FeatureUse");
			eventInfo.Add("p", jobject);
			eventInfo.Add("u", "");
			this.SendDataHandling(eventInfo, isAsync);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002E98 File Offset: 0x00001098
		private void SendDataHandling(JObject eventInfo, bool isAsync)
		{
			try
			{
				if (!this.networkChecked)
				{
					this.CheckNetwork();
				}
				if (isAsync)
				{
					this.PostStatisticsAsync(eventInfo);
				}
				else
				{
					this.PostStatistics(eventInfo);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("发送数据信息到服务器失败", exception);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002EFC File Offset: 0x000010FC
		private void PostStatisticsAsync(JObject eventInfo)
		{
			this.Send(eventInfo, true);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002F08 File Offset: 0x00001108
		private void PostStatistics(JObject eventInfo)
		{
			this.Send(eventInfo, false);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002F14 File Offset: 0x00001114
		private void Send(JObject eventInfo, bool isAsync)
		{
			if (this.networkOK)
			{
				string s = new JObject
				{
					{
						"time",
						this.GetTimeStamp()
					},
					{
						"device",
						this.deviceInfo
					},
					{
						"events",
						new JArray
						{
							eventInfo
						}
					}
				}.ToString();
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				byte[] array = new byte[bytes.Length];
				ulong num = (ulong)((long)array.Length);
				ulong sourceLen = (ulong)((long)bytes.Length);
				int num2 = UserStatisticsMonitor.compress(array, ref num, bytes, sourceLen);
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://cs.ucenter.appget.cn/csas");
				httpWebRequest.ContentType = "multipart/form-data";
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentLength = (long)array.Length;
				httpWebRequest.Timeout = 20000;
				try
				{
					using (Stream requestStream = httpWebRequest.GetRequestStream())
					{
						requestStream.Write(array, 0, array.Length);
						requestStream.Close();
					}
					if (isAsync)
					{
						RequestState state = new RequestState(httpWebRequest);
						IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(new AsyncCallback(this.ResponseCallback), state);
					}
					else
					{
						HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
						{
							string text = streamReader.ReadToEnd();
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000030E4 File Offset: 0x000012E4
		private void ResponseCallback(IAsyncResult asyncResult)
		{
			try
			{
				RequestState requestState = (RequestState)asyncResult.AsyncState;
				HttpWebRequest request = requestState.Request;
				Stream responseStream = request.EndGetResponse(asyncResult).GetResponseStream();
				using (StreamReader streamReader = new StreamReader(responseStream))
				{
					string text = streamReader.ReadToEnd();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000316C File Offset: 0x0000136C
		private void GenDeviceInfo()
		{
			this.deviceInfo = new JObject();
			this.deviceInfo.Add("4", this.installID);
			this.deviceInfo.Add("5", this.Version.ToString());
			this.deviceInfo.Add("6", this.localInfo.FrameVer);
			this.deviceInfo.Add("7", this.localInfo.OSName);
			this.deviceInfo.Add("8", this.localInfo.OSVersion);
			this.deviceInfo.Add("9", this.localInfo.ChannelNum);
			this.deviceInfo.Add("10", this.localInfo.OSBit);
			this.deviceInfo.Add("11", this.localInfo.CPUCoreCount);
			this.deviceInfo.Add("12", this.localInfo.OSLan);
			this.deviceInfo.Add("13", this.localInfo.ScreenResulotion);
			this.deviceInfo.Add("14", this.localInfo.ScreenCount);
			this.deviceInfo.Add("15", this.localInfo.ScreenDpi);
			this.deviceInfo.Add("16", this.localInfo.MACAddress);
			this.deviceInfo.Add("17", this.localInfo.isVM);
			this.deviceInfo.Add("18", "398332332");
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000336C File Offset: 0x0000156C
		private long GetTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000033A8 File Offset: 0x000015A8
		public string Hash(string source)
		{
			string result;
			using (MD5 md = MD5.Create())
			{
				byte[] inArray = md.ComputeHash(Encoding.UTF8.GetBytes(source));
				result = Convert.ToBase64String(inArray);
			}
			return result;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000033FC File Offset: 0x000015FC
		public void TrackUninstall(string uninstallreason)
		{
			JObject reason = this.GetReason(uninstallreason);
			JObject eventInfo = this.GetEventInfo("UninstallReasons");
			this.Send(eventInfo, false);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003428 File Offset: 0x00001628
		private JObject GetEventInfo(string eventName)
		{
			long timeStamp = this.GetTimeStamp();
			return new JObject
			{
				{
					"s",
					this.SessionID
				},
				{
					"e",
					eventName
				},
				{
					"t",
					timeStamp
				},
				{
					"d",
					timeStamp - this.sessionStart
				}
			};
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000349C File Offset: 0x0000169C
		private JObject GetReason(string reason)
		{
			JObject jobject = new JObject();
			JObject result;
			if (string.IsNullOrWhiteSpace(reason))
			{
				result = jobject;
			}
			else
			{
				string[] array = reason.Split(new char[]
				{
					'|'
				});
				if (array != null && array.Length > 0)
				{
					foreach (string text in array)
					{
						string[] array3 = text.Split(new char[]
						{
							'#'
						});
						if (array3 != null && array3.Length == 2)
						{
							jobject.Add(array3[0], array3[1]);
						}
					}
				}
				result = jobject;
			}
			return result;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003564 File Offset: 0x00001764
		private void CheckNetwork()
		{
			this.networkChecked = true;
			try
			{
				Ping ping = new Ping();
				PingReply pingReply = ping.Send(IPAddress.Parse("8.8.4.4"));
				if (pingReply != null && pingReply.Status == IPStatus.Success)
				{
					this.networkOK = true;
				}
				else
				{
					this.networkOK = false;
				}
			}
			catch (Exception)
			{
				this.networkOK = false;
			}
		}

		// Token: 0x04000057 RID: 87
		private const string StatisticUrl = "http://cs.ucenter.appget.cn/csas";

		// Token: 0x04000058 RID: 88
		private const string AppID = "398332332";

		// Token: 0x04000059 RID: 89
		public const string EVENT_BUG = "Bug";

		// Token: 0x0400005A RID: 90
		public const string EVENT_FEATURE_USE = "FeatureUse";

		// Token: 0x0400005B RID: 91
		public const string EVENT_INSTALL = "Install";

		// Token: 0x0400005C RID: 92
		public const string EVENT_SESSION = "Session";

		// Token: 0x0400005D RID: 93
		public Version Version;

		// Token: 0x0400005E RID: 94
		private JObject deviceInfo;

		// Token: 0x0400005F RID: 95
		private string SessionID;

		// Token: 0x04000060 RID: 96
		private bool networkChecked;

		// Token: 0x04000061 RID: 97
		private bool networkOK;

		// Token: 0x04000062 RID: 98
		private LocalInfo localInfo;

		// Token: 0x04000063 RID: 99
		private long sessionStart;

		// Token: 0x04000064 RID: 100
		private string installID;
	}
}
