using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000143 RID: 323
	public interface ILanguageBinding
	{
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000C25 RID: 3109
		string Language { get; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000C26 RID: 3110
		string SingleLineCommentTag { get; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000C27 RID: 3111
		string BlockCommentStartTag { get; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000C28 RID: 3112
		string BlockCommentEndTag { get; }

		// Token: 0x06000C29 RID: 3113
		bool IsSourceCodeFile(FilePath fileName);

		// Token: 0x06000C2A RID: 3114
		FilePath GetFileName(FilePath fileNameWithoutExtension);
	}
}
