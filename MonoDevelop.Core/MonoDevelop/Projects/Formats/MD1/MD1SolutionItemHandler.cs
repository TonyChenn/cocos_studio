using System;
using MonoDevelop.Core;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects.Formats.MD1
{
	// Token: 0x020001A8 RID: 424
	internal class MD1SolutionItemHandler : SolutionItemHandler
	{
		// Token: 0x06000FFA RID: 4090 RVA: 0x0003B0AB File Offset: 0x000392AB
		public MD1SolutionItemHandler(SolutionItem item) : base(item)
		{
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0003B0B4 File Offset: 0x000392B4
		public override void Save(IProgressMonitor monitor)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000FFC RID: 4092 RVA: 0x0003B0BB File Offset: 0x000392BB
		public override bool SyncFileName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x0003B0BE File Offset: 0x000392BE
		public override string ItemId
		{
			get
			{
				if (base.Item.ParentFolder != null)
				{
					return base.Item.ParentFolder.ItemId + "/" + base.Item.Name;
				}
				return base.Item.Name;
			}
		}
	}
}
