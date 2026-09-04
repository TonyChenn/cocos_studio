using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Represents a version identifier for a text source.
	/// </summary>
	/// <remarks>
	/// Verions can be used to efficiently detect whether a document has changed and needs reparsing;
	/// or even to implement incremental parsers.
	/// It is a separate class from ITextSource to allow the GC to collect the text source while
	/// the version checkpoint is still in use.
	/// </remarks>
	// Token: 0x02000013 RID: 19
	public interface ITextSourceVersion
	{
		/// <summary>
		/// Gets whether this checkpoint belongs to the same document as the other checkpoint.
		/// </summary>
		/// <remarks>
		/// Returns false when given <c>null</c>.
		/// </remarks>
		// Token: 0x0600006A RID: 106
		bool BelongsToSameDocumentAs(ITextSourceVersion other);

		/// <summary>
		/// Compares the age of this checkpoint to the other checkpoint.
		/// </summary>
		/// <remarks>This method is thread-safe.</remarks>
		/// <exception cref="T:System.ArgumentException">Raised if 'other' belongs to a different document than this version.</exception>
		/// <returns>-1 if this version is older than <paramref name="other" />.
		/// 0 if <c>this</c> version instance represents the same version as <paramref name="other" />.
		/// 1 if this version is newer than <paramref name="other" />.</returns>
		// Token: 0x0600006B RID: 107
		int CompareAge(ITextSourceVersion other);

		/// <summary>
		/// Gets the changes from this checkpoint to the other checkpoint.
		/// If 'other' is older than this checkpoint, reverse changes are calculated.
		/// </summary>
		/// <remarks>This method is thread-safe.</remarks>
		/// <exception cref="T:System.ArgumentException">Raised if 'other' belongs to a different document than this checkpoint.</exception>
		// Token: 0x0600006C RID: 108
		IEnumerable<TextChangeEventArgs> GetChangesTo(ITextSourceVersion other);

		/// <summary>
		/// Calculates where the offset has moved in the other buffer version.
		/// </summary>
		/// <exception cref="T:System.ArgumentException">Raised if 'other' belongs to a different document than this checkpoint.</exception>
		// Token: 0x0600006D RID: 109
		int MoveOffsetTo(ITextSourceVersion other, int oldOffset, AnchorMovementType movement = AnchorMovementType.Default);
	}
}
