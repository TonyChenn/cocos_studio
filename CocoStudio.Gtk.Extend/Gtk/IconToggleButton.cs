using System;
using Xwt.Drawing;

namespace Gtk
{
	// Token: 0x02000008 RID: 8
	public class IconToggleButton : IconButton
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002C94 File Offset: 0x00000E94
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002CAC File Offset: 0x00000EAC
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

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000043 RID: 67 RVA: 0x00002CDC File Offset: 0x00000EDC
		// (remove) Token: 0x06000044 RID: 68 RVA: 0x00002D18 File Offset: 0x00000F18
		public event EventHandler CheckChanged;

		// Token: 0x06000045 RID: 69 RVA: 0x00002D54 File Offset: 0x00000F54
		public IconToggleButton(Image normal, Image check = null) : base(normal)
		{
			this.checkedIcon = check;
			this.OnCheckedChanged();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002D6E File Offset: 0x00000F6E
		public void ChangeImage(Image normal, Image check)
		{
			this.checkedIcon = check;
			base.ChangeImage(normal);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002D80 File Offset: 0x00000F80
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

		// Token: 0x06000048 RID: 72 RVA: 0x00002E80 File Offset: 0x00001080
		protected override void OnRefreshIcon()
		{
			Image normalIcon;
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

		// Token: 0x06000049 RID: 73 RVA: 0x00002F2C File Offset: 0x0000112C
		protected override void OnMousePressed(ButtonPressEventArgs args)
		{
			this.IsChecked = !this.IsChecked;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002F40 File Offset: 0x00001140
		protected virtual void OnCheckedChanged()
		{
			this.OnRefreshUI();
			if (base.Sensitive && this.CheckChanged != null)
			{
				this.CheckChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x0400001C RID: 28
		protected Image checkedIcon;

		// Token: 0x0400001D RID: 29
		protected bool _IsChecked;
	}
}
