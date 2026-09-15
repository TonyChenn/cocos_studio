using System;
using System.Drawing;
using System.IO;
using CocoStudio.Basic;
using Gdk;
using Gtk;

namespace Modules.Communal.Packer
{
	internal class PackerSprite
	{
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

		public void SetPosition(System.Drawing.Point targetPos)
		{
			this.x = (float)targetPos.X;
			this.y = (float)targetPos.Y;
		}

		public void DrawToImage(Graphics gDraw, string resourcePath)
		{
			this.GetImage(resourcePath);
			System.Drawing.Rectangle destRect = new System.Drawing.Rectangle((int)this.x, (int)this.y, (int)this.width, (int)this.height);
			System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle((int)this.originX, (int)this.originY, (int)(this.width / this.fScale), (int)(this.height / this.fScale));
			gDraw.DrawImage(this.image, destRect, srcRect, GraphicsUnit.Pixel);
			this.image.Dispose();
			this.image = null;
		}

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

		private Bitmap image;

		public string name;

		private string pathName;

		private float originX;

		private float originY;

		public float x = 0f;

		public float y = 0f;

		public float width = 0f;

		public float height = 0f;

		public float offsetX = 0f;

		public float offsetY = 0f;

		public float originalWidth = 0f;

		public float originalHeight = 0f;

		private float fScale = 1f;

		public bool dataOnly;
	}
}
