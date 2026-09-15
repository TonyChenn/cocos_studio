using System;
using System.Runtime.InteropServices;

namespace Cocos.Update
{
	public class PlatformHelper
	{
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

		[DllImport("libc")]
		private static extern int uname(IntPtr buf);

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

		private static bool? isMacPlatform = null;
	}
}
