using System;
using System.Collections.Generic;

namespace CocoStudio.UserStatistics
{
	// Token: 0x0200000E RID: 14
	public class SendFeatureInfoEventArgs : EventArgs
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002A8C File Offset: 0x00000C8C
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002AA3 File Offset: 0x00000CA3
		public List<FeatureInfo> FeatureList { get; set; }

		// Token: 0x06000031 RID: 49 RVA: 0x00002AAC File Offset: 0x00000CAC
		public SendFeatureInfoEventArgs(List<FeatureInfo> featureList)
		{
			this.FeatureList = featureList;
		}
	}
}
