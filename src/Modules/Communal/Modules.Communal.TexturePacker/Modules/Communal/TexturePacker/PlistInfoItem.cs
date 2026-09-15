using System;
using System.Drawing;
using System.IO;
using Cairo;
using CocoStudio.Basic;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Modules.Communal.Packer;

namespace Modules.Communal.TexturePacker
{
	public class PlistInfoItem
	{
		public bool Rotate
		{
			get
			{
				return this.rotate;
			}
			set
			{
				if (this.rotate != value)
				{
					this.rotate = value;
					if (this.pixbuf != null)
					{
						this.RotatePixbuf();
					}
				}
			}
		}

		private Gdk.Point Location { get; set; }

		public Gdk.Point SourceLocation { get; set; }

		public Gdk.Point Offset { get; set; }

		public int SourceWidth { get; private set; }

		public int SourceHeight { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		private int RenderWidth
		{
			get
			{
				int num = this.Model.AllowTrim ? this.Width : this.SourceWidth;
				int num2 = this.Model.AllowTrim ? this.Height : this.SourceHeight;
				int num3 = this.rotate ? num2 : num;
				return (int)((double)num3 * this.RenderScale);
			}
		}

		private int RenderHeight
		{
			get
			{
				int num = this.Model.AllowTrim ? this.Width : this.SourceWidth;
				int num2 = this.Model.AllowTrim ? this.Height : this.SourceHeight;
				int num3 = this.rotate ? num : num2;
				return (int)((double)num3 * this.RenderScale);
			}
		}

		public Gdk.Rectangle RenderRect
		{
			get
			{
				return new Gdk.Rectangle(new Gdk.Point((int)((double)this.SourceLocation.X * this.renderScale), (int)((double)this.SourceLocation.Y * this.renderScale)), new Gdk.Size(this.RenderWidth, this.RenderHeight));
			}
		}

		public double Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				if (Math.Abs(this.scale - value) > 0.0001)
				{
					this.scale = value;
					if (this.pixbuf != null)
					{
						Pixbuf pixbuf = this.LoadImageFile(this.File);
						this.ScalePixbuf(pixbuf);
						pixbuf.Dispose();
					}
				}
			}
		}

		public double RenderScale
		{
			get
			{
				return this.renderScale;
			}
			set
			{
				if (Math.Abs(this.renderScale - value) > 0.0001)
				{
					this.renderScale = value;
					if (this.pixbuf != null)
					{
						Pixbuf pixbuf = this.LoadImageFile(this.File);
						this.ScalePixbuf(pixbuf);
						pixbuf.Dispose();
					}
				}
			}
		}

		public string File
		{
			get
			{
				return this.ResourceItem.FullPath;
			}
		}

		public string RelativeFile
		{
			get
			{
				return this.ResourceItem.RelativePath;
			}
		}

		public ResourceItem ResourceItem { get; set; }

		public bool Select { get; set; }

		public PlistInfoModel Model { get; private set; }

		public PlistInfoItem(PlistInfoModel model, ResourceItem resourceItem)
		{
			this.Model = model;
			this.ResourceItem = resourceItem;
			string fullPath = resourceItem.FullPath;
			Pixbuf pixbuf = this.LoadImageFile(fullPath);
			this.ScalePixbuf(pixbuf);
			this.Scale = (double)this.Model.ContentScale;
			this.Select = false;
			this.Trim(pixbuf);
			pixbuf.Dispose();
			if (this.pixbuf != null)
			{
				this.pixbuf.Dispose();
				this.pixbuf = null;
			}
		}

		private Pixbuf LoadImageFile(string filePath)
		{
			Pixbuf pixbuf = null;
			if (System.IO.File.Exists(this.File))
			{
				pixbuf = PixbufHelper.Load(filePath);
			}
			if (pixbuf == null)
			{
				string editorResourceFullPath = Option.GetEditorResourceFullPath("Default/Sprite.png");
				if (!string.IsNullOrEmpty(editorResourceFullPath))
				{
					pixbuf = PixbufHelper.Load(editorResourceFullPath);
				}
				this.broken = true;
			}
			else
			{
				this.broken = false;
			}
			return pixbuf;
		}

