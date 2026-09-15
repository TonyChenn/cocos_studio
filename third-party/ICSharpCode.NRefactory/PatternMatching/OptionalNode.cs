using System;

namespace ICSharpCode.NRefactory.PatternMatching
{
	public class OptionalNode : Pattern
	{
		public INode ChildNode
		{
			get
			{
				return this.childNode;
			}
		}

		public OptionalNode(INode childNode)
		{
			if (childNode == null)
			{
				throw new ArgumentNullException("childNode");
			}
			this.childNode = childNode;
		}

		public OptionalNode(string groupName, INode childNode) : this(new NamedNode(groupName, childNode))
		{
		}

		public override bool DoMatchCollection(Role role, INode pos, Match match, BacktrackingInfo backtrackingInfo)
		{
			backtrackingInfo.backtrackingStack.Push(new Pattern.PossibleMatch(pos, match.CheckPoint()));
			return this.childNode.DoMatch(pos, match);
		}

		public override bool DoMatch(INode other, Match match)
		{
			return other == null || other.IsNull || this.childNode.DoMatch(other, match);
		}

		private readonly INode childNode;
	}
}
