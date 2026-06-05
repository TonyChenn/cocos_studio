using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Composite format string parser.
	/// </summary>
	/// <remarks>
	/// Implements a complete parser for valid strings as well as
	/// error reporting and best-effort parsing for invalid strings.
	/// </remarks>		
	// Token: 0x02000131 RID: 305
	public class CompositeFormatStringParser
	{
		// Token: 0x06000A97 RID: 2711 RVA: 0x0001FBA5 File Offset: 0x0001EBA5
		public CompositeFormatStringParser()
		{
			this.errors = new List<IFormatStringError>();
		}

		/// <summary>
		/// Parse the specified format string.
		/// </summary>
		/// <param name="format">
		/// The format string.
		/// </param>
		// Token: 0x06000A98 RID: 2712 RVA: 0x0001FBB8 File Offset: 0x0001EBB8
		public FormatStringParseResult Parse(string format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			FormatStringParseResult formatStringParseResult = new FormatStringParseResult();
			int num = 0;
			int length = format.Length;
			for (int i = 0; i < length; i++)
			{
				this.GetText(format, ref i, "", false);
				if (i < format.Length && format[i] == '{')
				{
					int num2 = i;
					List<IFormatStringError> list = new List<IFormatStringError>(this.GetErrors());
					i++;
					int index = this.ParseIndex(format, ref i);
					this.CheckForMissingEndBrace(format, i, length);
					int? alignment = this.ParseAlignment(format, ref i, length);
					this.CheckForMissingEndBrace(format, i, length);
					string formatString = this.ParseSubFormatString(format, ref i, length);
					this.CheckForMissingEndBrace(format, i, length);
					if (i == num2 + 1 && (i == length || (i < length && format[i] != '}')))
					{
						this.SetErrors(list);
						if (i >= length || format[i] != '{')
						{
							this.AddError(new DefaultFormatStringError
							{
								Message = "Unescaped '{'",
								StartLocation = num2,
								EndLocation = num2 + 1,
								OriginalText = "{",
								SuggestedReplacementText = "{{"
							});
						}
					}
					else
					{
						if (num2 - num > 0)
						{
							TextSegment textSegment = new TextSegment(CompositeFormatStringParser.UnEscape(format.Substring(num, num2 - num)), 0, null);
							textSegment.Errors = list;
							formatStringParseResult.Segments.Add(textSegment);
						}
						if (i < length && format[i] != '}')
						{
							i--;
						}
						int endLocation = Math.Min(length, i + 1);
						formatStringParseResult.Segments.Add(new FormatItem(index, alignment, formatString)
						{
							StartLocation = num2,
							EndLocation = endLocation,
							Errors = this.GetErrors()
						});
						this.ClearErrors();
						num = i + 1;
					}
				}
			}
			if (num < length)
			{
				TextSegment textSegment2 = new TextSegment(CompositeFormatStringParser.UnEscape(format.Substring(num)), num, null);
				textSegment2.Errors = this.GetErrors();
				formatStringParseResult.Segments.Add(textSegment2);
			}
			return formatStringParseResult;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0001FDCC File Offset: 0x0001EDCC
		private int ParseIndex(string format, ref int i)
		{
			int num;
			int? andCheckNumber = this.GetAndCheckNumber(format, ",:}", ref i, i, out num);
			if (num == 0)
			{
				this.AddError(new DefaultFormatStringError
				{
					StartLocation = i,
					EndLocation = i,
					Message = "Missing index",
					OriginalText = "",
					SuggestedReplacementText = "0"
				});
			}
			int? num2 = andCheckNumber;
			if (num2 == null)
			{
				return 0;
			}
			return num2.GetValueOrDefault();
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0001FE40 File Offset: 0x0001EE40
		private int? ParseAlignment(string format, ref int i, int length)
		{
			if (i < length && format[i] == ',')
			{
				int num = i;
				i++;
				while (i < length && char.IsWhiteSpace(format[i]))
				{
					i++;
				}
				int num2;
				int? andCheckNumber = this.GetAndCheckNumber(format, ",:}", ref i, num + 1, out num2);
				if (num2 == 0)
				{
					this.AddError(new DefaultFormatStringError
					{
						StartLocation = i,
						EndLocation = i,
						Message = "Missing alignment",
						OriginalText = "",
						SuggestedReplacementText = "0"
					});
				}
				return new int?(andCheckNumber ?? 0);
			}
			return null;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0001FF04 File Offset: 0x0001EF04
		private string ParseSubFormatString(string format, ref int i, int length)
		{
			if (i < length && format[i] == ':')
			{
				i++;
				int num = i;
				this.GetText(format, ref i, "", true);
				string unEscaped = format.Substring(num, i - num);
				return CompositeFormatStringParser.UnEscape(unEscaped);
			}
			return null;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0001FF50 File Offset: 0x0001EF50
		private void CheckForMissingEndBrace(string format, int i, int length)
		{
			if (i == length)
			{
				int num = i - 1;
				while (format[num] == '}')
				{
					num--;
				}
				bool flag = (i - num) % 2 == 1;
				if (flag)
				{
					this.AddMissingEndBraceError(i, i, "Missing '}'", "");
				}
			}
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0001FF94 File Offset: 0x0001EF94
		private void GetText(string format, ref int index, string delimiters = "", bool allowEscape = false)
		{
			while (index < format.Length)
			{
				if (format[index] == '{' || format[index] == '}')
				{
					if (index + 1 >= format.Length || format[index + 1] != format[index] || !allowEscape)
					{
						break;
					}
					index++;
				}
				else if (delimiters.Contains(format[index].ToString()))
				{
					return;
				}
				index++;
			}
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00020010 File Offset: 0x0001F010
		private int? GetNumber(string format, ref int index)
		{
			if (format.Length == 0)
			{
				return null;
			}
			int num = 0;
			int num2 = index;
			bool flag = format[num2] != '-';
			if (!flag)
			{
				num2++;
			}
			int num3 = num2;
			while (num2 < format.Length && format[num2] >= '0' && format[num2] <= '9')
			{
				num = 10 * num + (int)format[num2] - 48;
				num2++;
			}
			if (num2 == num3)
			{
				return null;
			}
			index = num2;
			return new int?(flag ? num : (-num));
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x000200A4 File Offset: 0x0001F0A4
		private int? GetAndCheckNumber(string format, string delimiters, ref int index, int numberFieldStart, out int parsedCharacters)
		{
			int num = index;
			this.GetText(format, ref num, delimiters, false);
			int num2 = num;
			string text = format.Substring(index, num2 - index);
			parsedCharacters = text.Length;
			int num3 = 0;
			int? number = this.GetNumber(text, ref num3);
			if (num3 != parsedCharacters && num2 < format.Length && delimiters.Contains(format[num2]))
			{
				index = num2;
				string replacementText = (number ?? 0).ToString();
				this.AddInvalidNumberFormatError(numberFieldStart, format.Substring(numberFieldStart, index - numberFieldStart), replacementText);
			}
			else
			{
				int num4 = index + num3;
				if (num3 != parsedCharacters)
				{
					index = num4;
					this.AddMissingEndBraceError(index, index, "Missing ending '}'", "");
				}
				else
				{
					index = num4;
				}
			}
			return number;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0002016C File Offset: 0x0001F16C
		public static string UnEscape(string unEscaped)
		{
			return unEscaped.Replace("{{", "{").Replace("}}", "}");
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002018D File Offset: 0x0001F18D
		private void AddError(IFormatStringError error)
		{
			this.errors.Add(error);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0002019C File Offset: 0x0001F19C
		private void AddMissingEndBraceError(int start, int end, string message, string originalText)
		{
			if (this.hasMissingEndBrace)
			{
				return;
			}
			this.AddError(new DefaultFormatStringError
			{
				StartLocation = start,
				EndLocation = end,
				Message = message,
				OriginalText = originalText,
				SuggestedReplacementText = "}"
			});
			this.hasMissingEndBrace = true;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x000201F0 File Offset: 0x0001F1F0
		private void AddInvalidNumberFormatError(int i, string number, string replacementText)
		{
			this.AddError(new DefaultFormatStringError
			{
				StartLocation = i,
				EndLocation = i + number.Length,
				Message = string.Format("Invalid number '{0}'", number),
				OriginalText = number,
				SuggestedReplacementText = replacementText
			});
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0002023E File Offset: 0x0001F23E
		private IList<IFormatStringError> GetErrors()
		{
			return this.errors;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00020246 File Offset: 0x0001F246
		private void SetErrors(IList<IFormatStringError> errors)
		{
			this.errors = errors;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0002024F File Offset: 0x0001F24F
		private void ClearErrors()
		{
			this.hasMissingEndBrace = false;
			this.errors = new List<IFormatStringError>();
		}

		// Token: 0x04000397 RID: 919
		private IList<IFormatStringError> errors;

		// Token: 0x04000398 RID: 920
		private bool hasMissingEndBrace;
	}
}
