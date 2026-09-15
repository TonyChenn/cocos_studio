using System;
using System.ComponentModel;
using System.Runtime;
using Gdk;

namespace CocoStudio.Model
{
	public sealed class RectF
	{
		public RectF()
		{
		}

		public RectF(float x, float y, float width, float height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public RectF(PointF location, SizeF size)
		{
			this.x = location.X;
			this.y = location.Y;
			this.width = size.Width;
			this.height = size.Height;
		}

		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static RectF FromLTRB(float left, float top, float right, float bottom)
		{
			return new RectF(left, bottom, right - left, top - bottom);
		}

		[Browsable(false)]
		public PointF Location
		{
			[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
			get
			{
				return new PointF(this.X, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		[Browsable(false)]
		public PointF Size
		{
			[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
			get
			{
				return new PointF(this.Width, this.Height);
			}
			set
			{
				this.Width = value.X;
				this.Height = value.Y;
			}
		}

		public float X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		public float Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		[Browsable(false)]
		public float Left
		{
			get
			{
				return this.X;
			}
		}

		[Browsable(false)]
		public float Top
		{
			get
			{
				return this.Y + this.Height;
			}
		}

		[Browsable(false)]
		public float Right
		{
			[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
			get
			{
				return this.X + this.Width;
			}
		}

		[Browsable(false)]
		public float Bottom
		{
			[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
			get
			{
				return this.Y;
			}
		}

		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.Width <= 0f || this.Height <= 0f;
			}
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (!(obj is RectF))
			{
				result = false;
			}
			else
			{
				RectF rectF = (RectF)obj;
				result = (rectF.X == this.X && rectF.Y == this.Y && rectF.Width == this.Width && rectF.Height == this.Height);
			}
			return result;
		}

		public bool Contains(float x, float y)
		{
			return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
		}

		public bool Contains(PointF pt)
		{
			return this.Contains(pt.X, pt.Y);
		}

		public bool Contains(RectF rect)
		{
			return this.X <= rect.X && rect.X + rect.Width <= this.X + this.Width && this.Y <= rect.Y && rect.Y + rect.Height <= this.Y + this.Height;
		}

		public override int GetHashCode()
		{
			return (int)((uint)this.X ^ ((uint)this.Y << 13 | (uint)this.Y >> 19) ^ ((uint)this.Width << 26 | (uint)this.Width >> 6) ^ ((uint)this.Height << 7 | (uint)this.Height >> 25));
		}

		public void Inflate(float x, float y)
		{
			this.X -= x;
			this.Y -= y;
			this.Width += 2f * x;
			this.Height += 2f * y;
		}

		public void Inflate(PointF size)
		{
			this.Inflate(size.X, size.Y);
		}

		public static RectF Inflate(RectF rect, float x, float y)
		{
			rect.Inflate(x, y);
			return rect;
		}

		public void Intersect(RectF rect)
		{
			RectF rectF = RectF.Intersect(rect, this);
			this.X = rectF.X;
			this.Y = rectF.Y;
			this.Width = rectF.Width;
			this.Height = rectF.Height;
		}

		public static RectF Intersect(RectF a, RectF b)
		{
			float num = Math.Max(a.X, b.X);
			float num2 = Math.Min(a.X + a.Width, b.X + b.Width);
			float num3 = Math.Max(a.Y, b.Y);
			float num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
			RectF result;
			if (num2 >= num && num4 >= num3)
			{
				result = new RectF(num, num3, num2 - num, num4 - num3);
			}
			else
			{
				result = RectF.Empty;
			}
			return result;
		}

		public bool IntersectsWith(RectF rect)
		{
			return rect.X < this.X + this.Width && this.X < rect.X + rect.Width && rect.Y < this.Y + this.Height && this.Y < rect.Y + rect.Height;
		}

		public static RectF Union(RectF a, RectF b)
		{
			float num = Math.Min(a.X, b.X);
			float num2 = Math.Max(a.X + a.Width, b.X + b.Width);
			float num3 = Math.Min(a.Y, b.Y);
			float num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
			return new RectF(num, num3, num2 - num, num4 - num3);
		}

		public void Offset(PointF pos)
		{
			this.Offset(pos.X, pos.Y);
		}

		public void Offset(float x, float y)
		{
			this.X += x;
			this.Y += y;
		}

		public static implicit operator RectF(Rectangle r)
		{
			return new RectF((float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"{X=",
				this.X,
				",Y=",
				this.Y,
				",Width=",
				this.Width,
				",Height=",
				this.Height,
				"}"
			});
		}

		public static readonly RectF Empty = new RectF();

		private float x;

		private float y;

		private float width;

		private float height;
	}
}
