using System;
using System.IO;
using CocoStudio.Basic;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.CocosAdapter.Platform
{
	// Token: 0x02000013 RID: 19
	[Extension(typeof(IPlatform))]
	internal class MacPlatform : BasePlatform
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003D95 File Offset: 0x00001F95
		public override EnumPlatform PlatformType
		{
			get
			{
				return EnumPlatform.Mac;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003D99 File Offset: 0x00001F99
		public override int Order
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003D9C File Offset: 0x00001F9C
		protected override string PlatformName
		{
			get
			{
				return "mac";
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003DA3 File Offset: 0x00001FA3
		public override string GetDisplayName(EnumOperationType opType)
		{
			if (opType == EnumOperationType.Run)
			{
				return LanguageInfo.Run_Mac;
			}
			return string.Empty;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003DB4 File Offset: 0x00001FB4
		public override bool CanShow(EnumOperationType opType)
		{
			return opType == EnumOperationType.Run && MonoDevelop.Core.Platform.IsMac;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003DC1 File Offset: 0x00001FC1
		protected override bool OnCanExecute(EnumOperationType opType, PackageParams prms)
		{
			return opType == EnumOperationType.Run;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003DCC File Offset: 0x00001FCC
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

		// Token: 0x06000090 RID: 144 RVA: 0x00003E8C File Offset: 0x0000208C
		protected override string OnCreateConsoleArguments(EnumOperationType opType, PackageParams prms)
		{
			return base.CreateGeneralArguments(opType, prms.Directory, false);
		}
	}
}
