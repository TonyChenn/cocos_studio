using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Text
{
	// Token: 0x020001F7 RID: 503
	public interface ITextFile
	{
		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001325 RID: 4901
		FilePath Name { get; }

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001326 RID: 4902
		string Text { get; }

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001327 RID: 4903
		int Length { get; }

		// Token: 0x06001328 RID: 4904
		string GetText(int startPosition, int endPosition);

		// Token: 0x06001329 RID: 4905
		char GetCharAt(int position);

		// Token: 0x0600132A RID: 4906
		int GetPositionFromLineColumn(int line, int column);

		// Token: 0x0600132B RID: 4907
		void GetLineColumnFromPosition(int position, out int line, out int column);
	}
}
