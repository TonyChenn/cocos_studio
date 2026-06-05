using System;

namespace MonoDevelop.Projects
{
	// Token: 0x02000270 RID: 624
	internal static class MSBuildErrorParser
	{
		// Token: 0x0600168A RID: 5770 RVA: 0x0005A960 File Offset: 0x00058B60
		public static MSBuildErrorParser.Result TryParseLine(string line)
		{
			int num = 0;
			MSBuildErrorParser.Result result = new MSBuildErrorParser.Result();
			MSBuildErrorParser.MoveNextNonSpace(line, ref num);
			if (num >= line.Length)
			{
				return null;
			}
			int num2;
			if (line[num] != ':')
			{
				if (num + 2 >= line.Length)
				{
					return null;
				}
				if ((num2 = line.IndexOf(':', num + 2) - 1) < 0)
				{
					return null;
				}
			}
			else
			{
				num2 = num;
			}
			int num3 = num2 + 2;
			if (num3 > line.Length)
			{
				return null;
			}
			MSBuildErrorParser.MovePrevNonSpace(line, ref num2, 0);
			if (num2 < 0 || num2 < num)
			{
				return null;
			}
			MSBuildErrorParser.MoveNextNonSpace(line, ref num3);
			int num4 = line.IndexOf(':', num3) - 1;
			int num5 = num4 + 2;
			if (num4 >= 0)
			{
				MSBuildErrorParser.MovePrevNonSpace(line, ref num4, 0);
				if (num4 <= num3)
				{
					num4 = -1;
				}
			}
			if (num4 > 0 && MSBuildErrorParser.ParseCategory(line, num3, num4, result))
			{
				if (num2 > num && !MSBuildErrorParser.ParseOrigin(line, num, num2, result))
				{
					return null;
				}
			}
			else
			{
				if (!MSBuildErrorParser.ParseCategory(line, num, num2, result))
				{
					return null;
				}
				num5 = num3;
			}
			MSBuildErrorParser.MoveNextNonSpace(line, ref num5);
			int num6 = line.Length - 1;
			MSBuildErrorParser.MovePrevNonSpace(line, ref num6, num5);
			if (num6 > num5)
			{
				result.Message = line.Substring(num5, num6 - num5 + 1);
			}
			else
			{
				result.Message = "";
			}
			return result;
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x0005AA80 File Offset: 0x00058C80
		private static bool ParseOrigin(string line, int start, int end, MSBuildErrorParser.Result result)
		{
			if (line[end] != ')')
			{
				result.Origin = line.Substring(start, end - start + 1);
				return true;
			}
			int num = line.LastIndexOf('(', end - 2, end - start - 2);
			if (num < 0)
			{
				return false;
			}
			if (!MSBuildErrorParser.ParsePosition(line, num + 1, end, result))
			{
				result.Origin = line.Substring(start, end - start + 1);
				return true;
			}
			end = num - 1;
			MSBuildErrorParser.MovePrevNonSpace(line, ref end, start);
			result.Origin = line.Substring(start, end - start + 1);
			return true;
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x0005AB08 File Offset: 0x00058D08
		private static bool ParseLineColVal(string str, out int val)
		{
			bool result;
			try
			{
				val = int.Parse(str);
				result = true;
			}
			catch (OverflowException)
			{
				val = 0;
				result = true;
			}
			catch (FormatException)
			{
				val = 0;
				result = false;
			}
			return result;
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x0005AB50 File Offset: 0x00058D50
		private static bool ParsePosition(string str, int start, int end, MSBuildErrorParser.Result result)
		{
			int line = 0;
			int column = 0;
			int endLine = 0;
			int endColumn = 0;
			string[] array = str.Substring(start, end - start).Split(new char[]
			{
				','
			});
			if (array.Length > 4 || array.Length == 3)
			{
				return true;
			}
			if (array.Length == 4)
			{
				if (!MSBuildErrorParser.ParseLineColVal(array[0], out line) || !MSBuildErrorParser.ParseLineColVal(array[1], out column) || !MSBuildErrorParser.ParseLineColVal(array[2], out endLine) || !MSBuildErrorParser.ParseLineColVal(array[3], out endColumn))
				{
					return false;
				}
			}
			else
			{
				string[] array2 = array[0].Split(new char[]
				{
					'-'
				});
				if (array2.Length > 2)
				{
					return true;
				}
				if (!MSBuildErrorParser.ParseLineColVal(array2[0], out line))
				{
					return false;
				}
				if (array2.Length == 2)
				{
					if (array.Length == 2)
					{
						return true;
					}
					if (!MSBuildErrorParser.ParseLineColVal(array2[1], out endLine))
					{
						return false;
					}
				}
				if (array.Length == 2)
				{
					string[] array3 = array[1].Split(new char[]
					{
						'-'
					});
					if (array3.Length > 2)
					{
						return true;
					}
					if (!MSBuildErrorParser.ParseLineColVal(array3[0], out column))
					{
						return false;
					}
					if (array3.Length == 2 && !MSBuildErrorParser.ParseLineColVal(array3[1], out endColumn))
					{
						return false;
					}
				}
			}
			result.Line = line;
			result.Column = column;
			result.EndLine = endLine;
			result.EndColumn = endColumn;
			return true;
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x0005AC9C File Offset: 0x00058E9C
		private static bool ParseCategory(string line, int start, int end, MSBuildErrorParser.Result result)
		{
			int num = end;
			MSBuildErrorParser.MovePrevWordStart(line, ref num, start);
			if (num < start + 1)
			{
				return false;
			}
			string code = line.Substring(num, end - num + 1);
			num--;
			MSBuildErrorParser.MovePrevNonSpace(line, ref num, start);
			end = num;
			MSBuildErrorParser.MovePrevWordStart(line, ref num, start);
			if (num < start)
			{
				return false;
			}
			string a = line.Substring(num, end - num + 1);
			if (string.Equals(a, "error", StringComparison.OrdinalIgnoreCase))
			{
				result.IsError = true;
			}
			else if (!string.Equals(a, "warning", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			result.Code = code;
			num--;
			if (num > start)
			{
				MSBuildErrorParser.MovePrevNonSpace(line, ref num, start);
				result.Subcategory = line.Substring(start, num - start + 1);
			}
			else
			{
				result.Subcategory = "";
			}
			return true;
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x0005AD54 File Offset: 0x00058F54
		private static void MoveNextNonSpace(string s, ref int idx)
		{
			while (idx < s.Length && char.IsWhiteSpace(s[idx]))
			{
				idx++;
			}
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x0005AD77 File Offset: 0x00058F77
		private static void MovePrevNonSpace(string s, ref int idx, int min = 0)
		{
			while (idx > min && char.IsWhiteSpace(s[idx]))
			{
				idx--;
			}
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x0005AD95 File Offset: 0x00058F95
		private static void MovePrevWordStart(string s, ref int idx, int min = 0)
		{
			while (idx > min && char.IsLetterOrDigit(s[idx - 1]))
			{
				idx--;
			}
		}

		// Token: 0x02000271 RID: 625
		public class Result
		{
			// Token: 0x170004D3 RID: 1235
			// (get) Token: 0x06001692 RID: 5778 RVA: 0x0005ADB5 File Offset: 0x00058FB5
			// (set) Token: 0x06001693 RID: 5779 RVA: 0x0005ADBD File Offset: 0x00058FBD
			public string Origin { get; set; }

			// Token: 0x170004D4 RID: 1236
			// (get) Token: 0x06001694 RID: 5780 RVA: 0x0005ADC6 File Offset: 0x00058FC6
			// (set) Token: 0x06001695 RID: 5781 RVA: 0x0005ADCE File Offset: 0x00058FCE
			public int Line { get; set; }

			// Token: 0x170004D5 RID: 1237
			// (get) Token: 0x06001696 RID: 5782 RVA: 0x0005ADD7 File Offset: 0x00058FD7
			// (set) Token: 0x06001697 RID: 5783 RVA: 0x0005ADDF File Offset: 0x00058FDF
			public int Column { get; set; }

			// Token: 0x170004D6 RID: 1238
			// (get) Token: 0x06001698 RID: 5784 RVA: 0x0005ADE8 File Offset: 0x00058FE8
			// (set) Token: 0x06001699 RID: 5785 RVA: 0x0005ADF0 File Offset: 0x00058FF0
			public int EndLine { get; set; }

			// Token: 0x170004D7 RID: 1239
			// (get) Token: 0x0600169A RID: 5786 RVA: 0x0005ADF9 File Offset: 0x00058FF9
			// (set) Token: 0x0600169B RID: 5787 RVA: 0x0005AE01 File Offset: 0x00059001
			public int EndColumn { get; set; }

			// Token: 0x170004D8 RID: 1240
			// (get) Token: 0x0600169C RID: 5788 RVA: 0x0005AE0A File Offset: 0x0005900A
			// (set) Token: 0x0600169D RID: 5789 RVA: 0x0005AE12 File Offset: 0x00059012
			public string Subcategory { get; set; }

			// Token: 0x170004D9 RID: 1241
			// (get) Token: 0x0600169E RID: 5790 RVA: 0x0005AE1B File Offset: 0x0005901B
			// (set) Token: 0x0600169F RID: 5791 RVA: 0x0005AE23 File Offset: 0x00059023
			public bool IsError { get; set; }

			// Token: 0x170004DA RID: 1242
			// (get) Token: 0x060016A0 RID: 5792 RVA: 0x0005AE2C File Offset: 0x0005902C
			// (set) Token: 0x060016A1 RID: 5793 RVA: 0x0005AE34 File Offset: 0x00059034
			public string Code { get; set; }

			// Token: 0x170004DB RID: 1243
			// (get) Token: 0x060016A2 RID: 5794 RVA: 0x0005AE3D File Offset: 0x0005903D
			// (set) Token: 0x060016A3 RID: 5795 RVA: 0x0005AE45 File Offset: 0x00059045
			public string Message { get; set; }
		}
	}
}
