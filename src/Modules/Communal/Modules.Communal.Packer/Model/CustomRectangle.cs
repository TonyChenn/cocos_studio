using System;
using Gdk;

namespace Modules.Communal.Packer.Model
{
	// Token: 0x02000004 RID: 4
	public class CustomRectangle
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020B3 File Offset: 0x000002B3
		public CustomRectangle(object userData = null)
		{
			this.UserData = userData;
			this.inner = default(Rectangle);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020D2 File Offset: 0x000002D2
		public CustomRectangle(Point loc, Size sz, object userData = null)
		{
			this.UserData = userData;
			this.inner = new Rectangle(loc, sz);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020F2 File Offset: 0x000002F2
		public CustomRectangle(int x, int y, int width, int height, object userData = null)
		{
			this.UserData = userData;
			this.inner = new Rectangle(x, y, width, height);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002116 File Offset: 0x00000316
		public CustomRectangle(int x, int y, int width, int height, bool rotated, object userData = null)
		{
			this.Rotated = rotated;
			this.UserData = userData;
			this.inner = new Rectangle(x, y, width, height);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002144 File Offset: 0x00000344
		// (set) Token: 0x0600000B RID: 11 RVA: 0x0000215B File Offset: 0x0000035B
		public object UserData { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002164 File Offset: 0x00000364
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000217B File Offset: 0x0000037B
		public bool Rotated { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002184 File Offset: 0x00000384
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000021A1 File Offset: 0x000003A1
		public int X
		{
			get
			{
				return this.inner.X;
			}
			set
			{
				this.inner.X = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000021B0 File Offset: 0x000003B0
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000021CD File Offset: 0x000003CD
		public int Y
		{
			get
			{
				return this.inner.Y;
			}
			set
			{
				this.inner.Y = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000021DC File Offset: 0x000003DC
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000021F9 File Offset: 0x000003F9
		public int Width
		{
			get
			{
				return this.inner.Width;
			}
			set
			{
				this.inner.Width = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002208 File Offset: 0x00000408
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002225 File Offset: 0x00000425
		public int Height
		{
			get
			{
				return this.inner.Height;
			}
			set
			{
				this.inner.Height = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002234 File Offset: 0x00000434
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002251 File Offset: 0x00000451
		public Size Size
		{
			get
			{
				return this.inner.Size;
			}
			set
			{
				this.inner.Size = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002264 File Offset: 0x00000464
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002281 File Offset: 0x00000481
		public Point Location
		{
			get
			{
				return this.inner.Location;
			}
			set
			{
				this.inner.Location = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002294 File Offset: 0x00000494
		public int Right
		{
			get
			{
				return this.inner.Right;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000022B4 File Offset: 0x000004B4
		public int Top
		{
			get
			{
				return this.inner.Top;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000022D4 File Offset: 0x000004D4
		public int Bottom
		{
			get
			{
				return this.inner.Bottom;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000022F4 File Offset: 0x000004F4
		public int Left
		{
			get
			{
				return this.inner.Left;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002314 File Offset: 0x00000514
		public bool Contains(CustomRectangle other)
		{
			return this.inner.Contains(other.inner);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002338 File Offset: 0x00000538
		public CustomRectangle Copy()
		{
			return new CustomRectangle(this.X, this.Y, this.Width, this.Height, this.Rotated, this.UserData);
		}

		// Token: 0x04000002 RID: 2
		private Rectangle inner;
	}
}
