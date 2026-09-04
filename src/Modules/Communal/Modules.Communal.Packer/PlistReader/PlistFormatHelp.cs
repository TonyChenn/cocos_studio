using System;
using System.Drawing;

namespace Modules.Communal.Packer.PlistReader
{
	// Token: 0x02000015 RID: 21
	public static class PlistFormatHelp
	{
		// Token: 0x0600007A RID: 122 RVA: 0x00005260 File Offset: 0x00003460
		public static Rectangle ConvertToRect(string plistString)
		{
			int num = plistString.IndexOf('{');
			int num2 = 0;
			int i;
			for (i = 0; i < plistString.Length; i++)
			{
				if (plistString[i] == '}')
				{
					num2++;
				}
				if (num2 == 3)
				{
					break;
				}
			}
			int num3 = i;
			if (num < 0 || num2 != 3)
			{
				throw new FormatException("大括号不匹配," + plistString);
			}
			plistString = plistString.Substring(num + 1, num3 - num - 1);
			int num4 = plistString.IndexOf('}');
			if (num4 < 0)
			{
				throw new FormatException("大括号不匹配," + plistString);
			}
			num4 = plistString.IndexOf(',', num4);
			if (num4 < 0)
			{
				throw new FormatException("大括号不匹配," + plistString);
			}
			string plistString2 = plistString.Substring(0, num4);
			string plistString3 = plistString.Substring(num4 + 1, plistString.Length - num4 - 1);
			Point location = PlistFormatHelp.ConvertToPoint(plistString2);
			Size size = PlistFormatHelp.ConvertToSize(plistString3);
			return new Rectangle(location, size);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00005388 File Offset: 0x00003588
		public static Point ConvertToPoint(string plistString)
		{
			string[] array = PlistFormatHelp.SplitWithForm(plistString);
			int x = Convert.ToInt32(array[0]);
			int y = Convert.ToInt32(array[1]);
			return new Point(x, y);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000053BC File Offset: 0x000035BC
		public static Size ConvertToSize(string plistString)
		{
			string[] array = PlistFormatHelp.SplitWithForm(plistString);
			int width = Convert.ToInt32(array[0]);
			int height = Convert.ToInt32(array[1]);
			return new Size(width, height);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000053F0 File Offset: 0x000035F0
		public static PointF ConvertToPointF(string plistString)
		{
			string[] array = PlistFormatHelp.SplitWithForm(plistString);
			float x = Convert.ToSingle(array[0]);
			float y = Convert.ToSingle(array[1]);
			return new PointF(x, y);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005424 File Offset: 0x00003624
		private static string[] SplitWithForm(string plistString)
		{
			int num = plistString.IndexOf('{');
			int num2 = plistString.IndexOf('}');
			if (num < 0 || num2 < 0 || num > num2)
			{
				throw new FormatException("大括号不匹配," + plistString);
			}
			string text = plistString.Substring(num + 1, num2 - num - 1);
			if (text.Length == 0)
			{
				throw new FormatException("大括号内没有内容," + plistString);
			}
			int num3 = text.IndexOf('{');
			int num4 = text.IndexOf('}');
			if (num3 != -1 || num4 != -1)
			{
				throw new FormatException("大括号不匹配," + plistString);
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			if (array.Length != 2 || array[0].Length == 0 || array[1].Length == 0)
			{
				throw new FormatException("," + plistString);
			}
			return array;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005534 File Offset: 0x00003734
		public static string ConvertToString(Size size)
		{
			return string.Concat(new object[]
			{
				"{",
				size.Width,
				",",
				size.Height,
				"}"
			});
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000558C File Offset: 0x0000378C
		public static string ConvertToString(Point point)
		{
			return string.Concat(new object[]
			{
				"{",
				point.X,
				",",
				point.Y,
				"}"
			});
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000055E4 File Offset: 0x000037E4
		public static string ConvertToString(Rectangle rect)
		{
			return string.Concat(new string[]
			{
				"{",
				PlistFormatHelp.ConvertToString(rect.Location),
				",",
				PlistFormatHelp.ConvertToString(rect.Size),
				"}"
			});
		}
	}
}
