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
	internal class UserStatisticsMonitor
	{
		[DllImport("zlib1.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int compress(byte[] dest, ref ulong destLen, byte[] source, ulong sourceLen);

		public void Start()
		{
			this.SessionID = new Random().Next().ToString();
			this.localInfo = new LocalInfo();
			this.installID = this.Hash(this.localInfo.MACAddress);
			this.sessionStart = this.GetTimeStamp();
			this.GenDeviceInfo();
		}

		public void Stop()
		{
		}

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

		private void PostStatisticsAsync(JObject eventInfo)
		{
			this.Send(eventInfo, true);
		}

		private void PostStatistics(JObject eventInfo)
		{
			this.Send(eventInfo, false);
		}

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

		private long GetTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
		}

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

		public void TrackUninstall(string uninstallreason)
		{
			JObject reason = this.GetReason(uninstallreason);
			JObject eventInfo = this.GetEventInfo("UninstallReasons");
			this.Send(eventInfo, false);
		}

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

		private const string StatisticUrl = "http://cs.ucenter.appget.cn/csas";

		private const string AppID = "398332332";

		public const string EVENT_BUG = "Bug";

		public const string EVENT_FEATURE_USE = "FeatureUse";

		public const string EVENT_INSTALL = "Install";

		public const string EVENT_SESSION = "Session";

		public Version Version;

		private JObject deviceInfo;

		private string SessionID;

		private bool networkChecked;

		private bool networkOK;

		private LocalInfo localInfo;

		private long sessionStart;

		private string installID;
	}
}
