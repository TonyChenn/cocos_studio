using System;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Formats.MD1
{
	// Token: 0x020001A9 RID: 425
	internal class MD1SolutionEntityItemHandler : MD1SolutionItemHandler
	{
		// Token: 0x06000FFE RID: 4094 RVA: 0x0003B0FE File Offset: 0x000392FE
		public MD1SolutionEntityItemHandler(SolutionEntityItem item) : base(item)
		{
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0003B108 File Offset: 0x00039308
		public override void Save(IProgressMonitor monitor)
		{
			SolutionEntityItem solutionEntityItem = (SolutionEntityItem)base.Item;
			solutionEntityItem.FileFormat.Format.WriteFile(solutionEntityItem.FileName, solutionEntityItem, monitor);
		}
	}
}
