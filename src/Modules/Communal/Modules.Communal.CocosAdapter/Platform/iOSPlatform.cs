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
	[Extension(typeof(IPlatform))]
	internal class iOSPlatform : BasePlatform
	{
		public override EnumPlatform PlatformType
		{
			get
			{
				return EnumPlatform.iOS;
			}
		}

		public override int Order
		{
			get
			{
				return 3;
			}
		}

		protected override string PlatformName
		{
			get
			{
				return "ios";
			}
		}

		public override bool CanShow(EnumOperationType opType)
		{
			return opType == EnumOperationType.Package && MonoDevelop.Core.Platform.IsMac;
		}

		protected override bool OnCanExecute(EnumOperationType opType, PackageParams prms)
		{
			return opType == EnumOperationType.Package;
		}

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

		private const string extensionName = ".xcodeproj";
	}
}
