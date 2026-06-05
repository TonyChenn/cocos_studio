using System;
using System.Collections.Generic;
using System.Linq;

namespace CocoStudio.UserStatistics
{
	// Token: 0x0200000D RID: 13
	public class Tracker
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002928 File Offset: 0x00000B28
		// (set) Token: 0x06000028 RID: 40 RVA: 0x0000293E File Offset: 0x00000B3E
		public static List<FeatureInfo> FeatureList { get; private set; } = new List<FeatureInfo>();

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000029 RID: 41 RVA: 0x00002948 File Offset: 0x00000B48
		// (remove) Token: 0x0600002A RID: 42 RVA: 0x00002984 File Offset: 0x00000B84
		public static event EventHandler<SendFeatureInfoEventArgs> SendFeatureInfoEvent;

		// Token: 0x0600002C RID: 44 RVA: 0x000029CC File Offset: 0x00000BCC
		public static void Add(ViewRegions region, string featureName, string methodName = "", string newValue = "")
		{
			FeatureInfo item = new FeatureInfo(region, featureName, methodName, newValue);
			Tracker.FeatureList.Add(item);
			if (Tracker.FeatureList.Count >= 10)
			{
				List<FeatureInfo> featureList = Tracker.FeatureList.ToList<FeatureInfo>();
				Tracker.FeatureList.Clear();
				if (Tracker.SendFeatureInfoEvent != null)
				{
					Tracker.SendFeatureInfoEvent(null, new SendFeatureInfoEventArgs(featureList));
				}
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002A3C File Offset: 0x00000C3C
		public static void Send(ViewRegions region, string featureName, string methodName = "", string newValue = "")
		{
			FeatureInfo item = new FeatureInfo(region, featureName, methodName, newValue);
			if (Tracker.SendFeatureInfoEvent != null)
			{
				Tracker.SendFeatureInfoEvent(null, new SendFeatureInfoEventArgs(new List<FeatureInfo>
				{
					item
				}));
			}
		}

		// Token: 0x0400004F RID: 79
		private const int CacheCount = 10;
	}
}
