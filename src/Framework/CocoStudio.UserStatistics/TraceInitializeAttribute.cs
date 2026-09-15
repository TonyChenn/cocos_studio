using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;

namespace CocoStudio.UserStatistics
{
	[Serializable]
	public sealed class TraceInitializeAttribute : OnMethodBoundaryAspect
	{
		[MethodExecutionAdviceOptimization(MethodExecutionAdviceOptimizations.IgnoreAllEventArgsMembers)]
		public override void OnExit(MethodExecutionArgs args)
		{
			base.OnExit(args);
		}
	}
}
