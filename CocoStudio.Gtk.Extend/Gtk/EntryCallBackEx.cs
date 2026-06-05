using System;
using System.Text.RegularExpressions;
using Gdk;
using GLib;

namespace Gtk
{
	// Token: 0x0200001B RID: 27
	public class EntryCallBackEx : EntryEx
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000BD RID: 189 RVA: 0x00004CAC File Offset: 0x00002EAC
		// (remove) Token: 0x060000BE RID: 190 RVA: 0x00004CE8 File Offset: 0x00002EE8
		public event EventHandler<EntryCallBackEventArgs> EntryValueChanged;

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004D24 File Offset: 0x00002F24
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00004D3C File Offset: 0x00002F3C
		public string RegexFormat
		{
			get
			{
				return this.regexFormat;
			}
			set
			{
				this.regexFormat = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004D48 File Offset: 0x00002F48
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00004D60 File Offset: 0x00002F60
		public string RegexFormat_singleInput
		{
			get
			{
				return this.regexFormat_singleInput;
			}
			set
			{
				this.regexFormat_singleInput = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00004D6C File Offset: 0x00002F6C
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00004D84 File Offset: 0x00002F84
		public string Value
		{
			get
			{
				return this.strValue;
			}
			set
			{
				this.strValue = value;
				if (string.IsNullOrEmpty(this.strValue))
				{
					this.strValue = "";
				}
				base.Text = this.strValue;
				this.oldValue = value;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004DCC File Offset: 0x00002FCC
		public EntryCallBackEx()
		{
			base.HeightRequest = 22;
			base.TextInserted += this.EntryCallBackEx_TextInserted;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004E30 File Offset: 0x00003030
		[ConnectBefore]
		private void EntryCallBackEx_TextInserted(object o, TextInsertedArgs args)
		{
			this.canSet = false;
			if (this.CheckSingleValue(args.Text))
			{
				this.canSet = true;
			}
			else if (this.CheckFinalValue(args.Text))
			{
				this.canSet = true;
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004E80 File Offset: 0x00003080
		private bool CheckSingleValue(string value)
		{
			bool result;
			if (string.IsNullOrEmpty(value.Trim()))
			{
				result = false;
			}
			else
			{
				Regex regex = new Regex(this.RegexFormat_singleInput);
				result = regex.IsMatch(value.Trim());
			}
			return result;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004EC0 File Offset: 0x000030C0
		private bool CheckFinalValue(string value)
		{
			bool result;
			if (value == null)
			{
				result = false;
			}
			else if (string.IsNullOrWhiteSpace(value))
			{
				result = true;
			}
			else
			{
				Regex regex = new Regex(this.RegexFormat);
				result = regex.IsMatch(value.Trim());
			}
			return result;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004F0C File Offset: 0x0000310C
		protected override void OnTextInserted(string text, ref int position)
		{
			if (this.canSet)
			{
				base.OnTextInserted(text.Trim(), ref position);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004F38 File Offset: 0x00003138
		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			if ((evnt.Key == Gdk.Key.Return || evnt.Key == Gdk.Key.KP_Enter || evnt.Key == Gdk.Key.ISO_Enter) && base.IsFocus)
			{
				if (!this.CheckFinalValue(base.Text))
				{
					this.Value = this.oldValue;
					return false;
				}
				this.Value = base.Text.Trim();
				if (this.EntryValueChanged != null)
				{
					this.EntryValueChanged(this, new EntryCallBackEventArgs(this.Value));
				}
				this.oldValue = this.Value;
			}
			return base.OnKeyReleaseEvent(evnt);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004FF8 File Offset: 0x000031F8
		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			if (evnt.Button == 1U)
			{
				this.isPress = true;
			}
			return base.OnButtonPressEvent(evnt);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005028 File Offset: 0x00003228
		protected override bool OnFocusOutEvent(EventFocus evnt)
		{
			base.SelectRegion(0, 0);
			if (this.isPress)
			{
				try
				{
					if (!this.CheckFinalValue(base.Text))
					{
						this.Value = this.oldValue;
						return false;
					}
					this.Value = base.Text.Trim();
					if (this.EntryValueChanged != null)
					{
						this.EntryValueChanged(this, new EntryCallBackEventArgs(this.Value));
					}
				}
				catch
				{
					this.Value = this.oldValue;
				}
			}
			this.isPress = false;
			this.canSet = false;
			return base.OnFocusOutEvent(evnt);
		}

		// Token: 0x04000046 RID: 70
		private string strValue;

		// Token: 0x04000047 RID: 71
		private string oldValue = string.Empty;

		// Token: 0x04000048 RID: 72
		private bool canSet = false;

		// Token: 0x04000049 RID: 73
		private bool isPress = false;

		// Token: 0x0400004A RID: 74
		private string regexFormat = "^[A-Za-z_]$|^[A-Za-z_]+[A-Za-z0-9_]*[A-Za-z0-9_]$";

		// Token: 0x0400004B RID: 75
		private string regexFormat_singleInput = "^[A-Za-z0-9]{1}$|^[_]{1}$";
	}
}
