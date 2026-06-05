using System;
using System.Runtime.InteropServices;

namespace MonoDevelop.Core.FileSystem
{
	// Token: 0x02000234 RID: 564
	internal class UnixFileSystemExtension : DefaultFileSystemExtension
	{
		// Token: 0x060014FA RID: 5370
		[DllImport("libc")]
		private static extern IntPtr realpath(string path, IntPtr buffer);

		// Token: 0x060014FB RID: 5371 RVA: 0x00056214 File Offset: 0x00054414
		public override FilePath ResolveFullPath(FilePath path)
		{
			IntPtr intPtr = IntPtr.Zero;
			FilePath result;
			try
			{
				intPtr = Marshal.AllocHGlobal(4097);
				IntPtr value = UnixFileSystemExtension.realpath(path, intPtr);
				result = ((value == IntPtr.Zero) ? "" : Marshal.PtrToStringAuto(intPtr));
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			return result;
		}

		// Token: 0x04000659 RID: 1625
		private const int PATHMAX = 4097;
	}
}
