using System;
using System.Globalization;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public struct DomRegion : IEquatable<DomRegion>
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00008DC4 File Offset: 0x00007DC4
		public bool IsEmpty
		{
			get
			{
				return this.BeginLine <= 0;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00008DD2 File Offset: 0x00007DD2
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x00008DDA File Offset: 0x00007DDA
		public int BeginLine
		{
			get
			{
				return this.beginLine;
			}
		}

		/// <value>
		/// if the end line is == -1 the end column is -1 too
		/// this stands for an unknwon end
		/// </value>
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x00008DE2 File Offset: 0x00007DE2
		public int EndLine
		{
			get
			{
				return this.endLine;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060003AA RID: 938 RVA: 0x00008DEA File Offset: 0x00007DEA
		public int BeginColumn
		{
			get
			{
				return this.beginColumn;
			}
		}

		/// <value>
		/// if the end column is == -1 the end line is -1 too
		/// this stands for an unknown end
		/// </value>
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00008DF2 File Offset: 0x00007DF2
		public int EndColumn
		{
			get
			{
				return this.endColumn;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00008DFA File Offset: 0x00007DFA
		public TextLocation Begin
		{
			get
			{
				return new TextLocation(this.beginLine, this.beginColumn);
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00008E0D File Offset: 0x00007E0D
		public TextLocation End
		{
			get
			{
				return new TextLocation(this.endLine, this.endColumn);
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00008E20 File Offset: 0x00007E20
		public DomRegion(int beginLine, int beginColumn, int endLine, int endColumn)
		{
			this = new DomRegion(null, beginLine, beginColumn, endLine, endColumn);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00008E2E File Offset: 0x00007E2E
		public DomRegion(string fileName, int beginLine, int beginColumn, int endLine, int endColumn)
		{
			this.fileName = fileName;
			this.beginLine = beginLine;
			this.beginColumn = beginColumn;
			this.endLine = endLine;
			this.endColumn = endColumn;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00008E55 File Offset: 0x00007E55
		public DomRegion(int beginLine, int beginColumn)
		{
			this = new DomRegion(null, beginLine, beginColumn);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00008E60 File Offset: 0x00007E60
		public DomRegion(string fileName, int beginLine, int beginColumn)
		{
			this.fileName = fileName;
			this.beginLine = beginLine;
			this.beginColumn = beginColumn;
			this.endLine = -1;
			this.endColumn = -1;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00008E85 File Offset: 0x00007E85
		public DomRegion(TextLocation begin, TextLocation end)
		{
			this = new DomRegion(null, begin, end);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00008E90 File Offset: 0x00007E90
		public DomRegion(string fileName, TextLocation begin, TextLocation end)
		{
			this.fileName = fileName;
			this.beginLine = begin.Line;
			this.beginColumn = begin.Column;
			this.endLine = end.Line;
			this.endColumn = end.Column;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00008ECD File Offset: 0x00007ECD
		public DomRegion(TextLocation begin)
		{
			this = new DomRegion(null, begin);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00008ED7 File Offset: 0x00007ED7
		public DomRegion(string fileName, TextLocation begin)
		{
			this.fileName = fileName;
			this.beginLine = begin.Line;
			this.beginColumn = begin.Column;
			this.endLine = -1;
			this.endColumn = -1;
		}

		/// <remarks>
		/// Returns true, if the given coordinates (line, column) are in the region.
		/// This method assumes that for an unknown end the end line is == -1
		/// </remarks>
		// Token: 0x060003B6 RID: 950 RVA: 0x00008F08 File Offset: 0x00007F08
		public bool IsInside(int line, int column)
		{
			return !this.IsEmpty && (line >= this.BeginLine && (line <= this.EndLine || this.EndLine == -1) && (line != this.BeginLine || column >= this.BeginColumn)) && (line != this.EndLine || column <= this.EndColumn);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00008F65 File Offset: 0x00007F65
		public bool IsInside(TextLocation location)
		{
			return this.IsInside(location.Line, location.Column);
		}

		/// <remarks>
		/// Returns true, if the given coordinates (line, column) are in the region.
		/// This method assumes that for an unknown end the end line is == -1
		/// </remarks>
		// Token: 0x060003B8 RID: 952 RVA: 0x00008F7C File Offset: 0x00007F7C
		public bool Contains(int line, int column)
		{
			return !this.IsEmpty && (line >= this.BeginLine && (line <= this.EndLine || this.EndLine == -1) && (line != this.BeginLine || column >= this.BeginColumn)) && (line != this.EndLine || column < this.EndColumn);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00008FD6 File Offset: 0x00007FD6
		public bool Contains(TextLocation location)
		{
			return this.Contains(location.Line, location.Column);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00008FEC File Offset: 0x00007FEC
		public bool IntersectsWith(DomRegion region)
		{
			return region.Begin <= this.End && region.End >= this.Begin;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00009018 File Offset: 0x00008018
		public bool OverlapsWith(DomRegion region)
		{
			TextLocation left = (this.Begin > region.Begin) ? this.Begin : region.Begin;
			TextLocation right = (this.End < region.End) ? this.End : region.End;
			return left < right;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00009074 File Offset: 0x00008074
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "[DomRegion FileName={0}, Begin=({1}, {2}), End=({3}, {4})]", new object[]
			{
				this.fileName,
				this.beginLine,
				this.beginColumn,
				this.endLine,
				this.endColumn
			});
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000090D9 File Offset: 0x000080D9
		public override bool Equals(object obj)
		{
			return obj is DomRegion && this.Equals((DomRegion)obj);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000090F4 File Offset: 0x000080F4
		public override int GetHashCode()
		{
			int num = (this.fileName != null) ? this.fileName.GetHashCode() : 0;
			return num ^ this.beginColumn + 1100009 * this.beginLine + 1200007 * this.endLine + 1300021 * this.endColumn;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000914C File Offset: 0x0000814C
		public bool Equals(DomRegion other)
		{
			return this.beginLine == other.beginLine && this.beginColumn == other.beginColumn && this.endLine == other.endLine && this.endColumn == other.endColumn && this.fileName == other.fileName;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000091A9 File Offset: 0x000081A9
		public static bool operator ==(DomRegion left, DomRegion right)
		{
			return left.Equals(right);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000091B3 File Offset: 0x000081B3
		public static bool operator !=(DomRegion left, DomRegion right)
		{
			return !left.Equals(right);
		}

		// Token: 0x040000E4 RID: 228
		private readonly string fileName;

		// Token: 0x040000E5 RID: 229
		private readonly int beginLine;

		// Token: 0x040000E6 RID: 230
		private readonly int endLine;

		// Token: 0x040000E7 RID: 231
		private readonly int beginColumn;

		// Token: 0x040000E8 RID: 232
		private readonly int endColumn;

		// Token: 0x040000E9 RID: 233
		public static readonly DomRegion Empty = default(DomRegion);
	}
}
