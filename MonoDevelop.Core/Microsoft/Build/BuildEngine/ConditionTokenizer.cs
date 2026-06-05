using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001E4 RID: 484
	internal sealed class ConditionTokenizer
	{
		// Token: 0x06001253 RID: 4691 RVA: 0x0004A578 File Offset: 0x00048778
		static ConditionTokenizer()
		{
			for (int i = 0; i < 128; i++)
			{
				ConditionTokenizer.charIndexToTokenType[i] = TokenType.Invalid;
			}
			foreach (ConditionTokenizer.CharToTokenType charToTokenType in ConditionTokenizer.charToTokenType)
			{
				ConditionTokenizer.charIndexToTokenType[(int)charToTokenType.ch] = charToTokenType.tokenType;
			}
			ConditionTokenizer.keywords.Add("and", TokenType.And);
			ConditionTokenizer.keywords.Add("or", TokenType.Or);
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0004A72A File Offset: 0x0004892A
		public void Tokenize(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			this.inputString = s;
			this.position = 0;
			this.token = new Token(null, TokenType.BOF);
			this.GetNextToken();
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0004A75C File Offset: 0x0004895C
		private void SkipWhiteSpace()
		{
			int num;
			while ((num = this.PeekChar()) != -1)
			{
				if (!char.IsWhiteSpace((char)num))
				{
					return;
				}
				this.ReadChar();
			}
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0004A787 File Offset: 0x00048987
		private int PeekChar()
		{
			if (this.position < this.inputString.Length)
			{
				return (int)this.inputString[this.position];
			}
			return -1;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x0004A7B0 File Offset: 0x000489B0
		private int ReadChar()
		{
			if (this.position < this.inputString.Length)
			{
				return (int)this.inputString[this.position++];
			}
			return -1;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0004A7F0 File Offset: 0x000489F0
		public void Expect(TokenType type)
		{
			if (this.token.Type != type)
			{
				throw new ExpressionParseException(string.Concat(new object[]
				{
					"Expected token type of type: ",
					type,
					", got ",
					this.token.Type,
					" (",
					this.token.Value,
					") ."
				}));
			}
			this.GetNextToken();
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0004A86E File Offset: 0x00048A6E
		public bool IsEOF()
		{
			return this.token.Type == TokenType.EOF;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0004A87E File Offset: 0x00048A7E
		public bool IsNumber()
		{
			return this.token.Type == TokenType.Number;
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0004A88E File Offset: 0x00048A8E
		public bool IsToken(TokenType type)
		{
			return this.token.Type == type;
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x0004A89E File Offset: 0x00048A9E
		public bool IsPunctation()
		{
			return this.token.Type >= TokenType.FirstPunct && this.token.Type < TokenType.LastPunct;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0004A8C0 File Offset: 0x00048AC0
		public void Putback(Token token)
		{
			this.putback = token;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0004A8CC File Offset: 0x00048ACC
		public void GetNextToken()
		{
			if (this.putback != null)
			{
				this.token = this.putback;
				this.putback = null;
				return;
			}
			if (this.token.Type == TokenType.EOF)
			{
				throw new ExpressionParseException("Cannot read past the end of stream.");
			}
			this.SkipWhiteSpace();
			this.tokenPosition = this.position;
			int num = this.ReadChar();
			if (num == -1)
			{
				this.token = new Token(null, TokenType.EOF);
				return;
			}
			char c = (char)num;
			if (c == '-' && this.PeekChar() == 62)
			{
				this.ReadChar();
				this.token = new Token("->", TokenType.Transform);
				return;
			}
			if (char.IsDigit(c) || c == '-')
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(c);
				while ((num = this.PeekChar()) != -1)
				{
					c = (char)num;
					if (!char.IsDigit(c) && c != '.')
					{
						break;
					}
					stringBuilder.Append((char)this.ReadChar());
				}
				this.token = new Token(stringBuilder.ToString(), TokenType.Number);
				return;
			}
			if (c == '\'')
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(c);
				bool flag = this.PeekChar() == 64;
				int num2 = 0;
				bool flag2 = false;
				while ((num = this.PeekChar()) != -1)
				{
					c = (char)num;
					if (c == '(' && !flag2 && flag)
					{
						num2++;
					}
					if (c == ')' && !flag2 && flag)
					{
						num2--;
					}
					stringBuilder2.Append((char)this.ReadChar());
					if (c == '\'')
					{
						if (num2 == 0)
						{
							break;
						}
						flag2 = !flag2;
					}
				}
				string text = stringBuilder2.ToString();
				this.token = new Token(text.Substring(1, text.Length - 2), TokenType.String);
				return;
			}
			if (c == '_' || char.IsLetter(c))
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.Append(c);
				while ((num = this.PeekChar()) != -1 && ((ushort)num == 95 || char.IsLetterOrDigit((char)num)))
				{
					stringBuilder3.Append((char)this.ReadChar());
				}
				string text2 = stringBuilder3.ToString();
				if (ConditionTokenizer.keywords.ContainsKey(text2))
				{
					this.token = new Token(text2, ConditionTokenizer.keywords[text2]);
					return;
				}
				this.token = new Token(text2, TokenType.String);
				return;
			}
			else
			{
				if (c == '!' && this.PeekChar() == 61)
				{
					this.token = new Token("!=", TokenType.NotEqual);
					this.ReadChar();
					return;
				}
				if (c == '<' && this.PeekChar() == 61)
				{
					this.token = new Token("<=", TokenType.LessOrEqual);
					this.ReadChar();
					return;
				}
				if (c == '>' && this.PeekChar() == 61)
				{
					this.token = new Token(">=", TokenType.GreaterOrEqual);
					this.ReadChar();
					return;
				}
				if (c == '=' && this.PeekChar() == 61)
				{
					this.token = new Token("==", TokenType.Equal);
					this.ReadChar();
					return;
				}
				if (c < ' ' || c >= '\u0080')
				{
					throw new ExpressionParseException(string.Format("Invalid token: {0}", c));
				}
				if (ConditionTokenizer.charIndexToTokenType[(int)c] != TokenType.Invalid)
				{
					this.token = new Token(new string(c, 1), ConditionTokenizer.charIndexToTokenType[(int)c]);
					return;
				}
				throw new ExpressionParseException(string.Format("Invalid punctuation: {0}", c));
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x0004ABE8 File Offset: 0x00048DE8
		public int TokenPosition
		{
			get
			{
				return this.tokenPosition;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x0004ABF0 File Offset: 0x00048DF0
		public Token Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x04000546 RID: 1350
		private string inputString;

		// Token: 0x04000547 RID: 1351
		private int position;

		// Token: 0x04000548 RID: 1352
		private int tokenPosition;

		// Token: 0x04000549 RID: 1353
		private Token token;

		// Token: 0x0400054A RID: 1354
		private Token putback;

		// Token: 0x0400054B RID: 1355
		private static TokenType[] charIndexToTokenType = new TokenType[128];

		// Token: 0x0400054C RID: 1356
		private static Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>(StringComparer.InvariantCultureIgnoreCase);

		// Token: 0x0400054D RID: 1357
		private static ConditionTokenizer.CharToTokenType[] charToTokenType = new ConditionTokenizer.CharToTokenType[]
		{
			new ConditionTokenizer.CharToTokenType('<', TokenType.Less),
			new ConditionTokenizer.CharToTokenType('>', TokenType.Greater),
			new ConditionTokenizer.CharToTokenType('=', TokenType.Equal),
			new ConditionTokenizer.CharToTokenType('(', TokenType.LeftParen),
			new ConditionTokenizer.CharToTokenType(')', TokenType.RightParen),
			new ConditionTokenizer.CharToTokenType('.', TokenType.Dot),
			new ConditionTokenizer.CharToTokenType(',', TokenType.Comma),
			new ConditionTokenizer.CharToTokenType('!', TokenType.Not),
			new ConditionTokenizer.CharToTokenType('@', TokenType.Item),
			new ConditionTokenizer.CharToTokenType('$', TokenType.Property),
			new ConditionTokenizer.CharToTokenType('%', TokenType.Metadata),
			new ConditionTokenizer.CharToTokenType('\'', TokenType.Apostrophe)
		};

		// Token: 0x020001E5 RID: 485
		private struct CharToTokenType
		{
			// Token: 0x06001262 RID: 4706 RVA: 0x0004ABF8 File Offset: 0x00048DF8
			public CharToTokenType(char ch, TokenType tokenType)
			{
				this.ch = ch;
				this.tokenType = tokenType;
			}

			// Token: 0x0400054E RID: 1358
			public char ch;

			// Token: 0x0400054F RID: 1359
			public TokenType tokenType;
		}
	}
}
