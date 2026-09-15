using System;
using XwtImage = Xwt.Drawing.Image;

namespace Gtk
{
	public class IconOnlyToggleButton : IconToggleButton
	{
		public IconOnlyToggleButton(XwtImage normal, XwtImage check = null) : base(normal, check)
		{
		}

		protected override void OnSetStyle()
		{
			base.VisibleWindow = (this.bgBox.VisibleWindow = false);
		}

		protected override void OnRefreshUI()
		{
			this.OnRefreshIcon();
		}

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
