using System;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Status;

namespace Modules.Communal.Preference
{
	// Token: 0x0200000B RID: 11
	internal class EnvironmentWarningInfo : IStatusWarningInfo
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002565 File Offset: 0x00000765
		public string Tooltip
		{
			get
			{
				return LanguageInfo.StatusBar_ConfigIncomplete;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000256C File Offset: 0x0000076C
		public void OnClick()
		{
			PreferenceManager.Instance.OpenPreferenceDialog(EnumPreferenceSetting.Platform);
		}
	}
}
