using System;
using Gtk;

namespace Modules.Communal.Preference
{
	public interface IPreferenceWidget
	{
		EnumPreferenceSetting SettingID { get; }

		string DisplayName { get; }

		void ApplySetting();

		bool CanApply(out string output);

		Widget GetWidget();
	}
}
