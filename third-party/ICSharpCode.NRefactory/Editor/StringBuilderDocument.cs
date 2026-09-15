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
	public class StringBuilderDocument : IDocument, ITextSource, IServiceProvider
	{
		/// <summary>
		/// Creates a new StringBuilderDocument.
		/// </summary>
		public StringBuilderDocument()
		{
			this.b = new StringBuilder();
		}

		/// <summary>
		/// Creates a new StringBuilderDocument with the specified initial text.
		/// </summary>
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
		public event EventHandler<TextChangeEventArgs> TextChanging;

		/// <inheritdoc />
		public event EventHandler<TextChangeEventArgs> TextChanged;

		/// <inheritdoc />
		public event EventHandler ChangeCompleted;

		/// <inheritdoc />
		public ITextSourceVersion Version
		{
			get
			{
				return this.versionProvider.CurrentVersion;
			}
		}

		/// <inheritdoc />
		public int LineCount
		{
			get
			{
				return this.CreateDocumentSnapshot().LineCount;
			}
		}

		/// <inheritdoc />
		public IDocumentLine GetLineByNumber(int lineNumber)
		{
			return this.CreateDocumentSnapshot().GetLineByNumber(lineNumber);
		}

		/// <inheritdoc />
		public IDocumentLine GetLineByOffset(int offset)
		{
			return this.CreateDocumentSnapshot().GetLineByOffset(offset);
		}

		/// <inheritdoc />
		public int GetOffset(int line, int column)
		{
			return this.CreateDocumentSnapshot().GetOffset(line, column);
		}

		/// <inheritdoc />
		public int GetOffset(TextLocation location)
		{
			return this.CreateDocumentSnapshot().GetOffset(location);
		}

		/// <inheritdoc />
		public TextLocation GetLocation(int offset)
		{
			return this.CreateDocumentSnapshot().GetLocation(offset);
		}

		/// <inheritdoc />
		public void Insert(int offset, string text)
		{
			this.Replace(offset, 0, text);
		}

		/// <inheritdoc />
		public void Insert(int offset, ITextSource text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			this.Replace(offset, 0, text.Text);
		}

		/// <inheritdoc />
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
		public void Insert(int offset, ITextSource text, AnchorMovementType defaultAnchorMovementType)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			this.Insert(offset, text.Text, defaultAnchorMovementType);
		}

		/// <inheritdoc />
		public void Remove(int offset, int length)
		{
			this.Replace(offset, length, string.Empty);
		}

		/// <inheritdoc />
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
		public void Replace(int offset, int length, ITextSource newText)
		{
			if (newText == null)
			{
				throw new ArgumentNullException("newText");
			}
			this.Replace(offset, length, newText.Text);
		}

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
		public void StartUndoableAction()
		{
			if (this.isInChange)
			{
				throw new InvalidOperationException();
			}
			this.undoGroupNesting++;
		}

		/// <inheritdoc />
		public void EndUndoableAction()
		{
			this.undoGroupNesting--;
			if (this.undoGroupNesting == 0 && this.ChangeCompleted != null)
			{
				this.ChangeCompleted(this, EventArgs.Empty);
			}
		}

		/// <inheritdoc />
		public IDisposable OpenUndoGroup()
		{
			this.StartUndoableAction();
			return new CallbackOnDispose(new Action(this.EndUndoableAction));
		}

		/// <inheritdoc />
		public IDocument CreateDocumentSnapshot()
		{
			if (this.documentSnapshot == null)
			{
				this.documentSnapshot = new ReadOnlyDocument(this, this.FileName);
			}
			return this.documentSnapshot;
		}

		/// <inheritdoc />
		public ITextSource CreateSnapshot()
		{
			return new StringTextSource(this.Text, this.versionProvider.CurrentVersion);
		}

		/// <inheritdoc />
		public ITextSource CreateSnapshot(int offset, int length)
		{
			return new StringTextSource(this.GetText(offset, length));
		}

		/// <inheritdoc />
		public TextReader CreateReader()
		{
			return new StringReader(this.Text);
		}

		/// <inheritdoc />
		public TextReader CreateReader(int offset, int length)
		{
			return new StringReader(this.GetText(offset, length));
		}

		/// <inheritdoc />
		public void WriteTextTo(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.Write(this.Text);
		}

		/// <inheritdoc />
		public void WriteTextTo(TextWriter writer, int offset, int length)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.Write(this.GetText(offset, length));
		}

		/// <inheritdoc />
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
		public int TextLength
		{
			get
			{
				return this.b.Length;
			}
		}

		/// <inheritdoc />
		public char GetCharAt(int offset)
		{
			return this.b[offset];
		}

		/// <inheritdoc />
		public string GetText(int offset, int length)
		{
			return this.b.ToString(offset, length);
		}

		/// <inheritdoc />
		public string GetText(ISegment segment)
		{
			if (segment == null)
			{
				throw new ArgumentNullException("segment");
			}
			return this.b.ToString(segment.Offset, segment.Length);
		}

		/// <inheritdoc />
		public int IndexOf(char c, int startIndex, int count)
		{
			return this.Text.IndexOf(c, startIndex, count);
		}

		/// <inheritdoc />
		public int IndexOfAny(char[] anyOf, int startIndex, int count)
		{
			return this.Text.IndexOfAny(anyOf, startIndex, count);
		}

		/// <inheritdoc />
		public int IndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.Text.IndexOf(searchText, startIndex, count, comparisonType);
		}

		/// <inheritdoc />
		public int LastIndexOf(char c, int startIndex, int count)
		{
			return this.Text.LastIndexOf(c, startIndex + count - 1, count);
		}

		/// <inheritdoc />
		public int LastIndexOf(string searchText, int startIndex, int count, StringComparison comparisonType)
		{
			return this.Text.LastIndexOf(searchText, startIndex + count - 1, count, comparisonType);
		}

		/// <inheritdoc />
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
		public virtual object GetService(Type serviceType)
		{
			return null;
		}

		/// <inheritdoc />
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
		public virtual string FileName
		{
			get
			{
				return string.Empty;
			}
		}

		private readonly StringBuilder b;

		private readonly TextSourceVersionProvider versionProvider = new TextSourceVersionProvider();

		private bool isInChange;

		private int undoGroupNesting;

		private ReadOnlyDocument documentSnapshot;

		private string cachedText;

		private readonly List<WeakReference> anchors = new List<WeakReference>();

		[Serializable]
		private sealed class InsertionWithMovementBefore : TextChangeEventArgs
		{
			public InsertionWithMovementBefore(int offset, string newText) : base(offset, string.Empty, newText)
			{
			}

			public override int GetNewOffset(int offset, AnchorMovementType movementType)
			{
				if (offset == base.Offset && movementType == AnchorMovementType.Default)
				{
					return offset;
				}
				return base.GetNewOffset(offset, movementType);
			}
		}

		private sealed class SimpleAnchor : ITextAnchor
		{
			public SimpleAnchor(StringBuilderDocument document, int offset)
			{
				this.document = document;
				this.offset = offset;
			}

			public event EventHandler Deleted;

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

			public AnchorMovementType MovementType { get; set; }

			public bool SurviveDeletion { get; set; }

			public bool IsDeleted
			{
				get
				{
					return this.offset < 0;
				}
			}

			public void Update(TextChangeEventArgs change)
			{
				if (this.SurviveDeletion || this.offset <= change.Offset || this.offset >= change.Offset + change.RemovalLength)
				{
					this.offset = change.GetNewOffset(this.offset, this.MovementType);
					return;
				}
				this.offset = -1;
			}

			public void RaiseDeletedEvent()
			{
				if (this.Deleted != null)
				{
					this.Deleted(this, EventArgs.Empty);
				}
			}

			public int Line
			{
				get
				{
					return this.Location.Line;
				}
			}

			public int Column
			{
				get
				{
					return this.Location.Column;
				}
			}

			private readonly StringBuilderDocument document;

			private int offset;
		}
	}
}
