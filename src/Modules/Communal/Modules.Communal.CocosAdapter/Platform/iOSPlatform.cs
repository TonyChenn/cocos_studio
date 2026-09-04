using System;
using System.IO;
using System.Text;
using CocoStudio.Basic;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.CocosAdapter.Platform
{
	// Token: 0x02000012 RID: 18
	[Extension(typeof(IPlatform))]
	internal class iOSPlatform : BasePlatform
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003B9D File Offset: 0x00001D9D
		public override EnumPlatform PlatformType
		{
			get
			{
				return EnumPlatform.iOS;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003BA0 File Offset: 0x00001DA0
		public override int Order
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003BA3 File Offset: 0x00001DA3
		protected override string PlatformName
		{
			get
			{
				return "ios";
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003BAA File Offset: 0x00001DAA
		public override bool CanShow(EnumOperationType opType)
		{
			return opType == EnumOperationType.Package && MonoDevelop.Core.Platform.IsMac;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003BBA File Offset: 0x00001DBA
		protected override bool OnCanExecute(EnumOperationType opType, PackageParams prms)
		{
			return opType == EnumOperationType.Package;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003BC4 File Offset: 0x00001DC4
		public override string GetDisplayName(EnumOperationType opType)
		{
			switch (opType)
			{
			case EnumOperationType.Package:
				return LanguageInfo.Dialog_iOSPackage;
			case EnumOperationType.Run:
				return LanguageInfo.Run_iOS;
			default:
				return string.Empty;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003BF8 File Offset: 0x00001DF8
		protected override bool OnExecuteInitialize(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			if (string.IsNullOrEmpty(prms.iOS_BundleID))
			{
				monitor.SendInfo("0 valid identities found!");
				return false;
			}
			string text = prms.ProjectName + ".xcodeproj";
			string arg = Path.Combine(new string[]
			{
				prms.Directory,
				"frameworks",
				"runtime-src",
				"proj.ios_mac",
				text
			});
			string command_line = string.Format("xcodebuild -project \"{0}\" clean", arg);
			try
			{
				string text2;
				string text3;
				int num;
				if (!Process.SpawnCommandLineSync(command_line, out text2, out text3, out num))
				{
					monitor.SendInfo("Failed to clean project");
					return false;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("清理项目时出错", exception);
				monitor.SendInfo("Failed to clean project");
				return false;
			}
			return true;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003CD8 File Offset: 0x00001ED8
		protected override string OnCreateConsoleArguments(EnumOperationType opType, PackageParams prms)
		{
			string text = prms.ProjectName + ".xcodeproj";
			string sourcePath = Path.Combine(new string[]
			{
				prms.Directory,
				"frameworks",
				"runtime-src",
				"proj.ios_mac",
				text
			});
			string value = base.CreateGeneralArguments(opType, sourcePath, false);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value);
			stringBuilder.Append(string.Format(" -t \"{0}\"", prms.iOS_Target));
			stringBuilder.Append(string.Format(" --sign-identity \"{0}\"", prms.iOS_BundleID));
			stringBuilder.Append(" --compile-script 1");
			return stringBuilder.ToString();
		}

		// Token: 0x04000027 RID: 39
		private const string extensionName = ".xcodeproj";
	}
}
