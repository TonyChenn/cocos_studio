using System;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// A line inside a <see cref="T:ICSharpCode.NRefactory.Editor.IDocument" />.
	/// </summary>
	public interface IDocumentLine : ISegment
	{
		/// <summary>
		/// Gets the length of this line, including the line delimiter.
		/// </summary>
		int TotalLength { get; }

		/// <summary>
		/// Gets the length of the line terminator.
		/// Returns 1 or 2; or 0 at the end of the document.
		/// </summary>
		int DelimiterLength { get; }

		/// <summary>
		/// Gets the number of this line.
		/// The first line has the number 1.
		/// </summary>
		int LineNumber { get; }

		/// <summary>
		/// Gets the previous line. Returns null if this is the first line in the document.
		/// </summary>
		IDocumentLine PreviousLine { get; }

		/// <summary>
		/// Gets the next line. Returns null if this is the last line in the document.
		/// </summary>
		IDocumentLine NextLine { get; }

		/// <summary>
		/// Gets whether the line was deleted.
		/// </summary>
		bool IsDeleted { get; }
	}
}
