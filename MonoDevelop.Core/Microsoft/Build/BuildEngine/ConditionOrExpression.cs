using System;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001E0 RID: 480
	internal sealed class ConditionOrExpression : ConditionExpression
	{
		// Token: 0x06001232 RID: 4658 RVA: 0x00049D2A File Offset: 0x00047F2A
		public ConditionOrExpression(ConditionExpression left, ConditionExpression right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001233 RID: 4659 RVA: 0x00049D40 File Offset: 0x00047F40
		public ConditionExpression Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x00049D48 File Offset: 0x00047F48
		public ConditionExpression Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00049D50 File Offset: 0x00047F50
		public override bool BoolEvaluate(IExpressionContext context)
		{
			return this.left.BoolEvaluate(context) || this.right.BoolEvaluate(context);
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00049D73 File Offset: 0x00047F73
		public override float NumberEvaluate(IExpressionContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x00049D7A File Offset: 0x00047F7A
		public override string StringEvaluate(IExpressionContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x00049D81 File Offset: 0x00047F81
		public override bool CanEvaluateToBool(IExpressionContext context)
		{
			return true;
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00049D84 File Offset: 0x00047F84
		public override bool CanEvaluateToNumber(IExpressionContext context)
		{
			return false;
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00049D87 File Offset: 0x00047F87
		public override bool CanEvaluateToString(IExpressionContext context)
		{
			return false;
		}

		// Token: 0x04000539 RID: 1337
		private readonly ConditionExpression left;

		// Token: 0x0400053A RID: 1338
		private readonly ConditionExpression right;
	}
}