		public void ReloadFile()
		{
			Pixbuf pixbuf = this.LoadImageFile(this.File);
			this.ScalePixbuf(pixbuf);
			this.Trim(pixbuf);
			pixbuf.Dispose();
		}

		private unsafe void Trim(Pixbuf originPixbuf)
		{
			this.SourceWidth = (int)((double)originPixbuf.Width * this.Scale);
			this.SourceHeight = (int)((double)originPixbuf.Height * this.Scale);
			if (originPixbuf.HasAlpha)
			{
				int num = originPixbuf.Width - 1;
				int num2 = originPixbuf.Height - 1;
				int num3 = 0;
				int num4 = 0;
				int nchannels = originPixbuf.NChannels;
				int rowstride = originPixbuf.Rowstride;
				byte* ptr = (byte*)((void*)originPixbuf.Pixels);
				for (int i = 0; i < originPixbuf.Height; i++)
				{
					for (int j = 0; j < originPixbuf.Width; j++)
					{
						int num5 = i * rowstride + j * nchannels;
						byte* ptr2 = ptr + num5;
						byte b = *ptr2;
						byte b2 = ptr2[1];
						byte b3 = ptr2[2];
						int num6 = (int)ptr2[3];
						if (num6 != 0)
						{
							num = Math.Min(num, j);
							num3 = Math.Max(num3, j);
							num2 = Math.Min(num2, i);
							num4 = Math.Max(num4, i);
						}
					}
				}
				num3 = Math.Max(num3, num);
				num4 = Math.Max(num4, num2);
				this.Location = new Gdk.Point(num, num2);
				this.Width = num3 - num + 1;
				this.Height = num4 - num2 + 1;
				int x = this.Location.X + this.Width / 2 - this.SourceWidth / 2;
				int y = this.SourceHeight / 2 - (this.Location.Y + this.Height / 2);
				this.Offset = new Gdk.Point(x, y);
			}
			else
			{
				this.Width = originPixbuf.Width;
				this.Height = originPixbuf.Height;
				this.Location = new Gdk.Point(0, 0);
			}
			this.Width = (int)((double)this.Width * this.Scale);
			this.Height = (int)((double)this.Height * this.Scale);
		}

		private void ScalePixbuf(Pixbuf originPixbuf)
		{
			this.SourceWidth = (int)((double)originPixbuf.Width * this.Scale);
			this.SourceHeight = (int)((double)originPixbuf.Height * this.Scale);
			int num = (int)(this.renderScale * (double)this.SourceWidth);
			int num2 = (int)(this.renderScale * (double)this.SourceHeight);
			if (this.pixbuf == null || this.pixbuf.Width != num || this.pixbuf.Height != num2)
			{
				if (this.pixbuf != null)
				{
					this.pixbuf.Dispose();
					this.pixbuf = null;
				}
				this.pixbuf = originPixbuf.ScaleSimple(num, num2, InterpType.Bilinear);
				if (this.rotate)
				{
					this.pixbuf = this.pixbuf.RotateSimple(PixbufRotation.Clockwise);
				}
			}
		}

		private void RotatePixbuf()
		{
			Pixbuf pixbuf = this.pixbuf;
			if (this.rotate)
			{
				this.pixbuf = pixbuf.RotateSimple(PixbufRotation.Clockwise);
			}
			else
			{
				this.pixbuf = pixbuf.RotateSimple(PixbufRotation.Counterclockwise);
			}
			pixbuf.Dispose();
		}

		public ImageInfo GetImageInfo()
		{
			System.Drawing.Point sourceLocation = default(System.Drawing.Point);
			int width;
			int height;
			if (this.Model.AllowTrim)
			{
				width = this.Width;
				height = this.Height;
				sourceLocation.X = this.Location.X;
				sourceLocation.Y = this.Location.Y;
			}
			else
			{
				width = this.SourceWidth;
				height = this.SourceHeight;
				sourceLocation.X = 0;
				sourceLocation.Y = 0;
			}
			System.Drawing.Rectangle bounding = new System.Drawing.Rectangle(new System.Drawing.Point(this.SourceLocation.X, this.SourceLocation.Y), new System.Drawing.Size(width, height));
			ImageInfo result = new ImageInfo(this.RelativeFile, bounding, new System.Drawing.Size(this.SourceWidth, this.SourceHeight), sourceLocation, this.Rotate);
			return result;
		}

