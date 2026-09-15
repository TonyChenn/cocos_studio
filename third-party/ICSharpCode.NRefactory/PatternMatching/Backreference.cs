using System;
using System.Linq;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Matches the last entry in the specified named group.
	/// </summary>
	public class Backreference : Pattern
	{
		public string ReferencedGroupName
		{
			get
			{
				return this.referencedGroupName;
			}
		}

		public Backreference(string referencedGroupName)
		{
			if (referencedGroupName == null)
			{
				throw new ArgumentNullException("referencedGroupName");
			}
			this.referencedGroupName = referencedGroupName;
		}

		public override bool DoMatch(INode other, Match match)
		{
			INode node = match.Get(this.referencedGroupName).Last<INode>();
			return (node == null && other == null) || node.IsMatch(other);
		}

		private readonly string referencedGroupName;
	}
}
