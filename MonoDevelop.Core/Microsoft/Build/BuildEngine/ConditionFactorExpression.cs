using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001DD RID: 477
	internal sealed class ConditionFactorExpression : ConditionExpression
	{
		// Token: 0x06001219 RID: 4633 RVA: 0x0004992C File Offset: 0x00047B2C
		static ConditionFactorExpression()
		{
			string[] array = new string[]
			{
				"true",
				"on",
				"yes"
			};
			string[] array2 = new string[]
			{
				"false",
				"off",
				"no"
			};
			ConditionFactorExpression.allValues = CollectionsUtil.CreateCaseInsensitiveHashtable();
			ConditionFactorExpression.trueValues = CollectionsUtil.CreateCaseInsensitiveHashtable();
			ConditionFactorExpression.falseValues = CollectionsUtil.CreateCaseInsensitiveHashtable();
			foreach (string text in array)
			{
				ConditionFactorExpression.trueValues.Add(text, text);
				ConditionFactorExpression.allValues.Add(text, text);
			}
			foreach (string text2 in array2)
			{
				ConditionFactorExpression.falseValues.Add(text2, text2);
				ConditionFactorExpression.allValues.Add(text2, text2);
			}
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00049A0B File Offset: 0x00047C0B
		public ConditionFactorExpression(Token token)
		{
			this.token = token;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00049A1C File Offset: 0x00047C1C
		public override bool BoolEvaluate(IExpressionContext context)
		{
			Token token = ConditionFactorExpression.EvaluateToken(this.token, context);
			if (ConditionFactorExpression.trueValues[token.Value] != null)
			{
				return true;
			}
			if (ConditionFactorExpression.falseValues[token.Value] != null)
			{
				return false;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00049A64 File Offset: 0x00047C64
		public override float NumberEvaluate(IExpressionContext context)
		{
			Token token = ConditionFactorExpression.EvaluateToken(this.token, context);
			return float.Parse(token.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00049A90 File Offset: 0x00047C90
		public override string StringEvaluate(IExpressionContext context)
		{
			Token token = ConditionFactorExpression.EvaluateToken(this.token, context);
			return token.Value;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00049AB0 File Offset: 0x00047CB0
		public override bool CanEvaluateToBool(IExpressionContext context)
		{
			Token token = ConditionFactorExpression.EvaluateToken(this.token, context);
			return this.token.Type == TokenType.String && ConditionFactorExpression.allValues[token.Value] != null;
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00049AED File Offset: 0x00047CED
		public override bool CanEvaluateToNumber(IExpressionContext context)
		{
			return this.token.Type == TokenType.Number;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00049B00 File Offset: 0x00047D00
		public override bool CanEvaluateToString(IExpressionContext context)
		{
			return true;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00049B04 File Offset: 0x00047D04
		private static Token EvaluateToken(Token token, IExpressionContext context)
		{
			string tokenValue = context.EvaluateString(token.Value);
			return new Token(tokenValue, TokenType.String);
		}

		// Token: 0x04000531 RID: 1329
		private readonly Token token;

		// Token: 0x04000532 RID: 1330
		private static Hashtable allValues;

		// Token: 0x04000533 RID: 1331
		private static Hashtable trueValues;

		// Token: 0x04000534 RID: 1332
		private static Hashtable falseValues;
	}
}
