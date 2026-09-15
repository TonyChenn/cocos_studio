using System;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.UserStatistics;
using Gtk;
using Mono.Addins;
using MonoDevelop.Ide;

namespace Modules.Communal.ProjectSetting
{
	[Extension(Type = typeof(ICommandHandle))]
	public class ProjectSettingUC : ICommandHandle
	{
		public void Initialize()
		{
			GlobalCommand.ProjectSettingCmd.Execute += this.ProjectSettingCmd_Execute;
			GlobalCommand.ProjectSettingCmd.Update += this.ProjectSettingCmd_CanExecute;
		}

		private void ProjectSettingCmd_CanExecute(object sender, CommandUpdateArgs e)
		{
			if (Services.ProjectsService.CurrentSolution == null)
			{
				e.Info.Enabled = false;
				return;
			}
			e.Info.Enabled = true;
		}

		private void ProjectSettingCmd_Execute(object sender, CommandRunArgs e)
		{
			EnumProjectSetting initWidget = EnumProjectSetting.Default;
			if (e.DataItem != null && !Enum.TryParse<EnumProjectSetting>(e.DataItem.ToString(), out initWidget))
			{
				initWidget = EnumProjectSetting.Default;
			}
			Window defaultModalParent = MessageService.GetDefaultModalParent();
			bool modal = defaultModalParent.Modal;
			defaultModalParent.Modal = false;
			ProjectSettingDialog projectSettingDialog = new ProjectSettingDialog(initWidget);
			projectSettingDialog.Run();
			projectSettingDialog.Destroy();
			defaultModalParent.Modal = modal;
			Tracker.Add(ViewRegions.ResourcePanel, "RightMenuProjectSetting", "", "");
		}
	}
}
