using System;

namespace ICSharpCode.NRefactory.Completion
{
	// Token: 0x0200012A RID: 298
	[Flags]
	public enum DisplayFlags
	{
		// Token: 0x0400038B RID: 907
		None = 0,
		// Token: 0x0400038C RID: 908
		Hidden = 1,
		// Token: 0x0400038D RID: 909
		Obsolete = 2,
		// Token: 0x0400038E RID: 910
		DescriptionHasMarkup = 4,
		// Token: 0x0400038F RID: 911
		NamedArgument = 8,
		// Token: 0x04000390 RID: 912
		IsImportCompletion = 16,
		// Token: 0x04000391 RID: 913
		MarkedBold = 32
	}
}
