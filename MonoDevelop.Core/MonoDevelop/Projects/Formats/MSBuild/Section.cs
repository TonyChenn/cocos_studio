using System;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001BD RID: 445
	internal class Section
	{
		// Token: 0x0600110B RID: 4363 RVA: 0x000449A9 File Offset: 0x00042BA9
		public Section()
		{
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x000449B8 File Offset: 0x00042BB8
		public Section(string Key, string Val, int Start, int Count)
		{
			this.Key = Key;
			this.Val = Val;
			this.Start = Start;
			this.Count = Count;
		}

		// Token: 0x040004E7 RID: 1255
		public string Key;

		// Token: 0x040004E8 RID: 1256
		public string Val;

		// Token: 0x040004E9 RID: 1257
		public int Start = -1;

		// Token: 0x040004EA RID: 1258
		public int Count;
	}
}
