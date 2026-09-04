using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Completion
{
	// Token: 0x02000129 RID: 297
	public interface ICompletionData
	{
		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000A6A RID: 2666
		// (set) Token: 0x06000A6B RID: 2667
		CompletionCategory CompletionCategory { get; set; }

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000A6C RID: 2668
		// (set) Token: 0x06000A6D RID: 2669
		string DisplayText { get; set; }

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000A6E RID: 2670
		// (set) Token: 0x06000A6F RID: 2671
		string Description { get; set; }

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000A70 RID: 2672
		// (set) Token: 0x06000A71 RID: 2673
		string CompletionText { get; set; }

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000A72 RID: 2674
		// (set) Token: 0x06000A73 RID: 2675
		DisplayFlags DisplayFlags { get; set; }

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000A74 RID: 2676
		bool HasOverloads { get; }

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000A75 RID: 2677
		IEnumerable<ICompletionData> OverloadedData { get; }

		// Token: 0x06000A76 RID: 2678
		void AddOverload(ICompletionData data);
	}
}
