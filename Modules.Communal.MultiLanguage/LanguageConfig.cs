using System;
using CocoStudio.Basic;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.MultiLanguage
{
	// Token: 0x02000004 RID: 4
	[Extension(typeof(IUserConfig))]
	internal class LanguageConfig : IUserConfig
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000021B0 File Offset: 0x000003B0
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000021C7 File Offset: 0x000003C7
		[ItemProperty("LanguageType/Value")]
		public LanguageType LanguageType { get; set; }

		// Token: 0x04000005 RID: 5
		public const string conifgKey = "CCS_LanguageConfig";
	}
}
