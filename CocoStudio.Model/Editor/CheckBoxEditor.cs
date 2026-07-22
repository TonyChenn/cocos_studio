using System;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200008E RID: 142
	internal class CheckBoxEditor : BaseEditor
	{
		// Token: 0x060004E4 RID: 1252 RVA: 0x000153ED File Offset: 0x000135ED
		private void choice_Clicked(object sender, EventArgs e)
		{
			base.UpdatePropertyValue(this.choice.Active, null);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00015408 File Offset: 0x00013608
		protected override Widget OnCreateWidget()
		{
			this.choice = new RadioButton(LanguageInfo.Display_NormalState);
			this.unChoice = new RadioButton(LanguageInfo.Display_Disable);
			this.choice.CanFocus = false;
			this.choice.DrawIndicator = true;
			this.choice.UseUnderline = true;
			this.unChoice.CanFocus = false;
			this.unChoice.DrawIndicator = true;
			this.unChoice.UseUnderline = true;
			base.SetControl();
			this.choice.Clicked += this.choice_Clicked;
			HBox hbox = new HBox();
			hbox.Spacing = 6;
			hbox.PackStart(this.choice, false, false, 0U);
			hbox.PackStart(this.unChoice, false, false, 0U);
			VBox vbox = new VBox();
			vbox.Add(new Alignment(0.5f, 0.5f, 1f, 1f));
			vbox.PackStart(hbox, false, false, 0U);
			vbox.Add(new Alignment(0.5f, 0.5f, 1f, 1f));
			vbox.HeightRequest = 24;
			vbox.ShowAll();
			return vbox;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00015538 File Offset: 0x00013738
		protected override void OnSetControl()
		{
			object obj = base.PropertyItem.Values[0];
			if ((bool)obj)
			{
				this.choice.Group = new SList(IntPtr.Zero);
				this.unChoice.Group = this.choice.Group;
			}
			else
			{
				this.unChoice.Group = new SList(IntPtr.Zero);
				this.choice.Group = this.unChoice.Group;
			}
		}

		// Token: 0x04000243 RID: 579
		private RadioButton choice;

		// Token: 0x04000244 RID: 580
		private RadioButton unChoice;
	}
}
