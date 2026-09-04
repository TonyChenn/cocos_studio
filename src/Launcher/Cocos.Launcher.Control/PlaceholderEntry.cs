using System;
using System.ComponentModel;
using Gtk;

namespace Cocos.Launcher.Control
{
	// Token: 0x02000010 RID: 16
	[ToolboxItem(true)]
	public class PlaceholderEntry : Entry
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000034AA File Offset: 0x000016AA
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x000034B2 File Offset: 0x000016B2
		public string PlaceholderText
		{
			get
			{
				return this.placeholderText;
			}
			set
			{
				this.placeholderText = value;
				base.Text = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000034C2 File Offset: 0x000016C2
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x000034CC File Offset: 0x000016CC
		public bool IsPlaceholder
		{
			get
			{
				return this.isPlaceholder;
			}
			set
			{
				this.isPlaceholder = value;
				if (value)
				{
					base.ModifyText(StateType.Normal, ConstantConfig.Colors.NewsInfoColor);
					base.Visibility = true;
					return;
				}
				base.ModifyText(StateType.Normal, ConstantConfig.Colors.ContentLabelColor1);
				if (this.isPassword)
				{
					base.Visibility = false;
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000351C File Offset: 0x0000171C
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00003524 File Offset: 0x00001724
		public bool IsPassword
		{
			get
			{
				return this.isPassword;
			}
			set
			{
				this.isPassword = value;
				base.InvisibleChar = '*';
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003535 File Offset: 0x00001735
		public PlaceholderEntry()
		{
			base.ActivatesDefault = true;
			base.FocusInEvent += this.PlaceholderEntry_FocusInEvent;
			base.FocusOutEvent += this.PlaceholderEntry_FocusOutEvent;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003573 File Offset: 0x00001773
		private void PlaceholderEntry_FocusInEvent(object o, FocusInEventArgs args)
		{
			if (this.isPlaceholder)
			{
				base.Text = string.Empty;
				this.IsPlaceholder = false;
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000358F File Offset: 0x0000178F
		private void PlaceholderEntry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			if (base.Text.Trim() == string.Empty)
			{
				base.Text = this.PlaceholderText;
				this.IsPlaceholder = true;
				return;
			}
			this.IsPlaceholder = false;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000035C3 File Offset: 0x000017C3
		protected override void OnClipboardCopied()
		{
			if (!this.isPassword)
			{
				base.OnClipboardCopied();
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000035D3 File Offset: 0x000017D3
		protected override void OnClipboardCut()
		{
			if (!this.isPassword)
			{
				base.OnClipboardCut();
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000035E3 File Offset: 0x000017E3
		protected override void OnClipboardPasted()
		{
			if (!this.isPassword)
			{
				base.OnClipboardPasted();
			}
		}

		// Token: 0x0400004F RID: 79
		private string placeholderText = string.Empty;

		// Token: 0x04000050 RID: 80
		private bool isPlaceholder;

		// Token: 0x04000051 RID: 81
		private bool isPassword;
	}
}
