using System;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x02000137 RID: 311
	public interface IFormatStringError
	{
		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000ACE RID: 2766
		int StartLocation { get; }

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000ACF RID: 2767
		int EndLocation { get; }

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06000AD0 RID: 2768
		string Message { get; }

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06000AD1 RID: 2769
		string OriginalText { get; }

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06000AD2 RID: 2770
		string SuggestedReplacementText { get; }
	}
}
