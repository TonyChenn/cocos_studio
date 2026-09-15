using System;
using CocoStudio.Model;
using Gdk;

namespace CocoStudio.Projects
{
	public class PreviewImageInfo : IDisposable
	{
		public SizeF Size { get; internal set; }

		public Pixbuf Image { get; internal set; }

		public string ImageFormat { get; internal set; }

		public PreviewImageInfo(Pixbuf image)
		{
			this.Size = new SizeF((float)image.Width, (float)image.Height);
			this.Image = image;
		}

		public PreviewImageInfo()
		{
		}

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
