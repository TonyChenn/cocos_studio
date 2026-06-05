using System;
using System.IO;

namespace CocoStudio.Core
{
	// Token: 0x02000031 RID: 49
	public class CocosItemModel : IEquatable<CocosItemModel>
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00008AD8 File Offset: 0x00006CD8
		// (set) Token: 0x060001CD RID: 461 RVA: 0x00008AEF File Offset: 0x00006CEF
		public string Name { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00008AF8 File Offset: 0x00006CF8
		// (set) Token: 0x060001CF RID: 463 RVA: 0x00008B0F File Offset: 0x00006D0F
		public string LocalPath { get; private set; }

		// Token: 0x060001D0 RID: 464 RVA: 0x00008B18 File Offset: 0x00006D18
		public CocosItemModel(string localPath)
		{
			this.LocalPath = localPath;
			this.Name = Path.GetFileNameWithoutExtension(localPath);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008B38 File Offset: 0x00006D38
		public bool Equals(CocosItemModel other)
		{
			return !string.IsNullOrEmpty(this.LocalPath) && this.LocalPath.Equals(other.LocalPath);
		}
	}
}
