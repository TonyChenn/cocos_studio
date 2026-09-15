using System;
using System.Collections;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using PostSharp.Extensibility;

namespace CocoStudio.UserStatistics
{
	[HasInheritedAttribute(new long[]
	{

	})]
	[Serializable]
	public class TracePropertyListCountAttribute : TracePropertyCallAttribute
	{
		public TracePropertyListCountAttribute(ViewRegions region, string feature) : base(region, feature)
		{
		}

		[HasInheritedAttribute(new long[]
		{
			2905153543183646119L
		})]
		[LocationInterceptionAdviceOptimization(LocationInterceptionAdviceOptimizations.IgnoreGetLocation | LocationInterceptionAdviceOptimizations.IgnoreGetLocationFullName)]
		public override void OnSetValue(LocationInterceptionArgs args)
		{
			base.OnSetValue(args);
			IList list = args.Value as IList;
			int num = 0;
			if (list != null)
			{
				num = list.Count;
			}
			Tracker.Add(this.region, this.featureName, args.LocationName, num.ToString());
		}
	}
}
