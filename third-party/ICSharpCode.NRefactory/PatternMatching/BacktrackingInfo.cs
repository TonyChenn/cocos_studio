using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Container for the backtracking info.
	/// </summary>
	public class BacktrackingInfo
	{
		internal Stack<Pattern.PossibleMatch> backtrackingStack = new Stack<Pattern.PossibleMatch>();
	}
}
