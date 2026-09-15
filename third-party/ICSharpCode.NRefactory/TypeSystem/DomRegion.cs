using System;
using System.Globalization;

namespace ICSharpCode.NRefactory.TypeSystem
{
	[Serializable]
	public struct DomRegion : IEquatable<DomRegion>
	{
		public bool IsEmpty
		{
			get
			{
				return this.BeginLine <= 0;
			}
		}

		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

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
		public int EndLine
		{
			get
			{
				return this.endLine;
			}
		}

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
		public int EndColumn
		{
			get
			{
				return this.endColumn;
			}
		}

		public TextLocation Begin
		{
			get
			{
				return new TextLocation(this.beginLine, this.beginColumn);
			}
		}

		public TextLocation End
		{
			get
			{
				return new TextLocation(this.endLine, this.endColumn);
			}
		}

		public DomRegion(int beginLine, int beginColumn, int endLine, int endColumn)
		{
			this = new DomRegion(null, beginLine, beginColumn, endLine, endColumn);
		}

		public DomRegion(string fileName, int beginLine, int beginColumn, int endLine, int endColumn)
		{
			this.fileName = fileName;
			this.beginLine = beginLine;
			this.beginColumn = beginColumn;
			this.endLine = endLine;
			this.endColumn = endColumn;
		}

		public DomRegion(int beginLine, int beginColumn)
		{
			this = new DomRegion(null, beginLine, beginColumn);
		}

		public DomRegion(string fileName, int beginLine, int beginColumn)
		{
			this.fileName = fileName;
			this.beginLine = beginLine;
			this.beginColumn = beginColumn;
			this.endLine = -1;
			this.endColumn = -1;
		}

		public DomRegion(TextLocation begin, TextLocation end)
		{
			this = new DomRegion(null, begin, end);
		}

		public DomRegion(string fileName, TextLocation begin, TextLocation end)
		{
			this.fileName = fileName;
			this.beginLine = begin.Line;
			this.beginColumn = begin.Column;
			this.endLine = end.Line;
			this.endColumn = end.Column;
		}

		public DomRegion(TextLocation begin)
		{
			this = new DomRegion(null, begin);
		}

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
		public bool IsInside(int line, int column)
		{
			return !this.IsEmpty && (line >= this.BeginLine && (line <= this.EndLine || this.EndLine == -1) && (line != this.BeginLine || column >= this.BeginColumn)) && (line != this.EndLine || column <= this.EndColumn);
		}

		public bool IsInside(TextLocation location)
		{
			return this.IsInside(location.Line, location.Column);
		}

		/// <remarks>
		/// Returns true, if the given coordinates (line, column) are in the region.
		/// This method assumes that for an unknown end the end line is == -1
		/// </remarks>
		public bool Contains(int line, int column)
		{
			return !this.IsEmpty && (line >= this.BeginLine && (line <= this.EndLine || this.EndLine == -1) && (line != this.BeginLine || column >= this.BeginColumn)) && (line != this.EndLine || column < this.EndColumn);
		}

		public bool Contains(TextLocation location)
		{
			return this.Contains(location.Line, location.Column);
		}

		public bool IntersectsWith(DomRegion region)
		{
			return region.Begin <= this.End && region.End >= this.Begin;
		}

		public bool OverlapsWith(DomRegion region)
		{
			TextLocation left = (this.Begin > region.Begin) ? this.Begin : region.Begin;
			TextLocation right = (this.End < region.End) ? this.End : region.End;
			return left < right;
		}

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

		public override bool Equals(object obj)
		{
			return obj is DomRegion && this.Equals((DomRegion)obj);
		}

		public override int GetHashCode()
		{
			int num = (this.fileName != null) ? this.fileName.GetHashCode() : 0;
			return num ^ this.beginColumn + 1100009 * this.beginLine + 1200007 * this.endLine + 1300021 * this.endColumn;
		}

		public bool Equals(DomRegion other)
		{
			return this.beginLine == other.beginLine && this.beginColumn == other.beginColumn && this.endLine == other.endLine && this.endColumn == other.endColumn && this.fileName == other.fileName;
		}

		public static bool operator ==(DomRegion left, DomRegion right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(DomRegion left, DomRegion right)
		{
			return !left.Equals(right);
		}

		private readonly string fileName;

		private readonly int beginLine;

		private readonly int endLine;

		private readonly int beginColumn;

		private readonly int endColumn;

		public static readonly DomRegion Empty = default(DomRegion);
	}
}
