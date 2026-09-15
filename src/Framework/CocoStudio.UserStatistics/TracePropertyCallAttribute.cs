using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using PostSharp.Extensibility;

namespace CocoStudio.UserStatistics
{
	[HasInheritedAttribute(new long[]
	{

	})]
	[Serializable]
	public class TracePropertyCallAttribute : LocationInterceptionAspect
	{
		public TracePropertyCallAttribute(ViewRegions region, string feature)
		{
			this.region = region;
			this.featureName = feature;
		}

		[HasInheritedAttribute(new long[]
		{
			2905153543183646119L
		})]
		[LocationInterceptionAdviceOptimization(LocationInterceptionAdviceOptimizations.IgnoreGetLocation | LocationInterceptionAdviceOptimizations.IgnoreGetLocationFullName)]
		public override void OnSetValue(LocationInterceptionArgs args)
		{
			base.OnSetValue(args);
			Tracker.Add(this.region, this.featureName, args.LocationName, args.Value.ToString());
		}

		protected ViewRegions region;

		protected string featureName;
	}
}
