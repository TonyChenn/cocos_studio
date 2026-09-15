using System;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.CocosAdapter.Platform
{
	[Extension(typeof(IPlatform))]
	internal class WindowsPlatform : BasePlatform
	{
		public override EnumPlatform PlatformType
		{
			get
			{
				return EnumPlatform.Windows;
			}
		}

		public override int Order
		{
			get
			{
				return 0;
			}
		}

		protected override string PlatformName
		{
			get
			{
				return "win32";
			}
		}

		public override string GetDisplayName(EnumOperationType opType)
		{
			if (opType == EnumOperationType.Run)
			{
				return LanguageInfo.Run_Windows;
			}
			return string.Empty;
		}

		public override bool CanShow(EnumOperationType opType)
		{
			return opType == EnumOperationType.Run && MonoDevelop.Core.Platform.IsWindows;
		}

		protected override bool OnCanExecute(EnumOperationType opType, PackageParams prms)
		{
			return opType == EnumOperationType.Run;
		}

		protected override string OnCreateConsoleArguments(EnumOperationType opType, PackageParams prms)
		{
			return base.CreateGeneralArguments(opType, prms.Directory, false);
		}
	}
}
