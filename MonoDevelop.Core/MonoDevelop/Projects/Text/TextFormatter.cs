using System;
using System.Text;

namespace MonoDevelop.Projects.Text
{
	// Token: 0x02000201 RID: 513
	public class TextFormatter
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x00050493 File Offset: 0x0004E693
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x0005049B File Offset: 0x0004E69B
		public int MaxColumns { get; set; }

		// Token: 0x06001371 RID: 4977 RVA: 0x000504A4 File Offset: 0x0004E6A4
		public TextFormatter()
		{
			this.MaxColumns = 80;
			this.TabWidth = 4;
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x000504F5 File Offset: 0x0004E6F5
		// (set) Token: 0x06001373 RID: 4979 RVA: 0x000504FD File Offset: 0x0004E6FD
		public int TabWidth
		{
			get
			{
				return this.tabWidth;
			}
			set
			{
				this.tabWidth = value;
				this.formattedIndentString = null;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x0005050D File Offset: 0x0004E70D
		// (set) Token: 0x06001375 RID: 4981 RVA: 0x00050515 File Offset: 0x0004E715
		public string IndentString
		{
			get
			{
				return this.indentString;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.indentString = value;
				this.formattedIndentString = null;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x00050533 File Offset: 0x0004E733
		// (set) Token: 0x06001377 RID: 4983 RVA: 0x0005053B File Offset: 0x0004E73B
		public int LeftMargin
		{
			get
			{
				return this.leftMargin;
			}
			set
			{
				this.leftMargin = value;
				this.formattedIndentString = null;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001378 RID: 4984 RVA: 0x0005054B File Offset: 0x0004E74B
		// (set) Token: 0x06001379 RID: 4985 RVA: 0x00050553 File Offset: 0x0004E753
		public int ParagraphStartMargin
		{
			get
			{
				return this.paragraphStartMargin;
			}
			set
			{
				this.paragraphStartMargin = value;
				this.formattedIndentString = null;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600137A RID: 4986 RVA: 0x00050563 File Offset: 0x0004E763
		// (set) Token: 0x0600137B RID: 4987 RVA: 0x0005056B File Offset: 0x0004E76B
		public WrappingType Wrap
		{
			get
			{
				return this.wrap;
			}
			set
			{
				if (this.wrap != value)
				{
					this.AppendCurrentWord('x');
					this.wrap = value;
				}
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600137C RID: 4988 RVA: 0x00050585 File Offset: 0x0004E785
		// (set) Token: 0x0600137D RID: 4989 RVA: 0x0005058D File Offset: 0x0004E78D
		public bool TabsAsSpaces
		{
			get
			{
				return this.tabsAsSpaces;
			}
			set
			{
				this.tabsAsSpaces = value;
				this.formattedIndentString = null;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x0005059D File Offset: 0x0004E79D
		private string FormattedIndentString
		{
			get
			{
				if (this.formattedIndentString == null)
				{
					this.CreateIndentString();
				}
				return this.formattedIndentString;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x000505B3 File Offset: 0x0004E7B3
		private int IndentColumnWidth
		{
			get
			{
				if (this.formattedIndentString == null)
				{
					this.CreateIndentString();
				}
				return this.indentColumnWidth;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x000505C9 File Offset: 0x0004E7C9
		private string ParagFormattedIndentString
		{
			get
			{
				if (this.formattedIndentString == null)
				{
					this.CreateIndentString();
				}
				return this.paragFormattedIndentString;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001381 RID: 4993 RVA: 0x000505DF File Offset: 0x0004E7DF
		private int ParagIndentColumnWidth
		{
			get
			{
				if (this.formattedIndentString == null)
				{
					this.CreateIndentString();
				}
				return this.paragIndentColumnWidth;
			}
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x000505F5 File Offset: 0x0004E7F5
		public void Clear()
		{
			this.builder = new StringBuilder();
			this.currentWord = new StringBuilder();
			this.curCol = 0;
			this.lineStart = true;
			this.paragraphStart = true;
			this.lastWasSeparator = false;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00050629 File Offset: 0x0004E829
		public void AppendWord(string text)
		{
			this.BeginWord();
			this.Append(text);
			this.EndWord();
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00050640 File Offset: 0x0004E840
		public void Append(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (this.builder.Length == 0)
			{
				this.curCol = this.IndentColumnWidth;
				this.lineStart = true;
				this.paragraphStart = true;
			}
			if (this.Wrap == WrappingType.None || this.Wrap == WrappingType.Char)
			{
				this.AppendChars(text, this.Wrap == WrappingType.Char);
				return;
			}
			int i = 0;
			while (i < text.Length)
			{
				int num = i;
				bool flag = false;
				while (i < text.Length && !flag)
				{
					if ((char.IsWhiteSpace(text[i]) && this.wordLevel == 0) || text[i] == '\n')
					{
						flag = true;
					}
					else
					{
						i++;
					}
				}
				if (i != num)
				{
					this.currentWord.Append(text.Substring(num, i - num));
				}
				if (flag)
				{
					this.AppendCurrentWord(text[i]);
					i++;
				}
			}
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00050715 File Offset: 0x0004E915
		public void AppendLine()
		{
			this.AppendCurrentWord('x');
			this.AppendChar('\n', false);
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00050728 File Offset: 0x0004E928
		public void BeginWord()
		{
			this.wordLevel++;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00050738 File Offset: 0x0004E938
		public void EndWord()
		{
			if (this.wordLevel == 0)
			{
				throw new InvalidOperationException("Missing BeginWord call");
			}
			this.wordLevel--;
			char c = 'x';
			if (this.currentWord.Length > 0)
			{
				c = this.currentWord[this.currentWord.Length - 1];
				if (char.IsWhiteSpace(c))
				{
					this.currentWord.Remove(this.currentWord.Length - 1, 1);
				}
			}
			this.AppendCurrentWord(c);
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x000507B9 File Offset: 0x0004E9B9
		public void FlushWord()
		{
			this.AppendCurrentWord('x');
			if (this.curCol > this.MaxColumns)
			{
				this.AppendSoftBreak();
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x000507D7 File Offset: 0x0004E9D7
		public override string ToString()
		{
			if (this.currentWord.Length > 0)
			{
				this.AppendCurrentWord('x');
			}
			return this.builder.ToString();
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x000507FC File Offset: 0x0004E9FC
		private void AppendChars(string s, bool wrapChars)
		{
			foreach (char c in s)
			{
				this.AppendChar(c, wrapChars);
			}
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0005082C File Offset: 0x0004EA2C
		private void AppendSoftBreak()
		{
			this.AppendChar('\n', true);
			this.paragraphStart = false;
			this.curCol = this.IndentColumnWidth;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0005084C File Offset: 0x0004EA4C
		private void AppendChar(char c, bool wrapChars)
		{
			if (c == '\n')
			{
				this.lineStart = true;
				this.paragraphStart = true;
				this.builder.Append(c);
				this.curCol = this.ParagIndentColumnWidth;
				this.lastWasSeparator = false;
				return;
			}
			if (this.lineStart)
			{
				if (this.paragraphStart)
				{
					this.builder.Append(this.ParagFormattedIndentString);
				}
				else
				{
					this.builder.Append(this.FormattedIndentString);
				}
				this.lineStart = false;
				this.paragraphStart = false;
				this.lastWasSeparator = false;
			}
			if (wrapChars && this.curCol >= this.MaxColumns)
			{
				this.AppendSoftBreak();
				if (!char.IsWhiteSpace(c))
				{
					this.AppendChar(c, false);
				}
				return;
			}
			if (c == '\t')
			{
				int num = this.GetTabWidth(this.curCol);
				if (this.TabsAsSpaces)
				{
					this.builder.Append(' ', num);
				}
				else
				{
					this.builder.Append(c);
				}
				this.curCol += num;
				return;
			}
			this.builder.Append(c);
			this.curCol++;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00050964 File Offset: 0x0004EB64
		private void AppendCurrentWord(char separatorChar)
		{
			if (this.currentWord.Length == 0)
			{
				return;
			}
			if ((this.Wrap == WrappingType.Word || this.Wrap == WrappingType.WordChar) && this.curCol + this.currentWord.Length > this.MaxColumns)
			{
				if (this.lastWasSeparator)
				{
					this.builder.Remove(this.builder.Length - 1, 1);
				}
				if (!this.lineStart)
				{
					this.AppendSoftBreak();
				}
			}
			this.AppendChars(this.currentWord.ToString(), this.Wrap == WrappingType.WordChar);
			if (char.IsWhiteSpace(separatorChar) || (separatorChar == '\n' && !this.lineStart))
			{
				this.lastWasSeparator = true;
				this.AppendChar(separatorChar, true);
			}
			else
			{
				this.lastWasSeparator = false;
			}
			this.currentWord = new StringBuilder();
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00050A30 File Offset: 0x0004EC30
		private int GetTabWidth(int startCol)
		{
			int num = startCol % this.TabWidth;
			if (num == 0)
			{
				return this.TabWidth;
			}
			return this.TabWidth - num;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00050A58 File Offset: 0x0004EC58
		private void CreateIndentString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.indentColumnWidth = this.AddIndentString(stringBuilder, this.indentString);
			this.paragFormattedIndentString = stringBuilder.ToString() + new string(' ', this.paragraphStartMargin);
			this.paragIndentColumnWidth = this.indentColumnWidth + this.paragraphStartMargin;
			if (this.LeftMargin > 0)
			{
				stringBuilder.Append(' ', this.LeftMargin);
				this.indentColumnWidth += this.LeftMargin;
			}
			this.formattedIndentString = stringBuilder.ToString();
			if (this.paragraphStart)
			{
				this.curCol = this.paragIndentColumnWidth;
				return;
			}
			if (this.lineStart)
			{
				this.curCol = this.indentColumnWidth;
			}
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00050B10 File Offset: 0x0004ED10
		private int AddIndentString(StringBuilder sb, string txt)
		{
			if (string.IsNullOrEmpty(txt))
			{
				return 0;
			}
			int num = 0;
			foreach (char c in txt)
			{
				if (c == '\t')
				{
					int num2 = this.GetTabWidth(num);
					num += num2;
					if (this.TabsAsSpaces)
					{
						sb.Append(' ', num2);
					}
					else
					{
						sb.Append(c);
					}
				}
				else
				{
					sb.Append(c);
					num++;
				}
			}
			return num;
		}

		// Token: 0x040005B3 RID: 1459
		private string indentString = "";

		// Token: 0x040005B4 RID: 1460
		private string formattedIndentString;

		// Token: 0x040005B5 RID: 1461
		private int indentColumnWidth;

		// Token: 0x040005B6 RID: 1462
		private string paragFormattedIndentString;

		// Token: 0x040005B7 RID: 1463
		private int paragIndentColumnWidth;

		// Token: 0x040005B8 RID: 1464
		private int leftMargin;

		// Token: 0x040005B9 RID: 1465
		private int paragraphStartMargin;

		// Token: 0x040005BA RID: 1466
		private WrappingType wrap;

		// Token: 0x040005BB RID: 1467
		private int tabWidth;

		// Token: 0x040005BC RID: 1468
		private bool tabsAsSpaces;

		// Token: 0x040005BD RID: 1469
		private StringBuilder builder = new StringBuilder();

		// Token: 0x040005BE RID: 1470
		private StringBuilder currentWord = new StringBuilder();

		// Token: 0x040005BF RID: 1471
		private int curCol;

		// Token: 0x040005C0 RID: 1472
		private bool lineStart = true;

		// Token: 0x040005C1 RID: 1473
		private bool lastWasSeparator;

		// Token: 0x040005C2 RID: 1474
		private bool paragraphStart = true;

		// Token: 0x040005C3 RID: 1475
		private int wordLevel;
	}
}
