using System;

namespace Gtk
{
	// Token: 0x0200009C RID: 156
	internal class CsMessageBoxButton : Button, IDialogButton
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000F46C File Offset: 0x0000D66C
		// (set) Token: 0x0600035A RID: 858 RVA: 0x0000F483 File Offset: 0x0000D683
		public EnumMainButton ButtonType { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000F48C File Offset: 0x0000D68C
		// (set) Token: 0x0600035C RID: 860 RVA: 0x0000F4A4 File Offset: 0x0000D6A4
		public string Text
		{
			get
			{
				return base.Label;
			}
			set
			{
				base.Label = value;
			}
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x0600035D RID: 861 RVA: 0x0000F4B0 File Offset: 0x0000D6B0
		// (remove) Token: 0x0600035E RID: 862 RVA: 0x0000F4EC File Offset: 0x0000D6EC
		public new event EventHandler<ButtonReleaseEventArgs> Clicked;

		// Token: 0x0600035F RID: 863 RVA: 0x0000F528 File Offset: 0x0000D728
		public Widget GetWidget()
		{
			return this;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000F53B File Offset: 0x0000D73B
		public CsMessageBoxButton()
		{
			base.Name = "DefaultButton";
			base.HasFocus = false;
			base.CanFocus = false;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000F564 File Offset: 0x0000D764
		protected override void OnClicked()
		{
			base.OnClicked();
			if (this.Clicked != null)
			{
				this.Clicked(this, new ButtonReleaseEventArgs());
			}
		}
	}
}
