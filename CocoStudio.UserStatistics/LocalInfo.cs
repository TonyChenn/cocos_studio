using System;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using CocoStudio.Basic;
using Gdk;
using MonoDevelop.Core;

namespace CocoStudio.UserStatistics
{
	// Token: 0x0200000B RID: 11
	public class LocalInfo
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002390 File Offset: 0x00000590
		public LocalInfo()
		{
			try
			{
				this.init();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000023E0 File Offset: 0x000005E0
		private void init()
		{
			int num = (Screen.Default != null) ? Screen.Default.Width : 0;
			int num2 = (Screen.Default != null) ? Screen.Default.Height : 0;
			int num3 = (Screen.Default != null) ? Screen.Default.NMonitors : 0;
			this.OSVersion = Environment.OSVersion.ToString();
			this.OSBit = (Environment.Is64BitOperatingSystem ? "x64" : "x86");
			this.OSLan = this.GetLanguage();
			this.FrameVer = Environment.Version.ToString();
			this.ChannelNum = this.GetChannelNum();
			this.ScreenResulotion = num + "*" + num2;
			this.ScreenCount = num3.ToString();
			this.ScreenDpi = this.GetDPI().ToString();
			this.CPUCoreCount = Environment.ProcessorCount.ToString();
			this.OSName = this.GetOSName();
			this.MACAddress = LocalInfo.GetMacAddress();
			this.isVM = this.GetIsVM().ToString();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002500 File Offset: 0x00000700
		private static int GetPhisicalMemory()
		{
			int result;
			if (Platform.IsMac)
			{
				result = 0;
			}
			else
			{
				ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher
				{
					Query = new SelectQuery("Win32_PhysicalMemory ", "", new string[]
					{
						"Capacity"
					})
				}.Get();
				ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectCollection.GetEnumerator();
				long num = 0L;
				while (enumerator.MoveNext())
				{
					ManagementBaseObject managementBaseObject = enumerator.Current;
					if (managementBaseObject.Properties["Capacity"].Value != null)
					{
						try
						{
							num += long.Parse(managementBaseObject.Properties["Capacity"].Value.ToString());
						}
						catch
						{
							return 0;
						}
					}
				}
				result = (int)(num / 1024L / 1024L);
			}
			return result;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000025F8 File Offset: 0x000007F8
		private string GetChannelNum()
		{
			string result = "000000";
			try
			{
				if (File.Exists(Option.UserChannelPath))
				{
					using (FileStream fileStream = new FileStream(Option.UserChannelPath, FileMode.Open, FileAccess.Read))
					{
						StreamReader streamReader = new StreamReader(fileStream);
						result = streamReader.ReadLine();
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002680 File Offset: 0x00000880
		private string GetLanguage()
		{
			return CultureInfo.InstalledUICulture.NativeName.ToString();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000026A4 File Offset: 0x000008A4
		private double GetDPI()
		{
			Screen @default = Screen.Default;
			double result;
			if (@default == null)
			{
				result = 0.0;
			}
			else
			{
				result = @default.Resolution;
			}
			return result;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000026DC File Offset: 0x000008DC
		public string GetOSName()
		{
			Version version = Environment.OSVersion.Version;
			string result;
			if (Platform.IsWindows)
			{
				if (version.Major == 5 && version.Minor == 0)
				{
					result = "Windows 2000";
				}
				else if (version.Major == 5 && version.Minor == 1)
				{
					result = "Windows XP";
				}
				else if (version.Major == 5 && version.Minor == 2)
				{
					result = "Windows 2003";
				}
				else if (version.Major == 6 && version.Minor == 0)
				{
					result = "Windows Vista";
				}
				else if (version.Major == 6 && version.Minor == 1)
				{
					result = "Windows7";
				}
				else if (version.Major == 6 && version.Minor == 2)
				{
					result = "Windows8";
				}
				else if (version.Major == 6 && version.Minor == 4)
				{
					result = "Windows10";
				}
				else if (version.Major == 10)
				{
					result = "Windows10";
				}
				else
				{
					result = "未知";
				}
			}
			else
			{
				result = "MacOSX";
			}
			return result;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002864 File Offset: 0x00000A64
		public static string GetMacAddress()
		{
			string text = "";
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				foreach (NetworkInterface networkInterface in allNetworkInterfaces)
				{
					if (!networkInterface.GetPhysicalAddress().ToString().Equals(""))
					{
						text = networkInterface.GetPhysicalAddress().ToString();
						for (int j = 1; j < 6; j++)
						{
							text = text.Insert(3 * j - 1, ":");
						}
						break;
					}
				}
			}
			catch
			{
			}
			return text;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002914 File Offset: 0x00000B14
		public bool GetIsVM()
		{
			return false;
		}

		// Token: 0x0400003D RID: 61
		public string OSName;

		// Token: 0x0400003E RID: 62
		public string OSVersion;

		// Token: 0x0400003F RID: 63
		public string OSBit;

		// Token: 0x04000040 RID: 64
		public string OSLan;

		// Token: 0x04000041 RID: 65
		public string FrameVer;

		// Token: 0x04000042 RID: 66
		public string OSMemory;

		// Token: 0x04000043 RID: 67
		public string ChannelNum;

		// Token: 0x04000044 RID: 68
		public string ScreenResulotion;

		// Token: 0x04000045 RID: 69
		public string ScreenCount;

		// Token: 0x04000046 RID: 70
		public string ScreenDpi;

		// Token: 0x04000047 RID: 71
		public string CPUCoreCount;

		// Token: 0x04000048 RID: 72
		public string MACAddress;

		// Token: 0x04000049 RID: 73
		public string installationID = "";

		// Token: 0x0400004A RID: 74
		public string isVM;
	}
}
