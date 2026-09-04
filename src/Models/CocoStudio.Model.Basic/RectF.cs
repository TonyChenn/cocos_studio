using System;
using System.ComponentModel;
using System.Runtime;
using Gdk;

namespace CocoStudio.Model
{
	// Token: 0x02000008 RID: 8
	public sealed class RectF
	{
		// Token: 0x06000022 RID: 34 RVA: 0x0000242A File Offset: 0x0000062A
		public RectF()
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002435 File Offset: 0x00000635
		public RectF(float x, float y, float width, float height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000245D File Offset: 0x0000065D
		public RectF(PointF location, SizeF size)
		{
			this.x = location.X;
			this.y = location.Y;
			this.width = size.Width;
			this.height = size.Height;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002498 File Offset: 0x00000698
		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static RectF FromLTRB(float left, float top, float right, float bottom)
		{
			return new RectF(left, bottom, right - left, top - bottom);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000024B8 File Offset: 0x000006B8
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000024DB File Offset: 0x000006DB
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000024F8 File Offset: 0x000006F8
		// (set) Token: 0x06000029 RID: 41 RVA: 0x0000251B File Offset: 0x0000071B
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

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002538 File Offset: 0x00000738
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002550 File Offset: 0x00000750
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

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000255C File Offset: 0x0000075C
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002574 File Offset: 0x00000774
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002580 File Offset: 0x00000780
		// (set) Token: 0x0600002F RID: 47 RVA: 0x00002598 File Offset: 0x00000798
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

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000025A4 File Offset: 0x000007A4
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000025BC File Offset: 0x000007BC
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

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000025C8 File Offset: 0x000007C8
		[Browsable(false)]
		public float Left
		{
			get
			{
				return this.X;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000025E0 File Offset: 0x000007E0
		[Browsable(false)]
		public float Top
		{
			get
			{
				return this.Y + this.Height;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002600 File Offset: 0x00000800
		[Browsable(false)]
		public float Right
		{
			[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
			get
			{
				return this.X + this.Width;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002620 File Offset: 0x00000820
		[Browsable(false)]
		public float Bottom
		{
			[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
			get
			{
				return this.Y;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002638 File Offset: 0x00000838
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.Width <= 0f || this.Height <= 0f;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000266C File Offset: 0x0000086C
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

		// Token: 0x06000038 RID: 56 RVA: 0x000026D4 File Offset: 0x000008D4
		public bool Contains(float x, float y)
		{
			return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000271C File Offset: 0x0000091C
		public bool Contains(PointF pt)
		{
			return this.Contains(pt.X, pt.Y);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002740 File Offset: 0x00000940
		public bool Contains(RectF rect)
		{
			return this.X <= rect.X && rect.X + rect.Width <= this.X + this.Width && this.Y <= rect.Y && rect.Y + rect.Height <= this.Y + this.Height;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000027B0 File Offset: 0x000009B0
		public override int GetHashCode()
		{
			return (int)((uint)this.X ^ ((uint)this.Y << 13 | (uint)this.Y >> 19) ^ ((uint)this.Width << 26 | (uint)this.Width >> 6) ^ ((uint)this.Height << 7 | (uint)this.Height >> 25));
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000280C File Offset: 0x00000A0C
		public void Inflate(float x, float y)
		{
			this.X -= x;
			this.Y -= y;
			this.Width += 2f * x;
			this.Height += 2f * y;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002862 File Offset: 0x00000A62
		public void Inflate(PointF size)
		{
			this.Inflate(size.X, size.Y);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002878 File Offset: 0x00000A78
		public static RectF Inflate(RectF rect, float x, float y)
		{
			rect.Inflate(x, y);
			return rect;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002898 File Offset: 0x00000A98
		public void Intersect(RectF rect)
		{
			RectF rectF = RectF.Intersect(rect, this);
			this.X = rectF.X;
			this.Y = rectF.Y;
			this.Width = rectF.Width;
			this.Height = rectF.Height;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000028E4 File Offset: 0x00000AE4
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

		// Token: 0x06000041 RID: 65 RVA: 0x00002988 File Offset: 0x00000B88
		public bool IntersectsWith(RectF rect)
		{
			return rect.X < this.X + this.Width && this.X < rect.X + rect.Width && rect.Y < this.Y + this.Height && this.Y < rect.Y + rect.Height;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000029F4 File Offset: 0x00000BF4
		public static RectF Union(RectF a, RectF b)
		{
			float num = Math.Min(a.X, b.X);
			float num2 = Math.Max(a.X + a.Width, b.X + b.Width);
			float num3 = Math.Min(a.Y, b.Y);
			float num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
			return new RectF(num, num3, num2 - num, num4 - num3);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002A79 File Offset: 0x00000C79
		public void Offset(PointF pos)
		{
			this.Offset(pos.X, pos.Y);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002A8F File Offset: 0x00000C8F
		public void Offset(float x, float y)
		{
			this.X += x;
			this.Y += y;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002AB0 File Offset: 0x00000CB0
		public static implicit operator RectF(Rectangle r)
		{
			return new RectF((float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002AE8 File Offset: 0x00000CE8
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

		// Token: 0x0400002A RID: 42
		public static readonly RectF Empty = new RectF();

		// Token: 0x0400002B RID: 43
		private float x;

		// Token: 0x0400002C RID: 44
		private float y;

		// Token: 0x0400002D RID: 45
		private float width;

		// Token: 0x0400002E RID: 46
		private float height;
	}
}
