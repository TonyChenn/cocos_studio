using System;
using System.Runtime.InteropServices;

namespace Cocos.Update
{
	// Token: 0x02000003 RID: 3
	public class PlatformHelper
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002104 File Offset: 0x00000304
		public static bool IsMacPlatform
		{
			get
			{
				if (PlatformHelper.isMacPlatform == null)
				{
					PlatformHelper.isMacPlatform = new bool?(PlatformHelper.IsRunningOnMac());
				}
				return PlatformHelper.isMacPlatform.Value;
			}
		}

		// Token: 0x06000007 RID: 7
		[DllImport("libc")]
		private static extern int uname(IntPtr buf);

		// Token: 0x06000008 RID: 8 RVA: 0x0000212C File Offset: 0x0000032C
		private static bool IsRunningOnMac()
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = Marshal.AllocHGlobal(8192);
				if (PlatformHelper.uname(intPtr) == 0)
				{
					string a = Marshal.PtrToStringAnsi(intPtr);
					if (a == "Darwin")
					{
						return true;
					}
				}
			}
			catch
			{
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			return false;
		}

		// Token: 0x04000002 RID: 2
		private static bool? isMacPlatform = null;
	}
}
