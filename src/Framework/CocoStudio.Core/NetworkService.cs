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
	public class NetworkService : INetworkService
	{
		public static NetworkService Instance { get; private set; } = new NetworkService();

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

		public event EventHandler<NetworkChangedEventArgs> NetworkChanged;

		private NetworkService()
		{
		}

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

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.OnTryRequest();
		}

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

		public void TryRequest()
		{
			Task.Run(delegate()
			{
				this.OnTryRequest();
			});
		}

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

		public void Start()
		{
			this.TryRequest();
			this.timer.Start();
		}

		public void Stop()
		{
			this.timer.Stop();
		}

		private const int DefaultInterval = 300000;

		private bool isOK;

		private static bool HadSendData = false;

		private Timer timer;

		private string requestUrl;
	}
}
