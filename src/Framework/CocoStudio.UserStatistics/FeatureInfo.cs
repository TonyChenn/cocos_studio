using System;

namespace CocoStudio.UserStatistics
{
	public class FeatureInfo
	{
		public ViewRegions Region { get; private set; }

		public string FeatureName { get; private set; }

		public string MethodName { get; private set; }

		public string NewValue { get; private set; }

		public FeatureInfo(ViewRegions region, string featureName, string methodName, string newValue)
		{
			this.Region = region;
			this.FeatureName = featureName;
			this.MethodName = methodName;
			this.NewValue = newValue;
		}

		public bool Equals(FeatureInfo obj)
		{
			return this.Region.Equals(obj.Region) && this.FeatureName.Equals(obj.FeatureName);
		}

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
