using System;
using System.IO;
using CocoStudio.Basic;
using Microsoft.Win32;

namespace Cocos.Launcher.Library
{
	public class RegistryServices
	{
		public static bool IsKeysContainsDisplayName(string displayName)
		{
			return RegistryServices.GetDisplayNameToProductcode(displayName) != null;
		}

		public static string GetUninstallString(string displayName)
		{
			string text = string.Empty;
			try
			{
				RegistryKey uinstallRegistryKey = RegistryServices.GetUinstallRegistryKey(Registry.LocalMachine);
				if (uinstallRegistryKey != null)
				{
					text = RegistryServices.ForUninstallKeyNames(displayName, uinstallRegistryKey);
					if (string.IsNullOrEmpty(text))
					{
						RegistryKey uinstallRegistryKey2 = RegistryServices.GetUinstallRegistryKey(Registry.CurrentUser);
						if (uinstallRegistryKey2 != null)
						{
							text = RegistryServices.ForUninstallKeyNames(displayName, uinstallRegistryKey2);
						}
					}
				}
			}
			catch (Exception arg)
			{
				LogConfig.Output.Error("获取卸载列表失败：" + arg);
			}
			return text;
		}

		private static string ForUninstallKeyNames(string displayName, RegistryKey uninstall_LocalMachine)
		{
			string result = string.Empty;
			string[] subKeyNames = uninstall_LocalMachine.GetSubKeyNames();
			if (subKeyNames != null)
			{
				foreach (string name in subKeyNames)
				{
					RegistryKey registryKey = uninstall_LocalMachine.OpenSubKey(name);
					string a = registryKey.GetValue("DisplayName", string.Empty).ToString();
					if (a == displayName)
					{
						string text = registryKey.GetValue("UninstallString", string.Empty).ToString();
						if (text.ToLower().Contains("msiexec.exe"))
						{
							string[] array2 = text.Split(new char[]
							{
								'{',
								'}'
							});
							result = array2[1];
							break;
						}
						if (text.Contains("\""))
						{
							text = text.Replace("\"", "");
						}
						if (File.Exists(text))
						{
							result = text;
							break;
						}
					}
				}
			}
			return result;
		}

		public static string GetInstallLocation(string displayName)
		{
			string result = string.Empty;
			RegistryKey displayNameToProductcode = RegistryServices.GetDisplayNameToProductcode(displayName);
			if (displayNameToProductcode != null)
			{
				try
				{
					string a = displayNameToProductcode.GetValue("DisplayName", string.Empty).ToString();
					if (a == displayName)
					{
						string text = displayNameToProductcode.GetValue("InstallLocation", string.Empty).ToString();
						if (Directory.Exists(text))
						{
							result = text;
							return result;
						}
					}
				}
				catch (Exception arg)
				{
					LogConfig.Output.Error("获取卸载列表失败：" + arg);
				}
				return result;
			}
			return result;
		}

		public static string GetDisplayVersion(string displayName)
		{
			string result = string.Empty;
			RegistryKey displayNameToProductcode = RegistryServices.GetDisplayNameToProductcode(displayName);
			if (displayNameToProductcode != null)
			{
				try
				{
					result = displayNameToProductcode.GetValue("DisplayVersion", string.Empty).ToString();
				}
				catch (Exception arg)
				{
					LogConfig.Output.Error("获取卸载列表失败：" + arg);
				}
			}
			return result;
		}

		private static RegistryKey GetDisplayNameToProductcode(string displayName)
		{
			RegistryKey registryKey = null;
			RegistryKey uinstallRegistryKey = RegistryServices.GetUinstallRegistryKey(Registry.LocalMachine);
			if (uinstallRegistryKey != null)
			{
				registryKey = RegistryServices.GetContainsDisplayName(displayName, uinstallRegistryKey);
				if (registryKey == null)
				{
					RegistryKey uinstallRegistryKey2 = RegistryServices.GetUinstallRegistryKey(Registry.CurrentUser);
					if (uinstallRegistryKey2 != null)
					{
						registryKey = RegistryServices.GetContainsDisplayName(displayName, uinstallRegistryKey2);
					}
				}
			}
			return registryKey;
		}

		private static RegistryKey GetContainsDisplayName(string displayName, RegistryKey uninstall)
		{
			RegistryKey result = null;
			string[] subKeyNames = uninstall.GetSubKeyNames();
			if (subKeyNames != null)
			{
				foreach (string name in subKeyNames)
				{
					RegistryKey registryKey = uninstall.OpenSubKey(name);
					try
					{
						string a = registryKey.GetValue("DisplayName", string.Empty).ToString();
						if (a == displayName)
						{
							result = registryKey;
							break;
						}
					}
					catch (Exception arg)
					{
						LogConfig.Output.Error("获取卸载列表失败：" + arg);
					}
				}
			}
			return result;
		}

		private static RegistryKey GetUinstallRegistryKey(RegistryKey registryKey)
		{
			string name = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
			return registryKey.OpenSubKey(name);
		}

		public static string GetSetupFactoryRegistryKeyCode(string displayName)
		{
			string result = string.Empty;
			string setupFactoryInstallLocation = RegistryServices.GetSetupFactoryInstallLocation(displayName);
			if (!string.IsNullOrEmpty(setupFactoryInstallLocation))
			{
				string path = "Uninstall " + displayName + ".lnk";
				string text = Path.Combine(setupFactoryInstallLocation, path);
				if (File.Exists(text))
				{
					result = text;
				}
			}
			return result;
		}

		public static string GetSetupFactoryInstallLocation(string displayName)
		{
			string result = string.Empty;
			RegistryKey displayNameToProductcode = RegistryServices.GetDisplayNameToProductcode(displayName);
			if (displayNameToProductcode != null)
			{
				try
				{
					string a = displayNameToProductcode.GetValue("DisplayName", string.Empty).ToString();
					if (a == displayName)
					{
						string text = displayNameToProductcode.GetValue("InstallLocation", string.Empty).ToString();
						if (!string.IsNullOrEmpty(text))
						{
							result = text;
						}
					}
				}
				catch (Exception arg)
				{
					LogConfig.Output.Error("获取安装目录：" + displayName + arg);
				}
			}
			return result;
		}
	}
}
