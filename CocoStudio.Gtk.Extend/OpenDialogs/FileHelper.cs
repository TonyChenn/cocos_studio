using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace OpenDialogs
{
	// Token: 0x02000025 RID: 37
	public static class FileHelper
	{
		// Token: 0x06000124 RID: 292 RVA: 0x000063A8 File Offset: 0x000045A8
		public static bool IsFileOrDirectory(this string path)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				try
				{
					if (Directory.Exists(path))
					{
						return true;
					}
					if (File.Exists(path))
					{
						FileInfo fileInfo = new FileInfo(path);
						return fileInfo != null && fileInfo.Exists;
					}
				}
				catch
				{
				}
			}
			return false;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000641C File Offset: 0x0000461C
		public static string GetLnkRealtivePath(this string path)
		{
			LnkHelper.IShellLink shellLink = (LnkHelper.IShellLink)new LnkHelper.ShellLink();
			IPersistFile persistFile = shellLink as IPersistFile;
			persistFile.Load(path, 0);
			StringBuilder stringBuilder = new StringBuilder(260);
			LnkHelper.WIN32_FIND_DATA win32_FIND_DATA;
			shellLink.GetPath(stringBuilder, stringBuilder.Capacity, out win32_FIND_DATA, LnkHelper.SLGP_FLAGS.SLGP_RAWPATH);
			return stringBuilder.ToString();
		}
	}
}
