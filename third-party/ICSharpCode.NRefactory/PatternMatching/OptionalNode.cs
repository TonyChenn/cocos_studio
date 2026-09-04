using System;

namespace ICSharpCode.NRefactory.PatternMatching
{
	// Token: 0x0200002B RID: 43
	public class OptionalNode : Pattern
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00004F6E File Offset: 0x00003F6E
		public INode ChildNode
		{
			get
			{
				return this.childNode;
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00004F76 File Offset: 0x00003F76
		public OptionalNode(INode childNode)
		{
			if (childNode == null)
			{
				throw new ArgumentNullException("childNode");
			}
			this.childNode = childNode;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00004F93 File Offset: 0x00003F93
		public OptionalNode(string groupName, INode childNode) : this(new NamedNode(groupName, childNode))
		{
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00004FA2 File Offset: 0x00003FA2
		public override bool DoMatchCollection(Role role, INode pos, Match match, BacktrackingInfo backtrackingInfo)
		{
			backtrackingInfo.backtrackingStack.Push(new Pattern.PossibleMatch(pos, match.CheckPoint()));
			return this.childNode.DoMatch(pos, match);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00004FCA File Offset: 0x00003FCA
		public override bool DoMatch(INode other, Match match)
		{
			return other == null || other.IsNull || this.childNode.DoMatch(other, match);
		}

		// Token: 0x04000048 RID: 72
		private readonly INode childNode;
	}
}
