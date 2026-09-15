using System;
using System.IO;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Cocos.Launcher.Control
{
	public class ConstsPath
	{
		public virtual string CocosStudioExePath
		{
			get
			{
				if (Platform.IsWindows)
				{
					return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CocosStudio.exe");
				}
				return "/Applications/Cocos/Cocos Studio 2.app/Contents/MacOS/CocosStudio";
			}
		}

		public string DefaultDemoInfoFileName
		{
			get
			{
				return "SampleInfo.xml";
			}
		}

		public virtual string SelfDemoPath
		{
			get
			{
				return Path.Combine(Option.SamplesDir, "Demo");
			}
		}

		public virtual string DownloadDemoPath
		{
			get
			{
				if (Platform.IsWindows)
				{
					return Path.Combine(Option.SamplesDir, "DownloadDemo");
				}
				return Path.Combine(this.DownloadDemoMac, "DownloadDemo");
			}
		}

		public virtual string TempDirPath
		{
			get
			{
				if (Platform.IsWindows)
				{
					return Path.Combine(Option.SamplesDir, "Temporary");
				}
				return Path.Combine(this.DownloadDemoMac, "Temporary");
			}
		}

		public string ZipSuffix
		{
			get
			{
				return ".zip";
			}
		}

		public virtual string LauncherInfoPath
		{
			get
			{
				if (Platform.IsWindows)
				{
					return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Language");
				}
				return "/Applications/Cocos/Cocos Studio 2.app/Contents/MacOS/Language";
			}
		}

		public virtual string UpdateIdentifyXmlPath
		{
			get
			{
				string path = "UpdateIdentify-en.xml";
				if (LanguageOption.CurrentLanguage == LanguageType.Chinese)
				{
					path = "UpdateIdentify.xml";
				}
				else if (LanguageOption.CurrentLanguage == LanguageType.Traditional)
				{
					path = "UpdateIdentify-ZhTW.xml";
				}
				return Path.Combine(Option.UserCustomerConfigFolder, "Launcher", path);
			}
		}

		public virtual string AssetStorePath
		{
			get
			{
				if (Platform.IsWindows)
				{
					return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cocos", "AssetStore");
				}
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Cocos", "AssetStore");
			}
		}

		public virtual string AssetStoreImagePath
		{
			get
			{
				return Path.Combine(Option.UserCustomerConfigFolder, "AssetStore");
			}
		}

		public virtual string PluginListInfoFile
		{
			get
			{
				return Path.Combine(Option.UserCustomerConfigFolder, "pluginListInfo.xml");
			}
		}

		public virtual string LauncherLockPath
		{
			get
			{
				return Path.Combine(Option.UserCustomerConfigFolder, "LockTemp", "launcherRunningLock");
			}
		}

		private const string LauncherName = "Language";

		private const string samplesDirName = "Samples";

		private const string SelfDemoDirName = "Demo";

		private const string DownloadDemoDirName = "DownloadDemo";

		private const string TempDirName = "Temporary";

		private string DownloadDemoMac = Path.Combine(Option.UserCustomerConfigFolder, "Samples");
	}
}
