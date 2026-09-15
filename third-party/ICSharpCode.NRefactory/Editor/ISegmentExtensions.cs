using System;

namespace ICSharpCode.NRefactory.Editor
{
	/// <summary>
	/// Extension methods for <see cref="T:ICSharpCode.NRefactory.Editor.ISegment" />.
	/// </summary>
	public static class ISegmentExtensions
	{
		/// <summary>
		/// Gets whether <paramref name="segment" /> fully contains the specified segment.
		/// </summary>
		/// <remarks>
		/// Use <c>segment.Contains(offset, 0)</c> to detect whether a segment (end inclusive) contains offset;
		/// use <c>segment.Contains(offset, 1)</c> to detect whether a segment (end exclusive) contains offset.
		/// </remarks>
		public static bool Contains(this ISegment segment, int offset, int length)
		{
			return segment.Offset <= offset && offset + length <= segment.EndOffset;
		}

		/// <summary>
		/// Gets whether <paramref name="thisSegment" /> fully contains the specified segment.
		/// </summary>
		public static bool Contains(this ISegment thisSegment, ISegment segment)
		{
			return segment != null && thisSegment.Offset <= segment.Offset && segment.EndOffset <= thisSegment.EndOffset;
		}
	}
}
