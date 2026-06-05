using System;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// AST node that supports pattern matching.
	/// </summary>
	// Token: 0x02000022 RID: 34
	public interface INode
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000139 RID: 313
		Role Role { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600013A RID: 314
		INode FirstChild { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600013B RID: 315
		INode NextSibling { get; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600013C RID: 316
		bool IsNull { get; }

		// Token: 0x0600013D RID: 317
		bool DoMatch(INode other, Match match);

		// Token: 0x0600013E RID: 318
		bool DoMatchCollection(Role role, INode pos, Match match, BacktrackingInfo backtrackingInfo);
	}
}
