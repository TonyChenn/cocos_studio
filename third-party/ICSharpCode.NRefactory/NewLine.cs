using System;

namespace ICSharpCode.NRefactory
{
	/// <summary>
	/// Defines unicode new lines according to  Unicode Technical Report #13
	/// http://www.unicode.org/standard/reports/tr13/tr13-5.html
	/// </summary>
	// Token: 0x02000140 RID: 320
	public static class NewLine
	{
		/// <summary>
		/// Determines if a char is a new line delimiter.
		/// </summary>
		/// <returns>0 == no new line, otherwise it returns either 1 or 2 depending of the length of the delimiter.</returns>
		/// <param name="curChar">The current character.</param>
		/// <param name="nextChar">A callback getting the next character (may be null).</param>
		// Token: 0x06000AFA RID: 2810 RVA: 0x00021094 File Offset: 0x00020094
		public static int GetDelimiterLength(char curChar, Func<char> nextChar = null)
		{
			if (curChar == '\r')
			{
				if (nextChar != null && nextChar() == '\n')
				{
					return 2;
				}
				return 1;
			}
			else
			{
				if (curChar == '\n' || curChar == '\u0085' || curChar == '\v' || curChar == '\f' || curChar == '\u2028' || curChar == '\u2029')
				{
					return 1;
				}
				return 0;
			}
		}

		/// <summary>
		/// Determines if a char is a new line delimiter.
		/// </summary>
		/// <returns>0 == no new line, otherwise it returns either 1 or 2 depending of the length of the delimiter.</returns>
		/// <param name="curChar">The current character.</param>
		/// <param name="nextChar">The next character (if != LF then length will always be 0 or 1).</param>
		// Token: 0x06000AFB RID: 2811 RVA: 0x000210E1 File Offset: 0x000200E1
		public static int GetDelimiterLength(char curChar, char nextChar)
		{
			if (curChar == '\r')
			{
				if (nextChar == '\n')
				{
					return 2;
				}
				return 1;
			}
			else
			{
				if (curChar == '\n' || curChar == '\u0085' || curChar == '\v' || curChar == '\f' || curChar == '\u2028' || curChar == '\u2029')
				{
					return 1;
				}
				return 0;
			}
		}

		/// <summary>
		/// Determines if a char is a new line delimiter.
		/// </summary>
		/// <returns>0 == no new line, otherwise it returns either 1 or 2 depending of the length of the delimiter.</returns>
		/// <param name="curChar">The current character.</param>
		/// <param name="length">The length of the delimiter</param>
		/// <param name="type">The type of the delimiter</param>
		/// <param name="nextChar">A callback getting the next character (may be null).</param>
		// Token: 0x06000AFC RID: 2812 RVA: 0x0002111C File Offset: 0x0002011C
		public static bool TryGetDelimiterLengthAndType(char curChar, out int length, out UnicodeNewline type, Func<char> nextChar = null)
		{
			if (curChar == '\r')
			{
				if (nextChar != null && nextChar() == '\n')
				{
					length = 2;
					type = UnicodeNewline.CRLF;
				}
				else
				{
					length = 1;
					type = UnicodeNewline.CR;
				}
				return true;
			}
			switch (curChar)
			{
			case '\n':
				type = UnicodeNewline.LF;
				length = 1;
				return true;
			case '\v':
				type = UnicodeNewline.VT;
				length = 1;
				return true;
			case '\f':
				type = UnicodeNewline.FF;
				length = 1;
				return true;
			default:
				if (curChar == '\u0085')
				{
					type = UnicodeNewline.NEL;
					length = 1;
					return true;
				}
				switch (curChar)
				{
				case '\u2028':
					type = UnicodeNewline.LS;
					length = 1;
					return true;
				case '\u2029':
					type = UnicodeNewline.PS;
					length = 1;
					return true;
				default:
					length = -1;
					type = UnicodeNewline.Unknown;
					return false;
				}
				break;
			}
		}

		/// <summary>
		/// Determines if a char is a new line delimiter.
		/// </summary>
		/// <returns>0 == no new line, otherwise it returns either 1 or 2 depending of the length of the delimiter.</returns>
		/// <param name="curChar">The current character.</param>
		/// <param name="length">The length of the delimiter</param>
		/// <param name="type">The type of the delimiter</param>
		/// <param name="nextChar">The next character (if != LF then length will always be 0 or 1).</param>
		// Token: 0x06000AFD RID: 2813 RVA: 0x000211CC File Offset: 0x000201CC
		public static bool TryGetDelimiterLengthAndType(char curChar, out int length, out UnicodeNewline type, char nextChar)
		{
			if (curChar == '\r')
			{
				if (nextChar == '\n')
				{
					length = 2;
					type = UnicodeNewline.CRLF;
				}
				else
				{
					length = 1;
					type = UnicodeNewline.CR;
				}
				return true;
			}
			switch (curChar)
			{
			case '\n':
				type = UnicodeNewline.LF;
				length = 1;
				return true;
			case '\v':
				type = UnicodeNewline.VT;
				length = 1;
				return true;
			case '\f':
				type = UnicodeNewline.FF;
				length = 1;
				return true;
			default:
				if (curChar == '\u0085')
				{
					type = UnicodeNewline.NEL;
					length = 1;
					return true;
				}
				switch (curChar)
				{
				case '\u2028':
					type = UnicodeNewline.LS;
					length = 1;
					return true;
				case '\u2029':
					type = UnicodeNewline.PS;
					length = 1;
					return true;
				default:
					length = -1;
					type = UnicodeNewline.Unknown;
					return false;
				}
				break;
			}
		}

