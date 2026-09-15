using System;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class PasswordEditor : BaseEditor
	{
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

		protected override void OnSetControl()
		{
			PasswordValue value = base.PropertyItem.GetValue<PasswordValue>(0);
			this.checkBtn.Active = (this.passwordTxtEntry.Sensitive = value.PasswordEnable);
			this.passwordTxtEntry.Text = (this.lastText = value.PasswordStyleText);
		}

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

		private void EntryKeyReleaseHandler(object o, KeyReleaseEventArgs args)
		{
			if (KeyboardExtend.IsEnterKey(args.Event.Key))
			{
				this.SetPasswordTextStyle(this.passwordTxtEntry.Text);
			}
		}

		private void CheckButtonToggledHandler(object sender, EventArgs e)
		{
			PasswordValue value = base.PropertyItem.GetValue<PasswordValue>(0);
			value.PasswordEnable = this.checkBtn.Active;
			this.passwordTxtEntry.Sensitive = this.checkBtn.Active;
			base.UpdatePropertyValue(value, null);
		}

		private CheckButton checkBtn;

		private NoUndoEntry passwordTxtEntry;

		private string lastText = string.Empty;
	}
}
