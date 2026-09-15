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
	public class LocalInfo
	{
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

		private string GetLanguage()
		{
			return CultureInfo.InstalledUICulture.NativeName.ToString();
		}

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

		public bool GetIsVM()
		{
			return false;
		}

		public string OSName;

		public string OSVersion;

		public string OSBit;

		public string OSLan;

		public string FrameVer;

		public string OSMemory;

		public string ChannelNum;

		public string ScreenResulotion;

		public string ScreenCount;

		public string ScreenDpi;

		public string CPUCoreCount;

		public string MACAddress;

		public string installationID = "";

		public string isVM;
	}
}
