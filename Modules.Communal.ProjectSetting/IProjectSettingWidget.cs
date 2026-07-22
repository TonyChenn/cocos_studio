using System;
using System.Collections.Generic;
using Gtk;

namespace Modules.Communal.ProjectSetting
{
	// Token: 0x02000003 RID: 3
	public interface IProjectSettingWidget
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		EnumProjectSetting SettingID { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2
		string DisplayName { get; }

		// Token: 0x06000003 RID: 3
		void ApplySetting();

		// Token: 0x06000004 RID: 4
		bool CanApply(out string output);

		// Token: 0x06000005 RID: 5
		Widget GetWidget();

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6
		List<IProjectSettingWidget> SubWidgets { get; }
	}
}
