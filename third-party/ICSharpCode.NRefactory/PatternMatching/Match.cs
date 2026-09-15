using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Represents the result of a pattern matching operation.
	/// </summary>
	public struct Match
	{
		public bool Success
		{
			get
			{
				return this.results != null;
			}
		}

		internal static Match CreateNew()
		{
			Match result;
			result.results = new List<KeyValuePair<string, INode>>();
			return result;
		}

		internal int CheckPoint()
		{
			return this.results.Count;
		}

		internal void RestoreCheckPoint(int checkPoint)
		{
			this.results.RemoveRange(checkPoint, this.results.Count - checkPoint);
		}

		public IEnumerable<INode> Get(string groupName)
		{
			if (this.results != null)
			{
				foreach (KeyValuePair<string, INode> pair in this.results)
				{
					KeyValuePair<string, INode> keyValuePair = pair;
					if (keyValuePair.Key == groupName)
					{
						KeyValuePair<string, INode> keyValuePair2 = pair;
						yield return keyValuePair2.Value;
					}
				}
			}
			yield break;
		}

		public IEnumerable<T> Get<T>(string groupName) where T : INode
		{
			if (this.results != null)
			{
				foreach (KeyValuePair<string, INode> pair in this.results)
				{
					KeyValuePair<string, INode> keyValuePair = pair;
					if (keyValuePair.Key == groupName)
					{
						KeyValuePair<string, INode> keyValuePair2 = pair;
						yield return (T)((object)keyValuePair2.Value);
					}
				}
			}
			yield break;
		}

		public bool Has(string groupName)
		{
			if (this.results == null)
			{
				return false;
			}
			foreach (KeyValuePair<string, INode> keyValuePair in this.results)
			{
				if (keyValuePair.Key == groupName)
				{
					return true;
				}
			}
			return false;
		}

		public void Add(string groupName, INode node)
		{
			if (groupName != null && node != null)
			{
				this.results.Add(new KeyValuePair<string, INode>(groupName, node));
			}
		}

		internal void AddNull(string groupName)
		{
			if (groupName != null)
			{
				this.results.Add(new KeyValuePair<string, INode>(groupName, null));
			}
		}

		private List<KeyValuePair<string, INode>> results;
	}
}
