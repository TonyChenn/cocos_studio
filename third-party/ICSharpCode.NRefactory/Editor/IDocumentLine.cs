using System;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// A line inside a <see cref="T:ICSharpCode.NRefactory.Editor.IDocument" />.
	/// </summary>
	// Token: 0x0200000F RID: 15
	public interface IDocumentLine : ISegment
	{
		/// <summary>
		/// Gets the length of this line, including the line delimiter.
		/// </summary>
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000057 RID: 87
		int TotalLength { get; }

		/// <summary>
		/// Gets the length of the line terminator.
		/// Returns 1 or 2; or 0 at the end of the document.
		/// </summary>
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000058 RID: 88
		int DelimiterLength { get; }

		/// <summary>
		/// Gets the number of this line.
		/// The first line has the number 1.
		/// </summary>
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000059 RID: 89
		int LineNumber { get; }

		/// <summary>
		/// Gets the previous line. Returns null if this is the first line in the document.
		/// </summary>
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005A RID: 90
		IDocumentLine PreviousLine { get; }

		/// <summary>
		/// Gets the next line. Returns null if this is the last line in the document.
		/// </summary>
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005B RID: 91
		IDocumentLine NextLine { get; }

		/// <summary>
		/// Gets whether the line was deleted.
		/// </summary>
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005C RID: 92
		bool IsDeleted { get; }
	}
}
