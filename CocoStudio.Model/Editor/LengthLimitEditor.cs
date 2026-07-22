using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200008D RID: 141
	internal class LengthLimitEditor : BaseEditor
	{
		// Token: 0x060004DF RID: 1247 RVA: 0x00015220 File Offset: 0x00013420
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

		// Token: 0x060004E0 RID: 1248 RVA: 0x00015310 File Offset: 0x00013510
		protected override void OnSetControl()
		{
			AstrictLengthValue value = base.PropertyItem.GetValue<AstrictLengthValue>(0);
			this.enableLimitCheckBox.Active = value.MaxLengthEnable;
			this.lengthEntry.Sensitive = value.MaxLengthEnable;
			this.lengthEntry.Value = (float)value.MaxLengthText;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00015364 File Offset: 0x00013564
		private void EntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			AstrictLengthValue value = base.PropertyItem.GetValue<AstrictLengthValue>(0);
			value.MaxLengthText = (int)e.Value;
			base.UpdatePropertyValue(value, null);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00015398 File Offset: 0x00013598
		private void CheckBoxToggledHandler(object sender, EventArgs e)
		{
			AstrictLengthValue value = base.PropertyItem.GetValue<AstrictLengthValue>(0);
			value.MaxLengthEnable = this.enableLimitCheckBox.Active;
			this.lengthEntry.Sensitive = this.enableLimitCheckBox.Active;
			base.UpdatePropertyValue(value, null);
		}

		// Token: 0x04000241 RID: 577
		private CheckButton enableLimitCheckBox;

		// Token: 0x04000242 RID: 578
		private NoUndoNumEntry lengthEntry;
	}
}