		/// <summary>
		/// Gets the new line type of a given char/next char.
		/// </summary>
		/// <returns>0 == no new line, otherwise it returns either 1 or 2 depending of the length of the delimiter.</returns>
		/// <param name="curChar">The current character.</param>
		/// <param name="nextChar">A callback getting the next character (may be null).</param>
		// Token: 0x06000AFE RID: 2814 RVA: 0x00021274 File Offset: 0x00020274
		public static UnicodeNewline GetDelimiterType(char curChar, Func<char> nextChar = null)
		{
			switch (curChar)
			{
			case '\n':
				return UnicodeNewline.LF;
			case '\v':
				return UnicodeNewline.VT;
			case '\f':
				return UnicodeNewline.FF;
			case '\r':
				if (nextChar != null && nextChar() == '\n')
				{
					return UnicodeNewline.CRLF;
				}
				return UnicodeNewline.CR;
			default:
				if (curChar == '\u0085')
				{
					return UnicodeNewline.NEL;
				}
				switch (curChar)
				{
				case '\u2028':
					return UnicodeNewline.LS;
				case '\u2029':
					return UnicodeNewline.PS;
				default:
					return UnicodeNewline.Unknown;
				}
				break;
			}
		}

		/// <summary>
		/// Gets the new line type of a given char/next char.
		/// </summary>
		/// <returns>0 == no new line, otherwise it returns either 1 or 2 depending of the length of the delimiter.</returns>
		/// <param name="curChar">The current character.</param>
		/// <param name="nextChar">The next character (if != LF then length will always be 0 or 1).</param>
		// Token: 0x06000AFF RID: 2815 RVA: 0x000212EC File Offset: 0x000202EC
		public static UnicodeNewline GetDelimiterType(char curChar, char nextChar)
		{
			switch (curChar)
			{
			case '\n':
				return UnicodeNewline.LF;
			case '\v':
				return UnicodeNewline.VT;
			case '\f':
				return UnicodeNewline.FF;
			case '\r':
				if (nextChar == '\n')
				{
					return UnicodeNewline.CRLF;
				}
				return UnicodeNewline.CR;
			default:
				if (curChar == '\u0085')
				{
					return UnicodeNewline.NEL;
				}
				switch (curChar)
				{
				case '\u2028':
					return UnicodeNewline.LS;
				case '\u2029':
					return UnicodeNewline.PS;
				default:
					return UnicodeNewline.Unknown;
				}
				break;
			}
		}

		/// <summary>
		/// Determines if a char is a new line delimiter. 
		///
		/// Note that the only 2 char wide new line is CR LF and both chars are new line
		/// chars on their own. For most cases GetDelimiterLength is the better choice.
		/// </summary>
		// Token: 0x06000B00 RID: 2816 RVA: 0x0002135C File Offset: 0x0002035C
		public static bool IsNewLine(char ch)
		{
			return ch == '\r' || ch == '\n' || ch == '\u0085' || ch == '\v' || ch == '\f' || ch == '\u2028' || ch == '\u2029';
		}

		/// <summary>
		/// Gets the new line as a string.
		/// </summary>
		// Token: 0x06000B01 RID: 2817 RVA: 0x0002138C File Offset: 0x0002038C
		public static string GetString(UnicodeNewline newLine)
		{
			if (newLine <= UnicodeNewline.CR)
			{
				if (newLine == UnicodeNewline.Unknown)
				{
					return "";
				}
				switch (newLine)
				{
				case UnicodeNewline.LF:
					return "\n";
				case UnicodeNewline.VT:
					return "\v";
				case UnicodeNewline.FF:
					return "\f";
				case UnicodeNewline.CR:
					return "\r";
				}
			}
			else
			{
				if (newLine == UnicodeNewline.NEL)
				{
					return "\u0085";
				}
				if (newLine == UnicodeNewline.CRLF)
				{
					return "\r\n";
				}
				switch (newLine)
				{
				case UnicodeNewline.LS:
					return "\u2028";
				case UnicodeNewline.PS:
					return "\u2029";
				}
			}
			throw new ArgumentOutOfRangeException();
		}

		/// <summary>
		/// Carriage Return, U+000D
		/// </summary>
		// Token: 0x040003C9 RID: 969
		public const char CR = '\r';

		/// <summary>
		/// Line Feed, U+000A
		/// </summary>
		// Token: 0x040003CA RID: 970
		public const char LF = '\n';

		/// <summary>
		/// Next Line, U+0085
		/// </summary>
		// Token: 0x040003CB RID: 971
		public const char NEL = '\u0085';

		/// <summary>
		/// Vertical Tab, U+000B
		/// </summary>
		// Token: 0x040003CC RID: 972
		public const char VT = '\v';

		/// <summary>
		/// Form Feed, U+000C
		/// </summary>
		// Token: 0x040003CD RID: 973
		public const char FF = '\f';

		/// <summary>
		/// Line Separator, U+2028
		/// </summary>
		// Token: 0x040003CE RID: 974
		public const char LS = '\u2028';

		/// <summary>
		/// Paragraph Separator, U+2029
		/// </summary>
		// Token: 0x040003CF RID: 975
		public const char PS = '\u2029';
	}
}
