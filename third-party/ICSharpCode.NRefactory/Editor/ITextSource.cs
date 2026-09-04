using System;
using System.IO;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// A read-only view on a (potentially mutable) text source.
	/// The IDocument interface derives from this interface.
	/// </summary>
	// Token: 0x0200000C RID: 12
	public interface ITextSource
	{
		/// <summary>
		/// Gets a version identifier for this text source.
		/// Returns null for unversioned text sources.
		/// </summary>
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000026 RID: 38
		ITextSourceVersion Version { get; }

		/// <summary>
		/// Creates an immutable snapshot of this text source.
		/// Unlike all other methods in this interface, this method is thread-safe.
		/// </summary>
		// Token: 0x06000027 RID: 39
		ITextSource CreateSnapshot();

		/// <summary>
		/// Creates an immutable snapshot of a part of this text source.
		/// Unlike all other methods in this interface, this method is thread-safe.
		/// </summary>
		// Token: 0x06000028 RID: 40
		ITextSource CreateSnapshot(int offset, int length);

		/// <summary>
		/// Creates a new TextReader to read from this text source.
		/// </summary>
		// Token: 0x06000029 RID: 41
		TextReader CreateReader();

		/// <summary>
		/// Creates a new TextReader to read from this text source.
		/// </summary>
		// Token: 0x0600002A RID: 42
		TextReader CreateReader(int offset, int length);

		/// <summary>
		/// Gets the total text length.
		/// </summary>
		/// <returns>The length of the text, in characters.</returns>
		/// <remarks>This is the same as Text.Length, but is more efficient because
		///  it doesn't require creating a String object.</remarks>
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002B RID: 43
		int TextLength { get; }

		/// <summary>
		/// Gets the whole text as string.
		/// </summary>
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002C RID: 44
		string Text { get; }

		/// <summary>
		/// Gets a character at the specified position in the document.
		/// </summary>
		/// <paramref name="offset">The index of the character to get.</paramref>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Offset is outside the valid range (0 to TextLength-1).</exception>
		/// <returns>The character at the specified position.</returns>
		/// <remarks>This is the same as Text[offset], but is more efficient because
		///  it doesn't require creating a String object.</remarks>
		// Token: 0x0600002D RID: 45
		char GetCharAt(int offset);

		/// <summary>
		/// Retrieves the text for a portion of the document.
		/// </summary>
		/// <exception cref="T:System.ArgumentOutOfRangeException">offset or length is outside the valid range.</exception>
		/// <remarks>This is the same as Text.Substring, but is more efficient because
		///  it doesn't require creating a String object for the whole document.</remarks>
		// Token: 0x0600002E RID: 46
		string GetText(int offset, int length);

		/// <summary>
		/// Retrieves the text for a portion of the document.
		/// </summary>
		/// <exception cref="T:System.ArgumentOutOfRangeException">offset or length is outside the valid range.</exception>
		// Token: 0x0600002F RID: 47
		string GetText(ISegment segment);

		/// <summary>
		/// Writes the text from this document into the TextWriter.
		/// </summary>
		// Token: 0x06000030 RID: 48
		void WriteTextTo(TextWriter writer);

		/// <summary>
		/// Writes the text from this document into the TextWriter.
		/// </summary>
		// Token: 0x06000031 RID: 49
		void WriteTextTo(TextWriter writer, int offset, int length);

		/// <summary>
		/// Gets the index of the first occurrence of the character in the specified array.
		/// </summary>
		/// <param name="c">Character to search for</param>
		/// <param name="startIndex">Start index of the area to search.</param>
		/// <param name="count">Length of the area to search.</param>
		/// <returns>The first index where the character was found; or -1 if no occurrence was found.</returns>
		// Token: 0x06000032 RID: 50
		int IndexOf(char c, int startIndex, int count);

		/// <summary>
		/// Gets the index of the first occurrence of any character in the specified array.
		/// </summary>
		/// <param name="anyOf">Characters to search for</param>
		/// <param name="startIndex">Start index of the area to search.</param>
		/// <param name="count">Length of the area to search.</param>
		/// <returns>The first index where any character was found; or -1 if no occurrence was found.</returns>
		// Token: 0x06000033 RID: 51
		int IndexOfAny(char[] anyOf, int startIndex, int count);

		/// <summary>
		/// Gets the index of the first occurrence of the specified search text in this text source.
		/// </summary>
		/// <param name="searchText">The search text</param>
		/// <param name="startIndex">Start index of the area to search.</param>
		/// <param name="count">Length of the area to search.</param>
		/// <param name="comparisonType">String comparison to use.</param>
		/// <returns>The first index where the search term was found; or -1 if no occurrence was found.</returns>
		// Token: 0x06000034 RID: 52
		int IndexOf(string searchText, int startIndex, int count, StringComparison comparisonType);

		/// <summary>
		/// Gets the index of the last occurrence of the specified character in this text source.
		/// </summary>
		/// <param name="c">The search character</param>
		/// <param name="startIndex">Start index of the area to search.</param>
		/// <param name="count">Length of the area to search.</param>
		/// <returns>The last index where the search term was found; or -1 if no occurrence was found.</returns>
		/// <remarks>The search proceeds backwards from (startIndex+count) to startIndex.
		/// This is different than the meaning of the parameters on string.LastIndexOf!</remarks>
		// Token: 0x06000035 RID: 53
		int LastIndexOf(char c, int startIndex, int count);

		/// <summary>
		/// Gets the index of the last occurrence of the specified search text in this text source.
		/// </summary>
		/// <param name="searchText">The search text</param>
		/// <param name="startIndex">Start index of the area to search.</param>
		/// <param name="count">Length of the area to search.</param>
		/// <param name="comparisonType">String comparison to use.</param>
		/// <returns>The last index where the search term was found; or -1 if no occurrence was found.</returns>
		/// <remarks>The search proceeds backwards from (startIndex+count) to startIndex.
		/// This is different than the meaning of the parameters on string.LastIndexOf!</remarks>
		// Token: 0x06000036 RID: 54
		int LastIndexOf(string searchText, int startIndex, int count, StringComparison comparisonType);
	}
}
