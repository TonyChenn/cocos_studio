using System;
using CocoStudio.Model.Editor;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Editor
{
	// Token: 0x02000011 RID: 17
	internal class SizeLabelEditor : BaseEditor
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00004ED0 File Offset: 0x000030D0
		protected override Widget OnCreateWidget()
		{
			this.label = new Label();
			HBox hbox = new HBox();
			hbox.PackStart(this.label, false, false, 0U);
			hbox.ShowAll();
			base.SetControl();
			return hbox;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004F0C File Offset: 0x0000310C
		protected override void OnSetControl()
		{
			SizeValue sizeValue = (SizeValue)base.PropertyItem.Values[0];
			this.label.Text = string.Format("{0}{2} * {1}{2}", sizeValue.Width, sizeValue.Height, LanguageInfo.NewFile_Pixel);
		}

		// Token: 0x0400003A RID: 58
		private Label label;
	}
}
