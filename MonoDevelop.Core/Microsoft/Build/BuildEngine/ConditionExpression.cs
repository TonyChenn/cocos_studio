using System;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001DB RID: 475
	internal abstract class ConditionExpression
	{
		// Token: 0x06001209 RID: 4617
		public abstract bool BoolEvaluate(IExpressionContext context);

		// Token: 0x0600120A RID: 4618
		public abstract float NumberEvaluate(IExpressionContext context);

		// Token: 0x0600120B RID: 4619
		public abstract string StringEvaluate(IExpressionContext context);

		// Token: 0x0600120C RID: 4620
		public abstract bool CanEvaluateToBool(IExpressionContext context);

		// Token: 0x0600120D RID: 4621
		public abstract bool CanEvaluateToNumber(IExpressionContext context);

		// Token: 0x0600120E RID: 4622
		public abstract bool CanEvaluateToString(IExpressionContext context);
	}
}
