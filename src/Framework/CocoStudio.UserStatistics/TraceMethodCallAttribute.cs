using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;

namespace CocoStudio.UserStatistics
{
	// Token: 0x02000002 RID: 2
	[Serializable]
	public sealed class TraceMethodCallAttribute : OnMethodBoundaryAspect
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public TraceMethodCallAttribute(string functionName, string operationName = null, string labelName = null)
		{
			this.region = ViewRegions.None;
			this.featureName = functionName;
			this.OperationName = operationName;
			this.LabelName = labelName;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002078 File Offset: 0x00000278
		[MethodExecutionAdviceOptimization(MethodExecutionAdviceOptimizations.IgnoreSetFlowBehavior | MethodExecutionAdviceOptimizations.IgnoreGetArguments | MethodExecutionAdviceOptimizations.IgnoreSetArguments | MethodExecutionAdviceOptimizations.IgnoreGetInstance | MethodExecutionAdviceOptimizations.IgnoreSetInstance | MethodExecutionAdviceOptimizations.IgnoreGetException | MethodExecutionAdviceOptimizations.IgnoreSetReturnValue | MethodExecutionAdviceOptimizations.IgnoreGetMethodExecutionTag | MethodExecutionAdviceOptimizations.IgnoreSetMethodExecutionTag | MethodExecutionAdviceOptimizations.IgnoreGetYieldValue | MethodExecutionAdviceOptimizations.IgnoreSetYieldValue | MethodExecutionAdviceOptimizations.IgnoreGetDeclarationIdentifier)]
		public override void OnExit(MethodExecutionArgs args)
		{
			base.OnExit(args);
			string methodName;
			if (this.OperationName == null && args.Method != null)
			{
				methodName = args.Method.Name;
			}
			else
			{
				methodName = this.OperationName;
			}
			string newValue;
			if (this.LabelName == null && args.ReturnValue != null)
			{
				newValue = args.ReturnValue.ToString();
			}
			else
			{
				newValue = this.LabelName;
			}
			Tracker.Add(this.region, this.featureName, methodName, newValue);
		}

		// Token: 0x04000001 RID: 1
		private ViewRegions region;

		// Token: 0x04000002 RID: 2
		private string featureName;

		// Token: 0x04000003 RID: 3
		private string OperationName;

		// Token: 0x04000004 RID: 4
		private string LabelName;
	}
}
