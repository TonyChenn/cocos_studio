using System;
using System.Collections;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Matches one of several alternatives.
	/// </summary>
	public class Choice : Pattern, IEnumerable<INode>, IEnumerable
	{
		public void Add(string name, INode alternative)
		{
			if (alternative == null)
			{
				throw new ArgumentNullException("alternative");
			}
			this.alternatives.Add(new NamedNode(name, alternative));
		}

		public void Add(INode alternative)
		{
			if (alternative == null)
			{
				throw new ArgumentNullException("alternative");
			}
			this.alternatives.Add(alternative);
		}

		public override bool DoMatch(INode other, Match match)
		{
			int checkPoint = match.CheckPoint();
			foreach (INode node in this.alternatives)
			{
				if (node.DoMatch(other, match))
				{
					return true;
				}
				match.RestoreCheckPoint(checkPoint);
			}
			return false;
		}

		IEnumerator<INode> IEnumerable<INode>.GetEnumerator()
		{
			return this.alternatives.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.alternatives.GetEnumerator();
		}

		private readonly List<INode> alternatives = new List<INode>();
	}
}
