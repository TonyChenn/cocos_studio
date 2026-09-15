using System;
using System.Collections.Generic;
using Gtk;

namespace Modules.Communal.ProjectSetting
{
	public interface IProjectSettingWidget
	{
		EnumProjectSetting SettingID { get; }

		string DisplayName { get; }

		void ApplySetting();

		bool CanApply(out string output);

		Widget GetWidget();

		List<IProjectSettingWidget> SubWidgets { get; }
	}
}
