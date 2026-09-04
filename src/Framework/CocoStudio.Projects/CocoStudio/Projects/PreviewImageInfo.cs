using System;
using CocoStudio.Model;
using Gdk;

namespace CocoStudio.Projects
{
	// Token: 0x02000051 RID: 81
	public class PreviewImageInfo : IDisposable
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00008FDE File Offset: 0x000071DE
		// (set) Token: 0x0600023D RID: 573 RVA: 0x00008FE6 File Offset: 0x000071E6
		public SizeF Size { get; internal set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00008FEF File Offset: 0x000071EF
		// (set) Token: 0x0600023F RID: 575 RVA: 0x00008FF7 File Offset: 0x000071F7
		public Pixbuf Image { get; internal set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00009000 File Offset: 0x00007200
		// (set) Token: 0x06000241 RID: 577 RVA: 0x00009008 File Offset: 0x00007208
		public string ImageFormat { get; internal set; }

		// Token: 0x06000242 RID: 578 RVA: 0x00009011 File Offset: 0x00007211
		public PreviewImageInfo(Pixbuf image)
		{
			this.Size = new SizeF((float)image.Width, (float)image.Height);
			this.Image = image;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00009039 File Offset: 0x00007239
		public PreviewImageInfo()
		{
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00009041 File Offset: 0x00007241
		public void Dispose()
		{
			if (this.Image != null)
			{
				this.Image.Dispose();
				this.Image = null;
			}
		}
	}
}
