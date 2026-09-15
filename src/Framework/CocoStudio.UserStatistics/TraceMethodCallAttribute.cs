using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;

namespace CocoStudio.UserStatistics
{
	[Serializable]
	public sealed class TraceMethodCallAttribute : OnMethodBoundaryAspect
	{
		public TraceMethodCallAttribute(string functionName, string operationName = null, string labelName = null)
		{
			this.region = ViewRegions.None;
			this.featureName = functionName;
			this.OperationName = operationName;
			this.LabelName = labelName;
		}

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

		private ViewRegions region;

		private string featureName;

		private string OperationName;

		private string LabelName;
	}
}
