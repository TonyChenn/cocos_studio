using System;

namespace ICSharpCode.NRefactory.PatternMatching
{
	// Token: 0x02000028 RID: 40
	public static class PatternExtensions
	{
		/// <summary>
		/// Performs a pattern matching operation.
		/// <c>this</c> is the pattern, <paramref name="other" /> is the AST that is being matched.
		/// </summary>
		/// <returns>
		/// A match object. Check <see cref="P:ICSharpCode.NRefactory.PatternMatching.Match.Success" /> to see whether the match was successful.
		/// </returns>
		/// <remarks>
		/// Patterns are ASTs that contain special pattern nodes (from the PatternMatching namespace).
		/// However, it is also possible to match two ASTs without any pattern nodes -
		/// doing so will produce a successful match if the two ASTs are structurally identical.
		/// </remarks>
		// Token: 0x06000156 RID: 342 RVA: 0x000049BC File Offset: 0x000039BC
		public static Match Match(this INode pattern, INode other)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}
			Match match = ICSharpCode.NRefactory.PatternMatching.Match.CreateNew();
			if (pattern.DoMatch(other, match))
			{
				return match;
			}
			return default(Match);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000049F2 File Offset: 0x000039F2
		public static bool IsMatch(this INode pattern, INode other)
		{
			if (pattern == null)
			{
				throw new ArgumentNullException("pattern");
			}
			return pattern.DoMatch(other, ICSharpCode.NRefactory.PatternMatching.Match.CreateNew());
		}
	}
}
