using System;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Represents a named node within a pattern.
	/// </summary>
	public class NamedNode : Pattern
	{
		public string GroupName
		{
			get
			{
				return this.groupName;
			}
		}

		public INode ChildNode
		{
			get
			{
				return this.childNode;
			}
		}

		public NamedNode(string groupName, INode childNode)
		{
			if (childNode == null)
			{
				throw new ArgumentNullException("childNode");
			}
			this.groupName = groupName;
			this.childNode = childNode;
		}

		public override bool DoMatch(INode other, Match match)
		{
			match.Add(this.groupName, other);
			return this.childNode.DoMatch(other, match);
		}

		private readonly string groupName;

		private readonly INode childNode;
	}
}
