using System;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// A document representing a source code file for refactoring.
	/// Line and column counting starts at 1.
	/// Offset counting starts at 0.
	/// </summary>
	// Token: 0x0200000D RID: 13
	public interface IDocument : ITextSource, IServiceProvider
	{
		/// <summary>
		/// Creates an immutable snapshot of this document.
		/// </summary>
		// Token: 0x06000037 RID: 55
		IDocument CreateDocumentSnapshot();

		/// <summary>
		/// Gets/Sets the text of the whole document..
		/// </summary>
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000038 RID: 56
		// (set) Token: 0x06000039 RID: 57
		string Text { get; set; }

		/// <summary>
		/// This event is called directly before a change is applied to the document.
		/// </summary>
		/// <remarks>
		/// It is invalid to modify the document within this event handler.
		/// Aborting the change (by throwing an exception) is likely to cause corruption of data structures
		/// that listen to the Changing and Changed events.
		/// </remarks>
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600003A RID: 58
		// (remove) Token: 0x0600003B RID: 59
		event EventHandler<TextChangeEventArgs> TextChanging;

		/// <summary>
		/// This event is called directly after a change is applied to the document.
		/// </summary>
		/// <remarks>
		/// It is invalid to modify the document within this event handler.
		/// Aborting the event handler (by throwing an exception) is likely to cause corruption of data structures
		/// that listen to the Changing and Changed events.
		/// </remarks>
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600003C RID: 60
		// (remove) Token: 0x0600003D RID: 61
		event EventHandler<TextChangeEventArgs> TextChanged;

		/// <summary>
		/// This event is called after a group of changes is completed.
		/// </summary>
		/// <seealso cref="M:ICSharpCode.NRefactory.Editor.IDocument.EndUndoableAction" />
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600003E RID: 62
		// (remove) Token: 0x0600003F RID: 63
		event EventHandler ChangeCompleted;

		/// <summary>
		/// Gets the number of lines in the document.
		/// </summary>
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000040 RID: 64
		int LineCount { get; }

		/// <summary>
		/// Gets the document line with the specified number.
		/// </summary>
		/// <param name="lineNumber">The number of the line to retrieve. The first line has number 1.</param>
		// Token: 0x06000041 RID: 65
		IDocumentLine GetLineByNumber(int lineNumber);

		/// <summary>
		/// Gets the document line that contains the specified offset.
		/// </summary>
		// Token: 0x06000042 RID: 66
		IDocumentLine GetLineByOffset(int offset);

		/// <summary>
		/// Gets the offset from a text location.
		/// </summary>
		/// <seealso cref="M:ICSharpCode.NRefactory.Editor.IDocument.GetLocation(System.Int32)" />
		// Token: 0x06000043 RID: 67
		int GetOffset(int line, int column);

		/// <summary>
		/// Gets the offset from a text location.
		/// </summary>
		/// <seealso cref="M:ICSharpCode.NRefactory.Editor.IDocument.GetLocation(System.Int32)" />
		// Token: 0x06000044 RID: 68
		int GetOffset(TextLocation location);

		/// <summary>
		/// Gets the location from an offset.
		/// </summary>
		/// <seealso cref="M:ICSharpCode.NRefactory.Editor.IDocument.GetOffset(ICSharpCode.NRefactory.TextLocation)" />
		// Token: 0x06000045 RID: 69
		TextLocation GetLocation(int offset);

		/// <summary>
		/// Inserts text.
		/// </summary>
		/// <param name="offset">The offset at which the text is inserted.</param>
		/// <param name="text">The new text.</param>
		/// <remarks>
		/// Anchors positioned exactly at the insertion offset will move according to their movement type.
		/// For AnchorMovementType.Default, they will move behind the inserted text.
		/// The caret will also move behind the inserted text.
		/// </remarks>
		// Token: 0x06000046 RID: 70
		void Insert(int offset, string text);

		/// <summary>
		/// Inserts text.
		/// </summary>
		/// <param name="offset">The offset at which the text is inserted.</param>
		/// <param name="text">The new text.</param>
		/// <remarks>
		/// Anchors positioned exactly at the insertion offset will move according to their movement type.
		/// For AnchorMovementType.Default, they will move behind the inserted text.
		/// The caret will also move behind the inserted text.
		/// </remarks>
		// Token: 0x06000047 RID: 71
		void Insert(int offset, ITextSource text);

		/// <summary>
		/// Inserts text.
		/// </summary>
		/// <param name="offset">The offset at which the text is inserted.</param>
		/// <param name="text">The new text.</param>
		/// <param name="defaultAnchorMovementType">
		/// Anchors positioned exactly at the insertion offset will move according to the anchor's movement type.
		/// For AnchorMovementType.Default, they will move according to the movement type specified by this parameter.
		/// The caret will also move according to the <paramref name="defaultAnchorMovementType" /> parameter.
		/// </param>
		// Token: 0x06000048 RID: 72
		void Insert(int offset, string text, AnchorMovementType defaultAnchorMovementType);

		/// <summary>
		/// Inserts text.
		/// </summary>
		/// <param name="offset">The offset at which the text is inserted.</param>
		/// <param name="text">The new text.</param>
		/// <param name="defaultAnchorMovementType">
		/// Anchors positioned exactly at the insertion offset will move according to the anchor's movement type.
		/// For AnchorMovementType.Default, they will move according to the movement type specified by this parameter.
		/// The caret will also move according to the <paramref name="defaultAnchorMovementType" /> parameter.
		/// </param>
		// Token: 0x06000049 RID: 73
		void Insert(int offset, ITextSource text, AnchorMovementType defaultAnchorMovementType);

		/// <summary>
		/// Removes text.
		/// </summary>
		/// <param name="offset">Starting offset of the text to be removed.</param>
		/// <param name="length">Length of the text to be removed.</param>
		// Token: 0x0600004A RID: 74
		void Remove(int offset, int length);

		/// <summary>
		/// Replaces text.
		/// </summary>
		/// <param name="offset">The starting offset of the text to be replaced.</param>
		/// <param name="length">The length of the text to be replaced.</param>
		/// <param name="newText">The new text.</param>
		// Token: 0x0600004B RID: 75
		void Replace(int offset, int length, string newText);

		/// <summary>
		/// Replaces text.
		/// </summary>
		/// <param name="offset">The starting offset of the text to be replaced.</param>
		/// <param name="length">The length of the text to be replaced.</param>
		/// <param name="newText">The new text.</param>
		// Token: 0x0600004C RID: 76
		void Replace(int offset, int length, ITextSource newText);

		/// <summary>
		/// Make the document combine the following actions into a single
		/// action for undo purposes.
		/// </summary>
		// Token: 0x0600004D RID: 77
		void StartUndoableAction();

		/// <summary>
		/// Ends the undoable action started with <see cref="M:ICSharpCode.NRefactory.Editor.IDocument.StartUndoableAction" />.
		/// </summary>
		// Token: 0x0600004E RID: 78
		void EndUndoableAction();

		/// <summary>
		/// Creates an undo group. Dispose the returned value to close the undo group.
		/// </summary>
		/// <returns>An object that closes the undo group when Dispose() is called.</returns>
		// Token: 0x0600004F RID: 79
		IDisposable OpenUndoGroup();

		/// <summary>
		/// Creates a new <see cref="T:ICSharpCode.NRefactory.Editor.ITextAnchor" /> at the specified offset.
		/// </summary>
		/// <inheritdoc cref="T:ICSharpCode.NRefactory.Editor.ITextAnchor" select="remarks|example" />
		// Token: 0x06000050 RID: 80
		ITextAnchor CreateAnchor(int offset);

		/// <summary>
		/// Gets the name of the file the document is stored in.
		/// Could also be a non-existent dummy file name or null if no name has been set.
		/// </summary>
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000051 RID: 81
		string FileName { get; }

		/// <summary>
		/// Fired when the file name of the document changes.
		/// </summary>
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000052 RID: 82
		// (remove) Token: 0x06000053 RID: 83
		event EventHandler FileNameChanged;
	}
}
