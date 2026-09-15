using System;
using System.ComponentModel;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Editor
{
	internal class ExportButtonEditor : BaseEditor
	{
		protected override Widget OnCreateWidget()
		{
			Button button = new Button();
			button.WidthRequest = 130;
			button.Label = LanguageInfo.Command_ExportMergeImage;
			button.Clicked += this.ButtonClickedHandler;
			HBox hbox = new HBox();
			hbox.PackStart(button, false, false, 0U);
			hbox.ShowAll();
			return hbox;
		}

		private void ButtonClickedHandler(object sender, EventArgs e)
		{
			base.UpdatePropertyValue("", null);
			Services.MainWindow.Present();
		}

		protected override void OnSetControl()
		{
		}

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
		}
	}
}
