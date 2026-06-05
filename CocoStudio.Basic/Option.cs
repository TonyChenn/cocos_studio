using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using Xwt.GtkBackend;

namespace CocoStudio.Basic
{
	// Token: 0x0200000B RID: 11
	public static class Option
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000276C File Offset: 0x0000096C
		public static Version EditorVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002790 File Offset: 0x00000990
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000027A6 File Offset: 0x000009A6
		public static EnumApp CurrentApp { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000027B0 File Offset: 0x000009B0
		public static UserConfigService UserConfig
		{
			get
			{
				return UserConfigService.Instance;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000027C8 File Offset: 0x000009C8
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000027DE File Offset: 0x000009DE
		public static string AssemblyDir { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000027E8 File Offset: 0x000009E8
		// (set) Token: 0x06000035 RID: 53 RVA: 0x000027FE File Offset: 0x000009FE
		public static string EditorDefaultResourcePath { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002808 File Offset: 0x00000A08
		// (set) Token: 0x06000037 RID: 55 RVA: 0x0000281E File Offset: 0x00000A1E
		public static string UserCustomerConfigFolder { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002828 File Offset: 0x00000A28
		// (set) Token: 0x06000039 RID: 57 RVA: 0x0000283E File Offset: 0x00000A3E
		public static string UserChannelPath { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002848 File Offset: 0x00000A48
		// (set) Token: 0x0600003B RID: 59 RVA: 0x0000285E File Offset: 0x00000A5E
		public static string SamplesRootDir { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002868 File Offset: 0x00000A68
		// (set) Token: 0x0600003D RID: 61 RVA: 0x0000287E File Offset: 0x00000A7E
		public static string SamplesDir { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002888 File Offset: 0x00000A88
		// (set) Token: 0x0600003F RID: 63 RVA: 0x0000289E File Offset: 0x00000A9E
		public static string DefaultSamplesDir { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000028A8 File Offset: 0x00000AA8
		// (set) Token: 0x06000041 RID: 65 RVA: 0x000028BE File Offset: 0x00000ABE
		public static string DownloadSamplesDir { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000028C8 File Offset: 0x00000AC8
		// (set) Token: 0x06000043 RID: 67 RVA: 0x000028DE File Offset: 0x00000ADE
		public static string DefaultProjectsDir { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000028E8 File Offset: 0x00000AE8
		// (set) Token: 0x06000045 RID: 69 RVA: 0x000028FE File Offset: 0x00000AFE
		public static string HotkeyConfigPath { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002908 File Offset: 0x00000B08
		// (set) Token: 0x06000047 RID: 71 RVA: 0x0000291E File Offset: 0x00000B1E
		public static string AddinLocationFolder { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002928 File Offset: 0x00000B28
		// (set) Token: 0x06000049 RID: 73 RVA: 0x0000293E File Offset: 0x00000B3E
		public static string LuaScriptFolder { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002948 File Offset: 0x00000B48
		// (set) Token: 0x0600004B RID: 75 RVA: 0x0000295E File Offset: 0x00000B5E
		public static string AddinConfigFolder { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002968 File Offset: 0x00000B68
		// (set) Token: 0x0600004D RID: 77 RVA: 0x0000297E File Offset: 0x00000B7E
		public static string LauncherAddinConfigFolder { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002988 File Offset: 0x00000B88
		// (set) Token: 0x0600004F RID: 79 RVA: 0x0000299E File Offset: 0x00000B9E
		public static string MyDocumentsFolder { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000029A8 File Offset: 0x00000BA8
		// (set) Token: 0x06000051 RID: 81 RVA: 0x000029BE File Offset: 0x00000BBE
		public static string CocosInstallDir { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000029C8 File Offset: 0x00000BC8
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000029DE File Offset: 0x00000BDE
		public static string FrameworkDir { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000029E8 File Offset: 0x00000BE8
		public static string UpdateServerURL
		{
			get
			{
				foreach (string text in Option.UpdateServerURLs)
				{
					return text;
				}
				return string.Empty;
			}
		}

		public static IEnumerable<string> UpdateServerURLs
		{
			get
			{
				string text;
				if (Option.IsTestEnvironment)
				{
					text = Option.NormalizeUpdateServerURL("http://cocostest.chinacloudsites.cn/");
				}
				else
				{
					text = Option.NormalizeUpdateServerURL("http://localhost/");
				}
				if (!string.IsNullOrEmpty(text))
				{
					yield return text;
				}
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002A14 File Offset: 0x00000C14
		public static bool IsStartWithOpenProject
		{
			get
			{
				return Option.CheckIsStartWithOpenProject();
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002A2C File Offset: 0x00000C2C
		public static bool IsXP
		{
			get
			{
				return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002A78 File Offset: 0x00000C78
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002A8E File Offset: 0x00000C8E
		public static bool IsTestEnvironment { get; private set; }

		// Token: 0x06000059 RID: 89 RVA: 0x00002A98 File Offset: 0x00000C98
		static Option()
		{
			try
			{
				Option.Init();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002AE0 File Offset: 0x00000CE0
		private static void Init()
		{
			Option.AssemblyDir = AppDomain.CurrentDomain.BaseDirectory;
			Option.EditorDefaultResourcePath = Path.Combine(Option.AssemblyDir, "EditorDefaultRes");
			Option.UserCustomerConfigFolder = Path.Combine(Option.GetConfigFolderLocation(), "Cocos", "CocosStudio2");
			Log4Wrap.SetLocation(Option.UserCustomerConfigFolder);
			Option.SamplesRootDir = Option.GetSamplesFolderLocation();
			Option.SamplesDir = Path.Combine(Option.SamplesRootDir, "Cocos", "CocosStudio2", "Samples");
			Option.DefaultSamplesDir = Path.Combine(Option.SamplesDir, "Demo");
			if (Platform.IsMac)
			{
				Option.DownloadSamplesDir = Path.Combine(Option.UserCustomerConfigFolder, "Samples", "DownloadDemo");
			}
			else
			{
				Option.DownloadSamplesDir = Path.Combine(Option.SamplesDir, "DownloadDemo");
			}
			Option.DefaultProjectsDir = Path.Combine(Option.UserCustomerConfigFolder, "Projects");
			Option.AddinLocationFolder = Path.Combine(Option.UserCustomerConfigFolder, "Addins");
			Option.AddinConfigFolder = Path.Combine(Option.UserCustomerConfigFolder, "AddinConfig", Option.EditorVersion.ToString(3));
			Option.LauncherAddinConfigFolder = Path.Combine(Option.AddinConfigFolder, "Cocos");
			Option.AddinConfigFolder = Path.Combine(Option.AddinConfigFolder, "CocosStudio");
			Option.MyDocumentsFolder = Option.GetMyDocumentsFolder();
			Option.LuaScriptFolder = Path.Combine(Option.AddinLocationFolder, "LuaScript");
			Option.HotkeyConfigPath = Path.Combine(Option.UserCustomerConfigFolder, "Hotkey.config");
			Option.CocosInstallDir = Option.GetCocosInstallDir();
			Option.FrameworkDir = Path.Combine(Option.UserCustomerConfigFolder, "Framework");
			Option.UserChannelPath = Path.Combine(Option.UserCustomerConfigFolder, "Config.ini");
			Option.IsTestEnvironment = File.Exists(Option.GetUserConfigFileByName(Option.testMarkFileName));
			Option.CheckResourceDir();
			Option.CheckUserConfigDir();
			Option.CheckDefaultPojectsDir();
			Option.CheckPluginDir();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002CBF File Offset: 0x00000EBF
		private static void CheckUserConfigDir()
		{
			Option.CheckDirToCreat(Option.UserCustomerConfigFolder);
		}

		private static string NormalizeUpdateServerURL(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				return string.Empty;
			}
			string text = url.Trim();
			if (!text.EndsWith("/", StringComparison.Ordinal))
			{
				text += "/";
			}
			return text;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002CCD File Offset: 0x00000ECD
		private static void CheckDefaultPojectsDir()
		{
			Option.CheckDirToCreat(Option.DefaultProjectsDir);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002CDB File Offset: 0x00000EDB
		private static void CheckResourceDir()
		{
			Option.CheckDirToCreat(Option.EditorDefaultResourcePath);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002CE9 File Offset: 0x00000EE9
		private static void CheckPluginDir()
		{
			Option.CheckDirToCreat(Option.AddinLocationFolder);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002CF8 File Offset: 0x00000EF8
		private static bool CheckIsStartWithOpenProject()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			return commandLineArgs.Length > 1 && !string.IsNullOrEmpty(commandLineArgs[1]) && File.Exists(commandLineArgs[1].Trim());
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002D40 File Offset: 0x00000F40
		private static string GetConfigFolderLocation()
		{
			string result;
			if (Platform.IsMac)
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				result = Path.Combine(folderPath, "Library", "Application Support");
			}
			else
			{
				result = Option.GetConfigFolderOnWindows();
			}
			return result;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002D80 File Offset: 0x00000F80
		private static string GetSamplesFolderLocation()
		{
			string result;
			if (Platform.IsMac)
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				result = Path.Combine("/Library", "Application Support");
			}
			else
			{
				result = Option.GetConfigFolderOnWindows();
			}
			return result;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002DC0 File Offset: 0x00000FC0
		private static string GetConfigFolderOnWindows()
		{
			try
			{
				object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ChuKong\\CocoStudio\\AppSourceKey", "AppSourceFolder", null);
				if (value != null && !string.IsNullOrEmpty(value.ToString()))
				{
					string text = value.ToString();
					if (text.Substring(text.Length - 1, 1) == ":")
					{
						text += "\\";
					}
					if (Directory.Exists(text))
					{
						return text;
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Read install.config failed.", exception);
			}
			return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002E78 File Offset: 0x00001078
		private static string GetInstallDirOnWindows()
		{
			try
			{
				object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Cocos", "CocosStudioDir", null);
				if (value != null && !string.IsNullOrEmpty(value.ToString()))
				{
					string text = value.ToString();
					if (text.Substring(text.Length - 1, 1) == ":")
					{
						text += "\\";
					}
					if (Directory.Exists(text))
					{
						return text;
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Read install.config failed.", exception);
			}
			return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002F30 File Offset: 0x00001130
		private static string GetCocosInstallDir()
		{
			string result;
			if (Platform.IsMac)
			{
				result = Path.Combine("/Applications", "Cocos");
			}
			else
			{
				result = Option.GetInstallDirOnWindows();
			}
			return result;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002F68 File Offset: 0x00001168
		public static string GetMyDocumentsFolder()
		{
			string result;
			if (Platform.IsMac)
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				result = Path.Combine(folderPath, "Documents");
			}
			else
			{
				result = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
			return result;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002FA4 File Offset: 0x000011A4
		public static string GetEditorResourceFullPath(string fileName)
		{
			return Path.Combine(Option.EditorDefaultResourcePath, fileName);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002FC4 File Offset: 0x000011C4
		public static string GetAssemblyFullPath(string fileName)
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002FE6 File Offset: 0x000011E6
		public static void SetCurrentIDE(EnumApp editorIDE)
		{
			Option.CurrentApp = editorIDE;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002FF0 File Offset: 0x000011F0
		public static void CheckDirToCreat(string dirPath)
		{
			if (!Directory.Exists(dirPath))
			{
				try
				{
					Directory.CreateDirectory(dirPath);
				}
				catch (Exception exception)
				{
					LogConfig.Output.Error("创建配置目录失败", exception);
				}
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000303C File Offset: 0x0000123C
		public static string GetUserConfigFileByName(string fileName)
		{
			return Path.Combine(Option.UserCustomerConfigFolder, fileName);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000305C File Offset: 0x0000125C
		public static void SetFileAttributeCanRead(string filePath)
		{
			if (File.Exists(filePath))
			{
				FileInfo fileInfo = new FileInfo(filePath);
				if (fileInfo.Attributes.ToString().IndexOf("ReadOnly") != -1)
				{
					fileInfo.Attributes = FileAttributes.Normal;
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000030B0 File Offset: 0x000012B0
		public static string ConvertToMacPath(string filePath)
		{
			string result;
			if (filePath == null)
			{
				result = "";
			}
			else
			{
				result = filePath.Replace('\\', '/');
			}
			return result;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000030E0 File Offset: 0x000012E0
		public static bool CheckIsReadableDir(string dir)
		{
			string fullPath;
			try
			{
				fullPath = Path.GetFullPath(dir);
			}
			catch
			{
				return false;
			}
			bool result;
			if (Directory.Exists(fullPath))
			{
				result = true;
			}
			else
			{
				try
				{
					string testDirectory = Option.GetTestDirectory(fullPath);
					if (string.IsNullOrEmpty(testDirectory))
					{
						result = false;
					}
					else
					{
						Directory.CreateDirectory(testDirectory);
						Directory.Delete(testDirectory);
						result = true;
					}
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003164 File Offset: 0x00001364
		public static bool CheckIsWritableDir(string dir)
		{
			string fullPath;
			try
			{
				fullPath = Path.GetFullPath(dir);
			}
			catch
			{
				return false;
			}
			bool result;
			try
			{
				string testDirectory = Option.GetTestDirectory(fullPath);
				if (string.IsNullOrEmpty(testDirectory))
				{
					result = false;
				}
				else
				{
					Directory.CreateDirectory(testDirectory);
					Directory.Delete(testDirectory);
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000031D4 File Offset: 0x000013D4
		private static string GetTestDirectory(string dir)
		{
			string result;
			if (string.IsNullOrWhiteSpace(dir))
			{
				result = string.Empty;
			}
			else
			{
				string text = string.Empty;
				if (Directory.Exists(dir))
				{
					string arg = "testFolder";
					string fullPath = Path.GetFullPath(dir);
					int num = 0;
					text = Path.Combine(fullPath, arg + num);
					while (Directory.Exists(text))
					{
						num++;
						text = Path.Combine(fullPath, arg + num);
					}
				}
				else
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(dir);
					DirectoryInfo parent = Directory.GetParent(directoryInfo.FullName);
					while (parent != null && !Directory.Exists(parent.FullName))
					{
						directoryInfo = parent;
						parent = Directory.GetParent(directoryInfo.FullName);
					}
					if (parent != null)
					{
						text = directoryInfo.FullName;
					}
				}
				result = text;
			}
			return result;
		}

		// Token: 0x04000021 RID: 33
		public const string softwareName = "CocosStudio";

		// Token: 0x04000022 RID: 34
		public const string softwareName2 = "CocosStudio2";

		// Token: 0x04000023 RID: 35
		public const string softwareFullName = "Cocos Studio";

		// Token: 0x04000024 RID: 36
		public const string cocos = "Cocos";

		// Token: 0x04000025 RID: 37
		public const string buildVersion = "2.3.3.0";

		// Token: 0x04000026 RID: 38
		public const string displayVersion = "2.3.3.0";

		// Token: 0x04000027 RID: 39
		public const string MainVersion = "2.3";

		// Token: 0x04000028 RID: 40
		public const string AddinNamespace = "CocoStudio";

		// Token: 0x04000029 RID: 41
		public const string cocosStudioExeName = "CocosStudio.exe";

		// Token: 0x0400002A RID: 42
		public const string launcherExeName = "Cocos.exe";

		// Token: 0x0400002B RID: 43
		public const string installerExeName = "Cocos.Installer.exe";

		// Token: 0x0400002C RID: 44
		private const string sourceFileName = "Install.config";

		// Token: 0x0400002D RID: 45
		public const string mainSceneName = "MainScene.csd";

		// Token: 0x0400002E RID: 46
		public const string updateScriptName = "Cocos.Update";

		// Token: 0x0400002F RID: 47
		public const string hotkeyConfigName = "Hotkey.config";

		// Token: 0x04000030 RID: 48
		private const string defaultPluginFolderName = "Addins";

		// Token: 0x04000031 RID: 49
		public const string autoUpdateFolderName = "AutoUpdate";

		// Token: 0x04000032 RID: 50
		public const string lockFolderName = "LockTemp";

		// Token: 0x04000033 RID: 51
		private const string defaultProjectFolderName = "Projects";

		// Token: 0x04000034 RID: 52
		public const string samplesFolderName = "Samples";

		// Token: 0x04000035 RID: 53
		public const string demoFolderName = "Demo";

		// Token: 0x04000036 RID: 54
		public const string downloadDemoFolderName = "DownloadDemo";

		// Token: 0x04000037 RID: 55
		public const string luaScriptFolderName = "LuaScript";

		// Token: 0x04000038 RID: 56
		public const string frameworkFolderName = "Framework";

		// Token: 0x04000039 RID: 57
		public const string defaultFrameworkVersion = "cocos2d-x-3.6";

		// Token: 0x0400003A RID: 58
		public const string keystoreFileSuffix = ".keystore";

		// Token: 0x0400003B RID: 59
		private static string testMarkFileName = "Test.xml";
	}
}
