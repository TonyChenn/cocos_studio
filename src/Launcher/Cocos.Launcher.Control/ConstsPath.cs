using System;
using System.IO;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Cocos.Launcher.Control
{
	// Token: 0x02000007 RID: 7
	public class ConstsPath
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000025F9 File Offset: 0x000007F9
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

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000039 RID: 57 RVA: 0x0000261C File Offset: 0x0000081C
		public string DefaultDemoInfoFileName
		{
			get
			{
				return "SampleInfo.xml";
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002623 File Offset: 0x00000823
		public virtual string SelfDemoPath
		{
			get
			{
				return Path.Combine(Option.SamplesDir, "Demo");
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002634 File Offset: 0x00000834
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000265D File Offset: 0x0000085D
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

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002686 File Offset: 0x00000886
		public string ZipSuffix
		{
			get
			{
				return ".zip";
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600003E RID: 62 RVA: 0x0000268D File Offset: 0x0000088D
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000026B0 File Offset: 0x000008B0
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000026F1 File Offset: 0x000008F1
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000041 RID: 65 RVA: 0x0000272B File Offset: 0x0000092B
		public virtual string AssetStoreImagePath
		{
			get
			{
				return Path.Combine(Option.UserCustomerConfigFolder, "AssetStore");
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000273C File Offset: 0x0000093C
		public virtual string PluginListInfoFile
		{
			get
			{
				return Path.Combine(Option.UserCustomerConfigFolder, "pluginListInfo.xml");
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002750 File Offset: 0x00000950
		public virtual string LauncherLockPath
		{
			get
			{
				return Path.Combine(Option.UserCustomerConfigFolder, "LockTemp", "launcherRunningLock");
			}
		}

		// Token: 0x04000025 RID: 37
		private const string LauncherName = "Language";

		// Token: 0x04000026 RID: 38
		private const string samplesDirName = "Samples";

		// Token: 0x04000027 RID: 39
		private const string SelfDemoDirName = "Demo";

		// Token: 0x04000028 RID: 40
		private const string DownloadDemoDirName = "DownloadDemo";

		// Token: 0x04000029 RID: 41
		private const string TempDirName = "Temporary";

		// Token: 0x0400002A RID: 42
		private string DownloadDemoMac = Path.Combine(Option.UserCustomerConfigFolder, "Samples");
	}
}
