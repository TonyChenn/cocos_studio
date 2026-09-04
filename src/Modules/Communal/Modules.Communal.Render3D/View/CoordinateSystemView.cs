using System;
using CocoStudio.Core;
using CocoStudio.Lib.Prism;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.Render3D.View
{
	// Token: 0x0200000D RID: 13
	internal class CoordinateSystemView : HBox
	{
		// Token: 0x06000058 RID: 88 RVA: 0x00002E78 File Offset: 0x00001078
		public CoordinateSystemView()
		{
			ControlNode3D.Instance.CoordinateSystemChanged += this.CoordinateSystemChangedEvent;
			this.eventAggregator = Services.EventsService;
			this.InitView();
			this.InitEvent();
			base.ShowAll();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002EB4 File Offset: 0x000010B4
		private void CoordinateSystemChangedEvent(object sender, EventArgs e)
		{
			ControlNode3D controlNode3D = sender as ControlNode3D;
			bool sensitive = true;
			if (controlNode3D.Operate == ControlNode3D.Opt.Scale && controlNode3D.SelectObjectList.Count <= 1)
			{
				sensitive = false;
			}
			this.button_StatusImage1.Sensitive = sensitive;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002EF0 File Offset: 0x000010F0
		private void InitView()
		{
			base.Spacing = -1;
			this.button_StatusImage = new StatusImageButton("Pivot", LanguageInfo.MainTool_Pivot, ImageIcon.GetIcon("Modules.Communal.Render3D.Images.Pivot.png"), "Center", LanguageInfo.MainTool_Center, ImageIcon.GetIcon("Modules.Communal.Render3D.Images.Center.png"));
			base.PackStart(this.button_StatusImage, true, false, 0U);
			this.button_StatusImage1 = new StatusImageButton("Local", LanguageInfo.MainTool_Local, ImageIcon.GetIcon("Modules.Communal.Render3D.Images.Local.png"), "Global", LanguageInfo.MainTool_Global, ImageIcon.GetIcon("Modules.Communal.Render3D.Images.Global.png"));
			base.PackStart(this.button_StatusImage1, true, false, 0U);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002F88 File Offset: 0x00001188
		private void InitEvent()
		{
			this.button_StatusImage.ButtonReleaseEvent += this.button_StatusImage_ButtonReleaseEvent;
			this.button_StatusImage1.ButtonReleaseEvent += this.button_StatusImage1_ButtonReleaseEvent;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002FB8 File Offset: 0x000011B8
		private void button_StatusImage_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			int currentChoice = this.button_StatusImage.CurrentChoice;
			ControlNode3D.Instance.OptionPoint = (ControlNode3D.PivotPoint)currentChoice;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002FDC File Offset: 0x000011DC
		private void button_StatusImage1_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			int currentChoice = this.button_StatusImage1.CurrentChoice;
			ControlNode3D.Instance.OptitonSpace = (ControlNode3D.Space)currentChoice;
		}

		// Token: 0x04000011 RID: 17
		private IEventAggregator eventAggregator;

		// Token: 0x04000012 RID: 18
		private StatusImageButton button_StatusImage;

		// Token: 0x04000013 RID: 19
		private StatusImageButton button_StatusImage1;
	}
}
