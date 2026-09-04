using System;
using Gtk;

namespace Modules.Communal.Preference
{
	// Token: 0x02000006 RID: 6
	public interface IPreferenceWidget
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000D RID: 13
		EnumPreferenceSetting SettingID { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000E RID: 14
		string DisplayName { get; }

		// Token: 0x0600000F RID: 15
		void ApplySetting();

		// Token: 0x06000010 RID: 16
		bool CanApply(out string output);

		// Token: 0x06000011 RID: 17
		Widget GetWidget();
	}
}
