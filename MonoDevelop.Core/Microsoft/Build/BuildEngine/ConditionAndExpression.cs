using System;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001DC RID: 476
	internal sealed class ConditionAndExpression : ConditionExpression
	{
		// Token: 0x06001210 RID: 4624 RVA: 0x000498B1 File Offset: 0x00047AB1
		public ConditionAndExpression(ConditionExpression left, ConditionExpression right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x000498C7 File Offset: 0x00047AC7
		public ConditionExpression Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x000498CF File Offset: 0x00047ACF
		public ConditionExpression Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x000498D7 File Offset: 0x00047AD7
		public override bool BoolEvaluate(IExpressionContext context)
		{
			return this.left.BoolEvaluate(context) && this.right.BoolEvaluate(context);
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x000498FA File Offset: 0x00047AFA
		public override float NumberEvaluate(IExpressionContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00049901 File Offset: 0x00047B01
		public override string StringEvaluate(IExpressionContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00049908 File Offset: 0x00047B08
		public override bool CanEvaluateToBool(IExpressionContext context)
		{
			return this.left.CanEvaluateToBool(context) && this.right.CanEvaluateToBool(context);
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00049926 File Offset: 0x00047B26
		public override bool CanEvaluateToNumber(IExpressionContext context)
		{
			return false;
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x00049929 File Offset: 0x00047B29
		public override bool CanEvaluateToString(IExpressionContext context)
		{
			return false;
		}

		// Token: 0x0400052F RID: 1327
		private readonly ConditionExpression left;

		// Token: 0x04000530 RID: 1328
		private readonly ConditionExpression right;
	}
}
