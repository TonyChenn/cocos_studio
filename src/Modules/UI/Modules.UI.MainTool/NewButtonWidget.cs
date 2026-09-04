using System;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using CocoStudio.Model;
using CocoStudio.UserStatistics;
using Gtk;

namespace Modules.UI.MainTool
{
	// Token: 0x02000008 RID: 8
	internal class NewButtonWidget : BaseToolbarWidget
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002E1C File Offset: 0x0000101C
		public override Widget GtkWidget
		{
			get
			{
				return this.newFileButton;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002E34 File Offset: 0x00001034
		public NewButtonWidget()
		{
			this.newFileButton = new IconTrackPointButton(ImageIcon.GetIcon("Modules.UI.MainTool.Images.NewFile.png"));
			this.newFileButton.TooltipText = GlobalCommand.NewFileCmd.GetTooltipText(true);
			this.newFileButton.Show();
			this.newFileButton.Sensitive = GlobalCommand.NewFileCmd.IsEnable;
			this.newFileButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.ButtonNewFileClickedHandler);
			this.RefreshUI();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002EB8 File Offset: 0x000010B8
		private void RefreshUI()
		{
			bool isShowTrackPoint = false;
			foreach (IProjectFileCreator projectFileCreator in ProjectFileTemplateService.ProjectFileTemplateList)
			{
				if (projectFileCreator.IsShowTrackPoint)
				{
					isShowTrackPoint = true;
					break;
				}
			}
			this.newFileButton.IsShowTrackPoint = isShowTrackPoint;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002F2C File Offset: 0x0000112C
		public override void OnSolutionChanged(SolutionEventArgs args)
		{
			this.newFileButton.Sensitive = GlobalCommand.NewFileCmd.IsEnable;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002F45 File Offset: 0x00001145
		public override void OnProjectChanged(ProjectsOperations.ProjectEventArgs args)
		{
			this.RefreshUI();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002F4F File Offset: 0x0000114F
		private void ButtonNewFileClickedHandler(object sender, EventArgs e)
		{
			Services.CommandService.CurrentCommandSource = ViewRegions.UIMainTool;
			GlobalCommand.NewFileCmd.RaiseExecute(null);
			this.RefreshUI();
		}

		// Token: 0x04000010 RID: 16
		private IconTrackPointButton newFileButton;
	}
}
