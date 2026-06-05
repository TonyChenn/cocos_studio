using System;

namespace CocoStudio.UserStatistics
{
	// Token: 0x0200000F RID: 15
	public class FeatureInfo
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002AC0 File Offset: 0x00000CC0
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002AD7 File Offset: 0x00000CD7
		public ViewRegions Region { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002AE0 File Offset: 0x00000CE0
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002AF7 File Offset: 0x00000CF7
		public string FeatureName { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002B00 File Offset: 0x00000D00
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002B17 File Offset: 0x00000D17
		public string MethodName { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002B20 File Offset: 0x00000D20
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002B37 File Offset: 0x00000D37
		public string NewValue { get; private set; }

		// Token: 0x0600003A RID: 58 RVA: 0x00002B40 File Offset: 0x00000D40
		public FeatureInfo(ViewRegions region, string featureName, string methodName, string newValue)
		{
			this.Region = region;
			this.FeatureName = featureName;
			this.MethodName = methodName;
			this.NewValue = newValue;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002B6C File Offset: 0x00000D6C
		public bool Equals(FeatureInfo obj)
		{
			return this.Region.Equals(obj.Region) && this.FeatureName.Equals(obj.FeatureName);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002BBC File Offset: 0x00000DBC
		public string ToString(string editorType)
		{
			return string.Format("{0}_{1}_{2}_{3}_{4}", new object[]
			{
				editorType,
				this.Region,
				this.FeatureName,
				this.MethodName,
				this.NewValue
			});
		}
	}
}
