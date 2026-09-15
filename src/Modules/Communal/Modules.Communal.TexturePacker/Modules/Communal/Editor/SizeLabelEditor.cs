using System;
using CocoStudio.Model.Editor;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Editor
{
	internal class SizeLabelEditor : BaseEditor
	{
		protected override Widget OnCreateWidget()
		{
			this.label = new Label();
			HBox hbox = new HBox();
			hbox.PackStart(this.label, false, false, 0U);
			hbox.ShowAll();
			base.SetControl();
			return hbox;
		}

		protected override void OnSetControl()
		{
			SizeValue sizeValue = (SizeValue)base.PropertyItem.Values[0];
			this.label.Text = string.Format("{0}{2} * {1}{2}", sizeValue.Width, sizeValue.Height, LanguageInfo.NewFile_Pixel);
		}

		private Label label;
	}
}
