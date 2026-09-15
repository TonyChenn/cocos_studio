using System;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.UserStatistics;
using Gtk;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.UI.MainTool
{
	internal class SimulatorButtonWidget : BaseToolbarWidget
	{
		public override Widget GtkWidget
		{
			get
			{
				return this.mainEventBox;
			}
		}

		public SimulatorButtonWidget()
		{
			Xwt.Drawing.Image icon = ImageIcon.GetIcon("Modules.UI.MainTool.Images.stop.png");
			Xwt.Drawing.Image icon2 = ImageIcon.GetIcon("Modules.UI.MainTool.Images.play.png");
			this.button_play = new IconToggleButton(icon2, icon);
			this.button_play.HasTooltip = true;
			this.button_play.TooltipText = LanguageInfo.MainTool_Preview;
			this.button_play.Sensitive = false;
			this.button_play.CheckChanged += this.PlayButtonToggledHandler;
			SimulatorControl.Instance.StateChanged += this.SimulatorStateChangedHandler;
			this.mainEventBox = new EventBox();
			this.mainEventBox.VisibleWindow = false;
			this.mainEventBox.Add(this.button_play);
			this.mainEventBox.Show();
		}

		private void PlayButtonToggledHandler(object sender, EventArgs e)
		{
			this.button_play.Sensitive = false;
			if (this.button_play.IsChecked)
			{
				Tracker.Add(ViewRegions.UIMainTool, "RunSimulator", "", "");
				if (!SimulatorControl.Instance.Play())
				{
					this.button_play.IsChecked = false;
				}
			}
			else if (SimulatorControl.Instance.CanStop)
			{
				SimulatorControl.Instance.Stop();
			}
			this.RefreshTooltip();
		}

		private void RefreshTooltip()
		{
			if (this.button_play.IsChecked)
			{
				this.button_play.TooltipText = LanguageInfo.Command_Stop;
			}
			else
			{
				this.button_play.TooltipText = LanguageInfo.MainTool_Preview;
			}
		}

		private void SimulatorStateChangedHandler(object sender, StateChangedEventArgs e)
		{
			if (e.State.Equals("Play"))
			{
				this.button_play.IsChecked = true;
			}
			else
			{
				this.button_play.IsChecked = false;
			}
			this.button_play.Sensitive = SimulatorControl.Instance.CanPlay;
			this.RefreshTooltip();
		}

		public override void OnSolutionClosed(SolutionEventArgs args)
		{
			this.button_play.Sensitive = false;
		}

		public override void OnProjectChanged(ProjectsOperations.ProjectEventArgs args)
		{
			this.button_play.Sensitive = SimulatorControl.Instance.CanPlay;
		}

		private IconToggleButton button_play;

		private EventBox mainEventBox;
	}
}
