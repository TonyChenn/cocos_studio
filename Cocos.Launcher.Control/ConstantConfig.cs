using System;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Control
{
	// Token: 0x02000002 RID: 2
	public static class ConstantConfig
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		public static IConstsLink Constant { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205F File Offset: 0x0000025F
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002066 File Offset: 0x00000266
		public static ConstsColor Colors { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000206E File Offset: 0x0000026E
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002075 File Offset: 0x00000275
		public static ConstsPath Paths { get; private set; }

		// Token: 0x06000007 RID: 7 RVA: 0x00002080 File Offset: 0x00000280
		static ConstantConfig()
		{
			if (LanguageOption.CurrentLanguage == LanguageType.Chinese)
			{
				ConstantConfig.Constant = new ConstsLink();
			}
			else if (LanguageOption.CurrentLanguage == LanguageType.Traditional)
			{
				ConstantConfig.Constant = new ConstsLinkZhTW();
			}
			else
			{
				ConstantConfig.Constant = new ENConstsLink();
			}
			ConstantConfig.Colors = new ConstsColor();
			ConstantConfig.Paths = new ConstsPath();
		}
	}
}
