using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using Xwt.GtkBackend;

namespace CocoStudio.Basic
{
	public static class Option
	{
		public static Version EditorVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version;
			}
		}

		public static EnumApp CurrentApp { get; private set; }

		public static UserConfigService UserConfig
		{
			get
			{
				return UserConfigService.Instance;
			}
		}

		public static string AssemblyDir { get; private set; }

		public static string EditorDefaultResourcePath { get; private set; }

		public static string UserCustomerConfigFolder { get; private set; }

		public static string UserChannelPath { get; private set; }

		public static string SamplesRootDir { get; private set; }

		public static string SamplesDir { get; private set; }

		public static string DefaultSamplesDir { get; private set; }

		public static string DownloadSamplesDir { get; private set; }

		public static string DefaultProjectsDir { get; private set; }

		public static string HotkeyConfigPath { get; private set; }

		public static string AddinLocationFolder { get; private set; }

		public static string LuaScriptFolder { get; private set; }

		public static string AddinConfigFolder { get; private set; }

		public static string LauncherAddinConfigFolder { get; private set; }

		public static string MyDocumentsFolder { get; private set; }

		public static string CocosInstallDir { get; private set; }

		public static string FrameworkDir { get; private set; }

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

		public static bool IsStartWithOpenProject
		{
			get
			{
				return Option.CheckIsStartWithOpenProject();
			}
		}

		public static bool IsXP
		{
			get
			{
				return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1;
			}
		}

		public static bool IsTestEnvironment { get; private set; }

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

		private static void CheckDefaultPojectsDir()
		{
			Option.CheckDirToCreat(Option.DefaultProjectsDir);
		}

		private static void CheckResourceDir()
		{
			Option.CheckDirToCreat(Option.EditorDefaultResourcePath);
		}

		private static void CheckPluginDir()
		{
			Option.CheckDirToCreat(Option.AddinLocationFolder);
		}

		private static bool CheckIsStartWithOpenProject()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			return commandLineArgs.Length > 1 && !string.IsNullOrEmpty(commandLineArgs[1]) && File.Exists(commandLineArgs[1].Trim());
		}

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

		public static string GetEditorResourceFullPath(string fileName)
		{
			return Path.Combine(Option.EditorDefaultResourcePath, fileName);
		}

		public static string GetAssemblyFullPath(string fileName)
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
		}

		public static void SetCurrentIDE(EnumApp editorIDE)
		{
			Option.CurrentApp = editorIDE;
		}

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

		public static string GetUserConfigFileByName(string fileName)
		{
			return Path.Combine(Option.UserCustomerConfigFolder, fileName);
		}

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

		public const string softwareName = "CocosStudio";

		public const string softwareName2 = "CocosStudio2";

		public const string softwareFullName = "Cocos Studio";

		public const string cocos = "Cocos";

		public const string buildVersion = "2.3.3.0";

		public const string displayVersion = "2.3.3.0";

		public const string MainVersion = "2.3";

		public const string AddinNamespace = "CocoStudio";

		public const string cocosStudioExeName = "CocosStudio.exe";

		public const string launcherExeName = "Cocos.exe";

		public const string installerExeName = "Cocos.Installer.exe";

		private const string sourceFileName = "Install.config";

		public const string mainSceneName = "MainScene.csd";

		public const string updateScriptName = "Cocos.Update";

		public const string hotkeyConfigName = "Hotkey.config";

		private const string defaultPluginFolderName = "Addins";

		public const string autoUpdateFolderName = "AutoUpdate";

		public const string lockFolderName = "LockTemp";

		private const string defaultProjectFolderName = "Projects";

		public const string samplesFolderName = "Samples";

		public const string demoFolderName = "Demo";

		public const string downloadDemoFolderName = "DownloadDemo";

		public const string luaScriptFolderName = "LuaScript";

		public const string frameworkFolderName = "Framework";

		public const string defaultFrameworkVersion = "cocos2d-x-3.6";

		public const string keystoreFileSuffix = ".keystore";

		private static string testMarkFileName = "Test.xml";
	}
}
