using System;
using XwtImage = Xwt.Drawing.Image;

namespace Gtk
{
	// Token: 0x02000009 RID: 9
	public class IconOnlyToggleButton : IconToggleButton
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00002F80 File Offset: 0x00001180
		public IconOnlyToggleButton(XwtImage normal, XwtImage check = null) : base(normal, check)
		{
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002F90 File Offset: 0x00001190
		protected override void OnSetStyle()
		{
			base.VisibleWindow = (this.bgBox.VisibleWindow = false);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002FB5 File Offset: 0x000011B5
		protected override void OnRefreshUI()
		{
			this.OnRefreshIcon();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002FC0 File Offset: 0x000011C0
		protected override void OnRefreshIcon()
		{
			XwtImage image;
			if (base.IsChecked)
			{
				image = this.checkedIcon;
				if (image == null && this.normalIcon != null)
				{
					image = this.normalIcon;
				}
			}
			else
			{
				image = this.normalIcon;
				if (image == null && this.checkedIcon != null)
				{
					image = this.checkedIcon;
				}
			}
			if (image != null)
			{
				switch (base.State)
				{
				case StateType.Prelight:
					this.imgView.Image = image;
					break;
				case StateType.Selected:
					this.imgView.Image = image.WithAlpha(0.6);
					break;
				case StateType.Insensitive:
					this.imgView.Image = image.WithAlpha(0.4);
					break;
				default:
					this.imgView.Image = image.WithAlpha(0.75);
					break;
				}
			}
		}
	}
}
