using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Represents an optional node.
	/// </summary>
	public class Repeat : Pattern
	{
		public int MinCount { get; set; }

		public int MaxCount { get; set; }

		public INode ChildNode
		{
			get
			{
				return this.childNode;
			}
		}

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

		public override bool DoMatch(INode other, Match match)
		{
			if (other == null || other.IsNull)
			{
				return this.MinCount <= 0;
			}
			return this.MaxCount >= 1 && this.childNode.DoMatch(other, match);
		}

		private readonly INode childNode;
	}
}
