using System;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Defines how a text anchor moves.
	/// </summary>
	// Token: 0x02000012 RID: 18
	public enum AnchorMovementType
	{
		/// <summary>
		/// When text is inserted at the anchor position, the type of the insertion
		/// determines where the caret moves to. For normal insertions, the anchor will move
		/// after the inserted text.
		/// </summary>
		// Token: 0x04000013 RID: 19
		Default,
		/// <summary>
		/// Behaves like a start marker - when text is inserted at the anchor position, the anchor will stay
		/// before the inserted text.
		/// </summary>
		// Token: 0x04000014 RID: 20
		BeforeInsertion,
		/// <summary>
		/// Behave like an end marker - when text is insered at the anchor position, the anchor will move
		/// after the inserted text.
		/// </summary>
		// Token: 0x04000015 RID: 21
		AfterInsertion
	}
}
