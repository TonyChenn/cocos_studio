using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public sealed class TraceInitializeAttribute : OnMethodBoundaryAspect
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00003617 File Offset: 0x00001817
		[MethodExecutionAdviceOptimization(MethodExecutionAdviceOptimizations.IgnoreAllEventArgsMembers)]
		public override void OnExit(MethodExecutionArgs args)
		{
			base.OnExit(args);
		}
	}
}
