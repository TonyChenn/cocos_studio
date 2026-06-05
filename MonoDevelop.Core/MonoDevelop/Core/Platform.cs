using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace MonoDevelop.Core
{
	// Token: 0x02000224 RID: 548
	public static class Platform
	{
		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x000549B2 File Offset: 0x00052BB2
		// (set) Token: 0x06001487 RID: 5255 RVA: 0x000549B9 File Offset: 0x00052BB9
		public static Version OSVersion { get; private set; } = Environment.OSVersion.Version;

		// Token: 0x06001488 RID: 5256 RVA: 0x000549C4 File Offset: 0x00052BC4
		static Platform()
		{
			if (Platform.IsWindows)
			{
				Platform.InitWindowsNativeLibs();
				return;
			}
			if (Platform.IsMac)
			{
				Platform.InitMacFoundation();
			}
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x00054A32 File Offset: 0x00052C32
		public static void Initialize()
		{
		}

		// Token: 0x0600148A RID: 5258
		[DllImport("libc")]
		private static extern int uname(IntPtr buf);

		// Token: 0x0600148B RID: 5259 RVA: 0x00054A34 File Offset: 0x00052C34
		private static bool IsRunningOnMac()
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = Marshal.AllocHGlobal(8192);
				if (Platform.uname(intPtr) == 0)
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

		// Token: 0x0600148C RID: 5260
		[DllImport("libc")]
		private static extern IntPtr dlopen(string name, int mode);

		// Token: 0x0600148D RID: 5261 RVA: 0x00054AAC File Offset: 0x00052CAC
		private static void InitMacFoundation()
		{
			Platform.dlopen("/System/Library/Frameworks/Foundation.framework/Foundation", 1);
			Platform.OSVersion = new Version(Platform.Gestalt("sys1"), Platform.Gestalt("sys2"), Platform.Gestalt("sys3"));
		}

		// Token: 0x0600148E RID: 5262
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetDllDirectory(string lpPathName);

		// Token: 0x0600148F RID: 5263
		[DllImport("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
		private static extern int Gestalt(int selector, out int result);

		// Token: 0x06001490 RID: 5264 RVA: 0x00054AE4 File Offset: 0x00052CE4
		private static int Gestalt(string selector)
		{
			int selector2 = (int)(selector[3] | (int)selector[2] << 8 | (int)selector[1] << 16 | (int)selector[0] << 24);
			int result;
			int num = Platform.Gestalt(selector2, out result);
			if (num != 0)
			{
				LoggingService.LogError("Error reading gestalt for selector '{0}': {1}", new object[]
				{
					selector,
					num
				});
				return 0;
			}
			return result;
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00054B48 File Offset: 0x00052D48
		private static void InitWindowsNativeLibs()
		{
			string text = null;
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Xamarin\\GtkSharp\\InstallFolder"))
			{
				if (registryKey != null)
				{
					text = (registryKey.GetValue(null) as string);
				}
			}
			if (text == null || !File.Exists(Path.Combine(text, "bin", "libgtk-win32-2.0-0.dll")))
			{
				LoggingService.LogError("Did not find registered GTK# installation");
				return;
			}
			string dllDirectory = Path.Combine(text, "bin");
			try
			{
				if (Platform.SetDllDirectory(dllDirectory))
				{
					return;
				}
			}
			catch (EntryPointNotFoundException)
			{
			}
			LoggingService.LogError("Unable to set GTK# dll directory");
		}

		// Token: 0x04000628 RID: 1576
		public static readonly bool IsWindows = Path.DirectorySeparatorChar == '\\';

		// Token: 0x04000629 RID: 1577
		public static readonly bool IsMac = !Platform.IsWindows && Platform.IsRunningOnMac();

		// Token: 0x0400062A RID: 1578
		public static readonly bool IsLinux = !Platform.IsMac && !Platform.IsWindows;
	}
}
