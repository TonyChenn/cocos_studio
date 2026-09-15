using System;
using System.Collections.Generic;
using System.IO;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Read-only implementation of <see cref="T:ICSharpCode.NRefactory.Editor.IDocument" />.
	/// </summary>
	[Serializable]
	public sealed class ReadOnlyDocument : IDocument, ITextSource, IServiceProvider
	{
		/// <summary>
		/// Creates a new ReadOnlyDocument from the given text source.
		/// </summary>
		public ReadOnlyDocument(ITextSource textSource)
		{
			if (textSource == null)
			{
				throw new ArgumentNullException("textSource");
			}
			this.textSource = textSource.CreateSnapshot();
			List<int> list = new List<int>();
			list.Add(0);
			int num = 0;
			int textLength = textSource.TextLength;
			while ((num = textSource.IndexOfAny(ReadOnlyDocument.newline, num, textLength - num)) >= 0)
			{
				num++;
				if (textSource.GetCharAt(num - 1) == '\r' && num < textLength && textSource.GetCharAt(num) == '\n')
				{
					num++;
				}
				list.Add(num);
			}
			this.lines = list.ToArray();
		}

		/// <summary>
		/// Creates a new ReadOnlyDocument from the given string.
		/// </summary>
		public ReadOnlyDocument(string text) : this(new StringTextSource(text))
		{
		}

		/// <summary>
		/// Creates a new ReadOnlyDocument from the given text source;
		/// and sets IDocument.FileName to the specified file name.
		/// </summary>
		public ReadOnlyDocument(ITextSource textSource, string fileName) : this(textSource)
		{
			this.fileName = fileName;
		}

		/// <inheritdoc />
		public IDocumentLine GetLineByNumber(int lineNumber)
		{
			if (lineNumber < 1 || lineNumber > this.lines.Length)
			{
				throw new ArgumentOutOfRangeException("lineNumber", lineNumber, "Value must be between 1 and " + this.lines.Length);
			}
			return new ReadOnlyDocument.ReadOnlyDocumentLine(this, lineNumber);
		}

		private int GetStartOffset(int lineNumber)
		{
			return this.lines[lineNumber - 1];
		}

		private int GetTotalEndOffset(int lineNumber)
		{
			if (lineNumber >= this.lines.Length)
			{
				return this.textSource.TextLength;
			}
			return this.lines[lineNumber];
		}

		private int GetEndOffset(int lineNumber)
		{
			if (lineNumber == this.lines.Length)
			{
				return this.textSource.TextLength;
			}
			int num = this.lines[lineNumber] - 1;
			if (num > 0 && this.textSource.GetCharAt(num - 1) == '\r' && this.textSource.GetCharAt(num) == '\n')
			{
				num--;
			}
			return num;
		}

		/// <inheritdoc />
		public IDocumentLine GetLineByOffset(int offset)
		{
			return this.GetLineByNumber(this.GetLineNumberForOffset(offset));
		}

		private int GetLineNumberForOffset(int offset)
		{
			int num = Array.BinarySearch<int>(this.lines, offset);
			if (num >= 0)
			{
				return num + 1;
			}
			return ~num;
		}

		/// <inheritdoc />
		public int GetOffset(int line, int column)
		{
			if (line < 1 || line > this.lines.Length)
			{
				throw new ArgumentOutOfRangeException("line", line, "Value must be between 1 and " + this.lines.Length);
			}
			int startOffset = this.GetStartOffset(line);
			if (column <= 1)
			{
				return startOffset;
			}
			int endOffset = this.GetEndOffset(line);
			if (column - 1 >= endOffset - startOffset)
			{
				return endOffset;
			}
			return startOffset + column - 1;
		}

		/// <inheritdoc />
		public int GetOffset(TextLocation location)
		{
			return this.GetOffset(location.Line, location.Column);
		}

		/// <inheritdoc />
		public TextLocation GetLocation(int offset)
		{
			if (offset < 0 || offset > this.textSource.TextLength)
			{
				throw new ArgumentOutOfRangeException("offset", offset, "Value must be between 0 and " + this.textSource.TextLength);
			}
			int lineNumberForOffset = this.GetLineNumberForOffset(offset);
			return new TextLocation(lineNumberForOffset, offset - this.GetStartOffset(lineNumberForOffset) + 1);
		}

		/// <inheritdoc />
		public string Text
		{
			get
			{
				return this.textSource.Text;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <inheritdoc />
		public int LineCount
		{
			get
			{
				return this.lines.Length;
			}
		}

		/// <inheritdoc />
		public ITextSourceVersion Version
		{
			get
			{
				return this.textSource.Version;
			}
		}

		/// <inheritdoc />
		public int TextLength
		{
			get
			{
				return this.textSource.TextLength;
			}
		}

		event EventHandler<TextChangeEventArgs> IDocument.TextChanging
		{
			add
			{
			}
			remove
			{
			}
		}

		event EventHandler<TextChangeEventArgs> IDocument.TextChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		event EventHandler IDocument.ChangeCompleted
		{
			add
			{
			}
			remove
			{
			}
		}

		void IDocument.Insert(int offset, string text)
		{
			throw new NotSupportedException();
		}

		void IDocument.Insert(int offset, string text, AnchorMovementType defaultAnchorMovementType)
		{
			throw new NotSupportedException();
		}

		void IDocument.Remove(int offset, int length)
		{
			throw new NotSupportedException();
		}

		void IDocument.Replace(int offset, int length, string newText)
		{
			throw new NotSupportedException();
		}

		void IDocument.Insert(int offset, ITextSource text)
		{
			throw new NotSupportedException();
		}

		void IDocument.Insert(int offset, ITextSource text, AnchorMovementType defaultAnchorMovementType)
		{
			throw new NotSupportedException();
		}

		void IDocument.Replace(int offset, int length, ITextSource newText)
		{
			throw new NotSupportedException();
		}

		void IDocument.StartUndoableAction()
		{
		}

		void IDocument.EndUndoableAction()
		{
		}

		IDisposable IDocument.OpenUndoGroup()
		{
			return null;
		}

		/// <inheritdoc />
		public ITextAnchor CreateAnchor(int offset)
		{
			return new ReadOnlyDocument.ReadOnlyDocumentTextAnchor(this.GetLocation(offset), offset);
		}

		/// <inheritdoc />
		public ITextSource CreateSnapshot()
		{
			return this.textSource;
		}

		/// <inheritdoc />
		public ITextSource CreateSnapshot(int offset, int length)
		{
			return this.textSource.CreateSnapshot(offset, length);
		}

		/// <inheritdoc />
		public IDocument CreateDocumentSnapshot()
		{
			return this;
		}

		/// <inheritdoc />
		public TextReader CreateReader()
		{
			return this.textSource.CreateReader();
		}

		/// <inheritdoc />
		public TextReader CreateReader(int offset, int length)
		{
			return this.textSource.CreateReader(offset, length);
		}

		/// <inheritdoc />
		public void WriteTextTo(TextWriter writer)
		{
			this.textSource.WriteTextTo(writer);
		}

		/// <inheritdoc />
		public void WriteTextTo(TextWriter writer, int offset, int length)
		{
			this.textSource.WriteTextTo(writer, offset, length);
		}

		/// <inheritdoc />
		public char GetCharAt(int offset)
		{
			return this.textSource.GetCharAt(offset);
		}

		/// <inheritdoc />
		public string GetText(int offset, int length)
		{
			return this.textSource.GetText(offset, length);
		}

		/// <inheritdoc />
		public string GetText(ISegment segment)
		{
			return this.textSource.GetText(segment);
		}

		/// <inheritdoc />
		public int IndexOf(char c, int startIndex, int count)
		{
			return this.textSource.IndexOf(c, startIndex, count);
		}

		/// <inheritdoc />
		public int IndexOfAny(char[] anyOf, int startIndex, int count)
		{
			return this.textSource.IndexOfAny(anyOf, startIndex, count);
		}

		/// <inheritdoc />
		public int IndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.textSource.IndexOf(searchText, startIndex, count, comparisonType);
		}

		/// <inheritdoc />
		public int LastIndexOf(char c, int startIndex, int count)
		{
			return this.textSource.LastIndexOf(c, startIndex, count);
		}

		/// <inheritdoc />
		public int LastIndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.textSource.LastIndexOf(searchText, startIndex, count, comparisonType);
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			return null;
		}

		/// <inheritdoc />
		/// <remarks>Will never be raised on <see cref="T:ICSharpCode.NRefactory.Editor.ReadOnlyDocument" />.</remarks>
		public event EventHandler FileNameChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <inheritdoc />
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		private readonly ITextSource textSource;

		private readonly string fileName;

		private int[] lines;

		private static readonly char[] newline = new char[]
		{
			'\r',
			'\n'
		};

		private sealed class ReadOnlyDocumentLine : IDocumentLine, ISegment
		{
			public ReadOnlyDocumentLine(ReadOnlyDocument doc, int lineNumber)
			{
				this.doc = doc;
				this.lineNumber = lineNumber;
				this.offset = doc.GetStartOffset(lineNumber);
				this.endOffset = doc.GetEndOffset(lineNumber);
			}

			public override int GetHashCode()
			{
				return this.doc.GetHashCode() ^ this.lineNumber;
			}

			public override bool Equals(object obj)
			{
				ReadOnlyDocument.ReadOnlyDocumentLine readOnlyDocumentLine = obj as ReadOnlyDocument.ReadOnlyDocumentLine;
				return readOnlyDocumentLine != null && this.doc == readOnlyDocumentLine.doc && this.lineNumber == readOnlyDocumentLine.lineNumber;
			}

			public int Offset
			{
				get
				{
					return this.offset;
				}
			}

			public int Length
			{
				get
				{
					return this.endOffset - this.offset;
				}
			}

			public int EndOffset
			{
				get
				{
					return this.endOffset;
				}
			}

			public int TotalLength
			{
				get
				{
					return this.doc.GetTotalEndOffset(this.lineNumber) - this.offset;
				}
			}

			public int DelimiterLength
			{
				get
				{
					return this.doc.GetTotalEndOffset(this.lineNumber) - this.endOffset;
				}
			}

			public int LineNumber
			{
				get
				{
					return this.lineNumber;
				}
			}

			public IDocumentLine PreviousLine
			{
				get
				{
					if (this.lineNumber == 1)
					{
						return null;
					}
					return new ReadOnlyDocument.ReadOnlyDocumentLine(this.doc, this.lineNumber - 1);
				}
			}

			public IDocumentLine NextLine
			{
				get
				{
					if (this.lineNumber == this.doc.LineCount)
					{
						return null;
					}
					return new ReadOnlyDocument.ReadOnlyDocumentLine(this.doc, this.lineNumber + 1);
				}
			}

			public bool IsDeleted
			{
				get
				{
					return false;
				}
			}

			private readonly ReadOnlyDocument doc;

			private readonly int lineNumber;

			private readonly int offset;

			private readonly int endOffset;
		}

		private sealed class ReadOnlyDocumentTextAnchor : ITextAnchor
		{
			public ReadOnlyDocumentTextAnchor(TextLocation location, int offset)
			{
				this.location = location;
				this.offset = offset;
			}

			public event EventHandler Deleted
			{
				add
				{
				}
				remove
				{
				}
			}

			public TextLocation Location
			{
				get
				{
					return this.location;
				}
			}

			public int Offset
			{
				get
				{
					return this.offset;
				}
			}

			public AnchorMovementType MovementType { get; set; }

			public bool SurviveDeletion { get; set; }

			public bool IsDeleted
			{
				get
				{
					return false;
				}
			}

			public int Line
			{
				get
				{
					return this.location.Line;
				}
			}

			public int Column
			{
				get
				{
					return this.location.Column;
				}
			}

			private readonly TextLocation location;

			private readonly int offset;
		}
	}
}
