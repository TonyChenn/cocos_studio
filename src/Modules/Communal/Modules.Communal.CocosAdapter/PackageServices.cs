using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using CocoStudio.Projects;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Modules.Communal.CocosAdapter
{
	public class PackageServices
	{
		public PackageParams PackageParams
		{
			get
			{
				if (this.packageParams == null)
				{
					PackageServices.Instance.InitPackageParams();
				}
				return this.packageParams;
			}
			private set
			{
				this.packageParams = value;
			}
		}

		public static PackageServices Instance
		{
			get
			{
				if (PackageServices.instance == null)
				{
					PackageServices.instance = new PackageServices();
				}
				return PackageServices.instance;
			}
		}

		static PackageServices()
		{
			Services.ProjectOperations.CurrentSelectedSolutionChanged += PackageServices.SolutionChangedHandler;
		}

		private static void SolutionChangedHandler(object sender, SolutionEventArgs e)
		{
			PackageServices.Instance.PackageParams = null;
		}

		internal void InitPackageParams()
		{
			UserData userData = Services.ProjectOperations.CurrentSelectedSolution.UserData;
			PackageParams packageParams = null;
			if (userData.Properties != null)
			{
				IUserData userData2 = null;
				bool flag = userData.Properties.TryGetValue("PackageParamsKey", out userData2);
				if (flag)
				{
					packageParams = (userData2 as PackageParams);
				}
			}
			if (packageParams == null)
			{
				packageParams = new PackageParams();
				userData.Properties.Add("PackageParamsKey", packageParams);
			}
			this.PackageParams = packageParams;
			this.InitFrameworkVersion();
			this.InitPackageName();
			this.InitAndroidVersion();
			this.InitiOSTarget();
			this.InitiOSBundleID();
			this.InitAntPropAndManifest();
		}

		private void InitFrameworkVersion()
		{
			if (!string.IsNullOrWhiteSpace(this.PackageParams.FrameworkVersion))
			{
				return;
			}
			this.PackageParams.FrameworkVersion = string.Empty;
			IReadOnlyList<string> enabledVersions = FrameworkHelper.EnabledVersions;
			string currentFrameworkVersion = Cocos2dxServices.CocosProperties.CurrentFrameworkVersion;
			if (enabledVersions.Contains(currentFrameworkVersion))
			{
				this.PackageParams.FrameworkVersion = currentFrameworkVersion;
				return;
			}
			if (enabledVersions.Contains("cocos2d-x-3.6"))
			{
				this.PackageParams.FrameworkVersion = "cocos2d-x-3.6";
			}
		}

		private void InitPackageName()
		{
			if (!string.IsNullOrWhiteSpace(this.PackageParams.Android_PackageName))
			{
				return;
			}
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			string text = currentSolution.Name.Replace(",", "").Replace("_", "").Replace("-", "").Replace(".", "");
			if (string.IsNullOrEmpty(text))
			{
				text = "CocosProject";
			}
			this.PackageParams.Android_PackageName = "org.cocos." + text;
		}

		private void InitAndroidVersion()
		{
			if (!string.IsNullOrWhiteSpace(this.PackageParams.AndroidVersion))
			{
				return;
			}
			string androidVersion = string.Empty;
			List<string> androidVersions = this.GetAndroidVersions();
			if (androidVersions.Count > 0)
			{
				androidVersion = androidVersions[0];
			}
			this.PackageParams.AndroidVersion = androidVersion;
		}

		private void InitiOSTarget()
		{
			if (!string.IsNullOrWhiteSpace(this.PackageParams.iOS_Target))
			{
				return;
			}
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			Version v = FrameworkHelper.TryParseVersion(Cocos2dxServices.CocosProperties.CurrentFrameworkVersion);
			Version v2 = new Version("3.6");
			if (v != null && v > v2)
			{
				this.PackageParams.iOS_Target = currentSolution.Name + "-mobile";
				return;
			}
			this.PackageParams.iOS_Target = currentSolution.Name + " iOS";
		}

		private void InitiOSBundleID()
		{
			if (!string.IsNullOrWhiteSpace(this.PackageParams.iOS_BundleID))
			{
				return;
			}
			string iOS_BundleID = string.Empty;
			List<string> list = this.GetiOSBundleIDlist();
			if (list.Count > 0)
			{
				iOS_BundleID = list[0];
			}
			this.PackageParams.iOS_BundleID = iOS_BundleID;
		}

		private void InitAntPropAndManifest()
		{
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			if (currentSolution == null)
			{
				return;
			}
			if (Cocos2dxServices.CocosProperties.ProgramLanguage == EnumProgramLanguage.cpp)
			{
				this.PackageParams.AntProperties = currentSolution.BaseDirectory.Combine(new string[]
				{
					"proj.android",
					"ant.properties"
				});
				this.PackageParams.AndroidManifest = currentSolution.BaseDirectory.Combine(new string[]
				{
					"proj.android",
					"AndroidManifest.xml"
				});
				return;
			}
			if (Cocos2dxServices.CocosProperties.ProgramLanguage != EnumProgramLanguage.none)
			{
				this.PackageParams.AntProperties = currentSolution.BaseDirectory.Combine(new string[]
				{
					"frameworks",
					"runtime-src",
					"proj.android",
					"ant.properties"
				});
				this.PackageParams.AndroidManifest = currentSolution.BaseDirectory.Combine(new string[]
				{
					"frameworks",
					"runtime-src",
					"proj.android",
					"AndroidManifest.xml"
				});
			}
		}

		public List<string> GetAndroidVersions()
		{
			string sdkpath = Option.UserConfig.SDKPath;
			List<string> list = new List<string>();
			if (!string.IsNullOrWhiteSpace(sdkpath))
			{
				string path = Path.Combine(sdkpath, "platforms");
				if (Directory.Exists(path))
				{
					string[] directories = Directory.GetDirectories(path);
					foreach (string path2 in directories)
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path2);
						list.Add(fileNameWithoutExtension);
					}
				}
			}
			return list;
		}

		public List<string> GetiOSBundleIDlist()
		{
			List<string> list = new List<string>();
			try
			{
				string command_line = "security find-identity -v -p codesigning";
				string text;
				string text2;
				int num;
				Process.SpawnCommandLineSync(command_line, out text, out text2, out num);
				string[] array = text.Split(new char[]
				{
					'\n'
				});
				foreach (string text3 in array)
				{
					int num2 = text3.IndexOf('"');
					int num3 = text3.LastIndexOf('"');
					if (num2 != -1 && num3 != -1 && num2 != num3)
					{
						list.Add(text3.Substring(num2 + 1, num3 - num2 - 1));
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("获取iOS证书时失败", exception);
			}
			return list;
		}

		public static string GetANTPath()
		{
			string text = string.Empty;
			if (MonoDevelop.Core.Platform.IsWindows)
			{
				string frameworkBaseDirectory = FrameworkHelper.FrameworkBaseDirectory;
				if (!string.IsNullOrEmpty(frameworkBaseDirectory))
				{
					string directoryName = Path.GetDirectoryName(frameworkBaseDirectory);
					text = Path.Combine(directoryName, "tools\\ant\\bin");
				}
			}
			else if (MonoDevelop.Core.Platform.IsMac)
			{
				text = "/Applications/Cocos/tools/ant/bin";
			}
			if (Directory.Exists(text))
			{
				return text;
			}
			return string.Empty;
		}

		public static string GetJDKPath()
		{
			string text = string.Empty;
			if (MonoDevelop.Core.Platform.IsWindows)
			{
				try
				{
					string environmentVariable = Environment.GetEnvironmentVariable("JAVA_HOME");
					if (!string.IsNullOrEmpty(environmentVariable))
					{
						text = environmentVariable;
					}
					text = Path.Combine(text, "bin");
					goto IL_A3;
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("获取JDK路径失败", exception);
					text = string.Empty;
					goto IL_A3;
				}
			}
			if (MonoDevelop.Core.Platform.IsMac)
			{
				try
				{
					string command_line = "which javac";
					string text2;
					string text3;
					int num;
					Process.SpawnCommandLineSync(command_line, out text2, out text3, out num);
					string[] array = text2.Split(new char[]
					{
						'\n'
					});
					text = Path.GetDirectoryName(array[0]);
				}
				catch (Exception exception2)
				{
					LogConfig.Logger.Error("获取JDK路径失败", exception2);
					text = string.Empty;
				}
			}
			IL_A3:
			if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
			{
				return text;
			}
			return string.Empty;
		}

		public bool CheckPackageNameValidity(string packageName, out string output)
		{
			if (string.IsNullOrWhiteSpace(packageName))
			{
				output = LanguageInfo.MessageBox174_SetPackage;
				return false;
			}
			if (!Regex.IsMatch(packageName, "^[A-Za-z0-9.]+$"))
			{
				output = LanguageInfo.MessageBox220_PkgNameRestriction;
				return false;
			}
			if (!Regex.IsMatch(packageName[0].ToString(), "^[A-Za-z]+$"))
			{
				output = LanguageInfo.MessageBox222_Cocos2dNameDetection4;
				return false;
			}
			if (packageName.EndsWith("."))
			{
				output = LanguageInfo.MessageBox219_Cocos2dNameDetection1;
				return false;
			}
			string[] array = packageName.Split(new char[]
			{
				'.'
			});
			if (array.Length <= 2)
			{
				output = LanguageInfo.MessageBox221_Cocos2dNameDetection3;
				return false;
			}
			string[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				string text = array2[i];
				bool result;
				if (string.IsNullOrWhiteSpace(text))
				{
					output = LanguageInfo.MessageBox235_pkgSegmentsNotNull;
					result = false;
				}
				else
				{
					if (Regex.IsMatch(text[0].ToString(), "^[A-Za-z]+$"))
					{
						i++;
						continue;
					}
					output = LanguageInfo.MessageBox236_pkgSegmentsStart;
					result = false;
				}
				return result;
			}
			output = "";
			return true;
		}

		public bool CheckPackageSetting(bool checkAndroid, bool checkiOS, bool checkHTML5)
		{
			PackageParams packageParams = PackageServices.Instance.PackageParams;
			packageParams.RefreshEngineInfo();
			if (packageParams.EngineInfo == null)
			{
				if (MessageBox.Show(LanguageInfo.MessageBox255_disabledFramework, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null) == MessageBoxResult.Yes)
				{
					GlobalCommand.ProjectSettingCmd.RaiseExecute("Package");
				}
				return false;
			}
			return !checkAndroid || this.CheckAndroidPackageSetting();
		}

		private bool CheckAndroidPackageSetting()
		{
			if (string.IsNullOrWhiteSpace(Option.UserConfig.SDKPath) || string.IsNullOrWhiteSpace(Option.UserConfig.NDKPath) || string.IsNullOrWhiteSpace(Option.UserConfig.ANTPath) || string.IsNullOrWhiteSpace(Option.UserConfig.JDKPath))
			{
				if (MessageBox.Show(LanguageInfo.MessageBox253_wetherSetApkPath, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null) == MessageBoxResult.Yes)
				{
					GlobalCommand.PreferencesCmd.RaiseExecute("Platform");
				}
				return false;
			}
			string android_PackageName = PackageServices.Instance.PackageParams.Android_PackageName;
			string text;
			if (!PackageServices.Instance.CheckPackageNameValidity(android_PackageName, out text))
			{
				if (MessageBox.Show(LanguageInfo.MessageBox256_illegalPkgName, MessageBoxButton.YesNo, MessageBoxImage.Other, null, EnumMainButton.Yes, null) == MessageBoxResult.Yes)
				{
					GlobalCommand.ProjectSettingCmd.RaiseExecute("Android");
				}
				return false;
			}
			return true;
		}

		private const string defaultName = "CocosProject";

		private const string defaultAntDirWin = "tools\\ant\\bin";

		private const string defaultAntDirMac = "/Applications/Cocos/tools/ant/bin";

		private PackageParams packageParams;

		private static PackageServices instance = new PackageServices();
	}
}
