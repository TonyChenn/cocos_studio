using System;
using System.Text.RegularExpressions;
using Gdk;
using GLib;

namespace Gtk
{
	public class EntryCallBackEx : EntryEx
	{
		public event EventHandler<EntryCallBackEventArgs> EntryValueChanged;

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

		public EntryCallBackEx()
		{
			base.HeightRequest = 22;
			base.TextInserted += this.EntryCallBackEx_TextInserted;
		}

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

		protected override void OnTextInserted(string text, ref int position)
		{
			if (this.canSet)
			{
				base.OnTextInserted(text.Trim(), ref position);
			}
		}

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

		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			if (evnt.Button == 1U)
			{
				this.isPress = true;
			}
			return base.OnButtonPressEvent(evnt);
		}

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

		private string strValue;

		private string oldValue = string.Empty;

		private bool canSet = false;

		private bool isPress = false;

		private string regexFormat = "^[A-Za-z_]$|^[A-Za-z_]+[A-Za-z0-9_]*[A-Za-z0-9_]$";

		private string regexFormat_singleInput = "^[A-Za-z0-9]{1}$|^[_]{1}$";
	}
}
