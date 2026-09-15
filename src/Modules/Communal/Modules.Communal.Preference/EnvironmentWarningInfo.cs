using System;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Status;

namespace Modules.Communal.Preference
{
	internal class EnvironmentWarningInfo : IStatusWarningInfo
	{
		public string Tooltip
		{
			get
			{
				return LanguageInfo.StatusBar_ConfigIncomplete;
			}
		}

		public void OnClick()
		{
			PreferenceManager.Instance.OpenPreferenceDialog(EnumPreferenceSetting.Platform);
		}
	}
}
