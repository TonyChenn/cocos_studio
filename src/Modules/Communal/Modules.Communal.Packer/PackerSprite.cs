using System;
using System.Drawing;
using System.IO;
using CocoStudio.Basic;
using Gdk;
using Gtk;

namespace Modules.Communal.Packer
{
	// Token: 0x0200000F RID: 15
	internal class PackerSprite
	{
		// Token: 0x0600005E RID: 94 RVA: 0x0000430C File Offset: 0x0000250C
		public PackerSprite(Bitmap _map, string _name, bool bClip, bool bDataOnly, float scale)
		{
			this.image = _map;
			this.pathName = _name.Replace('\\', '/');
			if (Path.IsPathRooted(this.pathName))
			{
				this.name = this.pathName.Replace(Option.EditorDefaultResourcePath, "").Trim(new char[]
				{
					'/'
				});
			}
			else
			{
				this.name = this.pathName.Trim(new char[]
				{
					'/'
				});
			}
			this.dataOnly = bDataOnly;
			this.fScale = scale;
			this.CutEmpty(this.image, bClip);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004420 File Offset: 0x00002620
		private void CutEmpty(Bitmap _map, bool bclip)
		{
			int num = _map.Width - 1;
			int num2 = _map.Height - 1;
			int num3 = 0;
			int num4 = 0;
			if (bclip)
			{
				for (int i = 0; i < _map.Height; i++)
				{
					for (int j = 0; j < _map.Width; j++)
					{
						if (_map.GetPixel(j, i).A != 0)
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
				this.originX = (float)num;
				this.originY = (float)num2;
				this.width = (float)(num3 - num + 1);
				this.height = (float)(num4 - num2 + 1);
			}
			else
			{
				this.originX = 0f;
				this.originY = 0f;
				this.width = (float)_map.Width;
				this.height = (float)_map.Height;
			}
			this.width *= this.fScale;
			this.height *= this.fScale;
			this.originalWidth = (float)_map.Width * this.fScale;
			this.originalHeight = (float)_map.Height * this.fScale;
			this.offsetX = this.originX + this.width / 2f - this.originalWidth / 2f;
			this.offsetY = this.originalHeight / 2f - (this.originY + this.height / 2f);
			if (this.dataOnly)
			{
				this.image.Dispose();
				this.image = null;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000045F9 File Offset: 0x000027F9
		public void SetPosition(System.Drawing.Point targetPos)
		{
			this.x = (float)targetPos.X;
			this.y = (float)targetPos.Y;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004618 File Offset: 0x00002818
		public void DrawToImage(Graphics gDraw, string resourcePath)
		{
			this.GetImage(resourcePath);
			System.Drawing.Rectangle destRect = new System.Drawing.Rectangle((int)this.x, (int)this.y, (int)this.width, (int)this.height);
			System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle((int)this.originX, (int)this.originY, (int)(this.width / this.fScale), (int)(this.height / this.fScale));
			gDraw.DrawImage(this.image, destRect, srcRect, GraphicsUnit.Pixel);
			this.image.Dispose();
			this.image = null;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000046A8 File Offset: 0x000028A8
		private Bitmap GetImage(string resourcePath)
		{
			if (this.image == null)
			{
				string text = this.pathName;
				if (!Path.IsPathRooted(this.pathName))
				{
					text = Path.Combine(resourcePath, this.pathName);
				}
				try
				{
					if (File.Exists(text))
					{
						string text2 = Path.GetExtension(text).ToLower();
						Pixbuf pixbuf = PixbufHelper.Load(text);
						this.image = new Bitmap(new MemoryStream(pixbuf.SaveToBuffer(text2.Trim(new char[]
						{
							'.'
						}))));
						pixbuf.Dispose();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Ex : " + ex.Message, MessageBoxImage.Other, null, null);
					return null;
				}
			}
			return this.image;
		}

		// Token: 0x04000026 RID: 38
		private Bitmap image;

		// Token: 0x04000027 RID: 39
		public string name;

		// Token: 0x04000028 RID: 40
		private string pathName;

		// Token: 0x04000029 RID: 41
		private float originX;

		// Token: 0x0400002A RID: 42
		private float originY;

		// Token: 0x0400002B RID: 43
		public float x = 0f;

		// Token: 0x0400002C RID: 44
		public float y = 0f;

		// Token: 0x0400002D RID: 45
		public float width = 0f;

		// Token: 0x0400002E RID: 46
		public float height = 0f;

		// Token: 0x0400002F RID: 47
		public float offsetX = 0f;

		// Token: 0x04000030 RID: 48
		public float offsetY = 0f;

		// Token: 0x04000031 RID: 49
		public float originalWidth = 0f;

		// Token: 0x04000032 RID: 50
		public float originalHeight = 0f;

		// Token: 0x04000033 RID: 51
		private float fScale = 1f;

		// Token: 0x04000034 RID: 52
		public bool dataOnly;
	}
}
