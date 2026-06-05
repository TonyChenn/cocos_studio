using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Text
{
	// Token: 0x020001F9 RID: 505
	public interface ITextFileProvider
	{
		// Token: 0x06001330 RID: 4912
		IEditableTextFile GetEditableTextFile(FilePath filePath);
	}
}
