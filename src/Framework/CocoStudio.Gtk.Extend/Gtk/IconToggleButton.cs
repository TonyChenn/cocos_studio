using System;
using XwtImage = Xwt.Drawing.Image;

namespace Gtk
{
	public class IconToggleButton : IconButton
	{
		public bool IsChecked
		{
			get
			{
				return this._IsChecked;
			}
			set
			{
				if (this._IsChecked != value)
				{
					this._IsChecked = value;
					this.OnCheckedChanged();
				}
			}
		}

		public event EventHandler CheckChanged;

		public IconToggleButton(XwtImage normal, XwtImage check = null) : base(normal)
		{
			this.checkedIcon = check;
			this.OnCheckedChanged();
		}

		public void ChangeImage(XwtImage normal, XwtImage check)
		{
			this.checkedIcon = check;
			base.ChangeImage(normal);
		}

		protected override void OnRefreshUI()
		{
			base.OnRefreshUI();
			if (this.IsChecked)
			{
				base.ModifyBg(StateType.Normal, WindowStyle.CheckedDarkLine);
				this.bgBox.ModifyBg(StateType.Normal, WindowStyle.CheckedDark);
				this.bgBox.ModifyBg(StateType.Prelight, WindowStyle.CheckedDark);
				this.bgBox.ModifyBg(StateType.Selected, WindowStyle.CheckedDark);
			}
			else
			{
				if (base.NormalBg != null)
				{
					base.ModifyBg(StateType.Normal, base.NormalBg.Value);
					this.bgBox.ModifyBg(StateType.Normal, base.NormalBg.Value);
				}
				else
				{
					base.ModifyBg(StateType.Normal, WindowStyle.WindowBgColor);
					this.bgBox.ModifyBg(StateType.Normal, WindowStyle.WindowBgColor);
				}
				this.bgBox.ModifyBg(StateType.Prelight, WindowStyle.WindowBgColor);
				this.bgBox.ModifyBg(StateType.Selected, WindowStyle.LineDarkColor);
			}
		}

		protected override void OnRefreshIcon()
		{
			XwtImage normalIcon;
			if (this.IsChecked)
			{
				normalIcon = this.checkedIcon;
				if (normalIcon == null && this.normalIcon != null)
				{
					normalIcon = this.normalIcon;
				}
			}
			else
			{
				normalIcon = this.normalIcon;
				if (normalIcon == null && this.checkedIcon != null)
				{
					normalIcon = this.checkedIcon;
				}
			}
			if (normalIcon != null)
			{
				if (base.State == StateType.Insensitive)
				{
					this.imgView.Image = normalIcon.WithAlpha(0.5);
				}
				else
				{
					this.imgView.Image = normalIcon;
				}
			}
		}

		protected override void OnMousePressed(ButtonPressEventArgs args)
		{
			this.IsChecked = !this.IsChecked;
		}

		protected virtual void OnCheckedChanged()
		{
			this.OnRefreshUI();
			if (base.Sensitive && this.CheckChanged != null)
			{
				this.CheckChanged(this, EventArgs.Empty);
			}
		}

		protected XwtImage checkedIcon;

		protected bool _IsChecked;
	}
}
