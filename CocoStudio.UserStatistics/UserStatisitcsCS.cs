using System;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000015 RID: 21
	public class UserStatisitcsCS : BaseUserStatistics
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00003970 File Offset: 0x00001B70
		protected override void OnInit()
		{
			base.OnInit();
			this.monitor = new UserStatisticsMonitor();
			this.monitor.Version = base.EditorInfo.Version;
			this.monitor.Start();
			Tracker.SendFeatureInfoEvent += this.Tracker_SendFeatureInfoEvent;
			this.SendStartWay();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000039CB File Offset: 0x00001BCB
		public override void UnHandledException(Exception ex, string feedbackInfo)
		{
			this.monitor.TrackException(ex, feedbackInfo);
			base.UnHandledException(ex, feedbackInfo);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000039E8 File Offset: 0x00001BE8
		private void Tracker_SendFeatureInfoEvent(object sender, SendFeatureInfoEventArgs e)
		{
			foreach (FeatureInfo feature in e.FeatureList)
			{
				this.monitor.TrackFeature(base.EditorInfo.Type, feature, true);
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003A58 File Offset: 0x00001C58
		protected override void OnExit()
		{
			base.OnExit();
			int num = this.ConvertBoolToInt(base.IsStartFromLaunch());
			int num2 = (int)this.m_time / 1000;
			this.monitor.TrackSession("stop", base.EditorInfo.Type, num.ToString(), num2.ToString(), false);
			this.monitor.Stop();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003AC0 File Offset: 0x00001CC0
		public override void ExitAll()
		{
			foreach (FeatureInfo feature in Tracker.FeatureList)
			{
				this.monitor.TrackFeature(base.EditorInfo.Type, feature, false);
			}
			this.OnExit();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003B34 File Offset: 0x00001D34
		private void SendStartWay()
		{
			int num = this.ConvertBoolToInt(base.IsStartFromLaunch());
			this.monitor.TrackSession("start", base.EditorInfo.Type, num.ToString(), null, true);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003B74 File Offset: 0x00001D74
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

		// Token: 0x06000069 RID: 105 RVA: 0x00003B94 File Offset: 0x00001D94
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

		// Token: 0x04000072 RID: 114
		private UserStatisticsMonitor monitor;
	}
}
