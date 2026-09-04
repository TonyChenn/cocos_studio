using System;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Extension methods for <see cref="T:ICSharpCode.NRefactory.Editor.ISegment" />.
	/// </summary>
	// Token: 0x02000010 RID: 16
	public static class ISegmentExtensions
	{
		/// <summary>
		/// Gets whether <paramref name="segment" /> fully contains the specified segment.
		/// </summary>
		/// <remarks>
		/// Use <c>segment.Contains(offset, 0)</c> to detect whether a segment (end inclusive) contains offset;
		/// use <c>segment.Contains(offset, 1)</c> to detect whether a segment (end exclusive) contains offset.
		/// </remarks>
		// Token: 0x0600005D RID: 93 RVA: 0x00002D8C File Offset: 0x00001D8C
		public static bool Contains(this ISegment segment, int offset, int length)
		{
			return segment.Offset <= offset && offset + length <= segment.EndOffset;
		}

		/// <summary>
		/// Gets whether <paramref name="thisSegment" /> fully contains the specified segment.
		/// </summary>
		// Token: 0x0600005E RID: 94 RVA: 0x00002DA7 File Offset: 0x00001DA7
		public static bool Contains(this ISegment thisSegment, ISegment segment)
		{
			return segment != null && thisSegment.Offset <= segment.Offset && segment.EndOffset <= thisSegment.EndOffset;
		}
	}
}
