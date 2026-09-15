using System;
using Modules.Communal.MultiLanguage;

namespace Gtk
{
	public class ButtonText
	{
		public MessageBoxButton ButtonType { get; private set; }

		public string YesBtnText { get; private set; }

		public string NoBtnText { get; private set; }

		public string CancelBtnText { get; private set; }

		public bool IsYesBtnAutoSize { get; private set; }

		public bool IsNoBtnAutoSize { get; private set; }

		public bool IsCancelBtnAutoSize { get; private set; }

		public ButtonText(string yesText, bool yesAdjust = false)
		{
			this.SetDefaultText();
			this.ButtonType = MessageBoxButton.Yes;
			if (!string.IsNullOrEmpty(yesText))
			{
				this.YesBtnText = yesText;
			}
			this.IsYesBtnAutoSize = yesAdjust;
		}

		public ButtonText(string yesText, string noText, bool yesAdjust = false, bool noAdjust = false)
		{
			this.SetDefaultText();
			this.ButtonType = MessageBoxButton.YesNo;
			if (!string.IsNullOrEmpty(yesText))
			{
				this.YesBtnText = yesText;
			}
			if (!string.IsNullOrEmpty(noText))
			{
				this.NoBtnText = noText;
			}
			this.IsYesBtnAutoSize = yesAdjust;
			this.IsNoBtnAutoSize = noAdjust;
		}

		public ButtonText(string yesText, string noText, string cancelText, bool yesAdjust = false, bool noAdjust = false, bool cancelAdjust = false)
		{
			this.SetDefaultText();
			this.ButtonType = MessageBoxButton.YesNoCancel;
			if (!string.IsNullOrEmpty(yesText))
			{
				this.YesBtnText = yesText;
			}
			if (!string.IsNullOrEmpty(noText))
			{
				this.NoBtnText = noText;
			}
			if (!string.IsNullOrEmpty(cancelText))
			{
				this.CancelBtnText = cancelText;
			}
			this.IsYesBtnAutoSize = yesAdjust;
			this.IsNoBtnAutoSize = noAdjust;
			this.IsCancelBtnAutoSize = cancelAdjust;
		}

		private void SetDefaultText()
		{
			this.YesBtnText = LanguageInfo.Dialog_ButtonYes;
			this.NoBtnText = LanguageInfo.Dialog_ButtonNo;
			this.CancelBtnText = LanguageInfo.Dialog_ButtonCancel;
		}
	}
}
