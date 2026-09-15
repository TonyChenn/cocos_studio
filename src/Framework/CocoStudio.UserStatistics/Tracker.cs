using System;
using System.Collections.Generic;
using System.Linq;

namespace CocoStudio.UserStatistics
{
	public class Tracker
	{
		public static List<FeatureInfo> FeatureList { get; private set; } = new List<FeatureInfo>();

		public static event EventHandler<SendFeatureInfoEventArgs> SendFeatureInfoEvent;

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

		private const int CacheCount = 10;
	}
}
