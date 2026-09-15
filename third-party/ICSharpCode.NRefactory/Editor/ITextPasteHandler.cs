using System;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// The text paste handler can do formattings to a text that is about to be pasted
	/// into the text document.
	/// </summary>
	public interface ITextPasteHandler
	{
		/// <summary>
		/// Formats plain text that is inserted at a specified offset.
		/// </summary>
		/// <returns>
		/// The text that will get inserted at that position.
		/// </returns>
		/// <param name="offset">The offset where the text will be inserted.</param>
		/// <param name="text">The text to be inserted.</param>
		/// <param name="copyData">Additional data in case the text was copied from a Mono.TextEditor.</param>
		string FormatPlainText(int offset, string text, byte[] copyData);

		/// <summary>
		/// Gets the copy data for a specific segment inside the document. This can contain additional information.
		/// </summary>
		/// <param name="segment">The text segment that is about to be copied.</param>
		byte[] GetCopyData(ISegment segment);
	}
}