		public void Draw(PlistInfoItemRender pictureRender)
		{
			if (this.pixbuf == null)
			{
				this.ReloadFile();
			}
			if (this.pixbuf == null)
			{
				return;
			}
			int num = (int)((double)this.SourceLocation.X * this.renderScale) + pictureRender.Position.X;
			int num2 = (int)((double)this.SourceLocation.Y * this.renderScale) + pictureRender.Position.Y;
			int src_x = 0;
			int src_y = 0;
			if (this.Model.AllowTrim)
			{
				if (this.rotate)
				{
					src_x = this.SourceHeight - (int)((double)this.Location.Y * this.renderScale) - this.Height;
					src_y = (int)((double)this.Location.X * this.renderScale);
				}
				else
				{
					src_x = (int)((double)this.Location.X * this.renderScale);
					src_y = (int)((double)this.Location.Y * this.renderScale);
				}
			}
			Gdk.GC gc = pictureRender.Style.BackgroundGC(StateType.Normal);
			pictureRender.GdkWindow.DrawPixbuf(gc, this.pixbuf, src_x, src_y, num, num2, this.RenderWidth, this.RenderHeight, RgbDither.Normal, 0, 0);
			if (this.Select)
			{
				using (Context context = CairoHelper.Create(pictureRender.GdkWindow))
				{
					context.LineWidth = 0.5;
					context.SetSourceRGB(1.0, 1.0, 1.0);
					context.Rectangle((double)num, (double)num2, (double)this.RenderWidth, (double)this.RenderHeight);
					context.Stroke();
				}
			}
		}

		public bool DrawToImage(Pixbuf gImg, bool isDrawToJpeg = false)
		{
			if (!System.IO.File.Exists(this.File))
			{
				throw new FileNotFoundException("File not found. File is " + this.File, this.File);
			}
			if (this.pixbuf == null)
			{
				this.ReloadFile();
			}
			if (this.pixbuf == null || this.broken)
			{
				throw new FileNotFoundException("File is broken. File is " + this.File, this.File);
			}
			Pixbuf pixbuf = this.pixbuf;
			Pixbuf pixbuf2;
			if (isDrawToJpeg && System.IO.Path.GetExtension(this.File).ToLower().CompareTo(".png") == 0)
			{
				if (pixbuf.HasAlpha)
				{
					string message = string.Format("合图使用的素材文件 {0} 含有透明通道，输出到jpg可能会导致输出图形错误", System.IO.Path.GetFileName(this.File));
					LogConfig.Output.Error(message);
				}
				byte[] buffer = pixbuf.SaveToBuffer("jpeg");
				pixbuf2 = new Pixbuf(buffer);
			}
			else
			{
				pixbuf2 = pixbuf;
			}
			int src_x = 0;
			int src_y = 0;
			if (this.Model.AllowTrim)
			{
				if (this.rotate)
				{
					src_x = this.SourceHeight - (int)((double)this.Location.Y * this.renderScale) - this.Height;
					src_y = (int)((double)this.Location.X * this.renderScale);
				}
				else
				{
					src_x = (int)((double)this.Location.X * this.renderScale);
					src_y = (int)((double)this.Location.Y * this.renderScale);
				}
			}
			pixbuf2.CopyArea(src_x, src_y, this.RenderWidth, this.RenderHeight, gImg, this.SourceLocation.X, this.SourceLocation.Y);
			if (pixbuf2 != this.pixbuf)
			{
				pixbuf2.Dispose();
			}
			return true;
		}

		public void ReleaseRef()
		{
			if (this.pixbuf != null)
			{
				this.pixbuf.Dispose();
				this.pixbuf = null;
			}
			this.ResourceItem = null;
			this.Model = null;
		}

		private bool broken;

		private bool rotate;

		private Pixbuf pixbuf;

		private double scale = 1.0;

		private double renderScale = 1.0;
	}
}
