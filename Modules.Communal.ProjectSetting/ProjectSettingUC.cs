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
	// Token: 0x02000005 RID: 5
	[Extension(Type = typeof(ICommandHandle))]
	public class ProjectSettingUC : ICommandHandle
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000021D4 File Offset: 0x000003D4
		public void Initialize()
		{
			GlobalCommand.ProjectSettingCmd.Execute += this.ProjectSettingCmd_Execute;
			GlobalCommand.ProjectSettingCmd.Update += this.ProjectSettingCmd_CanExecute;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002202 File Offset: 0x00000402
		private void ProjectSettingCmd_CanExecute(object sender, CommandUpdateArgs e)
		{
			if (Services.ProjectsService.CurrentSolution == null)
			{
				e.Info.Enabled = false;
				return;
			}
			e.Info.Enabled = true;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000222C File Offset: 0x0000042C
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
