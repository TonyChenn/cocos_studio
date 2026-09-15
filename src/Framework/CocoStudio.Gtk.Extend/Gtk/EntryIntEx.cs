using System;
using System.Collections.Generic;
using System.Linq;
using Gdk;

namespace Gtk
{
	public class EntryIntEx : EntryEx
	{
		public int MaxValue { get; set; }

		public int MinValue { get; set; }

		public bool IsInteger { get; set; }

		public int DecimalPlaces { get; set; }

		public float ScrollNum { get; set; }

		public bool CanZero { get; set; }

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

		public bool IsRound { get; set; }

		public event EventHandler<EntryIntEventArgs> EntryValueChanged;

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

		public void SetEntryProperty(bool isInteger, int decimalPlaces, float scrollNum)
		{
			this.IsInteger = isInteger;
			this.DecimalPlaces = decimalPlaces;
			this.ScrollNum = scrollNum;
		}

		public void SetToSubState()
		{
			this.isSubState = true;
			this.SetSubText();
		}

		private void SetSubText()
		{
			this.isChangingToSub = true;
			base.Text = "-";
			this.isChangingToSub = false;
		}

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

		protected override void OnTextDeleted(int start_pos, int end_pos)
		{
			this.hasTextInserted = true;
			base.OnTextDeleted(start_pos, end_pos);
		}

		protected override void OnFocusGrabbed()
		{
			if (base.Text == '-'.ToString())
			{
				base.Text = "";
			}
			base.OnFocusGrabbed();
		}

		protected override bool OnKeyPressEvent(EventKey evnt)
		{
			bool result;
			if (evnt.Key == Gdk.Key.Up || evnt.Key == Gdk.Key.Down)
			{
				this.ScrollValueByOneStep(evnt.Key == Gdk.Key.Up);
				result = true;
			}
			else
			{
				result = base.OnKeyPressEvent(evnt);
			}
			return result;
		}

		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			bool result;
			if (string.IsNullOrEmpty(base.Text))
			{
				result = false;
			}
			else
			{
				if ((evnt.Key == Gdk.Key.Return || evnt.Key == Gdk.Key.KP_Enter || evnt.Key == Gdk.Key.ISO_Enter) && base.IsFocus)
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

		private const char macdot = '。';

		private const char dot = '.';

		private const char macsub = '－';

		private const char sub = '-';

		private const char comma = ',';

		private bool isChangingToSub = false;

		private bool isSubState = false;

		private bool isScrolling = false;

		private bool isResetValue = false;

		private bool hasTextInserted = false;

		private float _value = 0f;

		private float oldValue = float.NaN;
	}
}
