using System;
using System.ComponentModel;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Editor
{
	// Token: 0x02000002 RID: 2
	internal class ExportButtonEditor : BaseEditor
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
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

		// Token: 0x06000002 RID: 2 RVA: 0x000020A2 File Offset: 0x000002A2
		private void ButtonClickedHandler(object sender, EventArgs e)
		{
			base.UpdatePropertyValue("", null);
			Services.MainWindow.Present();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020BA File Offset: 0x000002BA
		protected override void OnSetControl()
		{
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020BC File Offset: 0x000002BC
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
		}
	}
}
