using System;
using CocoStudio.Core;
using CocoStudio.Lib.Prism;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.Render3D.View
{
	internal class CoordinateSystemView : HBox
	{
		public CoordinateSystemView()
		{
			ControlNode3D.Instance.CoordinateSystemChanged += this.CoordinateSystemChangedEvent;
			this.eventAggregator = Services.EventsService;
			this.InitView();
			this.InitEvent();
			base.ShowAll();
		}

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

		private void InitView()
		{
			base.Spacing = -1;
			this.button_StatusImage = new StatusImageButton("Pivot", LanguageInfo.MainTool_Pivot, ImageIcon.GetIcon("Modules.Communal.Render3D.Images.Pivot.png"), "Center", LanguageInfo.MainTool_Center, ImageIcon.GetIcon("Modules.Communal.Render3D.Images.Center.png"));
			base.PackStart(this.button_StatusImage, true, false, 0U);
			this.button_StatusImage1 = new StatusImageButton("Local", LanguageInfo.MainTool_Local, ImageIcon.GetIcon("Modules.Communal.Render3D.Images.Local.png"), "Global", LanguageInfo.MainTool_Global, ImageIcon.GetIcon("Modules.Communal.Render3D.Images.Global.png"));
			base.PackStart(this.button_StatusImage1, true, false, 0U);
		}

		private void InitEvent()
		{
			this.button_StatusImage.ButtonReleaseEvent += this.button_StatusImage_ButtonReleaseEvent;
			this.button_StatusImage1.ButtonReleaseEvent += this.button_StatusImage1_ButtonReleaseEvent;
		}

		private void button_StatusImage_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			int currentChoice = this.button_StatusImage.CurrentChoice;
			ControlNode3D.Instance.OptionPoint = (ControlNode3D.PivotPoint)currentChoice;
		}

		private void button_StatusImage1_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			int currentChoice = this.button_StatusImage1.CurrentChoice;
			ControlNode3D.Instance.OptitonSpace = (ControlNode3D.Space)currentChoice;
		}

		private IEventAggregator eventAggregator;

		private StatusImageButton button_StatusImage;

		private StatusImageButton button_StatusImage1;
	}
}
