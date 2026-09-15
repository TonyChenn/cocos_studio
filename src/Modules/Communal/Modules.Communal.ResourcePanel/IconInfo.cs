using System;
using MonoDevelop.Components;
using Xwt;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	public class IconInfo
	{
		public IconInfo()
		{
			this.Reset();
		}

		public void Reset()
		{
			this.ExpandIcon = CellRendererImage.NullImage;
			this.UnExpandIcon = CellRendererImage.NullImage;
			this.StatusIconInternal = CellRendererImage.NullImage;
		}

		public Image StatusIconInternal
		{
			get
			{
				return this.statusIconInternal;
			}
			set
			{
				this.statusIconInternal = this.AutoSize(value);
			}
		}

		public Image UnExpandIcon
		{
			get
			{
				if (this.unExpandIcon == CellRendererImage.NullImage || this.unExpandIcon == null)
				{
					return this.expandIcon;
				}
				return this.unExpandIcon;
			}
			set
			{
				this.unExpandIcon = this.AutoSize(value);
			}
		}

		public Image ExpandIcon
		{
			get
			{
				return this.expandIcon;
			}
			set
			{
				this.expandIcon = this.AutoSize(value);
			}
		}

		private Image AutoSize(Image image)
		{
			if (image == null)
			{
				return null;
			}
			if (image.Height > 16.0 || image.Width > 16.0)
			{
				Size relativeSize = this.GetRelativeSize(image.Size);
				image = image.WithSize(relativeSize.Width, relativeSize.Height);
			}
			return image;
		}

		private Size GetRelativeSize(Size size)
		{
			double num = (size.Width > size.Height) ? size.Width : size.Height;
			if (num > 16.0)
			{
				double num2 = 16.0 / num;
				double width = size.Width * num2;
				double height = size.Height * num2;
				return new Size(width, height);
			}
			return size;
		}

		private const int MaxSize = 16;

		private Image expandIcon;

		private Image statusIconInternal;

		private Image unExpandIcon;
	}
}
