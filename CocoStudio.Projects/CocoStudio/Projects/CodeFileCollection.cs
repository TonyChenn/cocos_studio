using System;

namespace CocoStudio.Projects
{
	// Token: 0x02000021 RID: 33
	public class CodeFileCollection : ItemCollection<CodeFile>
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00004092 File Offset: 0x00002292
		private CodeFileCollection()
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000409A File Offset: 0x0000229A
		public CodeFileCollection(CocosItem parentItem)
		{
			this.parentItem = parentItem;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000040A9 File Offset: 0x000022A9
		protected override void OnAdd(CodeFile item)
		{
			item.Parent = this.parentItem;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000040B7 File Offset: 0x000022B7
		protected override void OnRemove(CodeFile item)
		{
			item.Parent = null;
		}

		// Token: 0x04000034 RID: 52
		private CocosItem parentItem;
	}
}
