using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MonoDevelop.Core
{
	// Token: 0x02000231 RID: 561
	public class MacSystemInformation : UnixSystemInformation
	{
		// Token: 0x060014F0 RID: 5360
		[DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
		private static extern int Gestalt(int selector, out int result);

		// Token: 0x060014F1 RID: 5361 RVA: 0x00056094 File Offset: 0x00054294
		private static int Gestalt(string selector)
		{
			int selector2 = (int)(selector[3] | (int)selector[2] << 8 | (int)selector[1] << 16 | (int)selector[0] << 24);
			int result;
			int num = MacSystemInformation.Gestalt(selector2, out result);
			if (num != 0)
			{
				throw new Exception(string.Format("Error reading gestalt for selector '{0}': {1}", selector, num));
			}
			return result;
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x060014F3 RID: 5363 RVA: 0x00056168 File Offset: 0x00054368
		public static Version OsVersion
		{
			get
			{
				return MacSystemInformation.version;
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0005616F File Offset: 0x0005436F
		internal override void AppendOperatingSystem(StringBuilder sb)
		{
			sb.AppendFormat("Mac OS X {0}", MacSystemInformation.version);
			sb.AppendLine();
			base.AppendOperatingSystem(sb);
		}

		// Token: 0x04000653 RID: 1619
		public static readonly Version Yosemite = new Version(10, 10);

		// Token: 0x04000654 RID: 1620
		public static readonly Version Mavericks = new Version(10, 9);

		// Token: 0x04000655 RID: 1621
		public static readonly Version MountainLion = new Version(10, 8);

		// Token: 0x04000656 RID: 1622
		public static readonly Version Lion = new Version(10, 7);

		// Token: 0x04000657 RID: 1623
		public static readonly Version SnowLeopard = new Version(10, 6);

		// Token: 0x04000658 RID: 1624
		private static Version version = new Version(MacSystemInformation.Gestalt("sys1"), MacSystemInformation.Gestalt("sys2"), MacSystemInformation.Gestalt("sys3"));
	}
}
