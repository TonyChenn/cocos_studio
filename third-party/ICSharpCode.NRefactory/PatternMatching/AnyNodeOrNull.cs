using System;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Matches any node.
	/// </summary>
	/// <remarks>Does not match null nodes.</remarks>
	public class AnyNodeOrNull : Pattern
	{
		public string GroupName
		{
			get
			{
				return this.groupName;
			}
		}

		public AnyNodeOrNull(string groupName = null)
		{
			this.groupName = groupName;
		}

		public override bool DoMatch(INode other, Match match)
		{
			if (other == null)
			{
				match.AddNull(this.groupName);
			}
			else
			{
				match.Add(this.groupName, other);
			}
			return true;
		}

		private readonly string groupName;
	}
}
