using System;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class CheckBoxEditor : BaseEditor
	{
		private void choice_Clicked(object sender, EventArgs e)
		{
			base.UpdatePropertyValue(this.choice.Active, null);
		}

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

		private RadioButton choice;

		private RadioButton unChoice;
	}
}
