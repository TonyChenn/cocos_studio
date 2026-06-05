using System;
using System.Collections.Generic;
using System.Linq;
using Gdk;

namespace Gtk
{
	// Token: 0x0200001C RID: 28
	public class EntryIntEx : EntryEx
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000050F0 File Offset: 0x000032F0
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00005107 File Offset: 0x00003307
		public int MaxValue { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00005110 File Offset: 0x00003310
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00005127 File Offset: 0x00003327
		public int MinValue { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00005130 File Offset: 0x00003330
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00005147 File Offset: 0x00003347
		public bool IsInteger { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00005150 File Offset: 0x00003350
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00005167 File Offset: 0x00003367
		public int DecimalPlaces { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00005170 File Offset: 0x00003370
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00005187 File Offset: 0x00003387
		public float ScrollNum { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00005190 File Offset: 0x00003390
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x000051A7 File Offset: 0x000033A7
		public bool CanZero { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x000051B0 File Offset: 0x000033B0
		// (set) Token: 0x060000DA RID: 218 RVA: 0x000051C8 File Offset: 0x000033C8
		public float Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (this.IsRound)
				{
					if (value > (float)this.MaxValue)
					{
						value %= (float)this.MaxValue;
					}
					if (value < (float)this.MinValue)
					{
						value = value % (float)this.MaxValue + (float)this.MaxValue;
					}
				}
				else
				{
					if (value > (float)this.MaxValue)
					{
						value = (float)this.MaxValue;
					}
					if (value < (float)this.MinValue)
					{
						value = (float)this.MinValue;
					}
				}
				this._value = this.FormatValue(value);
				this.oldValue = Convert.ToSingle(base.Text);
				this.isSubState = false;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00005284 File Offset: 0x00003484
		// (set) Token: 0x060000DC RID: 220 RVA: 0x0000529B File Offset: 0x0000349B
		public bool IsRound { get; set; }

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060000DD RID: 221 RVA: 0x000052A4 File Offset: 0x000034A4
		// (remove) Token: 0x060000DE RID: 222 RVA: 0x000052E0 File Offset: 0x000034E0
		public event EventHandler<EntryIntEventArgs> EntryValueChanged;

		// Token: 0x060000DF RID: 223 RVA: 0x0000531C File Offset: 0x0000351C
		public EntryIntEx()
		{
			base.HeightRequest = 22;
			this.MaxValue = 9999999;
			this.MinValue = -9999999;
			this.IsInteger = false;
			this.ScrollNum = 1f;
			this.DecimalPlaces = 1;
			this.CanZero = true;
			this.IsRound = false;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000053B8 File Offset: 0x000035B8
		public void SetEntryProperty(bool isInteger, int decimalPlaces, float scrollNum)
		{
			this.IsInteger = isInteger;
			this.DecimalPlaces = decimalPlaces;
			this.ScrollNum = scrollNum;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000053D3 File Offset: 0x000035D3
		public void SetToSubState()
		{
			this.isSubState = true;
			this.SetSubText();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000053E4 File Offset: 0x000035E4
		private void SetSubText()
		{
			this.isChangingToSub = true;
			base.Text = "-";
			this.isChangingToSub = false;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005404 File Offset: 0x00003604
		private float FormatValue(float value)
		{
			string text = string.Empty;
			if (this.IsInteger)
			{
				text = value.ToString("F0");
			}
			else
			{
				text = value.ToString(string.Format("F{0}", this.DecimalPlaces));
			}
			if (base.Text != text)
			{
				this.isResetValue = true;
				base.Text = text;
				this.isResetValue = false;
			}
			return Convert.ToSingle(text);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000548C File Offset: 0x0000368C
		protected override void OnTextInserted(string text, ref int position)
		{
			if (this.isChangingToSub)
			{
				base.OnTextInserted(text.Trim(), ref position);
			}
			else if (this.isResetValue)
			{
				base.OnTextInserted(text, ref position);
			}
			else
			{
				this.hasTextInserted = true;
				if (this.isScrolling)
				{
					text = text.Replace('。', '.').Replace('－', '-');
					base.OnTextInserted(text.Trim(), ref position);
				}
				else if (text.FirstOrDefault<char>() != ',')
				{
					List<char> list = base.Text.ToList<char>();
					list.InsertRange(position, text);
					string s = new string(list.ToArray()).Replace('。', '.').Replace('－', '-');
					int num = 0;
					float num2 = 0f;
					bool flag;
					if (this.IsInteger)
					{
						flag = int.TryParse(s, out num);
					}
					else
					{
						flag = float.TryParse(s, out num2);
					}
					if (!flag)
					{
						if (position != 0 || (text.FirstOrDefault<char>() != '-' && text.FirstOrDefault<char>() != '－'))
						{
							return;
						}
					}
					base.OnTextInserted(text.Trim(), ref position);
				}
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000055E0 File Offset: 0x000037E0
		protected override void OnTextDeleted(int start_pos, int end_pos)
		{
			this.hasTextInserted = true;
			base.OnTextDeleted(start_pos, end_pos);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000055F4 File Offset: 0x000037F4
		protected override void OnFocusGrabbed()
		{
			if (base.Text == '-'.ToString())
			{
				base.Text = "";
			}
			base.OnFocusGrabbed();
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005634 File Offset: 0x00003834
		protected override bool OnKeyPressEvent(EventKey evnt)
		{
			bool result;
			if (evnt.Key == Key.Up || evnt.Key == Key.Down)
			{
				this.ScrollValueByOneStep(evnt.Key == Key.Up);
				result = true;
			}
			else
			{
				result = base.OnKeyPressEvent(evnt);
			}
			return result;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000568C File Offset: 0x0000388C
		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			bool result;
			if (string.IsNullOrEmpty(base.Text))
			{
				result = false;
			}
			else
			{
				if ((evnt.Key == Key.Return || evnt.Key == Key.KP_Enter || evnt.Key == Key.ISO_Enter) && base.IsFocus)
				{
					double num = 0.0;
					if (double.TryParse(base.Text, out num))
					{
						if (num == 0.0 && !this.CanZero)
						{
							this.Value = this.oldValue;
							return false;
						}
						this.Value = (float)num;
						if (this.EntryValueChanged != null)
						{
							this.EntryValueChanged(this, new EntryIntEventArgs(this.Value));
						}
					}
				}
				result = base.OnKeyReleaseEvent(evnt);
			}
			return result;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005774 File Offset: 0x00003974
		protected override bool OnScrollEvent(EventScroll evnt)
		{
			bool result;
			if (!base.IsFocus)
			{
				result = false;
			}
			else if (string.IsNullOrEmpty(base.Text))
			{
				result = false;
			}
			else
			{
				this.ScrollValueByOneStep(evnt.Direction == ScrollDirection.Up);
				result = true;
			}
			return result;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000057BC File Offset: 0x000039BC
		private void ScrollValueByOneStep(bool isUp)
		{
			float num = 0f;
			if (float.TryParse(base.Text, out num))
			{
				if (isUp)
				{
					num += this.ScrollNum;
					if (num == 0f && !this.CanZero)
					{
						num += this.ScrollNum;
					}
				}
				else
				{
					num -= this.ScrollNum;
					if (num == 0f && !this.CanZero)
					{
						num -= this.ScrollNum;
					}
				}
				this.isScrolling = true;
				if (this.IsInteger)
				{
					base.Text = Math.Round((double)num, 0).ToString();
				}
				else
				{
					base.Text = Math.Round((double)num, this.DecimalPlaces).ToString();
				}
				this.isScrolling = false;
				base.IsFocus = true;
				this.Value = num;
				if (this.EntryValueChanged != null)
				{
					this.EntryValueChanged(this, new EntryIntEventArgs(this.Value));
				}
				this.SelectAll();
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000058D0 File Offset: 0x00003AD0
		protected override bool OnFocusOutEvent(EventFocus evnt)
		{
			base.SelectRegion(0, 0);
			if (this.hasTextInserted)
			{
				if (!string.IsNullOrEmpty(base.Text))
				{
					try
					{
						float num = Convert.ToSingle(base.Text);
						if (num == 0f && !this.CanZero)
						{
							this.Value = this.oldValue;
							return false;
						}
						if (num != this.oldValue)
						{
							this.Value = num;
							if (this.EntryValueChanged != null)
							{
								this.EntryValueChanged(this, new EntryIntEventArgs(this.Value));
							}
						}
						else
						{
							this.FormatValue(num);
						}
					}
					catch
					{
						if (this.isSubState)
						{
							this.SetSubText();
						}
						else
						{
							this.Value = this.oldValue;
						}
					}
				}
				else if (this.isSubState)
				{
					this.SetSubText();
				}
				else
				{
					this.Value = this.oldValue;
				}
			}
			this.hasTextInserted = false;
			return base.OnFocusOutEvent(evnt);
		}

		// Token: 0x0400004D RID: 77
		private const char macdot = '。';

		// Token: 0x0400004E RID: 78
		private const char dot = '.';

		// Token: 0x0400004F RID: 79
		private const char macsub = '－';

		// Token: 0x04000050 RID: 80
		private const char sub = '-';

		// Token: 0x04000051 RID: 81
		private const char comma = ',';

		// Token: 0x04000052 RID: 82
		private bool isChangingToSub = false;

		// Token: 0x04000053 RID: 83
		private bool isSubState = false;

		// Token: 0x04000054 RID: 84
		private bool isScrolling = false;

		// Token: 0x04000055 RID: 85
		private bool isResetValue = false;

		// Token: 0x04000056 RID: 86
		private bool hasTextInserted = false;

		// Token: 0x04000057 RID: 87
		private float _value = 0f;

		// Token: 0x04000059 RID: 89
		private float oldValue = float.NaN;
	}
}
