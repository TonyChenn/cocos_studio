using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Timers;
using Cocos.Launcher.Library;
using CocoStudio.UserStatistics;
using GLib;

namespace CocoStudio.Core
{
	// Token: 0x0200002F RID: 47
	public class NetworkService : INetworkService
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00008700 File Offset: 0x00006900
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00008716 File Offset: 0x00006916
		public static NetworkService Instance { get; private set; } = new NetworkService();

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00008734 File Offset: 0x00006934
		// (set) Token: 0x060001BB RID: 443 RVA: 0x0000878C File Offset: 0x0000698C
		public bool IsOK
		{
			get
			{
				return this.isOK;
			}
			private set
			{
				if (this.isOK != value)
				{
					this.isOK = value;
					if (this.NetworkChanged != null)
					{
						Timeout.Add(0U, delegate
						{
							this.NetworkChanged(this, new NetworkChangedEventArgs(value));
							return false;
						});
					}
					if (this.isOK && !NetworkService.HadSendData)
					{
						NetworkService.HadSendData = true;
						Tracker.Add(ViewRegions.None, "FirstNetwork", "", "");
					}
				}
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060001BC RID: 444 RVA: 0x00008830 File Offset: 0x00006A30
		// (remove) Token: 0x060001BD RID: 445 RVA: 0x0000886C File Offset: 0x00006A6C
		public event EventHandler<NetworkChangedEventArgs> NetworkChanged;

		// Token: 0x060001BE RID: 446 RVA: 0x000088A8 File Offset: 0x00006AA8
		private NetworkService()
		{
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000088B4 File Offset: 0x00006AB4
		public void Intinalize(string requestUrl, int? interval)
		{
			this.requestUrl = requestUrl;
			int num = 300000;
			if (interval != null)
			{
				num = interval.Value;
			}
			this.timer = new Timer((double)num);
			this.timer.Elapsed += this.Timer_Elapsed;
			this.Start();
			NetworkChange.NetworkAvailabilityChanged += this.NetworkChange_NetworkAvailabilityChanged;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008940 File Offset: 0x00006B40
		private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
		{
			if (e.IsAvailable)
			{
				Timeout.Add(5000U, delegate
				{
					this.Start();
					return false;
				});
			}
			else
			{
				this.Stop();
				this.IsOK = false;
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00008991 File Offset: 0x00006B91
		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.OnTryRequest();
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000899C File Offset: 0x00006B9C
		private void OnTryRequest()
		{
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				if (this.IsRequest())
				{
					flag = true;
					break;
				}
			}
			this.IsOK = flag;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000089E1 File Offset: 0x00006BE1
		public void TryRequest()
		{
			Task.Run(delegate()
			{
				this.OnTryRequest();
			});
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000089F8 File Offset: 0x00006BF8
		private bool IsRequest()
		{
			bool result = true;
			WebResponse webResponse = null;
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = this.requestUrl.CreatRequest("get");
				webResponse = httpWebRequest.GetResponse();
			}
			catch (Exception)
			{
				result = false;
			}
			finally
			{
				if (webResponse != null)
				{
					webResponse.Close();
				}
				if (httpWebRequest != null)
				{
					httpWebRequest.Abort();
				}
			}
			return result;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008A7C File Offset: 0x00006C7C
		public void Start()
		{
			this.TryRequest();
			this.timer.Start();
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008A92 File Offset: 0x00006C92
		public void Stop()
		{
			this.timer.Stop();
		}

		// Token: 0x040000ED RID: 237
		private const int DefaultInterval = 300000;

		// Token: 0x040000EE RID: 238
		private bool isOK;

		// Token: 0x040000EF RID: 239
		private static bool HadSendData = false;

		// Token: 0x040000F0 RID: 240
		private Timer timer;

		// Token: 0x040000F1 RID: 241
		private string requestUrl;
	}
}
