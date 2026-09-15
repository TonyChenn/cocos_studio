using System;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Matches any node.
	/// </summary>
	/// <remarks>Does not match null nodes.</remarks>
	public class AnyNode : Pattern
	{
		public string GroupName
		{
			get
			{
				return this.groupName;
			}
		}

		public AnyNode(string groupName = null)
		{
			this.groupName = groupName;
		}

		public override bool DoMatch(INode other, Match match)
		{
			match.Add(this.groupName, other);
			return other != null && !other.IsNull;
		}

		private readonly string groupName;
	}
}
