using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using PostSharp.Extensibility;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000004 RID: 4
	[HasInheritedAttribute(new long[]
	{

	})]
	[Serializable]
	public class TracePropertyCallAttribute : LocationInterceptionAspect
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002110 File Offset: 0x00000310
		public TracePropertyCallAttribute(ViewRegions region, string feature)
		{
			this.region = region;
			this.featureName = feature;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002129 File Offset: 0x00000329
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

		// Token: 0x04000005 RID: 5
		protected ViewRegions region;

		// Token: 0x04000006 RID: 6
		protected string featureName;
	}
}
