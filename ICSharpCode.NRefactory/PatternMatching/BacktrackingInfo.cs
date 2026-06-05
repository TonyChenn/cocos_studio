using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Container for the backtracking info.
	/// </summary>
	// Token: 0x02000021 RID: 33
	public class BacktrackingInfo
	{
		// Token: 0x0400003E RID: 62
		internal Stack<Pattern.PossibleMatch> backtrackingStack = new Stack<Pattern.PossibleMatch>();
	}
}
