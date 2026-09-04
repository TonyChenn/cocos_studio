using System;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// An (Offset,Length)-pair.
	/// </summary>
	// Token: 0x0200000E RID: 14
	public interface ISegment
	{
		/// <summary>
		/// Gets the start offset of the segment.
		/// </summary>
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000054 RID: 84
		int Offset { get; }

		/// <summary>
		/// Gets the length of the segment.
		/// </summary>
		/// <remarks>For line segments (IDocumentLine), the length does not include the line delimeter.</remarks>
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000055 RID: 85
		int Length { get; }

		/// <summary>
		/// Gets the end offset of the segment.
		/// </summary>
		/// <remarks>EndOffset = Offset + Length;</remarks>
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000056 RID: 86
		int EndOffset { get; }
	}
}
