using System;
using Gdk;

namespace Modules.Communal.Packer.Model
{
	public class CustomRectangle
	{
		public CustomRectangle(object userData = null)
		{
			this.UserData = userData;
			this.inner = default(Rectangle);
		}

		public CustomRectangle(Point loc, Size sz, object userData = null)
		{
			this.UserData = userData;
			this.inner = new Rectangle(loc, sz);
		}

		public CustomRectangle(int x, int y, int width, int height, object userData = null)
		{
			this.UserData = userData;
			this.inner = new Rectangle(x, y, width, height);
		}

		public CustomRectangle(int x, int y, int width, int height, bool rotated, object userData = null)
		{
			this.Rotated = rotated;
			this.UserData = userData;
			this.inner = new Rectangle(x, y, width, height);
		}

		public object UserData { get; set; }

		public bool Rotated { get; set; }

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

		public int Right
		{
			get
			{
				return this.inner.Right;
			}
		}

		public int Top
		{
			get
			{
				return this.inner.Top;
			}
		}

		public int Bottom
		{
			get
			{
				return this.inner.Bottom;
			}
		}

		public int Left
		{
			get
			{
				return this.inner.Left;
			}
		}

		public bool Contains(CustomRectangle other)
		{
			return this.inner.Contains(other.inner);
		}

		public CustomRectangle Copy()
		{
			return new CustomRectangle(this.X, this.Y, this.Width, this.Height, this.Rotated, this.UserData);
		}

		private Rectangle inner;
	}
}
