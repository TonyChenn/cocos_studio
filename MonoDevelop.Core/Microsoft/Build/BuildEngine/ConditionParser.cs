using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001E1 RID: 481
	internal class ConditionParser
	{
		// Token: 0x0600123B RID: 4667 RVA: 0x00049D8A File Offset: 0x00047F8A
		private ConditionParser(string condition)
		{
			this.tokenizer = new ConditionTokenizer();
			this.tokenizer.Tokenize(condition);
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00049DAC File Offset: 0x00047FAC
		public static bool ParseAndEvaluate(string condition, IExpressionContext context)
		{
			if (string.IsNullOrEmpty(condition))
			{
				return true;
			}
			ConditionExpression conditionExpression = ConditionParser.ParseCondition(condition);
			if (!conditionExpression.CanEvaluateToBool(context))
			{
				throw new Exception(string.Format("Can not evaluate \"{0}\" to bool.", condition));
			}
			return conditionExpression.BoolEvaluate(context);
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00049DEC File Offset: 0x00047FEC
		public static ConditionExpression ParseCondition(string condition)
		{
			ConditionParser conditionParser = new ConditionParser(condition);
			ConditionExpression result = conditionParser.ParseExpression();
			if (!conditionParser.tokenizer.IsEOF())
			{
				throw new ExpressionParseException(string.Format("Unexpected token at end of condition: \"{0}\"", conditionParser.tokenizer.Token.Value));
			}
			return result;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00049E35 File Offset: 0x00048035
		private ConditionExpression ParseExpression()
		{
			return this.ParseBooleanExpression();
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00049E3D File Offset: 0x0004803D
		private ConditionExpression ParseBooleanExpression()
		{
			return this.ParseBooleanAnd();
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00049E48 File Offset: 0x00048048
		private ConditionExpression ParseBooleanAnd()
		{
			ConditionExpression conditionExpression = this.ParseBooleanOr();
			while (this.tokenizer.IsToken(TokenType.And))
			{
				this.tokenizer.GetNextToken();
				conditionExpression = new ConditionAndExpression(conditionExpression, this.ParseBooleanOr());
			}
			return conditionExpression;
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00049E88 File Offset: 0x00048088
		private ConditionExpression ParseBooleanOr()
		{
			ConditionExpression conditionExpression = this.ParseRelationalExpression();
			while (this.tokenizer.IsToken(TokenType.Or))
			{
				this.tokenizer.GetNextToken();
				conditionExpression = new ConditionOrExpression(conditionExpression, this.ParseRelationalExpression());
			}
			return conditionExpression;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00049EC8 File Offset: 0x000480C8
		private ConditionExpression ParseRelationalExpression()
		{
			ConditionExpression conditionExpression = this.ParseFactorExpression();
			if (this.tokenizer.IsToken(TokenType.Less) || this.tokenizer.IsToken(TokenType.Greater) || this.tokenizer.IsToken(TokenType.Equal) || this.tokenizer.IsToken(TokenType.NotEqual) || this.tokenizer.IsToken(TokenType.LessOrEqual) || this.tokenizer.IsToken(TokenType.GreaterOrEqual))
			{
				Token token = this.tokenizer.Token;
				this.tokenizer.GetNextToken();
				RelationOperator op;
				switch (token.Type)
				{
				case TokenType.Less:
					op = RelationOperator.Less;
					break;
				case TokenType.Greater:
					op = RelationOperator.Greater;
					break;
				case TokenType.LessOrEqual:
					op = RelationOperator.LessOrEqual;
					break;
				case TokenType.GreaterOrEqual:
					op = RelationOperator.GreaterOrEqual;
					break;
				case TokenType.Equal:
					op = RelationOperator.Equal;
					break;
				case TokenType.NotEqual:
					op = RelationOperator.NotEqual;
					break;
				default:
					throw new ExpressionParseException(string.Format("Wrong relation operator {0}", token.Value));
				}
				conditionExpression = new ConditionRelationalExpression(conditionExpression, this.ParseFactorExpression(), op);
			}
			return conditionExpression;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x00049FB4 File Offset: 0x000481B4
		private ConditionExpression ParseFactorExpression()
		{
			Token token = this.tokenizer.Token;
			this.tokenizer.GetNextToken();
			ConditionExpression result;
			if (token.Type == TokenType.LeftParen)
			{
				result = this.ParseExpression();
				this.tokenizer.Expect(TokenType.RightParen);
			}
			else if (token.Type == TokenType.String && this.tokenizer.Token.Type == TokenType.LeftParen)
			{
				result = this.ParseFunctionExpression(token.Value);
			}
			else if (token.Type == TokenType.String)
			{
				result = new ConditionFactorExpression(token);
			}
			else if (token.Type == TokenType.Number)
			{
				result = new ConditionFactorExpression(token);
			}
			else if (token.Type == TokenType.Item || token.Type == TokenType.Property || token.Type == TokenType.Metadata)
			{
				result = this.ParseReferenceExpression(token.Value);
			}
			else
			{
				if (token.Type != TokenType.Not)
				{
					throw new ExpressionParseException(string.Format("Unexpected token type {0}.", token.Type));
				}
				result = this.ParseNotExpression();
			}
			return result;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0004A0A4 File Offset: 0x000482A4
		private ConditionExpression ParseNotExpression()
		{
			return new ConditionNotExpression(this.ParseFactorExpression());
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0004A0B1 File Offset: 0x000482B1
		private ConditionExpression ParseFunctionExpression(string function_name)
		{
			return new ConditionFunctionExpression(function_name, this.ParseFunctionArguments());
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0004A0C0 File Offset: 0x000482C0
		private List<ConditionFactorExpression> ParseFunctionArguments()
		{
			List<ConditionFactorExpression> list = new List<ConditionFactorExpression>();
			for (;;)
			{
				this.tokenizer.GetNextToken();
				if (this.tokenizer.Token.Type == TokenType.RightParen)
				{
					break;
				}
				if (this.tokenizer.Token.Type != TokenType.Comma)
				{
					this.tokenizer.Putback(this.tokenizer.Token);
					ConditionFactorExpression item = (ConditionFactorExpression)this.ParseFactorExpression();
					list.Add(item);
				}
			}
			this.tokenizer.GetNextToken();
			return list;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0004A140 File Offset: 0x00048340
		private ConditionExpression ParseReferenceExpression(string prefix)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.ExpectToken(TokenType.LeftParen);
			this.tokenizer.GetNextToken();
			stringBuilder.AppendFormat("{0}({1}", prefix, this.tokenizer.Token.Value);
			this.tokenizer.GetNextToken();
			if (prefix == "@" && this.tokenizer.Token.Type == TokenType.Transform)
			{
				this.tokenizer.GetNextToken();
				stringBuilder.AppendFormat("->'{0}'", this.tokenizer.Token.Value);
				this.tokenizer.GetNextToken();
				if (this.tokenizer.Token.Type == TokenType.Comma)
				{
					this.tokenizer.GetNextToken();
					stringBuilder.AppendFormat(", '{0}'", this.tokenizer.Token.Value);
					this.tokenizer.GetNextToken();
				}
			}
			this.ExpectToken(TokenType.RightParen);
			this.tokenizer.GetNextToken();
			stringBuilder.Append(")");
			return new ConditionFactorExpression(new Token(stringBuilder.ToString(), TokenType.String));
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0004A25C File Offset: 0x0004845C
		private void ExpectToken(TokenType type)
		{
			if (this.tokenizer.Token.Type != type)
			{
				throw new ExpressionParseException(string.Concat(new object[]
				{
					"Expected token type of type: ",
					type,
					", got ",
					this.tokenizer.Token.Type,
					" (",
					this.tokenizer.Token.Value,
					") ."
				}));
			}
		}

		// Token: 0x0400053B RID: 1339
		private ConditionTokenizer tokenizer;
	}
}
