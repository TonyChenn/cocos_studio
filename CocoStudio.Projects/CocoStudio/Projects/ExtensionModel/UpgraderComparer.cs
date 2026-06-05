using System;
using System.Collections.Generic;

namespace CocoStudio.Projects.ExtensionModel
{
	// Token: 0x0200000F RID: 15
	internal class UpgraderComparer : IComparer<IUpgrader>
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002B07 File Offset: 0x00000D07
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00002B0E File Offset: 0x00000D0E
		public static UpgraderComparer Instance { get; private set; } = new UpgraderComparer();

		// Token: 0x0600003C RID: 60 RVA: 0x00002B22 File Offset: 0x00000D22
		private UpgraderComparer()
		{
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002B2A File Offset: 0x00000D2A
		public int Compare(IUpgrader x, IUpgrader y)
		{
			return x.Version.CompareTo(y.Version);
		}
	}
}
