using System;
using MonoDevelop.Components;
using Xwt;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200000D RID: 13
	public class IconInfo
	{
		// Token: 0x0600004A RID: 74 RVA: 0x0000301F File Offset: 0x0000121F
		public IconInfo()
		{
			this.Reset();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000302D File Offset: 0x0000122D
		public void Reset()
		{
			this.ExpandIcon = CellRendererImage.NullImage;
			this.UnExpandIcon = CellRendererImage.NullImage;
			this.StatusIconInternal = CellRendererImage.NullImage;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00003050 File Offset: 0x00001250
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00003058 File Offset: 0x00001258
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00003067 File Offset: 0x00001267
		// (set) Token: 0x0600004F RID: 79 RVA: 0x0000308B File Offset: 0x0000128B
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

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000050 RID: 80 RVA: 0x0000309A File Offset: 0x0000129A
		// (set) Token: 0x06000051 RID: 81 RVA: 0x000030A2 File Offset: 0x000012A2
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

		// Token: 0x06000052 RID: 82 RVA: 0x000030B4 File Offset: 0x000012B4
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

		// Token: 0x06000053 RID: 83 RVA: 0x0000310C File Offset: 0x0000130C
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

		// Token: 0x04000029 RID: 41
		private const int MaxSize = 16;

		// Token: 0x0400002A RID: 42
		private Image expandIcon;

		// Token: 0x0400002B RID: 43
		private Image statusIconInternal;

		// Token: 0x0400002C RID: 44
		private Image unExpandIcon;
	}
}
