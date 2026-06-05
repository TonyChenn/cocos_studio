using System;
using System.Globalization;

namespace MonoDevelop.Core.Text
{
	// Token: 0x0200020E RID: 526
	internal class BacktrackingStringMatcher : StringMatcher
	{
		// Token: 0x060013D9 RID: 5081 RVA: 0x00052418 File Offset: 0x00050618
		public BacktrackingStringMatcher(string filterText)
		{
			this.filterText = (filterText ?? "");
			if (filterText != null)
			{
				int num = 0;
				while (num < filterText.Length && num < 64)
				{
					this.filterTextLowerCaseTable |= (char.IsLower(filterText[num]) ? (1UL << num) : 0UL);
					this.filterIsNonLetter |= ((!char.IsLetterOrDigit(filterText[num])) ? (1UL << num) : 0UL);
					this.filterIsDigit |= (char.IsDigit(filterText[num]) ? (1UL << num) : 0UL);
					num++;
				}
				this.filterTextUpperCase = filterText.ToUpper();
				return;
			}
			this.filterTextUpperCase = "";
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x000524E4 File Offset: 0x000506E4
		public override bool CalcMatchRank(string name, out int matchRank)
		{
			if (this.filterTextUpperCase.Length == 0)
			{
				matchRank = int.MinValue;
				return true;
			}
			int[] match = this.GetMatch(name);
			if (match == null)
			{
				matchRank = int.MinValue;
				return false;
			}
			if (name.Length == this.filterText.Length)
			{
				matchRank = int.MaxValue;
				for (int i = 0; i < name.Length; i++)
				{
					if (this.filterText[i] != name[i])
					{
						matchRank--;
					}
				}
				return true;
			}
			if (name.Length - 1 == this.filterText.Length && name[name.Length - 1] == ':')
			{
				matchRank = 2147483646;
				for (int j = 0; j < name.Length - 1; j++)
				{
					if (this.filterText[j] != name[j])
					{
						matchRank--;
					}
				}
				return true;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = -1;
			for (int k = 0; k < match.Length; k++)
			{
				char c = this.filterText[k];
				int num6 = match[k];
				bool flag = num6 > num5 + 1;
				if (flag)
				{
					num4++;
				}
				num5 = num6;
				if (c == name[num6])
				{
					num3 += 1000 / (1 + num4);
					if (char.IsUpper(c))
					{
						num += Math.Max(1, 10000 - 1000 * num4);
					}
				}
				else if (flag || num6 == 0)
				{
					num3 += 900 / (1 + num4);
					if (char.IsUpper(c))
					{
						num += Math.Max(1, 1000 - 100 * num4);
					}
				}
				else
				{
					int num7 = 600 / (1 + num4);
					num2 += num7;
				}
			}
			matchRank = num + num3 - num4 + num2 + this.filterText.Length - name.Length;
			if (name[name.Length - 1] == ':')
			{
				matchRank /= 2;
			}
			return true;
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x000526D8 File Offset: 0x000508D8
		public override bool IsMatch(string text)
		{
			int[] match = this.GetMatch(text);
			this.cachedResult = (this.cachedResult ?? match);
			return match != null;
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x00052708 File Offset: 0x00050908
		private int GetMatchChar(string text, int i, int j, bool onlyWordStart)
		{
			char c = this.filterTextUpperCase[i];
			ulong num = 1UL << i;
			if ((this.filterIsNonLetter & num) != 0UL)
			{
				while (j < text.Length)
				{
					if (c == text[j])
					{
						return j;
					}
					j++;
				}
				return -1;
			}
			char c2 = text[j];
			bool flag = char.IsUpper(c2);
			if (!onlyWordStart && c == (flag ? c2 : char.ToUpper(c2)) && char.IsLetter(c2))
			{
				if (!flag && (this.filterTextLowerCaseTable & num) == 0UL && j + 1 < text.Length)
				{
					int matchChar = this.GetMatchChar(text, i, j + 1, onlyWordStart);
					if (matchChar >= 0)
					{
						return matchChar;
					}
				}
				return j;
			}
			bool flag2 = false;
			bool flag3 = false;
			int num2 = j + 1;
			while (j < text.Length)
			{
				c2 = text[j];
				UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c2);
				if (unicodeCategory == UnicodeCategory.LowercaseLetter)
				{
					if (flag3 && j - num2 > 0 && c == char.ToUpper(text[j - 1]))
					{
						return j - 1;
					}
					flag2 = true;
					flag3 = false;
				}
				else if (unicodeCategory == UnicodeCategory.UppercaseLetter)
				{
					if (flag2 && c == char.ToUpper(c2))
					{
						return j;
					}
					flag2 = false;
					flag3 = true;
				}
				else
				{
					if (c == c2)
					{
						return j;
					}
					if (j + 1 < text.Length && c == char.ToUpper(text[j + 1]))
					{
						return j + 1;
					}
					flag3 = (flag2 = false);
				}
				j++;
			}
			return -1;
		}

		/// <summary>
		/// Gets the match indices.
		/// </summary>
		/// <returns>
		/// The indices in the text which are matched by our filter.
		/// </returns>
		/// <param name="text">
		/// The text to match.
		/// </param>
		// Token: 0x060013DD RID: 5085 RVA: 0x00052858 File Offset: 0x00050A58
		public override int[] GetMatch(string text)
		{
			if (string.IsNullOrEmpty(this.filterTextUpperCase))
			{
				return new int[0];
			}
			if (string.IsNullOrEmpty(text) || this.filterText.Length > text.Length)
			{
				return null;
			}
			int[] array;
			if (this.cachedResult != null)
			{
				array = this.cachedResult;
			}
			else
			{
				array = (this.cachedResult = new int[this.filterTextUpperCase.Length]);
			}
			int num = 0;
			int i = 0;
			bool onlyWordStart = false;
			while (i < this.filterText.Length)
			{
				if (num >= text.Length)
				{
					if (i <= 0)
					{
						return null;
					}
					num = array[--i] + 1;
					onlyWordStart = true;
				}
				else
				{
					num = this.GetMatchChar(text, i, num, onlyWordStart);
					onlyWordStart = false;
					if (num == -1)
					{
						if (i <= 0)
						{
							return null;
						}
						num = array[--i] + 1;
						onlyWordStart = true;
					}
					else
					{
						array[i] = num++;
						i++;
					}
				}
			}
			this.cachedResult = null;
			return array;
		}

		// Token: 0x040005E8 RID: 1512
		private readonly string filterTextUpperCase;

		// Token: 0x040005E9 RID: 1513
		private readonly ulong filterTextLowerCaseTable;

		// Token: 0x040005EA RID: 1514
		private readonly ulong filterIsNonLetter;

		// Token: 0x040005EB RID: 1515
		private readonly ulong filterIsDigit;

		// Token: 0x040005EC RID: 1516
		private readonly string filterText;

		// Token: 0x040005ED RID: 1517
		private int[] cachedResult;
	}
}
