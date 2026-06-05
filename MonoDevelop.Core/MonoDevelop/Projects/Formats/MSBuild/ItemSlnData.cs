using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001C7 RID: 455
	internal class ItemSlnData
	{
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x00046561 File Offset: 0x00044761
		public List<string> ConfigLines
		{
			get
			{
				if (this.configLines == null)
				{
					this.configLines = new List<string>();
				}
				return this.configLines;
			}
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0004657C File Offset: 0x0004477C
		public static ItemSlnData ForItem(SolutionItem item)
		{
			ItemSlnData itemSlnData = (ItemSlnData)item.ExtendedProperties[typeof(ItemSlnData)];
			if (itemSlnData == null)
			{
				itemSlnData = new ItemSlnData();
				item.ExtendedProperties[typeof(ItemSlnData)] = itemSlnData;
			}
			return itemSlnData;
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x000465C4 File Offset: 0x000447C4
		public static void TransferData(SolutionItem source, SolutionItem target)
		{
			ItemSlnData itemSlnData = (ItemSlnData)source.ExtendedProperties[typeof(ItemSlnData)];
			if (itemSlnData != null)
			{
				target.ExtendedProperties[typeof(ItemSlnData)] = itemSlnData;
			}
		}

		// Token: 0x0400050A RID: 1290
		private List<string> configLines;
	}
}
