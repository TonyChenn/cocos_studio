using System;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.UserStatistics;
using Gtk;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.UI.MainTool
{
	// Token: 0x0200000C RID: 12
	internal class SimulatorButtonWidget : BaseToolbarWidget
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003764 File Offset: 0x00001964
		public override Widget GtkWidget
		{
			get
			{
				return this.mainEventBox;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000377C File Offset: 0x0000197C
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

		// Token: 0x0600004A RID: 74 RVA: 0x00003848 File Offset: 0x00001A48
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

		// Token: 0x0600004B RID: 75 RVA: 0x000038D4 File Offset: 0x00001AD4
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

		// Token: 0x0600004C RID: 76 RVA: 0x00003918 File Offset: 0x00001B18
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

		// Token: 0x0600004D RID: 77 RVA: 0x0000397A File Offset: 0x00001B7A
		public override void OnSolutionClosed(SolutionEventArgs args)
		{
			this.button_play.Sensitive = false;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000398A File Offset: 0x00001B8A
		public override void OnProjectChanged(ProjectsOperations.ProjectEventArgs args)
		{
			this.button_play.Sensitive = SimulatorControl.Instance.CanPlay;
		}

		// Token: 0x0400001F RID: 31
		private IconToggleButton button_play;

		// Token: 0x04000020 RID: 32
		private EventBox mainEventBox;
	}
}
