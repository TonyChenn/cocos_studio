using System;
using System.IO;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Implements the ITextSource interface using a string.
	/// </summary>
	[Serializable]
	public class StringTextSource : ITextSource
	{
		/// <summary>
		/// Creates a new StringTextSource with the given text.
		/// </summary>
		public StringTextSource(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			this.text = text;
		}

		/// <summary>
		/// Creates a new StringTextSource with the given text.
		/// </summary>
		public StringTextSource(string text, ITextSourceVersion version)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			this.text = text;
			this.version = version;
		}

		/// <inheritdoc />
		public ITextSourceVersion Version
		{
			get
			{
				return this.version;
			}
		}

		/// <inheritdoc />
		public int TextLength
		{
			get
			{
				return this.text.Length;
			}
		}

		/// <inheritdoc />
		public string Text
		{
			get
			{
				return this.text;
			}
		}

		/// <inheritdoc />
		public ITextSource CreateSnapshot()
		{
			return this;
		}

		/// <inheritdoc />
		public ITextSource CreateSnapshot(int offset, int length)
		{
			return new StringTextSource(this.text.Substring(offset, length));
		}

		/// <inheritdoc />
		public TextReader CreateReader()
		{
			return new StringReader(this.text);
		}

		/// <inheritdoc />
		public TextReader CreateReader(int offset, int length)
		{
			return new StringReader(this.text.Substring(offset, length));
		}

		/// <inheritdoc />
		public void WriteTextTo(TextWriter writer)
		{
			writer.Write(this.text);
		}

		/// <inheritdoc />
		public void WriteTextTo(TextWriter writer, int offset, int length)
		{
			writer.Write(this.text.Substring(offset, length));
		}

		/// <inheritdoc />
		public char GetCharAt(int offset)
		{
			return this.text[offset];
		}

		/// <inheritdoc />
		public string GetText(int offset, int length)
		{
			return this.text.Substring(offset, length);
		}

		/// <inheritdoc />
		public string GetText(ISegment segment)
		{
			if (segment == null)
			{
				throw new ArgumentNullException("segment");
			}
			return this.text.Substring(segment.Offset, segment.Length);
		}

		/// <inheritdoc />
		public int IndexOf(char c, int startIndex, int count)
		{
			return this.text.IndexOf(c, startIndex, count);
		}

		/// <inheritdoc />
		public int IndexOfAny(char[] anyOf, int startIndex, int count)
		{
			return this.text.IndexOfAny(anyOf, startIndex, count);
		}

		/// <inheritdoc />
		public int IndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.text.IndexOf(searchText, startIndex, count, comparisonType);
		}

		/// <inheritdoc />
		public int LastIndexOf(char c, int startIndex, int count)
		{
			return this.text.LastIndexOf(c, startIndex + count - 1, count);
		}

		/// <inheritdoc />
		public int LastIndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.text.LastIndexOf(searchText, startIndex + count - 1, count, comparisonType);
		}

		/// <summary>
		/// Gets a text source containing the empty string.
		/// </summary>
		public static readonly StringTextSource Empty = new StringTextSource(string.Empty);

		private readonly string text;

		private readonly ITextSourceVersion version;
	}
}
