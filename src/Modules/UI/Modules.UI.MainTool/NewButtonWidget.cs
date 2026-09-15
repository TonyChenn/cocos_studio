using System;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using CocoStudio.Model;
using CocoStudio.UserStatistics;
using Gtk;

namespace Modules.UI.MainTool
{
	internal class NewButtonWidget : BaseToolbarWidget
	{
		public override Widget GtkWidget
		{
			get
			{
				return this.newFileButton;
			}
		}

		public NewButtonWidget()
		{
			this.newFileButton = new IconTrackPointButton(ImageIcon.GetIcon("Modules.UI.MainTool.Images.NewFile.png"));
			this.newFileButton.TooltipText = GlobalCommand.NewFileCmd.GetTooltipText(true);
			this.newFileButton.Show();
			this.newFileButton.Sensitive = GlobalCommand.NewFileCmd.IsEnable;
			this.newFileButton.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.ButtonNewFileClickedHandler);
			this.RefreshUI();
		}

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

		public override void OnSolutionChanged(SolutionEventArgs args)
		{
			this.newFileButton.Sensitive = GlobalCommand.NewFileCmd.IsEnable;
		}

		public override void OnProjectChanged(ProjectsOperations.ProjectEventArgs args)
		{
			this.RefreshUI();
		}

		private void ButtonNewFileClickedHandler(object sender, EventArgs e)
		{
			Services.CommandService.CurrentCommandSource = ViewRegions.UIMainTool;
			GlobalCommand.NewFileCmd.RaiseExecute(null);
			this.RefreshUI();
		}

		private IconTrackPointButton newFileButton;
	}
}
