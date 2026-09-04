using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.Core.Commands;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter.Platform
{
	// Token: 0x02000011 RID: 17
	[Extension(typeof(IPlatform))]
	internal class AndroidPlatform : BasePlatform
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003627 File Offset: 0x00001827
		public override EnumPlatform PlatformType
		{
			get
			{
				return EnumPlatform.Android;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000076 RID: 118 RVA: 0x0000362A File Offset: 0x0000182A
		public override int Order
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000362D File Offset: 0x0000182D
		protected override string PlatformName
		{
			get
			{
				return "android";
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003634 File Offset: 0x00001834
		public override string GetDisplayName(EnumOperationType opType)
		{
			switch (opType)
			{
			case EnumOperationType.Package:
				return LanguageInfo.Dialog_AndroidPackage;
			case EnumOperationType.Run:
				return LanguageInfo.Run_Android;
			default:
				return string.Empty;
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003668 File Offset: 0x00001868
		protected override bool OnCanExecute(EnumOperationType opType, PackageParams prms)
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
			return opType != EnumOperationType.Run || this.CheckAndroidDevice();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003774 File Offset: 0x00001974
		private bool CheckAndroidDevice()
		{
			bool isDeviceConnected = false;
			Process process = new Process();
			using (process)
			{
				string text = string.Format("{0}/platform-tools/adb", Option.UserConfig.SDKPath);
				string fileName = text.Replace("\\", "/");
				try
				{
					process.StartInfo = new ProcessStartInfo();
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.FileName = fileName;
					process.StartInfo.Arguments = "devices";
					process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs args)
					{
						string data = args.Data;
						if (data != null && !data.Contains("List of devices attached") && data.Contains("device"))
						{
							isDeviceConnected = true;
						}
					};
					process.Start();
					process.BeginOutputReadLine();
					process.WaitForExit();
				}
				catch (Exception exception)
				{
					isDeviceConnected = false;
					LogConfig.Logger.Error("检测安卓设备时出错", exception);
				}
			}
			if (!isDeviceConnected)
			{
				RunInfoDialog runInfoDialog = new RunInfoDialog();
				runInfoDialog.Run();
				runInfoDialog.Destroy();
				return false;
			}
			return true;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000038A4 File Offset: 0x00001AA4
		protected override bool OnExecuteInitialize(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			if (!this.RegisterkeyStore(prms))
			{
				monitor.SendInfo("Failed to register keystore!");
				return false;
			}
			if (!this.RenamePackageName(prms))
			{
				monitor.SendInfo("Failed to rename package name!");
				return false;
			}
			return true;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000038D4 File Offset: 0x00001AD4
		private bool RegisterkeyStore(PackageParams info)
		{
			if (string.IsNullOrEmpty(info.AndroidkeyStore))
			{
				return true;
			}
			if (!File.Exists(info.AndroidkeyStore))
			{
				return false;
			}
			try
			{
				string text = string.Empty;
				text = string.Format("key.store={0}\n", info.AndroidkeyStore);
				text = text.Replace('\\', '/');
				text += string.Format("key.store.password={0}\n", info.KeystorePassword);
				text += string.Format("key.alias={0}\n", info.KeystoreAliasName);
				text += string.Format("key.alias.password={0}\n", info.KeystoreAliasPassword);
				if (!File.Exists(info.AntProperties))
				{
					throw new FileNotFoundException(info.AntProperties);
				}
				using (FileStream fileStream = File.Open(info.AntProperties, FileMode.Create, FileAccess.ReadWrite))
				{
					StreamWriter streamWriter = new StreamWriter(fileStream);
					streamWriter.Write(text);
					streamWriter.Flush();
					streamWriter.Close();
				}
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
				return false;
			}
			return true;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000039F8 File Offset: 0x00001BF8
		private bool RenamePackageName(PackageParams info)
		{
			if (string.IsNullOrEmpty(info.AndroidManifest) || !File.Exists(info.AndroidManifest))
			{
				return false;
			}
			try
			{
				XElement xelement = XElement.Load(info.AndroidManifest);
				for (XAttribute xattribute = xelement.FirstAttribute; xattribute != null; xattribute = xattribute.NextAttribute)
				{
					string a = xattribute.Name.ToString();
					if (a == "package" && info.Android_PackageName != xattribute.Value.ToString())
					{
						xattribute.SetValue(info.Android_PackageName);
						xelement.Save(info.AndroidManifest);
						break;
					}
				}
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
				return false;
			}
			return true;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003AC8 File Offset: 0x00001CC8
		protected override string OnCreateConsoleArguments(EnumOperationType opType, PackageParams prms)
		{
			string sourcePath = Path.Combine(prms.Directory, "frameworks", "runtime-src", "proj.android");
			string value = base.CreateGeneralArguments(opType, sourcePath, prms.IsDebugKeystore);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value);
			stringBuilder.Append(string.Format(" -a {0}", prms.AndroidVersion));
			stringBuilder.Append(" --ndk-mode release");
			stringBuilder.Append(" -j 3");
			stringBuilder.Append(" --compile-script 1");
			if (prms.LuaIsEncrypt)
			{
				stringBuilder.Append(" --lua-encrypt 1");
				stringBuilder.Append(string.Format(" --lua-encrypt-key {0}", prms.LuaEncryptKey));
				stringBuilder.Append(string.Format(" --lua-encrypt-sign {0}", prms.LuaEncryptSign));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000026 RID: 38
		private const string androidManifestKey = "package";
	}
}
