using System;
using System.Collections.Generic;

namespace CocoStudio.UserStatistics
{
	public class SendFeatureInfoEventArgs : EventArgs
	{
		public List<FeatureInfo> FeatureList { get; set; }

		public SendFeatureInfoEventArgs(List<FeatureInfo> featureList)
		{
			this.FeatureList = featureList;
		}
	}
}
