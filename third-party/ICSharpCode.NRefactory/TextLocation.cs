using System;
using System.ComponentModel;
using System.Globalization;

namespace ICSharpCode.NRefactory
{
	/// <summary>
	/// A line/column position.
	/// Text editor lines/columns are counted started from one.
	/// </summary>
	/// <remarks>
	/// The document provides the methods <see cref="M:ICSharpCode.NRefactory.Editor.IDocument.GetLocation(System.Int32)" /> and
	/// <see cref="M:ICSharpCode.NRefactory.Editor.IDocument.GetOffset(ICSharpCode.NRefactory.TextLocation)" /> to convert between offsets and TextLocations.
	/// </remarks>
	// Token: 0x02000052 RID: 82
	[TypeConverter(typeof(TextLocationConverter))]
	[Serializable]
	public struct TextLocation : IComparable<TextLocation>, IEquatable<TextLocation>
	{
		/// <summary>
		/// Creates a TextLocation instance.
		/// </summary>
		// Token: 0x0600024F RID: 591 RVA: 0x00006B77 File Offset: 0x00005B77
		public TextLocation(int line, int column)
		{
			this.line = line;
			this.column = column;
		}

		/// <summary>
		/// Gets the line number.
		/// </summary>
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00006B87 File Offset: 0x00005B87
		public int Line
		{
			get
			{
				return this.line;
			}
		}

		/// <summary>
		/// Gets the column number.
		/// </summary>
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00006B8F File Offset: 0x00005B8F
		public int Column
		{
			get
			{
				return this.column;
			}
		}

		/// <summary>
		/// Gets whether the TextLocation instance is empty.
		/// </summary>
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00006B97 File Offset: 0x00005B97
		public bool IsEmpty
		{
			get
			{
				return this.column < 1 && this.line < 1;
			}
		}

		/// <summary>
		/// Gets a string representation for debugging purposes.
		/// </summary>
		// Token: 0x06000253 RID: 595 RVA: 0x00006BB0 File Offset: 0x00005BB0
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "(Line {1}, Col {0})", new object[]
			{
				this.column,
				this.line
			});
		}

		/// <summary>
		/// Gets a hash code.
		/// </summary>
		// Token: 0x06000254 RID: 596 RVA: 0x00006BF0 File Offset: 0x00005BF0
		public override int GetHashCode()
		{
			return 191 * this.column.GetHashCode() ^ this.line.GetHashCode();
		}

		/// <summary>
		/// Equality test.
		/// </summary>
		// Token: 0x06000255 RID: 597 RVA: 0x00006C0F File Offset: 0x00005C0F
		public override bool Equals(object obj)
		{
			return obj is TextLocation && (TextLocation)obj == this;
		}

		/// <summary>
		/// Equality test.
		/// </summary>
		// Token: 0x06000256 RID: 598 RVA: 0x00006C2C File Offset: 0x00005C2C
		public bool Equals(TextLocation other)
		{
			return this == other;
		}

		/// <summary>
		/// Equality test.
		/// </summary>
		// Token: 0x06000257 RID: 599 RVA: 0x00006C3A File Offset: 0x00005C3A
		public static bool operator ==(TextLocation left, TextLocation right)
		{
			return left.column == right.column && left.line == right.line;
		}

		/// <summary>
		/// Inequality test.
		/// </summary>
		// Token: 0x06000258 RID: 600 RVA: 0x00006C5E File Offset: 0x00005C5E
		public static bool operator !=(TextLocation left, TextLocation right)
		{
			return left.column != right.column || left.line != right.line;
		}

		/// <summary>
		/// Compares two text locations.
		/// </summary>
		// Token: 0x06000259 RID: 601 RVA: 0x00006C85 File Offset: 0x00005C85
		public static bool operator <(TextLocation left, TextLocation right)
		{
			return left.line < right.line || (left.line == right.line && left.column < right.column);
		}

		/// <summary>
		/// Compares two text locations.
		/// </summary>
		// Token: 0x0600025A RID: 602 RVA: 0x00006CBB File Offset: 0x00005CBB
		public static bool operator >(TextLocation left, TextLocation right)
		{
			return left.line > right.line || (left.line == right.line && left.column > right.column);
		}

		/// <summary>
		/// Compares two text locations.
		/// </summary>
		// Token: 0x0600025B RID: 603 RVA: 0x00006CF1 File Offset: 0x00005CF1
		public static bool operator <=(TextLocation left, TextLocation right)
		{
			return !(left > right);
		}

		/// <summary>
		/// Compares two text locations.
		/// </summary>
		// Token: 0x0600025C RID: 604 RVA: 0x00006CFD File Offset: 0x00005CFD
		public static bool operator >=(TextLocation left, TextLocation right)
		{
			return !(left < right);
		}

		/// <summary>
		/// Compares two text locations.
		/// </summary>
		// Token: 0x0600025D RID: 605 RVA: 0x00006D09 File Offset: 0x00005D09
		public int CompareTo(TextLocation other)
		{
			if (this == other)
			{
				return 0;
			}
			if (this < other)
			{
				return -1;
			}
			return 1;
		}

		/// <summary>
		/// Constant of the minimum line.
		/// </summary>
		// Token: 0x040000AF RID: 175
		public const int MinLine = 1;

		/// <summary>
		/// Constant of the minimum column.
		/// </summary>
		// Token: 0x040000B0 RID: 176
		public const int MinColumn = 1;

		/// <summary>
		/// Represents no text location (0, 0).
		/// </summary>
		// Token: 0x040000B1 RID: 177
		public static readonly TextLocation Empty = new TextLocation(0, 0);

		// Token: 0x040000B2 RID: 178
		private int column;

		// Token: 0x040000B3 RID: 179
		private int line;
	}
}
