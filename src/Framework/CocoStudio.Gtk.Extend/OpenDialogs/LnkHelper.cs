using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace OpenDialogs
{
	// Token: 0x02000026 RID: 38
	public class LnkHelper
	{
		// Token: 0x02000027 RID: 39
		[Flags]
		public enum SLR_FLAGS
		{
			// Token: 0x04000088 RID: 136
			SLR_NO_UI = 1,
			// Token: 0x04000089 RID: 137
			SLR_ANY_MATCH = 2,
			// Token: 0x0400008A RID: 138
			SLR_UPDATE = 4,
			// Token: 0x0400008B RID: 139
			SLR_NOUPDATE = 8,
			// Token: 0x0400008C RID: 140
			SLR_NOSEARCH = 16,
			// Token: 0x0400008D RID: 141
			SLR_NOTRACK = 32,
			// Token: 0x0400008E RID: 142
			SLR_NOLINKINFO = 64,
			// Token: 0x0400008F RID: 143
			SLR_INVOKE_MSI = 128
		}

		// Token: 0x02000028 RID: 40
		[Flags]
		public enum SLGP_FLAGS
		{
			// Token: 0x04000091 RID: 145
			SLGP_SHORTPATH = 1,
			// Token: 0x04000092 RID: 146
			SLGP_UNCPRIORITY = 2,
			// Token: 0x04000093 RID: 147
			SLGP_RAWPATH = 4
		}

		// Token: 0x02000029 RID: 41
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct WIN32_FIND_DATA
		{
			// Token: 0x04000094 RID: 148
			private const int MAX_PATH = 260;

			// Token: 0x04000095 RID: 149
			public int dwFileAttributes;

			// Token: 0x04000096 RID: 150
			public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;

			// Token: 0x04000097 RID: 151
			public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;

			// Token: 0x04000098 RID: 152
			public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;

			// Token: 0x04000099 RID: 153
			public int nFileSizeHigh;

			// Token: 0x0400009A RID: 154
			public int nFileSizeLow;

			// Token: 0x0400009B RID: 155
			public int dwReserved0;

			// Token: 0x0400009C RID: 156
			public int dwReserved1;

			// Token: 0x0400009D RID: 157
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string cFileName;

			// Token: 0x0400009E RID: 158
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			public string cAlternateFileName;
		}

		// Token: 0x0200002A RID: 42
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("000214F9-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IShellLink
		{
			// Token: 0x06000127 RID: 295
			void GetPath([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxPath, out LnkHelper.WIN32_FIND_DATA pfd, LnkHelper.SLGP_FLAGS fFlags);

			// Token: 0x06000128 RID: 296
			void GetIDList(out IntPtr ppidl);

			// Token: 0x06000129 RID: 297
			void SetIDList(IntPtr pidl);

			// Token: 0x0600012A RID: 298
			void GetDescription([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszName, int cchMaxName);

			// Token: 0x0600012B RID: 299
			void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

			// Token: 0x0600012C RID: 300
			void GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszDir, int cchMaxPath);

			// Token: 0x0600012D RID: 301
			void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

			// Token: 0x0600012E RID: 302
			void GetArguments([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszArgs, int cchMaxPath);

			// Token: 0x0600012F RID: 303
			void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

			// Token: 0x06000130 RID: 304
			void GetHotkey(out short pwHotkey);

			// Token: 0x06000131 RID: 305
			void SetHotkey(short wHotkey);

			// Token: 0x06000132 RID: 306
			void GetShowCmd(out int piShowCmd);

			// Token: 0x06000133 RID: 307
			void SetShowCmd(int iShowCmd);

			// Token: 0x06000134 RID: 308
			void GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

			// Token: 0x06000135 RID: 309
			void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

			// Token: 0x06000136 RID: 310
			void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);

			// Token: 0x06000137 RID: 311
			void Resolve(IntPtr hwnd, LnkHelper.SLR_FLAGS fFlags);

			// Token: 0x06000138 RID: 312
			void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
		}

		// Token: 0x0200002B RID: 43
		[Guid("00021401-0000-0000-C000-000000000046")]
		[ComImport]
		public class ShellLink
		{
		}
	}
}
