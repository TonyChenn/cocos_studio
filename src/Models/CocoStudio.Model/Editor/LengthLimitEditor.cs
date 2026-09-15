using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class LengthLimitEditor : BaseEditor
	{
		protected override Widget OnCreateWidget()
		{
			HBox hbox = new HBox();
			hbox.Spacing = 4;
			HBox hbox2 = new HBox();
			this.enableLimitCheckBox = new CheckButton();
			hbox2.PackStart(new Label(), false, false, 0U);
			hbox2.PackStart(this.enableLimitCheckBox, false, false, 0U);
			hbox2.Spacing = -4;
			hbox.PackStart(hbox2, false, false, 0U);
			this.lengthEntry = new NoUndoNumEntry();
			this.lengthEntry.WidthRequest = 10;
			this.lengthEntry.CanFocus = true;
			this.lengthEntry.IsInteger = true;
			this.lengthEntry.MinValue = 1;
			hbox.PackStart(this.lengthEntry);
			this.enableLimitCheckBox.Toggled += this.CheckBoxToggledHandler;
			this.lengthEntry.EntryValueChanged += this.EntryValueChangedHandler;
			base.SetControl();
			hbox.ShowAll();
			return hbox;
		}

		protected override void OnSetControl()
		{
			AstrictLengthValue value = base.PropertyItem.GetValue<AstrictLengthValue>(0);
			this.enableLimitCheckBox.Active = value.MaxLengthEnable;
			this.lengthEntry.Sensitive = value.MaxLengthEnable;
			this.lengthEntry.Value = (float)value.MaxLengthText;
		}

		private void EntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			AstrictLengthValue value = base.PropertyItem.GetValue<AstrictLengthValue>(0);
			value.MaxLengthText = (int)e.Value;
			base.UpdatePropertyValue(value, null);
		}

		private void CheckBoxToggledHandler(object sender, EventArgs e)
		{
			AstrictLengthValue value = base.PropertyItem.GetValue<AstrictLengthValue>(0);
			value.MaxLengthEnable = this.enableLimitCheckBox.Active;
			this.lengthEntry.Sensitive = this.enableLimitCheckBox.Active;
			base.UpdatePropertyValue(value, null);
		}

		private CheckButton enableLimitCheckBox;

		private NoUndoNumEntry lengthEntry;
	}
}
