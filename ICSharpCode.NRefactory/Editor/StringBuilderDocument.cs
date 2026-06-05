using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Document based on a string builder.
	/// This class serves as a reference implementation for the IDocument interface.
	/// </summary>
	// Token: 0x02000017 RID: 23
	public class StringBuilderDocument : IDocument, ITextSource, IServiceProvider
	{
		/// <summary>
		/// Creates a new StringBuilderDocument.
		/// </summary>
		// Token: 0x060000BC RID: 188 RVA: 0x00003397 File Offset: 0x00002397
		public StringBuilderDocument()
		{
			this.b = new StringBuilder();
		}

		/// <summary>
		/// Creates a new StringBuilderDocument with the specified initial text.
		/// </summary>
		// Token: 0x060000BD RID: 189 RVA: 0x000033C0 File Offset: 0x000023C0
		public StringBuilderDocument(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			this.b = new StringBuilder(text);
		}

		/// <summary>
		/// Creates a new StringBuilderDocument with the initial text copied from the specified text source.
		/// </summary>
		// Token: 0x060000BE RID: 190 RVA: 0x000033F8 File Offset: 0x000023F8
		public StringBuilderDocument(ITextSource textSource)
		{
			if (textSource == null)
			{
				throw new ArgumentNullException("textSource");
			}
			this.b = new StringBuilder(textSource.TextLength);
			textSource.WriteTextTo(new StringWriter(this.b));
		}

		/// <inheritdoc />
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060000BF RID: 191 RVA: 0x00003454 File Offset: 0x00002454
		// (remove) Token: 0x060000C0 RID: 192 RVA: 0x0000348C File Offset: 0x0000248C
		public event EventHandler<TextChangeEventArgs> TextChanging;

		/// <inheritdoc />
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060000C1 RID: 193 RVA: 0x000034C4 File Offset: 0x000024C4
		// (remove) Token: 0x060000C2 RID: 194 RVA: 0x000034FC File Offset: 0x000024FC
		public event EventHandler<TextChangeEventArgs> TextChanged;

		/// <inheritdoc />
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060000C3 RID: 195 RVA: 0x00003534 File Offset: 0x00002534
		// (remove) Token: 0x060000C4 RID: 196 RVA: 0x0000356C File Offset: 0x0000256C
		public event EventHandler ChangeCompleted;

		/// <inheritdoc />
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000035A1 File Offset: 0x000025A1
		public ITextSourceVersion Version
		{
			get
			{
				return this.versionProvider.CurrentVersion;
			}
		}

		/// <inheritdoc />
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000035AE File Offset: 0x000025AE
		public int LineCount
		{
			get
			{
				return this.CreateDocumentSnapshot().LineCount;
			}
		}

		/// <inheritdoc />
		// Token: 0x060000C7 RID: 199 RVA: 0x000035BB File Offset: 0x000025BB
		public IDocumentLine GetLineByNumber(int lineNumber)
		{
			return this.CreateDocumentSnapshot().GetLineByNumber(lineNumber);
		}

		/// <inheritdoc />
		// Token: 0x060000C8 RID: 200 RVA: 0x000035C9 File Offset: 0x000025C9
		public IDocumentLine GetLineByOffset(int offset)
		{
			return this.CreateDocumentSnapshot().GetLineByOffset(offset);
		}

		/// <inheritdoc />
		// Token: 0x060000C9 RID: 201 RVA: 0x000035D7 File Offset: 0x000025D7
		public int GetOffset(int line, int column)
		{
			return this.CreateDocumentSnapshot().GetOffset(line, column);
		}

		/// <inheritdoc />
		// Token: 0x060000CA RID: 202 RVA: 0x000035E6 File Offset: 0x000025E6
		public int GetOffset(TextLocation location)
		{
			return this.CreateDocumentSnapshot().GetOffset(location);
		}

		/// <inheritdoc />
		// Token: 0x060000CB RID: 203 RVA: 0x000035F4 File Offset: 0x000025F4
		public TextLocation GetLocation(int offset)
		{
			return this.CreateDocumentSnapshot().GetLocation(offset);
		}

		/// <inheritdoc />
		// Token: 0x060000CC RID: 204 RVA: 0x00003602 File Offset: 0x00002602
		public void Insert(int offset, string text)
		{
			this.Replace(offset, 0, text);
		}

		/// <inheritdoc />
		// Token: 0x060000CD RID: 205 RVA: 0x0000360D File Offset: 0x0000260D
		public void Insert(int offset, ITextSource text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			this.Replace(offset, 0, text.Text);
		}

		/// <inheritdoc />
		// Token: 0x060000CE RID: 206 RVA: 0x0000362C File Offset: 0x0000262C
		public void Insert(int offset, string text, AnchorMovementType defaultAnchorMovementType)
		{
			if (offset < 0 || offset > this.TextLength)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (defaultAnchorMovementType == AnchorMovementType.BeforeInsertion)
			{
				this.PerformChange(new StringBuilderDocument.InsertionWithMovementBefore(offset, text));
				return;
			}
			this.Replace(offset, 0, text);
		}

		/// <inheritdoc />
		// Token: 0x060000CF RID: 207 RVA: 0x0000367A File Offset: 0x0000267A
		public void Insert(int offset, ITextSource text, AnchorMovementType defaultAnchorMovementType)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			this.Insert(offset, text.Text, defaultAnchorMovementType);
		}

		/// <inheritdoc />
		// Token: 0x060000D0 RID: 208 RVA: 0x00003698 File Offset: 0x00002698
		public void Remove(int offset, int length)
		{
			this.Replace(offset, length, string.Empty);
		}

		/// <inheritdoc />
		// Token: 0x060000D1 RID: 209 RVA: 0x000036A8 File Offset: 0x000026A8
		public void Replace(int offset, int length, string newText)
		{
			if (offset < 0 || offset > this.TextLength)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (length < 0 || length > this.TextLength - offset)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			if (newText == null)
			{
				throw new ArgumentNullException("newText");
			}
			this.PerformChange(new TextChangeEventArgs(offset, this.b.ToString(offset, length), newText));
		}

		/// <inheritdoc />
		// Token: 0x060000D2 RID: 210 RVA: 0x0000370F File Offset: 0x0000270F
		public void Replace(int offset, int length, ITextSource newText)
		{
			if (newText == null)
			{
				throw new ArgumentNullException("newText");
			}
			this.Replace(offset, length, newText.Text);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003730 File Offset: 0x00002730
		private void PerformChange(TextChangeEventArgs change)
		{
			this.StartUndoableAction();
			try
			{
				this.isInChange = true;
				try
				{
					if (this.TextChanging != null)
					{
						this.TextChanging(this, change);
					}
					this.documentSnapshot = null;
					this.cachedText = null;
					this.b.Remove(change.Offset, change.RemovalLength);
					this.b.Insert(change.Offset, change.InsertedText.Text);
					this.versionProvider.AppendChange(change);
					this.UpdateAnchors(change);
					if (this.TextChanged != null)
					{
						this.TextChanged(this, change);
					}
				}
				finally
				{
					this.isInChange = false;
				}
			}
			finally
			{
				this.EndUndoableAction();
			}
		}

		/// <inheritdoc />
		// Token: 0x060000D4 RID: 212 RVA: 0x000037FC File Offset: 0x000027FC
		public void StartUndoableAction()
		{
			if (this.isInChange)
			{
				throw new InvalidOperationException();
			}
			this.undoGroupNesting++;
		}

		/// <inheritdoc />
		// Token: 0x060000D5 RID: 213 RVA: 0x0000381A File Offset: 0x0000281A
		public void EndUndoableAction()
		{
			this.undoGroupNesting--;
			if (this.undoGroupNesting == 0 && this.ChangeCompleted != null)
			{
				this.ChangeCompleted(this, EventArgs.Empty);
			}
		}

		/// <inheritdoc />
		// Token: 0x060000D6 RID: 214 RVA: 0x0000384B File Offset: 0x0000284B
		public IDisposable OpenUndoGroup()
		{
			this.StartUndoableAction();
			return new CallbackOnDispose(new Action(this.EndUndoableAction));
		}

		/// <inheritdoc />
		// Token: 0x060000D7 RID: 215 RVA: 0x00003864 File Offset: 0x00002864
		public IDocument CreateDocumentSnapshot()
		{
			if (this.documentSnapshot == null)
			{
				this.documentSnapshot = new ReadOnlyDocument(this, this.FileName);
			}
			return this.documentSnapshot;
		}

		/// <inheritdoc />
		// Token: 0x060000D8 RID: 216 RVA: 0x00003886 File Offset: 0x00002886
		public ITextSource CreateSnapshot()
		{
			return new StringTextSource(this.Text, this.versionProvider.CurrentVersion);
		}

		/// <inheritdoc />
		// Token: 0x060000D9 RID: 217 RVA: 0x0000389E File Offset: 0x0000289E
		public ITextSource CreateSnapshot(int offset, int length)
		{
			return new StringTextSource(this.GetText(offset, length));
		}

		/// <inheritdoc />
		// Token: 0x060000DA RID: 218 RVA: 0x000038AD File Offset: 0x000028AD
		public TextReader CreateReader()
		{
			return new StringReader(this.Text);
		}

		/// <inheritdoc />
		// Token: 0x060000DB RID: 219 RVA: 0x000038BA File Offset: 0x000028BA
		public TextReader CreateReader(int offset, int length)
		{
			return new StringReader(this.GetText(offset, length));
		}

		/// <inheritdoc />
		// Token: 0x060000DC RID: 220 RVA: 0x000038C9 File Offset: 0x000028C9
		public void WriteTextTo(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.Write(this.Text);
		}

		/// <inheritdoc />
		// Token: 0x060000DD RID: 221 RVA: 0x000038E5 File Offset: 0x000028E5
		public void WriteTextTo(TextWriter writer, int offset, int length)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.Write(this.GetText(offset, length));
		}

		/// <inheritdoc />
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00003903 File Offset: 0x00002903
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00003924 File Offset: 0x00002924
		public string Text
		{
			get
			{
				if (this.cachedText == null)
				{
					this.cachedText = this.b.ToString();
				}
				return this.cachedText;
			}
			set
			{
				this.Replace(0, this.b.Length, value);
			}
		}

		/// <inheritdoc />
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00003939 File Offset: 0x00002939
		public int TextLength
		{
			get
			{
				return this.b.Length;
			}
		}

		/// <inheritdoc />
		// Token: 0x060000E1 RID: 225 RVA: 0x00003946 File Offset: 0x00002946
		public char GetCharAt(int offset)
		{
			return this.b[offset];
		}

		/// <inheritdoc />
		// Token: 0x060000E2 RID: 226 RVA: 0x00003954 File Offset: 0x00002954
		public string GetText(int offset, int length)
		{
			return this.b.ToString(offset, length);
		}

		/// <inheritdoc />
		// Token: 0x060000E3 RID: 227 RVA: 0x00003963 File Offset: 0x00002963
		public string GetText(ISegment segment)
		{
			if (segment == null)
			{
				throw new ArgumentNullException("segment");
			}
			return this.b.ToString(segment.Offset, segment.Length);
		}

		/// <inheritdoc />
		// Token: 0x060000E4 RID: 228 RVA: 0x0000398A File Offset: 0x0000298A
		public int IndexOf(char c, int startIndex, int count)
		{
			return this.Text.IndexOf(c, startIndex, count);
		}

		/// <inheritdoc />
		// Token: 0x060000E5 RID: 229 RVA: 0x0000399A File Offset: 0x0000299A
		public int IndexOfAny(char[] anyOf, int startIndex, int count)
		{
			return this.Text.IndexOfAny(anyOf, startIndex, count);
		}

		/// <inheritdoc />
		// Token: 0x060000E6 RID: 230 RVA: 0x000039AA File Offset: 0x000029AA
		public int IndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.Text.IndexOf(searchText, startIndex, count, comparisonType);
		}

		/// <inheritdoc />
		// Token: 0x060000E7 RID: 231 RVA: 0x000039BC File Offset: 0x000029BC
		public int LastIndexOf(char c, int startIndex, int count)
		{
			return this.Text.LastIndexOf(c, startIndex + count - 1, count);
		}

		/// <inheritdoc />
		// Token: 0x060000E8 RID: 232 RVA: 0x000039D0 File Offset: 0x000029D0
		public int LastIndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.Text.LastIndexOf(searchText, startIndex + count - 1, count, comparisonType);
		}

		/// <inheritdoc />
		// Token: 0x060000E9 RID: 233 RVA: 0x000039E8 File Offset: 0x000029E8
		public ITextAnchor CreateAnchor(int offset)
		{
			StringBuilderDocument.SimpleAnchor simpleAnchor = new StringBuilderDocument.SimpleAnchor(this, offset);
			for (int i = 0; i < this.anchors.Count; i++)
			{
				if (!this.anchors[i].IsAlive)
				{
					this.anchors[i] = new WeakReference(simpleAnchor);
				}
			}
			this.anchors.Add(new WeakReference(simpleAnchor));
			return simpleAnchor;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00003A4C File Offset: 0x00002A4C
		private void UpdateAnchors(TextChangeEventArgs change)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.anchors.Count; i++)
			{
				StringBuilderDocument.SimpleAnchor simpleAnchor = this.anchors[i].Target as StringBuilderDocument.SimpleAnchor;
				if (simpleAnchor != null)
				{
					simpleAnchor.Update(change);
					if (simpleAnchor.IsDeleted)
					{
						list.Add(i);
					}
				}
			}
			list.Reverse();
			foreach (int index in list)
			{
				StringBuilderDocument.SimpleAnchor simpleAnchor2 = this.anchors[index].Target as StringBuilderDocument.SimpleAnchor;
				if (simpleAnchor2 != null)
				{
					simpleAnchor2.RaiseDeletedEvent();
				}
				this.anchors.RemoveAt(index);
			}
		}

		/// <inheritdoc />
		// Token: 0x060000EB RID: 235 RVA: 0x00003B18 File Offset: 0x00002B18
		public virtual object GetService(Type serviceType)
		{
			return null;
		}

		/// <inheritdoc />
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060000EC RID: 236 RVA: 0x00003B1B File Offset: 0x00002B1B
		// (remove) Token: 0x060000ED RID: 237 RVA: 0x00003B1D File Offset: 0x00002B1D
		public virtual event EventHandler FileNameChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <inheritdoc />
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00003B1F File Offset: 0x00002B1F
		public virtual string FileName
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x04000022 RID: 34
		private readonly StringBuilder b;

		// Token: 0x04000023 RID: 35
		private readonly TextSourceVersionProvider versionProvider = new TextSourceVersionProvider();

		// Token: 0x04000027 RID: 39
		private bool isInChange;

		// Token: 0x04000028 RID: 40
		private int undoGroupNesting;

		// Token: 0x04000029 RID: 41
		private ReadOnlyDocument documentSnapshot;

		// Token: 0x0400002A RID: 42
		private string cachedText;

		// Token: 0x0400002B RID: 43
		private readonly List<WeakReference> anchors = new List<WeakReference>();

		// Token: 0x02000019 RID: 25
		[Serializable]
		private sealed class InsertionWithMovementBefore : TextChangeEventArgs
		{
			// Token: 0x060000F8 RID: 248 RVA: 0x00003C7E File Offset: 0x00002C7E
			public InsertionWithMovementBefore(int offset, string newText) : base(offset, string.Empty, newText)
			{
			}

			// Token: 0x060000F9 RID: 249 RVA: 0x00003C8D File Offset: 0x00002C8D
			public override int GetNewOffset(int offset, AnchorMovementType movementType)
			{
				if (offset == base.Offset && movementType == AnchorMovementType.Default)
				{
					return offset;
				}
				return base.GetNewOffset(offset, movementType);
			}
		}

		// Token: 0x0200001A RID: 26
		private sealed class SimpleAnchor : ITextAnchor
		{
			// Token: 0x060000FA RID: 250 RVA: 0x00003CA5 File Offset: 0x00002CA5
			public SimpleAnchor(StringBuilderDocument document, int offset)
			{
				this.document = document;
				this.offset = offset;
			}

			// Token: 0x14000010 RID: 16
			// (add) Token: 0x060000FB RID: 251 RVA: 0x00003CBC File Offset: 0x00002CBC
			// (remove) Token: 0x060000FC RID: 252 RVA: 0x00003CF4 File Offset: 0x00002CF4
			public event EventHandler Deleted;

			// Token: 0x1700003C RID: 60
			// (get) Token: 0x060000FD RID: 253 RVA: 0x00003D29 File Offset: 0x00002D29
			public TextLocation Location
			{
				get
				{
					if (this.IsDeleted)
					{
						throw new InvalidOperationException();
					}
					return this.document.GetLocation(this.offset);
				}
			}

			// Token: 0x1700003D RID: 61
			// (get) Token: 0x060000FE RID: 254 RVA: 0x00003D4A File Offset: 0x00002D4A
			public int Offset
			{
				get
				{
					if (this.IsDeleted)
					{
						throw new InvalidOperationException();
					}
					return this.offset;
				}
			}

			// Token: 0x1700003E RID: 62
			// (get) Token: 0x060000FF RID: 255 RVA: 0x00003D60 File Offset: 0x00002D60
			// (set) Token: 0x06000100 RID: 256 RVA: 0x00003D68 File Offset: 0x00002D68
			public AnchorMovementType MovementType { get; set; }

			// Token: 0x1700003F RID: 63
			// (get) Token: 0x06000101 RID: 257 RVA: 0x00003D71 File Offset: 0x00002D71
			// (set) Token: 0x06000102 RID: 258 RVA: 0x00003D79 File Offset: 0x00002D79
			public bool SurviveDeletion { get; set; }

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x06000103 RID: 259 RVA: 0x00003D82 File Offset: 0x00002D82
			public bool IsDeleted
			{
				get
				{
					return this.offset < 0;
				}
			}

			// Token: 0x06000104 RID: 260 RVA: 0x00003D90 File Offset: 0x00002D90
			public void Update(TextChangeEventArgs change)
			{
				if (this.SurviveDeletion || this.offset <= change.Offset || this.offset >= change.Offset + change.RemovalLength)
				{
					this.offset = change.GetNewOffset(this.offset, this.MovementType);
					return;
				}
				this.offset = -1;
			}

			// Token: 0x06000105 RID: 261 RVA: 0x00003DE8 File Offset: 0x00002DE8
			public void RaiseDeletedEvent()
			{
				if (this.Deleted != null)
				{
					this.Deleted(this, EventArgs.Empty);
				}
			}

			// Token: 0x17000041 RID: 65
			// (get) Token: 0x06000106 RID: 262 RVA: 0x00003E04 File Offset: 0x00002E04
			public int Line
			{
				get
				{
					return this.Location.Line;
				}
			}

			// Token: 0x17000042 RID: 66
			// (get) Token: 0x06000107 RID: 263 RVA: 0x00003E20 File Offset: 0x00002E20
			public int Column
			{
				get
				{
					return this.Location.Column;
				}
			}

			// Token: 0x0400002F RID: 47
			private readonly StringBuilderDocument document;

			// Token: 0x04000030 RID: 48
			private int offset;
		}
	}
}
