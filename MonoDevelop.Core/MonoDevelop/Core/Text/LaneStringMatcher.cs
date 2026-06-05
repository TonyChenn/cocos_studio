using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Text
{
	// Token: 0x0200020B RID: 523
	internal class LaneStringMatcher : StringMatcher
	{
		// Token: 0x060013CB RID: 5067 RVA: 0x00051A9B File Offset: 0x0004FC9B
		public LaneStringMatcher(string filter)
		{
			this.matchLanes = new List<LaneStringMatcher.MatchLane>();
			this.filter = filter;
			this.filterLowerCase = ((filter != null) ? filter.ToLowerInvariant() : "");
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00051AD8 File Offset: 0x0004FCD8
		public override bool CalcMatchRank(string name, out int matchRank)
		{
			if (this.filterLowerCase.Length == 0)
			{
				matchRank = int.MinValue;
				return true;
			}
			int num;
			LaneStringMatcher.MatchLane matchLane = this.MatchString(name, out num);
			if (matchLane != null)
			{
				matchRank = this.filterLowerCase.Length - name.Length;
				matchRank -= matchLane.Positions[0];
				matchRank += (matchLane.WordStartsMatched - num) * 100;
				if (matchLane.WordStartsMatched == matchLane.Index + 1)
				{
					matchRank += 100;
				}
				int num2 = (this.filter.Length - matchLane.Index - 1) * 5000;
				matchRank = num2 - (matchLane.Positions[0] + (name.Length - this.filterLowerCase.Length));
				matchRank += matchLane.ExactCaseMatches * 10;
				if (matchLane.Positions[0] == 0)
				{
					matchRank += matchLane.Lengths[0] * 50;
				}
				return true;
			}
			matchRank = int.MinValue;
			return false;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00051BC0 File Offset: 0x0004FDC0
		public override bool IsMatch(string name)
		{
			int num;
			return this.filterLowerCase.Length == 0 || this.MatchString(name, out num) != null;
		}

		/// <summary>
		/// Gets the match indices.
		/// </summary>
		/// <returns>
		/// The indices in the text which are matched by our filter.
		/// </returns>
		/// <param name="text">
		/// The text to match.
		/// </param>
		// Token: 0x060013CE RID: 5070 RVA: 0x00051BEC File Offset: 0x0004FDEC
		public override int[] GetMatch(string text)
		{
			if (this.filterLowerCase.Length == 0)
			{
				return new int[0];
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			int num;
			LaneStringMatcher.MatchLane matchLane = this.MatchString(text, out num);
			if (matchLane == null)
			{
				return null;
			}
			int num2 = 0;
			for (int i = 0; i <= matchLane.Index; i++)
			{
				num2 += matchLane.Lengths[i];
			}
			int[] array = new int[num2];
			int num3 = 0;
			for (int j = 0; j <= matchLane.Index; j++)
			{
				int num4 = matchLane.Positions[j];
				for (int k = 0; k < matchLane.Lengths[j]; k++)
				{
					array[num3++] = num4++;
				}
			}
			return array;
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00051C9C File Offset: 0x0004FE9C
		private LaneStringMatcher.MatchLane MatchString(string text, out int totalWords)
		{
			totalWords = 0;
			if (text == null || text.Length < this.filterLowerCase.Length)
			{
				return null;
			}
			string text2 = text.ToLowerInvariant();
			bool flag = false;
			int num = -1;
			int num2 = 0;
			int length = text.Length;
			int length2 = this.filterLowerCase.Length;
			int num3 = 0;
			while (num3 < length && num2 < length2)
			{
				char c = text2[num3];
				char c2 = this.filterLowerCase[num2];
				bool flag2 = c != text[num3] || num3 == 0 || flag;
				if (flag2)
				{
					totalWords++;
				}
				if (c == c2 && (c2 == this.filter[num2] || c != text[num3]))
				{
					bool flag3 = this.filter[num2] == text[num3];
					num2++;
					if (num == -1)
					{
						num = num3;
					}
					if (length2 == 1)
					{
						LaneStringMatcher.MatchLane matchLane = this.CreateLane(LaneStringMatcher.MatchMode.Substring, num3);
						if (flag3)
						{
							matchLane.ExactCaseMatches++;
						}
						return matchLane;
					}
				}
				flag = this.IsSeparator(c);
				num3++;
			}
			if (num2 < length2)
			{
				return null;
			}
			this.ResetLanePool();
			this.matchLanes.Clear();
			int i = num;
			char c3 = this.filterLowerCase[0];
			bool flag4 = c3 != this.filter[0];
			while (i < text.Length)
			{
				char c4 = text[i];
				char c5 = text2[i];
				bool flag5 = c4 != c5;
				bool flag6 = flag5 || i == 0 || flag;
				if (flag6)
				{
					totalWords++;
				}
				int num4 = (this.matchLanes != null) ? this.matchLanes.Count : 0;
				if (c5 == c3 && (!flag4 || flag5))
				{
					LaneStringMatcher.MatchLane matchLane2 = this.CreateLane(LaneStringMatcher.MatchMode.Substring, i);
					if (flag4 == flag5)
					{
						matchLane2.ExactCaseMatches++;
					}
					this.matchLanes.Add(matchLane2);
					if (this.filterLowerCase.Length == 1)
					{
						return this.matchLanes[0];
					}
					if (flag5 || flag)
					{
						this.matchLanes.Add(this.CreateLane(LaneStringMatcher.MatchMode.Acronym, i));
					}
				}
				for (int j = 0; j < num4; j++)
				{
					LaneStringMatcher.MatchLane matchLane3 = this.matchLanes[j];
					if (matchLane3 != null)
					{
						char c6 = this.filterLowerCase[matchLane3.MatchIndex];
						bool flag7 = c6 != this.filter[matchLane3.MatchIndex];
						bool flag8 = c5 == c6 && (!flag7 || flag5);
						bool flag9 = flag8 && flag7 == flag5;
						bool flag10 = flag8 && flag6;
						if (matchLane3.MatchMode == LaneStringMatcher.MatchMode.Substring)
						{
							if (flag10 && !this.LaneExists(LaneStringMatcher.MatchMode.Acronym, matchLane3.MatchIndex + 1))
							{
								LaneStringMatcher.MatchLane matchLane4 = this.CloneLane(matchLane3);
								matchLane4.MatchMode = LaneStringMatcher.MatchMode.Acronym;
								matchLane4.WordStartsMatched++;
								matchLane4.Index++;
								matchLane4.Positions[matchLane4.Index] = i;
								matchLane4.Lengths[matchLane4.Index] = 1;
								matchLane4.MatchIndex++;
								if (flag9)
								{
									matchLane4.ExactCaseMatches++;
								}
								this.matchLanes.Add(matchLane4);
							}
							if (flag8)
							{
								if (!this.LaneExists(LaneStringMatcher.MatchMode.Acronym, matchLane3.MatchIndex))
								{
									LaneStringMatcher.MatchLane matchLane5 = this.CloneLane(matchLane3);
									matchLane5.MatchMode = LaneStringMatcher.MatchMode.Acronym;
									this.matchLanes.Add(matchLane5);
									if (flag9)
									{
										matchLane5.ExactCaseMatches++;
									}
								}
								matchLane3.Lengths[matchLane3.Index]++;
								matchLane3.MatchIndex++;
							}
							else if (matchLane3.Lengths[matchLane3.Index] > 1)
							{
								matchLane3.MatchMode = LaneStringMatcher.MatchMode.Acronym;
							}
							else
							{
								this.matchLanes[j] = null;
							}
						}
						else if (matchLane3.MatchMode == LaneStringMatcher.MatchMode.Acronym && (flag10 || (flag8 && char.IsPunctuation(c6))))
						{
							if (!this.LaneExists(LaneStringMatcher.MatchMode.Substring, matchLane3.MatchIndex + 1))
							{
								LaneStringMatcher.MatchLane matchLane6 = this.CloneLane(matchLane3);
								matchLane6.MatchMode = LaneStringMatcher.MatchMode.Substring;
								matchLane6.Index++;
								matchLane6.Positions[matchLane6.Index] = i;
								matchLane6.Lengths[matchLane6.Index] = 1;
								matchLane6.MatchIndex++;
								if (flag9)
								{
									matchLane6.ExactCaseMatches++;
								}
								this.matchLanes.Add(matchLane6);
								if (matchLane6.MatchIndex == this.filterLowerCase.Length)
								{
									return matchLane6;
								}
							}
							if (!this.LaneExists(LaneStringMatcher.MatchMode.Acronym, matchLane3.MatchIndex + 1))
							{
								LaneStringMatcher.MatchLane matchLane7 = this.CloneLane(matchLane3);
								this.matchLanes.Add(matchLane7);
								matchLane3.Index++;
								matchLane3.Positions[matchLane3.Index] = i;
								matchLane3.Lengths[matchLane3.Index] = 1;
								matchLane3.MatchIndex++;
								if (flag10)
								{
									matchLane3.WordStartsMatched++;
								}
								if (flag9)
								{
									matchLane7.ExactCaseMatches++;
								}
							}
						}
						if (matchLane3.MatchIndex == this.filterLowerCase.Length)
						{
							return matchLane3;
						}
					}
				}
				flag = this.IsSeparator(c4);
				i++;
			}
			return null;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00052214 File Offset: 0x00050414
		private bool LaneExists(LaneStringMatcher.MatchMode mode, int matchIndex)
		{
			if (this.matchLanes == null)
			{
				return false;
			}
			for (int i = 0; i < this.matchLanes.Count; i++)
			{
				LaneStringMatcher.MatchLane matchLane = this.matchLanes[i];
				if (matchLane != null && matchLane.MatchMode == mode && matchLane.MatchIndex == matchIndex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00052266 File Offset: 0x00050466
		private bool IsSeparator(char ct)
		{
			return ct == '.' || ct == '_' || ct == '-' || ct == ' ' || ct == '/' || ct == '\\';
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00052288 File Offset: 0x00050488
		private void ResetLanePool()
		{
			this.lanePoolIndex = 0;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00052294 File Offset: 0x00050494
		private LaneStringMatcher.MatchLane GetPoolLane()
		{
			if (this.lanePoolIndex < this.lanePool.Count)
			{
				return this.lanePool[this.lanePoolIndex++];
			}
			LaneStringMatcher.MatchLane matchLane = new LaneStringMatcher.MatchLane(this.filterLowerCase.Length * 2);
			this.lanePool.Add(matchLane);
			this.lanePoolIndex++;
			return matchLane;
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x00052300 File Offset: 0x00050500
		private LaneStringMatcher.MatchLane CreateLane(LaneStringMatcher.MatchMode mode, int pos)
		{
			LaneStringMatcher.MatchLane poolLane = this.GetPoolLane();
			poolLane.Initialize(mode, pos);
			if (mode == LaneStringMatcher.MatchMode.Acronym)
			{
				poolLane.WordStartsMatched = 1;
			}
			return poolLane;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00052328 File Offset: 0x00050528
		private LaneStringMatcher.MatchLane CloneLane(LaneStringMatcher.MatchLane other)
		{
			LaneStringMatcher.MatchLane poolLane = this.GetPoolLane();
			poolLane.Initialize(other);
			return poolLane;
		}

		// Token: 0x040005D9 RID: 1497
		private readonly string filter;

		// Token: 0x040005DA RID: 1498
		private readonly string filterLowerCase;

		// Token: 0x040005DB RID: 1499
		private readonly List<LaneStringMatcher.MatchLane> matchLanes;

		// Token: 0x040005DC RID: 1500
		private int lanePoolIndex;

		// Token: 0x040005DD RID: 1501
		private List<LaneStringMatcher.MatchLane> lanePool = new List<LaneStringMatcher.MatchLane>();

		// Token: 0x0200020C RID: 524
		private enum MatchMode
		{
			// Token: 0x040005DF RID: 1503
			Substring,
			// Token: 0x040005E0 RID: 1504
			Acronym
		}

		// Token: 0x0200020D RID: 525
		private class MatchLane
		{
			// Token: 0x060013D6 RID: 5078 RVA: 0x00052344 File Offset: 0x00050544
			public MatchLane(int maxlen)
			{
				this.Positions = new int[maxlen];
				this.Lengths = new int[maxlen];
			}

			// Token: 0x060013D7 RID: 5079 RVA: 0x00052364 File Offset: 0x00050564
			public void Initialize(LaneStringMatcher.MatchMode mode, int pos)
			{
				this.MatchMode = mode;
				this.Positions[0] = pos;
				this.Lengths[0] = 1;
				this.Index = 0;
				this.MatchIndex = 1;
				this.ExactCaseMatches = 0;
				this.WordStartsMatched = 0;
			}

			// Token: 0x060013D8 RID: 5080 RVA: 0x0005239C File Offset: 0x0005059C
			public void Initialize(LaneStringMatcher.MatchLane other)
			{
				for (int i = 0; i <= other.Index; i++)
				{
					this.Positions[i] = other.Positions[i];
					this.Lengths[i] = other.Lengths[i];
				}
				this.MatchMode = other.MatchMode;
				this.MatchIndex = other.MatchIndex;
				this.Index = other.Index;
				this.ExactCaseMatches = other.ExactCaseMatches;
				this.WordStartsMatched = other.WordStartsMatched;
			}

			// Token: 0x040005E1 RID: 1505
			public int[] Positions;

			// Token: 0x040005E2 RID: 1506
			public int[] Lengths;

			// Token: 0x040005E3 RID: 1507
			public LaneStringMatcher.MatchMode MatchMode;

			// Token: 0x040005E4 RID: 1508
			public int Index;

			// Token: 0x040005E5 RID: 1509
			public int MatchIndex;

			// Token: 0x040005E6 RID: 1510
			public int ExactCaseMatches;

			// Token: 0x040005E7 RID: 1511
			public int WordStartsMatched;
		}
	}
}
