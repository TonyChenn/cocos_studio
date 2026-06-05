using System;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// The TextAnchor class references an offset (a position between two characters).
	/// It automatically updates the offset when text is inserted/removed in front of the anchor.
	/// </summary>
	/// <remarks>
	/// <para>Use the <see cref="P:ICSharpCode.NRefactory.Editor.ITextAnchor.Offset" /> property to get the offset from a text anchor.
	/// Use the <see cref="M:ICSharpCode.NRefactory.Editor.IDocument.CreateAnchor(System.Int32)" /> method to create an anchor from an offset.
	/// </para>
	/// <para>
	/// The document will automatically update all text anchors; and because it uses weak references to do so,
	/// the garbage collector can simply collect the anchor object when you don't need it anymore.
	/// </para>
	/// <para>Moreover, the document is able to efficiently update a large number of anchors without having to look
	/// at each anchor object individually. Updating the offsets of all anchors usually only takes time logarithmic
	/// to the number of anchors. Retrieving the <see cref="P:ICSharpCode.NRefactory.Editor.ITextAnchor.Offset" /> property also runs in O(lg N).</para>
	/// </remarks>
	/// <example>
	/// Usage:
	/// <code>TextAnchor anchor = document.CreateAnchor(offset);
	/// ChangeMyDocument();
	/// int newOffset = anchor.Offset;
	/// </code>
	/// </example>
	// Token: 0x02000011 RID: 17
	public interface ITextAnchor
	{
		/// <summary>
		/// Gets the text location of this anchor.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown when trying to get the Offset from a deleted anchor.</exception>
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005F RID: 95
		TextLocation Location { get; }

		/// <summary>
		/// Gets the offset of the text anchor.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown when trying to get the Offset from a deleted anchor.</exception>
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000060 RID: 96
		int Offset { get; }

		/// <summary>
		/// Controls how the anchor moves.
		/// </summary>
		/// <remarks>Anchor movement is ambiguous if text is inserted exactly at the anchor's location.
		/// Does the anchor stay before the inserted text, or does it move after it?
		/// The property <see cref="P:ICSharpCode.NRefactory.Editor.ITextAnchor.MovementType" /> will be used to determine which of these two options the anchor will choose.
		/// The default value is <see cref="F:ICSharpCode.NRefactory.Editor.AnchorMovementType.Default" />.</remarks>
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000061 RID: 97
		// (set) Token: 0x06000062 RID: 98
		AnchorMovementType MovementType { get; set; }

		/// <summary>
		/// <para>
		/// Specifies whether the anchor survives deletion of the text containing it.
		/// </para><para>
		/// <c>false</c>: The anchor is deleted when the a selection that includes the anchor is deleted.
		/// <c>true</c>: The anchor is not deleted.
		/// </para>
		/// </summary>
		/// <remarks><inheritdoc cref="P:ICSharpCode.NRefactory.Editor.ITextAnchor.IsDeleted" /></remarks>
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000063 RID: 99
		// (set) Token: 0x06000064 RID: 100
		bool SurviveDeletion { get; set; }

		/// <summary>
		/// Gets whether the anchor was deleted.
		/// </summary>
		/// <remarks>
		/// <para>When a piece of text containing an anchor is removed, then that anchor will be deleted.
		/// First, the <see cref="P:ICSharpCode.NRefactory.Editor.ITextAnchor.IsDeleted" /> property is set to true on all deleted anchors,
		/// then the <see cref="E:ICSharpCode.NRefactory.Editor.ITextAnchor.Deleted" /> events are raised.
		/// You cannot retrieve the offset from an anchor that has been deleted.</para>
		/// <para>This deletion behavior might be useful when using anchors for building a bookmark feature,
		/// but in other cases you want to still be able to use the anchor. For those cases, set <c><see cref="P:ICSharpCode.NRefactory.Editor.ITextAnchor.SurviveDeletion" /> = true</c>.</para>
		/// </remarks>
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000065 RID: 101
		bool IsDeleted { get; }

		/// <summary>
		/// Occurs after the anchor was deleted.
		/// </summary>
		/// <remarks>
		/// <inheritdoc cref="P:ICSharpCode.NRefactory.Editor.ITextAnchor.IsDeleted" />
		/// <para>Due to the 'weak reference' nature of text anchors, you will receive
		/// the Deleted event only while your code holds a reference to the TextAnchor object.
		/// </para>
		/// </remarks>
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000066 RID: 102
		// (remove) Token: 0x06000067 RID: 103
		event EventHandler Deleted;

		/// <summary>
		/// Gets the line number of the anchor.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown when trying to get the Offset from a deleted anchor.</exception>
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000068 RID: 104
		int Line { get; }

		/// <summary>
		/// Gets the column number of this anchor.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">Thrown when trying to get the Offset from a deleted anchor.</exception>
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000069 RID: 105
		int Column { get; }
	}
}
