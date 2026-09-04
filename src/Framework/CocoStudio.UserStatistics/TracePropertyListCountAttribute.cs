using System;
using System.Collections;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using PostSharp.Extensibility;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000005 RID: 5
	[HasInheritedAttribute(new long[]
	{

	})]
	[Serializable]
	public class TracePropertyListCountAttribute : TracePropertyCallAttribute
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002157 File Offset: 0x00000357
		public TracePropertyListCountAttribute(ViewRegions region, string feature) : base(region, feature)
		{
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002164 File Offset: 0x00000364
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
