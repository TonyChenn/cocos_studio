using System;
using Modules.Communal.MultiLanguage;

namespace Gtk
{
	// Token: 0x02000065 RID: 101
	public class ButtonText
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00009B14 File Offset: 0x00007D14
		// (set) Token: 0x0600022A RID: 554 RVA: 0x00009B2B File Offset: 0x00007D2B
		public MessageBoxButton ButtonType { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00009B34 File Offset: 0x00007D34
		// (set) Token: 0x0600022C RID: 556 RVA: 0x00009B4B File Offset: 0x00007D4B
		public string YesBtnText { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00009B54 File Offset: 0x00007D54
		// (set) Token: 0x0600022E RID: 558 RVA: 0x00009B6B File Offset: 0x00007D6B
		public string NoBtnText { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00009B74 File Offset: 0x00007D74
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00009B8B File Offset: 0x00007D8B
		public string CancelBtnText { get; private set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00009B94 File Offset: 0x00007D94
		// (set) Token: 0x06000232 RID: 562 RVA: 0x00009BAB File Offset: 0x00007DAB
		public bool IsYesBtnAutoSize { get; private set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00009BB4 File Offset: 0x00007DB4
		// (set) Token: 0x06000234 RID: 564 RVA: 0x00009BCB File Offset: 0x00007DCB
		public bool IsNoBtnAutoSize { get; private set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00009BD4 File Offset: 0x00007DD4
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00009BEB File Offset: 0x00007DEB
		public bool IsCancelBtnAutoSize { get; private set; }

		// Token: 0x06000237 RID: 567 RVA: 0x00009BF4 File Offset: 0x00007DF4
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

		// Token: 0x06000238 RID: 568 RVA: 0x00009C34 File Offset: 0x00007E34
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

		// Token: 0x06000239 RID: 569 RVA: 0x00009C90 File Offset: 0x00007E90
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

		// Token: 0x0600023A RID: 570 RVA: 0x00009D06 File Offset: 0x00007F06
		private void SetDefaultText()
		{
			this.YesBtnText = LanguageInfo.Dialog_ButtonYes;
			this.NoBtnText = LanguageInfo.Dialog_ButtonNo;
			this.CancelBtnText = LanguageInfo.Dialog_ButtonCancel;
		}
	}
}
