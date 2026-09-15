using System;
using System.IO;
using CocoStudio.Basic;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.CocosAdapter.Platform
{
	[Extension(typeof(IPlatform))]
	internal class MacPlatform : BasePlatform
	{
		public override EnumPlatform PlatformType
		{
			get
			{
				return EnumPlatform.Mac;
			}
		}

		public override int Order
		{
			get
			{
				return 1;
			}
		}

		protected override string PlatformName
		{
			get
			{
				return "mac";
			}
		}

		public override string GetDisplayName(EnumOperationType opType)
		{
			if (opType == EnumOperationType.Run)
			{
				return LanguageInfo.Run_Mac;
			}
			return string.Empty;
		}

		public override bool CanShow(EnumOperationType opType)
		{
			return opType == EnumOperationType.Run && MonoDevelop.Core.Platform.IsMac;
		}

		protected override bool OnCanExecute(EnumOperationType opType, PackageParams prms)
		{
			return opType == EnumOperationType.Run;
		}

		protected override bool OnExecuteInitialize(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			EnumProgramLanguage programLanguage = Cocos2dxServices.CocosProperties.ProgramLanguage;
			string text;
			if (programLanguage == EnumProgramLanguage.js || programLanguage == EnumProgramLanguage.lua)
			{
				text = Path.Combine(new string[]
				{
					prms.Directory,
					"frameworks",
					"runtime-src",
					"proj.ios_mac",
					"build"
				});
			}
			else
			{
				text = Path.Combine(prms.Directory, "proj.ios_mac", "build");
			}
			if (Directory.Exists(text))
			{
				try
				{
					Directory.Delete(text, true);
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error(string.Format("删除路径{0}失败", text), exception);
					return false;
				}
				return true;
			}
			return true;
		}

		protected override string OnCreateConsoleArguments(EnumOperationType opType, PackageParams prms)
		{
			return base.CreateGeneralArguments(opType, prms.Directory, false);
		}
	}
}
