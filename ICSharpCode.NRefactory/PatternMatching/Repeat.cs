using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Represents an optional node.
	/// </summary>
	// Token: 0x0200002C RID: 44
	public class Repeat : Pattern
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00004FE6 File Offset: 0x00003FE6
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00004FEE File Offset: 0x00003FEE
		public int MinCount { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00004FF7 File Offset: 0x00003FF7
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00004FFF File Offset: 0x00003FFF
		public int MaxCount { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00005008 File Offset: 0x00004008
		public INode ChildNode
		{
			get
			{
				return this.childNode;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00005010 File Offset: 0x00004010
		public Repeat(INode childNode)
		{
			if (childNode == null)
			{
				throw new ArgumentNullException("childNode");
			}
			this.childNode = childNode;
			this.MinCount = 0;
			this.MaxCount = int.MaxValue;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00005040 File Offset: 0x00004040
		public override bool DoMatchCollection(Role role, INode pos, Match match, BacktrackingInfo backtrackingInfo)
		{
			Stack<Pattern.PossibleMatch> backtrackingStack = backtrackingInfo.backtrackingStack;
			int num = 0;
			if (this.MinCount <= 0)
			{
				backtrackingStack.Push(new Pattern.PossibleMatch(pos, match.CheckPoint()));
			}
			while (num < this.MaxCount && pos != null && this.childNode.DoMatch(pos, match))
			{
				num++;
				do
				{
					pos = pos.NextSibling;
				}
				while (pos != null && pos.Role != role);
				if (num >= this.MinCount)
				{
					backtrackingStack.Push(new Pattern.PossibleMatch(pos, match.CheckPoint()));
				}
			}
			return false;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000050C5 File Offset: 0x000040C5
		public override bool DoMatch(INode other, Match match)
		{
			if (other == null || other.IsNull)
			{
				return this.MinCount <= 0;
			}
			return this.MaxCount >= 1 && this.childNode.DoMatch(other, match);
		}

		// Token: 0x04000049 RID: 73
		private readonly INode childNode;
	}
}
