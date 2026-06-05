using System;
using System.Collections;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001E2 RID: 482
	internal sealed class ConditionRelationalExpression : ConditionExpression
	{
		// Token: 0x06001249 RID: 4681 RVA: 0x0004A2E3 File Offset: 0x000484E3
		public ConditionRelationalExpression(ConditionExpression left, ConditionExpression right, RelationOperator op)
		{
			this.left = left;
			this.right = right;
			this.op = op;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0004A300 File Offset: 0x00048500
		public override bool BoolEvaluate(IExpressionContext context)
		{
			if (this.left.CanEvaluateToNumber(context) && this.right.CanEvaluateToNumber(context))
			{
				float l = this.left.NumberEvaluate(context);
				float r = this.right.NumberEvaluate(context);
				return ConditionRelationalExpression.NumberCompare(l, r, this.op);
			}
			if (this.left.CanEvaluateToBool(context) && this.right.CanEvaluateToBool(context))
			{
				bool l2 = this.left.BoolEvaluate(context);
				bool r2 = this.right.BoolEvaluate(context);
				return ConditionRelationalExpression.BoolCompare(l2, r2, this.op);
			}
			string l3 = this.left.StringEvaluate(context);
			string r3 = this.right.StringEvaluate(context);
			return ConditionRelationalExpression.StringCompare(l3, r3, this.op);
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0004A3C0 File Offset: 0x000485C0
		public override float NumberEvaluate(IExpressionContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0004A3C7 File Offset: 0x000485C7
		public override string StringEvaluate(IExpressionContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0004A3CE File Offset: 0x000485CE
		public override bool CanEvaluateToBool(IExpressionContext context)
		{
			return true;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0004A3D1 File Offset: 0x000485D1
		public override bool CanEvaluateToNumber(IExpressionContext context)
		{
			return false;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0004A3D4 File Offset: 0x000485D4
		public override bool CanEvaluateToString(IExpressionContext context)
		{
			return false;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0004A3D8 File Offset: 0x000485D8
		private static bool NumberCompare(float l, float r, RelationOperator op)
		{
			IComparer defaultInvariant = CaseInsensitiveComparer.DefaultInvariant;
			switch (op)
			{
			case RelationOperator.Equal:
				return defaultInvariant.Compare(l, r) == 0;
			case RelationOperator.NotEqual:
				return defaultInvariant.Compare(l, r) != 0;
			case RelationOperator.Less:
				return defaultInvariant.Compare(l, r) < 0;
			case RelationOperator.Greater:
				return defaultInvariant.Compare(l, r) > 0;
			case RelationOperator.LessOrEqual:
				return defaultInvariant.Compare(l, r) <= 0;
			case RelationOperator.GreaterOrEqual:
				return defaultInvariant.Compare(l, r) >= 0;
			default:
				throw new NotSupportedException(string.Format("Relational operator {0} is not supported.", op));
			}
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0004A4B4 File Offset: 0x000486B4
		private static bool BoolCompare(bool l, bool r, RelationOperator op)
		{
			IComparer defaultInvariant = CaseInsensitiveComparer.DefaultInvariant;
			switch (op)
			{
			case RelationOperator.Equal:
				return defaultInvariant.Compare(l, r) == 0;
			case RelationOperator.NotEqual:
				return defaultInvariant.Compare(l, r) != 0;
			default:
				throw new NotSupportedException(string.Format("Relational operator {0} is not supported.", op));
			}
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0004A520 File Offset: 0x00048720
		private static bool StringCompare(string l, string r, RelationOperator op)
		{
			IComparer defaultInvariant = CaseInsensitiveComparer.DefaultInvariant;
			switch (op)
			{
			case RelationOperator.Equal:
				return defaultInvariant.Compare(l, r) == 0;
			case RelationOperator.NotEqual:
				return defaultInvariant.Compare(l, r) != 0;
			default:
				throw new NotSupportedException(string.Format("Relational operator {0} is not supported.", op));
			}
		}

		// Token: 0x0400053C RID: 1340
		private readonly ConditionExpression left;

		// Token: 0x0400053D RID: 1341
		private readonly ConditionExpression right;

		// Token: 0x0400053E RID: 1342
		private readonly RelationOperator op;
	}
}
