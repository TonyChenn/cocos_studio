using System;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000093 RID: 147
	internal class PasswordEditor : BaseEditor
	{
		// Token: 0x0600050A RID: 1290 RVA: 0x00015C2C File Offset: 0x00013E2C
		protected override Widget OnCreateWidget()
		{
			this.checkBtn = new CheckButton();
			this.checkBtn.DrawIndicator = true;
			this.checkBtn.Toggled += this.CheckButtonToggledHandler;
			HBox hbox = new HBox();
			hbox.PackStart(new Label(), false, false, 0U);
			hbox.PackStart(this.checkBtn, false, false, 0U);
			hbox.Spacing = -4;
			this.passwordTxtEntry = new NoUndoEntry();
			this.passwordTxtEntry.WidthRequest = 10;
			this.passwordTxtEntry.CanFocus = true;
			this.passwordTxtEntry.IsEditable = true;
			this.passwordTxtEntry.MaxLength = 1;
			this.passwordTxtEntry.InvisibleChar = '●';
			this.passwordTxtEntry.TooltipText = LanguageInfo.txt_Password;
			this.passwordTxtEntry.KeyReleaseEvent += this.EntryKeyReleaseHandler;
			this.passwordTxtEntry.FocusOutEvent += this.EntryFocusOutHandler;
			HBox hbox2 = new HBox();
			hbox2.Spacing = 4;
			hbox2.PackStart(hbox, false, false, 0U);
			hbox2.PackStart(this.passwordTxtEntry);
			hbox2.ShowAll();
			base.SetControl();
			return hbox2;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00015D64 File Offset: 0x00013F64
		protected override void OnSetControl()
		{
			PasswordValue value = base.PropertyItem.GetValue<PasswordValue>(0);
			this.checkBtn.Active = (this.passwordTxtEntry.Sensitive = value.PasswordEnable);
			this.passwordTxtEntry.Text = (this.lastText = value.PasswordStyleText);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00015DBC File Offset: 0x00013FBC
		private void SetPasswordTextStyle(string textStyle)
		{
			if (!string.IsNullOrEmpty(textStyle))
			{
				if (textStyle.Length == 1)
				{
					char value = textStyle[0];
					int num = Convert.ToInt32(value);
					if (num > 32 && num < 127)
					{
						if (!(textStyle == this.lastText))
						{
							PasswordValue value2 = base.PropertyItem.GetValue<PasswordValue>(0);
							value2.PasswordStyleText = textStyle;
							base.UpdatePropertyValue(value2, null);
							this.lastText = textStyle;
						}
					}
				}
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00015E50 File Offset: 0x00014050
		private void EntryFocusOutHandler(object o, FocusOutEventArgs args)
		{
			if (string.IsNullOrEmpty(this.passwordTxtEntry.Text))
			{
				this.passwordTxtEntry.Text = this.lastText;
			}
			else
			{
				this.SetPasswordTextStyle(this.passwordTxtEntry.Text);
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00015E9C File Offset: 0x0001409C
		private void EntryKeyReleaseHandler(object o, KeyReleaseEventArgs args)
		{
			if (KeyboardExtend.IsEnterKey(args.Event.Key))
			{
				this.SetPasswordTextStyle(this.passwordTxtEntry.Text);
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00015ED8 File Offset: 0x000140D8
		private void CheckButtonToggledHandler(object sender, EventArgs e)
		{
			PasswordValue value = base.PropertyItem.GetValue<PasswordValue>(0);
			value.PasswordEnable = this.checkBtn.Active;
			this.passwordTxtEntry.Sensitive = this.checkBtn.Active;
			base.UpdatePropertyValue(value, null);
		}

		// Token: 0x0400024F RID: 591
		private CheckButton checkBtn;

		// Token: 0x04000250 RID: 592
		private NoUndoEntry passwordTxtEntry;

		// Token: 0x04000251 RID: 593
		private string lastText = string.Empty;
	}
}
