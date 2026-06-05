using System;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001DF RID: 479
	internal sealed class ConditionNotExpression : ConditionExpression
	{
		// Token: 0x0600122B RID: 4651 RVA: 0x00049CE8 File Offset: 0x00047EE8
		public ConditionNotExpression(ConditionExpression expression)
		{
			this.expression = expression;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00049CF7 File Offset: 0x00047EF7
		public override bool BoolEvaluate(IExpressionContext context)
		{
			return !this.expression.BoolEvaluate(context);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00049D08 File Offset: 0x00047F08
		public override float NumberEvaluate(IExpressionContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00049D0F File Offset: 0x00047F0F
		public override string StringEvaluate(IExpressionContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00049D16 File Offset: 0x00047F16
		public override bool CanEvaluateToBool(IExpressionContext context)
		{
			return this.expression.CanEvaluateToBool(context);
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00049D24 File Offset: 0x00047F24
		public override bool CanEvaluateToNumber(IExpressionContext context)
		{
			return false;
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00049D27 File Offset: 0x00047F27
		public override bool CanEvaluateToString(IExpressionContext context)
		{
			return false;
		}

		// Token: 0x04000538 RID: 1336
		private readonly ConditionExpression expression;
	}
}
