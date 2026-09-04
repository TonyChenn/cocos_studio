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
	// Token: 0x02000009 RID: 9
	public class PlistInfoItem
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002A6B File Offset: 0x00000C6B
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002A73 File Offset: 0x00000C73
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

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002A93 File Offset: 0x00000C93
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002A9B File Offset: 0x00000C9B
		private Gdk.Point Location { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002AA4 File Offset: 0x00000CA4
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002AAC File Offset: 0x00000CAC
		public Gdk.Point SourceLocation { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002AB5 File Offset: 0x00000CB5
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002ABD File Offset: 0x00000CBD
		public Gdk.Point Offset { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002AC6 File Offset: 0x00000CC6
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002ACE File Offset: 0x00000CCE
		public int SourceWidth { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002AD7 File Offset: 0x00000CD7
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00002ADF File Offset: 0x00000CDF
		public int SourceHeight { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002AE8 File Offset: 0x00000CE8
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00002AF0 File Offset: 0x00000CF0
		public int Width { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002AF9 File Offset: 0x00000CF9
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00002B01 File Offset: 0x00000D01
		public int Height { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002B0C File Offset: 0x00000D0C
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

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002B68 File Offset: 0x00000D68
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

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002BC4 File Offset: 0x00000DC4
		public Gdk.Rectangle RenderRect
		{
			get
			{
				return new Gdk.Rectangle(new Gdk.Point((int)((double)this.SourceLocation.X * this.renderScale), (int)((double)this.SourceLocation.Y * this.renderScale)), new Gdk.Size(this.RenderWidth, this.RenderHeight));
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002C14 File Offset: 0x00000E14
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002C1C File Offset: 0x00000E1C
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

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002C6C File Offset: 0x00000E6C
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002C74 File Offset: 0x00000E74
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

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002CC4 File Offset: 0x00000EC4
		public string File
		{
			get
			{
				return this.ResourceItem.FullPath;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002CD1 File Offset: 0x00000ED1
		public string RelativeFile
		{
			get
			{
				return this.ResourceItem.RelativePath;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002CDE File Offset: 0x00000EDE
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002CE6 File Offset: 0x00000EE6
		public ResourceItem ResourceItem { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002CEF File Offset: 0x00000EEF
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002CF7 File Offset: 0x00000EF7
		public bool Select { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002D00 File Offset: 0x00000F00
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002D08 File Offset: 0x00000F08
		public PlistInfoModel Model { get; private set; }

		// Token: 0x06000061 RID: 97 RVA: 0x00002D14 File Offset: 0x00000F14
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

		// Token: 0x06000062 RID: 98 RVA: 0x00002DB0 File Offset: 0x00000FB0
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

		// Token: 0x06000063 RID: 99 RVA: 0x00002E04 File Offset: 0x00001004
		public void ReloadFile()
		{
			Pixbuf pixbuf = this.LoadImageFile(this.File);
			this.ScalePixbuf(pixbuf);
			this.Trim(pixbuf);
			pixbuf.Dispose();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002E34 File Offset: 0x00001034
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

		// Token: 0x06000065 RID: 101 RVA: 0x00002FFC File Offset: 0x000011FC
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

		// Token: 0x06000066 RID: 102 RVA: 0x000030C0 File Offset: 0x000012C0
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

		// Token: 0x06000067 RID: 103 RVA: 0x00003104 File Offset: 0x00001304
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

		// Token: 0x06000068 RID: 104 RVA: 0x000031CC File Offset: 0x000013CC
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

		// Token: 0x06000069 RID: 105 RVA: 0x00003368 File Offset: 0x00001568
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

		// Token: 0x0600006A RID: 106 RVA: 0x00003506 File Offset: 0x00001706
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

		// Token: 0x04000016 RID: 22
		private bool broken;

		// Token: 0x04000017 RID: 23
		private bool rotate;

		// Token: 0x04000018 RID: 24
		private Pixbuf pixbuf;

		// Token: 0x04000019 RID: 25
		private double scale = 1.0;

		// Token: 0x0400001A RID: 26
		private double renderScale = 1.0;
	}
}
