using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Represents the result of a pattern matching operation.
	/// </summary>
	// Token: 0x02000029 RID: 41
	public struct Match
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00004A0E File Offset: 0x00003A0E
		public bool Success
		{
			get
			{
				return this.results != null;
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00004A1C File Offset: 0x00003A1C
		internal static Match CreateNew()
		{
			Match result;
			result.results = new List<KeyValuePair<string, INode>>();
			return result;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00004A36 File Offset: 0x00003A36
		internal int CheckPoint()
		{
			return this.results.Count;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00004A43 File Offset: 0x00003A43
		internal void RestoreCheckPoint(int checkPoint)
		{
			this.results.RemoveRange(checkPoint, this.results.Count - checkPoint);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00004C40 File Offset: 0x00003C40
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

		// Token: 0x0600015D RID: 349 RVA: 0x00004E54 File Offset: 0x00003E54
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

		// Token: 0x0600015E RID: 350 RVA: 0x00004E80 File Offset: 0x00003E80
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

		// Token: 0x0600015F RID: 351 RVA: 0x00004EEC File Offset: 0x00003EEC
		public void Add(string groupName, INode node)
		{
			if (groupName != null && node != null)
			{
				this.results.Add(new KeyValuePair<string, INode>(groupName, node));
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00004F06 File Offset: 0x00003F06
		internal void AddNull(string groupName)
		{
			if (groupName != null)
			{
				this.results.Add(new KeyValuePair<string, INode>(groupName, null));
			}
		}

		// Token: 0x04000045 RID: 69
		private List<KeyValuePair<string, INode>> results;
	}
}
