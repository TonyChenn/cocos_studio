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
	[TypeConverter(typeof(TextLocationConverter))]
	[Serializable]
	public struct TextLocation : IComparable<TextLocation>, IEquatable<TextLocation>
	{
		/// <summary>
		/// Creates a TextLocation instance.
		/// </summary>
		public TextLocation(int line, int column)
		{
			this.line = line;
			this.column = column;
		}

		/// <summary>
		/// Gets the line number.
		/// </summary>
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
		public override int GetHashCode()
		{
			return 191 * this.column.GetHashCode() ^ this.line.GetHashCode();
		}

		/// <summary>
		/// Equality test.
		/// </summary>
		public override bool Equals(object obj)
		{
			return obj is TextLocation && (TextLocation)obj == this;
		}

		/// <summary>
		/// Equality test.
		/// </summary>
		public bool Equals(TextLocation other)
		{
			return this == other;
		}

		/// <summary>
		/// Equality test.
		/// </summary>
		public static bool operator ==(TextLocation left, TextLocation right)
		{
			return left.column == right.column && left.line == right.line;
		}

		/// <summary>
		/// Inequality test.
		/// </summary>
		public static bool operator !=(TextLocation left, TextLocation right)
		{
			return left.column != right.column || left.line != right.line;
		}

		/// <summary>
		/// Compares two text locations.
		/// </summary>
		public static bool operator <(TextLocation left, TextLocation right)
		{
			return left.line < right.line || (left.line == right.line && left.column < right.column);
		}

		/// <summary>
		/// Compares two text locations.
		/// </summary>
		public static bool operator >(TextLocation left, TextLocation right)
		{
			return left.line > right.line || (left.line == right.line && left.column > right.column);
		}

		/// <summary>
		/// Compares two text locations.
		/// </summary>
		public static bool operator <=(TextLocation left, TextLocation right)
		{
			return !(left > right);
		}

		/// <summary>
		/// Compares two text locations.
		/// </summary>
		public static bool operator >=(TextLocation left, TextLocation right)
		{
			return !(left < right);
		}

		/// <summary>
		/// Compares two text locations.
		/// </summary>
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
		public const int MinLine = 1;

		/// <summary>
		/// Constant of the minimum column.
		/// </summary>
		public const int MinColumn = 1;

		/// <summary>
		/// Represents no text location (0, 0).
		/// </summary>
		public static readonly TextLocation Empty = new TextLocation(0, 0);

		private int column;

		private int line;
	}
}
