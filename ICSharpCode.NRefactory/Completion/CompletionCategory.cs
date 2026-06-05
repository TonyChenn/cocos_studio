using System;

namespace ICSharpCode.NRefactory.Completion
{
	// Token: 0x0200012C RID: 300
	public abstract class CompletionCategory : IComparable<CompletionCategory>
	{
		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x0001F193 File Offset: 0x0001E193
		// (set) Token: 0x06000A79 RID: 2681 RVA: 0x0001F19B File Offset: 0x0001E19B
		public string DisplayText { get; set; }

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0001F1A4 File Offset: 0x0001E1A4
		// (set) Token: 0x06000A7B RID: 2683 RVA: 0x0001F1AC File Offset: 0x0001E1AC
		public string Icon { get; set; }

		// Token: 0x06000A7C RID: 2684 RVA: 0x0001F1B5 File Offset: 0x0001E1B5
		protected CompletionCategory()
		{
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0001F1BD File Offset: 0x0001E1BD
		protected CompletionCategory(string displayText, string icon)
		{
			this.DisplayText = displayText;
			this.Icon = icon;
		}

		// Token: 0x06000A7E RID: 2686
		public abstract int CompareTo(CompletionCategory other);
	}
}
