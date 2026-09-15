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
	public class CompositeFormatStringParser
	{
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

		public static string UnEscape(string unEscaped)
		{
			return unEscaped.Replace("{{", "{").Replace("}}", "}");
		}

		private void AddError(IFormatStringError error)
		{
			this.errors.Add(error);
		}

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

		private IList<IFormatStringError> GetErrors()
		{
			return this.errors;
		}

		private void SetErrors(IList<IFormatStringError> errors)
		{
			this.errors = errors;
		}

		private void ClearErrors()
		{
			this.hasMissingEndBrace = false;
			this.errors = new List<IFormatStringError>();
		}

		private IList<IFormatStringError> errors;

		private bool hasMissingEndBrace;
	}
}
