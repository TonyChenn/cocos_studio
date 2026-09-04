using System;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.CocosAdapter.Platform
{
	// Token: 0x02000017 RID: 23
	[Extension(typeof(IPlatform))]
	internal class WindowsPlatform : BasePlatform
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000048B4 File Offset: 0x00002AB4
		public override EnumPlatform PlatformType
		{
			get
			{
				return EnumPlatform.Windows;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000048B7 File Offset: 0x00002AB7
		public override int Order
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000048BA File Offset: 0x00002ABA
		protected override string PlatformName
		{
			get
			{
				return "win32";
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000048C1 File Offset: 0x00002AC1
		public override string GetDisplayName(EnumOperationType opType)
		{
			if (opType == EnumOperationType.Run)
			{
				return LanguageInfo.Run_Windows;
			}
			return string.Empty;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000048D2 File Offset: 0x00002AD2
		public override bool CanShow(EnumOperationType opType)
		{
			return opType == EnumOperationType.Run && MonoDevelop.Core.Platform.IsWindows;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000048DF File Offset: 0x00002ADF
		protected override bool OnCanExecute(EnumOperationType opType, PackageParams prms)
		{
			return opType == EnumOperationType.Run;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000048E8 File Offset: 0x00002AE8
		protected override string OnCreateConsoleArguments(EnumOperationType opType, PackageParams prms)
		{
			return base.CreateGeneralArguments(opType, prms.Directory, false);
		}
	}
}
