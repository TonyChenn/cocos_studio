using System;

namespace CocoStudio.UserStatistics
{
	public class UserStatisitcsCS : BaseUserStatistics
	{
		protected override void OnInit()
		{
			base.OnInit();
			this.monitor = new UserStatisticsMonitor();
			this.monitor.Version = base.EditorInfo.Version;
			this.monitor.Start();
			Tracker.SendFeatureInfoEvent += this.Tracker_SendFeatureInfoEvent;
			this.SendStartWay();
		}

		public override void UnHandledException(Exception ex, string feedbackInfo)
		{
			this.monitor.TrackException(ex, feedbackInfo);
			base.UnHandledException(ex, feedbackInfo);
		}

		private void Tracker_SendFeatureInfoEvent(object sender, SendFeatureInfoEventArgs e)
		{
			foreach (FeatureInfo feature in e.FeatureList)
			{
				this.monitor.TrackFeature(base.EditorInfo.Type, feature, true);
			}
		}

		protected override void OnExit()
		{
			base.OnExit();
			int num = this.ConvertBoolToInt(base.IsStartFromLaunch());
			int num2 = (int)this.m_time / 1000;
			this.monitor.TrackSession("stop", base.EditorInfo.Type, num.ToString(), num2.ToString(), false);
			this.monitor.Stop();
		}

		public override void ExitAll()
		{
			foreach (FeatureInfo feature in Tracker.FeatureList)
			{
				this.monitor.TrackFeature(base.EditorInfo.Type, feature, false);
			}
			this.OnExit();
		}

		private void SendStartWay()
		{
			int num = this.ConvertBoolToInt(base.IsStartFromLaunch());
			this.monitor.TrackSession("start", base.EditorInfo.Type, num.ToString(), null, true);
		}

		private int ConvertBoolToInt(bool param)
		{
			int result;
			if (param)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public void SendUninstallReasons(string uninstallReasons)
		{
			if (this.monitor == null)
			{
				this.monitor = new UserStatisticsMonitor();
				this.monitor.Version = new Version("2.3.3.0");
				this.monitor.Start();
			}
			this.monitor.TrackUninstall(uninstallReasons);
		}

		private UserStatisticsMonitor monitor;
	}
}
