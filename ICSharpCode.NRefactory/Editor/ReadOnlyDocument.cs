using System;
using System.Collections.Generic;
using System.IO;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Read-only implementation of <see cref="T:ICSharpCode.NRefactory.Editor.IDocument" />.
	/// </summary>
	// Token: 0x02000014 RID: 20
	[Serializable]
	public sealed class ReadOnlyDocument : IDocument, ITextSource, IServiceProvider
	{
		/// <summary>
		/// Creates a new ReadOnlyDocument from the given text source.
		/// </summary>
		// Token: 0x0600006E RID: 110 RVA: 0x00002DD0 File Offset: 0x00001DD0
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
		// Token: 0x0600006F RID: 111 RVA: 0x00002E60 File Offset: 0x00001E60
		public ReadOnlyDocument(string text) : this(new StringTextSource(text))
		{
		}

		/// <summary>
		/// Creates a new ReadOnlyDocument from the given text source;
		/// and sets IDocument.FileName to the specified file name.
		/// </summary>
		// Token: 0x06000070 RID: 112 RVA: 0x00002E6E File Offset: 0x00001E6E
		public ReadOnlyDocument(ITextSource textSource, string fileName) : this(textSource)
		{
			this.fileName = fileName;
		}

		/// <inheritdoc />
		// Token: 0x06000071 RID: 113 RVA: 0x00002E7E File Offset: 0x00001E7E
		public IDocumentLine GetLineByNumber(int lineNumber)
		{
			if (lineNumber < 1 || lineNumber > this.lines.Length)
			{
				throw new ArgumentOutOfRangeException("lineNumber", lineNumber, "Value must be between 1 and " + this.lines.Length);
			}
			return new ReadOnlyDocument.ReadOnlyDocumentLine(this, lineNumber);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002EBE File Offset: 0x00001EBE
		private int GetStartOffset(int lineNumber)
		{
			return this.lines[lineNumber - 1];
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002ECA File Offset: 0x00001ECA
		private int GetTotalEndOffset(int lineNumber)
		{
			if (lineNumber >= this.lines.Length)
			{
				return this.textSource.TextLength;
			}
			return this.lines[lineNumber];
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002EEC File Offset: 0x00001EEC
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
		// Token: 0x06000075 RID: 117 RVA: 0x00002F46 File Offset: 0x00001F46
		public IDocumentLine GetLineByOffset(int offset)
		{
			return this.GetLineByNumber(this.GetLineNumberForOffset(offset));
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002F58 File Offset: 0x00001F58
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
		// Token: 0x06000077 RID: 119 RVA: 0x00002F7C File Offset: 0x00001F7C
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
		// Token: 0x06000078 RID: 120 RVA: 0x00002FE5 File Offset: 0x00001FE5
		public int GetOffset(TextLocation location)
		{
			return this.GetOffset(location.Line, location.Column);
		}

		/// <inheritdoc />
		// Token: 0x06000079 RID: 121 RVA: 0x00002FFC File Offset: 0x00001FFC
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
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600007A RID: 122 RVA: 0x0000305F File Offset: 0x0000205F
		// (set) Token: 0x0600007B RID: 123 RVA: 0x0000306C File Offset: 0x0000206C
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
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003073 File Offset: 0x00002073
		public int LineCount
		{
			get
			{
				return this.lines.Length;
			}
		}

		/// <inheritdoc />
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007D RID: 125 RVA: 0x0000307D File Offset: 0x0000207D
		public ITextSourceVersion Version
		{
			get
			{
				return this.textSource.Version;
			}
		}

		/// <inheritdoc />
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600007E RID: 126 RVA: 0x0000308A File Offset: 0x0000208A
		public int TextLength
		{
			get
			{
				return this.textSource.TextLength;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600007F RID: 127 RVA: 0x00003097 File Offset: 0x00002097
		// (remove) Token: 0x06000080 RID: 128 RVA: 0x00003099 File Offset: 0x00002099
		event EventHandler<TextChangeEventArgs> IDocument.TextChanging
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000081 RID: 129 RVA: 0x0000309B File Offset: 0x0000209B
		// (remove) Token: 0x06000082 RID: 130 RVA: 0x0000309D File Offset: 0x0000209D
		event EventHandler<TextChangeEventArgs> IDocument.TextChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000083 RID: 131 RVA: 0x0000309F File Offset: 0x0000209F
		// (remove) Token: 0x06000084 RID: 132 RVA: 0x000030A1 File Offset: 0x000020A1
		event EventHandler IDocument.ChangeCompleted
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000030A3 File Offset: 0x000020A3
		void IDocument.Insert(int offset, string text)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000030AA File Offset: 0x000020AA
		void IDocument.Insert(int offset, string text, AnchorMovementType defaultAnchorMovementType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000030B1 File Offset: 0x000020B1
		void IDocument.Remove(int offset, int length)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000030B8 File Offset: 0x000020B8
		void IDocument.Replace(int offset, int length, string newText)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000030BF File Offset: 0x000020BF
		void IDocument.Insert(int offset, ITextSource text)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000030C6 File Offset: 0x000020C6
		void IDocument.Insert(int offset, ITextSource text, AnchorMovementType defaultAnchorMovementType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000030CD File Offset: 0x000020CD
		void IDocument.Replace(int offset, int length, ITextSource newText)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000030D4 File Offset: 0x000020D4
		void IDocument.StartUndoableAction()
		{
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000030D6 File Offset: 0x000020D6
		void IDocument.EndUndoableAction()
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000030D8 File Offset: 0x000020D8
		IDisposable IDocument.OpenUndoGroup()
		{
			return null;
		}

		/// <inheritdoc />
		// Token: 0x0600008F RID: 143 RVA: 0x000030DB File Offset: 0x000020DB
		public ITextAnchor CreateAnchor(int offset)
		{
			return new ReadOnlyDocument.ReadOnlyDocumentTextAnchor(this.GetLocation(offset), offset);
		}

		/// <inheritdoc />
		// Token: 0x06000090 RID: 144 RVA: 0x000030EA File Offset: 0x000020EA
		public ITextSource CreateSnapshot()
		{
			return this.textSource;
		}

		/// <inheritdoc />
		// Token: 0x06000091 RID: 145 RVA: 0x000030F2 File Offset: 0x000020F2
		public ITextSource CreateSnapshot(int offset, int length)
		{
			return this.textSource.CreateSnapshot(offset, length);
		}

		/// <inheritdoc />
		// Token: 0x06000092 RID: 146 RVA: 0x00003101 File Offset: 0x00002101
		public IDocument CreateDocumentSnapshot()
		{
			return this;
		}

		/// <inheritdoc />
		// Token: 0x06000093 RID: 147 RVA: 0x00003104 File Offset: 0x00002104
		public TextReader CreateReader()
		{
			return this.textSource.CreateReader();
		}

		/// <inheritdoc />
		// Token: 0x06000094 RID: 148 RVA: 0x00003111 File Offset: 0x00002111
		public TextReader CreateReader(int offset, int length)
		{
			return this.textSource.CreateReader(offset, length);
		}

		/// <inheritdoc />
		// Token: 0x06000095 RID: 149 RVA: 0x00003120 File Offset: 0x00002120
		public void WriteTextTo(TextWriter writer)
		{
			this.textSource.WriteTextTo(writer);
		}

		/// <inheritdoc />
		// Token: 0x06000096 RID: 150 RVA: 0x0000312E File Offset: 0x0000212E
		public void WriteTextTo(TextWriter writer, int offset, int length)
		{
			this.textSource.WriteTextTo(writer, offset, length);
		}

		/// <inheritdoc />
		// Token: 0x06000097 RID: 151 RVA: 0x0000313E File Offset: 0x0000213E
		public char GetCharAt(int offset)
		{
			return this.textSource.GetCharAt(offset);
		}

		/// <inheritdoc />
		// Token: 0x06000098 RID: 152 RVA: 0x0000314C File Offset: 0x0000214C
		public string GetText(int offset, int length)
		{
			return this.textSource.GetText(offset, length);
		}

		/// <inheritdoc />
		// Token: 0x06000099 RID: 153 RVA: 0x0000315B File Offset: 0x0000215B
		public string GetText(ISegment segment)
		{
			return this.textSource.GetText(segment);
		}

		/// <inheritdoc />
		// Token: 0x0600009A RID: 154 RVA: 0x00003169 File Offset: 0x00002169
		public int IndexOf(char c, int startIndex, int count)
		{
			return this.textSource.IndexOf(c, startIndex, count);
		}

		/// <inheritdoc />
		// Token: 0x0600009B RID: 155 RVA: 0x00003179 File Offset: 0x00002179
		public int IndexOfAny(char[] anyOf, int startIndex, int count)
		{
			return this.textSource.IndexOfAny(anyOf, startIndex, count);
		}

		/// <inheritdoc />
		// Token: 0x0600009C RID: 156 RVA: 0x00003189 File Offset: 0x00002189
		public int IndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.textSource.IndexOf(searchText, startIndex, count, comparisonType);
		}

		/// <inheritdoc />
		// Token: 0x0600009D RID: 157 RVA: 0x0000319B File Offset: 0x0000219B
		public int LastIndexOf(char c, int startIndex, int count)
		{
			return this.textSource.LastIndexOf(c, startIndex, count);
		}

		/// <inheritdoc />
		// Token: 0x0600009E RID: 158 RVA: 0x000031AB File Offset: 0x000021AB
		public int LastIndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.textSource.LastIndexOf(searchText, startIndex, count, comparisonType);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000031BD File Offset: 0x000021BD
		object IServiceProvider.GetService(Type serviceType)
		{
			return null;
		}

		/// <inheritdoc />
		/// <remarks>Will never be raised on <see cref="T:ICSharpCode.NRefactory.Editor.ReadOnlyDocument" />.</remarks>
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060000A0 RID: 160 RVA: 0x000031C0 File Offset: 0x000021C0
		// (remove) Token: 0x060000A1 RID: 161 RVA: 0x000031C2 File Offset: 0x000021C2
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
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000031C4 File Offset: 0x000021C4
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x04000016 RID: 22
		private readonly ITextSource textSource;

		// Token: 0x04000017 RID: 23
		private readonly string fileName;

		// Token: 0x04000018 RID: 24
		private int[] lines;

		// Token: 0x04000019 RID: 25
		private static readonly char[] newline = new char[]
		{
			'\r',
			'\n'
		};

		// Token: 0x02000015 RID: 21
		private sealed class ReadOnlyDocumentLine : IDocumentLine, ISegment
		{
			// Token: 0x060000A4 RID: 164 RVA: 0x000031F0 File Offset: 0x000021F0
			public ReadOnlyDocumentLine(ReadOnlyDocument doc, int lineNumber)
			{
				this.doc = doc;
				this.lineNumber = lineNumber;
				this.offset = doc.GetStartOffset(lineNumber);
				this.endOffset = doc.GetEndOffset(lineNumber);
			}

			// Token: 0x060000A5 RID: 165 RVA: 0x00003220 File Offset: 0x00002220
			public override int GetHashCode()
			{
				return this.doc.GetHashCode() ^ this.lineNumber;
			}

			// Token: 0x060000A6 RID: 166 RVA: 0x00003234 File Offset: 0x00002234
			public override bool Equals(object obj)
			{
				ReadOnlyDocument.ReadOnlyDocumentLine readOnlyDocumentLine = obj as ReadOnlyDocument.ReadOnlyDocumentLine;
				return readOnlyDocumentLine != null && this.doc == readOnlyDocumentLine.doc && this.lineNumber == readOnlyDocumentLine.lineNumber;
			}

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003269 File Offset: 0x00002269
			public int Offset
			{
				get
				{
					return this.offset;
				}
			}

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003271 File Offset: 0x00002271
			public int Length
			{
				get
				{
					return this.endOffset - this.offset;
				}
			}

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003280 File Offset: 0x00002280
			public int EndOffset
			{
				get
				{
					return this.endOffset;
				}
			}

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x060000AA RID: 170 RVA: 0x00003288 File Offset: 0x00002288
			public int TotalLength
			{
				get
				{
					return this.doc.GetTotalEndOffset(this.lineNumber) - this.offset;
				}
			}

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x060000AB RID: 171 RVA: 0x000032A2 File Offset: 0x000022A2
			public int DelimiterLength
			{
				get
				{
					return this.doc.GetTotalEndOffset(this.lineNumber) - this.endOffset;
				}
			}

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x060000AC RID: 172 RVA: 0x000032BC File Offset: 0x000022BC
			public int LineNumber
			{
				get
				{
					return this.lineNumber;
				}
			}

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x060000AD RID: 173 RVA: 0x000032C4 File Offset: 0x000022C4
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

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x060000AE RID: 174 RVA: 0x000032E4 File Offset: 0x000022E4
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

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x060000AF RID: 175 RVA: 0x0000330E File Offset: 0x0000230E
			public bool IsDeleted
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0400001A RID: 26
			private readonly ReadOnlyDocument doc;

			// Token: 0x0400001B RID: 27
			private readonly int lineNumber;

			// Token: 0x0400001C RID: 28
			private readonly int offset;

			// Token: 0x0400001D RID: 29
			private readonly int endOffset;
		}

		// Token: 0x02000016 RID: 22
		private sealed class ReadOnlyDocumentTextAnchor : ITextAnchor
		{
			// Token: 0x060000B0 RID: 176 RVA: 0x00003311 File Offset: 0x00002311
			public ReadOnlyDocumentTextAnchor(TextLocation location, int offset)
			{
				this.location = location;
				this.offset = offset;
			}

			// Token: 0x1400000B RID: 11
			// (add) Token: 0x060000B1 RID: 177 RVA: 0x00003327 File Offset: 0x00002327
			// (remove) Token: 0x060000B2 RID: 178 RVA: 0x00003329 File Offset: 0x00002329
			public event EventHandler Deleted
			{
				add
				{
				}
				remove
				{
				}
			}

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000332B File Offset: 0x0000232B
			public TextLocation Location
			{
				get
				{
					return this.location;
				}
			}

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x060000B4 RID: 180 RVA: 0x00003333 File Offset: 0x00002333
			public int Offset
			{
				get
				{
					return this.offset;
				}
			}

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000333B File Offset: 0x0000233B
			// (set) Token: 0x060000B6 RID: 182 RVA: 0x00003343 File Offset: 0x00002343
			public AnchorMovementType MovementType { get; set; }

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000334C File Offset: 0x0000234C
			// (set) Token: 0x060000B8 RID: 184 RVA: 0x00003354 File Offset: 0x00002354
			public bool SurviveDeletion { get; set; }

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000335D File Offset: 0x0000235D
			public bool IsDeleted
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x060000BA RID: 186 RVA: 0x00003360 File Offset: 0x00002360
			public int Line
			{
				get
				{
					return this.location.Line;
				}
			}

			// Token: 0x17000031 RID: 49
			// (get) Token: 0x060000BB RID: 187 RVA: 0x0000337C File Offset: 0x0000237C
			public int Column
			{
				get
				{
					return this.location.Column;
				}
			}

			// Token: 0x0400001E RID: 30
			private readonly TextLocation location;

			// Token: 0x0400001F RID: 31
			private readonly int offset;
		}
	}
}
